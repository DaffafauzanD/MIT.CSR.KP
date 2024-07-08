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
    public partial class ProgresProgramExternalResponse
    {
        public Guid Id { get; set; }
        public ReferensiObject JenisProgram { get; set; }
        public string NamaProgram { get; set; }
        public string Deskripsi { get; set; }
        public Guid Lokasi { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndProgramKerja { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public MediaUrl Photo { get; set; }
        public List<ProgressProgramExternalItemObject> Items { get; set; }
    }
    public class ProgressProgramExternalItemObject
    {
        public Guid Id { get; set; }
        public double Progress { get; set; }
        public string Kegiatan { get; set; }
        public DateTime? LastProgress { get; set; }
        public string LastProgressBy { get; set; }
        public double Jumlah { get; set; }
        public double Total { get; set; }
        public ProgressDetailProgramExternalItemObject Detail { get; set; }
    }

    public class ProgressDetailProgramExternalItemObject : IMapResponse<ProgressDetailProgramExternalItemObject, MIT.ECSR.Data.Model.TrsProgresProgram>
    {
        public Guid? Id { get; set; }
        public double? Progress { get; set; }
        public DateTime? TglProgress { get; set; }
        public string Deskripsi { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Notes { get; set; }
        public List<MediaUrl> Lampiran { get; set; }
        public string ProgramItemName { get; set; }
        public double? Unit { get; set; }
        public double? Booking { get; set; }
        public string Satuan { get; set; }

        public void Mapping(IMappingExpression<TrsProgresProgram, ProgressDetailProgramExternalItemObject> map)
        {
            map.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.Progress))
                .ForMember(d => d.TglProgress, opt => opt.MapFrom(s => s.TglProgress))
                .ForMember(d => d.Deskripsi, opt => opt.MapFrom(s => s.Deskripsi))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.CreateBy, opt => opt.MapFrom(s => s.CreateBy))
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(s => s.CreateDate))
                .ForMember(d => d.ApprovedBy, opt => opt.MapFrom(s => s.ApprovedBy))
                .ForMember(d => d.ApprovedAt, opt => opt.MapFrom(s => s.ApprovedAt))
                .ForMember(d => d.Notes, opt => opt.MapFrom(s => s.Notes));
        }
    }
    
    public class PublicProgressDetailProgramExternalItemObject : IMapResponse<PublicProgressDetailProgramExternalItemObject, MIT.ECSR.Data.Model.TrsProgresProgram>
    {
        public double? Progress { get; set; }
        public DateTime? TglProgress { get; set; }
        public string Perusahaan { get; set; }
        public List<MediaUrl> Lampiran { get; set; }
        public string ProgramItemName { get; set; }
        public double? Unit { get; set; }
        public string Satuan { get; set; }

        public void Mapping(IMappingExpression<TrsProgresProgram, PublicProgressDetailProgramExternalItemObject> map)
        {
            map.ForMember(d => d.Progress, opt => opt.MapFrom(s => s.Progress))
                .ForMember(d => d.TglProgress, opt => opt.MapFrom(s => s.TglProgress));
        }
    }
}

