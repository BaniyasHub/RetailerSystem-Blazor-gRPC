using Retailer.WASM.ViewModels;

namespace Retailer.WASM.Service.IService
{
    public interface ICartService
    {
        Task DecrementCart(ShoppingCart shoppingCart);
        Task IncrementCart(ShoppingCart shoppingCart);

        event Action Onchange;
    }
}
