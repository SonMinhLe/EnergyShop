using EShop.Application.Common;
using EShop.Data.EF;
using EShop.Data.Entities;
using EShop.Utilities.Exceptions;
using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Categories
{
    public class ManageCategoryService : IManageCategoryService
    {

        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageCategoryService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;//ham dung de luu anh
        }

        public Task AddViewcount(int productId)
        {
            throw new NotImplementedException();
        }


        //Create an entity
        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                Alias = request.Alias,
                Description = request.Description,
                Image = await this.SaveFile(request.Image)
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.ID;
        }

        // Save Image
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Categories.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find an Category: {productId}");
            _context.Categories.Remove(product);
            return await _context.SaveChangesAsync();
        }



        public async Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageCategoryPagingReuest request)
        {
            //1. Select join
            var query = from p in _context.Categories
                        select new { p };
            //2.filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (request.CategoryIds.Count > 0)
            {
                //query = query.Where(p => request.CategoryIds.Contains(p.CategoryIds));

            }
            //3.Paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.PageSize - 1) * request.PageSize).Take(request.PageSize)
            .Take(request.PageSize)
            .Select(x => new CategoryViewModel()
            {
                ID = x.p.ID,
                Name = x.p.Name,
                Alias = x.p.Alias,
                Description = x.p.Description,
                //Image = x.p.await this.SaveFile(request.Image)
            }).ToListAsync();
            //4. Select and projection
            var pageResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecord = totalRow,
                // Items = data
            };
            return pageResult;
        }

        public async Task<CategoryViewModel> GetById(int productId)
        {
            var product = await _context.Categories.FindAsync(productId);

            var CategoryViewModel = new CategoryViewModel()
            {
                ID = product.ID,
                Name = product.Name,
                Alias = product.Alias,
                Description = product.Description

            };
            return CategoryViewModel;
        }

        public async Task<int> Update(CategoryUpdateRequest request)
        {
            var product = await _context.Categories.FindAsync(request.ID);

            if (product == null) throw new EShopException($"Cannot find a product with id: {request.ID}");


            product.Name = request.Name;
            product.Alias = request.Alias;
            product.Description = request.Description;
            //Save image
            product.Image = await this.SaveFile(request.Image);
            return await _context.SaveChangesAsync();
        }

    }

}
