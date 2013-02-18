namespace Feng
{
    using System;
    using System.Security.Permissions;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CancelEventHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class CancelEventArgs : EventArgs
    {
        private bool cancel;
        /// <summary>
        /// 
        /// </summary>
        public CancelEventArgs() : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancel"></param>
        public CancelEventArgs(bool cancel)
        {
            this.cancel = cancel;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }
    }
}

