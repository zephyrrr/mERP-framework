using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimpleGrid : IDisposable
    {
        /// <summary>
        /// Columns
        /// </summary>
        Xceed.Grid.Collections.ColumnList Columns
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.Collections.DataRowList DataRows
        {
            get;
        }

        /// <summary>
        /// Grid Width
        /// </summary>
        int Width
        {
            get;
        }
    }

    /// <summary>
    /// Interface for Grid(GridControl and DetailGrid)
    /// </summary>
    public interface IGrid : ISimpleGrid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.Form FindForm();

        /// <summary>
        /// GridHelper
        /// </summary>
        GridHelper GridHelper
        {
            get;
        }

        
        /// <summary>
        /// FixedHeaderRows
        /// </summary>
        Xceed.Grid.Collections.RowList FixedHeaderRows
        {
            get;
        }

        /// <summary>
        /// FixedHeaderRows
        /// </summary>
        Xceed.Grid.Collections.RowList FixedFooterRows
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.Collections.ReadOnlyGroupList Groups
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.Collections.GroupList GroupTemplates { get; }

        /// <summary>
        /// 
        /// </summary>
        void UpdateGrouping();

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.Collections.DetailGridList DetailGridTemplates { get; }

        /// <summary>
        /// 
        /// </summary>
        void UpdateDetailGrids();

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.DataRow DataRowTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Xceed.Grid.Row CurrentRow
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleSheet"></param>
        void ApplyStyleSheet(Xceed.Grid.StyleSheet styleSheet);

        /// <summary>
        /// 名称
        /// </summary>
        string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        void BeginInit();

        /// <summary>
        /// 
        /// </summary>
        void EndInit();

        /// <summary>
        /// Visible
        /// </summary>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool ReadOnly 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Main GridControl
        /// </summary>
        Xceed.Grid.GridControl GridControl
        {
            get;
        }
    }

    public interface IMasterGrid : IGrid
    {
    }
}
