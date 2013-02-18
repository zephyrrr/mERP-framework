using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserControlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userControl"></param>
        /// <returns></returns>
        public static DialogResult ShowDialog(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            Form form = new Form();
            form.Controls.Add(userControl);
            return form.ShowDialog();
        }

        public static T SearchChildControl<T>(Control parent, Func<T, bool> func = null)
            where T : class
        {
            if (parent == null)
                return null;

            T t = parent as T;
            if (t != null && (func == null || func(t)))
                return t;
            foreach (Control i in parent.Controls)
            {
                var r = SearchChildControl<T>(i, func);
                if (r != null)
                    return r;
            }
            return null;
        }

        public static T SearchParentControl<T>(Control child, Func<T, bool> func = null)
            where T : class
        {
            if (child == null)
                return null;

            T t = child as T;
            if (t != null && (func == null || func(t)))
                return t;
            var r = SearchParentControl<T>(child.Parent, func);
            if (r != null)
                return r;
            return null;
        }
    }
}
