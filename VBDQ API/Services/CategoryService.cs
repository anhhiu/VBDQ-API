using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class CategoryService : ICategorySevice
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public CategoryService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        public async Task<(CategoryDto, Mess)> AddCategory(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);

            await context.Categories.AddAsync(category);
                
            await context.SaveChangesAsync();

            var categoryclient = mapper.Map<CategoryDto>(category);

            return (categoryclient, new Mess {Error = null, Status ="sucess" });
            
        }

        public async Task<Mess> DeleteCategory(int id)
        {
           var category = await context.Categories.FindAsync(id);   
          
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return new Mess {Error =null, Status = "xoa thanh cong"};
            }
            return new Mess { Error = "500", Status = "Error"};
        }

        public async Task<(IEnumerable<CategoryDto>, Mess)> GetAllCategory()
        {
            try
            {
                var categories = await context.Categories.OrderByDescending(c => c.CategoryId).Include(c => c.Products).ToListAsync();
                var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);

                return (categoryDtos, new Mess { Error = null, Status = "success" });
            }
            catch (Exception ex)
            {
                // Log the exception (nếu cần thiết)
                return (null, new Mess { Error = ex.Message, Status = "failed" });
            }

        }

        public async Task<(CategoryDto, Mess)> GetCategoryById(int id )
        {
            var category = await context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return (null, new Mess { Error = "loi roi", Status = "loi" });
            }
            var categorydto = mapper.Map<CategoryDto>(category);
            
                return (categorydto, new Mess { Error = null, Status = "sucess" });
        }

        public async Task<(CategoryDto, Mess)> UpdatedCategory(CategoryDto categoryDto, int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return (null, new Mess { Error = "500", Status = "loi roi" });
            }

            mapper.Map(categoryDto, category);

            await context.SaveChangesAsync();

           var categoryclient = mapper.Map<CategoryDto>(category);

            return (categoryclient, new Mess { Error = null, Status = "cap nhat thanh cong" });
            
            
        }
    }
}
