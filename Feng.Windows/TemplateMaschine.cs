/* ---------------------------------------------------------------
    TemplateMaschine - an open source template engine for C#

    written by Stefan Sarstedt (http://www.stefansarstedt.com/)
    Released under GNU Lesser General Public License (LGPL),
    see file 'copying' for details about the license

    History:
        - initial release (version 0.5) on Oct 28th, 2004
        - minor bugfixes (version 0.6) on March 29th, 2005
        - updated to support referencing assemblies that are 
          installed in the GAC (version 0.7) on January 12th, 2007
          Thanks to William.Manning@ips-sendero.com
        - Added support for generic arguments. 
          Ability to pass dictionary of arguments relaxing ordered 
          object[] requirement. Version 0.8 on March 18th, 2007
          Thanks to vijay.santhanam@gmail.com
   --------------------------------------------------------------- */
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

// http://www.stefansarstedt.com/templatemaschine.html
namespace Feng.Windows
{
    /// <summary>
    /// Template Compiler Exception
    /// </summary>
    public class TemplateCompilerException : Exception
    {
        /// <summary>
        /// Template Compiler Exception 
        /// </summary>
        /// <param name="msg">Error message</param>
        public TemplateCompilerException(string msg)
            : base(msg)
        {
        }
    }


    /// <summary>
    /// Template class, used to load and execute a template
    /// TODO: default values, precompilation? 
    /// typed arguments i.e. Dictionary<string,Type>?? host assembly would need to reference the same assemblies
    /// TODO: publically exposed arguments for lookup? Argument from Assembly
    /// </summary>
    public class Template
    {
        private object generatorObject = null;


        /// <summary>
        /// Load an embedded template from assembly
        /// </summary>
        /// <param name="embeddedTemplate">Name of template</param>
        /// <param name="assembly">Assembly to load template from</param>
        public Template(string embeddedTemplate, Assembly assembly)
        {
            this.ReadFileFromResources(embeddedTemplate, assembly);
            this.ProcessTemplate(embeddedTemplate, null);
        }

        /// <summary>
        /// Load an embedded template from assembly
        /// </summary>
        /// <param name="embeddedTemplate">Name of template</param>
        /// <param name="assembly">Assembly to load template from</param>
        /// <param name="fileNameForDebugging">File to write generated template code to - for debugging purposes</param>
        public Template(string embeddedTemplate, Assembly assembly, string fileNameForDebugging)
        {
            this.ReadFileFromResources(embeddedTemplate, assembly);
            this.ProcessTemplate(embeddedTemplate, fileNameForDebugging);
        }

        /// <summary>
        /// Load a template from file
        /// </summary>
        /// <param name="templateFile">Template filename</param>
        public Template(string templateFile)
        {
            this.ReadFile(templateFile);
            this.ProcessTemplate(templateFile, null);
        }

        /// <summary>
        /// Load a template from file
        /// </summary>
        /// <param name="templateFile">Template filename</param>
        /// <param name="fileNameForDebugging">File to write generated template code to - for debugging purposes</param>
        public Template(string templateFile, string fileNameForDebugging)
        {
            this.ReadFile(templateFile);
            this.ProcessTemplate(templateFile, fileNameForDebugging);
        }

        private enum Token { LDirective, RTag, LTag, LAssignment, LScript, RScript, String, Backslash, Newline, QuotationMark, Eof }
        private struct TokenInfo
        {
            public Token token;
            public string s;

            public TokenInfo(Token token, string s)
            {
                this.token = token;
                this.s = s;
            }
        }

        private int linePos = 1, columnPos = 1;
        private StringBuilder resultString = new StringBuilder(50000);
        private StringBuilder resultStringScript = new StringBuilder(50000);
        private StringBuilder sourceStringOriginal = new StringBuilder(50000);
        private StringBuilder sourceString;
        private List<string> assemblies = new List<string>();
        private List<string> usings = new List<string>();
        private Dictionary<string, string> arguments = new Dictionary<string, string>(); // Key=name Value=type



