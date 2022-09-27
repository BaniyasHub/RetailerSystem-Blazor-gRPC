using Retailer.Interface.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Interfaces.Managers
{
    public interface ICategoryManager
    {
        Task<List<CategoryDto>> GetAllCategories();

        Task<CategoryDto> GetCategory(int id);

        Task AddCategory(CategoryDto categoryDto);

        Task DeleteCategory(int id);

        Task UpdateCategory(CategoryDto categoryDto);
    }
}
