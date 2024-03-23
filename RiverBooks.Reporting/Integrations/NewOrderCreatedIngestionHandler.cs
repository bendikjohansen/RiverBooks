using MediatR;

using Microsoft.Extensions.Logging;

using RiverBooks.Books.Contracts;
using RiverBooks.OrderProcessing.Contracts;

namespace RiverBooks.Reporting.Integrations;

internal class NewOrderCreatedIngestionHandler : INotificationHandler<OrderCreatedIntegrationEvent>
{
    private readonly ILogger<NewOrderCreatedIngestionHandler> _logger;
    private readonly OrderIngestionService _orderIngestionService;
    private readonly IMediator _mediator;

    public NewOrderCreatedIngestionHandler(ILogger<NewOrderCreatedIngestionHandler> logger,
        OrderIngestionService orderIngestionService, IMediator mediator)
    {
        _logger = logger;
        _orderIngestionService = orderIngestionService;
        _mediator = mediator;
    }

    public async Task Handle(OrderCreatedIntegrationEvent notification, CancellationToken ct)
    {
        var orderItems = notification.OrderDetails.OrderItems;
        var year = notification.OrderDetails.DateCreated.Year;
        var month = notification.OrderDetails.DateCreated.Month;

        foreach (var item in orderItems)
        {
            var bookDetailsQuery = new BookDetailsQuery(item.BookId);
            var result = await _mediator.Send(bookDetailsQuery, ct);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Issue loading book details for {id}", item.BookId);
                continue;
            }

            var author = result.Value.Author;
            var title = result.Value.Title;

            var sale = new BookSale
            {
                Author = author,
                BookId = item.BookId,
                Month = month,
                Title = title,
                Year = year,
                TotalSales = item.Quantity * item.UnitPrice,
                UnitsSold = item.Quantity
            };

            await _orderIngestionService.AddOrUpdateMonthlyBookSalesAsync(sale);
        }
    }
}