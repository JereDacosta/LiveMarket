namespace LiveMarket.Services.VideoGame
{
    public interface IVideoGameService
    {
        public Task<IEnumerable<VideoGameDto>> GetVideoGamesAndPricesParallelized();

        public Task<IEnumerable<VideoGameDto>> GetVideoGamesAndPrices();
    }
}
