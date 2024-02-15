// OrderRepository.cs
using CoreBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly GameStoreDbContext _context;

    public OrderRepository(GameStoreDbContext context)
    {
        _context = context;
    }

    public async Task BuyKeyAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Include(o=>o.Key).ThenInclude(k=>k.Game)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
}
