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
    public partial class ProgramResponse : IMapResponse<ProgramResponse, MIT.ECSR.Data.Model.TrsProgram>
    {
        public Guid Id { get; set; }
        public ReferensiObject JenisProgram { get; set; }
        public ReferensiObject NamaProgram { get; set; }
        public string Deskripsi { get; set; }
        public Guid Lokasi { get; set; }
        public string LokasiDati { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndProgramKerja { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public int Kegiatan { get; set; }
        public double Unit { get; set; }
        public double Rupiah { get;set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public MediaUrl Photo { get; set; }
        public int Progress { get; set; }

        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.TrsProgram, ProgramResponse> map)
        {
            map.ForMember(d => d.JenisProgram, opt => opt.MapFrom(s => new ReferensiObject()
            {
                Id = s.IdJenisProgram,
                Nama = s.IdJenisProgramNavigation.Name,
            }))
            .ForMember(d => d.NamaProgram, opt => opt.MapFrom(s => new ReferensiObject()
                {
                    Id = s.NamaProgramNavigation.Id,
                    Nama = s.NamaProgramNavigation.Name,
            }))
           .ForMember(d => d.Status, opt => opt.MapFrom(s => SetStatus(s)))
           .ForMember(d => d.Kegiatan, opt => opt.MapFrom(s => s.TrsProgramItem != null ? s.TrsProgramItem.Count():0))
           .ForMember(d => d.Unit, opt => opt.MapFrom(s => s.TrsProgramItem != null ? s.TrsProgramItem.Sum(e=>e.Jumlah) : 0))
           .ForMember(d => d.Rupiah, opt => opt.MapFrom(s => s.TrsProgramItem != null ? s.TrsProgramItem.Sum(e => e.Rupiah) : 0))
           .ForMember(d=>d.Progress,opt=>opt.MapFrom(s=> SetProgress(s)))
           .ForMember(d=>d.LokasiDati,opt=>opt.MapFrom(s=> s.LokasiNavigation.NamaDati4));
        }
        private int SetProgress(TrsProgram s)
        {
            int result = 0;
            int total_kegiatan = s.TrsProgramItem.Count();
            double total_progress = s.TrsProgramItem.Where(d => d.Progress.HasValue).Sum(d => d.Progress.Value);
            if (total_kegiatan > 0 && total_progress > 0)
                result = (int)Math.Round(total_progress / total_kegiatan);
            return result;
        }
        private string SetStatus(TrsProgram d)
        {
            if (d.EndProgramKerja < DateTime.Now)
                return "Expired";

            return ((ProgramStatusEnum)d.Status).ToString()?.Replace("_", " ");
        }
    }
}

