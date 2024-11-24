using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LiveMarket.Services.Books;
using LiveMarket.Services.VideoGame;

namespace LiveMarket.Controllers
{
    [ApiController]
    [Route("Books")]
    public sealed class BooksController : ControllerBase
    {
        private readonly IBookService _bookservice;
        private readonly IConversionService _conversionService;

        public BooksController(IBookService bookService, IConversionService conversionService)
        {
            this._bookservice = bookService;
            _conversionService = conversionService;
        }

        [HttpGet]
        [SwaggerOperation("Get the list of all books", description: "This calls has memory leaks.")]
        public async Task<IEnumerable<BookDto>> GetBooks([FromQuery] CurrencyType? conversionType = null)
        {
            var books = await _bookservice.GetBooks();

            if (conversionType.HasValue)
            {
                books = await _conversionService.GetConvertedBookPriceAsync(books, conversionType.Value);
            }

            return books;
        }
    }
}
