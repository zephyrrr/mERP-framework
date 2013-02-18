using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Feng.Windows.Forms
{
    public partial class CodeForm : Form
    {
        public CodeForm()
        {
            InitializeComponent();

            新建NToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.New.png").Reference;
            打开OToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Open.png").Reference;
            保存SToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Save.png").Reference;
            复制CToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Copy.png").Reference;
            粘贴PToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Paste.png").Reference;
            剪切UToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Cut.png").Reference;

            tsbRun.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.StartWithoutDebug.png").Reference;
            tsbDebug.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.Start.png").Reference;
            tsbStepInto.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.StepInto.png").Reference;
            tsbStepOver.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.StepOver.png").Reference;
            tsbStepOut.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.StepOut.png").Reference;
            tsbStop.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Debug.Stop.png").Reference;

            if (!SupportDebug)
            {
                tsbDebug.Visible = false;
                tsbStepInto.Visible = false;
                tsbStepOut.Visible = false;
                tsbStepOver.Visible = false;
            }
        }

        protected virtual string[] KeyWords
        {
            get { return new string[] {}; }
        }
        protected virtual string DefaultCode
        {
            get { return string.Empty; }
        }

        private void SetSyntax()
        {
            foreach (string keyWord in KeyWords)
            {
                txtCode.Settings.Keywords.Add(keyWord);
            }
            txtCode.Settings.KeywordColor = Color.Blue;
            txtCode.Settings.Comment = "//";

            txtCode.CompileKeywords();
        }

        protected virtual bool SupportDebug
        {
            get { return false; }
        }

        protected virtual void InitializeHost()
        {

        }
        protected virtual void ShutdownHost()
        {
        }
        private void CodeForm_Load(object sender, EventArgs e)
        {
            InitializeHost();

            SetSyntax();

            this.txtCode.Text = DefaultCode; 

            this.IsRuning = false;
        }

        private void CodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShutdownHost();
        }

        private bool m_isRuning = true;
        protected bool IsRuning
        {
            get { return m_isRuning; }
            set
            {
                if (m_isRuning != value)
                {
                    m_isRuning = value;

                    tsbRun.Enabled = !m_isRuning;
                    tsbDebug.Enabled = !m_isRuning;
                    tsbStepInto.Enabled = m_isRuning;
                    tsbStepOver.Enabled = m_isRuning;
                    tsbStepOut.Enabled = m_isRuning;
                    tsbStop.Enabled = m_isRuning;

                    RemoveHightlight();
                }
            }
        }

        protected virtual void DoRun(string code)
        {
        }
        protected virtual void DoStop()
        {
        }

        private void Run()
        {
            if (this.IsRuning)
                return;

            this.IsRuning = true;

            txtResult.Clear();
            string code = txtCode.Text;

            DoRun(code);
        }

        private void tsbDebug_Click(object sender, EventArgs e)
        {
            m_debugMode = DebugMode.StepInto;
            Run();
        }

        private void tsbRun_Click(object sender, EventArgs e)
        {
            m_debugMode = DebugMode.None;
            Run();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            DoStop();
        }

        protected void WriteStatus(string s)
        {
            dbgStatus.Text = s;
        }

        protected void WriteResult(string s)
        {
            if (string.IsNullOrEmpty(s))
                return;

            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => WriteResult(s)));
            }
            else
            {
                if (txtResult.IsHandleCreated)
                {
                    txtResult.AppendText(s);
                }
            }
        }


        private int m_oldHightlightLine = -1;
        private void RemoveHightlight()
        {
            if (m_oldHightlightLine != -1)
            {
                int idx = txtCode.GetFirstCharIndexFromLine(m_oldHightlightLine);
                txtCode.SelectionStart = idx;
                txtCode.SelectionLength = txtCode.Lines[m_oldHightlightLine].Length;
                txtCode.SelectionBackColor = Color.White;

                m_oldHightlightLine = -1;
            }

            txtCode.SelectionLength = 0;
        }
        protected void HighlightLine(int linenum, Brush foreground, Brush background)
        {
            linenum -= 1;
            if (linenum >= 0 && linenum < txtCode.Lines.Length)
            {
                m_oldHightlightLine = linenum;

                int idx = txtCode.GetFirstCharIndexFromLine(linenum);
                txtCode.SelectionStart = idx;
                txtCode.SelectionLength = txtCode.Lines[linenum].Length;
                txtCode.SelectionBackColor = Color.Red;
            }
        }

        protected virtual void DoStep()
        {
        }

        private void ExecuteStep()
        {
            dbgStatus.Text = "Running";

            RemoveHightlight();

            DoStep();
        }

        protected enum DebugMode
        {
            None = 0,
            StepInto = 1,
            SetpOver = 2,
            StepOut = 3
        }

        private DebugMode m_debugMode = DebugMode.None;
        protected DebugMode CurrentDebugMode
        {
            get { return m_debugMode; }
        }

        private void tsbStepIn_Click(object sender, EventArgs e)
        {
            if (!this.IsRuning)
                return;

            m_debugMode = DebugMode.StepInto;
            ExecuteStep();
        }

        private void tsbStepOver_Click(object sender, EventArgs e)
        {
            if (!this.IsRuning)
                return;

            m_debugMode = DebugMode.SetpOver;
            ExecuteStep();
        }

        private void tsbStepOut_Click(object sender, EventArgs e)
        {
            if (!this.IsRuning)
                return;

            m_debugMode = DebugMode.StepOut;
            ExecuteStep();
        }


        private string m_fileName;
        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            m_fileName = null;
            txtCode.ResetText();
        }

        private const string m_fileFilter = "All files (*.*)|*.*";
        protected virtual string FileFilter
        {
            get { return m_fileFilter; }
        }
        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_fileName))
            {
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                {
                    openFileDialog1.RestoreDirectory = true;
                    openFileDialog1.Filter = this.FileFilter;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        m_fileName = openFileDialog1.FileName;
                    }
                }
            }
            if (!string.IsNullOrEmpty(m_fileName))
            {
                txtCode.Text = System.IO.File.ReadAllText(m_fileName);

                txtCode.ProcessAllLines();
            }
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_fileName))
            {
                using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                {
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.Filter = this.FileFilter;
                    //saveFileDialog1.Title = "保存";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        m_fileName = saveFileDialog1.FileName;
                    }
                }
            }
            if (!string.IsNullOrEmpty(m_fileName))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_fileName))
                {
                    sw.Write(txtCode.Text);
                }
            }
        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            txtCode.Cut();
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            txtCode.Copy();
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            txtCode.Paste();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Run();
            }
        }
    }
}
