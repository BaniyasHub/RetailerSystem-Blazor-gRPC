using Grpc.Core;
using Grpc.Net.Client;
using Retailer.Grpc.Protos;
using System.Threading.Tasks;

namespace Retailer.Grpc.Services
{
    public class GrpcClient : IGrpcClient
    {
        private readonly GrpcChannel channel;
        private readonly Customer.CustomerClient customerClient;

        public GrpcClient()
        {
            channel = GrpcChannel.ForAddress("https://localhost:7251");
            customerClient = new Customer.CustomerClient(channel);
        }

        public async Task<string> GetGreeting()
        {
            var greeterClient = new Greeter.GreeterClient(channel);
            var input = new HelloRequest();
            input.Name = "Mustafa Suyi - Senior Software Developer";
            var reply = await greeterClient.SayHelloAsync(input);

            return reply.Message;
        }

        public async Task<CustomerModel> GetCustomerById(int id)
        {
            var input = new CustomerLookUpModel();
            input.CustomerId = id;

            var reply = await customerClient.GetCustomerInfoAsync(input);

            return reply;
        }

        public async Task<List<CustomerModel>> GetNewCustomers()
        {
            var input = new NewCustomerRequest();
            var reply = customerClient.GetNewCustomers(input);

            var customerList = new List<CustomerModel>();

            int a = 200;

            while (await reply.ResponseStream.MoveNext())
            {
                var currentCustomer = reply.ResponseStream.Current;

                currentCustomer.Age += a;
                a = a + 200;
                customerList.Add(currentCustomer);
            }

            return customerList;
        }

        public async Task<bool> AddNewCustomer(CustomerModel customer)
        {
            var response = await customerClient.AddCustomerAsync(customer);

            return response.IsAdded;
        }
    }
}
