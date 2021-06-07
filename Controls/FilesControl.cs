using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
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
        public static void AddText(ref Document document, string text, float fontSize, TextAlignment alignment, Color color)
        {
            document.Add(new Paragraph(text)
                                .SetTextAlignment(alignment)
                                .SetFontSize(fontSize)
                                .SetFontColor(color));
        }

        /// <summary>
        /// Add a text to the document
        /// </summary>
        /// <param name="document">Document that text will be add.</param>
        /// <param name="path">String with image name.</param>
        public static void AddImage(ref Document document, string path)
        {
            ImageData imageData = ImageDataFactory.Create(path);
            Image image = new(imageData);
            document.Add(image);
        }

        /// <summary>
        /// Add a table to the document.
        /// </summary>
        /// <param name="document">Document that text will be add.</param>
        /// <param name="objects">IEnumerable with the objects to complete the table.</param>
        /// <param name="fontSize">Text ont size.</param>
        /// <param name="textAlignment">TextAlignment.</param>
        /// <param name="percentWidth">Denominador da razão: Largura da página / denominadorWidth.</param>
        /// <param name="horizontalAlignment">Table HorizontalAlignment.</param>
        public static void AddTable<T>(ref Document document, float fontSize,
            IEnumerable<T> objects, TextAlignment textAlignment,
            float percentWidth, HorizontalAlignment horizontalAlignment)
        {
            Type type = objects.First().GetType();

            IEnumerable<PropertyInfo> properties = type.GetProperties()
                 .Where(p => p.PropertyType.Namespace == "System");

            Table table = new(properties.Count());

            table.SetWidth(new UnitValue(2, percentWidth));
            table.SetHorizontalAlignment(horizontalAlignment);

            table.SetFontSize(fontSize);
            table.SetTextAlignment(textAlignment);

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
