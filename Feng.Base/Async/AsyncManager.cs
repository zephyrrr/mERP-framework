using System.Threading;
using System.ComponentModel;
using System;

// http://www.codeproject.com/KB/threads/MultiThreadingWrapper.aspx
namespace Feng.Async
{

    /// <summary> 
    /// Manager class, handling the execution of the worker in synchronous or asynchronous mode 
    /// </summary> 
    /// <typeparam name="TProgressType"> 
    /// Type of the object that will contain the progress data 
    /// </typeparam> 
    /// <typeparam name="TResultType"> 
    /// Type of the object that will contain the results of the worker 
    /// </typeparam> 
    /// <remarks></remarks> 
    public class AsyncManager<TProgressType, TResultType>
    {

        #region "Fields"

        /// <summary> 
        /// Holds the worker instance (instance of a class derived from Worker.WorkerBase) 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly IWorker _worker;

        /// <summary> 
        /// Holds the worker thread 
        /// </summary> 
        /// <remarks></remarks> 
        private Thread _workerThread;

        /// <summary> 
        /// Holds the worker identity 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly string _identity;

        /// <summary> 
        /// Flag that indicates if the worker runs in synchronous or asynchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        private bool _isAsynchonous;

        /// <summary> 
        /// Handle to the calling thread, used to call methods on the calling thread 
        /// </summary> 
        /// <remarks></remarks> 
        private AsyncOperation _callingThreadAsyncOp;

        /// <summary> 
        /// Flag that indicates if the WorkerDone event needs to be cancelled due to the fact that the results 
        /// will already be processed when waiting for the worker to end 
        /// </summary> 
        /// <remarks></remarks> 
        private bool _cancelWorkerDoneEvent;

        #endregion

        #region "Constructor"

        /// <summary> 
        /// Constructor that receives the required parameters to manage the worker's execution 
        /// </summary> 
        /// <param name="identity">Identity of the worker</param> 
        /// <param name="worker">Instance of a worker class derived from Worker.WorkerBase</param> 
        /// <remarks></remarks> 
        public AsyncManager(string identity, IWorker worker)
        {
            _worker = worker;
            _identity = identity;

            //Give a handle to ourselves to the worker 

            _worker.AsyncManagerObject = this;
        }

        #endregion

        #region "Events"

        /// <summary> 
        /// Event used to signal the end of the worker 
        /// </summary> 
        /// <remarks></remarks> 
        public event EventHandler<WorkerDoneEventArgs<TResultType>> WorkerDone;

        /// <summary> 
        /// Event used to signal a progression in the worker 
        /// </summary> 
        /// <remarks></remarks> 
        public event EventHandler<WorkerProgressEventArgs<TProgressType>> WorkerProgress;

        #endregion

        #region "Properties"

        /// <summary> 
        /// Handle to the calling thread, used to call methods on the calling thread 
        /// </summary> 
        /// <value>aCallingThreadAsyncOp</value> 
        /// <returns>The AsyncOperation associated to the calling thread</returns> 
        /// <remarks></remarks> 
        internal AsyncOperation CallingThreadAsyncOp
        {
            get { return _callingThreadAsyncOp; }
        }

        /// <summary> 
        /// Identity of the worker 
        /// </summary> 
        /// <value>aIdentity (String)</value> 
        /// <returns>Identity of the worker</returns> 
        /// <remarks></remarks> 
        public string Identity
        {
            get { return _identity; }
        }

        /// <summary> 
        /// Indicates if the worker is running 
        /// </summary> 
        /// <value>Boolean</value> 
        /// <returns>True if the worker is running, otherwise False</returns> 
        /// <remarks></remarks> 
        public bool IsWorkerBusy
        {
            get { return (_workerThread != null); }
        }
        #endregion

        #region "Subs and Functions"

        #region "Friend"

        /// <summary> 
        /// Used as SendOrPostCallback, called through CallingThreadAsyncOp 
        /// </summary> 
        /// <param name="state">Object containing the parameters to transfer to the strongly-typed overload</param> 
        /// <remarks></remarks> 
        internal void WorkerProgressInternalSignal(object state)
        {
            WorkerProgressInternalSignal((TProgressType)state);
        }

        /// <summary> 
        /// Used as SendOrPostCallback, called through CallingThreadAsyncOp 
        /// </summary> 
        /// <param name="state">Object containing the parameters to transfer to the strongly-typed overload</param> 
        /// <remarks></remarks> 
        internal void WorkerExceptionInternalSignal(object state)
        {
            WorkerExceptionInternalSignal((Exception)state);
        }

        /// <summary> 
        /// Used as SendOrPostCallback, called through CallingThreadAsyncOp 
        /// </summary> 
        /// <param name="state">Object containing the parameters to transfer to the strongly-typed overload</param> 
        /// <remarks></remarks> 
        internal void WorkerDoneInternalSignal(object state)
        {
            WorkerDoneInternalSignal((TResultType)state);
        }

        /// <summary> 
        /// Sub called by the worker to signal a progression 
        /// </summary> 
        /// <param name="progressData"> 
        /// Progression data (if any) 
        /// </param> 
        /// <remarks></remarks> 
        internal void WorkerProgressInternalSignal(TProgressType progressData)
        {
            //Prepare and raise the event for the owner to process 
            var e = new WorkerProgressEventArgs<TProgressType>(Identity, progressData);

            if (WorkerProgress != null)
            {
                WorkerProgress(this, e);
            }
        }

