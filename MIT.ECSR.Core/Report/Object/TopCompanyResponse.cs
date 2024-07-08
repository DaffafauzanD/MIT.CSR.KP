using Hangfire.Annotations;
using MIT.ECSR.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Response
{
    public class TopCompanyResponse
    {
        public PerusahaanResponse Company { get; set; }
        public MediaUrl Logo { get; set; }
        public double Jumlah { get; set; }
        public double Rupiah { get; set; }
    }
}
