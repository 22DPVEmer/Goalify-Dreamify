using Backend_Goalify.Core.Models;

namespace Backend_Goalify.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
        Task<CategoryModel> GetCategoryByIdAsync(string id);
        Task<CategoryModel> CreateCategoryAsync(CategoryModel category);
        Task UpdateCategoryAsync(CategoryModel category);
        Task DeleteCategoryAsync(string id);
    }
} 