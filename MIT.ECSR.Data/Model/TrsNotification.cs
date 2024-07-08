using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsNotification : IEntity
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Navigation { get; set; }
        public bool IsOpen { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SetUser IdUserNavigation { get; set; }
    }
}
