using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsProgram : IEntity
    {
        public TrsProgram()
        {
            TrsProgramItem = new HashSet<TrsProgramItem>();
        }

        public Guid Id { get; set; }
        public int IdJenisProgram { get; set; }
        public int NamaProgram { get; set; }
        public DateTime EndProgramKerja { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public Guid Lokasi { get; set; }
        public string Deskripsi { get; set; }
        public short Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Notes { get; set; }

        public virtual RefJenisProgram IdJenisProgramNavigation { get; set; }
        public virtual RefDati LokasiNavigation { get; set; }
        public virtual RefKegiatan NamaProgramNavigation { get; set; }
        public virtual ICollection<TrsProgramItem> TrsProgramItem { get; set; }
    }
}
