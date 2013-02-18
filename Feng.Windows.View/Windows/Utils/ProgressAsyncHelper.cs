using System;
using System.Collections.Generic;
using System.Text;
using Feng.Async;

namespace Feng.Windows.Utils
{
    public class ProgressAsyncHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workDone"></param>
        public static AsyncHelper Start(AsyncHelper.DoWork doWork, AsyncHelper.WorkDone workDone, System.Windows.Forms.Form owner, string progressCaption)
        {
            var i = new ProgressAsyncHelper(doWork, workDone, owner, progressCaption);
            return i.m_asyncHelper;
        }

        private AsyncHelper m_asyncHelper;
        public ProgressAsyncHelper(AsyncHelper.DoWork doWork, AsyncHelper.WorkDone workDone, System.Windows.Forms.Form owner, string progressCaption)
        {
            m_progressForm = new Feng.Windows.Forms.ProgressForm();

            m_asyncHelper = AsyncHelper.Start(doWork, new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                {
                    this.m_progressForm.Stop();
                    this.m_progressForm.Close();
                    workDone(result);
                }));

            m_progressForm.ProgressStopped += new EventHandler((sender, e) =>
                {
                    m_asyncHelper.AbortWorker();
                });

            m_progressForm.Start(owner, progressCaption);
        }

        private Feng.Windows.Forms.ProgressForm m_progressForm;
    }
}
