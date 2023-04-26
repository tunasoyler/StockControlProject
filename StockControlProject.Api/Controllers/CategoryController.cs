using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Entities.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly IGenericService<Category> service;

        public CategoryController(IGenericService<Category> _categoryService)
        {
            service = _categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(service.GetAll());
        }

        [HttpGet]
        public IActionResult GetActiveCategories()
        {
            return Ok(service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            return Ok(service.GetById(id));
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            service.Add(category);
            return CreatedAtAction("GetCategoryById", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();
            try
            {
                service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!service.Any(x => x.Id == id))
                    return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = service.GetById(id);
            if (category == null)
                return NotFound();
            try
            {
                service.Remove(category);
                return Ok("Category deleted");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult ActivateCategory(int id)
        {
            var category = service.GetById(id);
            if(category == null)
                return NotFound();
            try
            {
                service.Activate(id);
                return Ok(category);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
