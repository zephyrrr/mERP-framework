using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    public interface IWinFormApplication : IApplication
    {
        void InsertStatusItem(int pos, System.Windows.Forms.ToolStripItem item);

        void RemoveStatusItem(System.Windows.Forms.ToolStripItem item);

        void UpdateStatus(string status);

        void OnChildFormShow(MyChildForm childForm);

        MyChildForm ActiveChildMdiForm
        {
            get;
        }
    }
}
