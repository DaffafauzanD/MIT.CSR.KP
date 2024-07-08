using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class RefDati : IEntity
    {
        public RefDati()
        {
            SetRole = new HashSet<SetRole>();
            TrsProgram = new HashSet<TrsProgram>();
        }

        public Guid Id { get; set; }
        public string KodeDati1 { get; set; }
        public string NamaDati1 { get; set; }
        public string KodeDati2 { get; set; }
        public string NamaDati2 { get; set; }
        public string KodeDati3 { get; set; }
        public string NamaDati3 { get; set; }
        public string KodeDati4 { get; set; }
        public string NamaDati4 { get; set; }
        public bool Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<SetRole> SetRole { get; set; }
        public virtual ICollection<TrsProgram> TrsProgram { get; set; }
    }
}
