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
    public partial class PenawaranResponse : IMapResponse<PenawaranResponse, TrsProgram>
    {
        public Guid Id { get; set; }
        public ReferensiObject JenisProgram { get; set; }
        public ReferensiObject NamaProgram { get; set; }
        public Guid Lokasi { get; set; }
        public string LokasiDati { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public List<PenawaranKegiatanItemResponse> Items { get; set; }
        public int Penawaran { get; set; }
        public int WaitingPenawaran { get; set; }
        public void Mapping(IMappingExpression<TrsProgram, PenawaranResponse> map)
        {
            map.ForMember(d => d.JenisProgram, opt => opt.MapFrom(s => new ReferensiObject()
            {
                Id = s.IdJenisProgram,
                Nama = s.IdJenisProgramNavigation.Name,
            }));
            map.ForMember(d => d.NamaProgram, opt => opt.MapFrom(s => new ReferensiObject()
            {
                Id = s.NamaProgram,
                Nama = s.NamaProgramNavigation.Name,
            }));
            map.ForMember(d => d.LokasiDati, opt => opt.MapFrom(s => s.LokasiNavigation.NamaDati4));
        }
    }
    public class PenawaranKegiatanItemResponse
    {
        public string Kegiatan { get; set; }
        public double Jumlah { get; set; }
        public string Satuan { get; set; }
        public double Rupiah { get; set; }
        public double Sisa { get; set; }
    }
}
