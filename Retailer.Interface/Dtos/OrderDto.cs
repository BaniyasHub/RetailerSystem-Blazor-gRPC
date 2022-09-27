using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class OrderDto
    {
        public OrderHeaderDto OrderHeader { get; set; } = new();

        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }
}
