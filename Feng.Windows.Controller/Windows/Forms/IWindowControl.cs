using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// WinForm Control
    /// </summary>
    public interface IWindowControl : IDisposable
    {
        /// <summary>
        /// Inner Control
        /// </summary>
        System.Windows.Forms.Control Control { get; }
    }

    public interface IWindowControlBetween : IDisposable
    {
        System.Windows.Forms.Control Control1 { get; }

        System.Windows.Forms.Control Control2 { get; }
    }

    /// <summary>
    /// Windows DataControl
    /// </summary>
    public interface IWindowDataControl : IDataControl, IWindowControl
    {
    }
}