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
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Core.Response
{
    public partial class UsulanDetailResponse : IMapResponse<UsulanDetailResponse, MIT.ECSR.Data.Model.TrsUsulan>
    {
        public Guid Id { get; set; }
        public ReferensiObject JenisProgram { get; set; }
        public string NamaProgram { get; set; }
        public string Lokasi { get; set; }
        public string Deskripsi { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Notes { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public MediaUrl Photo { get; set; }


        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsUsulan, UsulanDetailResponse> map)
        {
            map.ForMember(d => d.JenisProgram, opt => opt.MapFrom(s => new ReferensiObject()
            {
                Id = s.IdJenisProgram,
                Nama = s.IdJenisProgramNavigation.Name,
            }))
           .ForMember(d => d.Status, opt => opt.MapFrom(s => SetStatus(s)));
        }
        private string SetStatus(TrsUsulan d)
        {
            switch ((UsulanStatusEnum)d.Status)
            {
                case UsulanStatusEnum.DRAFT:
                    return "Draft";
                case UsulanStatusEnum.WAITING:
                    return "Waiting";
                case UsulanStatusEnum.APPROVE:
                    return "Approve";
                case UsulanStatusEnum.REJECT:
                    return "Reject";
            }
            return "";
        }
    }
}