        /// <summary> 
        /// Sub called by the worker to signal an exception 
        /// </summary> 
        /// <param name="workerException"> 
        /// Exception 
        /// </param> 
        /// <remarks></remarks> 
        internal void WorkerExceptionInternalSignal(Exception workerException)
        {
            if (_isAsynchonous && (_workerThread != null))
            {
                _workerThread.Join();
                _workerThread = null;
            }

            //Check if the results/exception have already been processed (because the owner was waiting for the worker to end) 
            if (!_cancelWorkerDoneEvent)
            {
                //Prepare and raise the event for the owner to process 
                var e = new WorkerDoneEventArgs<TResultType>(Identity, workerException, _worker.InterruptionRequested);

                if (WorkerDone != null)
                {
                    WorkerDone(this, e);
                }
            }

            //If the worker was running in asynchronous mode, we also need to post the "complete" message 
            if (_isAsynchonous)
            {
                _callingThreadAsyncOp.PostOperationCompleted(DoNothing, null);
            }
        }

        /// <summary> 
        /// Sub called by the worker to signal the end of the work 
        /// </summary> 
        /// <param name="result"> 
        /// Worker result 
        /// </param> 
        /// <remarks></remarks> 
        internal void WorkerDoneInternalSignal(TResultType result)
        {
            if (_isAsynchonous && (_workerThread != null))
            {
                _workerThread.Join();
                _workerThread = null;
            }

            //Check if the results/exception have already been processed (because the owner was waiting for the worker to end) 
            if (!_cancelWorkerDoneEvent)
            {
                //Prepare and raise the event for the owner to process 
                var e = new WorkerDoneEventArgs<TResultType>(Identity, result, _worker.InterruptionRequested);

                if (WorkerDone != null)
                {
                    WorkerDone(this, e);
                }
            }

            //If the worker was running in asynchronous mode, we also need to post the "complete" message 
            if (_isAsynchonous)
            {
                _callingThreadAsyncOp.PostOperationCompleted(DoNothing, null);
            }
        }

        #endregion

        #region "Public"

        /// <summary> 
        /// Called from the owner to start the worker 
        /// </summary> 
        /// <param name="asynchronous"> 
        /// Specifies if the worker must run in asynchronous mode (True) or not (False) 
        /// </param> 
        /// <remarks></remarks> 
        public void StartWorker(bool asynchronous)
        {
            _isAsynchonous = asynchronous;
            _cancelWorkerDoneEvent = false;

            if (_isAsynchonous)
            {
                //Asynchronous mode - we need to create a thread and start the worker using this thread 
                _callingThreadAsyncOp = AsyncOperationManager.CreateOperation(null);
                _workerThread = new Thread(_worker.StartWorkerAsynchronous);
                _workerThread.IsBackground = true;
                _workerThread.Start();
            }
            else
            {
                //Synchronous mode - simply call the worker's start method 
                _worker.StartWorkerSynchronous();

            }
        }

        /// <summary> 
        /// Called from the owner to stop the worker 
        /// </summary> 
        /// <remarks></remarks> 
        public void StopWorker()
        {
            //Signal the worker to stop working 
            _worker.StopWorker();
        }

        /// <summary> 
        /// Called from the owner to wait for the worker to complete 
        /// </summary> 
        /// <remarks></remarks> 
        public WorkerDoneEventArgs<TResultType> WaitForWorker()
        {
            if (((_workerThread != null)) && _workerThread.IsAlive)
            {
                _workerThread.Join();

                _workerThread = null;
            }

            //Since the results (or exception) are returned through this function to be immediately processed by 
            //the owner waiting for the worker's completion, we cancel the WorkerDone event 
            _cancelWorkerDoneEvent = true;

            if (_worker.WorkerException == null)
            {
                return new WorkerDoneEventArgs<TResultType>(Identity, (TResultType)_worker.ResultObject, _worker.InterruptionRequested);
            }
            return new WorkerDoneEventArgs<TResultType>(Identity, _worker.WorkerException, _worker.InterruptionRequested);
        }

        /// <summary> 
        /// Called from the owner to stop and wait for the worker 
        /// </summary> 
        /// <remarks></remarks> 
        public WorkerDoneEventArgs<TResultType> StopWorkerAndWait()
        {
            WorkerDoneEventArgs<TResultType> result = null;

            if (((_workerThread != null)) && _workerThread.IsAlive)
            {
                StopWorker();

                result = WaitForWorker();
            }

            return result;
        }

        /// <summary>
        /// Abort
        /// </summary>
        /// <returns></returns>
        public WorkerDoneEventArgs<TResultType> AbortWorker()
        {
            WorkerDoneEventArgs<TResultType> result = new WorkerDoneEventArgs<TResultType>(Identity, null, true);

            if (((_workerThread != null)) && _workerThread.IsAlive)
            {
                _workerThread.Abort();
                //result = WaitForWorker();
                _workerThread = null;
            }
            return result;
        }
        #endregion

        #region "Private"

        /// <summary> 
        /// Called when doing the "PostOperationCompleted" on the CallingThreadAsyncOp object 
        /// </summary> 
        /// <param name="state">Nothing</param> 
        /// <remarks></remarks> 
        private static void DoNothing(object state)
        {
            //We do nothing - it is only required by the AsyncOperation object 
        }

        #endregion

        #endregion

    }
}