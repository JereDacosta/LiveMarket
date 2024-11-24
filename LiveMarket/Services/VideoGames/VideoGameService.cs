using System.Text.Json;
using LiveMarket;
using LiveMarket.Services.VideoGame;

public sealed class VideoGameService : IVideoGameService
{
    private readonly ILogger<VideoGameService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public VideoGameService(ILogger<VideoGameService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<VideoGameDto>> GetVideoGamesAndPricesParallelized()
    {
        var xboxTask = this.GetGamesByStoreAsync("http://xbox-store:8080/XboxStore");
        var playstationTask = this.GetGamesByStoreAsync("http://playstation-store:8080/PlayStationStore");

        var results = await Task.WhenAll(xboxTask, playstationTask);

        var videoGames = results.SelectMany(games => games).ToList();

        return videoGames;
    }

    public async Task<IEnumerable<VideoGameDto>> GetVideoGamesAndPrices()
    {
        var videoGames = new List<VideoGameDto>();

        var xboxResults = await this.GetGamesByStoreAsync("http://xbox-store:8080/XboxStore");
        var playstationResults =  await this.GetGamesByStoreAsync("http://playstation-store:8080/PlayStationStore");

        videoGames.AddRange(xboxResults);
        videoGames.AddRange(playstationResults);

        return videoGames;
    }

    private async Task<IEnumerable<VideoGameDto>> GetGamesByStoreAsync(string storeUri)
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

                var data = JsonSerializer.Deserialize<List<VideoGameDto>>(json, options);

                if (data != null)
                {
                    return data;
                }
            }
            else
            {
                // TODO : Better logs ?
                _logger.LogError($"Error calling the API: {storeResponse.StatusCode}");
            }
        }
        catch (Exception e)
        {
            // TODO : Better logs ?
            _logger.LogError($"Error calling the API: {e}");
        }

        return [];
    }
}
