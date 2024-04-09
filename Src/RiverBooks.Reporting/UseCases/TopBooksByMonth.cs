using MediatR;

using RiverBooks.Reporting.Integrations;

namespace RiverBooks.Reporting.UseCases;

internal record TopBooksByMonthQuery(int Year, int Month) : IRequest<TopBooksByMonthReport>;

internal class TopBooksByMonthQueryHandler(IReportTopSalesByMonth reportTopSalesByMonth) : IRequestHandler<TopBooksByMonthQuery, TopBooksByMonthReport>
{
    public async Task<TopBooksByMonthReport> Handle(TopBooksByMonthQuery request, CancellationToken cancellationToken)
    {
        var result = await reportTopSalesByMonth.TopSalesByMonth(request.Year, request.Month);

        var report = new TopBooksByMonthReport
        {
            Month = request.Month,
            Year = request.Year,
            MonthName = new DateTime(request.Year, request.Month, 1).ToString("Y").Split(" ")[0],
            Reports = result.Select(sale =>
                new BookSalesReport(sale.BookId, sale.Title, sale.Author, sale.UnitsSold, sale.TotalSales)).ToList()
        };

        return report;
    }
}