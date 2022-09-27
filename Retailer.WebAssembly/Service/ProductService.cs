using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Retailer.Interface.Dtos;
using Retailer.WebAssembly.Service.IService;

namespace Retailer.WebAssembly.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            //_httpClient = new HttpClient() { BaseAddress = new Uri(_configuration.GetValue<string>("BaseAPIUrl")) };
            _httpClient = new HttpClient() { BaseAddress = new Uri(_configuration.GetSection("BaseAPIUrl").Value) };
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("/api/Product");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(content);
                foreach (var product in products)
                {
                    product.ImageUrl = _configuration.GetSection("BaseServerUrl").Value + product.ImageUrl;
                }
                return products.ToList();
            }

            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
            throw new Exception(errorModel.ErrorMessage);
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Product/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(content);
                product.ImageUrl = _configuration.GetSection("BaseServerUrl").Value + product.ImageUrl;
                return product;
            }

            var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
            throw new Exception(errorModel.ErrorMessage);
        }
    }
}
