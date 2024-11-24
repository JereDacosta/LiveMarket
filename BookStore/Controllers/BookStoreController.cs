using Microsoft.AspNetCore.Mvc;
using BookStore.Services;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookStoreController : ControllerBase
    {
        private readonly ILogger<BookStoreController> _logger;
        private readonly BookService _bookService;

        public BookStoreController(ILogger<BookStoreController> logger, BookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public IEnumerable<BookDto> GetBooks()
        {
            _logger.LogInformation("Fetching list of books from JSON file.");

            var books = _bookService.LoadBooksFromFile();
            return books ?? Enumerable.Empty<BookDto>();
        }
    }
}
