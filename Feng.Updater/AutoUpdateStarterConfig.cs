///*
// * AutoUpdateStarterConfig.cs
// * This class is the definition for the local XML config file which tells the starter what main application to run
// *  
// * Copyright 2004 Conversive, Inc.
// * 
// */

///*
// * Conversive's C# AutoUpdater Component
// * Copyright 2004 Conversive, Inc.
// * 
// * This is a component which allows automatic software updates.
// * It is written in C# and was developed by Conversive, Inc. on April 14th 2004.
// * 
// * The C# AutoUpdater Component is licensed under the LGPL:
// * ------------------------------------------------------------------------
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 2.1 of the License, or (at your option) any later version.
// * 
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// * Lesser General Public License for more details.
// * 
// * You should have received a copy of the GNU Lesser General Public
// * License along with this library; if not, write to the Free Software
// * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// * 
// * ------------------------------------------------------------------------
// */

//using System;
//using System.IO;
//using System.Xml;
//using System.Diagnostics;

//namespace Conversive.AutoUpdateStarter
//{
//    /// <summary>
//    /// Summary description for AutoUpdateStarterConfig.
//    /// </summary>
//    public class AutoUpdateStarterConfig
//    {
//        private string _ApplicationFolderName = string.Empty;
//        public string ApplicationFolderName 
//        { get { return _ApplicationFolderName; } set { _ApplicationFolderName = value; } }

//        private string _ApplicationExeName = "Feng.Run.exe";
//        public string ApplicationExeName 
//        { get { return _ApplicationExeName; } set { _ApplicationExeName = value; } }

//        private string configFilePath;

//        //calculated/readonly property
//        public string ApplicationExePath
//        {
//            get 
//            {
//                return (Path.Combine(Path.GetDirectoryName(this.configFilePath), this.ApplicationFolderName) + @"\" + this.ApplicationExeName);
//            }
//            set {}
//        }

//        //calculated/readonly property
//        public string ApplicationPath
//        {
//            get 
//            {
//                return (Path.Combine(Path.GetDirectoryName(this.configFilePath), this.ApplicationFolderName)+ @"\");
//            }
//            set {}
//        }

//        /// <summary>
//        /// Load: Returns a AutoUpdateStarterConfig object based on the xml file specified by filepath parameter
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <returns></returns>
//        public static AutoUpdateStarterConfig Load(string filePath)
//        {
//            AutoUpdateStarterConfig config = new AutoUpdateStarterConfig();
//            config.configFilePath = filePath;

//            try 
//            {
//                //Load the xml config file
//                XmlDocument XmlDoc = new XmlDocument();
//                try 
//                {
//                    XmlDoc.Load(filePath);
//                }
//                catch(Exception) 
//                {
//                    Debug.WriteLine("Unable to load the AutoUpdateStarter config file at: " + filePath); 
//                }

//                //Parse out the XML Nodes
//                XmlNode pathNode = XmlDoc.SelectSingleNode(@"//ApplicationFolderName");
//                config.ApplicationFolderName = pathNode.InnerText;

//                XmlNode exeNode = XmlDoc.SelectSingleNode(@"//ApplicationExeName");
//                config.ApplicationExeName = exeNode.InnerText;
//            } 
//            catch (Exception)
//            {
//                //Debug.WriteLine("Failed to read the AutoUpdateStarter config file at: " + filePath); 
//                //Debug.WriteLine("Make sure that the config file is present and has a valid format");
//            }
//            return config;
//        }
//    }
//}
