using MediatR;

using Microsoft.Extensions.Logging;

using RiverBooks.Users.Contracts;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Integrations;

internal class UserAddressIntegrationEventDispatcherHandler(
    IPublisher mediator,
    ILogger<UserAddressIntegrationEventDispatcherHandler> logger) : INotificationHandler<AddressAddedEvent>
{
    public async Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(notification.Address.UserId);
        var address = notification.Address.StreetAddress;
        var userAddressDetails = new UserAddressDetails(
            userId,
            notification.Address.Id,
            address.Street1,
            address.Street2,
            address.City,
            address.State,
            address.PostalCode,
            address.Country
        );
        await mediator.Publish(new NewUserAddressAddedIntegrationEvent(userAddressDetails), cancellationToken);

        logger.LogInformation("[DE Handler] New address integration event sent for user {user}: {address}",
            notification.Address.UserId,
            notification.Address.Id);
    }
}