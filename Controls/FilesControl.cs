using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PdfGenerationHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.Controls
{
    class FilesControl
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
    }
}
