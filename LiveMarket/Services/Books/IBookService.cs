namespace LiveMarket.Services.Books
{
    public interface IBookService
    {
        public Task<IEnumerable<BookDto>> GetBooks();
    }
}
