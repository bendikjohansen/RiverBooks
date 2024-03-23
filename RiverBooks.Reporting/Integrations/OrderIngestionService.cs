using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace RiverBooks.Reporting.Integrations;

internal class OrderIngestionService(ILogger<OrderIngestionService> logger, IConfiguration configuration)
{
    private readonly string _connString = configuration.GetConnectionString("Reporting")!;
    private static bool _ensureTableCreated = false;

    private async Task CreateTableAsync()
    {
        const string sql = @"CREATE SCHEMA IF NOT EXISTS Reporting;

CREATE TABLE IF NOT EXISTS Reporting.MonthlyBookSales
(
    BookId uuid,
    Title VARCHAR(128),
    Author VARCHAR(128),
    Year INT,
    Month INT,
    UnitsSold INT, 
    TotalSales numeric(18, 6),
    PRIMARY KEY (BookId, Year, Month)
);";
        await using var conn = new NpgsqlConnection(_connString);
        logger.LogInformation("Executing query: {sql}", sql);
        await conn.ExecuteAsync(sql);

        _ensureTableCreated = true;
    }

    public async Task AddOrUpdateMonthlyBookSalesAsync(BookSale sale)
    {
        if (!_ensureTableCreated)
        {
            await CreateTableAsync();
        }

        const string existsQuery =
            "SELECT 1 FROM Reporting.MonthlyBookSales WHERE BookId = @BookId AND Year = @Year AND Month = @Month;";
        const string updateQuery = @"
    UPDATE Reporting.MonthlyBookSales
    SET UnitsSold = UnitsSold + @UnitsSold, TotalSales = TotalSales + @TotalSales
    WHERE BookId = @BookId AND Year = @Year AND Month = @Month;";
        const string insertQuery = @"
    INSERT INTO Reporting.MonthlyBookSales (BookId, Title, Author, Year, Month, UnitsSold, TotalSales)
    VALUES (@BookId, @Title, @Author, @Year, @Month, @UnitsSold, @TotalSales);";

        var param = new
        {
            sale.BookId,
            sale.Title,
            sale.Author,
            sale.Year,
            sale.Month,
            sale.UnitsSold,
            sale.TotalSales
        };
        await using var conn = new NpgsqlConnection(_connString);
        logger.LogInformation("Executing exists query: {sql}", existsQuery);
        if ((await conn.QueryAsync(existsQuery, param)).Any())
        {
            logger.LogInformation("Executing update query: {sql}", updateQuery);
            await conn.ExecuteAsync(updateQuery, param);
        }
        else
        {
            logger.LogInformation("Executing insert query: {sql}", updateQuery);
            await conn.ExecuteAsync(insertQuery, param);
        }
    }
}