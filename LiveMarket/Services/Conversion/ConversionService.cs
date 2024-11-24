using System.Text.Json;
using LiveMarket;
using LiveMarket.Services.VideoGame;

public sealed class ConversionService : IConversionService
{
    private readonly ILogger<ConversionService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ConversionService(ILogger<ConversionService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<BookDto>> GetConvertedBookPriceAsync(IEnumerable<BookDto> books, CurrencyType currencyType)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var bookPriceDtos = books.Select(book => new ItemPriceDto
            {
                Id = book.Id,
                Price = book.Price
            }).ToList();

            var conversionRequest = new ItemPriceConversionDto
            {
                ItemPrices = bookPriceDtos,
                CurrencyType = currencyType
            };

            var response = await client.PostAsJsonAsync("http://conversion-service:8080/conversion/convertPrices", conversionRequest);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var convertedPrices = JsonSerializer.Deserialize<List<ItemPriceDto>>(json, options);

                var priceConvertedBooks = books.Select(book =>
                {
                    var convertedPrice = convertedPrices?.FirstOrDefault(cp => cp.Id == book.Id);
                    if (convertedPrice != null)
                    {
                        book.Price = convertedPrice.Price;
                    }
                    return book;
                }).ToList();

                return priceConvertedBooks;
            }
            else
            {
                _logger.LogError($"Error calling the conversion API: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occurred during API call: {e.Message}");
        }

        return Enumerable.Empty<BookDto>();
    }

    public async Task<IEnumerable<VideoGameDto>> GetConvertedGamePriceAsync(IEnumerable<VideoGameDto> videoGames, CurrencyType currencyType)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var videoGamePriceDtos = videoGames.Select(game => new ItemPriceDto
            {
                Id = game.Id,
                Price = game.Price
            }).ToList();

            var conversionRequest = new ItemPriceConversionDto
            {
                ItemPrices = videoGamePriceDtos,
                CurrencyType = currencyType
            };

            var response = await client.PostAsJsonAsync("http://conversion-service:8080/conversion/convertPrices", conversionRequest);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var convertedPrices = JsonSerializer.Deserialize<List<VideoGameDto>>(json, options);

                var priceConvertedGames = videoGames.Select(game =>
                {
                    var convertedPrice = convertedPrices?.FirstOrDefault(cp => cp.Id == game.Id);
                    if (convertedPrice != null)
                    {
                        game.Price = convertedPrice.Price;
                    }
                    return game;
                }).ToList();

                return priceConvertedGames;
            }
            else
            {
                _logger.LogError($"Error calling the conversion API: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occurred during API call: {e.Message}");
        }

        return [];
    }
}
