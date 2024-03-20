using MediatR;

using Microsoft.Extensions.Logging;

using RiverBooks.Users.Domain;

namespace RiverBooks.Users;

internal class LogNewAddressHandler(ILogger<LogNewAddressHandler> logger) : INotificationHandler<AddressAddedEvent>
{
    public Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[DE Handler] New address added to user {user}: {address}",
            notification.Address.UserId,
            notification.Address.StreetAddress);

        return Task.CompletedTask;
    }
}