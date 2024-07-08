using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsProgresProgram : IEntity
    {
        public Guid Id { get; set; }
        public Guid IdProgramItem { get; set; }
        public DateTime TglProgress { get; set; }
        public int Progress { get; set; }
        public string Deskripsi { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Notes { get; set; }
        public Guid IdPerusahaan { get; set; }

        public virtual MstPerusahaan IdPerusahaanNavigation { get; set; }
        public virtual TrsProgramItem IdProgramItemNavigation { get; set; }
    }
}
