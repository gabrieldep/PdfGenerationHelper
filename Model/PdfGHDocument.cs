using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public byte[] GetByteStream() => ((MemoryStream)Stream).ToArray();

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
            Document.Add(paragraph);
        }

        /// <summary>
        /// Close this Document
        /// </summary>
        public void Close()
        {
            Document.Close();
            PdfDocument.Close();
            PdfWriter.Close();
            Stream.Close();
        }

        /// <summary>
        /// Fill a table with the data received and add to a document
        /// </summary>
        /// <param name="objects">IEnumerable with the objects to complete the table.</param>
        /// <returns>Return a filled table.</returns>
        public void AddTable<T>(IEnumerable<T> objects)
        {
            Type type = objects.First().GetType();

            IEnumerable<PropertyInfo> properties = type
                .GetProperties()
                .Where(p => p.PropertyType.Namespace == "System");

            Table table = new(properties.Count());
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetWidth(PdfDocument.GetPage(1).GetPageSize().GetWidth() * 9 / 10);
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

        /// <summary>
        /// Add a image to the document
        /// </summary>
        /// <param name="path">String with image name.</param>
        public void AddImage(string path)
        {
            ImageData imageData = ImageDataFactory.Create(path);
            Image image = new(imageData);
            Document.Add(image);
        }

        /// <summary>
        /// Get the final variable before the pdf
        /// </summary>
        /// <returns>FileStreamResult with the Pdf.</returns>
        public FileStreamResult GetFileStreamResult() => new FileStreamResult(new MemoryStream(GetByteStream()), "application/pdf");

        public Document Document { get; set; }
        public PdfDocument PdfDocument { get; set; }
        public Stream Stream { get; set; }
        public PdfWriter PdfWriter { get; set; }
        public PdfReader PdfReader { get; set; }
    }
}
