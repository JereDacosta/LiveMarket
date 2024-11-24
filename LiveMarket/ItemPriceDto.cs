namespace LiveMarket
{
    public sealed record ItemPriceDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
    }
}
