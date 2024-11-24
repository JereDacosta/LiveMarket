using System.Text.Json;

namespace ConversionService.Services
{
    public class ConversionFromUsdService : IConversionService
    {
        private readonly ILogger<ConversionFromUsdService> _logger;
        private Dictionary<string, decimal> _conversionRates;

        public ConversionFromUsdService(ILogger<ConversionFromUsdService> logger)
        {
            this._logger = logger;
            this._conversionRates = LoadConversionRates();
        }

        private Dictionary<string, decimal> LoadConversionRates()
        {
            try
            {
                var jsonString = File.ReadAllText("Resources/conversion.json");

                return JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString) ?? [];
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Failed to load conversion rates: {ex.Message}");
                return [];
            }
        }

        public async Task<List<ItemPriceDto>> ConvertPricesFromUsdAsync(List<ItemPriceDto> videoGamePrices, CurrencyType currencyType)
        {
            if (currencyType == CurrencyType.EXCEPTION) throw new ArgumentException($"Bad currency type : {currencyType}");

            var conversionRate = await GetConversionRate(currencyType);

            var convertedPrices = videoGamePrices.Select(priceDto => new ItemPriceDto
            {
                Id = priceDto.Id,
                Price = priceDto.Price * conversionRate
            }).ToList();

            return convertedPrices;
        }

        private Task<decimal> GetConversionRate(CurrencyType currencyType)
        {
            var currencyCode = currencyType.ToString();

            if (this._conversionRates.TryGetValue(currencyCode, out var rate))
            {
                return Task.FromResult(rate);
            }

            this._logger.LogWarning($"Conversion rate for {currencyCode} not found. Using default 1:1 rate.");
            return Task.FromResult(1m);
        }
    }
}
