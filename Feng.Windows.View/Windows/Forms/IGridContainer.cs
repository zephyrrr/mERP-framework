using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Forms
{
    public interface IGridContainer
    {
        Feng.Grid.IGrid MasterGrid
        {
            get;
        }
    }
    public interface IGridNamesContainer
    {
        string[] GridNames
        {
            get;
        }
    }

    public interface IWindowNamesContainer
    {
        string[] WindowNames
        {
            get;
        }
    }
}
