using Retailer.Grpc.Protos;

namespace Retailer.Grpc.Services
{
    public interface IGrpcClient
    {
        Task<string> GetGreeting();

        Task<CustomerModel> GetCustomerById(int id);

        Task<List<CustomerModel>> GetNewCustomers();

        Task<bool> AddNewCustomer(CustomerModel customer);
    }
}