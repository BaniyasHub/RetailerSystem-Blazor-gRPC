using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Retailer.Interface.Dtos;
using Retailer.WASM.Service.IService;

namespace Retailer.WASM.Service
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OrderDto> GetOrder(int orderHeaderId)
        {
            //Creating Client via HttpClientFactory
            var client = _httpClientFactory.CreateClient("order");

            //Sending request with url/routeParameters/requestBody/requestHeaders
            var response = await client.GetAsync($"GetOrder/{orderHeaderId}");

            //Getting content of the response as Json
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                //if status code is Ok, we deserialize the content as the dto model(json and c# property names must match in this case)
                var order = JsonConvert.DeserializeObject<OrderDto>(content);
                if (order != null)
                {
                    return order;
                }
            }

            //If status code is not ok, we are deserializing the content as ErrorModel(again properties!!)
            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<List<OrderDto>> GetAllOrders()
        {
            var client = _httpClientFactory.CreateClient("order");
            var response = await client.GetAsync("getall");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orderList = JsonConvert.DeserializeObject<List<OrderDto>>(content);

                if (orderList != null)
                {
                    return orderList;
                }
            }

            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);

            throw new Exception(errorModel?.ErrorMessage);
        }

        public async Task<OrderDto> CreateOrder(OrderDto orderDto)
        {
            var client = _httpClientFactory.CreateClient("order");

            var jsonObject = JsonConvert.SerializeObject(orderDto);

            var requestBody = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Create", requestBody);

            var content = await response.Content.ReadAsStringAsync();


            if (response.StatusCode == HttpStatusCode.OK)
            {
                var order = JsonConvert.DeserializeObject<OrderDto>(content);

                return order;
            }

            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);

            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessfull(OrderHeaderDto orderHeaderDto)
        {
            var client = _httpClientFactory.CreateClient("order");

            var requestContent = JsonConvert.SerializeObject(orderHeaderDto);
            var requestBody = new StringContent(requestContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("markPaymentSuccessfull", requestBody);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(responseContent);

                return orderHeader;
            }

            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(responseContent);

            throw new Exception(errorModel.ErrorMessage);
        }

    }
}
