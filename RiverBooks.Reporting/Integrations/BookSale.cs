namespace RiverBooks.Reporting.Integrations;

internal record BookSale
{
    public string Author { get; init; } = string.Empty;
    public Guid BookId { get; init; }
    public decimal TotalSales { get; init; }
    public int UnitsSold { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Year { get; init; }
    public int Month { get; init; }
}