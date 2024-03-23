using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RiverBooks.Reporting.UseCases;

namespace RiverBooks.Reporting.Endpoints;

public record TopSalesByMonthResponse
{
    public TopBooksByMonthReport Report { get; init; } = new();
}

public record TopSalesByMonthRequest([FromQuery] int Year, [FromQuery] int Month);

internal class TopSalesByMonth(IMediator mediator) : Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
    public override void Configure()
    {
        Get("/topsales");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TopSalesByMonthRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new TopBooksByMonthQuery(req.Year, req.Month), ct);

        var response = new TopSalesByMonthResponse { Report = result };
        await SendAsync(response, cancellation: ct);
    }
}