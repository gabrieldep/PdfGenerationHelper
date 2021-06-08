using iText.Kernel.Colors;
using iText.Kernel.Events;
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
    public class BackgroundEventHandler : IEventHandler
    {
        private readonly Color _backgroundColor;
        public BackgroundEventHandler(Color backgroundColor)
        {
            _backgroundColor = backgroundColor;
        }

        /// <summary>
        /// HandleEvent- Add a background color
        /// </summary>
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            pdfCanvas.SaveState()
                        .SetFillColor(_backgroundColor)
                        .Rectangle(pageSize.GetLeft(), pageSize.GetBottom(), pageSize.GetWidth(), pageSize.GetHeight())
                        .Fill()
                        .RestoreState();
        }
    }
}
