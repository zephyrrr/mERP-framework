using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class MyMessageBox : IMessageBox
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="defaultNo"></param>
        /// <returns></returns>
        public bool ShowYesNo(string msg, string caption, bool defaultNo)
        {
            DialogResult ret;
            if (!defaultNo)
            {
                ret = System.Windows.Forms.MessageBox.Show(msg, caption, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                ret =  System.Windows.Forms.MessageBox.Show(msg, caption, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button2);
            }

            if (ret == System.Windows.Forms.DialogResult.Yes)
                return true;
            else
                return false;
        }

        public void ShowError(string msg, string caption)
        {
            System.Windows.Forms.MessageBox.Show(msg, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        public void ShowWarning(string msg, string caption)
        {
            System.Windows.Forms.MessageBox.Show(msg, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        public void ShowInfo(string msg, string caption)
        {
            System.Windows.Forms.MessageBox.Show(msg, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
