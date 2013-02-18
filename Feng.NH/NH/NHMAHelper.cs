using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate.Mapping.Attributes;

namespace Feng.NH
{
    public static class NHMAHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string ExportMappingAttribute()
        {
            List<string> assemblyNames = new List<string>();

            SessionFactoriesConfigSection repositoryConfig = System.Configuration.ConfigurationManager
               .GetSection("nhibernateSettings") as SessionFactoriesConfigSection;
            if (repositoryConfig != null)
            {
                foreach (SessionFactoriesConfigSectionElement i in repositoryConfig.SessionFactories)
                {
                    switch (i.Type)
                    {
                        case "attribute":
                            assemblyNames.Add(i.Name);
                            break;
                    }
                }
            }
            return ExportMappingAttribute(assemblyNames.ToArray());
        }

        /// <summary>
        /// 导出到Hbm文件
        /// </summary>
        /// <param name="assemblyNames"></param>
        public static string ExportMappingAttribute(string[] assemblyNames)
        {
            string outputFileName = System.IO.Directory.GetCurrentDirectory() + "\\hbm\\DomainModel.hbm.xml";
            return ExportMappingAttribute(outputFileName, assemblyNames);
        }

        /// <summary>
        /// 导出到Hbm文件
        /// </summary>
        /// <param name="outputFileName"></param>
        /// <param name="assemblyNames"></param>
        public static string ExportMappingAttribute(string outputFileName, string[] assemblyNames)
        {
            FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);

            HbmSerializer.Default.Validate = true;
            HbmSerializer.Default.WriteDateComment = false;

            System.Xml.XmlTextWriter writer = null;
            for (int i = 0; i < assemblyNames.Length; ++i)
            {
                Assembly assembly = System.Reflection.Assembly.Load(assemblyNames[i]);
                Type[] types = assembly.GetTypes();

                for (int j = 0; j < types.Length; ++j)
                {
                    bool isLast = false;
                    if (i == assemblyNames.Length - 1 && j == types.Length - 1)
                        isLast = true;

                    try
                    {
                        writer = HbmSerializer.Default.Serialize(fs, types[j], writer, isLast);
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                    }
                }
            }

            return outputFileName;
        }

        /// <summary>
        /// 读入Assembly, 读入Attribute，初始化SessionFactory
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="assemblyName"></param>
        public static void MemorizeMappingAttribute(NHibernate.Cfg.Configuration cfg, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            Assembly assembly = null;
            try
            {
                assembly = System.Reflection.Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }

            try
            {
                if (assembly != null)
                {
                    MemorizeMappingAttribute(cfg, assembly);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 读入Attribute到内存
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="assembly">要读入的AssemblyName</param>
        public static void MemorizeMappingAttribute(NHibernate.Cfg.Configuration cfg, Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            HbmSerializer.Default.Validate = true;

            HbmSerializer.Default.HbmAssembly = assembly.GetName().Name;
            HbmSerializer.Default.HbmNamespace = assembly.GetTypes()[0].Namespace;

            // Here, we serialize all decorated classes (but you can also do it class by class)
            HbmSerializer.Default.Serialize(stream, assembly);
            // System.Reflection.Assembly.GetExecutingAssembly()

            stream.Position = 0; // Rewind

            //using (System.IO.FileStream sw = new FileStream("c:\\test.xml", FileMode.Create))
            //{
            //    stream.WriteTo(sw);
            //}

            cfg.AddInputStream(stream); // Use the stream here

            stream.Dispose();
        }

        /// <summary>
        /// 输出ORM Mapping XML
        /// </summary>
        public static string ExportMappingAttribute(string assemblyName)
        {
            HbmSerializer.Default.Validate = true;
            HbmSerializer.Default.WriteDateComment = false;

            Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
            HbmSerializer.Default.HbmAssembly = assembly.GetName().Name;
            string ns = string.Empty;
            foreach(var type in assembly.GetTypes())
            {
                if (string.IsNullOrEmpty(ns) || (!string.IsNullOrEmpty(type.Namespace) && type.Namespace.Length < ns.Length))
                    ns = type.Namespace;
            }
            HbmSerializer.Default.HbmNamespace = ns;

            string outputFileName = System.IO.Directory.GetCurrentDirectory() + "\\hbm\\DomainModel.hbm.xml";
            HbmSerializer.Default.Serialize(assembly, outputFileName);
            return outputFileName;
        }

        /// <summary>
        /// 根据类型输出Hbm file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        public static string ExportMappingAttribute(string filePath, Type type)
        {
            //System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);

            HbmSerializer.Default.Validate = true; // Enable validation (optional)
            HbmSerializer.Default.WriteDateComment = false;
            HbmSerializer.Default.HbmNamespace = type.Assembly.GetName().Name;
            HbmSerializer.Default.HbmAssembly = type.Namespace;

            HbmSerializer.Default.Serialize(filePath, type);

            //stream.Close();
            return filePath;
        }

        /// <summary>
        /// 根据类型输出Hbm file
        /// </summary>
        /// <param name="type"></param>
        public static string ExportMappingAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return ExportMappingAttribute(System.IO.Directory.GetCurrentDirectory() + "\\hbm\\" + type.Name + ".hbm.xml", type);
        }
    }
}
