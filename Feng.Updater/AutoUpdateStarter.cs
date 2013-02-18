/*
 * AutoUpdateStarter.cs
 * This class is Main starter component that is in-charge of running the main app
 * this class will also copy the new update files after the main app has terminated.
 *  
 * Copyright 2004 Conversive, Inc.
 * 
 */

/*
 * Conversive's C# AutoUpdater Component
 * Copyright 2004 Conversive, Inc.
 * 
 * This is a component which allows automatic software updates.
 * It is written in C# and was developed by Conversive, Inc. on April 14th 2004.
 * 
 * The C# AutoUpdater Component is licensed under the LGPL:
 * ------------------------------------------------------------------------
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * ------------------------------------------------------------------------
 */

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Feng.Updater
{
	/// <summary>
	/// Summary description for AutoUpdateStarter.
	/// </summary>
	public class AutoUpdateStarter
	{
        //private AutoUpdateStarterConfig m_config;
		public AutoUpdateStarter()
		{
			string stConfigFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			stConfigFileName = Path.Combine(stConfigFileName, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location));
			stConfigFileName += @".config";

            //m_config = AutoUpdateStarterConfig.Load(stConfigFileName);
		}

        //private static readonly log4net.ILog m_log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string m_executeFile = "Feng.Run.exe";
        private const string m_processName = "Feng.Run";

		/// <summary>
		/// StartProcessAndWait: This will start the appropriate process and wait for it to complete
		/// </summary>
		public void StartProcessAndWait()
		{
            //if (System.Windows.Forms.MessageBox.Show("开始更新！", "Update", System.Windows.Forms.MessageBoxButtons.YesNo)
            //    == System.Windows.Forms.DialogResult.No)
            //    return;

            //m_log.Debug("开始应用更新...");

            Process[] mainProcesses = Process.GetProcessesByName(m_processName);
            if (mainProcesses != null)
            {
                foreach(Process mainProcess in mainProcesses)
                {
                    try
                    {
                        mainProcess.WaitForExit();
                    }
                    catch (Exception)
                    {
                        //Debug.WriteLine("AutoUpdateStarter:  Process.WaitForExit() failed, the process may not be running");
                        //Debug.WriteLine("AutoUpdateStarter:  " + e.ToString());
                        return;
                    }
                }

                UpdateHelper.UpdateFiles();

                //m_log.Debug("应用更新完毕，重启程序。");
                if (System.Windows.Forms.MessageBox.Show("程序更新完毕，确定将重启程序！", "更新", System.Windows.Forms.MessageBoxButtons.OK)
                    == System.Windows.Forms.DialogResult.No)
                    return;

                //Start the app
                try
                {
                    ProcessStartInfo p = new ProcessStartInfo(m_executeFile);
                    p.WorkingDirectory = Directory.GetCurrentDirectory();
                    p.Arguments = null;
                    var process = Process.Start(p);
                    //Debug.WriteLine("AutoUpdateStarter:  Started app:  " + this.executablePath);
                }
                catch (Exception)
                {
                    //Debug.WriteLine("AutoUpdateStarter:  Started app:  " + this.executablePath);
                    System.Windows.Forms.MessageBox.Show("程序运行错误，请重新安装程序！");
                    //m_log.Error("程序重启错误", ex);
                }
            }
		}
	}
}
