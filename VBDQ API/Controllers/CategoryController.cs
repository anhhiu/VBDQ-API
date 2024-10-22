using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategorySevice sevice;

        public CategoryController(ICategorySevice sevice)
        {
            this.sevice = sevice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var (category, mes) = await sevice.GetAllCategory();

            if (mes.Error != null) return BadRequest(mes.Status);
            
            return Ok(category);    
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var (category, mes) = await sevice.GetCategoryById(id);

            if (mes.Error != null) return NotFound(mes.Status);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categorydto)
        {
            var (category, mes) = await sevice.AddCategory(categorydto);


            return Ok(category);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto, int id)
        {
            var (category, mes) = await sevice.UpdatedCategory(categoryDto, id);

            if (mes.Error != null) return BadRequest(mes.Status);
            return Ok(category);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var mes = await sevice.DeleteCategory(id);

            if (mes.Error != null) { return BadRequest(mes.Status); }
            return Ok(mes.Status);
        }


    }
}
