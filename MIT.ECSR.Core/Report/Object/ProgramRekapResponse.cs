using Hangfire.Annotations;
using MIT.ECSR.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Response
{
    public class ProgramRekapResponse
    {
        public double OnProgress { get; set; }
        public double OnProgressRupiah { get; set; }
        public double Done { get; set; }
        public double DoneRupiah { get; set; }
    }
}
