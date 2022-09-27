using Retailer.Data.Models;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<Category> CategoryRepository { get; set; }

        public IRepository<Product> ProductRepository { get; set; }

        public IRepository<Customer> CustomerRepository { get; set; }

        public IRepository<ProductPrice> ProductPriceRepository { get; set; }

        public IRepository<OrderDetail> OrderDetailRepository { get; set; }

        public IRepository<OrderHeader> OrderHeaderRepository { get; set; }

        public RepositoryFactory(RetailerDbContext context)
        {
            CategoryRepository = new Repository<Category>(context);

            ProductRepository = new Repository<Product>(context);

            CustomerRepository = new Repository<Customer>(context);

            ProductPriceRepository = new Repository<ProductPrice>(context);

            OrderDetailRepository = new Repository<OrderDetail>(context);

            OrderHeaderRepository = new Repository<OrderHeader>(context);
        }

    }
}
