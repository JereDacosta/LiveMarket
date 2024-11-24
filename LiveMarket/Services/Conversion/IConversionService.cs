namespace LiveMarket.Services.VideoGame
{
    public interface IConversionService
    {
        public Task<IEnumerable<VideoGameDto>> GetConvertedGamePriceAsync(IEnumerable<VideoGameDto> videoGames, CurrencyType currencyType);

        public Task<IEnumerable<BookDto>> GetConvertedBookPriceAsync(IEnumerable<BookDto> books, CurrencyType currencyType);
    }
}
