﻿using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class RefKegiatan : IEntity
    {
        public RefKegiatan()
        {
            TrsProgram = new HashSet<TrsProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Kode { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int IdJenisProgram { get; set; }

        public virtual RefJenisProgram IdJenisProgramNavigation { get; set; }
        public virtual ICollection<TrsProgram> TrsProgram { get; set; }
    }
}
