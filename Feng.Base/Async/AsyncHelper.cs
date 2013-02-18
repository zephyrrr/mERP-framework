using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Async
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncHelper
    {
        /// <summary>
        /// Start (simple)
        /// </summary>
        /// <param name="action"></param>
        public static AsyncHelper Start(Action action)
        {
            return Start(() =>
                {
                    action();
                    return null;
                });
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workDone"></param>
        public static AsyncHelper Start(DoWork doWork, WorkDone workDone = null)
        {
            return new AsyncHelper(doWork, workDone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate object DoWork();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public delegate void WorkDone(object result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressData"></param>
        public delegate void WorkProgress(object progressData);

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public AsyncHelper()
        //{
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workDone"></param>
        public AsyncHelper(DoWork doWork, WorkDone workDone)
        {
            StartInternal(doWork, workDone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workDone"></param>
        /// <param name="workProgress"></param>
        public AsyncHelper(DoWork doWork, WorkDone workDone, WorkProgress workProgress)
        {
            StartInternal(doWork, workDone, workProgress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressData"></param>
        public void Progress(object progressData)
        {
            m_worker.ProgressSignal(progressData);
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitForWorker()
        {
            var e = m_asyncManager.WaitForWorker();

            if (m_workDone != null)
            {
                m_workDone(e.Result);

                if (e.Exception != null)
                {
                    ExceptionProcess.ProcessWithNotify(e.Exception);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AbortWorker()
        {
            var e = m_asyncManager.AbortWorker();

            if (m_workDone != null)
            {
                m_workDone(e.Result);

                if (e.Exception != null)
                {
                    ExceptionProcess.ProcessWithNotify(e.Exception);
                }
            }
        }

        private AsyncManager<object, object> m_asyncManager;

        private AsyncHelperWorker m_worker;

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="workDone"></param>
        /// <param name="workProgress"></param>
        private void StartInternal(DoWork doWork, WorkDone workDone, WorkProgress workProgress = null)
        {
            m_worker = new AsyncHelperWorker(doWork);
            m_asyncManager = new AsyncManager<object, object>(Guid.NewGuid().ToString(), m_worker);

            m_workDone = workDone;
            m_asyncManager.WorkerDone += new EventHandler<WorkerDoneEventArgs<object>>(asyncManager_WorkerDone);

            if (workProgress != null)
            {
                m_workProgress = workProgress;
                m_asyncManager.WorkerProgress += new EventHandler<WorkerProgressEventArgs<object>>(m_asyncManager_WorkerProgress);
            }

            m_asyncManager.StartWorker(SystemConfiguration.UseMultiThread);
        }

        private WorkProgress m_workProgress;
        void m_asyncManager_WorkerProgress(object sender, WorkerProgressEventArgs<object> e)
        {
            if (m_workProgress != null)
            {
                m_workProgress(e.ProgressData);
            }
        }

        private WorkDone m_workDone;
        private void asyncManager_WorkerDone(object sender, WorkerDoneEventArgs<object> e)
        {
            if (m_workDone != null)
            {
                m_workDone(e.Result);

                if (e.Exception != null)
                {
                    ExceptionProcess.ProcessWithNotify(e.Exception);
                }
            }
        }

        private class AsyncHelperWorker : WorkerBase<object, object, object>
        {
            public AsyncHelperWorker(DoWork doWork)
                : base(null)
            {
                m_doWork = doWork;
            }

            private DoWork m_doWork;
            protected override object DoWork(object inputParams)
            {
                return m_doWork();
            }

            public void ProgressSignal(object progressData)
            {
                base.WorkerProgressSignal(progressData);
            }
        }
    }
}
