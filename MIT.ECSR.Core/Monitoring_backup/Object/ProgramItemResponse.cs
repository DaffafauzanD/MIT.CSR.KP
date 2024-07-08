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
    public class ProgramItemResponse : IMapResponse<ProgramItemResponse, TrsProgramItem>
    {
        public ProgramItemObject Item { get; set; }
        public double Progress { get; set; }
        public void Mapping(IMappingExpression<TrsProgramItem, ProgramItemResponse> map)
        {
            map.ForMember(d => d.Item, opt => opt.MapFrom(s => new ProgramItemObject()
            {
                Id = s.Id,
                Jumlah = s.Jumlah,
                Nama = s.Nama,
                SatuanJenis = new ReferensiObject()
                {
                    Id = s.IdSatuanJenis,
                    Nama = s.IdSatuanJenisNavigation.Name
                }
            }))
            .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.TrsProgresProgram != null ?
                        s.TrsProgresProgram.Select(d => d.Progress).OrderByDescending(d => d).FirstOrDefault() : 0));
        }
    }
}