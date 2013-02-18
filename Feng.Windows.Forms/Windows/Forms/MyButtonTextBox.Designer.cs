namespace Feng.Windows.Forms
{
    partial class MyButtonTextBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.dropDownButton1 = new Xceed.Editors.WinButton();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // 
            // 
            this.TextBoxArea.Name = "";
            this.TextBoxArea.SelectOnFocus = true;
            this.TextBoxArea.TabIndex = 0;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.BackColor = System.Drawing.SystemColors.Control;
            this.dropDownButton1.ButtonType = new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown);
            this.dropDownButton1.CanSelect = false;
            this.dropDownButton1.CausesValidation = false;
            this.dropDownButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropDownButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dropDownButton1.Location = new System.Drawing.Point(101, 0);
            this.dropDownButton1.Name = "dropDownButton1";
            this.dropDownButton1.Size = new System.Drawing.Size(17, 19);
            this.dropDownButton1.TabIndex = 1;
            this.dropDownButton1.Click +=new System.EventHandler(dropDownButton1_Click);
            // 
            // OptionWinTextBox
            // 
            this.Controls.Add(this.dropDownButton1);
            this.AllowDropDown = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "OptionWinTextBox";
            this.Size = new System.Drawing.Size(120, 21);
            this.TabIndex = 0;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Xceed.Editors.WinButton dropDownButton1;
    }
}
