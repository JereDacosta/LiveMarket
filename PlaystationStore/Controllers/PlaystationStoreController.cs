using Microsoft.AspNetCore.Mvc;

namespace PlayStationStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayStationStoreController : ControllerBase
    {
        private readonly ILogger<PlayStationStoreController> _logger;

        public PlayStationStoreController(ILogger<PlayStationStoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "PlayStationGames")]
        public IEnumerable<VideoGame> Get()
        {
            return new List<VideoGame>
            {
                new VideoGame
                {
                    Id = Guid.NewGuid(),
                    Title = "The Last of Us Part II",
                    ReleaseDate = new DateOnly(2020, 6, 19),
                    Genre = "Action-Adventure",
                    Price = 59.99m,
                    Description = "A story-driven game about survival and revenge in a post-apocalyptic world."
                },
                new VideoGame
                {
                    Id = Guid.NewGuid(),
                    Title = "Spider-Man: Miles Morales",
                    ReleaseDate = new DateOnly(2020, 11, 12),
                    Genre = "Action-Adventure",
                    Price = 49.99m,
                    Description = "Swing through New York City as Miles Morales in this thrilling superhero adventure."
                },
                new VideoGame
                {
                    Id = Guid.NewGuid(),
                    Title = "Demon's Souls",
                    ReleaseDate = new DateOnly(2020, 11, 12),
                    Genre = "Action RPG",
                    Price = 69.99m,
                    Description = "A remake of the classic game that defined the action RPG genre, set in a dark fantasy world."
                },
                new VideoGame
                {
                    Id = Guid.NewGuid(),
                    Title = "Ratchet & Clank: Rift Apart",
                    ReleaseDate = new DateOnly(2021, 6, 11),
                    Genre = "Platformer",
                    Price = 69.99m,
                    Description = "An interdimensional adventure featuring stunning visuals and creative gameplay."
                }
            };
        }
    }
}
