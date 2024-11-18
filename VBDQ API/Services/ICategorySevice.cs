using VBDQ_API.Conmon;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ICategorySevice
    {
        public Task<(IEnumerable<CategoryDto>, Mess)> GetAllCategory();
        public Task<(CategoryDto, Mess)> GetCategoryById(int id);
        public Task<(CategoryDto, Mess)> AddCategory(CategoryDto categoryDto);
        public Task<(CategoryDto, Mess)> UpdatedCategory(CategoryDto categoryDto,int id);
        public Task<Mess> DeleteCategory(int id);

        Task<ServiceResponse<dynamic>> GetAllCategoryAsync();
      //  Task<ServiceResponse<dynamic>> GetAllCategoryAsync2(int pageNumber, int pageSize);
        Task<ServiceResponse<dynamic>> CreateCategoryAsync(CategoryCreate model);
        Task<ServiceResponse<dynamic>> UpdateCategoryAsync(CategoryUpdate model, int id);
        Task<ServiceResponse<dynamic>> GetCategoryByIdAsync(int id);
        Task<ServiceResponse<dynamic>> DeleteCategoryByIdAsync(int id);
    }
}
