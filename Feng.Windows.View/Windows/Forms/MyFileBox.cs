using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyFileBox : UserControl, IDataValueControl, IFormatControl
    {
        string IFormatControl.Format
        {
            get { return this.AssociateControlName; }
            set { this.AssociateControlName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyFileBox()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (!this.ReadOnly)
            {
                toolStripMenuItemModify_Click(toolStripMenuItemModify, System.EventArgs.Empty);
            }
            else
            {
                toolStripMenuItemOpen_Click(toolStripMenuItemSave, System.EventArgs.Empty);
            }
        }

        #region "IDataValueControl"
        private IDataControl m_associateControl = null;
        private string m_associateControlName;
        private IDataControl AssociateControl
        {
            get
            {
                if (!m_haveFound)
                {
                    if (!string.IsNullOrEmpty(m_associateControlName))
                    {
                        IDisplayManagerContainer dmc = this.FindForm() as IDisplayManagerContainer;
                        if (dmc != null && dmc.DisplayManager.DataControls[m_associateControlName] != null)
                        {
                            m_associateControl = dmc.DisplayManager.DataControls[m_associateControlName];
                        }
                        else
                        {
                            m_associateControl = null;
                        }
                    }
                    else
                    {
                        m_associateControl = null;
                    }
                    // 不然不能保存
                    //if (m_associateControl != null)
                    //{
                    //    m_associateControl.ReadOnly = true;
                    //}
                    m_haveFound = true;
                }
                return m_associateControl;
            }
        }

        private bool m_haveFound;

        /// <summary>
        /// AssociateControl
        /// </summary>
        public string AssociateControlName
        {
            get { return m_associateControlName; }
            set
            {
                if (m_associateControlName != value)
                {
                    m_associateControlName = value;
                    m_haveFound = false;
                }
            }
        }

        /// <summary>
        /// PictureBox.Picture, byte[] 类型
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get { return m_data; }
            set
            {
                if (value == null)
                {
                    m_data = null;
                }
                else
                {
                    try
                    {
                        byte[] data = (byte[]) value;
                        m_data = data;
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        throw new ArgumentException("MyFileBox's SelectedDataValue must be valid file stream(byte[])", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        private bool m_bReadOnly;

        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        [Category("Data")]
        [Description("是否可读")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                if (m_bReadOnly != value)
                {
                    m_bReadOnly = value;

                    //btnBrowser.Enabled = !value;
                    btnBrowser.Text = value ? "打开" : "浏览";

                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion

        #region "Operations"

        private byte[] m_data;

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            SelectedDataValue = null;
            this.AssociateControl.SelectedDataValue = null;
        }

        private void toolStripMenuItemModify_Click(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;
            //ofd.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "所有文件 (*.*)|*.*|Word 文档 (*.Doc)|*.Doc";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream myStream;
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    FileInfo fi = new FileInfo(openFileDialog1.FileName);
                    int len = Convert.ToInt32(fi.Length);
                    byte[] data = new byte[len];
                    myStream.Read(data, 0, len);
                    myStream.Close();

                    SelectedDataValue = data;

                    if (this.AssociateControl != null)
                    {
                        this.AssociateControl.SelectedDataValue = openFileDialog1.SafeFileName;
                    }
                }
            }
            openFileDialog1.Dispose();
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            if (this.m_data != null)
            {
                string defaultFileName = null;
                if (this.AssociateControl != null && this.AssociateControl.SelectedDataValue != null)
                {
                    defaultFileName = this.AssociateControl.SelectedDataValue.ToString();
                }
                string defaultExtention = Path.GetExtension(defaultFileName);
                if (string.IsNullOrEmpty(defaultExtention))
                {
                    defaultExtention = "*";
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = string.Format("所有文件 (*{0})|*{0}", defaultExtention);
                //saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.Title = "保存";
                if (this.AssociateControl != null && this.AssociateControl.SelectedDataValue != null)
                {
                    saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(defaultFileName);
                }
                saveFileDialog1.ShowDialog();

                if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    System.IO.FileStream fs = (System.IO.FileStream) saveFileDialog1.OpenFile();
                    fs.Write(m_data, 0, m_data.Length);

                    fs.Close();
                }
                saveFileDialog1.Dispose();
            }
        }

        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            if (this.m_data != null)
            {
                string name = "temp.doc";
                if (this.AssociateControl != null && this.AssociateControl.SelectedDataValue != null)
                {
                    name = this.AssociateControl.SelectedDataValue.ToString();
                }
                string fileName = Path.GetTempPath() + name;
                System.IO.FileStream fs = new FileStream(fileName, FileMode.Create);
                fs.Write(m_data, 0, m_data.Length);
                fs.Close();
                ProcessHelper.ExecuteApplication(fileName);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemModify.Enabled = !m_bReadOnly;
            toolStripMenuItemClear.Enabled = !m_bReadOnly;

            toolStripMenuItemSave.Enabled = (this.m_data != null);
            toolStripMenuItemOpen.Enabled = (this.m_data != null);
        }

        #endregion
    }
}