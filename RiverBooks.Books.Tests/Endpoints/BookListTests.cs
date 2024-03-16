using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using RiverBooks.Books.BookEndpoints;

namespace RiverBooks.Books.Tests.Endpoints;

[Collection(nameof(Fixture))]
public class BookListTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task ReturnsThreeBooksAsync()
    {
        var testResult = await fixture.Client.GETAsync<List, ListBooksResponse>();

        testResult.Response.EnsureSuccessStatusCode();
        testResult.Result.Books.Should().HaveCount(3);
    }
}