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
    public partial class PenawaranInternalResponse 
    {
        public ProgramItemResponse Item { get; set; }
        public List<PenawaranInternalItemResponse> Penawaran { get; set; }
    }
    public class PenawaranInternalItemResponse
    {
        public Guid Id { get; set; }
        public Guid IdPerusahaan { get; set; }
        public string Perusahaan { get; set; }
        public string Deskripsi { get; set; }
        public MediaUrl Photos { get; set; }
        public double Jumlah { get; set; }
        public double? Rupiah { get; set; }
        public List<MediaUrl> Lampiran { get; set; }
    }
}
