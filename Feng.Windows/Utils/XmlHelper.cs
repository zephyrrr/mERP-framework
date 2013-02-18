using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Collections.Specialized;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection.Emit;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlHelper
    {
        private static List<Type> s_xmlTypes = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public static void AddXmlType(Type type)
        {
            s_xmlTypes.Add(type);
        }

        /// <summary>
        /// xml序列化时，要包含内部数据的所有type
        /// </summary>
        /// <param name="assembly"></param>
        public static void AddXmlType(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (!type.IsEnum)
                {
                    continue;
                }
                s_xmlTypes.Add(type);
            }
        }

        /// <summary>
        /// 任意类型序列化成byte[](xml)
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ToXml(object src)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(src.GetType(), s_xmlTypes.ToArray());
            serializer.Serialize(ms, src);
            byte[] byteArray = ms.ToArray();

            return Encoding.UTF8.GetString(byteArray);
        }

        /// <summary>
        /// 任意类型从byte[](xml)反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object FromXml(Type type, string str)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            XmlSerializer serializer = new XmlSerializer(type, s_xmlTypes.ToArray());
            return serializer.Deserialize(ms);
        }

        /// <summary>
        /// 任意类型序列化成byte[](BinaryFormatter)
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] ToBytes(object src)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, src);
            return ms.ToArray();
        }

        /// <summary>
        /// 任意类型从byte[]反序列化(BinaryFormatter)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToObjFromBytes(byte[] value)
        {
            MemoryStream ms = new MemoryStream(value);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <param name="xsdStream"></param>
        public static bool ValidateXml(System.IO.Stream xmlStream, System.IO.Stream xsdStream)
        {
            StringBuilder sb = new StringBuilder();

			try
			{
                XmlSchema xmlSchema = XmlSchema.Read(xsdStream, 
                    new ValidationEventHandler(delegate(object sender, ValidationEventArgs args)
                    {
                        sb.Append(args.Message);
                        sb.Append(Environment.NewLine);
                    }));

                // Create the XmlSchemaSet class.
                XmlSchemaSet sc = new XmlSchemaSet();
                sc.Add(xmlSchema);

                ////Create the XmlParserContext.
                //XmlParserContext context = new XmlParserContext(null, null, string.Empty, XmlSpace.None);
              
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = sc;
                settings.ValidationEventHandler += new ValidationEventHandler(delegate(object sender, ValidationEventArgs args)
                {
                    sb.Append(args.Message);
                    sb.Append(Environment.NewLine);
                });
				//Implement the reader. 
                XmlReader xmlReader = XmlReader.Create(xmlStream, settings);

                while (xmlReader.Read())
				{
				}
               
				Console.WriteLine("Completed validating xmlfragment");
			}
			catch (XmlException ex)
			{
                sb.Append(ex.Message);
                sb.Append(Environment.NewLine);
			}
			catch(XmlSchemaException ex)
			{
                sb.Append(ex.Message);
                sb.Append(Environment.NewLine);
			}
			catch(Exception ex)
			{
                sb.Append(ex.Message);
                sb.Append(Environment.NewLine);
			}

            if (sb.Length != 0)
            {
                throw new ArgumentException("ValidateXml Failed with message " + sb.ToString()); 
            }
            else
            {
                //exceptionsMessage = string.Empty;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xsdStream"></param>
        public static void CompileXsdToClass(System.IO.Stream xsdStream)
        {
            CodeDomProvider codeProvider = null;
            string fileExtension = string.Empty;
            CreateCodeProvider("c#", ref codeProvider, ref fileExtension);

            ImportSchemasAsClasses(codeProvider, xsdStream, string.Empty, string.Empty, CodeGenerationOptions.GenerateProperties, 
                new StringCollection(), new StringCollection());
        }

        private static void CreateCodeProvider(string language, ref CodeDomProvider codeProvider, ref string fileExtension)
        {
            if (CodeDomProvider.IsDefinedLanguage(language))
            {
                try
                {
                    codeProvider = CodeDomProvider.CreateProvider(language);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Invalid language of " + language, ex);
                }
            }
            else
            {
                Type type = Type.GetType(language, false, true);
                if (type == null)
                {
                    throw new ArgumentException("Invalid language of " + language);
                }
                object obj2 = Activator.CreateInstance(type);
                codeProvider = obj2 as CodeDomProvider;
                if (codeProvider == null)
                {
                    throw new ArgumentException("Invalid language of " + language);
                }
            }

            if (codeProvider != null)
            {
                fileExtension = codeProvider.FileExtension;
                if (fileExtension == null)
                {
                    fileExtension = string.Empty;
                    return;
                }
                if ((fileExtension.Length > 0) && (fileExtension[0] != '.'))
                {
                    fileExtension = "." + fileExtension;
                    return;
                }
            }
            else
            {
                fileExtension = ".src";
            }
        }

        private static XmlSchema ReadSchema(System.IO.Stream xsdStream)
        {
            XmlTextReader reader = new XmlTextReader(xsdStream);

            XmlSchema schema = XmlSchema.Read(reader, ValidationCallbackWithErrorCode);

            return schema;
        }

        private static void GenerateVersionComment(CodeNamespace codeNamespace)
        {
            codeNamespace.Comments.Add(new CodeCommentStatement(""));
            AssemblyName name = Assembly.GetExecutingAssembly().GetName();
            codeNamespace.Comments.Add(new CodeCommentStatement(string.Format("This source code was auto-generated by {0}, Version={1}.", name.Name, "2.0.50727.3038")));
            codeNamespace.Comments.Add(new CodeCommentStatement(""));
        }


        private static void ImportSchemasAsClasses(CodeDomProvider codeProvider, System.IO.Stream xsdStream, string ns, string uri, CodeGenerationOptions options, IList elements, StringCollection schemaImporterExtensions)
        {
            XmlSchemas userSchemas = new XmlSchemas();

            Hashtable uris = new Hashtable();
            XmlSchema schema = ReadSchema(xsdStream);

            Uri uri2 = new Uri("http://www.w3.org/2001/XMLSchema/temp");
            uris.Add(schema, uri2);
            userSchemas.Add(schema, uri2);

            Hashtable includeSchemas = new Hashtable();
            Compile(userSchemas, uris, includeSchemas);
            try
            {
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                CodeNamespace namespace2 = new CodeNamespace(ns);
                codeCompileUnit.Namespaces.Add(namespace2);
                GenerateVersionComment(namespace2);
                XmlCodeExporter codeExporter = new XmlCodeExporter(namespace2, codeCompileUnit, codeProvider, options, null);
                XmlSchemaImporter schemaImporter = new XmlSchemaImporter(userSchemas, options, codeProvider, new ImportContext(new CodeIdentifiers(), false));
                schemaImporter.Extensions.Add(new System.Data.DataSetSchemaImporterExtension());

                {
                    StringEnumerator enumerator2 = schemaImporterExtensions.GetEnumerator();
                    {
                        while (enumerator2.MoveNext())
                        {
                            Type type = Type.GetType(enumerator2.Current.Trim(), true, false);
                            schemaImporter.Extensions.Add(type.FullName, type);
                        }
                    }
                }
                AddImports(namespace2, GetNamespacesForTypes(new Type[] { typeof(XmlAttributeAttribute) }));
                for (int i = 0; i < userSchemas.Count; i++)
                {
                    ImportSchemaAsClasses(userSchemas[i], uri, elements, schemaImporter, codeExporter);
                }
                foreach (XmlSchema schema2 in includeSchemas.Values)
                {
                    ImportSchemaAsClasses(schema2, uri, elements, schemaImporter, codeExporter);
                }

                CompilerParameters compilePrams = new CompilerParameters();
                CompilerResults compileResults = codeProvider.CompileAssemblyFromDom(compilePrams, codeCompileUnit);

                if (compileResults.Errors.Count > 0)
                {
                    throw new ArgumentException("Compile Error of " + compileResults.Errors[0].ToString());
                }
                // Feng.Windows.Utils.ReflectionHelper.CreateInstanceFromType(compileResults.CompiledAssembly.GetTypes()[0])

                //CodeTypeDeclarationCollection types = namespace2.Types;
                //CodeGenerator.ValidateIdentifiers(namespace2);
                //TextWriter writer = this.CreateOutputWriter(outputdir, fileName, fileExtension);
                //codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, writer, null);
                //writer.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Compile Xsd Error!", ex);
            }
        }

        private static void ImportSchemaAsClasses(XmlSchema schema, string uri, IList elements, XmlSchemaImporter schemaImporter, XmlCodeExporter codeExporter)
        {
            if (schema != null)
            {
                ArrayList list = new ArrayList();

                foreach (XmlSchemaElement element in schema.Elements.Values)
                {
                    bool flag;
                    if (element.IsAbstract || ((uri.Length != 0) && !(element.QualifiedName.Namespace == uri)))
                    {
                        continue;
                    }
                    if (elements.Count == 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        foreach (string str in elements)
                        {
                            if (str == element.Name)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        list.Add(schemaImporter.ImportTypeMapping(element.QualifiedName));
                    }
                }
                foreach (XmlTypeMapping mapping in list)
                {
                    codeExporter.ExportTypeMapping(mapping);
                }
            }
        }

        private static void AddImports(CodeNamespace codeNamespace, string[] namespaces)
        {
            foreach (string str in namespaces)
            {
                codeNamespace.Imports.Add(new CodeNamespaceImport(str));
            }
        }
        private static string[] GetNamespacesForTypes(Type[] types)
        {
            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < types.Length; i++)
            {
                string fullName = types[i].FullName;
                int length = fullName.LastIndexOf('.');
                if (length > 0)
                {
                    hashtable[fullName.Substring(0, length)] = types[i];
                }
            }
            string[] array = new string[hashtable.Keys.Count];
            hashtable.Keys.CopyTo(array, 0);
            return array;
        }


        private static void Compile(XmlSchemas userSchemas, Hashtable uris, Hashtable includeSchemas)
        {
            foreach (XmlSchema schema in userSchemas)
            {
                if ((schema.TargetNamespace != null) && (schema.TargetNamespace.Length == 0))
                {
                    schema.TargetNamespace = null;
                }
                Uri baseUri = (Uri)uris[schema];
                CollectIncludes(schema, baseUri, includeSchemas, baseUri.ToString().ToLower(CultureInfo.InvariantCulture));
            }
            try
            {
                userSchemas.Compile(new ValidationEventHandler(ValidationCallbackWithErrorCode), true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Compile Xsd Error!", ex);
            }
            if (!userSchemas.IsCompiled)
            {
                throw new ArgumentException("Compile Xsd Error!");
            }
        }

        private static void ValidationCallbackWithErrorCode(object sender, ValidationEventArgs args)
        {
            string str;
            if ((args.Exception.LineNumber == 0) && (args.Exception.LinePosition == 0))
            {
                str = string.Format("Schema validation warning: {0}", args.Message);
            }
            else
            {
                str = string.Format("Schema validation warning: {0} Line {1}, position {2}.", new object[] { args.Message, args.Exception.LineNumber.ToString(CultureInfo.InvariantCulture), args.Exception.LinePosition.ToString(CultureInfo.InvariantCulture) });
            }
            if (args.Severity == XmlSeverityType.Error)
            {
                throw new ArgumentException(str, args.Exception);
            }
        }

        private static void CollectIncludes(XmlSchema schema, Uri baseUri, Hashtable includeSchemas, string topUri)
        {
        //    if (schema != null)
        //    {
        //        foreach (XmlSchemaExternal external in schema.Includes)
        //        {
        //            string schemaLocation = external.SchemaLocation;
        //            if (external is XmlSchemaImport)
        //            {
        //                external.SchemaLocation = null;
        //                continue;
        //            }
        //            if (((external.Schema == null) && (schemaLocation != null)) && (schemaLocation.Length > 0))
        //            {
        //                Uri uri = ResolveUri(baseUri, schemaLocation);
        //                string str2 = uri.ToString().ToLower(CultureInfo.InvariantCulture);
        //                if (topUri == str2)
        //                {
        //                    external.Schema = new XmlSchema();
        //                    external.Schema.TargetNamespace = schema.TargetNamespace;
        //                    external.SchemaLocation = null;
        //                    break;
        //                }
        //                XmlSchema schema2 = (XmlSchema)includeSchemas[str2];
        //                if (schema2 == null)
        //                {
        //                    string path = schemaLocation;
        //                    string pathFromUri = GetPathFromUri(uri);
        //                    bool flag = (pathFromUri != null) && File.Exists(pathFromUri);
        //                    if (File.Exists(path))
        //                    {
        //                        if (flag)
        //                        {
        //                            string str5 = Path.GetFullPath(path).ToLower(CultureInfo.InvariantCulture);
        //                            if (str5 != pathFromUri)
        //                            {
        //                                Warning(Res.GetString("MultipleFilesFoundMatchingInclude4", new object[] { schemaLocation, GetPathFromUri(baseUri), str5, pathFromUri }));
        //                            }
        //                        }
        //                    }
        //                    else if (flag)
        //                    {
        //                        path = pathFromUri;
        //                    }
        //                    schema2 = ReadSchema(path, false);
        //                    includeSchemas[str2] = schema2;
        //                    CollectIncludes(schema2, uri, includeSchemas, topUri);
        //                }
        //                if (schema2 != null)
        //                {
        //                    external.Schema = schema2;
        //                    external.SchemaLocation = null;
        //                }
        //            }
        //        }
        //    }
        }

        //private static Uri ResolveUri(Uri baseUri, string relativeUri)
        //{
        //    if ((baseUri == null) || (!baseUri.IsAbsoluteUri && (baseUri.OriginalString.Length == 0)))
        //    {
        //        Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
        //        if (!uri.IsAbsoluteUri)
        //        {
        //            uri = new Uri(Path.GetFullPath(relativeUri));
        //        }
        //        return uri;
        //    }
        //    if ((relativeUri != null) && (relativeUri.Length != 0))
        //    {
        //        return new Uri(baseUri, relativeUri);
        //    }
        //    return baseUri;
        //}

        //private static string GetPathFromUri(Uri uri)
        //{
        //    if (uri != null)
        //    {
        //        try
        //        {
        //            return Path.GetFullPath(uri.LocalPath).ToLower(CultureInfo.InvariantCulture);
        //        }
        //        catch (Exception exception)
        //        {
        //            if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || ((exception is OutOfMemoryException) || (exception is ConfigurationException)))
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="predict"></param>
        /// <returns></returns>
        public static XmlNode FindXmlNode(XmlNode parentNode, Predicate<XmlNode> predict)
        {
            if (predict(parentNode))
                return parentNode;

            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                XmlNode ret = FindXmlNode(childNode, predict);
                if (ret != null)
                    return ret;
            }
            return null;
        }
    }
}