        /*
                private struct Argument
                {
                    public string name;
                    public string type;

                    public Argument(string name, string type)
                    {
                        this.name = name;
                        this.type = type;
                    }
                }
        */

        private int IndexOfStopToken()
        {
            int i = 0;
            for (; i < sourceString.Length; i++)
            {
                if ((sourceString[i] == '<') || (sourceString[i] == '\\') || (sourceString[i] == '%') || (sourceString[i] == '\r') || (sourceString[i] == '"'))
                    break;
            }
            //if (i == sourceString.Length)
            //	throw new ApplicationException("IndexOfStopToken");
            if (i == 0)
                return 1;

            return i;
        }

        private int IndexOf(char c)
        {
            int i = 0;
            for (; i < sourceString.Length; i++)
            {
                if (sourceString[i] == c)
                    break;
            }

            return i;
        }

        private TokenInfo NextToken()
        {
            Token token = Token.Eof;
            string tokenVal = null;
            int pos = 0;

            if (sourceString.Length == 0)
                return new TokenInfo(Token.Eof, null);

            switch (sourceString[0])
            {
                case '<':
                    // Comment?
                    if (sourceString.ToString(0, 5).IndexOf("<!--") == 0)
                    {
                        // skip comment
                        pos += sourceString.ToString(0, 100).IndexOf("-->") + 3;
                        sourceString.Remove(0, pos);
                        return NextToken();
                    }
                    // LDirective, LTag or LAssignment?
                    if (sourceString[1] == '%')
                    {
                        // LDirective
                        if (sourceString[2] == '@')
                        {
                            pos += 3;
                            token = Token.LDirective;
                            break;
                        }
                        // LAssignment
                        if (sourceString[2] == '=')
                        {
                            pos += 3;
                            token = Token.LAssignment;
                            break;
                        }
                        // LTag
                        pos += 2;
                        token = Token.LTag;
                        break;
                    }
                    if (sourceString.ToString(0, 7) == "<script")
                    {
                        pos += IndexOf('>') + 1;
                        token = Token.LScript;
                        break;
                    }
                    if (sourceString.ToString(0, 9) == "</script>")
                    {
                        pos += 9;
                        token = Token.RScript;
                        break;
                    }
                    goto default;
                case '%':
                    // RTag?
                    if (sourceString[1] == '>')
                    {
                        pos += 2;
                        token = Token.RTag;
                        break;
                    }
                    goto default;
                case '"':
                    token = Token.QuotationMark;
                    tokenVal = "\"";
                    pos++;
                    break;
                case '\\':
                    token = Token.Backslash;
                    tokenVal = "\\";
                    pos++;
                    break;
                case '\r':
                    // Newline?
                    if (sourceString[1] == '\n')
                    {
                        pos += 2;
                        linePos++;
                        token = Token.Newline;
                        break;
                    }
                    goto default;
                default:
                    token = Token.String;
                    int stPos = IndexOfStopToken();
                    tokenVal = sourceString.ToString(pos, stPos);
                    pos += stPos;
                    break;
            }

            if (tokenVal == null)
                tokenVal = sourceString.ToString(0, pos);
            if (token == Token.Newline)
                columnPos = 1;
            else
                columnPos += pos;
            sourceString.Remove(0, pos);

            return new TokenInfo(token, tokenVal);
        }

        private void ParseTemplateAssignment()
        {
            TokenInfo tokenInfo;
            while ((tokenInfo = NextToken()).token != Token.RTag)
            {
                if ((tokenInfo.token != Token.String) && (tokenInfo.token != Token.QuotationMark))
                    throw new ApplicationException("invalid syntax");
                resultString.Append(tokenInfo.s);
            }
        }

        private void ParseTemplateTagBlock()
        {
            TokenInfo tokenInfo;

            while ((tokenInfo = NextToken()).token != Token.RTag)
            {
                switch (tokenInfo.token)
                {
                    case Token.LAssignment:
                        resultString.Append("Response.Write(");
                        ParseTemplateAssignment();
                        resultString.Append(");\r\n");
                        break;
                    case Token.String:
                    case Token.QuotationMark:
                    case Token.Backslash:
                        resultString.Append(tokenInfo.s);
                        break;
                    case Token.Newline:
                        resultString.Append("\r\n");
                        break;
                    default:
                        throw new ApplicationException("invalid syntax");
                }
            }
        }

