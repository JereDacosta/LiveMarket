namespace ConversionService.Services
{
    public interface IConversionService
    {
        Task<List<ItemPriceDto>> ConvertPricesFromUsdAsync(List<ItemPriceDto> videoGamePrices, CurrencyType currencyType);
    }
}
