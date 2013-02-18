using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SearchWindowSetupForm : MyForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fm"></param>
        public SearchWindowSetupForm(ISearchManager sm)
        {
            InitializeComponent();

            m_sm = sm;
            txtMaxResult.Value = m_sm.MaxResult;
        }

        private ISearchManager m_sm;
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaxResult.TextBoxArea.Text))
            {
                m_sm.MaxResult = Feng.Utils.ConvertHelper.ToInt(txtMaxResult.Value).Value;
            }
        }
    }
}
