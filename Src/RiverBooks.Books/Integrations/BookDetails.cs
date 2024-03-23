using Ardalis.Result;

using MediatR;

using RiverBooks.Books.Contracts;

namespace RiverBooks.Books.Integrations;

internal class BookDetailsQueryHandler(IBookService bookService) : IRequestHandler<BookDetailsQuery, Result<BookDetailsResponse>>
{
    public async Task<Result<BookDetailsResponse>> Handle(BookDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await bookService.GetBookByIdAsync(request.BookId);

        if (result.Status == ResultStatus.NotFound)
        {
            return Result.NotFound();
        }

        var book = result.Value;
        var response = new BookDetailsResponse(book.Id, book.Title, book.Author, book.Price);
        return response;
    }
}