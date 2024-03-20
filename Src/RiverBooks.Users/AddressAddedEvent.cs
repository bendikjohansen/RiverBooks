using RiverBooks.Users.Data;

namespace RiverBooks.Users;

internal sealed record AddressAddedEvent(UserStreetAddress Address) : DomainEventBase;