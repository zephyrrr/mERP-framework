using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public static class ArchiveDetailFormExtention
    {
        public static void Close(this IArchiveDetailForm detailWindow, bool force)
        {
             if (force)
            {
                detailWindow.Close();
            }
            else
            {
                detailWindow.Hide();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IArchiveDetailFormWithDetailGrids : IArchiveDetailForm
    {
        /// <summary>
        /// 
        /// </summary>
        IList<Feng.Grid.IBoundGrid> DetailGrids
        {
            get;
        }
    }

    public interface IArchiveDetailForm : IChildMdiForm, IDisposable
    {
        //ArchiveSeeForm ParentArchiveForm
        //{
        //    get;
        //    set;
        //}

        //bool Visible
        //{
        //    get;
        //}

        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        void Hide();

        /// <summary>
        /// 
        /// </summary>
        void UpdateContent();

        //bool DoCancel();

        //bool DoDelete();

        //bool DoAdd();

        //bool DoEdit();
    }
}
