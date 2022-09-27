using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Dtos
{
    public class SuccessModelDto
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public object Data { get; set; }
    }
}
