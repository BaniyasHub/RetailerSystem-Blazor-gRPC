using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Can't pass without given your name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Not without a date")]
        public DateTime? CreatedDate { get; set; }
    }
}
