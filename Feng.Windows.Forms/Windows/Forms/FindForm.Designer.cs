namespace Feng.Windows.Forms
{
	partial class FindForm
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
			this.myLabel1 = new Feng.Windows.Forms.MyLabel();
			this.txtContent = new Feng.Windows.Forms.MyTextBox();
			this.btnFind = new Feng.Windows.Forms.MyButton();
			this.btnCancel = new Feng.Windows.Forms.MyButton();
			this.ckbCaseSensitive = new Feng.Windows.Forms.MyCheckBox();
			this.myGroupBox1 = new Feng.Windows.Forms.MyGroupBox();
			this.myRadioButton2 = new Feng.Windows.Forms.MyRadioButton();
			this.rbDirectionUp = new Feng.Windows.Forms.MyRadioButton();
			this.myGroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// myLabel1
			// 
			this.myLabel1.AutoSize = true;
			this.myLabel1.Location = new System.Drawing.Point(13, 28);
			this.myLabel1.Name = "myLabel1";
			this.myLabel1.Size = new System.Drawing.Size(77, 12);
			this.myLabel1.TabIndex = 0;
			this.myLabel1.Text = "查找内容(&N):";
			// 
			// txtContent
			// 
			this.txtContent.Location = new System.Drawing.Point(96, 25);
			this.txtContent.Name = "txtContent";
			this.txtContent.Size = new System.Drawing.Size(190, 21);
			this.txtContent.TabIndex = 1;
			this.txtContent.TextChanged += new System.EventHandler(this.txtContent_TextChanged);
            this.txtContent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtContent_KeyPress);
			// 
			// btnFind
			// 
			this.btnFind.Enabled = false;
			this.btnFind.Location = new System.Drawing.Point(305, 25);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(93, 21);
			this.btnFind.TabIndex = 2;
			this.btnFind.Text = "查找下一个(&F)";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(305, 56);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.ReadOnly = false;
			this.btnCancel.Size = new System.Drawing.Size(93, 21);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ckbCaseSensitive
			// 
			this.ckbCaseSensitive.AutoSize = true;
			this.ckbCaseSensitive.Location = new System.Drawing.Point(12, 79);
			this.ckbCaseSensitive.Name = "ckbCaseSensitive";
			this.ckbCaseSensitive.Size = new System.Drawing.Size(108, 17);
			this.ckbCaseSensitive.TabIndex = 4;
			this.ckbCaseSensitive.Text = "区分大小写(&C)";
			this.ckbCaseSensitive.UseVisualStyleBackColor = true;
			// 
			// myGroupBox1
			// 
			this.myGroupBox1.Controls.Add(this.myRadioButton2);
			this.myGroupBox1.Controls.Add(this.rbDirectionUp);
			this.myGroupBox1.Location = new System.Drawing.Point(124, 56);
			this.myGroupBox1.Name = "myGroupBox1";
			this.myGroupBox1.ReadOnly = false;
			this.myGroupBox1.Size = new System.Drawing.Size(162, 40);
			this.myGroupBox1.TabIndex = 5;
			this.myGroupBox1.TabStop = false;
			this.myGroupBox1.Text = "方向";
			// 
			// myRadioButton2
			// 
			this.myRadioButton2.AutoSize = true;
			this.myRadioButton2.Checked = true;
			this.myRadioButton2.Location = new System.Drawing.Point(84, 17);
			this.myRadioButton2.Name = "myRadioButton2";
			this.myRadioButton2.Size = new System.Drawing.Size(71, 17);
			this.myRadioButton2.TabIndex = 1;
			this.myRadioButton2.TabStop = true;
			this.myRadioButton2.Text = "向下(&D)";
			this.myRadioButton2.UseVisualStyleBackColor = true;
			// 
			// rbDirectionUp
			// 
			this.rbDirectionUp.AutoSize = true;
			this.rbDirectionUp.Location = new System.Drawing.Point(7, 17);
			this.rbDirectionUp.Name = "rbDirectionUp";
			this.rbDirectionUp.Size = new System.Drawing.Size(71, 17);
			this.rbDirectionUp.TabIndex = 0;
			this.rbDirectionUp.Text = "向上(&U)";
			this.rbDirectionUp.UseVisualStyleBackColor = true;
			// 
			// FindForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 116);
            this.ShowInTaskbar = false;
			this.Controls.Add(this.myGroupBox1);
			this.Controls.Add(this.ckbCaseSensitive);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.txtContent);
			this.Controls.Add(this.myLabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FindForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "查找";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FindForm_FormClosing);
			this.myGroupBox1.ResumeLayout(false);
			this.myGroupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Feng.Windows.Forms.MyLabel myLabel1;
		private Feng.Windows.Forms.MyTextBox txtContent;
		private Feng.Windows.Forms.MyButton btnFind;
		private Feng.Windows.Forms.MyButton btnCancel;
		private Feng.Windows.Forms.MyCheckBox ckbCaseSensitive;
		private Feng.Windows.Forms.MyGroupBox myGroupBox1;
		private Feng.Windows.Forms.MyRadioButton myRadioButton2;
		private Feng.Windows.Forms.MyRadioButton rbDirectionUp;
	}
}