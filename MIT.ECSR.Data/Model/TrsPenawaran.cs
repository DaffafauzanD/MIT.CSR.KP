using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsPenawaran : IEntity
    {
        public TrsPenawaran()
        {
            TrsPenawaranItem = new HashSet<TrsPenawaranItem>();
        }

        public Guid Id { get; set; }
        public Guid IdPerusahaan { get; set; }
        public string Deskripsi { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual MstPerusahaan IdPerusahaanNavigation { get; set; }
        public virtual ICollection<TrsPenawaranItem> TrsPenawaranItem { get; set; }
    }
}
