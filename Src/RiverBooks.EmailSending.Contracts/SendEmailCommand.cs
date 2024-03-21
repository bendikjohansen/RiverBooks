using Ardalis.Result;

using MediatR;

namespace RiverBooks.EmailSending.Contracts;

public record SendEmailCommand(string To, string From, string Subject, string Body) : IRequest<Result<Guid>>;