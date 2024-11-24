using ConversionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConversionService.Controllers
{
    [ApiController]
    [Route("conversion")]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;
        private readonly ILogger<ConversionController> _logger;

        public ConversionController(IConversionService conversionService, ILogger<ConversionController> logger)
        {
            _conversionService = conversionService;
            _logger = logger;
        }

        [HttpPost("convertPrices")]
        public async Task<IActionResult> ConvertPrices([FromBody] ItemPriceConversionDto itemGamePricesDto)
        {
            Thread.Sleep(10); // Simulate 10ms latency

            if (itemGamePricesDto == null || !itemGamePricesDto.ItemPrices.Any())
            {
                this._logger.LogError("No currency provided");
                return BadRequest("No prices provided.");
            }

            var convertedPrices = await _conversionService.ConvertPricesFromUsdAsync(itemGamePricesDto.ItemPrices.ToList(), itemGamePricesDto.CurrencyType);

            return Ok(convertedPrices);
        }
    }
}
