using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid;
using Feng.Windows.Forms;

namespace Feng.Windows.Utils
{

    /// <summary>
    /// 控件工厂（自动初始化数据源）
    /// 类中    	数据控件  		表格查看 		表格编辑  		查找
    /// Enum		Combo(Enum)		Null			Combo(Enum)		MultiComb(Enum)  包含集合	，例如信誉情况@客户备案 (现在是集合包含集合）
    /// String		Combo(String)		Combo(String)		Combo(String)		MultiComb(string) 包含集合	，例如委托人@备案(现在是集合包含集合）
    /// MultiEnum(s)	MultiCombo(enum:*) 	MultiCombo(enum:*)	MultiCombo(enum:*)	MultiComb(enum:*)  集合包含集合	，例如业务类型@客户备案
    /// MultiString(s)	MultiCombo(string)	MultiCombo(string)	MultiCombo(string)	MultiComb(string) 集合包含集合	，例如角色用途@客户备案
    /// 在视图中，1还有另外一种表现形式：
    /// int         Combo(enum:*)   Combo(enum:*)   Combo(enum:*)   Combo(enum:*)   包含集合， 例如信誉情况@人员信息视图 (现在是集合包含集合）
    /// </summary>
    public static class ControlFactory
    {
        public static void SetSearchControls(IDataValueControl dc, GridColumnInfo info, string smName)
        {

            //IWindowDataControl wdc = dc as IWindowDataControl;
            //if (wdc != null)
            //{
            //    InitDataControl(wdc, info, dmName, false);

            //    if (!string.IsNullOrEmpty(info.BackColor))
            //    {
            //        wdc.Control.BackColor = System.Drawing.Color.FromName(info.BackColor);
            //    }
            //    if (!string.IsNullOrEmpty(info.ForeColor))
            //    {
            //        wdc.Control.ForeColor = System.Drawing.Color.FromName(info.ForeColor);

            //        // textbox, when readonly=true, shold explit set backcolor to active forecolor
            //        wdc.Control.BackColor = wdc.Control.BackColor;
            //    }
            //    if (!string.IsNullOrEmpty(info.FontName) && info.FontSize.HasValue)
            //    {
            //        wdc.Control.Font = new System.Drawing.Font(info.FontName, info.FontSize.Value);
            //    }
            //}
            //control.Tag = info;
        }

        /// <summary>
        /// 设置DataControl属性（某些是直接在界面上的，而不是通过ControlFactory创建的，所以需要设置其属性）
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param>
        /// <param name="dmName"></param>
        /// <param name="asDataControl"></param>
        public static void SetDataControls(Control control, GridColumnInfo info, string dmName)
        {
            IDataControl dc = control as IDataControl;
            if (dc == null)
                return;

            dc.Caption = info.Caption;
            dc.PropertyName = info.PropertyName;
            dc.Navigator = info.Navigator;
            //dc.Index = info.SeqNo;
            dc.Name = info.GridColumnName;
            dc.ResultType = GridColumnInfoHelper.CreateType(info);
            dc.NotNull = Authority.AuthorizeByRule(info.NotNull);

            if (info.GridColumnType == GridColumnType.Normal)
                dc.ControlType = DataControlType.Normal;
            else if (info.GridColumnType == GridColumnType.ExpressionColumn)
                dc.ControlType = DataControlType.Expression;
            else if (info.GridColumnType == GridColumnType.ImageColumn)
                return;
            else if (info.GridColumnType == GridColumnType.SplitColumn)
                return;
            else if (info.GridColumnType == GridColumnType.StatColumn)
                dc.ControlType = DataControlType.Stat;
            else if (info.GridColumnType == GridColumnType.UnboundColumn)
                dc.ControlType = DataControlType.Unbound;
            else 
                dc.ControlType = DataControlType.Normal;

            dc.Visible = Authority.AuthorizeByRule(info.DataControlVisible);

            IWindowDataControl wdc = dc as IWindowDataControl;
            if (wdc != null)
            {
                InitDataControl(wdc, info, dmName, false);

                if (!string.IsNullOrEmpty(info.BackColor))
                {
                    wdc.Control.BackColor = System.Drawing.Color.FromName(info.BackColor);
                }
                if (!string.IsNullOrEmpty(info.ForeColor))
                {
                    wdc.Control.ForeColor = System.Drawing.Color.FromName(info.ForeColor);

                    // textbox, when readonly=true, shold explit set backcolor to active forecolor ???
                    //wdc.Control.BackColor = wdc.Control.BackColor;
                }
                if (!string.IsNullOrEmpty(info.FontName) && info.FontSize.HasValue)
                {
                    wdc.Control.Font = new System.Drawing.Font(info.FontName, info.FontSize.Value);
                }
            }
            control.Tag = info;
        }

