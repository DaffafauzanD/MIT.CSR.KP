//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MIT.ECSR.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MIT.ECSR.Core.Request
{
    public partial class ProgresProgramRequest
    {
        [Required]
        public DateTime TglProgress { get; set; }
        [Required]
		public string Deskripsi{ get; set; }
		[Required]
		public Guid IdProgramItem{ get; set; }
		[Required]
		public int Progress{ get; set; }
        public List<FileObject> Lampiran { get; set; }

    }
}

