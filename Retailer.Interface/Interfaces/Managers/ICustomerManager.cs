using Retailer.Interface.Dtos;

namespace Retailer.Interface.Interfaces.Managers
{
    public interface ICustomerManager
    {
        Task AddCustomer(CustomerDto customerDto);
        Task DeleteCustomer(int id);
        Task<List<CustomerDto>> GetAllCustomers();
        Task<CustomerDto> GetCustomer(int id);
        Task UpdateCustomer(CustomerDto customerDto);
    }
}