using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class RefJenisProgram : IEntity
    {
        public RefJenisProgram()
        {
            RefKegiatan = new HashSet<RefKegiatan>();
            TrsProgram = new HashSet<TrsProgram>();
            TrsUsulan = new HashSet<TrsUsulan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int IdSubProgram { get; set; }
        public string Kode { get; set; }

        public virtual RefSubProgram IdSubProgramNavigation { get; set; }
        public virtual ICollection<RefKegiatan> RefKegiatan { get; set; }
        public virtual ICollection<TrsProgram> TrsProgram { get; set; }
        public virtual ICollection<TrsUsulan> TrsUsulan { get; set; }
    }
}
