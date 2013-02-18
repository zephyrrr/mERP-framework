using System.Threading;
using System;

namespace Feng.Async
{

    /// <summary> 
    /// Abstract base worker class from which to inherit to create its own worker class 
    /// </summary> 
    /// <typeparam name="TInputParamsType"> 
    /// Type of the object that will contain the input parameters required by the worker 
    /// </typeparam> 
    /// <typeparam name="TProgressType"> 
    /// Type of the object that will contain the progress data 
    /// </typeparam> 
    /// <typeparam name="TResultType"> 
    /// Type of the object that will contain the results of the worker 
    /// </typeparam> 
    /// <remarks></remarks> 
    public abstract class WorkerBase<TInputParamsType, TProgressType, TResultType> : IWorker
    {
        #region "Fields"

        /// <summary> 
        /// Holds the input parameters required by the worker 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly TInputParamsType _inputParams;

        /// <summary> 
        /// Indicates if an interruption request was received 
        /// </summary> 
        /// <remarks></remarks> 
        private bool _interruptionRequested;

        /// <summary> 
        /// Indicates if the worker is running in asynchronous or synchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        private bool _isAsynchronous;

        /// <summary> 
        /// SendOrPostCallback delegate to signal a progression in the worker 
        /// </summary> 
        /// <remarks></remarks> 
        private SendOrPostCallback _workerProgressInternalSignalCallback;

        /// <summary> 
        /// Used between AsyncManager and WorkerBase, to become AsyncManagerTyped 
        /// </summary> 
        /// <remarks></remarks> 
        private object _asyncManagerObject;

        /// <summary> 
        /// Allows access to the members of the parent AsyncManager instance 
        /// </summary> 
        /// <remarks></remarks> 
        private AsyncManager<TProgressType, TResultType> _asyncManagerTyped;

        /// <summary> 
        /// Holds the worker result 
        /// </summary> 
        /// <remarks></remarks> 
        private TResultType _result;

        /// <summary> 
        /// Holds the worker exception (if any occurred) 
        /// </summary> 
        /// <remarks></remarks> 
        private Exception _workerException;

        #endregion

        #region "Constructor"

        /// <summary> 
        /// Constructor receiving the input parameters required by the worker 
        /// </summary> 
        /// <param name="inputParams">Input parameters required by the worker</param> 
        /// <remarks></remarks> 
        protected WorkerBase(TInputParamsType inputParams)
        {
            _inputParams = inputParams;
        }

        #endregion

        #region "Properties"

        /// <summary> 
        /// Indicates if the worker is running in asynchronous or synchronous mode 
        /// </summary> 
        /// <value>aIsAsynchronous</value> 
        /// <returns>True if asynchronous, False if synchronous</returns> 
        /// <remarks></remarks> 
        public bool IsAsynchronous
        {
            get { return _isAsynchronous; }
        }

        #endregion

        #region "Properties from IAsyncTravailleurBase"

        /// <summary> 
        /// Used between AsyncManager and WorkerBase, to become AsyncManagerTyped 
        /// </summary> 
        /// <value>aAsyncManagerObject (Object)</value> 
        /// <returns>The handle to the parent AsyncManager instance</returns> 
        /// <remarks></remarks> 
        object IWorker.AsyncManagerObject
        {
            get { return _asyncManagerObject; }
            set { _asyncManagerObject = value; }
        }
        
        /// <summary> 
        /// Indicates if an interruption request was received 
        /// </summary> 
        /// <value>aInterruptionRequested (Boolean)</value> 
        /// <returns>True if an interruption request was received, otherwise False</returns> 
        /// <remarks></remarks> 
        bool IWorker.InterruptionRequested
        {
            get { return _interruptionRequested; }
        }

        /// <summary>
        /// Indicates if an interruption request was received 
        /// </summary>
        protected bool InterruptionRequested
        {
            get { return _interruptionRequested; }
        }

        /// <summary> 
        /// Used to hold the results of the worker, as object 
        /// </summary> 
        /// <value>aResult</value> 
        /// <returns>Le résultat du travail</returns> 
        /// <remarks></remarks> 
        object IWorker.ResultObject
        {
            get { return _result; }
        }

        /// <summary> 
        /// Used to hold the exception (if any) that occurred in the worker 
        /// </summary> 
        /// <value>aWorkerException</value> 
        /// <returns>Exception (if any) that occured in the worker</returns> 
        /// <remarks></remarks> 
        Exception IWorker.WorkerException
        {
            get { return _workerException; }
        }

