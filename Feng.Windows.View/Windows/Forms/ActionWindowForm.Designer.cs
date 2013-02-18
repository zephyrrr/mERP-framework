namespace Feng.Windows.Forms
{
    partial class ActionWindowForm
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
            this.actionButtons1 = new Feng.Windows.Forms.ActionButtons();
            this.SuspendLayout();
            // 
            // actionButtons1
            // 
            this.actionButtons1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.actionButtons1.Location = new System.Drawing.Point(228, 577);
            this.actionButtons1.Name = "actionButtons1";
            this.actionButtons1.ReadOnly = false;
            this.actionButtons1.Size = new System.Drawing.Size(389, 24);
            this.actionButtons1.TabIndex = 0;
            // 
            // ActionWindowForm
            // 
            this.ClientSize = new System.Drawing.Size(850, 613);
            this.Controls.Add(this.actionButtons1);
            this.Name = "ActionWindowForm";
            this.Text = "ActionWindowForm";
            this.ResumeLayout(false);

        }

        #endregion

		private Feng.Windows.Forms.ActionButtons actionButtons1;
	}
}
