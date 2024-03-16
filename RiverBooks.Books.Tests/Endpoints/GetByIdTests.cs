using FastEndpoints;
using FastEndpoints.Testing;

using FluentAssertions;

using RiverBooks.Books.BookEndpoints;

namespace RiverBooks.Books.Tests.Endpoints;

[Collection(nameof(Fixture))]
public class GetByIdTests(Fixture fixture) : TestBase<Fixture>
{
    [Theory]
    [InlineData("DFFE455B-8F20-4B08-9EC5-3B4A1FFC4D18", "The Fellowship of the Ring")]
    [InlineData("F8E9B841-E4AD-45ED-A68C-04E5EDD375FC", "The Two Towers")]
    [InlineData("BBB5A494-3CED-4E39-9BC9-59E1AC8123A1", "The Return of the King")]
    public async Task ReturnsExpectedBookGivenId(string bookId, string expectedTitle)
    {
        var request = new GetByIdRequest(Guid.Parse(bookId));
        var testResult = await fixture.Client.GETAsync<GetById, GetByIdRequest, BookDto>(request);

        testResult.Response.EnsureSuccessStatusCode();
        testResult.Result.Title.Should().Be(expectedTitle);
    }
}