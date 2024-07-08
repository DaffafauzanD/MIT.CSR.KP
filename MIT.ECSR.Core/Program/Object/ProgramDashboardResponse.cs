namespace MIT.ECSR.Core.Program.Object
{
    public class ProgramDashboardResponse
    {
        public List<string> DataPage { get; set; }
        public List<ProgramItemDashboardResponse> Items { get; set; }
    }

    public class ProgramItemDashboardResponse
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<int> Data { get; set; }
    }
}
