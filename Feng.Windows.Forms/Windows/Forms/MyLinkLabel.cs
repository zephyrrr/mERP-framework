using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyLinkLabel.
    /// </summary>
    public class MyLinkLabel : LinkLabel
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyLinkLabel()
        {
            base.BorderStyle = BorderStyle.None;
            base.FlatStyle = System.Windows.Forms.FlatStyle.System;
            base.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            base.AutoSize = true;
            this.Size = new System.Drawing.Size(72, 21);
        }


        /// <summary>
        /// AutoSize
        /// </summary>
        [DefaultValue(false)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        /// <summary>
        /// Default FlatStyle
        /// </summary>
        [DefaultValue(System.Windows.Forms.FlatStyle.System)]
        public new System.Windows.Forms.FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        /// <summary>
        /// Default BorderStyle 
        /// </summary>
        [DefaultValue(System.Windows.Forms.BorderStyle.None)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Default TextAlign 
        /// </summary>
        [DefaultValue(System.Drawing.ContentAlignment.MiddleLeft)]
        public new System.Drawing.ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        #endregion
    }
}