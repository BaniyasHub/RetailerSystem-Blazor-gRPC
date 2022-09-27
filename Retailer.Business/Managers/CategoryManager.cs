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
    public class CategoryManager : ICategoryManager
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public CategoryManager(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categoryList = await _repositoryFactory.CategoryRepository.FindMany(CategoryBy.All());
            var categoryDtoList = _mapper.Map<List<CategoryDto>>(categoryList);
            return categoryDtoList;
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var category = await _repositoryFactory.CategoryRepository.FindOne(CategoryBy.Id(id));
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }

        public async Task AddCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _repositoryFactory.CategoryRepository.Add(category);
            await _repositoryFactory.CategoryRepository.SaveChanges();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _repositoryFactory.CategoryRepository.FindOne(CategoryBy.Id(id));

            await _repositoryFactory.CategoryRepository.Delete(category);
            await _repositoryFactory.CategoryRepository.SaveChanges();
        }

        public async Task UpdateCategory(CategoryDto categoryDto)
        {
            var categoryEntity = await _repositoryFactory.CategoryRepository.FindOne(CategoryBy.Id(categoryDto.Id));

            if (categoryEntity != null)
            {
                categoryEntity.Name = categoryDto.Name;
                categoryEntity.CreatedDate = categoryDto.CreatedDate;

                await _repositoryFactory.CategoryRepository.SaveChanges();
            }
        }

    }
}
