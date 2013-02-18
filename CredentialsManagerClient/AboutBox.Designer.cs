namespace CredentialsManagerClient
{
   partial class AboutBox
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if(disposing && (components != null))
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
         System.Windows.Forms.Button okButton;
         System.Windows.Forms.Label companyNameLabel;
         System.Windows.Forms.Label copyrightLabel;
         System.Windows.Forms.Label productNameLabel;
         System.Windows.Forms.Label prodcutLabel;
         System.Windows.Forms.Label versionLabel;
         System.Windows.Forms.Label copyrightsLabel;
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.m_LinkLabel = new System.Windows.Forms.LinkLabel();
         okButton = new System.Windows.Forms.Button();
         companyNameLabel = new System.Windows.Forms.Label();
         copyrightLabel = new System.Windows.Forms.Label();
         productNameLabel = new System.Windows.Forms.Label();
         prodcutLabel = new System.Windows.Forms.Label();
         versionLabel = new System.Windows.Forms.Label();
         copyrightsLabel = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.SuspendLayout();
         // 
         // okButton
         // 
         okButton.Anchor = System.Windows.Forms.AnchorStyles.None;
         okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         okButton.Location = new System.Drawing.Point(258,168);
         okButton.Name = "okButton";
         okButton.Size = new System.Drawing.Size(75,23);
         okButton.TabIndex = 24;
         okButton.Text = "&OK";
         okButton.Click += new System.EventHandler(this.OnOK);
         // 
         // companyNameLabel
         // 
         companyNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
         companyNameLabel.Location = new System.Drawing.Point(347,20);
         companyNameLabel.Margin = new System.Windows.Forms.Padding(6,0,3,0);
         companyNameLabel.MaximumSize = new System.Drawing.Size(0,17);
         companyNameLabel.Name = "companyNameLabel";
         companyNameLabel.Size = new System.Drawing.Size(0,17);
         companyNameLabel.TabIndex = 22;
         companyNameLabel.Text = "IDesign Inc.";
         companyNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // copyrightLabel
         // 
         copyrightLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
         copyrightLabel.Location = new System.Drawing.Point(347,3);
         copyrightLabel.Margin = new System.Windows.Forms.Padding(6,0,3,0);
         copyrightLabel.MaximumSize = new System.Drawing.Size(0,17);
         copyrightLabel.Name = "copyrightLabel";
         copyrightLabel.Size = new System.Drawing.Size(0,17);
         copyrightLabel.TabIndex = 21;
         copyrightLabel.Text = "©  2005 IDesign Inc. All rights reserved ";
         copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // productNameLabel
         // 
         productNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
         productNameLabel.Location = new System.Drawing.Point(347,-31);
         productNameLabel.Margin = new System.Windows.Forms.Padding(6,0,3,0);
         productNameLabel.MaximumSize = new System.Drawing.Size(0,17);
         productNameLabel.Name = "productNameLabel";
         productNameLabel.Size = new System.Drawing.Size(0,17);
         productNameLabel.TabIndex = 19;
         productNameLabel.Text = "Credentials Manager";
         productNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // prodcutLabel
         // 
         prodcutLabel.AutoSize = true;
         prodcutLabel.Location = new System.Drawing.Point(257,11);
         prodcutLabel.Name = "prodcutLabel";
         prodcutLabel.Size = new System.Drawing.Size(100,13);
         prodcutLabel.TabIndex = 27;
         prodcutLabel.Text = "Credentials Manager";
         // 
         // pictureBox1
         // 
         this.pictureBox1.AutoSize = true;
         this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.pictureBox1.Image = CredentialsManagerClient.Properties.Resources.About;
         this.pictureBox1.Location = new System.Drawing.Point(12,9);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(222,182);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.pictureBox1.TabIndex = 26;
         this.pictureBox1.TabStop = false;
         // 
         // versionLabel
         // 
         versionLabel.AutoSize = true;
         versionLabel.Location = new System.Drawing.Point(257,33);
         versionLabel.Name = "versionLabel";
         versionLabel.Size = new System.Drawing.Size(56,13);
         versionLabel.TabIndex = 28;
         versionLabel.Text = "Version 1.0";
         // 
         // copyrightsLabel
         // 
         copyrightsLabel.AutoSize = true;
         copyrightsLabel.Location = new System.Drawing.Point(257,57);
         copyrightsLabel.Name = "copyrightsLabel";
         copyrightsLabel.Size = new System.Drawing.Size(188,13);
         copyrightsLabel.TabIndex = 29;
         copyrightsLabel.Text = "© 2005 IDesign Inc. All rights reserved ";
         // 
         // m_LinkLabel
         // 
         this.m_LinkLabel.AutoSize = true;
         this.m_LinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
         this.m_LinkLabel.Location = new System.Drawing.Point(257,77);
         this.m_LinkLabel.Name = "m_LinkLabel";
         this.m_LinkLabel.Size = new System.Drawing.Size(81,13);
         this.m_LinkLabel.TabIndex = 30;
         this.m_LinkLabel.TabStop = true;
         this.m_LinkLabel.Text = "www.idesign.net";
         this.m_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addressLabel_LinkClicked);
         // 
         // AboutBox
         // 
         this.AcceptButton = okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(445,203);
         this.Controls.Add(this.m_LinkLabel);
         this.Controls.Add(copyrightsLabel);
         this.Controls.Add(versionLabel);
         this.Controls.Add(prodcutLabel);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(okButton);
         this.Controls.Add(companyNameLabel);
         this.Controls.Add(copyrightLabel);
         this.Controls.Add(productNameLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "AboutBox";
         this.Padding = new System.Windows.Forms.Padding(9);
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "About Credentials Manager";
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.LinkLabel m_LinkLabel;
   }
}
