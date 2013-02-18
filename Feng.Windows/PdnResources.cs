/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Feng.Windows
{
    /// <summary>
    /// PdnResourcesCollection
    /// </summary>
    public class PdnResourcesCollection : Singleton<PdnResourcesCollection>
    {
        private Dictionary<string, PdnResources> m_dict = new Dictionary<string, PdnResources>();
        /// <summary>
        /// AddPdnResource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultNamespace"></param>
        /// <param name="assemblyName"></param>
        /// <param name="isDefault"></param>
        public void AddPdnResource(string name, string defaultNamespace, string assemblyName, bool isDefault)
        {
            m_dict[name] = new PdnResources(defaultNamespace, assemblyName);
            if (isDefault)
            {
                if (!string.IsNullOrEmpty(m_defaultPdnResourceName))
                {
                    throw new ArgumentException("default can only be set to one PdnResources", "defaultNamespace");
                }
                m_defaultPdnResourceName = name;
            }
        }

        /// <summary>
        /// DefaultResourceName
        /// </summary>
        public string DefaultResourceName
        {
            get
            {
                if (string.IsNullOrEmpty(m_defaultPdnResourceName))
                {
                    throw new ArgumentNullException("DefaultResourceName", "Default PdnResourceName is null!");
                }
                return m_defaultPdnResourceName;
            }
        }
        private string m_defaultPdnResourceName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PdnResources this[string name]
        {
            get
            {
                if (!m_dict.ContainsKey(name))
                {
                    //throw new ArgumentException("there is no " + name + " in PdnResourcesCollection!");
                    return null;
                }
                return m_dict[name];
            }
        }
    }
    // How to use
    // System.Drawing.Icon a = PdnResources.GetIconFromImage("Icons.信息.png");
    // ImageResource.Get("Icons.time_star.gif")
    // string s = PdnResources.GetString("SampleString");

    /// <summary>
    /// 资源
    /// </summary>
    public sealed class PdnResources
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <param name="assemblyName"></param>
        public PdnResources(string defaultNamespace, string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            m_assemblyName = assemblyName;
            m_namespace = defaultNamespace;

            //ourAssembly = System.Reflection.Assembly.Load(assemblyName);

            //if (string.IsNullOrEmpty(defaultNamespace))
            //{
            //    ourNamespace = ourAssembly.GetName().Name;
            //}
            //else
            //{
            //    ourNamespace = defaultNamespace;
            //}

            //ourAssembly = Assembly.GetExecutingAssembly();
            //ourNamespace = Assembly.GetExecutingAssembly().GetName().Name;

            Initialize();
        }

        private void Initialize()
        {
            pdnCulture = CultureInfo.CurrentUICulture;
            localeDirs = GetLocaleDirs();
        }

        private ResourceManager m_resourceManager;
        //private string ourNamespace;
        //private Assembly ourAssembly;
        private string[] localeDirs;
        private CultureInfo pdnCulture;
        private string resourcesDir;

        private string m_assemblyName;
        private Assembly m_assembly;
        private string m_namespace;
        private Assembly ResourceAssembly
        {
            get
            {
                if (m_assembly == null)
                {
                    m_assembly = System.Reflection.Assembly.Load(m_assemblyName);
                }
                return m_assembly;
            }
        }
        private string Namespace
        {
            get
            {
                if (string.IsNullOrEmpty(m_namespace))
                {
                    return ResourceAssembly.GetName().Name;
                }
                else
                {
                    return m_namespace;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResourcesDir
        {
            get
            {
                if (resourcesDir == null)
                {
                    resourcesDir = Path.GetDirectoryName(typeof (PdnResources).Assembly.Location);
                }

                return resourcesDir;
            }

            set
            {
                resourcesDir = value;
                Initialize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture
        {
            get { return pdnCulture; }

            set
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;
                Initialize();
            }
        }

        //public static void SetNewCulture(string newLocaleName)
        //{
        //    // TODO, HACK: post-3.0 we must refactor and have an actual user data manager that can handle all this renaming
        //    string oldUserDataPath = PdnInfo.UserDataPath;
        //    string oldPaletteDirName = PdnResources.GetString("ColorPalettes.UserDataSubDirName");
        //    // END HACK

        //    CultureInfo newCI = new CultureInfo(newLocaleName);
        //    //Settings.CurrentUser.SetString("LanguageName", newLocaleName);
        //    Culture = newCI;

        //    // TODO, HACK: finish up renaming
        //    string newUserDataPath = PdnInfo.UserDataPath;
        //    string newPaletteDirName = PdnResources.GetString("ColorPalettes.UserDataSubDirName");

        //    // 1. rename user data dir from old localized name to new localized name
        //    if (oldUserDataPath != newUserDataPath)
        //    {
        //        try
        //        {
        //            Directory.Move(oldUserDataPath, newUserDataPath);
        //        }

        //        catch (Exception)
        //        {
        //        }
        //    }

        //    // 2. rename palette dir from old localized name (in new localized user data path) to new localized name
        //    string oldPalettePath = Path.Combine(newUserDataPath, oldPaletteDirName);
        //    string newPalettePath = Path.Combine(newUserDataPath, newPaletteDirName);

        //    if (oldPalettePath != newPalettePath)
        //    {
        //        try
        //        {
        //            Directory.Move(oldPalettePath, newPalettePath);
        //        }

        //        catch (Exception)
        //        {
        //        }
        //    }
        //    // END HACK
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetInstalledLocales()
        {
            const string left = "Strings.3";
            const string right = ".resources";
            string ourDir = ResourcesDir;
            string fileSpec = left + "*" + right;
            string[] pathNames = Directory.GetFiles(ourDir, fileSpec);
            List<String> locales = new List<string>();

            for (int i = 0; i < pathNames.Length; ++i)
            {
                string pathName = pathNames[i];
                string dirName = Path.GetDirectoryName(pathName);
                string fileName = Path.GetFileName(pathName);
                string sansRight = fileName.Substring(0, fileName.Length - right.Length);
                string sansLeft = sansRight.Substring(left.Length);

                string locale;

                if (sansLeft.Length > 0 && sansLeft[0] == '.')
                {
                    locale = sansLeft.Substring(1);
                }
                else if (sansLeft.Length == 0)
                {
                    locale = "en-US";
                }
                else
                {
                    locale = sansLeft;
                }

                try
                {
                    // Ensure this locale can create a valid CultureInfo object.
                    CultureInfo ci = new CultureInfo(locale);
                }

                catch (Exception)
                {
                    // Skip past invalid locales -- don't let them crash us
                    continue;
                }

                locales.Add(locale);
            }

            return locales.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetLocaleNameChain()
        {
            List<string> names = new List<string>();
            CultureInfo ci = pdnCulture;

            while (!string.IsNullOrEmpty(ci.Name))
            {
                names.Add(ci.Name);
                ci = ci.Parent;
            }

            return names.ToArray();
        }

        private string[] GetLocaleDirs()
        {
            const string rootDirName = "Resources";
            string appDir = ResourcesDir;
            string rootDir = Path.Combine(appDir, rootDirName);
            List<string> dirs = new List<string>();

            CultureInfo ci = pdnCulture;

            while (ci.Name != string.Empty)
            {
                string localeDir = Path.Combine(rootDir, ci.Name);

                if (Directory.Exists(localeDir))
                {
                    dirs.Add(localeDir);
                }

                ci = ci.Parent;
            }

            return dirs.ToArray();
        }

        private ResourceManager CreateResourceManager()
        {
            //const string stringsFileName = "Demo.Resources.Dll";
            //ResourceManager rm = ResourceManager.CreateFileBasedResourceManager(stringsFileName, ResourcesDir, null);
            //ResourceManager rm = new ResourceManager(ourNamespace + ".Strings.Strings", typeof(PdnResources).Assembly);
            ResourceManager rm = new ResourceManager(this.Namespace + ".Resources.Strings", this.ResourceAssembly);

            return rm;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResourceManager Strings
        {
            get 
            {
                if (m_resourceManager == null)
                {
                    m_resourceManager = CreateResourceManager();
                }
                return m_resourceManager; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringName"></param>
        /// <returns></returns>
        public string GetString(string stringName)
        {
            string theString = m_resourceManager.GetString(stringName, pdnCulture);

            if (theString == null)
            {
                Debug.WriteLine(stringName + " not found");
            }

            return theString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream GetResourceStream(string fileName)
        {
            Stream stream = null;

            for (int i = 0; i < localeDirs.Length; ++i)
            {
                string filePath = Path.Combine(localeDirs[i], fileName);

                if (File.Exists(filePath))
                {
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                }
            }

            if (stream == null)
            {
                string fullName = this.Namespace + "." + fileName;
                stream = this.ResourceAssembly.GetManifestResourceStream(fullName);
            }

            return stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNameNoExt"></param>
        /// <returns></returns>
        public Image GetImageBmpOrPng(string fileNameNoExt)
        {
            // using Path.ChangeExtension is not what we want; quite often filenames are "Icons.BlahBlahBlah"
            string fileNameBmp = fileNameNoExt + ".bmp";
            Image image = GetImage(fileNameBmp);

            if (image == null)
            {
                string fileNamePng = fileNameNoExt + ".png";
                image = GetImage(fileNamePng);
            }

            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Image GetImage(string fileName)
        {
            Stream stream = GetResourceStream(fileName);

            Image image = null;
            if (stream != null)
            {
                image = LoadImage(stream);
            }

            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Icon GetIcon(string fileName)
        {
            Stream stream = GetResourceStream(fileName);
            Icon icon = null;

            if (stream != null)
            {
                icon = new Icon(stream);
            }

            return icon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Icon GetIconFromImageFile(string fileName)
        {
            Stream stream = GetResourceStream(fileName);

            Icon icon = null;

            if (stream != null)
            {
                Image image = LoadImage(stream);
                icon = Icon.FromHandle(((Bitmap) image).GetHicon());
                image.Dispose();
                stream.Close();
            }

            return icon;
        }

        /// <summary>
        /// Convert Image To Icon(must be Bitmap)
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Icon GetIconFromImage(Image image)
        {
            Icon icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            image.Dispose();

            return icon;
        }

        private bool CheckForSignature(Stream input, byte[] signature)
        {
            long oldPos = input.Position;
            byte[] inputSig = new byte[signature.Length];
            int amountRead = input.Read(inputSig, 0, inputSig.Length);

            bool foundSig = false;
            if (amountRead == signature.Length)
            {
                foundSig = true;

                for (int i = 0; i < signature.Length; ++i)
                {
                    foundSig &= (signature[i] == inputSig[i]);
                }
            }

            input.Position = oldPos;
            return foundSig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsGdiPlusImageAllowed(Stream input)
        {
            byte[] wmfSig = new byte[] {0xd7, 0xcd, 0xc6, 0x9a};
            byte[] emfSig = new byte[] {0x01, 0x00, 0x00, 0x00};

            // Check for and explicitely block WMF and EMF images
            return !(CheckForSignature(input, emfSig) || CheckForSignature(input, wmfSig));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Image LoadImage(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return LoadImage(stream);
            }
        }

        /// <summary>
        /// Loads an image from the given stream. The stream must be seekable.
        /// </summary>
        /// <param name="input">The Stream to load the image from.</param>
        public Image LoadImage(Stream input)
        {
            /*
            if (!IsGdiPlusImageAllowed(input))
            {
                throw new IOException("File format is not supported");
            }
            */

            Image image = Image.FromStream(input);

            if (image.RawFormat == ImageFormat.Wmf || image.RawFormat == ImageFormat.Emf)
            {
                image.Dispose();
                throw new NotSupportedException("File format isn't supported");
            }

            return image;
        }
    }
}