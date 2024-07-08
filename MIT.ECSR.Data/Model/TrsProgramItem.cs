using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsProgramItem : IEntity
    {
        public TrsProgramItem()
        {
            TrsPenawaranItem = new HashSet<TrsPenawaranItem>();
            TrsProgresProgram = new HashSet<TrsProgresProgram>();
        }

        public Guid Id { get; set; }
        public Guid IdProgram { get; set; }
        public string SatuanUnit { get; set; }
        public string Nama { get; set; }
        public double Jumlah { get; set; }
        public double Rupiah { get; set; }
        public double? Progress { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartTglPelaksanaan { get; set; }
        public DateTime EndTglPelaksanaan { get; set; }
        public string Lokasi { get; set; }

        public virtual TrsProgram IdProgramNavigation { get; set; }
        public virtual ICollection<TrsPenawaranItem> TrsPenawaranItem { get; set; }
        public virtual ICollection<TrsProgresProgram> TrsProgresProgram { get; set; }
    }
}
