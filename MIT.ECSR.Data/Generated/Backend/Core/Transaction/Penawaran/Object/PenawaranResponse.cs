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
    public partial class PenawaranResponse: IMapResponse<PenawaranResponse, MIT.ECSR.Data.Model.TrsPenawaran>
    {
		public Guid Id{ get; set; }
		public string CreateBy{ get; set; }
		public DateTime CreateDate{ get; set; }
		public string Deskripsi{ get; set; }
		public Guid IdPerusahaan{ get; set; }


        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsPenawaran, PenawaranResponse> map)
        {
            //use this for mapping
            //map.ForMember(d => d.object, opt => opt.MapFrom(s => s.EF_COLUMN));

        }
    }
}

