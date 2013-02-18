using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// DisplayManager with BindingSource
    /// </summary>
    public interface IDisplayManagerBindingSource
    {
        /// <summary>
        /// BindingSource
        /// </summary>
        BindingSource BindingSource
        {
            get;
        }
    }
}
