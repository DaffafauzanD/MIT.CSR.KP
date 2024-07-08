using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class MstPerusahaan : IEntity
    {
        public MstPerusahaan()
        {
            TrsPenawaran = new HashSet<TrsPenawaran>();
            TrsProgresProgram = new HashSet<TrsProgresProgram>();
            TrsUsulan = new HashSet<TrsUsulan>();
        }

        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public string NamaPerusahaan { get; set; }
        public string Nib { get; set; }
        public string JenisPerseroan { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public string BidangUsaha { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual SetUser IdUserNavigation { get; set; }
        public virtual ICollection<TrsPenawaran> TrsPenawaran { get; set; }
        public virtual ICollection<TrsProgresProgram> TrsProgresProgram { get; set; }
        public virtual ICollection<TrsUsulan> TrsUsulan { get; set; }
    }
}