        /// <summary>
        /// Used to hold the exception (if any) that occurred in the worker 
        /// </summary>
        public Exception WorkerException
        {
            get { return _workerException; }   
        }

        #endregion

        #region "Subs and Functions"

        #region "MustOverride"

        /// <summary> 
        /// Function to hold the worker code (to actually do the work!) 
        /// </summary> 
        /// <param name="inputParams">Paramètres d'entrée de type TTypeParametre</param> 
        /// <returns>Result (as TTypeResultat) of the worker</returns> 
        /// <remarks></remarks> 
        protected abstract TResultType DoWork(TInputParamsType inputParams);

        #endregion

        #region "Protected"

        /// <summary> 
        /// Called by the derived class to signal a progression in the work process 
        /// </summary> 
        /// <param name="progressData"> 
        /// Object (as TProgressType) containing the progression data 
        /// </param> 
        /// <remarks></remarks> 
        protected void WorkerProgressSignal(TProgressType progressData)
        {
            if (_isAsynchronous)
            {
                //We are in asynchronous mode - the call must be performed on the calling thread 
                _asyncManagerTyped.CallingThreadAsyncOp.Post(_workerProgressInternalSignalCallback, progressData);
            }
            else
            {
                //We are in synchronous mode - the call can be performed directly 
                _asyncManagerTyped.WorkerProgressInternalSignal(progressData);
            }
        }

        #endregion

        #endregion

        #region "Subs and Functions from IAsyncTravailleurBase"

        /// <summary> 
        /// Called from the AsyncManager to start the worker in asynchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        void IWorker.StartWorkerAsynchronous()
        {
            _isAsynchronous = true;
            _interruptionRequested = false;

            //We can strongly-type the AsyncManager parent object 
            _asyncManagerTyped = (AsyncManager<TProgressType, TResultType>)_asyncManagerObject;

            //Set the SendOrPostCallback delegate to signal progress 
            _workerProgressInternalSignalCallback = _asyncManagerTyped.WorkerProgressInternalSignal;

            //Set the SendOrPostCallback delegate to signal the normal end of the worker 
            var workerDoneInternalSignalCallback = new SendOrPostCallback(_asyncManagerTyped.WorkerDoneInternalSignal);

            //Set the SendOrPostCallback delegate to signal an exception that occurred in the worker 
            var workerExceptionInternalSignalCallback = new SendOrPostCallback(_asyncManagerTyped.WorkerExceptionInternalSignal);

            //We must catch all exceptions 
            try
            {
                //Do the actual work 
                _result = DoWork(_inputParams);

                //When the worker is done, we must signal the AsyncManager (on its own thread) 

                _asyncManagerTyped.CallingThreadAsyncOp.Post(workerDoneInternalSignalCallback, _result);
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                if (ex.InnerException is ThreadAbortException)
                {
                }
                else
                {
                    //When an exception occurs, we must signal the AsyncManager (on its own thread) 
                    _workerException = ex;

                    _asyncManagerTyped.CallingThreadAsyncOp.Post(workerExceptionInternalSignalCallback, _workerException);
                }
            }
        }

        /// <summary> 
        /// Called from the AsyncManager to start the worker in synchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        void IWorker.StartWorkerSynchronous()
        {
            _isAsynchronous = false;
            _interruptionRequested = false;

            //We can strongly-type the AsyncManager parent object 
            _asyncManagerTyped = (AsyncManager<TProgressType, TResultType>)_asyncManagerObject;

            //We must catch all exceptions 
            try
            {
                //Do the actual work 
                _result = DoWork(_inputParams);

                //When the worker is done, we must signal the AsyncManager (we're on the same thread) 
                _asyncManagerTyped.WorkerDoneInternalSignal(_result);
            }
            catch (Exception ex)
            {
                //When an exception occurs, we must signal the AsyncManager (we're on the same thread) 
                _workerException = ex;

                _asyncManagerTyped.WorkerExceptionInternalSignal(_workerException);
            }
        }

        /// <summary> 
        /// Called from the AsyncManager to stop the worker 
        /// </summary> 
        /// <remarks></remarks> 
        public void StopWorker()
        {
            _interruptionRequested = true;
        }

        #endregion

    }
}