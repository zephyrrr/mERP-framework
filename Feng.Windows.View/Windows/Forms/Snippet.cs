using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Represents a snippet of code - typically the text within the text box
    /// of the UI
    /// </summary>
    public sealed class Snippet
    {
        const string ClassPrologue="\r\npublic class Snippet\r\n{\r\n";
        const string MainPrologue="    [STAThread]\r\n    public static void Main(string[] args)\r\n    {\r\n";
        const string ClassEpilogue = "    }\r\n}\r\n";
        const string UsingFormat = "using {0};\r\n";
        const string LinePragmaFormat = "#line {0}\r\n";
        const string LineFormat = "        {0}\r\n";

        string snippetText;
        SnippetOptions options;

        /// <summary>
        /// Generates a snippet from the given text, with the given options.
        /// </summary>
        public Snippet(string snippetText, SnippetOptions options)
        {
            this.snippetText = snippetText;
            this.options = options;
        }

        static readonly Regex AnnotationExpression = new Regex(@" *\|?#[A-Z0-9]\s*$");

        /// <summary>
        /// Removes the annotations from each line of code. These are of the form:
        /// (any number of spaces)(optional |)#(digit)(end of line)
        /// </summary>
        static internal string RemoveAnnotations(string text)
        {            
            StringBuilder builder = new StringBuilder();
            using (StringReader reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = AnnotationExpression.Replace(line, "");
                    builder.AppendLine(line);
                }
            }
            return builder.ToString();
        }

        string GenerateCode(bool includeSnippetLineNumbers)
        {
            string nonMain = "";
            StringBuilder builder = new StringBuilder();
            foreach (string ns in options.Namespaces)
            {
                builder.AppendFormat(UsingFormat, ns);
            }

            string namespaces = builder.ToString();
            builder = new StringBuilder();
            StringReader reader = new StringReader(RemoveAnnotations(snippetText));
            string line;
            int lineNumber = 1;
            while ((line = reader.ReadLine()) != null)
            {
                // If we see "..." for the first time
                if (line == "..." && nonMain == "")
                {
                    nonMain = builder.ToString();
                    builder = new StringBuilder();
                }
                else
                {
                    if (includeSnippetLineNumbers)
                    {
                        builder.AppendFormat(LinePragmaFormat, lineNumber);
                    }
                    builder.AppendFormat(LineFormat, line);
                    lineNumber++;
                }
            }

            return namespaces + ClassPrologue + nonMain + MainPrologue + builder.ToString() + ClassEpilogue;
        }

        public CompilerResults Compile()
        {
            string executable = Application.ExecutablePath;
            string baseDir = Path.GetDirectoryName(executable);

            CodeDomProvider compiler = new CSharpCodeProvider(options.CompilerOptions);
            CompilerParameters parameters = new CompilerParameters();
            parameters.WarningLevel = 4;
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            foreach (string assembly in options.Assemblies)
            {
                string suffix = assembly.EndsWith(".exe") ? "" : ".dll";
                string prefix = "";
                if (!assembly.StartsWith("System"))
                {
                    prefix = baseDir + "\\";
                }
                parameters.ReferencedAssemblies.Add(prefix + assembly + suffix);

            }

            return compiler.CompileAssemblyFromSource(parameters, GenerateCode(true));
        }

        /// <summary>
        /// Exports the snippet by generating code (without extra line numbers)
        /// </summary>
        public void Export(string location)
        {
            File.WriteAllText(location, GenerateCode(false));
        }

        /// <summary>
        /// Saves the snippet in its raw form
        /// </summary>
        public void Save(string location)
        {
            File.WriteAllText(location, snippetText);
        }
    }
}
