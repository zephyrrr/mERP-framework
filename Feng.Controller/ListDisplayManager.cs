//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;

//namespace Feng
//{
//    /// <summary>
//    /// Use DisplayManager
//    /// </summary>
//    public class ListDisplayManager : AbstractDisplayManager
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="disposing"></param>
//        protected override void Dispose(bool disposing)
//        {
//            base.Dispose(disposing);
//        }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="sm"></param>
//        public ListDisplayManager(ISearchManager sm)
//            : base(sm)
//        {
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="dataItem"></param>
//        public override void AddDataItem(object dataItem)
//        {
//            throw new NotSupportedException();
//        }

//        /// <summary>
//        /// 设置数据绑定。
//        /// 最后的数据源为DataView
//        /// 设置完数据源后，State为<see cref="StateType.View"/>
//        /// </summary>
//        /// <param name="dataSource">数据源</param>
//        /// <param name="dataMember">数据成员</param>
//        /// <exception cref="NotSupportedException">数据源不符合格式时抛出</exception>
//        public override void SetDataBinding(object dataSource, string dataMember)
//        {
//            if (dataSource == null)
//            {
//                this.Position = -1;
//            }
//            else
//            {
//                m_data = dataSource as IList;

//                if (m_data == null)
//                {
//                    throw new ArgumentException("dataSource in DictionaryDisplayManager must be IList!");
//                }

//                if (m_data.Count == 0)
//                {
//                    this.Position = -1;
//                }
//                else
//                {
//                    this.Position = 0;
//                }
//            }

//            SetBindingControlsData(this.BindingControls, dataSource, dataMember);
//        }

//        private IList m_data;
//        /// <summary>
//        /// Items
//        /// </summary>
//        public override IList Items
//        {
//            get { return m_data; }
//        }

//        internal static void SetBindingControlsData(IBindingControlCollection bcs, object dataSource, string dataMember)
//        {
//            foreach (IBindingControl bc in bcs)
//            {
//                bc.SetDataBinding(dataSource, dataMember);
//            }
//        }

//        /// <summary>
//        /// 复制
//        /// </summary>
//        /// <returns></returns>
//        public override object Clone()
//        {
//            ListDisplayManager dm = new ListDisplayManager(this.SearchManager.Clone() as ISearchManager);
//            Copy(this, dm);
//            return dm;
//        }

//    }
//}