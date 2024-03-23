using MediatR;

using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Domain;

internal class SendConfirmationEmailOrderCreatedEventHandler(IMediator mediator) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        var userByIdQuery = new UserDetailsByIdQuery(notification.Order.UserId);
        var result = await mediator.Send(userByIdQuery, ct);
        if (!result.IsSuccess)
        {
            return;
        }

        var userEmail = result.Value.EmailAddress;

        var command = new SendEmailCommand(userEmail,
            "noreply@riverbooks.com",
            "Your RiverBooks purchase",
            $"You bought {notification.Order.OrderItems.Count} items.");

        _ = await mediator.Send(command, ct);
    }
}