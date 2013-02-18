using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Feng.Updater
{
    public class UpdateHelper
    {
        public static void UpdateFiles()
        {
            //System.Windows.Forms.MessageBox.Show("Fd");

            string executeFile = Assembly.GetExecutingAssembly().Location;
            string executablePath = Path.GetDirectoryName(executeFile) ;
            string updatePath = executablePath
                    + Path.DirectorySeparatorChar + "update" + Path.DirectorySeparatorChar;
            string callFileName = Path.GetFileName(executeFile);

            if (Directory.Exists(executablePath) && Directory.Exists(updatePath)
                && !File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "update.zip")))
            {
                //This is how we used to do it: Delete the old directory and then move(rename) the new one
                //Directory.Delete(this.executablePath, true);
                //Directory.Move(this.updatePath, this.executablePath);

                //Now we just move the new files in the update directory and then delete it
                string error = moveDirectoryFiles(updatePath, executablePath, callFileName);
                
                try
                {
                    if (Directory.GetFiles(updatePath).Length == 0)
                    {
                        Directory.Delete(updatePath, true);
                    }
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    error += ex.Message;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    System.Windows.Forms.MessageBox.Show(error);
                }
            }
        }

        private static string moveDirectoryFiles(string stSourcePath, string stDestPath, string callFileName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (!stSourcePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                stSourcePath = stSourcePath + Path.DirectorySeparatorChar;
            if (!stDestPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                stDestPath = stDestPath + Path.DirectorySeparatorChar;

            foreach (string stFile in Directory.GetFiles(stSourcePath))
            {
                string stFileName = Path.GetFileName(stFile);
                if (stFileName == callFileName)
                    continue;

                try
                {
                    // 扩展名是.delete，则删除
                    if (Path.GetExtension(stFileName) == ".delete")
                    {
                        File.Delete(stDestPath + Path.GetFileNameWithoutExtension(stFileName));
                        File.Delete(stSourcePath + stFileName);
                    }
                    else
                    {
                        if (File.Exists(stDestPath + stFileName))
                            File.Delete(stDestPath + stFileName);
                        File.Move(stSourcePath + stFileName, stDestPath + stFileName);
                    }
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
            }
            foreach (string stDir in Directory.GetDirectories(stSourcePath))
            {
                string stDirName = Path.GetFileName(stDir);
                try
                {
                    if (!Directory.Exists(stDestPath + stDirName))
                        Directory.CreateDirectory(stDestPath + stDirName);
                    string error = moveDirectoryFiles(stSourcePath + stDirName, stDestPath + stDirName, callFileName);
                    if (!string.IsNullOrEmpty(error))
                    {
                        sb.Append(error);
                    }
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
            }

            return sb.ToString();
        }
    }
}
