using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class RefStatus : IEntity
    {
        public int Id { get; set; }
        public string Tipe { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
