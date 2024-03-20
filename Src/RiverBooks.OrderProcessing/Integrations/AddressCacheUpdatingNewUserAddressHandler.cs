using MediatR;

using Microsoft.Extensions.Logging;

using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressCacheUpdatingNewUserAddressHandler(
    IOrderAddressCache addressCache,
    ILogger<AddressCacheUpdatingNewUserAddressHandler> logger)
    : INotificationHandler<NewUserAddressAddedIntegrationEvent>
{
    public async Task Handle(NewUserAddressAddedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var orderAddress = new OrderAddress(notification.Details.AddressId,
            new Address(
                notification.Details.Street1,
                notification.Details.Street2,
                notification.Details.City,
                notification.Details.State,
                notification.Details.PostalCode,
                notification.Details.Country));
        await addressCache.StoreAsync(orderAddress);

        logger.LogInformation("Cache updated with new address {address}", orderAddress.Id);
    }
}