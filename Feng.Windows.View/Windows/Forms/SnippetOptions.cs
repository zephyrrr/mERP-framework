using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Options for snippets: assembly references, namespaces etc.
    /// </summary>
    public class SnippetOptions
    {
        readonly List<string> namespaces = new List<string>();
        readonly List<string> assemblies = new List<string>();
        readonly Dictionary<string,string> compilerOptions = new Dictionary<string,string>();
        readonly string name;

        public string Name
        {
            get { return name; }
        }

        public IList<string> Namespaces
        {
            get { return namespaces; }
        }

        public IList<string> Assemblies
        {
            get { return assemblies; }
        }

        public IDictionary<string,string> CompilerOptions
        {
            get { return compilerOptions; }
        }

        public SnippetOptions(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// The absolute filename from which to load the options
        /// </summary>
        static string OptionsFile
        {
            get
            {
                string executable = Application.ExecutablePath;
                return Path.Combine(Path.GetDirectoryName(executable), "SnippyOptions.xml");
            }
        }

        public static IEnumerable<SnippetOptions> LoadAvailableOptions()
        {
            IList<SnippetOptions> ret = new List<SnippetOptions>();

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(OptionsFile))
            {
                doc.Load(OptionsFile);
                foreach (XmlElement element in doc.DocumentElement.GetElementsByTagName("SnippetOptions"))
                {
                    ret.Add(FromXmlElement(element));
                }
            }
            else
            {
                SnippetOptions defaultOptions = new SnippetOptions("default");
                defaultOptions.Assemblies.Add("System");
                defaultOptions.Assemblies.Add("System.Windows.Forms");
                defaultOptions.Assemblies.Add("System.Core");
                defaultOptions.Namespaces.Add("System");
                defaultOptions.Namespaces.Add("System.Text");
                defaultOptions.Namespaces.Add("System.Text.RegularExpressions");
                defaultOptions.Namespaces.Add("System.IO");
                defaultOptions.Namespaces.Add("System.Collections");
                defaultOptions.Namespaces.Add("System.Collections.Generic");
                defaultOptions.Namespaces.Add("System.ComponentModel");
                defaultOptions.Namespaces.Add("System.Windows.Forms");
                defaultOptions.Namespaces.Add("System.Threading");
                defaultOptions.Namespaces.Add("System.Reflection");

                defaultOptions.CompilerOptions["CompilerVersion"] = "v3.5";

                ret.Add(defaultOptions);
            }

            return ret;
        }

        private static SnippetOptions FromXmlElement(XmlElement element)
        {
            SnippetOptions ret = new SnippetOptions(element.Attributes["name"].Value);
            foreach (XmlElement tag in element.GetElementsByTagName("Assembly"))
            {
                ret.Assemblies.Add(tag.Attributes["name"].Value);
            }
            foreach (XmlElement tag in element.GetElementsByTagName("Namespace"))
            {
                ret.Namespaces.Add(tag.Attributes["name"].Value);
            }
            foreach (XmlElement tag in element.GetElementsByTagName("CompilerOption"))
            {
                ret.CompilerOptions[tag.Attributes["name"].Value] = tag.Attributes["value"].Value;
            }
            return ret;
        }
    }
}
