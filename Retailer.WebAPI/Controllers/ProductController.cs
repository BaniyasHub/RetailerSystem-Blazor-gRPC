using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;

namespace Retailer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productManager.GetAllProducts());
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new ErrorModelDto { StatusCode = StatusCodes.Status400BadRequest, ErrorMessage = "Id is corrupted" });
            }

            var product = await _productManager.GetProduct(id.Value);


            await Task.Delay(5000);
            System.Diagnostics.Debug.WriteLine("HEYYYYYYYYYYYYYYYYYYYYYYYYYYYY");

            if (product == null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "There is no product with that id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            //return Ok(new SuccessModelDto { Data =  product, StatusCode = StatusCodes.Status200OK });
            return Ok(product);
        }

        [HttpGet("productPriceId/{id:int}")]
        public async Task<IActionResult> GetProductPriceById(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new ErrorModelDto { StatusCode = StatusCodes.Status400BadRequest, ErrorMessage = "Id is corrupted" });
            }

            var productPrice = await _productManager.GetProductPriceById(id.Value);

            await Task.Delay(5000);
            System.Diagnostics.Debug.WriteLine("HEYYYYYYYYYYYYYYYYYYYYYYYYYYYY");

            if (productPrice == null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "There is no product with that id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            //return Ok(new SuccessModelDto { Data =  product, StatusCode = StatusCodes.Status200OK });
            return Ok(productPrice);
        }
    }
}
