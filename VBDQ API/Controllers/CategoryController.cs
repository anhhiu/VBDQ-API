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
            var response = await sevice.GetAllCategoryAsync();

            if (response != null) return StatusCode(response.StatusCode, response);
            
            return StatusCode(response.StatusCode, response.Message);    
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await sevice.GetCategoryByIdAsync(id);

            if (response != null) return StatusCode(response.StatusCode, response);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryCreate model)
        {
            var response = await sevice.CreateCategoryAsync(model);

            if (response != null) return StatusCode(response.StatusCode, response);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateCategory(CategoryUpdate model, int id)
        {
            var response = await sevice.UpdateCategoryAsync(model, id);

            if (response != null) return StatusCode(response.StatusCode, response);

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await sevice.DeleteCategoryByIdAsync(id);

            if (response != null) return StatusCode(response.StatusCode, response);

            return StatusCode(response.StatusCode, response.Message);
        }


    }
}
