using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class PageBindingSource : System.Windows.Forms.BindingSource
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_searchManager != null)
                {
                    m_searchManager.DataLoaded -= new EventHandler<DataLoadedEventArgs>(SearchManagerDataLoaded);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sm"></param>
        public PageBindingSource(ISearchManager sm)
        {
            m_searchManager = sm;

            m_searchManager.DataLoaded += new EventHandler<DataLoadedEventArgs>(SearchManagerDataLoaded);
        }

        private bool m_initialize;

        private void SearchManagerDataLoaded(object sender, DataLoadedEventArgs e)
        {
            m_initialize = true;
            if (m_searchManager.EnablePage)
            {
                if (m_searchManager.MaxResult != 0)
                {
                    int page = (m_searchManager.Count - 1) / m_searchManager.MaxResult + 1;
                    base.DataSource = new int[page];
                    base.DataMember = "";
                    this.Position = m_searchManager.FirstResult / m_searchManager.MaxResult;
                }
                else
                {
                    int page = 0;
                    base.DataSource = new int[page];
                    base.DataMember = "";
                    this.Position = 0;
                }
            }
            else
            {
                int page = m_searchManager.MaxResult == 0 ? 0 : 1;
                base.DataSource = new int[page];
                base.DataMember = "";
                this.Position = 0;
            }
            m_initialize = false;
        }

        private ISearchManager m_searchManager;

        /// <summary>
        /// 
        /// </summary>
        public ISearchManager SearchManager
        {
            get { return m_searchManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPositionChanged(EventArgs e)
        {
            if (!m_initialize)
            {
                m_searchManager.FirstResult = base.Position * m_searchManager.MaxResult;
                m_searchManager.ReloadData();
            }

            base.OnPositionChanged(e);
        }
    }
}