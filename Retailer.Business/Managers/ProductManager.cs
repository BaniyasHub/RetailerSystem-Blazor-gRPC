using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ProductManager : IProductManager
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public ProductManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        //Cancellation condition put temporary, it will be refactored to situation it's can't be null
        public async Task<List<ProductDto>> GetAllProducts(CancellationToken? cancellationToken = null)
        {
            var productList = await _repositoryFactory.ProductRepository.FindMany(ProductBy.All(), includePaths: new string[] { "Category", "ProductPriceList" });

            var productEntityList = new List<Product>();

            if (cancellationToken != null)
            {
                productEntityList = await productList.ToListAsync((CancellationToken)cancellationToken);
                var productDtoList1 = _mapper.Map<List<ProductDto>>(productEntityList);
                return productDtoList1;

            }
            var productDtoList2 = _mapper.Map<List<ProductDto>>(productList);

            return productDtoList2;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _repositoryFactory.ProductRepository.FindOne(ProductBy.Id(id), includeProperties: new string[] { "Category", "ProductPriceList" });
            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            await _repositoryFactory.ProductRepository.Add(product);
            await _repositoryFactory.ProductRepository.SaveChanges();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _repositoryFactory.ProductRepository.FindOne(ProductBy.Id(id));

            await _repositoryFactory.ProductRepository.Delete(product);
            await _repositoryFactory.ProductRepository.SaveChanges();
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            var productEntity = await _repositoryFactory.ProductRepository.FindOne(ProductBy.Id(productDto.Id));

            if (productEntity != null)
            {
                productEntity.Name = productDto.Name;
                productEntity.Description = productDto.Description;
                productEntity.ShopFavorites = productDto.ShopFavorites;
                productEntity.CustomerFavorites = productDto.CustomerFavorites;
                productEntity.Color = productDto.Color;
                productEntity.CategoryId = productDto.CategoryId;
                productEntity.ImageUrl = productDto.ImageUrl;

                await _repositoryFactory.ProductRepository.SaveChanges();
            }
        }

        public async Task<List<ProductDto>> GetProductsByIds(List<int> ids, CancellationToken? cancellationToken = null)
        {
            var productListQuery = await _repositoryFactory.ProductRepository.FindMany(ProductBy.ProductIds(ids), includePaths: new string[] { "Category", "ProductPriceList" });

            var productList = await productListQuery.ToListAsync(cancellationToken: (CancellationToken)cancellationToken);

            var productDtoList = _mapper.Map<List<ProductDto>>(productList);

            return productDtoList;
        }

        public async Task DeleteProductPriceById(int id)
        {
            var productPrice = await _repositoryFactory.ProductPriceRepository.FindOne(x => x.Id == id);

            await _repositoryFactory.ProductPriceRepository.Delete(productPrice);
            await _repositoryFactory.ProductPriceRepository.SaveChanges();
        }

        public async Task EditProductPrice(ProductPriceDto productPrice)
        {
            var productPriceEntity = await _repositoryFactory.ProductPriceRepository.FindOne(x => x.Id == productPrice.Id);

            if (productPriceEntity != null)
            {
                productPriceEntity.Price = productPrice.Price;
                productPriceEntity.Size = productPrice.Size;
            }

            await _repositoryFactory.ProductPriceRepository.SaveChanges();
        }

        public async Task AddProductPrice(ProductPriceDto productPrice)
        {
            var productPriceEntity = _mapper.Map<ProductPrice>(productPrice);

            await _repositoryFactory.ProductPriceRepository.Add(productPriceEntity);
            await _repositoryFactory.ProductPriceRepository.SaveChanges();
        }

        public async Task<ProductPriceDto> GetProductPriceById(int id)
        {
            var productPriceEntity = await _repositoryFactory.ProductPriceRepository.FindOne(x => x.Id == id, includeProperties: "Product");

            var productPriceDto = _mapper.Map<ProductPriceDto>(productPriceEntity);

            return productPriceDto;
        }


    }
}
