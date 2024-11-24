using System.Text.Json;
using XboxStore;
using XboxStore.Services;

namespace XboxService.Services
{
    public class XboxGameService : IXboxGameService
    {
        static readonly string XboxGamesResourcesFile = "Resources/xbox.json";
        static readonly string XboxLegacyGamesResourcesFile = "Resources/xbox-legacy.json";

        private readonly ILogger<XboxGameService> _logger;

        public XboxGameService(ILogger<XboxGameService> logger)
        {
            this._logger = logger;
        }

        public Task<List<VideoGame>> GetXboxGames()
        {
            var xboxGames = LoadXboxGames(XboxGamesResourcesFile);
            var xboxLegacyGames = LoadXboxGames(XboxLegacyGamesResourcesFile);

            return Task.FromResult(xboxGames.Concat(xboxLegacyGames).ToList());
        }

        private IList<VideoGame> LoadXboxGames(string resourceUri)
        {
            try
            {
                var jsonString = File.ReadAllText(resourceUri);

                return JsonSerializer.Deserialize<List<VideoGame>>(jsonString) ?? new List<VideoGame>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load xbox games: Resource file {resourceUri} Exception {ex.Message}");
                return new List<VideoGame>();
            }
        }
    }
}
