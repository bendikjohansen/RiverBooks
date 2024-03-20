using System.Text.Json;

using Ardalis.Result;

using Microsoft.Extensions.Logging;

using RiverBooks.OrderProcessing.Integrations;

using StackExchange.Redis;

namespace RiverBooks.OrderProcessing;

internal interface IOrderAddressCache
{
    Task<Result<OrderAddress>> GetAddressByIdAsync(Guid id);
    Task<Result> StoreAsync(OrderAddress orderAddress);
}

internal class RedisOrderAddressCache : IOrderAddressCache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisOrderAddressCache> _logger;

    public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
    {
        var redis = ConnectionMultiplexer.Connect("localhost"); // TODO: Get server from configuration
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<Result<OrderAddress>> GetAddressByIdAsync(Guid id)
    {
        var fetchedJson = await _db.StringGetAsync(id.ToString());
        if (string.IsNullOrEmpty(fetchedJson))
        {
            return Result.NotFound();
        }

        var address = JsonSerializer.Deserialize<OrderAddress>(fetchedJson!);
        if (address is null)
        {
            return Result.NotFound();
        }

        _logger.LogInformation("Address {id} returned from {db}", id, "REDIS");
        return Result.Success(address);
    }

    public async Task<Result> StoreAsync(OrderAddress orderAddress)
    {
        var key = orderAddress.Id.ToString();
        var addressJson = JsonSerializer.Serialize(orderAddress);

        await _db.StringSetAsync(key, addressJson);
        _logger.LogInformation("Address {id} stored in {db}", orderAddress.Id, "REDIS");

        return Result.Success();
    }
}