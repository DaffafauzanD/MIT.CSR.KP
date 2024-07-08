using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsUsulanItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid IdUsulan { get; set; }
        public string SatuanUnit { get; set; }
        public string Nama { get; set; }
        public double Jumlah { get; set; }
        public double Rupiah { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual TrsUsulan IdUsulanNavigation { get; set; }
    }
}
