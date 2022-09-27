using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Repository.IRepository
{
    public interface IRepositoryFactory
    {
        IRepository<Category> CategoryRepository { get; set; }

        IRepository<Product> ProductRepository { get; set; }

        IRepository<Customer> CustomerRepository { get; set; }

        IRepository<ProductPrice> ProductPriceRepository { get; set; }
        IRepository<OrderDetail> OrderDetailRepository { get; set; }
        IRepository<OrderHeader> OrderHeaderRepository { get; set; }

    }
}
