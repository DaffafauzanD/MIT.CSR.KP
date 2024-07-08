using AutoMapper;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Response
{
    public partial class PenawaranDetailResponse 
    {
        public ProgramDetailResponse Program { get; set; }
        public List<ProgramItemResponse> Items { get; set; }
        public List<MediaUrl> ProgramLampiran { get; set; }
    }
}
