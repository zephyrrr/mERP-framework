using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Feng.Utils
{
    /// <summary>
    /// IOHelper
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// 创建目录（如果不存在）
        /// </summary>
        /// <param name="dirPath"></param>
        public static void TryCreateDirectory(string dirPath)
        {
            if (dirPath.IndexOf('\\') >= 0)
            {
                dirPath = Path.GetDirectoryName(dirPath);
            }

            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// 创建文件（如果不存在）
        /// </summary>
        /// <param name="filePath"></param>
        public static void TryCreateFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);
            }
        }

        /// <summary>
        /// 根据文件名返回临时文件夹中唯一命名的文件的完整路径
        ///   形如：公司文档(1).doc，公司文档(2).doc
        /// </summary>
        public static string GetTempPathFileName(string fileName)
        {
            // 系统临时文件夹
            string tempPath = Path.GetTempPath();
            // 文件的完成路径
            fileName = tempPath + Path.GetFileName(fileName);
            // 文件名
            string fileNameWithoutExt =
                Path.GetFileNameWithoutExtension(fileName);
            // 扩展名
            string fileExt = Path.GetExtension(fileName);
            int i = 0;
            while (File.Exists(fileName))
            {
                // 生成类似这样的文件名：公司文档(1).doc，公司文档(2).doc
                fileName = tempPath + fileNameWithoutExt +
                           string.Format("({0})", ++i) + fileExt;
            }
            return fileName;
        }

        /// <summary>
        /// 强力删除目录
        /// </summary>
        /// <param name="source"></param>
        public static void HardDirectoryDelete(string source)
        {
            try
            {
                if (!Directory.Exists(source))
                {
                    return;
                }

                //Get a reference to the source directory
                DirectoryInfo Source = new DirectoryInfo(source);
                Source.Attributes = FileAttributes.Normal;

                //Clear the attributes on the files
                var Files = Source.EnumerateFiles();
                foreach (FileInfo pFile in Files)
                {
                    pFile.Attributes = FileAttributes.Normal;
                }

                //Recurse into subdirectories
                var Directories = Source.EnumerateDirectories();
                foreach (DirectoryInfo pDirectory in Directories)
                {
                    HardDirectoryDelete(Path.Combine(source, pDirectory.Name));
                }

                Source.Delete(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine("APPMANAGER:  FAILED to delete:  " + source);
                Debug.WriteLine("APPMANAGER:  " + e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}