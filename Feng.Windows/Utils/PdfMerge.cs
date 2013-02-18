using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// Merge Pdf Files
    /// </summary>
    [CLSCompliant(false)]   
    public class PdfMerge
    {
        /// <summary>
        /// 
        /// </summary>
        public struct PdfFile
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name"></param>
            /// <param name="pdfReader"></param>
            public PdfFile(string name, PdfReader pdfReader)
            {
                this.Name = name;
                this.PdfReader = pdfReader;
            }
            /// <summary>
            /// 
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            public PdfReader PdfReader;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PdfMerge()
        {
            documents = new List<PdfFile>();
        }

        private BaseFont baseFont;
        private bool enablePagination = false;
        private readonly List<PdfFile> documents;
        private int totalPages;

        /// <summary>
        /// 字体（用于显示页码）
        /// </summary>
        public BaseFont BaseFont
        {
            get { return baseFont; }
            set { baseFont = value; }
        }

        /// <summary>
        /// 是否显示页码
        /// </summary>
        public bool EnablePagination
        {
            get { return enablePagination; }
            set
            {
                enablePagination = value;
                if (value && baseFont == null)
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            }
        }

        /// <summary>
        /// 要Merge的文档
        /// </summary>
        public List<PdfFile> Documents
        {
            get { return documents; }
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="filename"></param>
        public void AddDocument(string filename)
        {
            documents.Add(new PdfFile(filename, new PdfReader(filename)));
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pdfStream"></param>
        public void AddDocument(string name, Stream pdfStream)
        {
            documents.Add(new PdfFile(name, new PdfReader(pdfStream)));
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pdfContents"></param>
        public void AddDocument(string name, byte[] pdfContents)
        {
            documents.Add(new PdfFile(name, new PdfReader(pdfContents)));
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pdfDocument"></param>
        public void AddDocument(string name, PdfReader pdfDocument)
        {
            documents.Add(new PdfFile(name, pdfDocument));
        }

        /// <summary>
        ///  合并文档
        /// </summary>
        /// <param name="outputFilename"></param>
        public void Merge(string outputFilename)
        {
            Merge(new FileStream(outputFilename, FileMode.Create));
        }

        /// <summary>
        /// 合并文档
        /// </summary>
        /// <param name="outputStream"></param>
        public void Merge(Stream outputStream)
        {
            if (outputStream == null || !outputStream.CanWrite)
                throw new ArgumentException("OutputStream es nulo o no se puede escribir en éste.");

            Document newDocument = null;
            try
            {
                newDocument = new Document();
                PdfWriter pdfWriter = PdfWriter.GetInstance(newDocument, outputStream);

                newDocument.Open();
                PdfContentByte pdfContentByte = pdfWriter.DirectContent;

                if (EnablePagination)
                {
                    documents.ForEach(delegate(PdfFile doc)
                    {
                        totalPages += doc.PdfReader.NumberOfPages;
                    });
                }

                int currentPage = 1;
                for(int i=0; i<documents.Count; ++i)
                {
                    PdfReader pdfReader = documents[i].PdfReader;

                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        PdfImportedPage importedPage = pdfWriter.GetImportedPage(pdfReader, page);
                        // 按照原有尺寸设置纸张大小
                        newDocument.SetPageSize(importedPage.BoundingBox);

                        if (page == 1)
                        {
                            Chapter chapter1 = new Chapter(documents[i].Name, i + 1);
                            chapter1.BookmarkTitle = documents[i].Name;
                            // invisible it. but not to set 0 which will cause error
                            chapter1.Title.Font.Size = 0.01f;  
                            newDocument.Add(chapter1);
                        }
                        else
                        {
                            newDocument.NewPage();
                        }
                        pdfContentByte.AddTemplate(importedPage, 0, 0);

                        if (EnablePagination)
                        {
                            pdfContentByte.BeginText();
                            pdfContentByte.SetFontAndSize(baseFont, 9);
                            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                                string.Format("{0} de {1}", currentPage++, totalPages), 520, 5, 0);
                            pdfContentByte.EndText();
                        }
                    }
                }
            }
            finally
            {
                outputStream.Flush();
                if (newDocument != null)
                    newDocument.Close();
                outputStream.Close();
            }
        }

        
    }
}
