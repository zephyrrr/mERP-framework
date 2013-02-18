// © 2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


internal class SplashForm : Form
{
   System.Windows.Forms.Timer m_Timer;
   PictureBox m_SplashPictureBox;
   private System.ComponentModel.IContainer components;
   bool m_HideSplash = false;
   
   public SplashForm(Bitmap splashImage)
   {
      InitializeComponent();
      m_SplashPictureBox.Image = splashImage;
      ClientSize = m_SplashPictureBox.Size;
   }
   #region Windows Form Designer generated code
   void InitializeComponent()
   {
      this.components = new System.ComponentModel.Container();
      this.m_SplashPictureBox = new System.Windows.Forms.PictureBox();
      this.m_Timer = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_SplashPictureBox)).BeginInit();
      this.SuspendLayout();
// 
// m_SplashPictureBox
// 
      this.m_SplashPictureBox.AutoSize = true;
      this.m_SplashPictureBox.Cursor = System.Windows.Forms.Cursors.AppStarting;
      this.m_SplashPictureBox.Location = new System.Drawing.Point(0,0);
      this.m_SplashPictureBox.Name = "m_SplashPictureBox";
      this.m_SplashPictureBox.Size = new System.Drawing.Size(112,80);
      this.m_SplashPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.m_SplashPictureBox.TabIndex = 0;
      this.m_SplashPictureBox.TabStop = false;
// 
// m_Timer
// 
      this.m_Timer.Enabled = true;
      this.m_Timer.Interval = 500;
      this.m_Timer.Tick += new System.EventHandler(this.OnTick);
// 
// SplashForm
// 
      
      this.ClientSize = new System.Drawing.Size(227,172);
      this.ControlBox = false;
      this.Controls.Add(this.m_SplashPictureBox);
      this.Cursor = System.Windows.Forms.Cursors.Cross;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "SplashForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.m_SplashPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

   }
   #endregion
   private void OnTick(object sender,EventArgs e)
   {
      if(HideSplash == true)
      {
         m_Timer.Enabled = false;
         Close();
      }
   }
   public bool HideSplash
   {
      get
      {
         lock(this)
         {
            return m_HideSplash;
         }
      }
      set
      {
         lock(this)
         {
            m_HideSplash = value;   
         }
      }
   }
}
public class SplashScreen
{
   public SplashScreen(Bitmap splash)
   {
      m_SplashImage = splash;
      ThreadStart threadStart = new ThreadStart(Show);
      m_WorkerThread = new Thread(threadStart);
      m_WorkerThread.Start();
   }
   void Show()
   {
      m_SplashForm = new SplashForm(m_SplashImage);
      m_SplashForm.ShowDialog();
   }
   public void Close()
   {
      m_SplashForm.HideSplash = true;
      m_WorkerThread.Join();
   }
   Bitmap m_SplashImage;      
   SplashForm m_SplashForm;
   Thread m_WorkerThread;
}
