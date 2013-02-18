using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xceed.Compression;
using Xceed.Compression.Formats;
using Xceed.FileSystem;
using Xceed.Zip;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// Compression
    /// </summary>
    public static class CompressionHelper
    {
        static CompressionHelper()
        {
            XceedLicense.SetXceedLicense("Xceed.Compression", "Xceed.Compression.v5.1", "ZIN51-TR163-A813S-NWTA");
            XceedLicense.SetXceedLicense("Xceed.Compression.Formats", "Xceed.Compression.Formats.v5.1", "ZIN51-TR163-A813S-NWTA");
            XceedLicense.SetXceedLicense("Xceed.Zip", "Xceed.Zip.v5.1", "ZIN51-TR163-A813S-NWTA");
            XceedLicense.SetXceedLicense("Xceed.FileSystem", "Xceed.FileSystem.v5.1", "ZIN51-TR163-A813S-NWTA");
        }

        private const CompressionMethod DefaultCompressionMethod = CompressionMethod.Deflated;
        private const CompressionLevel DefaultCompressionLevel = CompressionLevel.Highest;

        //private static readonly string s_tempZipMemoryFileName;

        #region"byte[] to byte[]"
        /// <summary>
        /// 压缩byte[]至byte[]
        /// </summary>
        /// <param name="srcByte"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] srcByte)
        {
            using (MemoryStream destStream = new MemoryStream())
            {
                using (XceedCompressedStream compressStream = new XceedCompressedStream(destStream, DefaultCompressionMethod, DefaultCompressionLevel))
                {
                    compressStream.Write(srcByte, 0, srcByte.Length);
                }
                return destStream.ToArray();
            }
        }

        /// <summary>
        /// 解压缩byte[]至byte[]
        /// </summary>
        /// <param name="srcByte"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] srcByte)
        {
            using (MemoryStream srcStream = new MemoryStream(srcByte))
            {
                using (MemoryStream destStream = new MemoryStream())
                {
                    byte[] buffer = new byte[32768];
                    int read = 0;

                    using (XceedCompressedStream compressStream = new XceedCompressedStream(srcStream, DefaultCompressionMethod, DefaultCompressionLevel))
                    {
                        while ((read = compressStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destStream.Write(buffer, 0, read);
                        }

                        return destStream.ToArray();
                    }
                }
            }
        }
        #endregion

        #region "string"
        /// <summary>
        /// 压缩文本至byte[]
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static byte[] Compress(string srcText)
        {
            return Compress(System.Text.Encoding.Default.GetBytes(srcText));
        }

        ///// <summary>
        ///// 解压缩byte[]至文本
        ///// </summary>
        ///// <param name="srcByte"></param>
        ///// <returns></returns>
        //public static string Decompress(byte[] srcByte)
        //{
        //    return System.Text.Encoding.Default.GetString(Decompress(srcByte));
        //}
        #endregion

        #region "File"
        /// <summary>
        /// 压缩文件文件至byte[]
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static byte[] CompressFromFile(string srcFile)
        {
            FileStream fileStream = new FileStream(srcFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();

            return Compress(buffer);
        }

        /// <summary>
        /// 解压缩byte[]至文件
        /// </summary>
        /// <param name="srcByte"></param>
        /// <param name="destFile"></param>
        public static string DecompressToFile(byte[] srcByte, string destFile)
        {
            byte[] buffer = Decompress(srcByte);
            FileStream fileStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.Write);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();
            return destFile;
        }

        #endregion

        #region "Folder"
        /// <summary>
        ///  Zip压缩至byte[]
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <returns></returns>
        public static byte[] CompressFromFolder(string srcFolder)
        {
            return CompressFromFolder(srcFolder, true);
        }

        /// <summary>
        /// Zip压缩至byte[]
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <param name="最顶层是否是文件夹（如果不是，则是文件夹下的一个个文件）"></param>
        /// <returns></returns>
        public static byte[] CompressFromFolder(string srcFolder, bool topFolder)
        {
            AbstractFile memoryFile = new MemoryFile();
            //AbstractFile memoryFile = new DiskFile(System.IO.Path.GetTempFileName());

            ZipArchive archive = CreateZipArchive(memoryFile);

            if (topFolder)
            {
                AbstractFolder source = new DiskFolder(srcFolder);
                source.CopyTo(archive, true);
            }
            else
            {
                foreach (var i in archive.GetItems(false))
                {
                    i.CopyTo(archive, true);
                }
            }

            byte[] buffer;
            using (Stream destStream = memoryFile.OpenRead(FileShare.Read))
            {
                buffer = new byte[destStream.Length];
                destStream.Read(buffer, 0, buffer.Length);
                destStream.Close();
            }

            memoryFile.Delete();
            return buffer;
        }

        /// <summary>
        /// Zip解压缩byte[]至文件夹
        /// </summary>
        /// <param name="srcByte"></param>
        /// <param name="destFolder"></param>
        public static void DecompressToFolder(byte[] srcByte, string destFolder)
        {
            AbstractFile memoryFile = new MemoryFile();
            //AbstractFile memoryFile = new DiskFile(System.IO.Path.GetTempPath() + "memoryFile.zip");
            memoryFile.Create();

            using (Stream stream = memoryFile.OpenWrite(true, FileShare.Write))
            {
                stream.Write(srcByte, 0, srcByte.Length);
                stream.Close();
            }

            // 必须先写MemoryFile再创建ZipArchive
            ZipArchive archive = CreateZipArchive(memoryFile);

            AbstractFolder dest = new DiskFolder(destFolder);
            archive.CopyFilesTo(dest, true, true, null);

            memoryFile.Delete();
        }
        #endregion

        private static ZipArchive CreateZipArchive(AbstractFile zipFile)
        {
            // Now that the file does not exist, we can create a new zip file.
            ZipArchive archive = new ZipArchive(zipFile);
            archive.DefaultCompressionMethod = DefaultCompressionMethod;
            archive.DefaultCompressionLevel = DefaultCompressionLevel;

            return archive;
        }

        private static ZipArchive CreateZipArchive(string destFile, bool createNew)
        {
            AbstractFile zipFile = new DiskFile(destFile);

            if (createNew)
            {
                // In order to create a new zip file, all we have to do is make sure
                // that file does not exist before creating our ZipArchive.
                if (zipFile.Exists)
                {
                    zipFile.Delete();
                }
            }
            else
            {
                if (!zipFile.Exists)
                {
                    throw new ArgumentException(destFile + " is not exist!");
                }
            }

            return CreateZipArchive(zipFile);
        }

        /// <summary>
        /// Zip压缩一个文件至压缩文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static string ZipFromFile(string srcFile, string destFile)
        {
            ZipArchive archive = CreateZipArchive(destFile, true);

            AbstractFile source = new DiskFile(srcFile);
            source.CopyTo(archive, true);

            return destFile;
        }

        /// <summary>
        /// Zip压缩文件夹至压缩文件
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static string ZipFromFolder(string srcFolder, string destFile)
        {
            ZipArchive archive = CreateZipArchive(destFile, true);

            AbstractFolder source = new DiskFolder(srcFolder);
            source.CopyTo(archive, true);

            return destFile;
        }

        /// <summary>
        /// 解压缩压缩文件至文件夹
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFolder"></param>
        public static void UnzipToFolder(string srcFile, string destFolder)
        {
            ZipArchive archive = CreateZipArchive(srcFile, false);

            // Just as zipping means "copy to a zip archive", unzipping means
            // "copy from a zip archive". Let's copy all DLL files in a temp folder.
            AbstractFolder dest = new DiskFolder(destFolder);

            // filter = "*.*" will filter file without extention
            archive.CopyFilesTo(dest, true, true, null);
        }
    }
}
