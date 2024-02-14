using CoreBot.BLL.Dto;
using CoreBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task BuyKeyAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                KeyId = orderDto.KeyId,
                OrderDate = DateTime.UtcNow // или используйте соответствующий формат даты
            };

            await _orderRepository.BuyKeyAsync(order);
        }
    }
}
