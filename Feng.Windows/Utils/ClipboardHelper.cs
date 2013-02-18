using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Utils
{
    public static class ClipboardHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dataFormat"></param>
        public static void CopyValueToClipboard(string str, string dataFormat)
        {
            //byte[] blob = System.Text.Encoding.UTF8.GetBytes(str);
            //System.IO.MemoryStream s = new System.IO.MemoryStream(blob);
            //DataObject data = new DataObject();
            //data.SetData(DataFormats.CommaSeparatedValue, s);
            //Clipboard.SetDataObject(data); 
            DataObject data = new DataObject();
            data.SetData(dataFormat, str);
            Clipboard.SetDataObject(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public static string GetValueToClipboard(string dataFormat)
        {
            //IDataObject o = Clipboard.GetDataObject();
            //if (o.GetDataPresent(DataFormats.CommaSeparatedValue))
            //{
            //    System.IO.StreamReader sr = new System.IO.StreamReader((System.IO.Stream)o.GetData(DataFormats.CommaSeparatedValue));
            //    string s = sr.ReadToEnd();
            //    sr.Close();
            //    return s;
            //}
            //return null;
            IDataObject o = Clipboard.GetDataObject();
            if (o.GetDataPresent(dataFormat))
            {
                return o.GetData(dataFormat).ToString();
            }
            return null;
        }

        public static void CopyTextToClipboard(string str)
        {
            CopyValueToClipboard(str, DataFormats.Text);
        }

        /// <summary>
        /// GetTextFromClipboard
        /// </summary>
        /// <returns></returns>
        public static string GetTextFromClipboard()
        {
            return GetValueToClipboard(DataFormats.Text);
        }
    }
}
