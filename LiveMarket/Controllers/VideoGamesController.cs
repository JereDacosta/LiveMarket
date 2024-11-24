using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LiveMarket.Services.VideoGame;

namespace LiveMarket.Controllers
{
    [ApiController]
    [Route("VideoGames")]
    public sealed class VideoGamesController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;
        private readonly IConversionService _conversionService;

        public VideoGamesController(IVideoGameService videoGameService, IConversionService conversionService)
        {
            this._videoGameService = videoGameService;
            _conversionService = conversionService;
        }

        [HttpGet("optimized")]
        [SwaggerOperation("Get the list of all video games (optimized)", description: "The calls to the game stores are parallelized. All game prices are converted in one call.")]
        public async Task<IEnumerable<VideoGameDto>> GetGamesOptimized([FromQuery] CurrencyType? conversionType = null)
        {
            var games = await _videoGameService.GetVideoGamesAndPricesParallelized();

            if (conversionType.HasValue)
            {
                games = await _conversionService.GetConvertedGamePriceAsync(games, conversionType.Value);
            }

            return games;
        }

        [HttpGet("nonoptimized")]
        [SwaggerOperation("Get the list of all video games (non-optimized)", description: "The calls to the game stores are not parallelized. All game prices are converted in one call.")]
        public async Task<IEnumerable<VideoGameDto>> GetGamesNonoptimized([FromQuery] CurrencyType? conversionType = null)
        {
            var games = await _videoGameService.GetVideoGamesAndPrices();

            if (conversionType.HasValue)
            {
                games = await _conversionService.GetConvertedGamePriceAsync(games, conversionType.Value);
            }

            return games;
        }

        [HttpGet("inefficient")]
        [SwaggerOperation("Get the list of all video games (inefficient)", description: "The calls to the game stores are not parallelized. Game price are fetched one by one. Really bad !!")]
        public async Task<IEnumerable<VideoGameDto>> GetGamesUnoptimized([FromQuery] CurrencyType? conversionType = null)
        {
            var games = await _videoGameService.GetVideoGamesAndPrices();

            IList<VideoGameDto> convertedGames = [];

            if (conversionType.HasValue)
            {
                foreach (var game in games) {
                    var convertedGame = await _conversionService.GetConvertedGamePriceAsync([game], conversionType.Value);

                    game.Price = convertedGame.FirstOrDefault()!.Price;

                    convertedGames.Add(game);
                }
            }

            return convertedGames;
        }
    }
}
