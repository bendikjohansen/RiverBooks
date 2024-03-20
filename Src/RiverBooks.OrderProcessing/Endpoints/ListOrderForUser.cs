using System.Security.Claims;

using Ardalis.Result;

using FastEndpoints;

using MediatR;

using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrderForUser(IMediator mediator) : EndpointWithoutRequest<ListOrdersForUserResponse>
{
    private const string EmailAddress = "EmailAddress";

    public override void Configure()
    {
        Get("/orders");
        Claims(EmailAddress);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var emailAddress = User.FindFirstValue(EmailAddress)!;
        var query = new ListOrdersForUserQuery(emailAddress);
        var result = await mediator.Send(query, ct);

        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(ct);
        }
        else
        {
            var response = new ListOrdersForUserResponse(result.Value
                .Select(o => new OrderSummary(o.UserId, o.DateCreated, o.DateShipped, o.Total, o.UserId))
                .ToList());

            await SendAsync(response, cancellation: ct);
        }
    }
}

internal record ListOrdersForUserQuery(string EmailAddress) : IRequest<Result<List<OrderSummary>>>;

internal class ListOrdersForUserQueryHandler(IOrderRepository orderRepository) : IRequestHandler<ListOrdersForUserQuery, Result<List<OrderSummary>>>
{
    public async Task<Result<List<OrderSummary>>> Handle(ListOrdersForUserQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.ListAsync();

        // TODO: Filter by user
        var summaries = orders.Select(o => new OrderSummary(o.UserId,
                o.DateCreated,
                null,
                o.OrderItems.Sum(oi => oi.UnitPrice),
                o.Id))
            .ToList();

        return summaries;
    }
}

public record ListOrdersForUserResponse(List<OrderSummary> Orders);

public record OrderSummary(Guid UserId, DateTimeOffset DateCreated, DateTimeOffset? DateShipped, decimal Total, Guid OrderId);