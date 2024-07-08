﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MIT.ECSR.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MIT.ECSR.Core.Request
{
    public class ApproveRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool IsApprove { get; set; }
        public string Notes { get; set; }
    }
    public partial class PenawaranRequest
    {
        [Required]
        public List<PenawaranItemRequest> Items { get; set; }
    }
    public class PenawaranItemRequest
    {
        public Guid IdProgramItem { get; set; }
        public double Value { get; set; }
        public double Anggaran { get; set; }
    }
}

