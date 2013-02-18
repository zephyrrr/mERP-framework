using System;

namespace Feng.Windows.Forms
{
    public interface IButton : IDisposable
    {
        bool Enabled
        {
            get;
            set;
        }

        bool Visible
        {
            get;
            set;
        }

        event EventHandler Click;

        object Tag
        {
            get;
            set;
        }
    }
}
