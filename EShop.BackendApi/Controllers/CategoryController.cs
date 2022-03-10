using EShop.Application.Catalog.Categories;
using EShop.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IPublicCategoryService _publicCategoryService;
        private readonly IManageCategoryService _manageCategoryService;


        public CategoryController(IPublicCategoryService publicCategoryService,

            IManageCategoryService manageCategoryService)
        {
            _publicCategoryService = publicCategoryService;
            _manageCategoryService = manageCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _publicCategoryService.GetAll();
            return Ok(products);
        }

        [HttpGet("public-paging")]
        public async Task<IActionResult> Get([FromQuery] GetPublicCategoryPagingRequest request)
        {
            var products = await _publicCategoryService.GetAllByCategoryId(request);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _manageCategoryService.GetById(id);
            if (product == null)

                return BadRequest("Cannot find the product");
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            var productId = await _manageCategoryService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _manageCategoryService.GetById(productId);
            return CreatedAtAction(nameof(Create), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request)
        {
            var affectedResult = await _manageCategoryService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageCategoryService.Delete(id);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

    }
}
