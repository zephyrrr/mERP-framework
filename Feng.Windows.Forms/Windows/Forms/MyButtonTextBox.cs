using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using Xceed.Editors;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyButtonTextBox
    /// </summary>
    public partial class MyButtonTextBox : Xceed.Editors.WinTextBox
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public MyButtonTextBox()
        {
            InitializeComponent();
            this.Initialize();
        }

        protected MyButtonTextBox(MyButtonTextBox template)
            : base(template)
        {
            // will add sidebutton twice
            //InitializeComponent();

            this.Initialize();
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public MyButtonTextBox(System.Type callerType, EnhancedBorderStyle borderStyle, bool allowDropDown)
        //    : base(callerType, borderStyle)
        //{
        //    this.Initialize();
        //}


        public override object Clone()
        {
            return new MyButtonTextBox(this);
        }

        protected override Xceed.Editors.TextBoxArea CreateTextBoxArea()
        {
            return base.CreateTextBoxArea();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            this.SideButtons[0].Click -= new EventHandler(dropDownButton1_Click);
            this.SideButtons[0].Click += new EventHandler(dropDownButton1_Click);
            //WinButton button = new WinButton(base.GetType());
            //base.SideButtons.Add(button);
            //button.Width = 20;
        }

        /// <summary>
        /// SelectButtonClick
        /// </summary>
        public EventHandler ButtonClick;
        void dropDownButton1_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }
    }
}
