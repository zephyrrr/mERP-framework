using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class GridFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="info"></param>
        public static void CreateCellViewerManager(Xceed.Grid.Column column, GridColumnInfo info, IDisplayManager dm)
        {
            try
            {
                Xceed.Grid.Viewers.CellViewerManager viewer = ControlFactory.CreateCellViewerManager(dm.Name,
                    info.CellViewerManager, info.CellViewerManagerParam, GridColumnInfoHelper.CreateType(info));
                if (viewer != null)
                {
                    column.CellViewerManager = viewer;

                    System.Collections.IComparer comp = ControlFactory.CreateColumnDataComparer(
                        info.CellViewerManager, info.CellViewerManagerParam, GridColumnInfoHelper.CreateType(info));
                    if (comp != null)
                    {
                        column.DataComparer = comp;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GridColumnInfo of " + info.Name + " is Invalid when CreateCellViewerManager!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="info"></param>
        public static void CreateCellEditorManager(Xceed.Grid.Column column, GridColumnInfo info, IDisplayManager dm)
        {
            try
            {
                Xceed.Grid.Editors.CellEditorManager editor = ControlFactory.CreateCellEditorManager(dm.Name,
                    info.CellEditorManager, info.CellEditorManagerParam, GridColumnInfoHelper.CreateType(info), (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter), 
                    info.CellViewerManagerParam);
                if (editor != null)
                {
                    column.CellEditorManager = editor;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GridColumnInfo of " + info.Name + " is Invalid when CreateCellEditorManager!", ex);
            }
        }
    }
}
