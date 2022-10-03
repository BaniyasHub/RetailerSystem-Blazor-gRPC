using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool ShopFavorites { get; set; }

        public bool CustomerFavorites { get; set; }

        public string Color { get; set; }

        public string ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public CategoryDto Category { get; set; } = new();

        public List<ProductPriceDto> ProductPriceList { get; set; } = new();
    }
}
