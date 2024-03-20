using Ardalis.Result;

using MediatR;

using Microsoft.Extensions.Logging;

using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Infrastructure;

internal class ReadThroughOrderAddressCache(
    RedisOrderAddressCache redisCache,
    IMediator mediator,
    ILogger<ReadThroughOrderAddressCache> logger) : IOrderAddressCache
{
    public async Task<Result<OrderAddress>> GetAddressByIdAsync(Guid id)
    {
        var result = await redisCache.GetAddressByIdAsync(id);
        if (result.IsSuccess)
        {
            return result;
        }

        if (result.Status == ResultStatus.NotFound)
        {
            logger.LogInformation("Address {id} not found; fetching from source.", id);
            var query = new UserAddressDetailsByIdQuery(id);
            var queryResult = await mediator.Send(query);

            if (queryResult.IsSuccess)
            {
                var dto = queryResult.Value;
                var address = new Address(dto.Street1, dto.Street2, dto.City, dto.State, dto.PostalCode, dto.Country);
                var orderAddress = new OrderAddress(dto.AddressId, address);
                await StoreAsync(orderAddress);
                return orderAddress;
            }
        }

        return Result.NotFound();
    }

    public Task<Result> StoreAsync(OrderAddress orderAddress) => redisCache.StoreAsync(orderAddress);
}