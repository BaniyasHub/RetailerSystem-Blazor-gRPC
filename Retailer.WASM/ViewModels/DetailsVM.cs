using Retailer.Interface.Dtos;

namespace Retailer.WASM.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            ProductPrice = new();
            Quantity = 1;
        }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; } = new();

        public int ProductPriceId { get; set; }
        public ProductPriceDto ProductPrice { get; set; } = new();
    }
}
