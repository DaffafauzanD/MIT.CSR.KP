using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class SetUser : IEntity
    {
        public SetUser()
        {
            MstPerusahaan = new HashSet<MstPerusahaan>();
            TrsNotification = new HashSet<TrsNotification>();
        }

        public Guid Id { get; set; }
        public int IdRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual SetRole IdRoleNavigation { get; set; }
        public virtual ICollection<MstPerusahaan> MstPerusahaan { get; set; }
        public virtual ICollection<TrsNotification> TrsNotification { get; set; }
    }
}
