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
            //use this for mapping
            //map.ForMember(d => d.object, opt => opt.MapFrom(s => s.EF_COLUMN));

        }
    }
}

