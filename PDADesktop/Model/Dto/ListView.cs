namespace PDADesktop.Model.Dto
{
    public class ListView
    {
        public int page { get; set; }
        public int records { get; set; }
        public Row[] rows { get; set; }
        public int total { get; set; }
    }
}
