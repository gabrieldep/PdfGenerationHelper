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
    public class WaterMarkEventHandler : IEventHandler
    {
        private readonly Color _textColor;
        private readonly string _text;
        public WaterMarkEventHandler(Color textColor, string text)
        {
            _textColor = textColor;
            _text = text;
        }

        /// <summary>
        /// HandleEvent- Add a water mark
        /// </summary>
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            Canvas canvas = new(pdfCanvas, pdfDoc, page.GetPageSize());
            canvas.SetProperty(Property.FONT_COLOR, _textColor);
            canvas.SetProperty(Property.FONT_SIZE, 60);
            canvas.SetProperty(Property.FONT, PdfFontFactory.CreateFont());
            canvas.ShowTextAligned(new Paragraph(_text), 298, 421, pdfDoc.GetPageNumber(page), TextAlignment.
                CENTER, VerticalAlignment.MIDDLE, 45);
            pdfCanvas.Release();
        }
    }
}
