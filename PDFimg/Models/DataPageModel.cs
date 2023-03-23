namespace PDFimg.Models
{
    internal class DataPageModel
    {
        public string Name { get; set; } = default!;
        public string PathToImage { get; set; } = default!;
        public string PageNumbers { get; set; } = default!;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
