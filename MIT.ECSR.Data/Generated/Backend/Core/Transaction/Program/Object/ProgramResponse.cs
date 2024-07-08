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
    public partial class ProgramResponse: IMapResponse<ProgramResponse, MIT.ECSR.Data.Model.TrsProgram>
    {
		public Guid Id{ get; set; }
		public DateTime? ApprovedAt{ get; set; }
		public string ApprovedBy{ get; set; }
		public string CreateBy{ get; set; }
		public DateTime CreateDate{ get; set; }
		public string Deskripsi{ get; set; }
		public DateTime EndProgramKerja{ get; set; }
		public DateTime EndTglPelaksanaan{ get; set; }
		public int IdJenisProgram{ get; set; }
		public Guid Lokasi{ get; set; }
		public int NamaProgram{ get; set; }
		public string Notes{ get; set; }
		public DateTime StartTglPelaksanaan{ get; set; }
		public short Status{ get; set; }
		public string UpdateBy{ get; set; }
		public DateTime UpdateDate{ get; set; }


        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsProgram, ProgramResponse> map)
        {
            //use this for mapping
            //map.ForMember(d => d.object, opt => opt.MapFrom(s => s.EF_COLUMN));

        }
    }
}

