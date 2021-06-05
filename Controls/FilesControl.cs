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

        public static void AddText(ref Document document, string text, int fontSize, TextAlignment alignment)
        {
            document.Add(new Paragraph(text)
                                .SetTextAlignment(alignment)
                                .SetFontSize(fontSize));
        }
    }
}
