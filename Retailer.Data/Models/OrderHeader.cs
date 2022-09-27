using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Data.Models
{
    public class OrderHeader : BaseModel
    {
        public string UserId { get; set; }

        public List<OrderDetail> OrderDetails{ get; set; }

        public double OrderTotal { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public string Status { get; set; }

        //Stripe Payment
        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string StreetAddress { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Email { get; set; }
    }
}
