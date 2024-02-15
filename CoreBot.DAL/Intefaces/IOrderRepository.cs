// IOrderRepository.cs
using CoreBot.DAL.Models;

public interface IOrderRepository
{
    Task BuyKeyAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}