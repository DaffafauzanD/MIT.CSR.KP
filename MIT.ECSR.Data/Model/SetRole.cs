using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class SetRole : IEntity
    {
        public SetRole()
        {
            SetUser = new HashSet<SetUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? IdOpd { get; set; }
        public Guid? IdDati { get; set; }

        public virtual RefDati IdDatiNavigation { get; set; }
        public virtual RefOpd IdOpdNavigation { get; set; }
        public virtual ICollection<SetUser> SetUser { get; set; }
    }
}
