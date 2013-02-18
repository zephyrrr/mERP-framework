using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 表格列类型
    /// </summary>
    public enum GridColumnType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 不生成Column，只用于查找
        /// </summary>
        NoColumn = 9,
        /// <summary>
        /// 打钩用于选择的Column
        /// </summary>
        CheckColumn = 1,
        /// <summary>
        /// 警示Column，会根据<see cref="GridColumnWarningInfo"/>计算值，然后再以CellViewManager（默认是图形方式）表现
        /// </summary>
        WarningColumn = 2,
        /// <summary>
        /// 用于统计的Column，必须有子DetailGrid[0].
        /// CellViewManagerParam为显示内容，格式为 SUM: 收款金额 where = "$%费用项%="101"$"， where里面是表达式
        /// </summary>
        StatColumn = 3,
        /// <summary>
        /// 表达式（可以为Python Expression、Statement或者是Python文件(.py结尾)
        /// </summary>
        ExpressionColumn = 4,
        /// <summary>
        /// 图像
        /// </summary>
        ImageColumn = 5,
        /// <summary>
        /// 空白列，用于分隔
        /// </summary>
        SplitColumn = 6,
        /// <summary>
        /// 不和数据绑定的Column，空白内容
        /// </summary>
        UnboundColumn = 7,
        /// <summary>
        /// 序号
        /// </summary>
        IndexColumn = 8
    }

    /// <summary>
    /// 表格列定义数据，用于<see cref="T:Grid.DataBoundGrid"/>和<see cref="T:Grid.DataUnboundGrid"/>
    /// 关于表格定义的信息，有<see cref="GridInfo"/>、<see cref="GridColumnInfo"/>、<see cref="GridRowInfo"/>和<see cref="GridCellInfo"/>
    /// 表格定义的数据，也用于设置窗体上的<see cref="Feng.IDataControl"/>控件属性。
    /// 如果是和Entity无关的Column，可以设置PropertyName = Null，但此时必须设置GridColumnName
    /// </summary>
    [Class(0, Name = "Feng.GridColumnInfo", Table = "AD_Grid_Column", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridColumnInfo : BaseADEntity
    {
        #region "Common"
        /// <summary>
        /// 表格名，和<see cref="WindowTabInfo.GridName"/>对应
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }
        #endregion

        #region "Entity"
        /// <summary>
        /// 此表格列要显示的实体类属性路径，和<see cref="PropertyName"/>配合。
        /// 例如，要取得员工姓名，在任务备案中，PropertyName = "姓名", Navigator = "员工"。
        /// 查找表达式<see cref="Feng.SearchExpression"/>中可写成"员工:姓名"
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string Navigator
        {
            get;
            set;
        }

        /// <summary>
        /// 此表格列要显示的实体类属性名称。
        /// </summary>
        [Property(Length = 4000, NotNull = false)]
        public virtual string PropertyName
        {
            get;
            set;
        }


        /// <summary>
        /// 内部数值类型(Column.DateType)
        /// 目前支持
        /// <list type="bullet">
        /// <item>System.String</item>
        /// <item>System.DateTime</item>
        /// <item>System.Int32</item>
        /// <item>System.Decimal</item>
        /// <item>System.Double</item>
        /// <item>System.....</item>
        /// <item>其他内部自定义类型，例如enum(jkhd2.Model.是否标志,jkhd2.Model)</item>
        /// </list>
        /// </summary>
        [Property(Column = "Type", Length = 100, NotNull = true)]
        public virtual string TypeName
        {
            get;
            set;
        }

        ///// <summary>
        ///// 根据<see cref="TypeName"/>创建类型
        ///// </summary>
        //public virtual Type CreateType()
        //{
        //    return ReflectionHelper.GetTypeFromName(TypeName);
        //}
        #endregion

        #region "Grid & DataControls"
        /// <summary>
        /// 标题(Column.Title) and (Label.Text)
        /// 如为空，默认用PropertyName
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// 多栏标题行，如为空则不用
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string GroupCaption
        {
            get;
            set;
        }

        /// <summary>
        /// 显示顺序(Column.VisibleIndex) and (IDataControl.Index)
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }

        /// <summary>
        /// 是否只读(Column.ReadOnly) and (IDataControl.ReadOnly)。
        /// 格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行是否显示(Column.Visible)。格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ColumnVisible
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行内数据是否要求非空的表达式。格式见<see cref="T:Feng.Authority"/>
        /// 此属性只针对新增操作。如需针对编辑操作设置，则需要在<see cref="GridCellInfo"/>中设置
        ///  配合<see cref="ValidRegularExpression"/>做检查
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string NotNull
        {
            get;
            set;
        }

        /// <summary>
        /// 新增操作时检查的正则表达式。是否为空在<see cref="NotNull"/>中设置，如果有值了，值的格式在这里设置。
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ValidRegularExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 没通过正则表达式检查<see cref="ValidRegularExpression"/>时显示的错误信息
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ValidErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 新增操作时检查的Script
        /// </summary>
        [Property(Length = 4000, NotNull = false)]
        public virtual string ValidScript
        {
            get;
            set;
        }
        #endregion

        #region "only to grid"
        /// <summary>
        /// 表格列的类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual GridColumnType GridColumnType
        {
            get;
            set;
        }

        /// <summary>
        /// 表格列名称（Column.Name）。如为空，用<see cref="DefaultGridColumnName"/>。
        /// 但某些时候，会出现2列的<see cref="DefaultGridColumnName"/>相同（相同值但不同显示方式），所以需要手工指定表格列名称
        /// </summary>
        [Property(Column = "GridColumnName", Length = 100, NotNull = false)]
        public virtual string SpecialGridColumnName
        {
            get;
            set;
        }

        private string m_gridColumnName = null;
        /// <summary>
        /// 经过计算的表格列名称。
        /// 如果<see cref="SpecialGridColumnName"/>不为空，则为<see cref="SpecialGridColumnName"/>；
        /// 否则为<see cref="DefaultGridColumnName"/>。如果<see cref="FullPropertyName"/>也为空，则为<see cref="Caption"/>
        /// </summary>
        public virtual string GridColumnName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_gridColumnName))
                    return m_gridColumnName;

                if (!string.IsNullOrEmpty(SpecialGridColumnName))
                {
                    m_gridColumnName = SpecialGridColumnName;
                }
                else
                {
                    var s = (string.IsNullOrEmpty(Navigator) ? PropertyName : Navigator + "." + PropertyName);
                    if (!string.IsNullOrEmpty(s))
                    {
                        m_gridColumnName = s;
                    }
                    else
                    {
                        m_gridColumnName = Caption;
                    }
                }

                m_gridColumnName = Feng.Utils.StringHelper.NormalizePropertyName(m_gridColumnName);
                return m_gridColumnName;
            }
        }

        /// <summary>
        /// Cell的显示控件。如需参数，在<see cref="CellViewerManagerParam"/>中指定。
        /// 目前支持类型有
        /// <list type="bullet">
        /// <item>Combo: 内部单个值，显示值含义描述。例如，内部值900001，显示"客户1"。必须要参数，
        /// 参数可为内部自定义缓存名称(对应数据表，in GlobalData.cs)或者Enum（对应Enum类型）</item>
        /// <item>MultiCombo: 内部多个值，显示值含义描述。例如，内部值a,f,g, 显示"报关员，报检员，查验员"。
        /// 必须要参数，参数含义同Combo</item>
        /// <item>MultiLine: 多行。如有多行文字，最后显示"..."，无参数</item>
        /// <item>Numeric: 货币，数值。并可设置显示格式。参数可选，为2位字符，第一位为符号，第二位为小数点位数，
        /// 默认为" 2",即前面不显示币值符号，2位小数</item>
        /// <item>DateTime: 时间，可设置显示格式。参数可选，格式为"yyyy-MM-dd hh:mm:ss"类似格式，默认为"yyyy-MM-dd"</item>
        /// <item>Image: 图像，用于给int显示。参数中为图像资源名称</item>
        /// <item>MultiImage: 多个图像，用于显示以','分开的int。参数中是以','分开的图像资源名称。显示的int为几就是显示第几个图像</item>
        /// <item>DetailSummaryRow: DetailGrid中的SummaryRow中的某个数据显示在ParentRow中。可用DetailGridStatistics替代</item>
        /// <item>DetailGridStatistics: DetailGrid中的数据统计。格式为 "函数名: 表格列名 level=? format=? where=?"， 例如"SUM: 金额 format=n2 where="收付标志=收"</item>
        /// </list>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string CellViewerManager
        {
            get;
            set;
        }


        /// <summary>
        /// CellViewerManager的参数,具体设置见<see cref="CellViewerManager"/>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string CellViewerManagerParam
        {
            get;
            set;
        }


        /// <summary>
        /// Cell的编辑控件。如需参数，在<see cref="CellEditorManagerParam"/>中指定。
        /// 目前支持类型有
        /// <list type="bullet">
        /// <item>Combo: 内部单个值，显示值含义描述。例如，内部值900001，显示"客户1"。必须要参数，
        /// 参数可为内部自定义缓存名称(对应数据表，in GlobalData.cs)或者Enum（对应Enum类型）</item>
        /// <item>FreeCombo: 可选可自由编辑，内部值和显示值相同，只是提供备选可选。
        /// 必须要参数，参数含义同Combo</item>
        /// <item>MultiCombo: 内部多个值，显示值含义描述。例如，内部值a,f,g, 显示"报关员，报检员，查验员"。
        /// 必须要参数，参数含义同Combo</item>
        /// <item>Text: 单行文本。参数为Upper, Lower, Normal之一，控制CharacterCasing</item>
        /// <item>MultiLine: 多行文本。无参数</item>
        /// <item>Date: 日期，无参数，下拉显示日历</item>
        /// <item>Time: 时间，用文本方式输入时间，格式固定。
        /// 必须要参数，格式为"yyyy-MM-dd hh:mm:ss"类似格式。</item>
        /// <item>Numeric: 货币，数值。无参数，下拉显示计算器</item>
        /// </list>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string CellEditorManager
        {
            get;
            set;
        }


        /// <summary>
        /// CellEditorManager的参数,具体设置见<see cref="CellEditorManager"/>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string CellEditorManagerParam
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许执行右键操作"默认本列"
        /// 如果为空，不允许；为True，允许，但如果原来有值，会提示；False，不提示直接更改
        /// </summary>
        [Property(NotNull = false)]
        public virtual bool? AllowSetList
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许右键复制
        /// </summary>
        [Property(NotNull = false)]
        public virtual bool? EnableCopy
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许右键选择全部
        /// </summary>
        [Property(NotNull = false)]
        public virtual bool? EnableSelectAll
        {
            get;
            set;
        }

        /// <summary>
        /// 统计函数，用于在表格末尾SummaryRow显示统计值。同时，可在右侧显示其他信息，见<see cref="StatTitle"/>
        /// 目前支持
        /// <list type="bullet">
        /// <item>SUM: 总计</item>
        /// <item>COUNT: 个数</item>
        /// </list>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string StatFunction
        {
            get;
            set;
        }


        /// <summary>
        /// 统计函数标题，可自定义。
        /// <para>
        /// The following table provides a list of the supported variables and their definitions.
        ///Note: Variables must be preceded and followed by a % symbol. See below for a usage example. 
        ///Supported variables Descriptions 
        ///%GROUPBYCOLUMNFIELDNAME% The Column.FieldName of the column as defined by the parent group's Group.GroupBy property. 
        ///%GROUPBYCOLUMNTITLE% The Title of the column represented by the parent group's Group.GroupBy property. 
        ///%GROUPKEY% The Key of the parent group. 
        ///%GROUPTITLE% The GroupBase.Title of the parent group. 
        ///%DATAROWCOUNT% The number of datarows in the parent group. 
        ///%STATCOLUMNFIELDNAME% The Column.FieldName of the column as defined by the SummaryCell.StatFieldName property. 
        ///%STATCOLUMNTITLE% The Title of the column represented by the SummaryCell.StatFieldName property. 
        ///%COUNT: (see below for usage)% The result of the Count statistical function. 
        ///%MAX: (see below for usage)% The result of the Maximum statistical function. 
        ///%MIN: (see below for usage)% The result of the Minimum statistical function. 
        ///%SUM: (see below for usage)% The result of the Sum statistical function. 
        ///%AVG: (see below for usage)% The result of the Average statistical function. 
        ///%STDEV: (see below for usage)% The result of the Standard Deviation statistical function. 
        ///%STDEVP: (see below for usage)% The result of the Standard Deviation Population statistical function. 
        ///%VAR: (see below for usage)% The result of the Variance statistical function. 
        ///%VARP: (see below for usage)% The result of the Variance Population statistical function. 
        ///%MEDIAN: (see below for usage)% The result of the Median statistical function. 
        ///%MODE: (see below for usage)% The result of the Mode statistical function. 
        ///%GEOMEAN: (see below for usage)% The result of the Geometric Mean statistical function. 
        ///%HARMEAN: (see below for usage)% The result of the Harmonic Mean statistical function. 
        ///%RMS: (see below for usage)% The result of the Root Mean Square statistical function. 
        ///In the case of the variables that represent StatFunctions (COUNT, MAX, MIN, SUM, AVG, STDEV, STDEVP, VAR, VARP, MEDIAN, MODE, GEOMEAN, HARMEAN, and RMS), additional parameters must be provided within the variable after the colon (:). These parameters are the fieldname of the column from which the data is retrieved, the (optional) format specifier with which the result of the statistical function is displayed, and the (optional) group level for which the statistical function is calculated.
        ///%COUNT: ["]stat_fieldname["] [ format=["]format_specifier["] ] [ level=running_stat_group_level ]%
        ///All items placed in square brackets [] are optional. If the stat_fieldname and/or format_specifier parameters contain spaces, quotes must be used. If a running_stat_group_level is not specified, -1 (current group) is assumed. Example 1 below demonstrates how to set the TitleFormat property using statistical function variables.
        ///The format of a mathematical function depends on 2 things: the function itself and the datatype of the column whose values are used for the statistical function. 
        ///The following table provides a list of supported and unsupported format specifiers relative to the datatype of the column whose values are used for the statistical function. 
        ///Statistical function Column datatype Datatype sent to the format specifier Unsupported format specifiers 
        ///COUNT Any Int64 R 
        ///MAX, MIN, MODE Anything that implements the IComparable interface. Always the same as the datatype of the column specified by the SummaryCell.StatFieldName property.  
        ///SUM Integral type* Int64 R 
        ///Floating point types** Double D and X 
        ///Decimal Decimal D, X and R 
        ///AVG, VAR, VARP, HARMEAN Integral* and Floating point** types Double D and X 
        ///Decimal Decimal D, X and R 
        ///STDEV, STDEVP, GEOMEAN, RMS Integral*, Floating point**, and Decimal types Double D and X 
        ///MEDIAN Integral type* (even item count) Double D and X 
        ///Integral type* (odd item count) Int64 R 
        ///Floating point types** Double D and X 
        ///Decimal Decimal D, X and R 
        ///* Integral types : Byte, SByte, Char, Int16, UInt16, Int32, UInt32, Int64, UInt64
        ///** Floating point types: Single and Double
        ///The values are rounded in the same manner as the string.Format round, meaning a banker's round.
        ///Hint: You can use F0 instead of D. It will have the same behavior and will work for either integral types or floating point types. 
        ///Example 2 below demonstrates how quotes can be used to specify literals in the format_specifier. 
        ///Example
        ///Example 1 demonstrates how to set the TitleFormat property using statistical function variables.
        ///Example 2 results in "val: 9.00" without the quotes, for the value "9". The internal quotes are necessary when the format_specifier contains a label (i.e., "val: ").
        ///Example 1VisualBasic Copy Code 
        ///summaryCell.TitleFormat = "A minimum of %MIN:Quantity% units were ordered at an average price of %AVG:UnitPrice format=c%." 
        ///C# Copy Code 
        ///summaryCell.TitleFormat = "A minimum of %MIN:Quantity% units were ordered at an average price of %AVG:UnitPrice format=c%."; 
        ///C# Copy Code 
        ///summaryCell.TitleFormat = "%MIN:Quantity format=\"\"val: \"0.00\"%"; 
        ///</para>
        /// 加入功能： 支持Expression。 例如 %SUM expression="$iif[#使用年限#=40,#购入金额#,0]$"%， %COUNT expression=""%.
        /// 注意，此时expression中不能有%，有%的用#代替
        ///</summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string StatTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 背景色
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// 前景色
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// 字体
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string FontName
        {
            get;
            set;
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        [Property(NotNull = false)]
        public virtual float? FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// HorizontalAlignment
        /// </summary>
        [Property(NotNull = false)]
        public virtual string HorizontalAlignment
        {
            get;
            set;
        }

        /// <summary>
        /// 默认排序
        /// </summary>
        [Property(NotNull = false)]
        public virtual string SortDirection
        {
            get;
            set;
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [Property(NotNull = false)]
        public virtual int? ColumnMaxWidth
        {
            get;
            set;
        }
        /// <summary>
        /// 默认Fixed
        /// </summary>
        [Property(NotNull = false)]
        public virtual bool? ColumnFixed
        {
            get;
            set;
        }
        #endregion

        #region "only to MasterDetail(DataUnboundWithDetailGridLoadonce) Grid"

        /// <summary>
        /// 此栏目的父级栏目名，和<see cref="PropertyName"/>配合，用于显示DetailGrid。
        /// 只用于<see cref="T:Feng.Grid.DataUnboundWithDetailGridLoadonce"/>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ParentPropertyName
        {
            get;
            set;
        }

        #endregion

        #region "only to Control"

        /// <summary>
        /// 数据控件是否可见，格式见<see cref="T:Feng.Authority"/>。
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string DataControlVisible { get; set; }

        /// <summary>
        /// 查找控件是否可见，格式见<see cref="T:Feng.Authority"/>。
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string SearchControlVisible { get; set; }

        /// <summary>
        /// 设置<see cref="Feng.ISearchControl.SpecialPropertyName"/>。
        /// 如果为空，则<see cref="Feng.ISearchControl"/>的查找属性名称为 Navigator + "." + PropertyName
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string SearchControlFullPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 查找控件如果是多行的（ComboBox），需要定义的筛选器。
        /// 例如，数据源是员工，但此处只需要显示男员工，则Filter为“性别=男”
        /// </summary>
        [Property(Column = "ControlInitParamFilter", Length = 255, NotNull = false)]
        public virtual string SearchControlInitParamFilter { get; set; }


        /// <summary>
        /// 设置<see cref="Feng.ISearchControl.AdditionalSearchExpression"/>。
        /// 当选择这个查找控件时，除了自身查找条件外，其他的查找条件。
        /// 例如，查找“付款金额”，但实体类中只有属性“金额”，此时SearchControlAdditionalSearchExpression要设置为"收付标志=付"
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string SearchControlAdditionalSearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 控件类型。
        /// 数据控件目前支持
        /// <list type="bullet">
        /// <item>MyTextBox：单行文本框</item>
        /// <item>MyMultilineTextBox：多行文本框，多行需下拉，平常显示为单行</item>
        /// <item>MyMultilineTextBox2：多行文本框，显示为多行</item>
        /// <item>MyRichTextBox：格式丰富的多行文本框</item>
        /// <item>MyIntegerTextBox：整数输入文本框，返回类型为Int32</item>
        /// <item>MyLongTextBox：整数输入文本框，返回类型为Long</item>
        /// <item>MyNumericTextBox：数值输入文本框</item>
        /// <item>MyCurrencyTextBox：货币输入文本框（带货币符号）</item>
        /// <item>MyComboBox：选择框（限定于下拉框）</item>
        /// <item>MyFreeComboBox：选择框，并可只有输入（不限定于下拉框）</item>
        /// <item>MyListBox：列表框</item>
        /// <item>MyRadioListBox：单选按钮列表框</item>
        /// <item>MyCheckedListBox：多选按钮列表框</item>
        /// <item>MyOptionPicker：下拉多选框</item>
        /// <item>MyRadioButton：单选按钮</item>
        /// <item>MyCheckBox：多选按钮</item>
        /// <item>MyThreeStateCheckbox: 3态多选按钮</item>
        /// <item>MyPictureBox：图片框</item>
        /// <item>MyDateTimePicker：时间日期选择框</item>
        /// <item>MyDatePicker：日期选择框</item>
        /// <item>MyMonthPicker：月份选择框</item>
        /// <item>MyFileBox：文件选择框</item>
        /// <item>MyGrid：数据表格</item>
        /// </list>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string DataControlType { get; set; }

        /// <summary>
        /// 控件类型。
        /// 查找控件目前支持
        /// <list type="bullet">
        /// <item>MyTextBox：单行文本框。可用%通配符。默认为模糊匹配（开头末尾加%），如果需精确匹配，末尾加'#'</item>
        /// <item>MyMultilineTextBox：多行文本框。如数据为单行，模式同MyTextBox。如多行，模式同SQL语句中的IN功能</item>
        /// <item>MyIntegerTextBox：整数范围内查找，匹配Int32</item>
        /// <item>MyLongTextBox： 整数范围内查找，匹配Long</item>
        /// <item>MyNumericTextBox：数值范围内查找</item>
        /// <item>MyCurrencyTextBox：货币范围内查找</item>
        /// <item>MyComboBox：选择框（限定于下拉框）</item>
        /// <item>MyFreeComboBox：选择框，并可只有输入（不限定于下拉框）</item>
        /// <item>MyOptionPicker：下拉多选框</item>
        /// <item>MyRadioButton：单选按钮</item>
        /// <item>MyDateTimePicker：时间日期范围内查找</item>
        /// <item>MyDatePicker：日期范围内查找</item>
        /// <item>MyMonthPicker：月份范围内查找</item>
        /// <item>CustomSearchControl：自定义查找，数据定义在<see cref="CustomSearchInfo"/>，需要指定PropertyName，以匹配GroupName</item>
        /// </list>
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string SearchControlType { get; set; }

        /// <summary>
        /// 查找控件初始化参数。
        /// 如果类型<see cref="SearchControlType"/>是MyComboBox、MyFreeComboBox、MyOptionPicker，需要有参数。
        /// 参数可为内部DataTable缓存名称<see cref="NameValueMappingInfo"/>或者Enum（对应Enum类型）。具体见<see cref="T:Feng.Windows.Forms.ControlFactory"/>
        /// 数据控件的初始化参数用<see cref="CellEditorManagerParam"/> 
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string SearchControlInitParam { get; set; }

        /// <summary>
        /// 数据控件的默认值，同时也是表格InsertRow的默认值
        /// 如果是Enum，例如"性别.男"，只需要写"男"。
        /// 格式同<see cref="SearchControlDefaultValue"/>
        /// </summary>
        [Property(Length = 400, NotNull = false)]
        public virtual string DataControlDefaultValue { get; set; }

        /// <summary>
        /// 查找控件的默认值(如是LabeledSearchControlBetween，则折返回的值用","分隔
        /// 如果需要不同权限设置不同值，用;分开。默认值后加#后跟权限表格式。
        /// 例如a,b#I:A; c,d#I:B。默认值可用Python表达式（返回结果为result变量）。
        /// 如果要用Python表达式，必须以#Python开头，而且前后要添加""。
        /// 如果需要取反，前面加NOT; 如果需要为空，用null。（如果不为空，则是NOT null）
        /// </summary>
        [Property(Length = 400, NotNull = false)]
        public virtual string SearchControlDefaultValue { get; set; }

        /// <summary>
        /// 当查找IsNull时，是FullPropertyName IsNull 还是 Navigator IsNull
        /// 例如，对帐单.对账单号 为空，或者 对帐单 为空
        /// True时为前者。
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool SearchNullUseFull { get; set; }

        /// <summary>
        /// 查找控件的默认顺序
        /// <list>
        /// <item>ASC: 顺序</item>
        /// <item>DESC: 逆序</item>
        /// </list>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string SearchControlDefaultOrder { get; set; }

        /// <summary>
        /// 用于<see cref="CellEditorManager"/>的筛选器。具体见<see cref="SearchControlInitParamFilter"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string CellEditorManagerParamFilter { get; set; }

        #endregion

        #region "Events"
        /// <summary>
        /// 此Column的Cell DoubleClickd时执行的Event Process
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string DoubleClick
        {
            get;
            set;
        }
        #endregion
    }
}