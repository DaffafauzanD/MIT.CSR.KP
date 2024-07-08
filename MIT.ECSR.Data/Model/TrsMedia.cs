using System;
using System.Collections.Generic;
using WonderKid.DAL.Interface;


namespace MIT.ECSR.Data.Model 
{
    public partial class TrsMedia : IEntity
    {
        public Guid Id { get; set; }
        public string Tipe { get; set; }
        public string Modul { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public bool IsImage { get; set; }
        public string OriginalPath { get; set; }
        public string ResizePath { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
