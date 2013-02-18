using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Scripting.Hosting.Shell;
using Microsoft.Scripting.Hosting;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Script Form
    /// </summary>
    public abstract partial class ScriptForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageName"></param>
        public ScriptForm(Feng.Scripts.Host host)
        {
            InitializeComponent();
            打开OToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Open.png").Reference;
            保存SToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Save.png").Reference;
            复制CToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Copy.png").Reference;
            粘贴PToolStripButton.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.Document.Paste.png").Reference;


            this.txtScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            this.txtScript.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);

            txtScript.BackColor = _backColor;
            txtScript.ForeColor = _inputColor;
            txtScript.ShortcutsEnabled = true;

            m_host = host;
            _history = new CommandHistory();

            ResetConsole();

            m_host.Scope.SetVariable("scriptTextBox", this.txtScript);
            m_host.Runtime.Globals.SetVariable("prompt", PromptString);
        }

        private const string m_promptString = ">>>";
        protected virtual string PromptString
        {
            get { return m_promptString; }
        }
        private const string m_promptContinuationString = "...";
        protected virtual string PromptContinuationString
        {
            get { return m_promptContinuationString; }
        }
        protected virtual string InitializeCommand
        {
            get { return string.Empty; }
        }

        private const string s_newLine = "\n";
        private bool _inMultiLineEdit = false;
        private int _lastPromptPosition;
        Feng.Scripts.Host m_host;
        CommandHistory _history;

        public Feng.Scripts.Host Host
        {
            get { return m_host; }
        }

        private static Color _backColor = Color.Black;
        private static Color _inputColor = Color.Yellow;
        private static Color _promptColor = Color.Gray;
        private static Color _outColor = Color.Cyan;
        private static Color _errorColor = Color.Red;
        private static Color _warningColor = Color.Purple;


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is RichTextBox)
            {
                if (HandleRTFKeyPress(e))
                    return;
            }
            if ((int)e.KeyChar == (int)Keys.Escape)
            {
                this.Close();
            }
        }
        private void DoPaste()
        {
            string text = Feng.Windows.Utils.ClipboardHelper.GetTextFromClipboard();
            DoPaste(text);
        }

        private void DoPaste(string text)
        {
            //MessageBox.Show(txtScript.SelectionStart.ToString() + ", " + (txtScript.TextLength - 1).ToString());
            if (txtScript.SelectionStart == txtScript.TextLength)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    string[] cmds = text.Split(new string[] { s_newLine, "\r" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < cmds.Length; ++i)
                    {
                        string cmd = cmds[i];
                        WriteColor(cmd, _inputColor);

                        if (i != cmds.Length - 1)
                        {
                            if (!cmd.EndsWith(s_newLine))
                            {
                                WriteLine();
                            }
                            HandleEnterKey();
                        }
                    }
                }
            }
            else
            {
                txtScript.SelectedText = text;
            }
        }
        private bool optionsObsolete = false;
        private string m_tabPrefix = null;
        private bool _prepareQuit;
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //// disable ctrl+c/ctrl-v/ctrl-x
            //if (e.Modifiers == Keys.Control)
            //{
            //    switch (e.KeyCode)
            //    {
            //        // you can add what ever keys you want to handle here
            //        case Keys.C:
            //        case Keys.X:
            //        case Keys.V:
            //        case Keys.Z:
            //            e.Handled = true;
            //            break;
            //        default:
            //            break;
            //    }
            //}

            // Exist
            if (e.Control && e.KeyCode == Keys.Z)
            {
                _prepareQuit = true;
                return;
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                DoPaste();

                e.Handled = e.SuppressKeyPress = true;
                return;
            }
            switch (e.KeyData)
            {
                case Keys.Tab:
                    {
                        if (optionsObsolete)
                        {
                            m_tabPrefix = GetOptions();
                            _currentTabedCmd = null;
                            optionsObsolete = false;
                        }
                        
                        // Displays the next option in the option list,
                        // or beeps if no options available for current input prefix.
                        // If no input prefix, simply print tab.
                        DisplayNextOption(e, m_tabPrefix);
                        e.Handled = e.SuppressKeyPress = true;
                    }
                    break;
                case Keys.Enter:
                    // First KeyDown then KeyPress
                    if (_prepareQuit)
                    {
                        this.Close();
                        return;
                    }
                    txtScript.SelectionStart = txtScript.TextLength;
                    optionsObsolete = true;
                    break;
                case Keys.Down:
                    if (!_inMultiLineEdit)
                    {
                        CurrentCommand = _history.GetNext();
                        e.Handled = e.SuppressKeyPress = true;
                        optionsObsolete = true;
                    }
                    break;
                case Keys.Up:
                    if (!_inMultiLineEdit)
                    {
                        CurrentCommand = _history.GetPrevious();
                        e.Handled = e.SuppressKeyPress = true;
                        optionsObsolete = true;
                    }
                    break;
                case Keys.Home:
                    txtScript.SelectionStart = 0;
                    e.SuppressKeyPress = e.Handled = true;
                    optionsObsolete = true;
                    break;
                case Keys.End:
                    txtScript.SelectionStart = txtScript.TextLength;
                    e.SuppressKeyPress = e.Handled = true;
                    optionsObsolete = true;
                    break;
                case Keys.Back:
                    HandleBackSpace(e);
                    optionsObsolete = true;
                    break;
                default:
                    if (!IsKeyAlwaysAllowed(e))
                        SuppressKeyIfCurrentSelIsReadOnly(e);
                    optionsObsolete = true;
                    break;
            }

            _prepareQuit = false;
        }

        private bool HandleRTFKeyPress(KeyPressEventArgs e)
        {
            switch ((Keys)(int)e.KeyChar)
            {
                case Keys.Enter:
                    HandleEnterKey();
                    return true;
            }
            return false;
        }

        private void ProcessCurrentCommand(string command)
        {
            _history.Add(command);
            string output = m_host.ExecuteInCurrentScope(command);
            if (output != null)
            {
                Write(output, Style.Out);
            }
            else
            {
                //probably an error
                WriteLine(m_host.ErrorFromLastExecution, Style.Error);
            }
        }

        protected virtual bool IsCommandComplete
        {
            get { return true; }
        }

        private void HandleEnterKey()
        {
            if (!_inMultiLineEdit)
            {
                if (!CurrentCommand.IsNullOrEmpty())
                {
                    if (!this.IsCommandComplete)
                    {
                        _inMultiLineEdit = true;
                        ShowPromptContinuation();
                        return;
                    }
                    ProcessCurrentCommand(CurrentCommand);
                }
                ShowPrompt();
            }
            else
            {
                string curLine = GetLastLine(CurrentCommand);
                if (curLine.IsNullOrEmptyOrWhiteSpace())
                {
                    _inMultiLineEdit = false;
                    ProcessCurrentCommand(CurrentCommand);
                    ShowPrompt();
                }
                else
                {
                    ShowPromptContinuation();
                }
            }
        }

        private string GetLastLine(string contents)
        {
            //return rtb.Lines.Last();

            int idxLast = contents.LastIndexOf(s_newLine);
            Debug.Assert(idxLast != -1);

            int idxPrev = contents.LastIndexOf(s_newLine, idxLast - 1);
            Debug.Assert(idxPrev != -1);
            Debug.Assert(idxPrev < idxLast);

            //hell\nwo\n

            return contents.Substring(idxPrev + 1, idxLast - idxPrev - 1);
        }

        protected string CurrentCommand
        {
            get
            {
                string code = txtScript.Text.Substring(_lastPromptPosition);
                if (code.IsNullOrEmpty() || code.IsWhiteSpace())
                {
                    return string.Empty;
                }
                string s;
                if (_inMultiLineEdit)
                {
                    s = code.TrimEnd(new char[] { ' ' });
                }
                else
                {
                    s = code.TrimEnd(new char[] { ' ', '\n', '\r' });
                }
                //remove promptContinue
                return s.Replace(s_newLine + this.PromptContinuationString, s_newLine);
            }
            set
            {
                txtScript.SelectionStart = _lastPromptPosition;
                txtScript.SelectionLength = txtScript.TextLength - _lastPromptPosition;

                txtScript.SelectedText = value;
            }
        }

        private void HandleBackSpace(KeyEventArgs e)
        {
            if (txtScript.SelectionStart <= _lastPromptPosition)
            {
                e.Handled = e.SuppressKeyPress = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private bool IsKeyAlwaysAllowed(KeyEventArgs e)
        {
            var keyCode = e.KeyCode;

            //CTRL-C is always allowed
            if (
                (keyCode == Keys.C) && (e.Control == true) ||
                ((keyCode >= Keys.PageUp) && (keyCode <= Keys.Down)) ||
                (keyCode == Keys.Escape))
            {
                return true;
            }

            return false;
        }

        private void SuppressKeyIfCurrentSelIsReadOnly(KeyEventArgs e)
        {
            if (txtScript.SelectionStart < _lastPromptPosition)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        ///// <summary>
        ///// Not yet implemented. Currently it just adds the text
        ///// </summary>
        //private void AddToRTBWithFormatting(RichTextBox rtb, string text, Color color, bool bold)
        //{
        //    rtb.Text += text;
        //}

        //private void AddToConsole(string message, bool addNewLine)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(message);

        //    rtb1.Text += sb.ToString();
        //}

        //private void AddToConsole(string message)
        //{
        //    AddToConsole(message, true);
        //}

        //private void AddErrorToConsole(string error)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(ERROR_PROMPT);
        //    sb.AppendLine(error);

        //    rtb1.Text += sb.ToString();
        //}

        public void WriteLine(string text, Style style)
        {
            Write(text + Environment.NewLine, style);
        }

        private void WriteLine()
        {
            Write(Environment.NewLine, Style.Out);
        }

        private void Write(string text, Style style)
        {
            switch (style)
            {
                case Style.Prompt: WriteColor(text, _promptColor); break;
                case Style.Out: WriteColor(text, _outColor); break;
                case Style.Error: WriteColor(text, _errorColor); break;
                case Style.Warning: WriteColor(text, _warningColor); break;
            }
        }
        private void Remove(int len)
        {
            if (txtScript.Text.Length < len)
            {
                txtScript.ResetText();
            }
            else
            {
                txtScript.Select(txtScript.Text.Length - len, len);
                txtScript.SelectedText = string.Empty;
            }
        }
        private void WriteColor(string str, Color c)
        {
            int length = txtScript.TextLength;
            txtScript.AppendText(str);
            txtScript.SelectionStart = length;
            txtScript.SelectionLength = str.Length;
            txtScript.SelectionColor = c;

            txtScript.SelectionLength = 0;
            txtScript.SelectionStart = txtScript.TextLength;
            
            txtScript.ClearUndo();
        }

        private void ResetConsole()
        {
            txtScript.Clear();
            ShowPrompt();
            Write(m_host.GetLogoDisplay(), Style.Out);

            ShowPrompt();

            if (!string.IsNullOrEmpty(this.InitializeCommand))
            {
                WriteColor(this.InitializeCommand, _inputColor);
                HandleEnterKey();
            }
        }

        private void ShowPromptContinuation()
        {
            string contents = txtScript.Text;
            if ((contents.Length != 0) && !contents.EndsWith(s_newLine))
            {
                txtScript.AppendText(s_newLine);
            }

            Write(this.PromptContinuationString, Style.Prompt);

            txtScript.SelectionStart = txtScript.TextLength;

            txtScript.SelectionColor = _inputColor;
        }

        private void ShowPrompt()
        {
            string contents = txtScript.Text;
            if ((contents.Length != 0) && !contents.EndsWith(s_newLine))
            {
                txtScript.AppendText(s_newLine);
            }

            Write(this.PromptString, Style.Prompt);

            txtScript.SelectionStart = txtScript.TextLength;

            _lastPromptPosition = txtScript.SelectionStart;

            txtScript.SelectionColor = _inputColor;
        }

        #region "AutoComplete"
        // class.property, variable, class.method(variable)
        private string GetOptions()
        {
            _options.Clear();

            StringBuilder _input = new StringBuilder();
            string prefix = CurrentCommand;
            _input.Append(prefix);

            int len;
            for (len = _input.Length; len > 0; len--)
            {
                char c = _input[len - 1];
                if (Char.IsLetterOrDigit(c))
                {
                    continue;
                }
                else if (c == '.' || c == '_')
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            int rootidx = 0;
            for (int i = len; i > 0; i--)
            {
                char c = _input[i - 1];
                if (c == ' ' || c == '(')
                {
                    rootidx = i;
                    break;
                }
            }


            string name = _input.ToString(len, _input.Length - len);
            if (name.Trim().Length > 0)
            {
                int lastDot = name.LastIndexOf('.');
                string attr, pref, root;
                if (lastDot < 0)
                {
                    attr = String.Empty;
                    pref = name;
                    root = _input.ToString(rootidx, len - rootidx);
                }
                else
                {
                    attr = name.Substring(0, lastDot);
                    pref = name.Substring(lastDot + 1);
                    root = _input.ToString(rootidx, len + lastDot + 1 - rootidx);
                }

                try
                {
                    IList<string> result;
                    if (String.IsNullOrEmpty(attr))
                    {
                        result = GetGlobals(name);
                    }
                    else
                    {
                        result = GetMemberNames(attr);
                    }

                    _options.Root = root;
                    foreach (string option in result)
                    {
                        if (option.StartsWith(pref, StringComparison.CurrentCultureIgnoreCase))
                        {
                            _options.Add(option);
                        }
                    }
                }
                catch
                {
                    _options.Clear();
                }

                if (lastDot < 0)
                {
                    return root + name;
                }
                else
                {
                    return name;
                }
            }
            else
            {
                return null;
            }
        }

        private string _currentTabedCmd;

        /// <summary>
        /// Displays the next option in the option list,
        /// or beeps if no options available for current input prefix.
        /// If no input prefix, simply print tab.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefix"></param>
        private void DisplayNextOption(KeyEventArgs e, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                InsertTab();
            }
            else
            {
                if (_options.Count > 0)
                {
                    string part = (e.Modifiers & Keys.Shift) != 0 ? _options.Previous() : _options.Next();
                    if (string.IsNullOrEmpty(_currentTabedCmd))
                    {
                        Remove(prefix.Length);
                    }
                    else
                    {
                        Remove(_currentTabedCmd.Length);
                    }
                    _currentTabedCmd = _options.Root + part;
                    WriteColor(_currentTabedCmd, _inputColor);
                }
                else
                {
                    //Console.Beep();
                }
            }
        }

        public IList<string> GetGlobals(string name)
        {
            List<string> res = new List<string>();
            foreach (string scopeName in m_host.Scope.GetVariableNames())
            {
                if (scopeName.StartsWith(name))
                {
                    res.Add(scopeName);
                }
            }
            IList<string> res2 = GetGlobalsOfLanguage(name);
            if (res != null)
            {
                res.AddRange(res2);
            }
            return res;
        }

        protected virtual IList<string> GetGlobalsOfLanguage(string name)
        {
            return null;
        }
        public IList<string> GetMemberNames(string code)
        {
            object value = m_host.Engine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.Expression).Execute(m_host.Scope);
            return m_host.Engine.Operations.GetMemberNames(value);
            // TODO: why doesn't this work ???
            //return _memberCompletionSite.Invoke(new CodeContext(_scope, _engine), value);
        }

        /// <summary>
        /// Tab options available in current context
        /// </summary>
        private SuperConsoleOptions _options = new SuperConsoleOptions();

        private const int TabSize = 4;
        private void InsertTab()
        {
            int haveSpace = 0;
            string cmd = CurrentCommand;
            if (!string.IsNullOrEmpty(cmd))
            {
                int idx = cmd.Length - 1;
                while (true)
                {
                    if (cmd[idx] == ' ')
                    {
                        idx--;
                    }
                    else
                    {
                        break;
                    }
                    if (idx < 0)
                    {
                        break;
                    }
                }
                haveSpace = cmd.Length - 1 - idx;
            }
            for (int i = TabSize - (haveSpace % TabSize); i > 0; i--)
            {
                WriteColor(" ", _inputColor);
            }
        }
        #endregion

        private const string m_fileFilter = "All files (*.*)|*.*";
        protected virtual string FileFilter
        {
            get { return m_fileFilter; }
        }
        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Filter = this.FileFilter;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    DoPaste(System.IO.File.ReadAllText(openFileDialog1.FileName));
                }
            }
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = this.FileFilter;
                //saveFileDialog1.Title = "保存";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (string s in _history.GetAll())
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
            }
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            txtScript.Copy();
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            DoPaste();
        }

    }

    /// <summary>
    /// List of available options
    /// </summary>
    class SuperConsoleOptions
    {
        private List<string> _list = new List<string>();
        private int _current;
        private string _root;

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        private string Current
        {
            get
            {
                return _current >= 0 && _current < _list.Count ? _list[_current] : String.Empty;
            }
        }

        public void Clear()
        {
            _list.Clear();
            _current = -1;
        }

        public void Add(string line)
        {
            if (line != null && line.Length > 0)
            {
                _list.Add(line);
            }
        }

        public string Previous()
        {
            if (_list.Count > 0)
            {
                _current = ((_current - 1) + _list.Count) % _list.Count;
            }
            return Current;
        }

        public string Next()
        {
            if (_list.Count > 0)
            {
                _current = (_current + 1) % _list.Count;
            }
            return Current;
        }

        public string Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }
    }

    internal static class Extensions
    {
        internal static bool IsNullOrEmpty(this string s)
        {
            return (s == null) || (s.Length == 0);
        }

        internal static bool IsWhiteSpace(this string s)
        {
            if (s.Trim().Trim('\n').Length == 0)
                return true;

            return false;
        }
        internal static bool IsNullOrEmptyOrWhiteSpace(this string s)
        {
            return s.IsNullOrEmpty() || s.IsWhiteSpace();

        }
    }
}
