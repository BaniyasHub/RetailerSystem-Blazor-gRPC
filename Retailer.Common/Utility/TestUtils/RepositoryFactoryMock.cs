using Moq;
using Retailer.Data.Models;
using Retailer.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Common.Utility.TestUtils
{
    public class RepositoryFactoryMock
    {
        public Mock<IRepositoryFactory> FactoryMock = new Mock<IRepositoryFactory>();
        public Mock<IRepository<Category>> CategoryRepositoryMock = new Mock<IRepository<Category>>();
        public Mock<IRepository<Product>> ProductRepositoryMock = new Mock<IRepository<Product>>();
        public Mock<IRepository<ProductPrice>> ProductPriceRepositoryMock = new Mock<IRepository<ProductPrice>>();

        public virtual IRepositoryFactory GetFactoryMock()
        {
            FactoryMock.Setup(u => u.CategoryRepository).Returns(CategoryRepositoryMock?.Object);
            FactoryMock.Setup(u => u.ProductRepository).Returns(ProductRepositoryMock?.Object);
            FactoryMock.Setup(u => u.ProductPriceRepository).Returns(ProductPriceRepositoryMock?.Object);

            return FactoryMock.Object;
        }
    }
}
