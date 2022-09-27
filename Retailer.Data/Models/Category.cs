using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Data.Models
{
    public class Category : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<Product> ProductList { get; set; }
    }
}
