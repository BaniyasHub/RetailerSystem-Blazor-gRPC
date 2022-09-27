using Blazored.LocalStorage;
using Retailer.Common.Utility;
using Retailer.WASM.Service.IService;
using Retailer.WASM.ViewModels;

namespace Retailer.WASM.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorageService;

        public event Action Onchange;

        public CartService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cartList = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

            //for (int i = 0; i < cartList.Count; i++)
            //{
            //    if (cartList[i].ProductId == cartToDecrement.ProductId && cartList[i].ProductPriceId == cartToDecrement.ProductPriceId)
            //    {
            //        if (cartList[i].Count == 1 || cartList[i].Count == 0)
            //        {
            //            cartList.Remove(cartList[i]);
            //        }
            //        else
            //        {
            //            cartList[i].Count -= 1;
            //        }
            //    }
            //}

            var shoppingCart = cartList.FirstOrDefault(x => x.ProductId == cartToDecrement.ProductId && x.ProductPriceId == cartToDecrement.ProductPriceId);

            if (shoppingCart?.Count > 0)
            {
                if (shoppingCart.Count == 1)
                {
                    cartList.Remove(shoppingCart);
                }
                else
                {
                    cartList.FirstOrDefault(x => x.ProductId == shoppingCart.ProductId && x.ProductPriceId == shoppingCart.ProductPriceId).Count -= 1;
                }
            }

            if (cartList == null)
            {
                cartList = new List<ShoppingCart>();
            }

            await _localStorageService.SetItemAsync(SD.ShoppingCart, cartList);
            Onchange.Invoke();

        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            var cartList = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;

            if (cartList == null)
            {
                cartList = new List<ShoppingCart>();
            }

            foreach (var cart in cartList)
            {
                if (cart.ProductId == cartToAdd.ProductId && cart.ProductPriceId == cartToAdd.ProductPriceId)
                {
                    itemInCart = true;
                    cart.Count += cartToAdd.Count;
                }
            }

            if (!itemInCart)
            {
                cartList.Add(new ShoppingCart
                {
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }

            await _localStorageService.SetItemAsync(SD.ShoppingCart, cartList);

            Onchange.Invoke();
        }
    }
}
