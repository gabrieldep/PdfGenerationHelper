using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.IEventHandlers
{
    public class PaginationEventHandler : IEventHandler
    {
        private readonly Color _textColor;
        private readonly string _text;
        private readonly float _textFontSize;
        public PaginationEventHandler(Color textColor, float textFontSize)
        {
            _textColor = textColor;
            _textFontSize = textFontSize;
        }

        /// <summary>
        /// HandleEvent- Add footer for each page
        /// </summary>
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            int pageNumber = pdfDoc.GetPageNumber(page);

            pdfCanvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(), _textFontSize)
                    .MoveText(pageSize.GetWidth() / 2 - 60, pageSize.GetTop() - 20)
                    .ShowText("")
                    .MoveText(60, -pageSize.GetTop() + 30)                    
                    .ShowText(pageNumber.ToString())
                    .EndText();
        }
    }
}
