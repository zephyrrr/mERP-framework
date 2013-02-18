using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Scripting.Hosting;
using System.Threading;

namespace Feng.Windows.Forms
{
    public class RubyCodeForm : CodeForm
    {
        public static readonly string[] KeyWordsPython = new string[] { 
            "alias", "and", "begin","break","case","class","def","defined?","do","else","elsif","end","ensure","false","for","if","in","module","next","nil","not","or","redo","rescue","retry","return","self","super","then","true","undef","unless","until","when","while","yield"};

        protected override string[] KeyWords
        {
            get { return KeyWordsPython; }
        }
        protected override string DefaultCode
        {
            get
            {
                return "puts \'Hello World\'";
            }
        }

        private const string m_fileFilter = "Ruby and text files (*.rb *.txt)|*.rb;*.txt";
        protected override string FileFilter
        {
            get { return m_fileFilter; }
        }

        Feng.Scripts.RubyHost _host;
        ScriptEngine _engine;
        AutoResetEvent _dbgContinue = new AutoResetEvent(false);

        //Action<TraceBackFrame, string, object> _tracebackAction;
        //TraceBackFrame _curFrame;
        //FunctionCode _curCode;

        protected override void InitializeHost()
        {
            _host = Feng.Scripts.RubyHost.Instance;
            _engine = _host.Engine;

            //_tracebackAction = new Action<TraceBackFrame, string, object>(this.OnTraceback);

            base.InitializeHost();
        }
        protected override void ShutdownHost()
        {
            //_engine.SetTrace(null);
            _dbgContinue.Set();

            base.ShutdownHost();
        }

        private System.Threading.Thread _debugThread;

        protected override void DoRun(string code)
        {
            _debugThread = new System.Threading.Thread(() =>
            {
                try
                {
                    ////DebugWindow.InitDebugWindow(_host.Engine);
                    //if (this.CurrentDebugMode != DebugMode.None)
                    //{
                    //    _engine.SetTrace(OnTracebackReceived);
                    //}
                    //else
                    //{
                    //    _engine.SetTrace(null);
                    //}

                    string output = _host.ExecuteInCurrentScope(code, Microsoft.Scripting.SourceCodeKind.File);
                    ShowOutput(output);
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.IsRuning = false;
                    WriteStatus("Idle");
                }
            });
            _debugThread.IsBackground = true;
            _debugThread.SetApartmentState(System.Threading.ApartmentState.STA);
            _debugThread.Start();
        }

        protected override void DoStop()
        {
            if (this.IsRuning && _debugThread != null)
            {
                _debugThread.Abort();
            }
        }
        private void ShowOutput(string output)
        {
            if (output != null)
            {
                WriteResult(output);
            }
            else
            {
                WriteResult(_host.ErrorFromLastExecution);
            }
        }
        //private void TracebackCall()
        //{
        //    this.WriteStatus(string.Format("Call {0}", _curCode.co_name));
        //    if (_curFrame.f_back != null)
        //    {
        //        HighlightLine((int)_curFrame.f_back.f_lineno, Brushes.LightGreen, Brushes.Black);
        //    }
        //    else
        //    {
        //        HighlightLine((int)_curFrame.f_lineno, Brushes.LightGreen, Brushes.Black);
        //    }
        //}

        //private void TracebackReturn()
        //{
        //    this.WriteStatus(string.Format("Return {0}", _curCode.co_name));
        //    if (_curFrame.f_back != null)
        //    {
        //        HighlightLine((int)_curFrame.f_back.f_lineno, Brushes.LightPink, Brushes.Black);
        //    }
        //    else
        //    {
        //        HighlightLine((int)_curFrame.f_lineno, Brushes.LightPink, Brushes.Black);
        //    }
        //}

        //private void TracebackLine()
        //{
        //    this.WriteStatus(string.Format("Line {0}", _curFrame.f_lineno));
        //    HighlightLine((int)_curFrame.f_lineno, Brushes.Yellow, Brushes.Black);
        //}


        //private void OnTraceback(TraceBackFrame frame, string result, object payload)
        //{
        //    var code = (FunctionCode)frame.f_code;
        //    if (_curCode == null || _curCode.co_filename != code.co_filename)
        //    {
        //        //_source.Inlines.Clear();
        //        //foreach (var line in System.IO.File.ReadAllLines(code.co_filename))
        //        //{
        //        //    _source.Inlines.Add(new Run(line + "\r\n"));
        //        //}
        //    }

        //    //_host.TraceBackFrame = frame;
        //    _curFrame = frame;
        //    _curCode = code;

        //    switch (result)
        //    {
        //        case "call":
        //            TracebackCall();
        //            break;

        //        case "line":
        //            TracebackLine();
        //            break;

        //        case "return":
        //            TracebackReturn();
        //            break;

        //        default:
        //            System.Windows.Forms.MessageBox.Show(string.Format("{0} not supported!", result));
        //            break;
        //    }

        //    ShowOutput(_host.ReadOutput());
        //}

        //private TracebackDelegate OnTracebackReceived(TraceBackFrame frame, string result, object payload)
        //{
        //    //WriteResult(result);

        //    switch (this.CurrentDebugMode)
        //    {
        //        case DebugMode.StepInto:
        //            break;
        //        case DebugMode.SetpOver:
        //            if (result == "call")
        //                return null;
        //            break;
        //        case DebugMode.StepOut:
        //            if (result == "call")
        //                return null;
        //            else if (result == "line")
        //                return this.OnTracebackReceived;
        //            break;
        //        default:
        //            throw new InvalidOperationException();
        //    }

        //    this.BeginInvoke(_tracebackAction, frame, result, payload);

        //    _dbgContinue.WaitOne();
        //    return this.OnTracebackReceived;
        //}

        protected override void DoStep()
        {
            _dbgContinue.Set();
        }
    }
}