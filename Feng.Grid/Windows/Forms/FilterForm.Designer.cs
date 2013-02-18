namespace Feng.Windows.Forms
{
    partial class FilterForm
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
			this.btnCancel = new Feng.Windows.Forms.MyButton();
			this.btnOK = new Feng.Windows.Forms.MyButton();
			this.rbgFilterMethod = new Feng.Windows.Forms.MyRadioListBox();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(308, 261);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.ReadOnly = false;
			this.btnCancel.Size = new System.Drawing.Size(72, 21);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(98, 261);
			this.btnOK.Name = "btnOK";
			this.btnOK.ReadOnly = false;
			this.btnOK.Size = new System.Drawing.Size(72, 21);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// rbgFilterMethod
			// 
			this.rbgFilterMethod.BackColor = System.Drawing.SystemColors.Control;
			this.rbgFilterMethod.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rbgFilterMethod.Location = new System.Drawing.Point(349, 43);
			this.rbgFilterMethod.Name = "rbgFilterMethod";
			this.rbgFilterMethod.Size = new System.Drawing.Size(108, 130);
			this.rbgFilterMethod.TabIndex = 10;
			// 
			// FilterForm
			// 
			this.ClientSize = new System.Drawing.Size(481, 300);
			this.Controls.Add(this.rbgFilterMethod);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "FilterForm";
			this.Text = "筛选";
			this.ResumeLayout(false);

        }

        #endregion

		private Feng.Windows.Forms.MyButton btnCancel;
        private Feng.Windows.Forms.MyButton btnOK;
		private Feng.Windows.Forms.MyRadioListBox rbgFilterMethod;
    }
}