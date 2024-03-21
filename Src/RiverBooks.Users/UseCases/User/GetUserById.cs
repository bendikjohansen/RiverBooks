using Ardalis.Result;

using MediatR;

using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.User;

internal record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

internal class GetUserByIdHandler(IApplicationUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.NotFound();
        }

        var userDto = new UserDto(Guid.Parse(user.Id), user.Email!);
        return Result.Success(userDto);
    }
}