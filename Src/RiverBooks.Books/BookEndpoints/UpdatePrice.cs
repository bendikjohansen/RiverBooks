using FastEndpoints;

using FluentValidation;

namespace RiverBooks.Books.BookEndpoints;

public record UpdateBookPriceRequest(Guid Id, decimal NewPrice);

public class UpdateBookPriceRequestValidator : Validator<UpdateBookPriceRequest>
{
    public UpdateBookPriceRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("A book ID is required.");

        RuleFor(x => x.NewPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Book prices may not be negative.");
    }
}

internal class UpdatePrice(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
    public override void Configure()
    {
        Post("/books/{Id}/pricehistory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
    {
        // TODO: Handle not found
        await bookService.UpdateBookPriceAsync(req.Id, req.NewPrice);
        var updatedBook = await bookService.GetBookByIdAsync(req.Id);
        await SendAsync(updatedBook, cancellation: ct);
    }
}