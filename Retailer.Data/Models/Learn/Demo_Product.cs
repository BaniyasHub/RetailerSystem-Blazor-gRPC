using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Data.Models.Learn
{
    public class Demo_Product : BaseModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsActive { get; set; }

        public List<Demo_DD_Product> DD_Products { get; set; }
    }
}
