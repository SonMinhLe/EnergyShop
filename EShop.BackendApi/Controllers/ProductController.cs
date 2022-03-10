using EShop.Application.Catalog.Products;
using EShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        //https://localhost:5001/api/Product
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _publicProductService.GetAll();
            return Ok(products);
        }

        [HttpGet("RecycleBin")]
        public async Task<IActionResult> RecycleBin()
        {
            var products = await _manageProductService.RecycleBin();
            return Ok(products);
        }

        //https://localhost:5001/api/Product/public-paging
        [HttpGet("public-paging")]
        public async Task<IActionResult> Get([FromQuery]GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }

        //https://localhost:5001/api/Product/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _manageProductService.GetById(id);
            if (product == null)
                return BadRequest("Can't find product");
            return Ok(product);
        }


        [HttpPost("Create"), DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(request);
            if(productId == 0 )
                return BadRequest();

            var product = await _manageProductService.GetById(productId);

            return CreatedAtAction(nameof(GetById), new { id = productId},product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.id = Id;

            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok(affectedResult);
        }


        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageProductService.Delete(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpGet("recycle/{id}")]

        public async Task<IActionResult> Recycle(int id)
        {
            var affectedResult = await _manageProductService.RecycleBin(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok(affectedResult);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var affectedResult = await _manageProductService.Restore(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok(affectedResult);
        }

        [HttpGet("search={Keyword}")]
        public async Task<IActionResult> Search(string Keyword)
        {
            var product = await _manageProductService.Search(Keyword);
            if (product == null)
                return BadRequest();
            return Ok(product);
        }

        [HttpPatch("UploadImage/{id}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            var affectedResult = await _manageProductService.UploadImage(id, image);
            if (affectedResult == 0)
                return BadRequest();
            return Ok(affectedResult);
        }


    }
}
