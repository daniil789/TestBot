// OrderRepository.cs
using CoreBot.DAL.Models;

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
}
