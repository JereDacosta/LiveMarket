using System.Text.Json;

namespace BookStore.Services
{
    public class BookService
    {
        private static readonly List<byte[]> _leakyMemory = new();
        private readonly ILogger<BookService> _logger;

        public BookService(ILogger<BookService> logger)
        {
            _logger = logger;
        }

        public List<BookDto>? LoadBooksFromFile()
        {
            try
            {
                // Simulate memory leak
                SimulateMemoryLeak();

                var jsonData = System.IO.File.ReadAllText("Resources/books.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<BookDto>>(jsonData, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load books from JSON file.");
                return null;
            }
        }

        // For simulation purposes
        private void SimulateMemoryLeak()
        {
            // Allocate 1 MB of memory on each call and add to the list
            var chunk = new byte[1024 * 1024]; // 1 MB
            _leakyMemory.Add(chunk);

            // Log the total size of the leaky memory
            _logger.LogWarning($"Simulated memory leak: Allocated {_leakyMemory.Count} MB.");
        }
    }

    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
