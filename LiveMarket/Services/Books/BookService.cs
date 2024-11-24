using System.Text.Json;
using LiveMarket;
using LiveMarket.Services.Books;

public sealed class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public BookService(ILogger<BookService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<BookDto>> GetBooks()
    {
        var books = new List<BookDto>();

        var bookResults = await this.GetBookByStoreAsync("http://book-store:8080/books");

        books.AddRange(bookResults);

        return books;
    }

    private async Task<IEnumerable<BookDto>> GetBookByStoreAsync(string storeUri)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var storeResponse = await client.GetAsync(storeUri);

            if (storeResponse.IsSuccessStatusCode)
            {
                var json = await storeResponse.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<List<BookDto>>(json, options);

                if (data != null)
                {
                    return data;
                }
            }
            else
            {
                _logger.LogError($"Error calling the API: {storeResponse.StatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error calling the API: {e}");
        }

        return [];
    }
}
