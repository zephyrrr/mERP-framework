using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// ExcelXmlWriter
    /// </summary>
    public class ExcelXmlWriter : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName"></param>
        public ExcelXmlWriter(string fileName)
        {
            m_fileName = fileName;
        }
        /// <summary>
        /// Dispose RadioButton and remove it from parent's controls
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ExcelXmlWriter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        private string m_fileName;

        private Dictionary<string, StreamWriter> m_streams = new Dictionary<string, StreamWriter>();
        /// <summary>
        /// Write
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        public void WriteExcelXml(DataTable dt, string sheetName)
        {
            bool first = false;
            if (!m_streams.ContainsKey(sheetName))
            {
                m_streams[sheetName] = new StreamWriter(new MemoryStream());
                first = true;
            }

            ExcelXmlHelper.WriteExcelXmlRows(dt, m_streams[sheetName], first);
            m_streams[sheetName].Flush();
        }

        /// <summary>
        /// Close(Write all buffer)
        /// </summary>
        public void Close()
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(m_fileName);
            ExcelXmlHelper.WriteExcelXmlHead(sw);
            sw.Flush();
            foreach (KeyValuePair<string, StreamWriter> kvp in m_streams)
            {
                ExcelXmlHelper.WriteExcelXmlTableHead(sw, kvp.Key);
                sw.Flush();

                //byte[] b = new byte[kvp.Value.Length];
                //kvp.Value.Read(b, 0, b.Length);
                (kvp.Value.BaseStream as MemoryStream).WriteTo(sw.BaseStream);
                sw.Flush();
                kvp.Value.Close();

                ExcelXmlHelper.WriteExcelXmlTableTail(sw);
                sw.Flush();
            }
            ExcelXmlHelper.WriteExcelXmlTail(sw);
            sw.Close();
        }
    }
}
