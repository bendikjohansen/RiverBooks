using Ardalis.Result;

using MediatR;

namespace RiverBooks.Books.Contracts;

public record BookDetailsResponse(Guid BookId, string Title, string Author, decimal Price);

public record BookDetailsQuery(Guid BookId) : IRequest<Result<BookDetailsResponse>>;