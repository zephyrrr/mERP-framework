namespace Feng.Windows.Forms
{
    partial class MyObjectPicker
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
            this.dropDownGrid = new Feng.Grid.DataUnboundGrid();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropDownGrid)).BeginInit();
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
            // 
            // 
            // 
            this.dropDownGrid.Location = new System.Drawing.Point(0, 0);
            this.dropDownGrid.Name = "OptionGrid";
            this.dropDownGrid.Padding = new System.Windows.Forms.Padding(5, 0, 0, 20);
            this.dropDownGrid.Size = new System.Drawing.Size(120, 60);
            this.dropDownGrid.TabIndex = 1;
            // 
            // OptionPicker
            // 
            // 
            // OptionWinTextBox
            // 
            this.Controls.Add(this.dropDownButton1);
            this.DropDownAllowFocus = true;
            this.DropDownButton = this.dropDownButton1;
            this.DropDownControl = this.dropDownGrid;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "OptionWinTextBox";
            this.Size = new System.Drawing.Size(120, 21);
            this.TabIndex = 0;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropDownGrid)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Xceed.Editors.WinButton dropDownButton1;
        private Feng.Grid.DataUnboundGrid dropDownGrid;
    }
}
