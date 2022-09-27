using AutoMapper;
using Microsoft.Extensions.Logging;
using Retailer.Business.Filters;
using Retailer.Data.Models;
using Retailer.DataAccess.Repository.IRepository;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Business.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public CustomerManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            var customerList = await _repositoryFactory.CustomerRepository.FindMany(CustomerBy.All());
            var customerDtoList = _mapper.Map<List<CustomerDto>>(customerList);
            return customerDtoList;
        }

        public async Task<CustomerDto> GetCustomer(int id)
        {
            var customer = await _repositoryFactory.CustomerRepository.FindOne(CustomerBy.Id(id));
            var customerDto = _mapper.Map<CustomerDto>(customer);

            return customerDto;
        }

        public async Task AddCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);

            await _repositoryFactory.CustomerRepository.Add(customer);
            await _repositoryFactory.CustomerRepository.SaveChanges();
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _repositoryFactory.CustomerRepository.FindOne(CustomerBy.Id(id));

            await _repositoryFactory.CustomerRepository.Delete(customer);
            await _repositoryFactory.CustomerRepository.SaveChanges();
        }

        public async Task UpdateCustomer(CustomerDto customerDto)
        {
            var customerEntity = await _repositoryFactory.CustomerRepository.FindOne(CustomerBy.Id(customerDto.Id));

            if (customerEntity != null)
            {
                customerEntity.FirstName = customerDto.FirstName;
                customerEntity.LastName = customerDto.LastName;
                customerEntity.Age = customerDto.Age;
                customerEntity.EmailAddress = customerDto.EmailAddress;
                customerEntity.IsAlive = customerDto.IsAlive;

                await _repositoryFactory.CustomerRepository.SaveChanges();
            }
        }

    }
}
