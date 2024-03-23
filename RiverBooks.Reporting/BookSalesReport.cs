namespace RiverBooks.Reporting;

public record BookSalesReport(Guid BookId, string Title, string Author, int Units, decimal Sales);