using System;

namespace Feng.Async
{

    #region "Class BaseEventArgs"

    /// <summary> 
    /// Abstract base class for all defined events 
    /// </summary> 
    /// <remarks></remarks> 
    public abstract class BaseEventArgs : EventArgs
    {

        /// <summary> 
        /// ID of the worker that fires the event 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly string _identity;

        /// <summary> 
        /// ID of the worker that fires the event 
        /// </summary> 
        /// <value>aIdentity (String)</value> 
        /// <returns>Identity of the worker that fires the event</returns> 
        /// <remarks></remarks> 
        public string Identity
        {
            get { return _identity; }
        }

        /// <summary> 
        /// Base constructor 
        /// </summary> 
        /// <param name="identity">Identity of the worker that fires the event</param> 
        /// <remarks></remarks> 
        protected BaseEventArgs(string identity)
        {
            _identity = identity;
        }

    }

    #endregion

    #region "Class WorkerDoneEventArgs"

    /// <summary> 
    /// Arguments for the event that signals the end of the worker 
    /// </summary> 
    /// <typeparam name="TResultType"> 
    /// Type of the object that will contain the results of the worker 
    /// </typeparam> 
    /// <remarks></remarks> 
    public class WorkerDoneEventArgs<TResultType> : BaseEventArgs
    {

        /// <summary> 
        /// Worker result 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly TResultType _result;

        /// <summary> 
        /// Possible exception that has occurred in the worker 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly Exception _exception;

        /// <summary> 
        /// Flags that indicates if the worker has received an interruption request 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly bool _interrupted;

        /// <summary> 
        /// Worker result 
        /// </summary> 
        /// <value>aResult (TResultType)</value> 
        /// <returns>The worker results, or Nothing if an exception occured</returns> 
        /// <remarks></remarks> 
        public TResultType Result
        {
            get { return _result; }
        }

        /// <summary> 
        /// Possible exception that has occurred in the worker 
        /// </summary> 
        /// <value>aException</value> 
        /// <returns>Nothing if there was not exception, otherwise the exception</returns> 
        /// <remarks></remarks> 
        public Exception Exception
        {
            get { return _exception; }
        }

        /// <summary> 
        /// Flags that indicates if the worker has received an interruption request 
        /// </summary> 
        /// <value>aInterrupted (Boolean)</value> 
        /// <returns>True if the worker has received an interruption request, otherwise False</returns> 
        /// <remarks></remarks> 
        public bool Interrupted
        {
            get { return _interrupted; }
        }

        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="identity">Identify of the worker</param> 
        /// <param name="result">Worker result</param> 
        /// <param name="interrupted">Indicates if the worker received an interruption request</param> 
        /// <remarks></remarks> 
        public WorkerDoneEventArgs(string identity, TResultType result, bool interrupted)
            : base(identity)
        {
            _result = result;
            _interrupted = interrupted;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="exception"></param>
        /// <param name="interrupted"></param>
        public WorkerDoneEventArgs(string identity, Exception exception, bool interrupted)
            : base(identity)
        {
            _exception = exception;
            _interrupted = interrupted;
        }

    }

    #endregion

    #region "Class WorkerProgressEventArgs"

    /// <summary> 
    /// Arguments for the event that signals a progression in the worker 
    /// </summary> 
    /// <typeparam name="TProgressType"> 
    /// Type of the object that will contain the progress data 
    /// </typeparam> 
    /// <remarks></remarks> 
    public class WorkerProgressEventArgs<TProgressType> : BaseEventArgs
    {

        /// <summary> 
        /// Progress data 
        /// </summary> 
        /// <remarks></remarks> 
        private readonly TProgressType _progressData;

        /// <summary> 
        /// Progress data 
        /// </summary> 
        /// <value>aProgressData (TProgressType)</value> 
        /// <returns>The progress data coming from the worker</returns> 
        /// <remarks></remarks> 
        public TProgressType ProgressData
        {
            get { return _progressData; }
        }

        /// <summary> 
        /// Constructor 
        /// </summary> 
        /// <param name="identity">Identify of the worker</param> 
        /// <param name="progressData">Progress data</param> 
        /// <remarks></remarks> 

        public WorkerProgressEventArgs(string identity, TProgressType progressData)
            : base(identity)
        {

            if ((progressData != null))
            {
                _progressData = progressData;

            }
        }

    }

    #endregion

}