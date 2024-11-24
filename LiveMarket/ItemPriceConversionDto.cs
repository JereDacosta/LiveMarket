namespace LiveMarket
{
    public sealed record ItemPriceConversionDto
    {
        public IEnumerable<ItemPriceDto> ItemPrices { get; set; } = [];
        public CurrencyType CurrencyType { get; set; }
    }
}
