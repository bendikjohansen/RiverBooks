using System.Net;

using FastEndpoints;
using FastEndpoints.Testing;

using FluentAssertions;

using RiverBooks.Books.BookEndpoints;

namespace RiverBooks.Books.Tests.Endpoints;

[Collection(nameof(Fixture))]
public class UpdatePriceTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task UpdatesPriceExpectedly()
    {
        var createdBook = await CreateBook();
        var updateBookRequest = new UpdateBookPriceRequest(createdBook.Id, 9.99M);
        var updateResponse = await fixture.Client.POSTAsync<UpdatePrice, UpdateBookPriceRequest, BookDto>(updateBookRequest);
        updateResponse.Response.EnsureSuccessStatusCode();
        updateResponse.Result.Price.Should().Be(9.99M);
    }

    [Fact]
    public async Task DoesNotAllowNegativePrices()
    {
        var createdBook = await CreateBook();
        var updateBookRequest = new UpdateBookPriceRequest(createdBook.Id, -9.99M);
        var updateResponse = await fixture.Client.POSTAsync<UpdatePrice, UpdateBookPriceRequest, BookDto>(updateBookRequest);
        updateResponse.Response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    private async Task<BookDto> CreateBook()
    {
        var createBookRequest = new CreateBookRequest
        {
            Id = Guid.NewGuid(),
            Title = "Modular Monoliths - Getting Started",
            Author = "Steve Smith",
            Price = 29.99M
        };
        var createResponse = await fixture.Client.POSTAsync<Create, CreateBookRequest, BookDto>(createBookRequest);
        createResponse.Response.EnsureSuccessStatusCode();
        return createResponse.Result;
    }
}