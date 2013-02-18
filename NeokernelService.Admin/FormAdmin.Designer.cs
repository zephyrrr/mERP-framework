namespace NeokernelService
{
    partial class FormAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSVNBinaryPath = new System.Windows.Forms.Button();
            this.textBoxSVNBinaryPath = new System.Windows.Forms.TextBox();
            this.labelSVNBinaryPath = new System.Windows.Forms.Label();
            this.textBoxRepositoryPath = new System.Windows.Forms.TextBox();
            this.labelRepositoryPath = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.toolTipListenHost = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSVNBinaryPath);
            this.groupBox1.Controls.Add(this.textBoxSVNBinaryPath);
            this.groupBox1.Controls.Add(this.labelSVNBinaryPath);
            this.groupBox1.Controls.Add(this.textBoxRepositoryPath);
            this.groupBox1.Controls.Add(this.labelRepositoryPath);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(14, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 172);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // buttonSVNBinaryPath
            // 
            this.buttonSVNBinaryPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSVNBinaryPath.Location = new System.Drawing.Point(484, 26);
            this.buttonSVNBinaryPath.Name = "buttonSVNBinaryPath";
            this.buttonSVNBinaryPath.Size = new System.Drawing.Size(48, 24);
            this.buttonSVNBinaryPath.TabIndex = 2;
            this.buttonSVNBinaryPath.Text = "...";
            this.buttonSVNBinaryPath.Click += new System.EventHandler(this.buttonSVNBinaryPath_Click);
            // 
            // textBoxSVNBinaryPath
            // 
            this.textBoxSVNBinaryPath.Location = new System.Drawing.Point(170, 26);
            this.textBoxSVNBinaryPath.Name = "textBoxSVNBinaryPath";
            this.textBoxSVNBinaryPath.Size = new System.Drawing.Size(288, 21);
            this.textBoxSVNBinaryPath.TabIndex = 1;
            // 
            // labelSVNBinaryPath
            // 
            this.labelSVNBinaryPath.AutoSize = true;
            this.labelSVNBinaryPath.Location = new System.Drawing.Point(27, 29);
            this.labelSVNBinaryPath.Name = "labelSVNBinaryPath";
            this.labelSVNBinaryPath.Size = new System.Drawing.Size(137, 12);
            this.labelSVNBinaryPath.TabIndex = 7;
            this.labelSVNBinaryPath.Text = "Neokernel Binary Path:";
            // 
            // textBoxRepositoryPath
            // 
            this.textBoxRepositoryPath.Location = new System.Drawing.Point(170, 63);
            this.textBoxRepositoryPath.Name = "textBoxRepositoryPath";
            this.textBoxRepositoryPath.Size = new System.Drawing.Size(288, 21);
            this.textBoxRepositoryPath.TabIndex = 3;
            // 
            // labelRepositoryPath
            // 
            this.labelRepositoryPath.AutoSize = true;
            this.labelRepositoryPath.Location = new System.Drawing.Point(32, 66);
            this.labelRepositoryPath.Name = "labelRepositoryPath";
            this.labelRepositoryPath.Size = new System.Drawing.Size(71, 12);
            this.labelRepositoryPath.TabIndex = 0;
            this.labelRepositoryPath.Text = "Parameters:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(302, 210);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(86, 25);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonClose.Location = new System.Drawing.Point(23, 210);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(87, 25);
            this.buttonClose.TabIndex = 13;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSave.Location = new System.Drawing.Point(398, 210);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(86, 25);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // toolTipListenHost
            // 
            this.toolTipListenHost.AutoPopDelay = 5000;
            this.toolTipListenHost.InitialDelay = 100;
            this.toolTipListenHost.ReshowDelay = 200;
            this.toolTipListenHost.ShowAlways = true;
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 265);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Name = "FormAdmin";
            this.Text = "nkServiceAdmin";
            this.Load += new System.EventHandler(this.FormAdmin_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSVNBinaryPath;
        private System.Windows.Forms.TextBox textBoxSVNBinaryPath;
        private System.Windows.Forms.Label labelSVNBinaryPath;
        private System.Windows.Forms.TextBox textBoxRepositoryPath;
        private System.Windows.Forms.Label labelRepositoryPath;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTipListenHost;
    }
}