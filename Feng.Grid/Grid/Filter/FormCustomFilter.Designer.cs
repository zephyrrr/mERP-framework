namespace Feng.Grid.Filter
{
	partial class FormCustomFilter
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.cobOperator1 = new Feng.Windows.Forms.MyComboBox();
            this.dropDownButton1 = new Xceed.Editors.WinButton();
            this.cobValue1 = new Feng.Windows.Forms.MyFreeComboBox();
            this.dropDownButton2 = new Xceed.Editors.WinButton();
            this.rbnLogic = new Feng.Windows.Forms.MyRadioButton();
            this.myRadioButton2 = new Feng.Windows.Forms.MyRadioButton();
            this.cobValue2 = new Feng.Windows.Forms.MyFreeComboBox();
            this.winButton1 = new Xceed.Editors.WinButton();
            this.cobOperator2 = new Feng.Windows.Forms.MyComboBox();
            this.winButton2 = new Xceed.Editors.WinButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator1.DropDownControl)).BeginInit();
            this.cobOperator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue1.DropDownControl)).BeginInit();
            this.cobValue1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue2.DropDownControl)).BeginInit();
            this.cobValue2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator2.DropDownControl)).BeginInit();
            this.cobOperator2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "显示行";
            // 
            // cobOperator1
            // 
            this.cobOperator1.Controls.Add(this.dropDownButton1);
            this.cobOperator1.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            this.cobOperator1.DropDownButton = this.dropDownButton1;
            // 
            // 
            // 
            this.cobOperator1.DropDownControl.Size = new System.Drawing.Size(98, 248);
            this.cobOperator1.DropDownControl.TabIndex = 0;
            this.cobOperator1.Location = new System.Drawing.Point(14, 41);
            this.cobOperator1.Name = "cobOperator1";
            this.cobOperator1.Size = new System.Drawing.Size(171, 21);
            this.cobOperator1.TabIndex = 1;
            // 
            // 
            // 
            this.cobOperator1.TextBoxArea.Name = "";
            this.cobOperator1.TextBoxArea.TabIndex = 0;
            this.cobOperator1.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.BackColor = System.Drawing.SystemColors.Control;
            this.dropDownButton1.ButtonType = new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown);
            this.dropDownButton1.CanSelect = false;
            this.dropDownButton1.CausesValidation = false;
            this.dropDownButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropDownButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dropDownButton1.Location = new System.Drawing.Point(152, 0);
            this.dropDownButton1.Name = "dropDownButton1";
            this.dropDownButton1.Size = new System.Drawing.Size(17, 19);
            this.dropDownButton1.TabIndex = 1;
            // 
            // cobValue1
            // 
            this.cobValue1.Columns.AddRange(new Xceed.Editors.ColumnInfo[] {
            new Xceed.Editors.ColumnInfo("ColumnValue", typeof(string), 100, -1, Xceed.Editors.ColumnSortDirection.None, true)});
            this.cobValue1.Controls.Add(this.dropDownButton2);
            this.cobValue1.DisplayFormat = "%ColumnValue%";
            this.cobValue1.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            this.cobValue1.DropDownButton = this.dropDownButton2;
            // 
            // 
            // 
            this.cobValue1.DropDownControl.Size = new System.Drawing.Size(98, 248);
            this.cobValue1.DropDownControl.TabIndex = 0;
            this.cobValue1.Location = new System.Drawing.Point(241, 41);
            this.cobValue1.Name = "cobValue1";
            this.cobValue1.Size = new System.Drawing.Size(177, 21);
            this.cobValue1.TabIndex = 2;
            // 
            // 
            // 
            this.cobValue1.TextBoxArea.Name = "";
            this.cobValue1.TextBoxArea.TabIndex = 0;
            this.cobValue1.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
            this.cobValue1.ValidateText = false;
            // 
            // dropDownButton2
            // 
            this.dropDownButton2.BackColor = System.Drawing.SystemColors.Control;
            this.dropDownButton2.ButtonType = new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown);
            this.dropDownButton2.CanSelect = false;
            this.dropDownButton2.CausesValidation = false;
            this.dropDownButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dropDownButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dropDownButton2.Location = new System.Drawing.Point(158, 0);
            this.dropDownButton2.Name = "dropDownButton2";
            this.dropDownButton2.Size = new System.Drawing.Size(17, 19);
            this.dropDownButton2.TabIndex = 1;
            // 
            // rbnLogic
            // 
            this.rbnLogic.AutoSize = true;
            this.rbnLogic.Checked = true;
            this.rbnLogic.Location = new System.Drawing.Point(83, 87);
            this.rbnLogic.Name = "rbnLogic";
            this.rbnLogic.ReadOnly = false;
            this.rbnLogic.Size = new System.Drawing.Size(59, 17);
            this.rbnLogic.TabIndex = 3;
            this.rbnLogic.TabStop = true;
            this.rbnLogic.Text = "与(&A)";
            this.rbnLogic.UseVisualStyleBackColor = true;
            // 
            // myRadioButton2
            // 
            this.myRadioButton2.AutoSize = true;
            this.myRadioButton2.Location = new System.Drawing.Point(166, 87);
            this.myRadioButton2.Name = "myRadioButton2";
            this.myRadioButton2.ReadOnly = false;
            this.myRadioButton2.Size = new System.Drawing.Size(59, 17);
            this.myRadioButton2.TabIndex = 4;
            this.myRadioButton2.TabStop = true;
            this.myRadioButton2.Text = "或(&O)";
            this.myRadioButton2.UseVisualStyleBackColor = true;
            // 
            // cobValue2
            // 
            this.cobValue2.Columns.AddRange(new Xceed.Editors.ColumnInfo[] {
            new Xceed.Editors.ColumnInfo("ColumnValue", typeof(string), 100, -1, Xceed.Editors.ColumnSortDirection.None, true)});
            this.cobValue2.Controls.Add(this.winButton1);
            this.cobValue2.DisplayFormat = "%ColumnValue%";
            this.cobValue2.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            this.cobValue2.DropDownButton = this.winButton1;
            // 
            // 
            // 
            this.cobValue2.DropDownControl.Size = new System.Drawing.Size(98, 248);
            this.cobValue2.DropDownControl.TabIndex = 0;
            this.cobValue2.Location = new System.Drawing.Point(241, 122);
            this.cobValue2.Name = "cobValue2";
            this.cobValue2.Size = new System.Drawing.Size(177, 21);
            this.cobValue2.TabIndex = 6;
            // 
            // 
            // 
            this.cobValue2.TextBoxArea.Name = "";
            this.cobValue2.TextBoxArea.TabIndex = 0;
            this.cobValue2.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
            this.cobValue2.ValidateText = false;
            // 
            // winButton1
            // 
            this.winButton1.BackColor = System.Drawing.SystemColors.Control;
            this.winButton1.ButtonType = new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown);
            this.winButton1.CanSelect = false;
            this.winButton1.CausesValidation = false;
            this.winButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.winButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.winButton1.Location = new System.Drawing.Point(158, 0);
            this.winButton1.Name = "winButton1";
            this.winButton1.Size = new System.Drawing.Size(17, 19);
            this.winButton1.TabIndex = 1;
            // 
            // cobOperator2
            // 
            this.cobOperator2.Controls.Add(this.winButton2);
            this.cobOperator2.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            this.cobOperator2.DropDownButton = this.winButton2;
            // 
            // 
            // 
            this.cobOperator2.DropDownControl.Size = new System.Drawing.Size(98, 248);
            this.cobOperator2.DropDownControl.TabIndex = 0;
            this.cobOperator2.Location = new System.Drawing.Point(14, 122);
            this.cobOperator2.Name = "cobOperator2";
            this.cobOperator2.Size = new System.Drawing.Size(171, 21);
            this.cobOperator2.TabIndex = 5;
            // 
            // 
            // 
            this.cobOperator2.TextBoxArea.Name = "";
            this.cobOperator2.TextBoxArea.TabIndex = 0;
            this.cobOperator2.UIStyle = Xceed.UI.UIStyle.ResourceAssembly;
            // 
            // winButton2
            // 
            this.winButton2.BackColor = System.Drawing.SystemColors.Control;
            this.winButton2.ButtonType = new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown);
            this.winButton2.CanSelect = false;
            this.winButton2.CausesValidation = false;
            this.winButton2.Dock = System.Windows.Forms.DockStyle.Right;
            this.winButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.winButton2.Location = new System.Drawing.Point(152, 0);
            this.winButton2.Name = "winButton2";
            this.winButton2.Size = new System.Drawing.Size(17, 19);
            this.winButton2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(110, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "确定(&O)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(241, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "取消(&C)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormCustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 216);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cobValue2);
            this.Controls.Add(this.cobOperator2);
            this.Controls.Add(this.myRadioButton2);
            this.Controls.Add(this.rbnLogic);
            this.Controls.Add(this.cobValue1);
            this.Controls.Add(this.cobOperator1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "自定义筛选方式";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCustomFilter_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator1.DropDownControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator1)).EndInit();
            this.cobOperator1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cobValue1.DropDownControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue1)).EndInit();
            this.cobValue1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cobValue2.DropDownControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobValue2)).EndInit();
            this.cobValue2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator2.DropDownControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cobOperator2)).EndInit();
            this.cobOperator2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private Feng.Windows.Forms.MyComboBox cobOperator1;
		private Xceed.Editors.WinButton dropDownButton1;
		private Feng.Windows.Forms.MyFreeComboBox cobValue1;
		private Xceed.Editors.WinButton dropDownButton2;
		private Feng.Windows.Forms.MyRadioButton rbnLogic;
		private Feng.Windows.Forms.MyRadioButton myRadioButton2;
		private Feng.Windows.Forms.MyFreeComboBox cobValue2;
		private Xceed.Editors.WinButton winButton1;
		private Feng.Windows.Forms.MyComboBox cobOperator2;
		private Xceed.Editors.WinButton winButton2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}