using RiverBooks.SharedKernel;

namespace RiverBooks.Users.Domain;

internal sealed record AddressAddedEvent(UserStreetAddress Address) : DomainEventBase;