using System.Threading.Tasks;
using CoreBot.BLL.Dto;

public interface IOrderService
{
    Task BuyKeyAsync(OrderDto orderDto);
}