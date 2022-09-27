using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Data.Models
{
    public class ProductPrice : BaseModel
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string Size { get; set; }

        public double Price { get; set; }
    }
}
