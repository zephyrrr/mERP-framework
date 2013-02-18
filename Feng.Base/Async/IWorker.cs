using System;

namespace Feng.Async
{

    /// <summary> 
    /// Worker interface (without any generics) 
    /// </summary> 
    /// <remarks></remarks> 
    public interface IWorker
    {

        #region "Properties"

        /// <summary> 
        /// Used between AsyncManager and WorkerBase, to become AsyncManagerTyped 
        /// </summary> 
        /// <returns>The handle to the parent async manager</returns> 
        /// <remarks></remarks> 
        object AsyncManagerObject
        {
            get;
            set;
        }

        /// <summary> 
        /// Used by the derived worker class to check if an interruption request was received 
        /// </summary> 
        /// <returns>Flag that indicates if an interruption request was received</returns> 
        /// <remarks></remarks> 
        bool InterruptionRequested
        {
            get;
        }

        /// <summary> 
        /// Used to hold the results of the worker, as object 
        /// </summary> 
        /// <returns>Worker result</returns> 
        /// <remarks></remarks> 
        object ResultObject
        {
            get;
        }

        /// <summary> 
        /// Used to hold the exception (if any) that occurred in the worker 
        /// </summary> 
        /// <returns>Exception (if any) that occured in the worker</returns> 
        /// <remarks></remarks> 
        Exception WorkerException
        {
            get;
        }

        #endregion

        #region "Subs and Functions"

        /// <summary> 
        /// Called by the AsyncManager to start the worker in asynchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        void StartWorkerAsynchronous();

        /// <summary> 
        /// Called by the AsyncManager to start the worker in synchronous mode 
        /// </summary> 
        /// <remarks></remarks> 
        void StartWorkerSynchronous();

        /// <summary> 
        /// Called by the AsyncManager to stop the worker (interruption request) 
        /// </summary> 
        /// <remarks></remarks> 
        void StopWorker();

        #endregion

    }
}