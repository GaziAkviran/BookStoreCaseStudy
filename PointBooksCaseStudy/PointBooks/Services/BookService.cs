using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;
        private readonly PublisherRepository _publisherRepository;
        private readonly AuthorRepository _authorRepository;
        private readonly BookAuthorRepository _bookAuthorRepository;

       public BookService(BookRepository bookRepository, PublisherRepository publisherRepository, AuthorRepository authorRepository, BookAuthorRepository bookAuthorRepository)
        {
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
            _authorRepository = authorRepository;
            _bookAuthorRepository = bookAuthorRepository;
        }


        public async Task<IEnumerable<Book>> GetBooksByQueryAsync(string query)
        {
            return await _bookRepository.SearchAsync(query);
        }

        public IEnumerable<Book> SortBooks(IEnumerable<Book> books, string sortBy)
        {
            return sortBy switch
            {
                "price-asc" => books.OrderBy(b => b.Price).ToList(),
                "price-desc" => books.OrderByDescending(b => b.Price).ToList(),
                "a-z" => books.OrderBy(b => b.Title).ToList(),
                _ => books,
            };
        }

        public List<Book> GetBooksByPublisher(List<Book> books, List<string> selectedPublishers)
        {
            var publishers = _publisherRepository.GetAll().Result;
            var publisherBooks = books
            .Where(book => publishers
                .Where(publisher => selectedPublishers.Contains(publisher.PublisherName))
                .Select(p => p.PublisherID)
                .Contains(book.PublisherID))
            .ToList();

            return publisherBooks;
        }

        public List<Book> GetBooksByAuthor(List<Book> books, List<string> selectedAuthors)
        {
            var bookAuthors = _bookAuthorRepository.GetAll().Result;
            var authors = _authorRepository.GetAll().Result;

            var authorBooks = books
            .Where(book => bookAuthors
                .Where(ba => ba.BookID == book.BookID)
                .Select(ba => ba.AuthorID)
                .Intersect(authors.Where(author => selectedAuthors.Contains(author.Name + " " + author.Surname))
                                  .Select(a => a.AuthorID))
                .Any())
            .ToList();

            return authorBooks;
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int? categoryID)
        {
            if (categoryID == null || categoryID == 0)
            {
                return await _bookRepository.GetAllBooksAsync();
            }
            else
            {
                return await _bookRepository.GetBooksByCategoryAsync(categoryID.Value);
            }
        }

        public async Task<IEnumerable<PublisherCount>> GetPublisherCountsAsync()
        {
            return await _bookRepository.GetPublisherCountsAsync();
        }

        public async Task<IEnumerable<AuthorCount>> GetAuthorCountsAsync()
        {
            return await _bookRepository.GetAuthorCountsAsync();
        }


        public async Task<bool> AddBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            await _bookRepository.AddBookAsync(book);
            return true;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            await _bookRepository.UpdateBookAsync(book);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int bookID)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookID);
            if (book == null)
                return false;

            await _bookRepository.DeleteBookAsync(bookID);
            return true;
        }

        public async Task<Book> GetBookByIdAsync(int bookID)
        {
            return await _bookRepository.GetBookByIdAsync(bookID);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }



    }
}
