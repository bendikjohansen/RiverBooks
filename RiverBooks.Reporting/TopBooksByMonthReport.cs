namespace RiverBooks.Reporting;

public record TopBooksByMonthReport
{
    public int Year { get; init; }
    public int Month { get; init; }
    public string MonthName { get; init; } = string.Empty;
    public List<BookSalesReport> Reports { get; init; } = [];
}