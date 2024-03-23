CREATE SCHEMA IF NOT EXISTS Reporting;

CREATE TABLE IF NOT EXISTS Reporting.MonthlyBookSales
(
    BookId uuid,
    Title VARCHAR(128),
    Author VARCHAR(128),
    Year VARCHAR(128),
    Month VARCHAR(128),
    UnitsSold VARCHAR(128),
    PRIMARY KEY (BookId, Year, Month)
);
