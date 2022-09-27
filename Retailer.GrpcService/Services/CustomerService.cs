using AutoMapper;
using Grpc.Core;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;
using Retailer.GrpcService.Protos;

namespace Retailer.GrpcService.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerManager customerManager, IMapper mapper)
        {
            _customerManager = customerManager;
            _mapper = mapper;
        }

      
        public override async Task<CustomerAddResponse> AddCustomer(CustomerModel request, ServerCallContext context)
        {
            var customerDto = _mapper.Map<CustomerDto>(request);
            await _customerManager.AddCustomer(customerDto);

            return new CustomerAddResponse { IsAdded = true };
        }

        public override async Task<CustomerModel> GetCustomerInfo(CustomerLookUpModel request, ServerCallContext context)
        {
            var customerDto = await _customerManager.GetCustomer(request.CustomerId);

            var customerModel = _mapper.Map<CustomerModel>(customerDto);

            return customerModel;

        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            var customerDtoList = await _customerManager.GetAllCustomers();

            var customerModelList = _mapper.Map<List<CustomerModel>>(customerDtoList);

            foreach (var customerModel in customerModelList)
            {
                await responseStream.WriteAsync(customerModel);
            }
        }

        public override async Task<CustomerDeleteResponse> DeleteCustomer(CustomerLookUpModel request, ServerCallContext context)
        {
            await _customerManager.DeleteCustomer(request.CustomerId);

            return new CustomerDeleteResponse { IsDeleted = true };
        }
    }
}
