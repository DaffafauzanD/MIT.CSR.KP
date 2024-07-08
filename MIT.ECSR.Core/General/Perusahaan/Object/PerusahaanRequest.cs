using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Request
{
    public class PerusahaanRequest
    {
        public string Alamat { get; set; }
        public string BidangUsaha { get; set; }
        public string NamaPerusahaan { get; set; }
        public string JenisPerseroan { get; set; }
        public string Nib { get; set; }
        public string Npwp { get; set; }
    }
}
