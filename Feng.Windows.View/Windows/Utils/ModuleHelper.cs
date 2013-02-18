using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Feng.Data;
using Feng.Utils;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// ModuleHelper
    /// </summary>
    public static class ModuleHelper
    {
        private static void CreateNecessaryDirectories(string moduleName)
        {
            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;
            IOHelper.TryCreateDirectory(dir + "\\");
            IOHelper.TryCreateDirectory(dir + "\\referencedata\\");
            IOHelper.TryCreateDirectory(dir + "\\referencedata\\standard\\");

            IOHelper.TryCreateDirectory(dir + "\\src\\");

            IOHelper.TryCreateDirectory(dir + "\\src-db\\");
            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\");
            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\model\\");

            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\model\\functions\\");
            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\model\\triggers\\");
            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\model\\views\\");
            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\model\\procedures\\");

            IOHelper.TryCreateDirectory(dir + "\\src-db\\database\\sourcedata\\");
        }

        /// <summary>
        /// 把当前数据库的所有内容作为一个模块Export
        /// </summary>
        /// <param name="moduleName"></param>
        public static void PackageCurrentDatabase(string moduleName)
        {
            CreateNecessaryDirectories(moduleName);

            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;
            
            string[] userDataTables = DbHelper.GetUserDataTables();
            foreach (string s in userDataTables)
            {
                System.Data.DataTable dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + s);
                if (dt.Rows.Count > 0)
                {
                    using (ExcelXmlWriter ew = new ExcelXmlWriter(moduleName + "\\referencedata\\standard\\" + s + ".xml"))
                    {
                        ew.WriteExcelXml(dt, s);
                    }
                }
            }

            System.Data.DataTable views = DbHelper.GetViewFuncProcTrigs();
            foreach (System.Data.DataRow row in views.Rows)
            {
                if (row["type"].ToString() == "Function")
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(moduleName + "\\src-db\\database\\model\\functions\\" + row["name"].ToString(), false, Encoding.UTF8))
                    {
                        sw.Write(row["definition"].ToString());
                    }
                }
            }
            
            foreach (System.Data.DataRow row in views.Rows)
            {
                if (row["type"].ToString() == "Trigger")
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(moduleName + "\\src-db\\database\\model\\triggers\\" + row["name"].ToString(), false, Encoding.UTF8))
                    {
                        sw.Write(row["definition"].ToString());
                    }
                }
            }
            
            foreach (System.Data.DataRow row in views.Rows)
            {
                if (row["type"].ToString() == "View")
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(moduleName + "\\src-db\\database\\model\\views\\" + row["name"].ToString(), false, Encoding.UTF8))
                    {
                        sw.Write(row["definition"].ToString());
                    }
                }
            }
            
            foreach (System.Data.DataRow row in views.Rows)
            {
                if (row["type"].ToString() == "Procedure")
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(moduleName + "\\src-db\\database\\model\\procedures\\" + row["name"].ToString(), false, Encoding.UTF8))
                    {
                        sw.Write(row["definition"].ToString());
                    }
                }
            }

            string[] adDataTables = DbHelper.GetAdDataTables();
            foreach (string s in adDataTables)
            {
                System.Data.DataTable dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + s);
                if (dt.Rows.Count > 0)
                {
                    using (ExcelXmlWriter ew = new ExcelXmlWriter(moduleName + "\\src-db\\database\\sourcedata\\" + s + ".xml"))
                    {
                        ew.WriteExcelXml(dt, s);
                    }
                }
            }
        }

        private static string ReadFile(string fileName)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName))
            {
                return sr.ReadToEnd();
            }
        }
        /// <summary>
        /// 按照模块文件信息，生成响应ModuleInfo。（粗略，可修改）
        /// </summary>
        /// <param name="moduleName"></param>
        public static void GenerateModuleInfos(string moduleName)
        {
            MultiOrgEntityDao<ModuleInfo> moduleDao = new MultiOrgEntityDao<ModuleInfo>();
            MultiOrgEntityDao<ModuleIncludeInfo> moduleIncludeDao = new MultiOrgEntityDao<ModuleIncludeInfo>();

            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;

            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<ModuleInfo>())
            {
                try
                {
                    rep.BeginTransaction();

                    ModuleInfo moduleInfo = new ModuleInfo();
                    moduleInfo.Author = "system";
                    moduleInfo.ID = System.IO.Path.GetFileNameWithoutExtension(moduleName);
                    moduleInfo.ModuleData = CompressionHelper.CompressFromFolder(dir);
                    moduleInfo.IsActive = true;
                    moduleInfo.ModuleVersion = "1.0.0.0";
                    moduleDao.Save(rep, moduleInfo);

                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\referencedata\\standard\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.ReferenceData;
                        includeInfo.ID = moduleInfo.ID + "_RD_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.ID.Substring(0, Math.Min(50, includeInfo.Name.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }

                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\functions\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.DbFunction;
                        includeInfo.ID = moduleInfo.Name + "_DF_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.Name.Substring(0, Math.Min(50, includeInfo.ID.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }
                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\triggers\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.DbTrigger;
                        includeInfo.ID = moduleInfo.ID + "_DT_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.ID.Substring(0, Math.Min(50, includeInfo.ID.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }
                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\views\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.DbView;
                        includeInfo.ID = moduleInfo.ID + "_DV_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.ID.Substring(0, Math.Min(50, includeInfo.ID.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }
                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\procedures\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.DbProcedure;
                        includeInfo.ID = moduleInfo.ID + "_DP_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.ID.Substring(0, Math.Min(50, includeInfo.ID.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }

                    foreach (string s in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\sourcedata\\"))
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(s);

                        ModuleIncludeInfo includeInfo = new ModuleIncludeInfo();
                        includeInfo.Module = moduleInfo;
                        includeInfo.ModuleIncludeType = ModuleIncludeType.ApplicationDictionaryData;
                        includeInfo.ID = moduleInfo.ID + "_AD_" + name;
                        includeInfo.Params = name;
                        includeInfo.IsActive = true;
                        includeInfo.ID = includeInfo.ID.Substring(0, Math.Min(50, includeInfo.ID.Length));
                        moduleIncludeDao.Save(rep, includeInfo);
                    }

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        /// <summary>
        /// 按照Module定义信息打包Module
        /// </summary>
        /// <param name="moduleName"></param>
        public static void PackageModule(string moduleName)
        {
            CreateNecessaryDirectories(moduleName);

            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;

            // Write ModuleInfo
            DataTable dt;
            dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Module WHERE NAME = '" + moduleName + "'");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                row["ModuleData"] = System.DBNull.Value;
            }
            using (ExcelXmlWriter ew = new ExcelXmlWriter(dir + "\\src-db\\database\\sourcedata\\AD_Module.xml"))
            {
                ew.WriteExcelXml(dt, "AD_Module");
            }
            dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Module_Dependency WHERE Module = '" + moduleName + "'");
            using (ExcelXmlWriter ew = new ExcelXmlWriter(dir + "\\src-db\\database\\sourcedata\\AD_Module_Dependency.xml"))
            {
                ew.WriteExcelXml(dt, "AD_Module_Dependency");
            }
            dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Module_Include WHERE Module = '" + moduleName + "'");
            using (ExcelXmlWriter ew = new ExcelXmlWriter(dir + "\\src-db\\database\\sourcedata\\AD_Module_Include.xml"))
            {
                ew.WriteExcelXml(dt, "AD_Module_Include");
            }

            // Write Module Include Info
            foreach (System.Data.DataRow includeInfo in dt.Rows)
            {
                switch ((ModuleIncludeType)includeInfo["ModuleIncludeType"])
                {
                    case ModuleIncludeType.ReferenceData:
                        {
                            dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + includeInfo["Params"].ToString());
                            using (ExcelXmlWriter ew = new ExcelXmlWriter(dir + "\\referencedata\\standard\\" + includeInfo["Params"].ToString() + ".xml"))
                            {
                                ew.WriteExcelXml(dt, includeInfo["Params"].ToString());
                            }
                        }
                        break;
                    case ModuleIncludeType.ApplicationDictionaryData:
                        {
                            dt = DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + includeInfo["Params"].ToString());
                            using (ExcelXmlWriter ew = new ExcelXmlWriter(dir + "\\src-db\\database\\sourcedata\\" + includeInfo["Params"].ToString() + ".xml"))
                            {
                                ew.WriteExcelXml(dt, includeInfo["Params"].ToString());
                            }
                        }
                        break;
                    //case ModuleIncludeType.DbTable:
                    //    {
                    //        string script = includeInfo["Params"].ToString();
                    //        DbHelper.Instance.ExecuteNonQuery(script);
                    //    }
                    //    break;
                    case ModuleIncludeType.DbView:
                        {
                            string script = DbHelper.Instance.GetSqlObjectDefinition(includeInfo["Params"].ToString());
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dir + "\\src-db\\database\\model\\views\\" + includeInfo["Params"].ToString(), false, Encoding.UTF8))
                            {
                                sw.Write(script);
                            }
                        }
                        break;
                    case ModuleIncludeType.DbFunction:
                        {
                            string script = DbHelper.Instance.GetSqlObjectDefinition(includeInfo["Params"].ToString());
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dir + "\\src-db\\database\\model\\functions\\" + includeInfo["Params"].ToString(), false, Encoding.UTF8))
                            {
                                sw.Write(script);
                            }
                        }
                        break;
                    case ModuleIncludeType.DbTrigger:
                        {
                            string script = DbHelper.Instance.GetSqlObjectDefinition(includeInfo["Params"].ToString());
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dir + "\\src-db\\database\\model\\triggers\\" + includeInfo["Params"].ToString(), false, Encoding.UTF8))
                            {
                                sw.Write(script);
                            }
                        }
                        break;
                    case ModuleIncludeType.DbProcedure:
                        {
                            string script = DbHelper.Instance.GetSqlObjectDefinition(includeInfo["Params"].ToString());
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(dir + "\\src-db\\database\\model\\procedures\\" + includeInfo["Params"].ToString(), false, Encoding.UTF8))
                            {
                                sw.Write(script);
                            }
                        }
                        break;
                    //case ModuleIncludeType.SourceModel:
                    //    {
                    //        string fileName = includeInfo["Params"].ToString();
                    //        string newFileName = null;
                    //        System.IO.File.Copy(fileName, newFileName);
                    //    }
                    //    break;
                    //case ModuleIncludeType.SourceScript:
                    //    {
                    //        string fileName = includeInfo["Params"].ToString();
                    //        string newFileName = null;
                    //        System.IO.File.Copy(fileName, newFileName);
                    //    }
                    //    break;
                    //case ModuleIncludeType.SourceReport:
                    //    {
                    //        string fileName = includeInfo["Params"].ToString();
                    //        string newFileName = null;
                    //        System.IO.File.Copy(fileName, newFileName);
                    //    }
                    //    break;
                    default:
                        throw new NotSupportedException("ModuleIncludeType");
                }

            }

            // zip
            string zipFile = dir + "\\..\\" + moduleName + ".zip";
            CompressionHelper.ZipFromFolder(dir, zipFile);

            byte[] data;
            using (System.IO.FileStream fs = new System.IO.FileStream(zipFile, System.IO.FileMode.Open))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }

            using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<ModuleInfo>())
            {
                try
                {
                    rep.BeginTransaction();
                    IList<ModuleInfo> modules = (rep as Feng.NH.INHibernateRepository).Session
                        .CreateCriteria<ModuleInfo>()
                        .Add(NHibernate.Criterion.Expression.Eq("Name", moduleName))
                        .List<ModuleInfo>();
                    modules[0].ModuleData = data;
                    rep.Update(modules[0]);

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }


        /// <summary>
        /// 安装Module
        /// </summary>
        /// <param name="moduleName"></param>
        public static void InstallModule(string moduleName)
        {
            CreateNecessaryDirectories(moduleName);

            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;

            //// CheckModuleDependency
            //IList<ModuleDependencyInfo> moduleDependencyInfos = ADInfoBll.Instance.GetInfos<ModuleDependencyInfo>(
            //    "from ModuleDependencyInfo where Module.Name = :moduleName", new Dictionary<string, object> { { "moduleName", moduleName } });
            //foreach (ModuleDependencyInfo dependency in moduleDependencyInfos)
            //{
            //    if (dependency.DependentModule
            //}
            //// No use now
            ////ModuleByClientInfo moduleByClientInfo = null;
            ////ModuleByOrgInfo moduleByOrgInfo = null;

            IList<ModuleInfo> moduleInfos = ADInfoBll.Instance.GetInfos<ModuleInfo>("from Feng.ModuleInfo where Id = '" + moduleName + "'");
            if (moduleInfos.Count == 0)
            {
                throw new ArgumentException("There is no module named " + moduleName);
            }
            CompressionHelper.DecompressToFolder(moduleInfos[0].ModuleData, System.IO.Directory.GetCurrentDirectory());

            // ReferenceData
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\referencedata\\standard\\"))
            {
                ADUtils.ImportFromXmlFile(file);
            }
            // ApplicationDictionaryData
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\sourcedata\\"))
            {
                string s = System.IO.Path.GetFileNameWithoutExtension(file);
                if (s.StartsWith("AD_Module"))
                    continue;
                ADUtils.ImportFromXmlFile(file);
            }

            //// DbTable
            //foreach (string file in System.IO.Directory.GetFiles(dir + "src-db\\database\\model\\tables\\"))
            //{
            //}

            // DbView
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\views\\"))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    string script = sr.ReadToEnd();
                    DbHelper.Instance.ExecuteNonQuery(script);
                }
            }
            // DbFunction
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\functions\\"))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    string script = sr.ReadToEnd();
                    DbHelper.Instance.ExecuteNonQuery(script);
                }
            }
            // DbTrigger
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\triggers\\"))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    string script = sr.ReadToEnd();
                    DbHelper.Instance.ExecuteNonQuery(script);
                }
            }
            // DbProcedure
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\procedures\\"))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    string script = sr.ReadToEnd();
                    DbHelper.Instance.ExecuteNonQuery(script);
                }
            }

            //// SourceModel
            //foreach (string file in System.IO.Directory.GetFiles(dir + "\\src\\model\\"))
            //{
            //}

            //// SourceScript
            //foreach (string file in System.IO.Directory.GetFiles(dir + "\\src\\script\\"))
            //{
            //}

            //// SourceReport
            //foreach (string file in System.IO.Directory.GetFiles(dir + "\\src\\report\\"))
            //{
            //}
        }

        /// <summary>
        /// 反安装
        /// </summary>
        /// <param name="moduleName"></param>
        public static void UninstallModule(string moduleName)
        {
            CreateNecessaryDirectories(moduleName);

            string dir = System.IO.Directory.GetCurrentDirectory() + "\\" + moduleName;

            IList<ModuleInfo> moduleInfos = ADInfoBll.Instance.GetInfos<ModuleInfo>("from Feng.ModuleInfo where Id = '" + moduleName + "'");
            if (moduleInfos.Count == 0)
            {
                throw new ArgumentException("There is no module named " + moduleName);
            }
            CompressionHelper.DecompressToFolder(moduleInfos[0].ModuleData, System.IO.Directory.GetCurrentDirectory());

            // ReferenceData
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\referencedata\\standard\\"))
            {
                ADUtils.DeleteFromXmlFile(file);
            }
            // ApplicationDictionaryData
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\sourcedata\\"))
            {
                string s = System.IO.Path.GetFileNameWithoutExtension(file);
                if (s.StartsWith("AD_Module"))
                    continue;

                ADUtils.DeleteFromXmlFile(file);
            }

            //// DbTable
            //foreach (string file in System.IO.Directory.GetFiles(dir + "src-db\\database\\model\\tables\\"))
            //{
            //}

            // DbView
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\views\\"))
            {
                string script = "DROP VIEW " + System.IO.Path.GetFileNameWithoutExtension(file);
                DbHelper.Instance.ExecuteNonQuery(script);
            }
            // DbFunction
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\functions\\"))
            {
                string script = "DROP FUNCTION " + System.IO.Path.GetFileNameWithoutExtension(file);
                DbHelper.Instance.ExecuteNonQuery(script);
            }
            // DbTrigger
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\triggers\\"))
            {
                string script = "DROP TRIGGER " + System.IO.Path.GetFileNameWithoutExtension(file);
                DbHelper.Instance.ExecuteNonQuery(script);
            }
            // DbProcedure
            foreach (string file in System.IO.Directory.GetFiles(dir + "\\src-db\\database\\model\\procedures\\"))
            {
                string script = "DROP PROCEDURE " + System.IO.Path.GetFileNameWithoutExtension(file);
                DbHelper.Instance.ExecuteNonQuery(script);
            }
        }

    }
}
