using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    public class EventProcessUtils
    {
        internal static Tuple<string, object> GetDataCellValue(string s2, Xceed.Grid.Cell cell)
        {
            string s1 = s2;
            // maybe there is '.'
            int idx = s1.IndexOf('_');
            string dataControlName, propertyName;
            if (idx == -1)
            {
                dataControlName = s1;
                propertyName = null;
            }
            else
            {
                dataControlName = s1.Substring(0, idx);
                propertyName = s1.Substring(idx + 1);
            }

            object o = null;
            if (cell.ParentRow.Cells[dataControlName] != null)
            {
                o = cell.ParentRow.Cells[dataControlName].Value;
            }
            else
            {
                // 如果未找到Cell，则找父级DataControl
                IDisplayManagerContainer form = cell.GridControl.FindForm() as IDisplayManagerContainer;
                if (form != null)
                {
                    IDataControl parentDc = form.DisplayManager.DataControls[dataControlName];
                    o = parentDc.SelectedDataValue;
                }
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyName = propertyName.Replace('_', '.');
                o = EntityScript.GetPropertyValue(o, propertyName);
            }

            //if (o == null)
            //{
            //    throw new ArgumentException("there is no column or datacontrol with name " + s1 + "!");
            //}

            return new Tuple<string, object>(dataControlName, o);
        }

        internal static Tuple<string, object> GetDataControlValue(string s2, IDisplayManager dm)
        {
            string s1 = s2;
            // maybe there is '.'
            // '.', ':' is invalid in sql
            int idx = s1.IndexOf('_');
            string dataControlName, propertyName;
            if (idx == -1)
            {
                dataControlName = s1;
                propertyName = null;
            }
            else
            {
                dataControlName = s1.Substring(0, idx);
                propertyName = s1.Substring(idx + 1);
            }

            if (dm.DataControls[dataControlName] == null)
            {
                throw new ArgumentException("there is no IDataControl with name " + dataControlName + "!");
            }
            object o = dm.DataControls[dataControlName].SelectedDataValue;
            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyName = propertyName.Replace('_', '.');
                o = EntityScript.GetPropertyValue(o, propertyName);
            }
            return new Tuple<string,object>(dataControlName, o);
        }

        internal static Tuple<string, object> GetEntityValue(string s2, IDisplayManager dm)
        {
            string s1 = s2;
            // maybe there is '.'
            // '.', ':' is invalid in sql
            int idx = s1.IndexOf('_');
            string dataControlName, propertyName;
            if (idx == -1)
            {
                dataControlName = s1;
                propertyName = null;
            }
            else
            {
                dataControlName = s1.Substring(0, idx);
                propertyName = s1.Substring(idx + 1);
            }


            object o = EntityScript.GetPropertyValue(dm.CurrentItem, dataControlName);
            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyName = propertyName.Replace('_', '.');
                o = EntityScript.GetPropertyValue(o, propertyName);
            }
            return new Tuple<string, object>(dataControlName, o);
        }

        /// <summary>
        /// 按照EventProcessInfo执行
        /// </summary>
        /// <param name="eventProcessInfos"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ExecuteEventProcess(IList<EventProcessInfo> eventProcessInfos, object sender, EventArgs e)
        {
            if (eventProcessInfos == null)
                return;
            foreach (EventProcessInfo i in eventProcessInfos)
            {
                ExecuteEventProcess(i, sender, e);
            }
        }

        /// <summary>
        /// 按照EventProcessInfo执行
        /// </summary>
        /// <param name="eventProcessInfo"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        private static void ExecuteEventProcess(EventProcessInfo eventProcessInfo, object sender, EventArgs e)
        {
            if (eventProcessInfo == null)
                return;

            if (!eventProcessInfo.IsActive)
            {
                return;
            }

            switch (eventProcessInfo.Type)
            {
                case EventProcessType.SelectedDataValueChanged:
                    {
                        SelectedDataValueChangedEventArgs e2 = e as SelectedDataValueChangedEventArgs;

                        string[] ss = eventProcessInfo.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in ss)
                        {
                            string[] sss = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (sss.Length >= 2)
                            {
                                if (sss[0].Trim() == e2.DataControlName)
                                {
                                    ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(sss[1].Trim()), sender, e);
                                }
                            }
                        }
                    }
                    break;

                case EventProcessType.ReloadNv:
                    {
                        SelectedDataValueChangedEventArgs e2 = e as SelectedDataValueChangedEventArgs;
                        if (e2 != null)
                        {
                            IDisplayManager dm = sender as IDisplayManager;

                            IDataControl dc = e2.Container as IDataControl;
                            if (dc != null)
                            {
                                ReloadNvFromDataControl(eventProcessInfo, dm, dc, e2.DataControlName);
                            }
                            else
                            {
                                Xceed.Grid.Cell cell = e2.Container as Xceed.Grid.Cell;
                                //if (cell == null)
                                //{
                                //    throw new ArgumentException("SelectedDataValueChangedEventArgs's Container type is invalid!");
                                //}
                                if (cell != null)
                                {
                                    ReloadNvFromGridCell(eventProcessInfo, dm, cell, e2.DataControlName);
                                }
                                else
                                {
                                    ReloadNvFromEntity(eventProcessInfo, sender, e2.DataControlName);
                                }
                            }
                        }    
                    }
                    break;
                case EventProcessType.Process:
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict["sender"] = sender;
                    dict["e"] = e;
                    ProcessInfoHelper.ExecuteProcess(ADInfoBll.Instance.GetProcessInfo(eventProcessInfo.ExecuteParam), dict);
                    break;
                default:
                    throw new ArgumentException("Invalid EventProcessInfo'Type of " + eventProcessInfo.Name + "!");
            }
        }

        private static void ReloadNvFromGridCell(EventProcessInfo eventProcessInfo, IDisplayManager dm, Xceed.Grid.Cell cell, string changedDataControlName)
        {
            string[] columns = eventProcessInfo.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in columns)
            {
                Xceed.Grid.Cell iCell = cell.ParentRow.Cells[s];
                if (iCell == null)
                {
                    continue;
                    //throw new ArgumentException("there is no column with name " + s + "!");
                }
                GridColumnInfo iInfo = iCell.ParentColumn.Tag as GridColumnInfo;

                switch (iInfo.CellEditorManager)
                {
                    case "Combo":
                    case "MultiCombo":
                    case "FreeCombo":
                        {
                            NameValueMapping nv = NameValueMappingCollection.Instance[iInfo.CellEditorManagerParam];
                            List<string> ls = new List<string>();
                            foreach (KeyValuePair<string, object> kvp in nv.Params)
                            {
                                ls.Add(kvp.Key.Substring(1));
                            }
                            if (!ls.Contains(changedDataControlName))
                                continue;

                            iCell.Value = null;
                            foreach (string key in ls)
                            {
                                object o = GetDataCellValue(key, iCell).Item2;
                                nv.Params["@" + key] = o == null ? System.DBNull.Value : o;
                            }
                            NameValueMappingCollection.Instance.Reload(dm.Name, iInfo.CellEditorManagerParam);

                            // when in grid, we can't use comboBox's DataTableChanged because combobox only created when in edit
                            System.Data.DataView dv = NameValueMappingCollection.Instance.GetDataSource(dm.Name, iInfo.CellEditorManagerParam, iInfo.CellEditorManagerParamFilter);
                            if (dv.Count == 1)
                            {
                                object toValue = dv[0][NameValueMappingCollection.Instance[iInfo.CellEditorManagerParam].ValueMember];
                                if (!Feng.Utils.ReflectionHelper.ObjectEquals(iCell.Value, toValue))
                                {
                                    dm.OnSelectedDataValueChanged(new SelectedDataValueChangedEventArgs(s, iCell));
                                }
                                iCell.Value = toValue;
                            }

                            iCell.ReadOnly = (dv.Count == 0);

                            nv.ResetParams();
                        }
                        break;
                    case "ObjectPicker":
                        {
                            // Todo: if (!ls.Contains(changedDataControlName))

                            iCell.Value = null;
                            Feng.Windows.Forms.MyObjectPicker op = (iCell.CellEditorManager as Feng.Grid.Editors.MyObjectPickerEditor).TemplateControl;
                            string exp = (string)ParamCreatorHelper.TryGetParam(iInfo.CellEditorManagerParamFilter);
                            exp = EntityHelper.ReplaceEntity(exp, new EntityHelper.GetReplaceValue(delegate(string paramName)
                            {
                                return GetDataCellValue(paramName, iCell).Item2;
                            }));
                            op.DropDownControl.DisplayManager.SearchManager.LoadData(SearchExpression.Parse(exp), null);
                        }
                        break;
                }
            }
        }
        private static void ReloadNvFromDataControl(EventProcessInfo eventProcessInfo, IDisplayManager dm, IDataControl dc, string changedDataControlName)
        {
            string[] columns = eventProcessInfo.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in columns)
            {
                IDataControl idc = dm.DataControls[s];
                if (idc == null)
                {
                    throw new ArgumentException("there is no IDataControl with name " + s + " in eventProcess's SelectedDataValueChanged!");
                }

                //object saveValue = idc.SelectedDataValue;
                GridColumnInfo iInfo = idc.Tag as GridColumnInfo;

                switch (iInfo.CellEditorManager)
                {
                    case "Combo":
                    case "MultiCombo":
                    case "FreeCombo":
                        {
                            // NameValueMapping
                            NameValueMapping nv = NameValueMappingCollection.Instance[iInfo.CellEditorManagerParam];

                            // will throw 集合已改变Exception
                            List<string> ls = new List<string>();
                            foreach (KeyValuePair<string, object> kvp in nv.Params)
                            {
                                ls.Add(kvp.Key.Substring(1));
                            }
                            if (!ls.Contains(changedDataControlName))
                                continue;

                            object savedValue = idc.SelectedDataValue;
                            idc.SelectedDataValue = null;
                            foreach (string key in ls)
                            {
                                object o = GetDataControlValue(key, dm).Item2;

                                nv.Params["@" + key] = o == null ? System.DBNull.Value : o;
                            }
                            NameValueMappingCollection.Instance.Reload(dm.Name, iInfo.CellEditorManagerParam);
                            nv.ResetParams();

                            idc.SelectedDataValue = savedValue;
                        }
                        break;
                    case "ObjectPicker":
                        {
                            object savedValue = idc.SelectedDataValue;
                            idc.SelectedDataValue = null;

                            Feng.Windows.Forms.MyObjectPicker op = (idc as Feng.Windows.Forms.IWindowControl).Control as Feng.Windows.Forms.MyObjectPicker;
                            string exp = (string)ParamCreatorHelper.TryGetParam(iInfo.CellEditorManagerParamFilter);
                            exp = EntityHelper.ReplaceEntity(exp, new EntityHelper.GetReplaceValue(delegate(string paramName)
                            {
                                return GetDataControlValue(paramName, dm).Item2;
                            }));
                            op.DropDownControl.DisplayManager.SearchManager.LoadData(SearchExpression.Parse(exp), null);

                            idc.SelectedDataValue = savedValue;
                        }
                        break;
                    default:
                        throw new ArgumentException("EventProcess's SelectedDataValueChanged CellEditorManagerType is not support!");
                }

                //idc.SelectedDataValue = saveValue;
            }
        }
        private static void ReloadNvFromEntity(EventProcessInfo eventProcessInfo, object sender, string changedDataControlName)
        {
            IDisplayManager dm;
            if (sender is IDisplayManager)
                dm = sender as IDisplayManager;
            else if (sender is ISearchManager)
                dm = (sender as ISearchManager).DisplayManager;
            else
                return;

            string[] columns = eventProcessInfo.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in columns)
            {
                string dsName = dm.Name;
                string nvName = s;

                string[] ss = s.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 1)
                {
                    nvName = ss[1];
                    dsName = ss[0];
                }
                
                NameValueMapping nv = NameValueMappingCollection.Instance[nvName];

                // will throw 集合已改变Exception
                List<string> ls = new List<string>();
                foreach (KeyValuePair<string, object> kvp in nv.Params)
                {
                    ls.Add(kvp.Key.Substring(1));
                }
                if (!ls.Contains(changedDataControlName))
                    continue;
                foreach (string key in ls)
                {
                    object o = GetEntityValue(key, dm).Item2;

                    nv.Params["@" + key] = o == null ? System.DBNull.Value : o;
                }
                NameValueMappingCollection.Instance.Reload(dsName, nvName);
                nv.ResetParams();
                
                //switch (iInfo.CellEditorManager)
                //{
                //    case "Combo":
                //    case "MultiCombo":
                //    case "FreeCombo":
                //        {
                            
                //        }
                //        break;
                //    case "ObjectPicker":
                //        {
                //            Feng.Windows.Forms.MyObjectPicker op = (idc as Feng.Windows.Forms.IWindowControl).Control as Feng.Windows.Forms.MyObjectPicker;
                //            string exp = (string)ParamCreatorHelper.TryGetParam(iInfo.CellEditorManagerParamFilter);
                //            exp = EntityHelper.ReplaceEntity(exp, new EntityHelper.GetReplaceValue(delegate(string paramName)
                //            {
                //                return GetEntityValue(paramName, dm).Second;
                //            }));
                //            op.DropDownControl.DisplayManager.SearchManager.LoadData(SearchExpression.Parse(exp), null);
                //        }
                //        break;
                //    default:
                //        throw new ArgumentException("EventProcess's SelectedDataValueChanged CellEditorManagerType is not support!");
                //}
            }
        }
    }
}