        private void ParseTemplate(string templateFile)
        {
            bool isBlockOpen = false;

            TokenInfo tokenInfo, lastTokenInfo = new TokenInfo(Token.Eof, null);
            while ((tokenInfo = NextToken()).token != Token.Eof)
            {
                switch (tokenInfo.token)
                {
                    case Token.LDirective:
                        // ignore directives
                        string s = "";
                        while ((tokenInfo = NextToken()).token != Token.RTag)
                            s += tokenInfo.s;
                        s = s.Trim(new char[] { ' ' });

                        if (s.StartsWith("Import"))
                        {
                            Regex directiveRegex = new Regex("[ ]*Import[ ]*NameSpace=\"(?<namespace>[a-zA-Z0-9._]+)\"", RegexOptions.IgnoreCase);
                            Match match = directiveRegex.Match(s);
                            if (match == null)
                                throw new ApplicationException("invalid syntax in 'Import' expression");
                            string namspaceToImport = match.Groups["namespace"].Value;
                            usings.Add(namspaceToImport);
                        }
                        else
                            if (s.StartsWith("Assembly"))
                            {
                                Regex directiveRegex = new Regex("[ ]*Assembly[ ]*Name=\"(?<assemblyName>[a-zA-Z0-9._]+)\"", RegexOptions.IgnoreCase);
                                Match match = directiveRegex.Match(s);
                                if (match == null)
                                    throw new ApplicationException("invalid syntax in 'Assembly' expression");
                                string assemblyToImport = match.Groups["assemblyName"].Value;
                                assemblies.Add(assemblyToImport);
                            }
                            else
                                if (s.StartsWith("Argument"))
                                {
                                    Regex directiveRegex = new Regex("[ ]*Argument[ ]+Name=\"(?<name>[a-zA-Z0-9_]+)\"[ ]+Type=\"(?<type>[\\<>a-zA-Z0-9.\\]\\[]+)\"", RegexOptions.IgnoreCase);
                                    Match match = directiveRegex.Match(s);
                                    Console.WriteLine(directiveRegex.Match("Argument=\"strings\" Type=\"List<string>\""));
                                    if (match == null)
                                        throw new ApplicationException("invalid syntax in 'Argument' expression");
                                    string argumentName = match.Groups["name"].Value;
                                    string argumentType = match.Groups["type"].Value;

                                    arguments.Add(argumentName, argumentType);
                                }
                                else
                                    throw new ApplicationException("invalid syntax in directive");
                        break;
                    case Token.LAssignment:
                        if (isBlockOpen)
                        {
                            resultString.Append("\"+");
                            ParseTemplateAssignment();
                            resultString.Append("+\"");
                        }
                        else
                        {
                            resultString.Append("Response.Write(");
                            ParseTemplateAssignment();
                            resultString.Append(");\r\n");
                        }
                        break;
                    case Token.LScript:
                        while ((tokenInfo = NextToken()).token != Token.RScript)
                            resultStringScript.Append(tokenInfo.s);
                        break;
                    case Token.LTag:
                        if (isBlockOpen)
                        {
                            int i;
                            for (i = resultString.Length - 1; i >= 0; i--)
                            {
                                if ((resultString[i] != ' ') && (resultString[i] != '\t'))
                                    break;
                            }
                            if ((resultString[i] == '"') && (resultString[i - 1] == '('))
                                resultString.Remove(i - 15, resultString.Length - i + 15);
                            else
                                resultString.Append("\");\r\n");
                            isBlockOpen = false;
                        }
                        ParseTemplateTagBlock();
                        break;
                    case Token.String:
                        if (!isBlockOpen)
                            resultString.Append("Response.Write(\"");
                        isBlockOpen = true;
                        resultString.Append(tokenInfo.s);
                        break;
                    case Token.QuotationMark:
                        if (!isBlockOpen)
                            resultString.Append("Response.Write(\"");
                        isBlockOpen = true;
                        resultString.Append("\\\"");
                        break;
                    case Token.Backslash:
                        resultString.Append("\\\\");
                        break;
                    case Token.Newline:
                        if (isBlockOpen)
                            resultString.Append("\");Response.WriteLine();\r\n" + "#line " + linePos + " \"" + templateFile + "\"\r\n");
                        else
                        {
                            if ((lastTokenInfo.token == Token.Newline) || (lastTokenInfo.token == Token.String))
                                resultString.Append("Response.WriteLine();\r\n" + "#line " + linePos + " \"" + templateFile + "\"\r\n");
                        }
                        isBlockOpen = false;
                        break;
                    default:
                        break;
                }
                lastTokenInfo = tokenInfo;
            }
        }

