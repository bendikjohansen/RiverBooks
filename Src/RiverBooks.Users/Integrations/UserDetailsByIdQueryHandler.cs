using Ardalis.Result;

using MediatR;

using RiverBooks.Users.Contracts;
using RiverBooks.Users.UseCases.User;

namespace RiverBooks.Users.Integrations;

internal class UserDetailsByIdQueryHandler(IMediator mediator) : IRequestHandler<UserDetailsByIdQuery, Result<UserDetails>>
{
    public async Task<Result<UserDetails>> Handle(UserDetailsByIdQuery request, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(request.UserId);
        var result = await mediator.Send(query, ct);
        return result.Map(u => new UserDetails(u.UserId, u.EmailAddress));
    }

}