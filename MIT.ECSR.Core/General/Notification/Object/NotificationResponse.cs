//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MIT.ECSR.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MIT.ECSR.Data.Model;

namespace MIT.ECSR.Core.Response
{
    public partial class NotificationResponse: IMapResponse<NotificationResponse, MIT.ECSR.Data.Model.TrsNotification>
    {
		public Guid Id{ get; set; }
		public string CreateBy{ get; set; }
		public DateTime CreateDate{ get; set; }
		public string Description{ get; set; }
		public Guid IdUser{ get; set; }
		public bool IsOpen{ get; set; }
		public string Navigation{ get; set; }
		public string Subject{ get; set; }


        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsNotification, NotificationResponse> map)
        {
            map.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.CreateBy, opt => opt.MapFrom(s => s.CreateBy))
            .ForMember(d => d.CreateDate, opt => opt.MapFrom(s => s.CreateDate))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.IdUser, opt => opt.MapFrom(s => s.IdUser))
            .ForMember(d => d.IsOpen, opt => opt.MapFrom(s => s.IsOpen))
            .ForMember(d => d.Navigation, opt => opt.MapFrom(s => s.Navigation))
            .ForMember(d => d.Subject, opt => opt.MapFrom(s => s.Subject));

        }
    }
}

