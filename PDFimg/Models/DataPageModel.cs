namespace PDFimg.Models
{
    internal class DataPageModel
    {
        public string Name { get; set; } = default!;
        public string PathToFile { get; set; } = default!;
        public string Pages { get; set; } = default!;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
