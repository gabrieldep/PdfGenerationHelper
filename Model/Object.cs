using System.Collections.Generic;

namespace PdfGenerationHelper.Model
{
    public class PdfObject<T>
    {
        public T OriginalObject { get; set; }
        public IEnumerable<PdfObjectProperty> Properties { get; set; }
    }
}
