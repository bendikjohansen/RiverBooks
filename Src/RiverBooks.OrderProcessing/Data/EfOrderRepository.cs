using Microsoft.EntityFrameworkCore;

namespace RiverBooks.OrderProcessing.Data;

internal interface IOrderRepository
{
    Task<List<Order>> ListAsync();
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}

internal class EfOrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
    public Task<List<Order>> ListAsync() => dbContext.Orders.Include(o => o.OrderItems).ToListAsync();

    public async Task AddAsync(Order order) => await dbContext.Orders.AddAsync(order);

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}