        private void ReadFile(string templateFile)
        {
            if (!File.Exists(templateFile))
                throw new FileNotFoundException("Template file '" + templateFile + "' could not be found.");

            using (StreamReader sr = new StreamReader(templateFile))
            {
                sourceString = new StringBuilder();

                // process include directives
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.IndexOf("Include") != -1)
                    {
                        Regex directiveRegex = new Regex("<%@[ ]+Include[ ]+File=\"(?<fileName>[a-zA-Z0-9. -\\\\]+)\"[ ]*%>", RegexOptions.IgnoreCase);
                        Match match = directiveRegex.Match(s);
                        if (match == null)
                            throw new ApplicationException("invalid syntax in 'Include' expression");
                        string fileName = match.Groups["fileName"].Value;
                        using (StreamReader sr_includeFile = new StreamReader(fileName))
                        {
                            sourceString.Insert(0, sr_includeFile.ReadToEnd());
                        }
                    }
                    else
                        sourceString.Append(s + "\r\n");
                }
            }

            sourceStringOriginal.Append(sourceString.ToString());
        }

        private void ReadFileFromResources(string templateFile, Assembly assembly)
        {
            bool found = false;
            foreach (string s in assembly.GetManifestResourceNames())
            {
                if (s.ToLower().EndsWith(templateFile.ToLower()))
                {
                    templateFile = s;
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new FileNotFoundException("Template '" + templateFile + "' could not be found as an embedded resource in assembly.");

            Stream fileStream = assembly.GetManifestResourceStream(templateFile);
            using (StreamReader sr = new StreamReader(fileStream))
            {
                sourceString = new StringBuilder();
                // process include directives
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.IndexOf("Include") != -1)
                    {
                        Regex directiveRegex = new Regex("<%@[ ]+Include[ ]+File=\"(?<fileName>[a-zA-Z0-9. -\\\\]+)\"[ ]*%>", RegexOptions.IgnoreCase);
                        Match match = directiveRegex.Match(s);
                        if (match == null)
                        {
                            throw new ApplicationException("invalid syntax in 'Include' expression");
                        }
                        string fileName = match.Groups["fileName"].Value;
                        string[] resourceNames = assembly.GetManifestResourceNames();
                        string resourceName = Array.Find<string>(resourceNames, delegate(string fn) { return fn.EndsWith(fileName); });
                        /*string resourceName = null;
                        foreach (string resName in resourceNames)
                        {
                            if (resName.EndsWith(fileName))
                            {
                                resourceName = resName;
                                break;
                            }
                        }*/
                        if (resourceName == null)
                            throw new FileNotFoundException("Included file '" + fileName + "' not found in resource.");

                        Stream includefileStream = assembly.GetManifestResourceStream(resourceName);
                        using (StreamReader sr_includeFile = new StreamReader(includefileStream))
                        {
                            sourceString.Insert(0, sr_includeFile.ReadToEnd());
                        }
                    }
                    else
                        sourceString.Append(s + "\r\n");
                }
            }

            sourceStringOriginal.Append(sourceString.ToString());
        }

        private string GetLine(StringBuilder resultString, int lineNo)
        {
            int lineCount = 1;
            int charPos = 0;
            string line = "";
            while (true)
            {
                if (resultString[charPos] == '\n')
                {
                    lineCount++;
                    if (lineCount == lineNo)
                    {
                        int endPos = charPos;
                        while (endPos < resultString.Length)
                        {
                            endPos++;
                            if (resultString[endPos] == '\n')
                                break;
                        }
                        line = resultString.ToString(charPos, endPos - charPos);
                    }
                }
                charPos++;
                if (charPos >= resultString.Length)
                    break;
            }

            return line;
        }

        private void ProcessTemplate(string templateFile, string fileNameForDebugging)
        {
            ParseTemplate(templateFile);

            int argNo = 0;
            foreach (KeyValuePair<string, string> arg in arguments)
            {
                resultString.Insert(0, "this." + arg.Key + " = (" + arg.Value + ")args[" + argNo + "];\r\n");
                argNo++;
            }
            resultString.Insert(0, "try {\r\n");
            resultString.Insert(0, "public StringBuilder Generate(object[] args) {\r\n");
            resultString.Insert(0, "public void SetOutput(string outputFile, bool outputToString) { Response.Init(outputFile,outputToString); }\r\n");
            resultString.Insert(0, "public TempGeneratorClass() {}\r\n");
            foreach (KeyValuePair<string, string> argument in arguments)
            {
                resultString.Insert(0, "private " + argument.Value + " " + argument.Key + ";\r\n");
            }
            resultString.Insert(0, resultStringScript);
            resultString.Insert(0, "class TempGeneratorClass {\r\n");
            resultString.Insert(0, "}\r\n");
            resultString.Insert(0, "public static void Init(string outputFile,bool outputToString) { if (outputFile != null) outputStream = new StreamWriter(outputFile); if (outputToString) outputString = new StringBuilder();}\r\n");
            resultString.Insert(0, "public static void Cleanup() {if (outputStream != null) outputStream.Close();}\r\n");
            resultString.Insert(0, "public static void Write(string s) {if (outputStream != null) outputStream.Write(s); if (outputString != null) outputString.Append(s);} public static void WriteLine(string s) {if (outputStream != null) outputStream.WriteLine(s); if (outputString != null) {outputString.Append(s);outputString.Append(\"\\r\\n\");}} public static void WriteLine() {if (outputStream != null) outputStream.WriteLine(); if (outputString != null) outputString.Append(\"\\r\\n\");}\r\n");
            resultString.Insert(0, "public static void Flush() {if (outputStream != null) outputStream.Flush();}\r\n");
            resultString.Insert(0, "public static StringBuilder Result {get { return outputString; }}\r\n");
            resultString.Insert(0, "private static StreamWriter outputStream = null;\r\n");
            resultString.Insert(0, "private static StringBuilder outputString  = null;\r\n");
            resultString.Insert(0, "internal class Response {\r\n");
            resultString.Insert(0, "namespace TempGenerator {\r\n");
            foreach (string usingExpr in usings)
            {
                resultString.Insert(0, "using " + usingExpr + ";\r\n");
            }
            resultString.Insert(0, "using System.Diagnostics;\r\n");
            resultString.Insert(0, "using System.Text;\r\n");
            resultString.Insert(0, "using System.IO;\r\n");
            resultString.Insert(0, "using System;\r\n");
            resultString.Append("} catch(Exception ex) { Response.Flush(); throw ex; }\r\n");
            resultString.Append("finally { Response.Cleanup(); }\r\n");
            resultString.Append("return Response.Result;\r\n");
            resultString.Append("} } }\r\n");

            if (!String.IsNullOrEmpty(fileNameForDebugging))
            {
                StreamWriter debugFile = new StreamWriter(fileNameForDebugging);
                debugFile.Write(resultString.ToString());
                debugFile.Close();
            }

            // compile assembly in memory with a yet unknown name
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            //parameters.OutputAssembly = "Generator.dll";
            parameters.ReferencedAssemblies.Add("System.dll");
            foreach (string assembly in assemblies)
            {
                parameters.ReferencedAssemblies.Add(this.ResolveAssemblyPath(assembly));
            }
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, resultString.ToString());
            if (results.Errors.Count > 0)
            {
                generatorObject = null;
                string errs = "";

                foreach (CompilerError CompErr in results.Errors)
                {
                    errs += "Template: " + CompErr.FileName + Environment.NewLine +
                        "Line number: " + CompErr.Line + Environment.NewLine +
                        "Error: " + CompErr.ErrorNumber + " '" + CompErr.ErrorText + "'";
                    string line = GetLine(sourceStringOriginal, CompErr.Line);
                    throw new TemplateCompilerException("Error compiling template: " + Environment.NewLine + errs + Environment.NewLine + "Line: '" + line + "'");
                }
            }
            else
            {
                Assembly generatorAssembly = results.CompiledAssembly;
                generatorObject = generatorAssembly.CreateInstance("TempGenerator.TempGeneratorClass", false, System.Reflection.BindingFlags.CreateInstance, null, null, null, null);
            }
        }

        private string ResolveAssemblyPath(string name)
        {
            name = name.ToLower();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (this.IsDynamicAssembly(assembly))
                {
                    continue;
                }

                if (Path.GetFileNameWithoutExtension(assembly.Location).ToLower().Equals(name))
                {
                    return assembly.Location;
                }
            }

            return Path.GetFullPath(name + ".dll");
        }

        private bool IsDynamicAssembly(Assembly assembly)
        {
            return assembly.ManifestModule.Name.StartsWith("<");
        }

        /// <summary>
        /// Creates an ordered argument array from a named Dictionary collection
        /// </summary>
        /// <param name="args">Dictionary of Template Arguments keyed by argument name</param>
        /// <returns>Array of objects for arguments(in order as they are defined in template)</returns>
        private object[] CreateOrderedArgumentArray(Dictionary<string, object> args)
        {

            object[] newargs = new object[arguments.Count];
            int n = 0;

            foreach (KeyValuePair<string, string> arg in arguments)
            {

                if (args.ContainsKey(arg.Key))
                {
                    newargs[n] = args[arg.Key];
                }
                else
                {
                    throw new ArgumentException("Template Argument " + arg.Key + " was not specified");
                }
                n++;
            }

            return newargs;

        }


        /// <summary>
        /// Processes a template
        /// </summary>
        /// <param name="args">Dictionary of Template Arguments keyed by argument name</param>
        /// <returns>Result of processed template</returns>
        public string Generate(Dictionary<string, object> args)
        {
            return Generate(CreateOrderedArgumentArray(args), null, true);
        }

        /// <summary>
        /// Processes a template
        /// </summary>
        /// <param name="args">Template Arguments (in order as they are defined in template)</param>
        /// <param name="outputFile">File to write results to</param>
        /// <param name="outputToString">Flag if output should be written to a string</param>
        /// <returns>Result of processed template, if outputToString is set to true</returns>
        public string Generate(object[] args, string outputFile, bool outputToString)
        {
            if (generatorObject == null)
                throw new ApplicationException("please load a template first");

            try
            {
                generatorObject.GetType().InvokeMember("SetOutput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, generatorObject, new object[] { outputFile, outputToString });
                object str = generatorObject.GetType().InvokeMember("Generate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, generatorObject, new object[] { args });
                if (str == null)
                    return null;

                return ((StringBuilder)str).ToString();
            }
            catch (Exception ex)
            {
                //                if (ex is TargetInvocationException)
                //                    Console.WriteLine(ex.InnerException.ToString());
                //                else
                if (ex is TargetInvocationException)
                    throw ex.InnerException;
                throw ex;
            }
        }

        /// <summary>
        /// Processes a template
        /// </summary>
        /// <param name="args">Template Arguments (in order as they are defined in template)</param>
        /// <param name="outputFile">File to write results to</param>
        public void Generate(object[] args, string outputFile)
        {
            Generate(args, outputFile, false);
        }

        /// <summary>
        /// Processes a template
        /// </summary>
        /// <param name="args">Template Arguments (in order as they are defined in template)</param>
        /// <returns>Result of processed template</returns>
        public string Generate(object[] args)
        {
            return Generate(args, null, true);
        }
    }
}
