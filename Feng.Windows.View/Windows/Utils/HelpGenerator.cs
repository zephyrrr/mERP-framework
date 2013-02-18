using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Feng.Windows.Utils
{
	/// <summary>
	/// HelpGenerator
	/// </summary>
    public static class HelpGenerator
    {
        ///// <summary>
        ///// 用pre标签输出原有的模板格式，为了不让长的语句将页面撑宽，在语句中加入换行符
        ///// </summary>
        ///// <param name="oldHelp"></param>
        ///// <returns></returns>
        //private static string ProcessHelp(string oldHelp)
        //{
        //    int length = 50;    // 换行的长度
        //    string newHelp = "";
        //    string[] temp = System.Text.RegularExpressions.Regex.Split(oldHelp, "\r\n");

        //    for (int i = 0; i < temp.Length; i++)
        //    {
        //        if (temp[i].Length > length)
        //        {
        //            for (int k = 0; k < temp[i].Length / length; k++)// 每50长度插入“\r\n”
        //            {
        //                temp[i] = temp[i].Insert(length * (k + 1), "\r\n");
        //            }
        //        }
        //    }
        //    for (int i = 0; i < temp.Length; i++)
        //    {
        //        if (i == length - 1)
        //        {
        //            newHelp += temp[i];
        //        }
        //        else
        //        {
        //            newHelp += temp[i] + "\r\n";
        //        }
        //    }

        //    return newHelp;
        //}

        private static string ProcessXmlString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            return str.Replace("\r\n", "<br/>");
        }

        private static string GetImageName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return "iconProcess";
            else
                return imageName;
        }

    	/// <summary>
    	/// GenerateWindowXml
    	/// </summary>
    	/// <param name="fileName"></param>
    	/// <param name="window"></param>
    	public static string GenerateWindowXml(string fileName, WindowInfo window)
    	{
            using (NH.INHibernateRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<WindowInfo>() as NH.INHibernateRepository)
            {
	    		XmlDocument xmlDoc = new XmlDocument();
	    		// Write down the XML declaration
	    		XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
	    		
	    		// Create the root element
	    		XmlElement rootNode = xmlDoc.CreateElement("Window");
                rootNode.SetAttribute("Name", ProcessXmlString(window.Name));
	    		rootNode.SetAttribute("Text", ProcessXmlString(window.Text));
                //// 处理Help
                //if (window.Help != null)
                //{
                //    window.Help = ProcessHelp(window.Help);
                //}
	    		rootNode.SetAttribute("Help", ProcessXmlString(window.Help));
	    		rootNode.SetAttribute("Description", ProcessXmlString(window.Description));
	    		xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
	    		xmlDoc.AppendChild(rootNode);
	    		
	    		// WindowMenu
	    		var windowMenus = rep.Session.CreateCriteria<WindowMenuInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("Window", window))
	    			.Add(NHibernate.Criterion.Expression.Not(NHibernate.Criterion.Expression.Eq("Type", WindowMenuType.Separator)))
	    			.AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
	    			.List<WindowMenuInfo>();
	    		foreach(var i in windowMenus)
	    		{
	    			XmlElement node = xmlDoc.CreateElement("WindowMenu");
	    			node.SetAttribute("Name", ProcessXmlString(i.Name));
	    			node.SetAttribute("Text", ProcessXmlString(i.Text));
	    			node.SetAttribute("Help", ProcessXmlString(i.Help));
	    			node.SetAttribute("Description", ProcessXmlString(i.Description));
                    node.SetAttribute("Icon", GetImageName(i.ImageName));
	    			rootNode.AppendChild(node);
	    		}
	    		
	    		// WindowTab
	    		var tabs = rep.Session.CreateCriteria<WindowTabInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("Window", window))
	    			.AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
	    			.List<WindowTabInfo>();
	    		foreach (var tab in tabs)
	    		{
	    			XmlElement tabNode = xmlDoc.CreateElement("WindowTab");
	    			tabNode.SetAttribute("Name", ProcessXmlString(tab.Name));
	    			tabNode.SetAttribute("Text", ProcessXmlString(tab.Text));
	    			tabNode.SetAttribute("Help", ProcessXmlString(tab.Help));
	    			tabNode.SetAttribute("Description", ProcessXmlString(tab.Description));
	    			tabNode.SetAttribute("GridName", ProcessXmlString(tab.GridName));
	    			rootNode.AppendChild(tabNode);
	
	    			var gridRelateds = rep.Session.CreateCriteria<GridRelatedInfo>()
	    				.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    				.Add(NHibernate.Criterion.Expression.Eq("GridName", tab.GridName))
	    				.AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
	    				.List<GridRelatedInfo>();
	    			foreach (var gridRelated in gridRelateds)
	    			{
	    				XmlElement relatedNode = xmlDoc.CreateElement("GridRelated");
	    				relatedNode.SetAttribute("Name", ProcessXmlString(gridRelated.Name));
	    				relatedNode.SetAttribute("Text", ProcessXmlString(gridRelated.Text));
	    				relatedNode.SetAttribute("Help", ProcessXmlString(gridRelated.Help));
	    				relatedNode.SetAttribute("Description", ProcessXmlString(gridRelated.Description));
	    				relatedNode.SetAttribute("ActionWindowText", gridRelated.Action != null && gridRelated.Action.Window != null ? gridRelated.Action.Window.Text : string.Empty);
	    				relatedNode.SetAttribute("ActionWindowName", gridRelated.Action != null && gridRelated.Action.Window != null ? gridRelated.Action.Window.Name : string.Empty);
	    				tabNode.AppendChild(relatedNode);
	    			}

                    var searchCustoms = rep.Session.CreateCriteria<CustomSearchInfo>()
                        .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                        .Add(NHibernate.Criterion.Expression.Eq("GroupName", tab.GridName))
                        .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                        .List<CustomSearchInfo>();
                    foreach (var searchCustom in searchCustoms)
                    {
                        XmlElement relatedNode = xmlDoc.CreateElement("SearchCustom");
                        relatedNode.SetAttribute("Name", searchCustom.Name);
                        relatedNode.SetAttribute("Text", searchCustom.Text);
                        relatedNode.SetAttribute("GroupName", searchCustom.GroupName);
                        relatedNode.SetAttribute("Help", searchCustom.Help);
                        tabNode.AppendChild(relatedNode);
                    }
	    		}
	    		
	    		// Attachment
	    		IList<AttachmentInfo> attachments = rep.Session.CreateCriteria<AttachmentInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("EntityName",  "WindowInfo"))
	    			.Add(NHibernate.Criterion.Expression.Eq("EntityId", window.Name))
	    			.List<AttachmentInfo>();
	    		foreach(var attachment in attachments)
	    		{
	    			XmlElement node = xmlDoc.CreateElement("Attachment");
	    			node.SetAttribute("Id", attachment.ID.ToString());
	    			node.SetAttribute("Description", ProcessXmlString(attachment.Description));
	    			node.SetAttribute("FileName", ProcessXmlString(GetDefaultAttachmentFileName(attachment)));
	    			//node.SetAttribute("Data", Convert.ToBase64String(attachment.Data));
	    			rootNode.AppendChild(node);
	    		}

	    		xmlDoc.Save(fileName);
	    		
	    		return fileName;
    		}
    	}
    	
    	private static string GetDefaultAttachmentFileName(AttachmentInfo attachment)
    	{
    		if (!attachment.FileName.EndsWith(".zip"))
    			return attachment.FileName;
    		
    		// remove ".zip"
    		return attachment.FileName + "/" + attachment.FileName.Substring(0, attachment.FileName.Length - 4);
    	}
    	
    	/// <summary>
    	/// GenerateGridXml
    	/// </summary>
    	/// <param name="fileName"></param>
    	/// <param name="gridName"></param>
    	public static string GenerateGridXml(string fileName, string gridName)
    	{
            using (NH.INHibernateRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<GridColumnInfo>() as NH.INHibernateRepository)
            {
	    		XmlDocument xmlDoc = new XmlDocument();
	    		// Write down the XML declaration
	    		XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
	    		
	    		// Create the root element
	    		XmlElement rootNode = xmlDoc.CreateElement("Grid");
	    		rootNode.SetAttribute("GridName", ProcessXmlString(gridName));
	    		xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
	    		xmlDoc.AppendChild(rootNode);
	    		
	    		var gridColumns = rep.Session.CreateCriteria<GridColumnInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
	    			.AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
	    			.List<GridColumnInfo>();
	    		foreach (var gridColumn in gridColumns)
	    		{
	    			XmlElement columnNode = xmlDoc.CreateElement("GridColumn");
	    			columnNode.SetAttribute("Name", ProcessXmlString(gridColumn.Name));
	    			columnNode.SetAttribute("Caption", ProcessXmlString(gridColumn.Caption));
	    			columnNode.SetAttribute("Help", ProcessXmlString(gridColumn.Help));
	    			columnNode.SetAttribute("Description", ProcessXmlString(gridColumn.Description));
	    			columnNode.SetAttribute("IsDataControl", (gridColumn.GridColumnType != GridColumnType.NoColumn).ToString());
	    			columnNode.SetAttribute("IsSearchControl", (!string.IsNullOrEmpty(gridColumn.SearchControlType)).ToString());
	    			rootNode.AppendChild(columnNode);
	    		}

	    		var grids = rep.Session.CreateCriteria<GridInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
	    			.List<GridInfo>();
	    		foreach (var grid in grids)
	    		{
	    			XmlElement gridNode = xmlDoc.CreateElement("Grid");
	    			gridNode.SetAttribute("Name", ProcessXmlString(grid.Name));
	    			gridNode.SetAttribute("Help", ProcessXmlString(grid.Help));
	    			gridNode.SetAttribute("Description", ProcessXmlString(grid.Description));
	    			rootNode.AppendChild(gridNode);
	    		}
	    		
	    		var gridRows = rep.Session.CreateCriteria<GridRowInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
	    			.List<GridRowInfo>();
	    		foreach (var gridRow in gridRows)
	    		{
	    			XmlElement rowNode = xmlDoc.CreateElement("GridRow");
	    			rowNode.SetAttribute("Name", ProcessXmlString(gridRow.Name));
	    			rowNode.SetAttribute("Help", ProcessXmlString(gridRow.Help));
	    			rowNode.SetAttribute("Description", ProcessXmlString(gridRow.Description));
	    			rootNode.AppendChild(rowNode);
	    		}
	    		
	    		var gridCells = rep.Session.CreateCriteria<GridCellInfo>()
	    			.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
	    			.Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
	    			.List<GridCellInfo>();
	    		foreach (var gridCell in gridCells)
	    		{
	    			XmlElement cellNode = xmlDoc.CreateElement("GridCell");
	    			cellNode.SetAttribute("Name", ProcessXmlString(gridCell.Name));
	    			cellNode.SetAttribute("Help", ProcessXmlString(gridCell.Help));
	    			cellNode.SetAttribute("Description", ProcessXmlString(gridCell.Description));
	    			rootNode.AppendChild(cellNode);
	    		}
	    			
	    		xmlDoc.Save(fileName);
	    		return fileName;
    		}
    	}
    	
    	/// <summary>
    	/// GenerateXml
    	/// </summary>
    	/// <param name="folderName"></param>
        public static void GenerateXml(string folderName)
        {
            using (NH.INHibernateRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<WindowInfo>() as NH.INHibernateRepository)
            {
            	var windows = rep.Session.CreateCriteria<WindowInfo>()
            		.Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
            		.List<WindowInfo>();

                foreach (var window in windows)
                {
                	GenerateWindowXml(folderName + "\\Window\\" + window.Name + ".xml", window);
                }
                
                System.Data.DataTable dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT DISTINCT GRIDNAME FROM AD_WINDOW_TAB");
                foreach(System.Data.DataRow row in dt.Rows)
                {
                	string gridName = row[0].ToString();
                	
                	GenerateGridXml(folderName + "\\Grid\\" + gridName + ".xml", gridName);
                }
            }
        }

        /// <summary>
        /// Generate Html 
        /// </summary>
        /// <param name="xmlDataFileName"></param>
        /// <param name="xsltFileName"></param>
        /// <param name="helpFileName"></param>
        public static string GenerateHtml(string xmlDataFileName, string xsltFileName, string helpFileName)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            using (var sr = new System.IO.StreamReader(xsltFileName))
            {
                xslt.Load(XmlReader.Create(sr));
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(helpFileName))
            {
                using (var sr = new System.IO.StreamReader(xmlDataFileName))
                {
                    xslt.Transform(XmlReader.Create(sr), null, sw);
                }
            }
            
            return helpFileName;
        }

        private const string s_helpDir = "Help\\";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowName"></param>
        public static string GenerateWindowHelp(string windowName)
        {
        	WindowInfo window = ADInfoBll.Instance.GetWindowInfo(windowName);
        	if (window == null)
        		return null;
        	string fileName = GenerateWindowXml(System.IO.Path.GetTempFileName(), window);

        	string ret = GenerateHtml(fileName, s_helpDir + "window_template.xslt", s_helpDir + "window_" + windowName + ".html");
            System.IO.File.Delete(fileName);

        	IList<WindowTabInfo> tabs = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowName);
        	foreach(string gridName in GetGridNameFromWindowTabs(tabs))
        	{

        		fileName = GenerateGridXml(System.IO.Path.GetTempFileName(), gridName);
                GenerateHtml(fileName, s_helpDir + "grid_template.xslt", s_helpDir + "grid_" + gridName + ".html");
                System.IO.File.Delete(fileName);
        	}
        	
        	return ret;
        }
        
        private static List<string> GetGridNameFromWindowTabs(IList<WindowTabInfo> tabs)
        {
        	List<string> ret = new List<string>();
        	foreach(var i in tabs)
        	{
        		ret.Add(i.GridName);
        		
        		var ret2 = GetGridNameFromWindowTabs(i.ChildTabs);
        		foreach(var j in ret2)
        		{
        			ret.Add(j);
        		}
        	}
        	return ret;
        }

        //private void btnGenerateHelpXml_Click(object sender, EventArgs e)
        //{
        //    IOHelper.TryCreateDirectory(".\\Help\\Data\\Window\\");
        //    IOHelper.TryCreateDirectory(".\\Help\\Data\\Grid\\");

        //    Feng.Utils.HelpGenerator.GenerateXml(".\\Help\\Data");
        //}

        //private void btnGenerateHelpHtml_Click(object sender, EventArgs e)
        //{
        //    IOHelper.TryCreateDirectory(".\\Help\\");

        //    foreach (string fileName in System.IO.Directory.GetFiles(".\\Help\\Data\\Window"))
        //    {
        //        if (!fileName.Contains("资金票据_凭证.xml"))
        //            continue;
        //        Feng.Utils.HelpGenerator.GenerateHtml(fileName, ".\\Help\\window_template.xslt",
        //                                              ".\\Help\\window_" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".html");
        //    }
        //    foreach (string fileName in System.IO.Directory.GetFiles(".\\Help\\Data\\Grid"))
        //    {
        //        if (!fileName.Contains("凭证.xml"))
        //            continue;
        //        Feng.Utils.HelpGenerator.GenerateHtml(fileName, ".\\Help\\grid_template.xslt",
        //                                              ".\\Help\\grid_" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".html");
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileName"></param>
        //public static void GenerateXml(string fileName)
        //{
        //    //DataSet ds = new DataSet();
        //    //ds.DataSetName = "Help";
        //    //ds.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;

        //    //System.Data.DataTable dtWindow = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT Name, Help, Description FROM AD_WINDOW");
        //    //dtWindow.TableName = "AD_WINDOW";
        //    //ds.Tables.Add(dtWindow);

        //    //System.Data.DataTable dtWindowTab = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT Name, Window, GridName, Help, Description FROM AD_WINDOW_TAB");
        //    //dtWindowTab.TableName = "AD_WINDOW_TAB";
        //    //ds.Tables.Add(dtWindowTab);

        //    //DataRelation dr = new DataRelation("Window_Tab", dtWindow.Columns["Name"], dtWindowTab.Columns["Window"]);
        //    //dr.Nested = true;
        //    //ds.Relations.Add(dr);

        //    //System.Data.DataTable dtGrid = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT Name, GridName, Help, Description FROM AD_Grid");
        //    //dtGrid.TableName = "AD_Grid";
        //    //ds.Tables.Add(dtGrid);
        //    //DataRelation dr2 = new DataRelation("Tab_Grid", dtWindowTab.Columns["GridName"], dtGrid.Columns["GridName"], false);
        //    //dr2.Nested = true;
        //    //ds.Relations.Add(dr2);

        //    //ds.WriteXml("c:\\a.xml");

        //    IList<WindowInfo> windows;
        //    using (Feng.NH.INHibernateRepository rep = new Feng.NH.Repository("default"))
        //    {
        //        windows = rep.Session.CreateCriteria<WindowInfo>()
        //            .SetFetchMode("WindowTabs", NHibernate.FetchMode.Join)
        //            //.SetFetchMode("WindowTabs.GridInfos", NHibernate.FetchMode.Select)
        //            //.SetFetchMode("WindowTabs.GridColumnInfos", NHibernate.FetchMode.Select)
        //            //.SetFetchMode("WindowTabs.GridRowInfos", NHibernate.FetchMode.Select)
        //            //.SetFetchMode("WindowTabs.GridCellInfos", NHibernate.FetchMode.Select)
        //            .SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer())
        //            .List<WindowInfo>();

        //        foreach (var i in windows)
        //        {
        //            //if (i.Name.StartsWith("AD_") || i.Name.StartsWith("SD_")
        //            //    || i.Name == "报表_财务费用开支明细")
        //            //    continue;

        //            //rep.Initialize(i.WindowTabs, i);

        //            foreach (var j in i.WindowTabs)
        //            {
        //                var criteria1 = NHibernate.Criterion.DetachedCriteria.For<GridInfo>()
        //                    .Add(NHibernate.Criterion.Restrictions.Eq("GridName", j.GridName));

        //                var criteria2 = NHibernate.Criterion.DetachedCriteria.For<GridColumnInfo>()
        //                    .Add(NHibernate.Criterion.Restrictions.Eq("GridName", j.GridName));
        //                var criteria3 = NHibernate.Criterion.DetachedCriteria.For<GridRowInfo>()
        //                    .Add(NHibernate.Criterion.Restrictions.Eq("GridName", j.GridName));
        //                var criteria4 = NHibernate.Criterion.DetachedCriteria.For<GridCellInfo>()
        //                    .Add(NHibernate.Criterion.Restrictions.Eq("GridName", j.GridName));

        //                var result = rep.Session.CreateMultiCriteria()
        //                        .Add(criteria1)
        //                        .Add(criteria2)
        //                        .Add(criteria3)
        //                        .Add(criteria4)
        //                        .List();

        //                j.GridInfos = ConvertHelper.ChangeListType<GridInfo>((System.Collections.IList)result[0]);
        //                j.GridColumnInfos = ConvertHelper.ChangeListType<GridColumnInfo>((System.Collections.IList)result[1]);
        //                j.GridRowInfos = ConvertHelper.ChangeListType<GridRowInfo>((System.Collections.IList)result[2]);
        //                j.GridCellInfos = ConvertHelper.ChangeListType<GridCellInfo>((System.Collections.IList)result[3]);

        //                //rep.Initialize(j.GridInfos, j);
        //                //rep.Initialize(j.GridColumnInfos, j);
        //                //rep.Initialize(j.GridRowInfos, j);
        //                //rep.Initialize(j.GridCellInfos, j);
        //            }
        //        }

        //        XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
        //        textWriter.Formatting = Formatting.Indented;

        //        textWriter.WriteStartDocument();
        //        textWriter.WriteComment("Feng.Help");

        //        textWriter.WriteStartElement("Windows");
        //        foreach (var i in windows)
        //        {
        //            textWriter.WriteStartElement("Window");

        //            textWriter.WriteAttributeString("Name", i.Name);
        //            textWriter.WriteAttributeString("Description", i.Description);
        //            textWriter.WriteAttributeString("Help", i.Help);

        //            textWriter.WriteStartElement("WindowTabs");
        //            foreach (var j in i.WindowTabs)
        //            {
        //                //if (!NHibernate.NHibernateUtil.IsInitialized(j.GridInfos))
        //                //    continue;

        //                textWriter.WriteStartElement("WindowTab");

        //                textWriter.WriteAttributeString("Name", j.Name);
        //                textWriter.WriteAttributeString("Description", j.Description);
        //                textWriter.WriteAttributeString("Help", j.Help);

        //                // Grid
        //                textWriter.WriteStartElement("Grids");
        //                foreach (var k in j.GridInfos)
        //                {
        //                    textWriter.WriteStartElement("Grid");

        //                    textWriter.WriteAttributeString("Name", k.Name);
        //                    textWriter.WriteAttributeString("Description", k.Description);
        //                    textWriter.WriteAttributeString("Help", k.Help);

        //                    textWriter.WriteEndElement();
        //                }
        //                textWriter.WriteEndElement();

        //                // GridColumn
        //                textWriter.WriteStartElement("GridColumns");
        //                foreach (var k in j.GridColumnInfos)
        //                {
        //                    textWriter.WriteStartElement("GridColumn");

        //                    textWriter.WriteAttributeString("Name", k.Name);
        //                    textWriter.WriteAttributeString("Description", k.Description);
        //                    textWriter.WriteAttributeString("Help", k.Help);

        //                    textWriter.WriteEndElement();
        //                }
        //                textWriter.WriteEndElement();

        //                // GridRow
        //                textWriter.WriteStartElement("GridRows");
        //                foreach (var k in j.GridRowInfos)
        //                {
        //                    textWriter.WriteStartElement("GridRow");

        //                    textWriter.WriteAttributeString("Name", k.Name);
        //                    textWriter.WriteAttributeString("Description", k.Description);
        //                    textWriter.WriteAttributeString("Help", k.Help);

        //                    textWriter.WriteEndElement();
        //                }
        //                textWriter.WriteEndElement();

        //                // GridCell
        //                textWriter.WriteStartElement("GridCells");
        //                foreach (var k in j.GridCellInfos)
        //                {
        //                    textWriter.WriteStartElement("GridCell");

        //                    textWriter.WriteAttributeString("Name", k.Name);
        //                    textWriter.WriteAttributeString("Description", k.Description);
        //                    textWriter.WriteAttributeString("Help", k.Help);

        //                    textWriter.WriteEndElement();
        //                }
        //                textWriter.WriteEndElement();

        //                textWriter.WriteEndElement();
        //            }
        //            textWriter.WriteEndElement();

        //            textWriter.WriteEndElement();
        //        }
        //        textWriter.WriteEndElement();

        //        textWriter.WriteEndDocument();

        //        textWriter.Close();
        //    }
        //}
    }
}
