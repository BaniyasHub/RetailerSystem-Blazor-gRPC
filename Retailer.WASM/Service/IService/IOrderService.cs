using Retailer.Interface.Dtos;

namespace Retailer.WASM.Service.IService
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrder(int orderHeaderId);

        Task<List<OrderDto>> GetAllOrders();

        Task<OrderDto> CreateOrder(OrderDto orderDto);

        Task<OrderHeaderDto> MarkPaymentSuccessfull(OrderHeaderDto orderHeaderDto);
    }
}
