using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PdfGenerationHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.Controls
{
    public class FilesControl
    {
        /// <summary>
        /// Add a text to the document
        /// </summary>
        /// <param name="document">Document that text will be add.</param>
        /// <param name="text">String with the text.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="alignment">Text alignment.</param>
        public static void AddText(ref Document document, string text, int fontSize, TextAlignment alignment)
        {
            document.Add(new Paragraph(text)
                                .SetTextAlignment(alignment)
                                .SetFontSize(fontSize));
        }

        /// <summary>
        /// Add a table to the document
        /// </summary>
        /// <param name="document">Document that text will be add.</param>
        /// <param name="objects">IEnumerable with the objects to complete the table.</param>
        public static void AddTable<T>(ref Document document, IEnumerable<T> objects)
        {
            Type type = objects.First().GetType();

            IEnumerable<PropertyInfo> properties = type.GetProperties()
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
                                .GetValue(o).ToString()
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
                    table.AddCell(property.Value);
                }
            }
            document.Add(table);
        }
    }
}
