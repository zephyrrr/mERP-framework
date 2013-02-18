using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;

namespace Feng.Windows.Forms
{
	/// <summary>
	/// ErrorReport
	/// </summary>
    public partial class ErrorReport : System.Windows.Forms.Form
	{
		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing &&(components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
            this.txtErrorMsg = new System.Windows.Forms.TextBox();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.btnSendBugReport = new System.Windows.Forms.Button();
            this.picMsgPicture = new System.Windows.Forms.PictureBox();
            this.panTop = new System.Windows.Forms.Panel();
            this.panBottom = new System.Windows.Forms.Panel();
            this.btnThrowException = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMsgPicture)).BeginInit();
            this.panTop.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.BackColor = System.Drawing.SystemColors.Control;
            this.txtErrorMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrorMsg.Location = new System.Drawing.Point(61, 23);
            this.txtErrorMsg.Multiline = true;
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.ReadOnly = true;
            this.txtErrorMsg.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtErrorMsg.Size = new System.Drawing.Size(247, 32);
            this.txtErrorMsg.TabIndex = 1;
            this.txtErrorMsg.TabStop = false;
            this.txtErrorMsg.Text = "错误信息 1234567890 1234567890 1234567890 1234567890 1234567890 1234567890";
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnViewDetails.Location = new System.Drawing.Point(237, 74);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(75, 25);
            this.btnViewDetails.TabIndex = 2;
            this.btnViewDetails.Text = "详细(&D)↓";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Location = new System.Drawing.Point(131, 74);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtDetails
            // 
            this.txtDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetails.Location = new System.Drawing.Point(0, 3);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDetails.Size = new System.Drawing.Size(321, 96);
            this.txtDetails.TabIndex = 4;
            this.txtDetails.TabStop = false;
            this.txtDetails.WordWrap = false;
            // 
            // btnSendBugReport
            // 
            this.btnSendBugReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendBugReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendBugReport.Location = new System.Drawing.Point(237, 105);
            this.btnSendBugReport.Name = "btnSendBugReport";
            this.btnSendBugReport.Size = new System.Drawing.Size(75, 25);
            this.btnSendBugReport.TabIndex = 5;
            this.btnSendBugReport.Text = "复制";
            this.btnSendBugReport.UseVisualStyleBackColor = true;
            this.btnSendBugReport.Click += new System.EventHandler(this.btnSendBugReport_Click);
            // 
            // picMsgPicture
            // 
            this.picMsgPicture.Location = new System.Drawing.Point(12, 15);
            this.picMsgPicture.Name = "picMsgPicture";
            this.picMsgPicture.Size = new System.Drawing.Size(32, 32);
            this.picMsgPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picMsgPicture.TabIndex = 6;
            this.picMsgPicture.TabStop = false;
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.picMsgPicture);
            this.panTop.Controls.Add(this.txtErrorMsg);
            this.panTop.Controls.Add(this.btnViewDetails);
            this.panTop.Controls.Add(this.btnClose);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 0);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(324, 112);
            this.panTop.TabIndex = 7;
            // 
            // panBottom
            // 
            this.panBottom.Controls.Add(this.btnThrowException);
            this.panBottom.Controls.Add(this.btnSendBugReport);
            this.panBottom.Controls.Add(this.txtDetails);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBottom.Location = new System.Drawing.Point(0, 112);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new System.Drawing.Size(324, 137);
            this.panBottom.TabIndex = 8;
            // 
            // btnThrowException
            // 
            this.btnThrowException.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnThrowException.Location = new System.Drawing.Point(131, 105);
            this.btnThrowException.Name = "btnThrowException";
            this.btnThrowException.Size = new System.Drawing.Size(75, 25);
            this.btnThrowException.TabIndex = 7;
            this.btnThrowException.Text = "抛出异常";
            this.btnThrowException.UseVisualStyleBackColor = true;
            this.btnThrowException.Visible = false;
            this.btnThrowException.Click += new System.EventHandler(this.btnThrowException_Click);
            // 
            // ErrorReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(324, 249);
            this.Controls.Add(this.panBottom);
            this.Controls.Add(this.panTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorReport";
            this.ShowIcon = true;
            this.ShowInTaskbar = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "错误";
            ((System.ComponentModel.ISupportInitialize)(this.picMsgPicture)).EndInit();
            this.panTop.ResumeLayout(false);
            this.panTop.PerformLayout();
            this.panBottom.ResumeLayout(false);
            this.panBottom.PerformLayout();
            this.ResumeLayout(false);

        }
		internal System.Windows.Forms.Button btnViewDetails;
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.TextBox txtDetails;
		internal System.Windows.Forms.Button btnSendBugReport;
		internal System.Windows.Forms.PictureBox picMsgPicture;
        private TextBox txtErrorMsg;
        private Panel panTop;
        private Panel panBottom;
        internal Button btnThrowException;
	}
	
}
