using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class ProductPriceDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductDto Product { get; set; }

        public string Size { get; set; }

        public double Price { get; set; }
    }
}
