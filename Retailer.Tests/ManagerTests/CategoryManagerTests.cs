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
    public class CategoryManagerTests
    {
        private ICategoryManager _categoryManager;
        private RepositoryFactoryMock _repositoryFactoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryFactoryMock = new RepositoryFactoryMock();

            var mapperConfiguration = CreateMapperConfiguration();
            var mapper = mapperConfiguration.CreateMapper();

            _categoryManager = new CategoryManager(
                 _repositoryFactoryMock.GetFactoryMock(),
                 mapper
                );
        }

        [Test]
        public async Task GetAllCategories_ReturnTwoCategories()
        {
            _repositoryFactoryMock.CategoryRepositoryMock.Setup(x => x
                .FindMany(It.IsAny<Expression<Func<Category, bool>>>(), true, It.IsAny<string[]>()))
                .Returns(GetCategoryList());

            var categories = await _categoryManager.GetAllCategories();

            _repositoryFactoryMock.CategoryRepositoryMock.Verify(x => x
                .FindMany(It.IsAny<Expression<Func<Category, bool>>>(), true, It.IsAny<string[]>()),
                Times.AtLeastOnce);

            Assert.That(categories.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCategoryById_IdOne_ReturnCategory()
        {
            var categories = await GetCategoryList();
            _repositoryFactoryMock.CategoryRepositoryMock.Setup(x => x.FindOne(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<bool>(),
                It.IsAny<string[]>()))
                .Returns(GetCategory());

            var category = await _categoryManager.GetCategory(1);

            _repositoryFactoryMock.CategoryRepositoryMock.Verify(x => x.FindOne(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<bool>(),
                It.IsAny<string[]>()), Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(category, Is.Not.Null);
                Assert.That(category, Is.TypeOf<CategoryDto>());
                Assert.That(categories, Is.Ordered.By("Id"));
                Assert.That(category.Name.Count, Is.GreaterThan(1));

                Assert.That(category, Has.Property("Id"));
                Assert.That(categories, Has.Member(categories.FirstOrDefault()));

                Assert.That(category.Name.ToLower(), Does.StartWith("v"));
                Assert.That(category.Name.ToLower(), Does.Contain("o"));
            });


        }

        private async static Task<IQueryable<Category>> GetCategoryList()
        {
            var categoryList = new List<Category>();
            var task = Task.Run(() =>
            {
                categoryList.AddRange(new List<Category>
             {
                 new Category{Name = "Volvo"},
                 new Category{Name = "Audi"}

             });
            });


            await task;

            return categoryList.AsQueryable();
        }

        private async static Task<Category> GetCategory()
        {
            var category = new Category();
            var task = Task.Run(() =>
            {
                category.Name = "Volvo";
            });
           
            await task;

            return category;

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


