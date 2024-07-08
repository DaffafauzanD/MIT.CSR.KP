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
    public class ProgressResponse : IMapResponse<ProgressResponse, TrsProgresProgram>
    {
        public ProgramItemObject Item { get; set; }
        public Guid Id { get; set; }
        public int Progress { get; set; }
        public string Deskripsi { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public List<ProgramMediaObject> Media { get; set; }
        public void Mapping(IMappingExpression<TrsProgresProgram, ProgressResponse> map)
        {
            map.ForMember(d => d.Item, opt => opt.MapFrom(s => new ProgramItemObject()
            {
                Id = s.Id,
                Jumlah = s.IdProgramItemNavigation.Jumlah,
                Nama = s.IdProgramItemNavigation.Nama,
                SatuanJenis = new ReferensiObject()
                {
                    Id = s.IdProgramItemNavigation.IdSatuanJenis,
                    Nama = s.IdProgramItemNavigation.IdSatuanJenisNavigation.Name
                }
            }))
            .ForMember(d => d.Media, opt => opt.MapFrom(s => s.TrsProgresProgramMedia != null ? s.TrsProgresProgramMedia.Select(a => new ProgramMediaObject()
            {
                Id = a.IdMedia,
                Filename = a.IdMediaNavigation.FileName
            }) : null));
        }
    }
}