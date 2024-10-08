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
    public partial class ProgramItemResponse: IMapResponse<ProgramItemResponse, MIT.ECSR.Data.Model.TrsProgramItem>
    {
		public Guid Id{ get; set; }
		public string CreateBy{ get; set; }
		public DateTime CreateDate{ get; set; }
		public DateTime EndTglPelaksanaan{ get; set; }
		public Guid IdProgram{ get; set; }
		public double Jumlah{ get; set; }
		public string Lokasi{ get; set; }
		public string Nama{ get; set; }
		public double? Progress{ get; set; }
		public double Rupiah{ get; set; }
		public string SatuanUnit{ get; set; }
		public DateTime StartTglPelaksanaan{ get; set; }
		public string UpdateBy{ get; set; }
		public DateTime UpdateDate{ get; set; }


        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsProgramItem, ProgramItemResponse> map)
        {
            //use this for mapping
            //map.ForMember(d => d.object, opt => opt.MapFrom(s => s.EF_COLUMN));

        }
    }
}

