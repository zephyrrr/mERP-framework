using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 放在右下角执行的任务
    /// </summary>
    public abstract class ExecuteTask : IDisposable, IPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }

        private IWinFormApplication app;
        void IPlugin.OnLoad()
        {
            app = ServiceProvider.GetService<IApplication>() as IWinFormApplication;
            if (app != null)
            {
                m_tsbItem = new ToolStripLoadingCircle();
                m_tsbItem.LoadingCircleControl.AccessibleName = m_taskName;
                m_tsbItem.LoadingCircleControl.Active = false;
                m_tsbItem.LoadingCircleControl.Color = System.Drawing.Color.DarkGray;
                m_tsbItem.LoadingCircleControl.InnerCircleRadius = 8;
                m_tsbItem.LoadingCircleControl.Location = new System.Drawing.Point(1, 2);
                m_tsbItem.LoadingCircleControl.Name = "toolStripLoadingCircle1";
                m_tsbItem.LoadingCircleControl.NumberSpoke = 10;
                m_tsbItem.LoadingCircleControl.OuterCircleRadius = 10;
                m_tsbItem.LoadingCircleControl.RotationSpeed = 100;
                m_tsbItem.LoadingCircleControl.Size = new System.Drawing.Size(28, 20);
                m_tsbItem.LoadingCircleControl.SpokeThickness = 4;
                m_tsbItem.LoadingCircleControl.StylePreset = Feng.Windows.Forms.LoadingCircle.StylePresets.MacOSX;
                m_tsbItem.LoadingCircleControl.TabIndex = 1;
                m_tsbItem.LoadingCircleControl.Text = m_taskName;
                m_tsbItem.Name = "toolStripLoadingCircle1";
                m_tsbItem.Size = new System.Drawing.Size(28, 20);
                m_tsbItem.Text = m_taskName;
                m_tsbItem.ToolTipText = m_taskName;

                app.InsertStatusItem(-1, m_tsbItem);
                m_tsbItem.Text = m_taskName;
                m_tsbItem.DoubleClick += new EventHandler(loadingCircle_DoubleClick);
            }
        }

        void IPlugin.OnUnload()
        {
            Stop();

            if (app != null && m_tsbItem != null)
            {
                app.RemoveStatusItem(m_tsbItem);
            }
        }

        ToolStripLoadingCircle m_tsbItem;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskName"></param>
        public ExecuteTask(string taskName)
        {
            m_taskName = taskName;
        }

        void loadingCircle_DoubleClick(object sender, EventArgs e)
        {
            ToolStripLoadingCircle item = sender as ToolStripLoadingCircle;

            if (m_asyncHelper == null)
            {
                Start();
                
                item.LoadingCircleControl.Active = true;
            }
            else
            {
                Stop();
                item.LoadingCircleControl.Active = false;
            }
        }

        private string m_taskName;
        private string m_actionAddress;
        private Feng.Async.AsyncHelper m_asyncHelper;

        public abstract void DoWork();

        /// <summary>
        /// Start
        /// </summary>
        public virtual void Start()
        {
            if (m_asyncHelper == null)
            {
                m_asyncHelper = new Feng.Async.AsyncHelper(
                               new Feng.Async.AsyncHelper.DoWork(delegate()
                               {
                                   DoWork();
                                   return null;
                               }),
                               new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                               {

                               }),
                               new Feng.Async.AsyncHelper.WorkProgress(delegate(object result)
                               {
                                   string[] s = result as string[];
                                   if (s != null && s.Length == 2)
                                   {
                                       Notify(s[0], s[1]);
                                   }
                               }));
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public virtual void Stop()
        {
            if (m_asyncHelper != null)
            {
                m_asyncHelper.AbortWorker();
                m_asyncHelper = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressData"></param>
        public void Progress(object progressData)
        {
            m_asyncHelper.Progress(progressData);
        }

        /// <summary>
        /// Nofity
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionAddress"></param>
        public void Notify(string message, string actionAddress)
        {
            m_actionAddress = actionAddress;
            TaskbarNotifier taskbarNotifier = CreateTaskbarNotifier();

            if (taskbarNotifier.TaskbarState != TaskbarNotifier.TaskbarStates.hidden)
                taskbarNotifier.Hide();
            taskbarNotifier.Show(m_taskName, message, 500, 3000, 500);
        }

        protected virtual TaskbarNotifier CreateTaskbarNotifier()
        {
            TaskbarNotifier taskbarNotifier = new TaskbarNotifier();
            taskbarNotifier.SetBackgroundBitmap(Feng.Windows.ImageResource.Get("Feng", "Images.notifierSkin.png").Reference, Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(Feng.Windows.ImageResource.Get("Feng", "Images.notifierClose.png").Reference, Color.FromArgb(255, 0, 255), new Point(184, 9));
            taskbarNotifier.TitleRectangle = new Rectangle(10, 3, 180, 20);
            taskbarNotifier.ContentRectangle = new Rectangle(10, 22, 191, 80);
            //m_taskbarNotifier.TitleClick += new EventHandler(TitleClick);
            
            //m_taskbarNotifier.CloseClick += new EventHandler(CloseClick);
            taskbarNotifier.CloseClickable = true;
            taskbarNotifier.TitleClickable = false;
            taskbarNotifier.ContentClickable = true;
            taskbarNotifier.EnableSelectionRectangle = true;
            taskbarNotifier.KeepVisibleOnMousOver = true;
            taskbarNotifier.ReShowOnMouseOver = true;

            taskbarNotifier.ContentClick += new EventHandler(taskbarNotifier_ContentClick);
            return taskbarNotifier;
        }

        void taskbarNotifier_ContentClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_actionAddress))
            {
                ApplicationExtention.NavigateTo(ServiceProvider.GetService<IApplication>(), m_actionAddress);
            }
        }
    }
}
