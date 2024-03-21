using Ardalis.Result;

using MediatR;

namespace RiverBooks.Users.Contracts;

public record UserDetailsByIdQuery(Guid UserId) : IRequest<Result<UserDetails>>;

public record UserDetails(Guid UserId, string EmailAddress);