        private static Control GetInnerDataControl(string dataControlType, GridColumnInfo info)
        {
            Control ic = null;
            switch (dataControlType)
            {
                case "Text":
                case "MyTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "MaskText":
                case "MyMaskText":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyMaskTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "MultiLine":
                case "MyMultilineTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyMultilineTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "MyMultilineTextBox2":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyMultilineTextBox2, Feng.Windows.Forms") as Control;
                    break;
                case "Integer":
                case "MyIntegerTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyIntegerTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "Long":
                case "MyLongTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyLongTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "Numeric":
                case "MyNumericTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyNumericTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "Currency":
                case "MyCurrencyTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyCurrencyTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "MyRadioButton":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyRadioButton, Feng.Windows.Forms") as Control;
                    break;
                case "Bool":
                case "MyCheckBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyCheckBox, Feng.Windows.Forms") as Control;
                    break;
                case "Bool?":
                case "MyThreeStateCheckbox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyThreeStateCheckbox, Feng.Windows.Forms") as Control;
                    break;
                case "DateTime":
                case "Time":
                case "MyDateTimeTextBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyDateTimeTextBox, Feng.Windows.Forms") as Control;
                    break;
                case "MyDateTimePicker":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyDateTimePicker, Feng.Windows.Forms") as Control;
                    break;
                case "Date":
                case "MyDatePicker":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyDatePicker, Feng.Windows.Forms") as Control;
                    break;
                case "MyMonthPicker":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyMonthPicker, Feng.Windows.Forms") as Control;
                    break;
                case "Combo":
                case "MyComboBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyComboBox, Feng.Grid") as Control;
                    break;
                case "FreeCombo":
                case "MyFreeComboBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyFreeComboBox, Feng.Grid") as Control;
                    break;
                case "MultiCombo":
                case "MyOptionPicker":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyOptionPicker, Feng.Grid") as Control;
                    break;
                case "Picture":
                case "MyPictureBox":
                    ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyPictureBox, Feng.Windows.Forms") as Control;
                    break;
                case "ObjectSelect":
                case "MyObjectSelectTextBox":
                    {
                        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyObjectSelectTextBox, Feng.Windows.Application") as Control;
                        MyObjectSelectTextBox tb = ic as MyObjectSelectTextBox;
                        InitObjectSelect(tb, info.CellEditorManagerParam, info.CellViewerManagerParam);
                    }
                    break;
                case "ObjectPicker":
                case "MyObjectPicker":
                    {
                        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyObjectPicker, Feng.Windows.Application") as Control;
                        MyObjectPicker tb = ic as MyObjectPicker;

                        InitObjectPicker(tb, info.CellEditorManagerParam, info.CellViewerManagerParam, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                    break;
                case "ObjectTextBox":
                case "MyObjectTextBox":
                    {
                        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyObjectTextBox, Feng.Windows.Application") as Control;
                        MyObjectTextBox tb = ic as MyObjectTextBox;
                        tb.ObjectType = GridColumnInfoHelper.CreateType(info);
                        tb.DisplayMember = info.CellViewerManagerParam;
                    }
                    break;
                case "File":
                case "MyFileBox":
                    {
                        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyFileBox, Feng.Windows.Forms") as Control;
                    }
                    break;
                case "StarRating":
                    {
                        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.Windows.Forms.MyImageRating, Feng.Windows.Forms") as Control;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid DataControlType of " + dataControlType);
            }
            return ic;
        }

        public static IDataControl GetDataControlWrapper(GridColumnInfo info, string dmName)
        {
            if (string.IsNullOrEmpty(info.DataControlType))
            {
                throw new ArgumentException("info " + info.GridColumnName + "'s DataControlType should not be null");
            }

            Control ic = GetInnerDataControl(info.DataControlType, info);
            DataControlWrapper control = new DataControlWrapper();

            //foreach (string s in ControlsName.DataControlsTypeName)
            //{
            //    if (s.LastIndexOf(info.DataControlType, StringComparison.OrdinalIgnoreCase) != -1)
            //    {
            //        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(s)) as Control;
            //        break;
            //    }
            //}

            control.Controls.Add(ic);
            control.Control = ic;

            SetDataControls(control, info, dmName);

            control.ResetLayout();

            return control;
        }

        /// <summary>
        /// 根据GridColumn信息获取DataControl
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dmName"></param>
        /// <returns></returns>
        public static IDataControl GetDataControl(GridColumnInfo info, string dmName)
        {
            if (string.IsNullOrEmpty(info.DataControlType))
            {
                throw new ArgumentException("info " + info.GridColumnName + "'s DataControlType should not be null");
            }

            Control ic = GetInnerDataControl(info.DataControlType, info);
            LabeledDataControl control = new LabeledDataControl();

            //foreach (string s in ControlsName.DataControlsTypeName)
            //{
            //    if (s.LastIndexOf(info.DataControlType, StringComparison.OrdinalIgnoreCase) != -1)
            //    {
            //        ic = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(s)) as Control;
            //        break;
            //    }
            //}

            MyLabel lbl = new MyLabel();
            lbl.Name = "lbl" + info.GridColumnName;
            lbl.Text = info.Caption;

            control.Controls.Add(lbl);
            control.Controls.Add(ic);
            control.Label = lbl;
            control.Control = ic;

            SetDataControls(control, info, dmName);

            control.ResetLayout();

            return control;
        }

        internal static void InitObjectSelect(MyObjectSelectTextBox tb, string windowId, string displayMamber)
        {
            tb.ButtonClick += new EventHandler(delegate(object sender, System.EventArgs e)
            {
                MyObjectSelectTextBox here = (sender as System.Windows.Forms.Control).Parent as MyObjectSelectTextBox;

                WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(windowId);
                if (windowInfo.WindowType != WindowType.Select)
                {
                    windowInfo.WindowType = WindowType.Select;
                }
                GeneratedArchiveCheckForm checkForm = ServiceProvider.GetService<IWindowFactory>().CreateWindow(windowInfo) as GeneratedArchiveCheckForm;
                if (checkForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (checkForm.SelectedEntites.Count > 1)
                    {
                        throw new InvalidUserOperationException("最多只能选择一项！");
                    }
                    if (here.Parent is IDataControl)
                    {
                        (here.Parent as IDataControl).SelectedDataValue = checkForm.SelectedEntites.Count == 0 ? null : checkForm.SelectedEntites[0];
                    }
                    else
                    {
                        here.SelectedDataValue = checkForm.SelectedEntites.Count == 0 ? null : checkForm.SelectedEntites[0];
                    }
                }
                checkForm.Dispose();
            });
            tb.DisplayMember = displayMamber;
        }

        internal static void InitObjectPicker(MyObjectPicker tb, string windowId, string displayMamber, string searchExpression)
        {
            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(windowId);
            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);
            if (tabInfos == null)
            {
                throw new ArgumentException("there is no windowTab with windowId of " + windowInfo.Name);
            }
            if (tabInfos.Count == 0)
            {
                throw new ArgumentException("There should be at least one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }
            if (tabInfos.Count > 1)
            {
                throw new ArgumentException("There should be at most one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }

            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);
            IDisplayManager dmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(tabInfos[0], smMaster);

            tb.DropDownControl.SetDisplayManager(dmMaster, tabInfos[0].GridName);
            tb.DisplayMember = displayMamber;
            tb.SearchExpressionParam = searchExpression;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="infos"></param>
        ///// <returns></returns>
        //public static IList<IDataControl> GetDataControls(IList<GridColumnInfo> infos)
        //{
        //    IList<IDataControl> ret = new List<IDataControl>();
        //    foreach (GridColumnInfo info in infos)
        //    {
        //        ret.Add(GetDataControl(info));
        //    }

        //    return ret;
        //}

        private static void InitDataControl(IWindowControlBetween wc, GridColumnInfo info, string dmName, bool asSearchControl)
        {
            if (asSearchControl)
            {
                string initParam = info.SearchControlInitParam;
                if (!string.IsNullOrEmpty(initParam))
                {
                    if (initParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wc.Control1, type, false, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, type, false, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                    else if (initParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wc.Control1, type, true, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, type, true, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                    else
                    {
                        ControlDataLoad.InitDataControl(wc.Control1, dmName, initParam, initParam, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, dmName, initParam, initParam, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                }
            }
            else
            {
                string initParam = info.CellEditorManagerParam;
                if (!string.IsNullOrEmpty(initParam))
                {
                    if (initParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wc.Control1, type, false, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, type, false, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                    else if (initParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wc.Control1, type, true, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, type, true, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                    else
                    {
                        ControlDataLoad.InitDataControl(wc.Control1, dmName, info.CellViewerManagerParam, info.CellEditorManagerParam, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                        ControlDataLoad.InitDataControl(wc.Control2, dmName, info.CellViewerManagerParam, info.CellEditorManagerParam, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                }
            }
        }

        private static void InitDataControl(IWindowControl wdc, GridColumnInfo info, string dmName, bool asSearchControl)
        {
            if (asSearchControl)
            {
                string initParam = info.SearchControlInitParam;
                if (!string.IsNullOrEmpty(initParam))
                {
                    if (initParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wdc.Control, type, false, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                    else if (initParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wdc.Control, type, true, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                    else
                    {
                        ControlDataLoad.InitDataControl(wdc.Control, dmName, initParam, initParam, (string)ParamCreatorHelper.TryGetParam(info.SearchControlInitParamFilter));
                    }
                }
            }
            else
            {
                 string initParam = info.CellEditorManagerParam;
                if (!string.IsNullOrEmpty(initParam))
                {
                    if (initParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wdc.Control, type, false, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                    else if (initParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal))
                    {
                        Type type = GridColumnInfoHelper.CreateTypeFromEnumInitParam(GridColumnInfoHelper.CreateType(info), initParam);
                        System.Diagnostics.Debug.Assert(type != null, info.GridColumnName + "'s ControlInitParam should be valid");
                        ControlDataLoad.InitDataControl(wdc.Control, type, true, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                    else
                    {
                        ControlDataLoad.InitDataControl(wdc.Control, dmName, info.CellViewerManagerParam,
                            info.CellEditorManagerParam, (string)ParamCreatorHelper.TryGetParam(info.CellEditorManagerParamFilter));
                    }
                }
            }
        }

        private static string[] SearchBetweenControls = new string[] { "Integer", "MyIntegerTextBox", "Long", "MyLongTextBox", "Numeric", "MyNumericTextBox", "Currency", "MyCurrencyTextBox", "DateTime", "Time", "MyDateTimeTextBox", "MyDateTimePicker", "Date", "MyDatePicker", "MyMonthPicker" };

        /// <summary>
        /// 根据GridColumn信息获取FindControl
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static ISearchControl GetSearchControl(GridColumnInfo info, ISearchManager sm)
        {
            try
            {
                if (string.IsNullOrEmpty(info.SearchControlType))
                {
                    throw new ArgumentException("info " + info.GridColumnName + "'s SearchControlType should not be null");
                }

                LabeledSearchControl control1 = null;
                LabeledSearchControlBetween control2 = null;
                Control ic1 = null, ic2 = null;

                if (info.SearchControlType == "Custom")
                {
                    ISearchControl cfc = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Windows.Forms.CustomSearchControl, Feng.Windows.Application")) as ISearchControl;

                    cfc.Caption = info.Caption;
                    cfc.Index = info.SeqNo;
                    cfc.Name = info.GridColumnName;
                    (cfc as CustomSearchControl).LoadFromDatabase(cfc.Name);
                    cfc.Visible = Authority.AuthorizeByRule(info.SearchControlVisible);
                    cfc.Tag = info;

                    InitSearchControl(cfc, info);
                    return cfc;
                }
                else
                {
                    ic1 = GetInnerDataControl(info.SearchControlType, info);
                    if (Array.IndexOf<string>(SearchBetweenControls, info.SearchControlType) != -1)
                    {
                        ic2 = GetInnerDataControl(info.SearchControlType, info);
                    }

                    if (info.SearchControlType == "MultiLine"
                        || info.SearchControlType == "MyMultilineTextBox")
                    {
                        (ic1 as Feng.Windows.Forms.MyMultilineTextBox).SplitWithComma = true;
                    }

                    // 区间搜索的时候，如果是日期搜索，区间上限为23：59：59
                    if (info.SearchControlType == "Date"
                        || info.SearchControlType == "MyDatePicker")
                    {
                        MyDatePicker dtp = ic2 as MyDatePicker;
                        dtp.IsReturnLastTime = true;
                    }
                }
                Label lbl, lbl1, lbl2;

                if (ic2 != null)
                {
                    control2 = new LabeledSearchControlBetween();
                }
                else
                {
                    control1 = new LabeledSearchControl();
                }


                if (control1 != null)
                {
                    lbl = new MyLabel();
                    lbl.Name = "lbl" + info.GridColumnName;
                    lbl.Text = info.Caption;

                    control1.Controls.Add(lbl);
                    control1.Controls.Add(ic1);
                    control1.Label = lbl;
                    control1.Control = ic1;

                    control1.Caption = info.Caption;
                    control1.PropertyName = info.PropertyName;
                    control1.Navigator = info.Navigator;
                    control1.Index = info.SeqNo;    // control2.TabIndex = info.SeqNo;
                    control1.Name = info.GridColumnName;
                    control1.ResultType = GridColumnInfoHelper.CreateType(info);

                    control1.Visible = Authority.AuthorizeByRule(info.SearchControlVisible);

                    control1.ResetLayout();

                    control1.Tag = info;

                    InitDataControl(control1, info, sm.Name, true);
                    InitSearchControl(control1, info);

                    return control1;
                }
                else
                {
                    lbl = new MyLabel();
                    lbl.Name = "lbl" + info.GridColumnName;
                    lbl.Text = info.Caption;
                    lbl1 = new Label();
                    lbl2 = new Label();
                    lbl1.Text = ">";
                    lbl2.Text = "<";

                    control2.Controls.Add(lbl);
                    control2.Controls.Add(lbl1);
                    control2.Controls.Add(lbl2);
                    control2.Controls.Add(ic1);
                    control2.Controls.Add(ic2);
                    control2.Label = lbl;
                    control2.Control1 = ic1;
                    control2.Control2 = ic2;

                    control2.Caption = info.Caption;
                    control2.PropertyName = info.PropertyName;
                    control2.Navigator = info.Navigator;
                    control2.Index = info.SeqNo;
                    control2.Name = info.GridColumnName;
                    control2.ResultType = GridColumnInfoHelper.CreateType(info);
                    control2.Visible = Authority.AuthorizeByRule(info.SearchControlVisible);
                    control2.Tag = info;

                    LabeledSearchControlBetween.ResetLayout(ic1, ic2, lbl, lbl1, lbl2);

                    InitDataControl(control2, info, sm.Name, true);
                    InitSearchControl(control2, info);

                    return control2;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GridColumnInfo of " + info.Name + " is invalid!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object GetControlDefaultValueByUser(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string[] ss = Feng.Utils.StringHelper.Split(str, ";", "\"", true, false); 
            foreach (string s in ss)
            {
                string[] sss = Feng.Utils.StringHelper.Split(s, '#');
                if (sss.Length == 1 || (sss.Length >= 2 && Authority.AuthorizeByRule(sss[1])))
                {
                    if (sss[0].StartsWith(c_pythonStatementHeader))
                    {
                        return Script.CalculateExpression(sss[0]);
                    }
                    else
                    {
                        return sss[0];
                    }
                }
            }
            return null;
        }
        private const string c_pythonStatementHeader = "#Python";

        private static void InitSearchControl(ISearchControl fc, GridColumnInfo info)
        {
            fc.AdditionalSearchExpression = info.SearchControlAdditionalSearchExpression;
            fc.SpecialPropertyName = info.SearchControlFullPropertyName;
            fc.SearchNullUseFull = info.SearchNullUseFull;

            object defaultValue = GetControlDefaultValueByUser(info.SearchControlDefaultValue);

            if (defaultValue != null)
            {
                try
                {
                    string defaultValueString = defaultValue.ToString();
                    if (defaultValueString.ToUpper().StartsWith("NOT"))
                    {
                        fc.IsNot = true;
                        defaultValueString = defaultValueString.Substring(3).Trim();
                    }

                    if (defaultValueString.ToUpper() == "NULL")
                    {
                        fc.IsNull = true;
                    }
                    else
                    {
                        System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        string[] values = defaultValueString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string v in values)
                        {
                            arr.Add(Feng.Utils.ConvertHelper.ChangeType(v, GridColumnInfoHelper.CreateType(info)));
                        }
                        fc.SelectedDataValues = arr;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }

            if (string.IsNullOrEmpty(info.SearchControlDefaultOrder))
            {
                fc.Order = null;
            }
            else
            {
                string order = info.SearchControlDefaultOrder.ToUpper();
                if (order == "ASC")
                {
                    fc.Order = true;
                }
                else if (order == "DESC")
                {
                    fc.Order = false;
                }
            }

            if (info.SearchControlType == "MyTextBox" || info.SearchControlType == "MyMultilineTextBox"
                || info.SearchControlType == "MyFreeComboBox")
            {
                fc.CanSelectFuzzySearch = true;
            }

            //  例如人员单位里的类型，保存的数据是以,分隔的多选项，必须模糊查找
            else if (/*info.SearchControlType == "MyOptionPicker" &&*/ info.DataControlType == "MyOptionPicker")
            {
                fc.CanSelectFuzzySearch = false;
                fc.UseFuzzySearch = true;
            }
        }

        /// <summary>
        /// 创建CellEditorManager
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="editorType"></param>
        /// <param name="editorParams"></param>
        /// <param name="columnType"></param>
        /// <param name="editorParamFilter"></param>
        /// <returns></returns>
        public static Xceed.Grid.Editors.CellEditorManager CreateCellEditorManager(string dsName, string editorType, string editorParams, Type columnType, string editorParamFilter, string viewerParams)
        {
            if (!string.IsNullOrEmpty(editorType))
            {
                switch (editorType)
                {
                    case "Combo":
                        if (string.IsNullOrEmpty(editorParams))
                        {
                            throw new ArgumentNullException("Combo editorParams of type " + editorType + " should not be null!");
                        }
                        return editorParams.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            GridDataLoad.GetGridComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), false, editorParamFilter) :
                                editorParams.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                                GridDataLoad.GetGridComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), true, editorParamFilter) :
                                GridDataLoad.GetGridComboEditor(dsName, editorParams, editorParamFilter);
                    case "FreeCombo":
                        if (string.IsNullOrEmpty(editorParams))
                        {
                            throw new ArgumentNullException("FreeCombo editorParams of type " + editorType + " should not be null!");
                        }
                        return editorParams.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            GridDataLoad.GetGridFreeComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), false, editorParamFilter) :
                                editorParams.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                                GridDataLoad.GetGridFreeComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), true, editorParamFilter) :
                                GridDataLoad.GetGridFreeComboEditor(dsName, editorParams, string.Empty);
                    case "MultiCombo":
                        if (string.IsNullOrEmpty(editorParams))
                        {
                            throw new ArgumentNullException("MultiCombo editorParams of type " + editorType + " should not be null!");
                        }
                        return editorParams.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            GridDataLoad.GetGridMultiComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), false, editorParamFilter) :
                                editorParams.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                                GridDataLoad.GetGridMultiComboEditor(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, editorParams), true) :
                                GridDataLoad.GetGridMultiComboEditor(dsName, editorParams, editorParamFilter);
                    case "Text":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyTextEditor() :
                            new Feng.Grid.Editors.MyTextEditor(editorParams);
                    case "MaskText":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyMaskTextEditor() :
                            new Feng.Grid.Editors.MyMaskTextEditor(editorParams);
                    case "MultiLine":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MultiLineEditor() :
                            new Feng.Grid.Editors.MultiLineEditor(editorParams);
                    case "Date":
                        return new Feng.Grid.Editors.DateEditor();
                    case "Time":
                    case "DateTime":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.DateTimeEditor("yyyy-MM-dd") :
                            new Feng.Grid.Editors.DateTimeEditor(editorParams);
                    case "Integer":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyIntegerEditor() :
                            new Feng.Grid.Editors.MyNumericEditor(NumericTextBoxType.Integer, editorParams);
                    case "Long":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyLongEditor() :
                            new Feng.Grid.Editors.MyNumericEditor(NumericTextBoxType.Long, editorParams);
                    case "Currency":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyCurrencyEditor() :
                            new Feng.Grid.Editors.MyNumericEditor(NumericTextBoxType.Currency, editorParams);
                    case "Numeric":
                        return string.IsNullOrEmpty(editorParams) ?
                            new Feng.Grid.Editors.MyNumericEditor() :
                            new Feng.Grid.Editors.MyNumericEditor(NumericTextBoxType.Number, editorParams);
                    case "ObjectPicker":
                        {
                            //return new Feng.Grid.Editors.MyObjectPickerEditor(editorParams, viewerParams, editorParamFilter);
                            Feng.Grid.Editors.MyObjectPickerEditor r = new Feng.Grid.Editors.MyObjectPickerEditor();
                            InitObjectPicker(r.TemplateControl, editorParams, viewerParams, editorParamFilter);
                            return r;
                        }
                    case "ObjectSelect":
                        {
                            Feng.Grid.Editors.MyObjectSelectEditor r = new Feng.Grid.Editors.MyObjectSelectEditor();
                            InitObjectSelect(r.TemplateControl, editorParams, viewerParams);

                            return r;
                        }
                    case "ObjectTextBox":
                        {
                            Feng.Grid.Editors.MyObjectTextEditor r = new Feng.Grid.Editors.MyObjectTextEditor();
                            r.TemplateControl.ObjectType = columnType;
                            r.TemplateControl.DisplayMember = viewerParams;
                            return r;
                        }
                    case "StarRating":
                        {
                            return new Feng.Grid.Editors.StarRatingEditor();
                        }
                    default:
                        return CreateDefaultCellEditorManager(columnType);
                        //throw new NotSupportedException("Invalid CellEditorManagerType of " + editorType + "!");
                }
            }
            return null;
        }

        public static System.Collections.IComparer CreateColumnDataComparer(string viewerType, string viewerParam, Type columnType)
        {
            if (!string.IsNullOrEmpty(viewerType))
            {
                switch (viewerType)
                {
                    case "Combo":
                        if (string.IsNullOrEmpty(viewerParam))
                        {
                            throw new ArgumentNullException("Combo viewerParams of type " + viewerType + " should not be null!");
                        }
                        return viewerParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            new Feng.Grid.Comparer.ComboComparer(NameValueMappingCollection.Instance.Add(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), false)) :
                                viewerParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                            new Feng.Grid.Comparer.ComboComparer(NameValueMappingCollection.Instance.Add(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), true)) :
                                new Feng.Grid.Comparer.ComboComparer(viewerParam);
                    case "MultiCombo":
                        if (string.IsNullOrEmpty(viewerParam))
                        {
                            throw new ArgumentNullException("MultiCombo viewerParams of type " + viewerType + " should not be null!");
                        }
                        return viewerParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            new Feng.Grid.Comparer.MultiComboComparer(NameValueMappingCollection.Instance.Add(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), false)) :
                                viewerParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                            new Feng.Grid.Comparer.MultiComboComparer(NameValueMappingCollection.Instance.Add(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), true)) :
                                new Feng.Grid.Comparer.MultiComboComparer(viewerParam);
                    case "IntMultiImage":
                        return new Feng.Grid.Comparer.IntMultiImageComparer();;
                    default:
                        return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 创建CellViewerManager
        /// </summary>
        /// <param name="dsName"></param>
        /// <param name="viewerType"></param>
        /// <param name="viewerParams"></param>
        /// <param name="columnType"></param>
        /// <param name="viewerParamsFilter"></param>
        /// <returns></returns>
        public static Xceed.Grid.Viewers.CellViewerManager CreateCellViewerManager(string dsName, string viewerType, string viewerParam, Type columnType)
        {
            if (!string.IsNullOrEmpty(viewerType))
            {
                switch (viewerType)
                {
                    case "Combo":
                        if (string.IsNullOrEmpty(viewerParam))
                        {
                            throw new ArgumentNullException("Combo viewerParams of type " + viewerType + " should not be null!");
                        }
                        return viewerParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            GridDataLoad.GetGridComboViewer(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), false) :
                                viewerParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                                GridDataLoad.GetGridComboViewer(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), true) :
                                GridDataLoad.GetGridComboViewer(dsName, viewerParam, string.Empty);
                    case "MultiCombo":
                        if (string.IsNullOrEmpty(viewerParam))
                        {
                            throw new ArgumentNullException("MultiCombo viewerParams of type " + viewerType + " should not be null!");
                        }
                        return viewerParam.StartsWith(GridColumnInfoHelper.StartWithBigEnum, StringComparison.Ordinal) ?
                            GridDataLoad.GetGridMultiComboViewer(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), false) :
                                viewerParam.StartsWith(GridColumnInfoHelper.StartWithLittleEnum, StringComparison.Ordinal) ?
                                GridDataLoad.GetGridMultiComboViewer(GridColumnInfoHelper.CreateTypeFromEnumInitParam(columnType, viewerParam), true) :
                                GridDataLoad.GetGridMultiComboViewer(dsName, viewerParam, string.Empty);
                    case "MultiLine":
                        return new Feng.Grid.Viewers.MultiLineViewer();
                    case "Numeric":
                        return string.IsNullOrEmpty(viewerParam) ?
                            new Feng.Grid.Viewers.NumericViewer() :
                            new Feng.Grid.Viewers.NumericViewer(viewerParam);
                    case "DateTime":
                        return string.IsNullOrEmpty(viewerParam) ?
                            new Feng.Grid.Viewers.DateViewer() :
                            new Feng.Grid.Viewers.DateViewer(viewerParam);
                    case "BoolImage":
                        {
                            if (string.IsNullOrEmpty(viewerParam))
                            {
                                return new Feng.Grid.Viewers.BoolImageViewer();
                            }
                            else
                            {
                                string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (ss.Length >= 2)
                                {
                                    return new Feng.Grid.Viewers.BoolImageViewer(ss[0].Trim(), Feng.Utils.ConvertHelper.ToBoolean(ss[1]));
                                }
                                else if (ss.Length == 1)
                                {
                                    return new Feng.Grid.Viewers.BoolImageViewer(ss[0].Trim(), null);
                                }
                                else
                                {
                                    return new Feng.Grid.Viewers.BoolImageViewer();
                                }
                            }
                        }
                    case "BoolText":
                        {
                            if (string.IsNullOrEmpty(viewerParam))
                            {
                                return new Feng.Grid.Viewers.BoolTextViewer();
                            }
                            else
                            {
                                string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (ss.Length >= 2)
                                {
                                    return new Feng.Grid.Viewers.BoolTextViewer(ss[0].Trim(),ss[1].Trim());
                                }
                                else
                                {
                                    return new Feng.Grid.Viewers.BoolTextViewer();
                                }
                            }
                        }
                    case "EnableText":
                        {
                            string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ss.Length == 5)
                            {
                                return new Feng.Grid.Viewers.EnableTextViewer(ss[0].Trim(), ss[1].Trim(), ss[2].Trim(), ss[3].Trim(), ss[4].Trim());
                            }
                            else if (ss.Length == 3)
                            {
                                return new Feng.Grid.Viewers.EnableTextViewer(ss[0].Trim(), ss[1].Trim(), ss[2].Trim());
                            }
                            else
                            {
                                return new Feng.Grid.Viewers.EnableTextViewer(ss[0].Trim());
                            }
                        }
                    case "IntImage":
                        return (string.IsNullOrEmpty(viewerParam) ? new Feng.Grid.Viewers.IntImageViewer() : new Feng.Grid.Viewers.IntImageViewer(viewerParam));
                    case "IntMultiImage":
                        {
                            if (string.IsNullOrEmpty(viewerParam))
                            {
                                return new Feng.Grid.Viewers.IntMultiImageViewer();
                            }
                            else
                            {
                                string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (ss.Length >= 2)
                                {
                                    return new Feng.Grid.Viewers.IntMultiImageViewer(ss[0].Trim(), ss[1].Trim());
                                }
                                else
                                {
                                    return new Feng.Grid.Viewers.IntMultiImageViewer();
                                }
                            }
                        }
                    case "IntImageText":
                        {
                            if (string.IsNullOrEmpty(viewerParam))
                            {
                                return new Feng.Grid.Viewers.IntImageTextViewer();
                            }
                            else
                            {
                                string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                return new Feng.Grid.Viewers.IntImageTextViewer(ss);
                            }
                        }
                    case "ImageText":
                        return new Feng.Grid.Viewers.ImageTextViewer(viewerParam);
                    case "StartStopAction":
                        {
                            if (string.IsNullOrEmpty(viewerParam))
                            {
                                return new Feng.Grid.Viewers.StartStopActionViewer();
                            }
                            else
                            {
                                string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (ss.Length >= 2)
                                {
                                    return new Feng.Grid.Viewers.StartStopActionViewer(ss[0].Trim(), ss[1].Trim());
                                }
                                else
                                {
                                    return new Feng.Grid.Viewers.StartStopActionViewer();
                                }
                            }
                        }
                    case "DetailSummaryRow":
                        return new Feng.Grid.Viewers.DetailSummaryRowViewer(viewerParam);
                    case "DetailGridStatistics":
                        return new Feng.Grid.Viewers.DetailGridStatisticsViewer(viewerParam);
                    case "Object":
                        return new Feng.Grid.Viewers.ObjectViewer(viewerParam);
                    case "MultiColorText":
                        if (string.IsNullOrEmpty(viewerParam))
                        {
                            return new Feng.Grid.Viewers.MultiColorTextViewer();
                        }
                        else
                        {
                            string[] ss = viewerParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            return new Feng.Grid.Viewers.MultiColorTextViewer(ss);
                        }
                    default:
                        return CreateDefaultCellViewerManager(columnType);
                        //throw new NotSupportedException("Invalid CellViewerManagerType of " + viewerType + "!");
                }
            }
            return null;
        }

        public static Xceed.Grid.Viewers.CellViewerManager CreateDefaultCellViewerManager(Type columnType)
        {
            if (columnType == typeof(DateTime) || columnType == typeof(DateTime?))
            {
                return new Feng.Grid.Viewers.DateViewer();
            }
            else if (columnType == typeof(decimal) || columnType == typeof(decimal?)
                || columnType == typeof(double) || columnType == typeof(double?))
            {
                return new Feng.Grid.Viewers.NumericViewer();
            }
            else
            {
                return null;
            }
        }

        public static Xceed.Grid.Editors.CellEditorManager CreateDefaultCellEditorManager(Type columnType)
        {
            if (columnType == typeof(DateTime) || columnType == typeof(DateTime?))
            {
                return new Feng.Grid.Editors.DateEditor();
            }
            else if (columnType == typeof(decimal) || columnType == typeof(decimal?)
                || columnType == typeof(double) || columnType == typeof(double?))
            {
                return new Feng.Grid.Editors.MyNumericEditor();
            }
            else if (columnType == typeof(string))
            {
                return new Feng.Grid.Editors.MyTextEditor();
            }
            else
            {
                return null;
            }
        }
    }
}
