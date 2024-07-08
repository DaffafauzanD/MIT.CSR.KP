using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsUsulan : IEntity
    {
        public TrsUsulan()
        {
            TrsUsulanItem = new HashSet<TrsUsulanItem>();
        }

        public Guid Id { get; set; }
        public int IdJenisProgram { get; set; }
        public Guid IdPerusahaan { get; set; }
        public string NamaProgram { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public string Lokasi { get; set; }
        public string Deskripsi { get; set; }
        public short Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Notes { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public virtual RefJenisProgram IdJenisProgramNavigation { get; set; }
        public virtual MstPerusahaan IdPerusahaanNavigation { get; set; }
        public virtual ICollection<TrsUsulanItem> TrsUsulanItem { get; set; }
    }
}
