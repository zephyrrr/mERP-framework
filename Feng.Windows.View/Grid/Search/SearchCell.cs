using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Xceed.Grid;
using Xceed.Grid.Collections;
using Xceed.Grid.Editors;
using Xceed.Grid.Viewers;
using Xceed.Editors;
using Feng.Grid.Viewers;

namespace Feng.Grid.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchEditor : TextEditor
    {
        /// <summary>
        /// 
        /// </summary>
        public SearchEditor()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override CreateControlMode CreateControlMode
        {
            get
            {
                // The template control will be the one used.
                // It won't be cloned each time the cell enters edition.
                return CreateControlMode.SingleInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override System.Windows.Forms.Control CreateControl()
        {
            return this.TemplateControl;
        }
    }

    /// <summary>
    /// The FilterCell class is used to populate a FilterRow.  
    /// It contains a list of possible filters that is dynamic to its sibling cells values.
    /// </summary>
    public class SearchCell : ValueCell
    {
        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        public SearchCell()
            : base()
        {
            this.InitializeInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        public SearchCell(Column parentColumn)
            : base(parentColumn)
        {
            this.InitializeInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public SearchCell(SearchCell template)
            : base(template)
        {
        }

        //private Dictionary<string, IFilter> m_filterItems;

        private void InitializeInstance()
        {
        //    m_filterItems = new Dictionary<string, IFilter>();
        //    this.DoubleClick += new EventHandler(FilterCell_DoubleClick);
        }

        //void FilterCell_DoubleClick(object sender, EventArgs e)
        //{
        //    TemplateControl_SelectedValueChanged(this.CellEditorManager.TemplateControl, System.EventArgs.Empty);
        //}

        #endregion CONSTRUCTORS


        internal static string GetSearchPropertyName(GridColumnInfo info)
        {
            string searchPropertyName = null;
            if (info != null)
            {
                if (string.IsNullOrEmpty(info.SearchControlFullPropertyName))
                {
                    if (string.IsNullOrEmpty(info.Navigator))
                    {
                        searchPropertyName = info.PropertyName;
                    }
                    else
                    {
                        searchPropertyName = info.Navigator.Replace('.', ':') + ":" + info.PropertyName;
                    }
                }
                else
                {
                    searchPropertyName = info.SearchControlFullPropertyName;
                }
            }
            return searchPropertyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISearchExpression GetSearchExpression()
        {
            if (this.Value != null && this.Value != this.NullValue && this.Value.ToString() != this.NullText)
            {
                GridColumnInfo info = this.ParentColumn.Tag as GridColumnInfo;
                if (info != null)
                {
                    string searchPropertyName = GetSearchPropertyName(info);

                    ISearchExpression ret = SearchExpression.Like(searchPropertyName, this.Value);

                    object searchValue = this.Value;
                    if (searchValue != null && info.CellViewerManager == "Combo")
                    {
                        string nvName = Feng.Windows.Utils.GridColumnInfoHelper.GetNameValueMappingName(info, true);
                        searchValue = Feng.Utils.NameValueControlHelper.GetMultiValue(nvName, searchValue.ToString());

                        ret = SearchExpression.Or(ret, SearchExpression.Eq(searchPropertyName, searchValue));
                    }

                    return ret;
                }
            }

            return null;
            //object value;
            //if (this.IsBeingEdited)
            //{
            //    value = ((FilterEditor)this.CellEditorManager).TemplateControl.SelectedValue;
            //}
            //else
            //{
            //    value = this.Value;
            //}

            //if (m_filterItems.ContainsKey(value.ToString()))
            //{
            //    return m_filterItems[value.ToString()];
            //}
            //else
            //{
            //    // 因为Filter有保存，但是里面的内容有变化，所以可能产生无内容的情况
            //    // this.Value = s_anyText; // 但是还是保存Value
            //    return m_filterItems[s_anyText];
            //}
        }

        #region PROTECTED PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        public override bool IsCellEditorManagerAmbient
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsCellViewerManagerAmbient
        {
            get { return false; }
        }

        #endregion PROTECTED PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Cell CreateInstance()
        {
            return new SearchCell(this);
        }
    }
}
