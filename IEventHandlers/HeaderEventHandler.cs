using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.IEventHandlers
{
    public class HeaderEventHandler : IEventHandler
    {
        private readonly Color _textColor;
        private readonly string _text;
        private readonly float _textFontSize;
        public HeaderEventHandler(string text, Color textColor, float textFontSize)
        {
            _textColor = textColor;
            _text = text;
            _textFontSize = textFontSize;
        }

        /// <summary>
        /// HandleEvent- Add header for each page
        /// </summary>
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            pdfCanvas.BeginText()
                        .SetFontAndSize(PdfFontFactory.CreateFont(), _textFontSize)
                        .SetColor(_textColor, false)
                        .MoveText(pageSize.GetWidth() / 2 - 60, pageSize.GetTop() - 20)
                        .ShowText(_text)
                        .EndText();
        }
    }
}
