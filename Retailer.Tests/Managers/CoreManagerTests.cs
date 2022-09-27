using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Retailer.Business.Managers;
using Retailer.Common.Utility.TestUtils;
using Retailer.Data.Models;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Tests.Managers
{
    [TestFixture]
    public class CoreManagerTests
    {
        private ICategoryManager _coreManager;
        private RepositoryFactoryMock _repositoryFactoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryFactoryMock = new RepositoryFactoryMock();
            var mapperConfiguration = CreateMapperConfiguration();
            var mapper = mapperConfiguration.CreateMapper();

            _coreManager = new CategoryManager(
                 _repositoryFactoryMock.GetFactoryMock(),
                 mapper
                );
        }

        [Test]
        public async Task GetAllCategories()
        {
            _repositoryFactoryMock.CategoryRepositoryMock.Setup(x => x
                .FindMany(It.IsAny<Expression<Func<Category, bool>>>(), true, It.IsAny<string[]>()))
                .Returns(GetCategory());

            await _coreManager.GetAllCategories();

            _repositoryFactoryMock.CategoryRepositoryMock.Verify(x => x
                .FindMany(It.IsAny<Expression<Func<Category, bool>>>(), true, It.IsAny<string[]>()),
                Times.AtLeastOnce);
        }

        private async static Task<IQueryable<Category>> GetCategory()
        {
            var categoryList = new List<Category>
            {
                new Category
                {
                    Name = "Volvo"
                },
               new Category
                {
                    Name = "Audi"
                }
            };

            return categoryList.AsQueryable();
        }

        private MapperConfiguration CreateMapperConfiguration()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
            });

            return mapper;
        }
    }
}
