using Retailer.Interface.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Interfaces.Managers
{
    public interface IOrderManager
    {
        Task<OrderDto> GetOrder(int orderId);

        Task<List<OrderDto>> GetAllOrders(string? userId = null, string? status = null);

        Task<OrderDto> CreateOrder(OrderDto order);

        Task DeleteOrder(int id);

        Task<OrderHeaderDto> UpdateHeader(OrderHeaderDto orderHeaderDto);

        Task<OrderHeaderDto> MarkPaymentSuccessfull(int id);

        Task<bool> UpdateOrderStatus(int orderId, string status);

    }
}
