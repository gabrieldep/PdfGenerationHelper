using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.Model
{
    public class PdfGHDocument
    {
        public PdfGHDocument()
        {
            Stream = new MemoryStream();
            PdfWriter = new PdfWriter(Stream);
            PdfDocument = new PdfDocument(PdfWriter);
            Document = new Document(PdfDocument);
        }

        /// <summary>
        /// Get byte array from stream
        /// </summary>
        /// <returns>byte[] with document</returns>
        public byte[] GetByteStream()
        {
            return ((MemoryStream)Stream).ToArray();
        }

        /// <summary>
        /// Add a text to the document
        /// </summary>
        /// <param name="text">String with the text.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="alignment">Text alignment.</param>
        public void AddText(string text, float fontSize, TextAlignment alignment, Color color)
        {
            Paragraph paragraph = new Paragraph(text)
                    .SetTextAlignment(alignment)
                    .SetFontSize(fontSize)
                    .SetFontColor(color);
            this.Document.Add(paragraph);
        }

        /// <summary>
        /// Close this Document
        /// </summary>
        public void Close()
        {
            Document.Close();
        }

        public void AddTable<T>(IEnumerable<T> objects)
        {
            Type type = objects.First().GetType();

            IEnumerable<PropertyInfo> properties = type
                .GetProperties()
                .Where(p => p.PropertyType.Namespace == "System");

            Table table = new(properties.Count());

            IEnumerable<PdfObject<T>> ListPdfObjects = objects
                .Select(o => new PdfObject<T>
                {
                    OriginalObject = o,
                    Properties = properties.Select(p => new PdfObjectProperty
                    {
                        PropertyInfo = p,
                        Value = type
                                .GetProperty(p.Name)
                                .GetValue(o)?.ToString()
                    })
                });

            foreach (PropertyInfo property in properties)
            {
                table.AddHeaderCell(property.Name);
            }

            foreach (PdfObject<T> pdfObject in ListPdfObjects)
            {
                foreach (PdfObjectProperty property in pdfObject.Properties)
                {
                    table.AddCell(string.IsNullOrEmpty(property.Value) ? "" : property.Value);
                }
            }
            Document.Add(table);
        }

        public Document Document { get; set; }
        public PdfDocument PdfDocument { get; set; }
        public Stream Stream { get; set; }
        public PdfWriter PdfWriter { get; set; }
        public PdfReader PdfReader { get; set; }
    }
}
