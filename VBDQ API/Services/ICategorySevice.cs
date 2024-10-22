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
    }
}
