//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace MIT.ECSR.Core.Request
{
    public partial class OpdRequest
    {
		[Required]
		public bool Active{ get; set; }
		[Required]
		public string Name{ get; set; }

    }
}

