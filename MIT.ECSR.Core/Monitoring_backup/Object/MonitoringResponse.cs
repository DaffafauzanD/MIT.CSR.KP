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
    public class MonitoringResponse : IMapResponse<MonitoringResponse, TrsPenawaran>
    {
        public PenawaranResponse Penawaran { get; set; }
        public double TotalProgress { get; set; }
        public List<string> Action { get; set; }
        public void Mapping(IMappingExpression<TrsPenawaran, MonitoringResponse> map)
        {
            map.ForMember(d => d.Penawaran, opt => opt.MapFrom(s => s));
            //   .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.IdProgramNavigation.TrsProgramItem));
        }
    }
}