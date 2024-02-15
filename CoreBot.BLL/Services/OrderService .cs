using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot.DAL.Intefaces;
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
        private readonly IKeyRepository _keyRepository;

        public OrderService(IOrderRepository orderRepository, IKeyRepository keyRepository)
        {
            _orderRepository = orderRepository;
            _keyRepository = keyRepository;
        }

        public async Task BuyKeyAsync(OrderDto orderDto)
        {
            var keys = await _keyRepository.GetKeysByGameIdAsync(orderDto.GameId);
            var keyId = keys.FirstOrDefault().Id;
            var order = new Order
            {
                UserId = orderDto.UserId,
                KeyId = keyId,
                OrderDate = DateTime.UtcNow,
                Status = "Оплачен"// или используйте соответствующий формат даты
            };

            await _orderRepository.BuyKeyAsync(order);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
    }
}
