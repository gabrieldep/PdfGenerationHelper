using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.Model
{
    public class PdfObject<T>
    {
        public T OriginalObject { get; set; }
        public IEnumerable<PdfObjectProperty> Properties { get; set; }
    }
}
