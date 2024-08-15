namespace PointBooks.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int CategoryID { get; set; }
        public int PublisherID { get; set; }
        public decimal Price { get; set; }
        public string Language { get; set; }
        public int PublicationYear { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
    }
}
