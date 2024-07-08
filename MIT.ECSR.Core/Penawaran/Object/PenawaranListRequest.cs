using MIT.ECSR.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Penawaran.Object
{
    public class PenawaranListRequest : ListRequest
    {
        public Guid? IdPerusahaan { get; set; }
    }
}
