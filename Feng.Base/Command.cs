using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExecutedEventArgs : EventArgs
    {
        private static ExecutedEventArgs s_empty = new ExecutedEventArgs(null);

        /// <summary>
        /// 
        /// </summary>
        public new static ExecutedEventArgs Empty
        {
            get { return s_empty; }
        }

        private object _parameter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public ExecutedEventArgs(object parameter)
        {
            this._parameter = parameter;
        }
        //protected override void InvokeEventHandler(Delegate genericHandler, object target);


        /// <summary>
        /// 
        /// </summary>
        public object Parameter 
        {
            get
            {
                return this._parameter;
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExecutedCommandHandler(object sender, ExecutedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        public Command(string commandName)
        {
            _Name = commandName;
        }

        private string _Name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object sender, ExecutedEventArgs e)
        {
            throw new NotSupportedException("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ICommand.Execute(object sender, ExecutedEventArgs e)
        {
            CommandManager.Execute(this, sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                throw new NotSupportedException("");
            }
            remove
            {
                throw new NotSupportedException("");
            }
        }
    }
}
