using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Grid
{
    public class GridDataGragEventArgs : EventArgs
    {
        public GridDataGragEventArgs()
        {
        }

        public DragDropEffects AllowedEffect { get; set; }

        public IDataObject Data { get; set; }
    }
}
