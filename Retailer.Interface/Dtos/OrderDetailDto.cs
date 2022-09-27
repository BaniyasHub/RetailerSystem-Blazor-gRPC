using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }

        public int ProductId { get; set; }

        public ProductDto Product { get; set; } = new();

        public int Count { get; set; }

        public double Price { get; set; }

        public string Size { get; set; }

        public string ProductName { get; set; }
    }
}
