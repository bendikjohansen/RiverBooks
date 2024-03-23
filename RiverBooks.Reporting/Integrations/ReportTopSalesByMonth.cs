using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace RiverBooks.Reporting.Integrations;

internal interface IReportTopSalesByMonth
{
    Task<IEnumerable<BookSale>> TopSalesByMonth(int year, int month);
}

internal class DefaultReportTopSalesByMonth(ILogger<OrderIngestionService> logger, IConfiguration configuration) : IReportTopSalesByMonth
{
    private readonly string _connString = configuration.GetConnectionString("Reporting")!;

    public async Task<IEnumerable<BookSale>> TopSalesByMonth(int year, int month)
    {
        const string sql = "SELECT * FROM reporting.monthlybooksales WHERE Year = @Year AND Month = @Month ORDER BY totalsales DESC;";

        await using var conn = new NpgsqlConnection(_connString);
        logger.LogInformation("Executing query: {sql}", sql);
        var response = await conn.QueryAsync<BookSale>(sql, new { year, month });
        return response;
    }
}