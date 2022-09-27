using AutoMapper;
using Moq;
using NUnit.Framework;
using Retailer.BlazorServer.GrpcClient.Protos;
using Retailer.Business.Managers;
using Retailer.Common.Utility.TestUtils;
using Retailer.Data.Models;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer = Retailer.Data.Models.Customer;

namespace Retailer.Tests.Managers
{
    [TestFixture]
    public class CustomerManagerTests
    {
        private ICustomerManager _customerManager;
        private RepositoryFactoryMock _repositoryFactoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfiguration = CreateMapperConfiguration();
            var mapper = mapperConfiguration.CreateMapper();

            _customerManager = new CustomerManager(_repositoryFactoryMock.GetFactoryMock(), mapper);
        }


        //[Test]
        //public void GetCustomerById()
        //{
        //    _repositoryFactoryMock.ProductRepositoryMock.Setup(x => x.FindOne(It.IsAny<int>()))
        //}


        private MapperConfiguration CreateMapperConfiguration()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>().ReverseMap();
                cfg.CreateMap<CustomerModel, CustomerDto>().ReverseMap();
            });

            return mapper;
        }
    }
}
