using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class RefOpd : IEntity
    {
        public RefOpd()
        {
            RefSubProgram = new HashSet<RefSubProgram>();
            SetRole = new HashSet<SetRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<RefSubProgram> RefSubProgram { get; set; }
        public virtual ICollection<SetRole> SetRole { get; set; }
    }
}
