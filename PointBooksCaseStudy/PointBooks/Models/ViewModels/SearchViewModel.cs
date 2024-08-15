namespace PointBooks.Models.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<PublisherCount> PublisherCounts { get; set; }
        public IEnumerable<AuthorCount> AuthorCounts { get; set; }
        public string Query { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
