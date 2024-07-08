using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsPenawaranItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid IdPenawaran { get; set; }
        public Guid IdProgramItem { get; set; }
        public double Jumlah { get; set; }
        public double? Rupiah { get; set; }
        public int Status { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string Notes { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual TrsPenawaran IdPenawaranNavigation { get; set; }
        public virtual TrsProgramItem IdProgramItemNavigation { get; set; }
    }
}
