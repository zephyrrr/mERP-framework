using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class BookmarkPropertyForm : Form
    {
        private BookmarkPropertyForm()
        {
            InitializeComponent();
        }

        public BookmarkPropertyForm(BookmarkInfo bookmarkInfo)
            : this()
        {
            txtName.Text = bookmarkInfo.Name;
            txtAddress.Text = bookmarkInfo.Address;

            m_bookmarkInfo = bookmarkInfo;
        }
        private BookmarkInfo m_bookmarkInfo;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageForm.ShowWarning("名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageForm.ShowWarning("地址不能为空！");
                return;
            }

            m_bookmarkInfo.Name = txtName.Text;
            m_bookmarkInfo.Address = txtAddress.Text;
        }

    }
}
