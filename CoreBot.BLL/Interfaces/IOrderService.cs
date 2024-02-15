using System.Threading.Tasks;
using CoreBot.BLL.Dto;
using CoreBot.DAL.Models;

public interface IOrderService
{
    Task BuyKeyAsync(OrderDto orderDto);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}