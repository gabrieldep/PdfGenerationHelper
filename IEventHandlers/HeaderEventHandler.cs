using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
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
        private readonly int _columns;
        public HeaderEventHandler(string text, Color textColor, float textFontSize, int columns)
        {
            _textColor = textColor;
            _text = text;
            _textFontSize = textFontSize;
            _columns = columns;
        }

        /// <summary>
        /// HandleEvent- Add header for each page
        /// </summary>
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle rec = new(1/10, 1, page.GetPageSize().GetWidth()/5, page.GetPageSize().GetHeight() / 5);
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            ImageData imageData = ImageDataFactory.Create("wwwroot/logo.jpg");
            pdfCanvas.AddImage(imageData, rec, true);
        }
    }
}
