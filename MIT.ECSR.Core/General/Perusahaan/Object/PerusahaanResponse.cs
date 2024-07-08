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
    public class PerusahaanMiniResponse
    {
        public Guid Id { get; set; }
        public string NamaPerusahaan { get; set; }
        public string JenisPerseroan { get; set; }
        public MediaUrl Media { get; set; }
    }
    public class PerusahaanResponse : IMapResponse<PerusahaanResponse, MstPerusahaan>
    {
        public Guid Id { get; set; }
        public string Alamat { get; set; }
        public string BidangUsaha { get; set; }
        public string NamaPerusahaan { get; set; }
        public string JenisPerseroan { get; set; }
        public string Nib { get; set; }
        public string Npwp { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public void Mapping(IMappingExpression<MstPerusahaan, PerusahaanResponse> map)
        {
            map
                .ForMember(x => x.Id, opt => opt.MapFrom(z => z.Id))
                .ForMember(x => x.Alamat, opt => opt.MapFrom(z => z.Alamat))
                .ForMember(x => x.BidangUsaha, opt => opt.MapFrom(z => z.BidangUsaha))
                .ForMember(x => x.NamaPerusahaan, opt => opt.MapFrom(z => z.NamaPerusahaan))
                .ForMember(x => x.JenisPerseroan, opt => opt.MapFrom(z => z.JenisPerseroan))
                .ForMember(x => x.Nib, opt => opt.MapFrom(z => z.Nib))
                .ForMember(x => x.Npwp, opt => opt.MapFrom(z => z.Npwp))
                .ForMember(x => x.UpdateBy, opt => opt.MapFrom(z => z.UpdateBy))
                .ForMember(x => x.UpdateDate, opt => opt.MapFrom(z => z.UpdateDate))
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(z => z.CreateDate))
                .ForMember(x => x.CreateBy, opt => opt.MapFrom(z => z.CreateBy));
        }
    }
}
