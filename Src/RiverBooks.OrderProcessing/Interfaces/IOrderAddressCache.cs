using Ardalis.Result;

using RiverBooks.OrderProcessing.Infrastructure;

namespace RiverBooks.OrderProcessing.Interfaces;

internal interface IOrderAddressCache
{
    Task<Result<OrderAddress>> GetAddressByIdAsync(Guid id);
    Task<Result> StoreAsync(OrderAddress orderAddress);
}