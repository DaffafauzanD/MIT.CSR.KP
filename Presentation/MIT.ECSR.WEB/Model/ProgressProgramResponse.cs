using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Web.Model
{
    public class ProgressProgramResponse
    {
        public Guid Id { get; set; }
        public DateTime TglProgress { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Deskripsi { get; set; }
        public Guid IdProgramItem { get; set; }
        public Guid IdProgram { get; set; }
        public int Progress { get; set; }
        public int Status { get; set; }
        public string ItemName { get; set; }
        public List<MediaUrl> Lampiran { get; set; }
    }
}
