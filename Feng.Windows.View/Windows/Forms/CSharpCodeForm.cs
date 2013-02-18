using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;

namespace Feng.Windows.Forms
{
    public class CSharpCodeForm : CodeForm
    {
        public static string[] KeyWordsCSharp = { "abstract", "event", "new", "struct", "as", "explicit", "null", "switch", 
                                                    "base", "extern", "object", "this", "bool", "false", "operator", "throw", 
                                                    "break", "finally", "out", "true", "byte", "fixed", "override", "try", 
                                                    "case", 	"float",  	"params", 	"typeof",
                                                    "catch", 	"for", 	"private", 	"uint",
                                                    "char", 	"foreach", 	"protected", 	"ulong",
                                                    "checked", 	"goto", 	"public", 	"unchecked",
                                                    "class", 	"if", 	"readonly", 	"unsafe",
                                                    "const", 	"implicit", 	"ref", 	"ushort",
                                                    "continue", 	"in", 	"return", 	"using",
                                                    "decimal", 	"int", 	"sbyte", 	"virtual",
                                                    "default", 	"interface", 	"sealed", 	"volatile",
                                                    "delegate", 	"internal", 	"short", 	"void",
                                                    "do", 	"is", 	"sizeof", 	"while",
                                                    "double", 	"lock", 	"stackalloc",	 
                                                    "else", 	"long", 	"static ",	 
                                                    "enum", 	"namespace", 	"string" };

        protected override string[] KeyWords
        {
            get { return KeyWordsCSharp; }
        }
        protected override string DefaultCode
        {
            get
            {
                return "Console.WriteLine(\"Hello world\");";
            }
        }
        private const string m_fileFilter = "cs and text files (*.cs *.txt)|*.cs;*.txt";
        protected override string FileFilter
        {
            get { return m_fileFilter; }
        }
        private System.IO.StringWriter m_output = new System.IO.StringWriter();
        protected override void InitializeHost()
        {
            LoadSnippetOptions();
            Console.SetOut(m_output);

            base.InitializeHost();
        }

        private SnippetOptions CurrentOptions;
        private void LoadSnippetOptions()
        {
            foreach (SnippetOptions options in SnippetOptions.LoadAvailableOptions())
            {
                CurrentOptions = options;
                break;
            }
        }

        protected override void DoRun(string code)
        {
            m_output.GetStringBuilder().Remove(0, m_output.GetStringBuilder().Length);

            try
            {
                Snippet snippet = new Snippet(code, CurrentOptions);
                CompilerResults results = snippet.Compile();

                StringBuilder sb = new StringBuilder();
                if (results.Errors.HasErrors)
                {
                    sb.Append("(Build failed)");
                    sb.Append(System.Environment.NewLine);
                    foreach (CompilerError error in results.Errors)
                    {
                        sb.Append(error.Line + ": " + error.ErrorText);
                        sb.Append(System.Environment.NewLine);
                    }
                }
                else
                {
                    sb.Append("(Build successful)");
                    sb.Append(System.Environment.NewLine);

                    Type snippetType = results.CompiledAssembly.GetType("Snippet");
                    try
                    {
                        snippetType.GetMethod("Main").Invoke(null, new object[] { new string[] { } });

                        m_output.Flush();
                        if (m_output.GetStringBuilder().Length > 0)
                        {
                            sb.Append(m_output.GetStringBuilder());
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        sb.Append(string.Format("Exception: {0}", ex.InnerException));
                    }
                }

                WriteResult(sb.ToString());
            }
            finally
            {
                this.IsRuning = false;
                WriteStatus("Idle");
            }
        }

    }
}
