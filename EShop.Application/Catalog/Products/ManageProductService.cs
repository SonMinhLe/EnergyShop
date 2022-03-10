using EShop.Data.EF;
using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Data.Enum;
using EShop.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using EShop.ViewModels.Common;
using EShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using EShop.Application.Common;
using EShop.Application.Catalog.Products.Dtos;

namespace EShop.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product();
            {
                product.Name = request.Name;
                product.Alias = request.Alias;
                product.CategoryID = request.CategoryID;
                if (request.Image != null)
                {
                    //product.Image = request.Image;
                    product.Image = await this.SaveFile(request.Image);
                }
                else
                {
                    product.Image = "no-image.jpg";
                }
                product.Price = request.Price;
                product.PromotionPrice = request.PromotionPrice;
                product.Warranty = request.Warranty;
                product.Description = request.Description;
                product.CreatedDate = DateTime.Now;
                product.Status = Status.Active;

            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.ID;
        }

        public async Task<int> RecycleBin(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new EShopException($"Cannot find a product: {productId}");
            else
                product.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingRequest request)
        {
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryID equals c.ID
                        select p;
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(p => p.Name.Contains(request.Keyword));

            if (request.CategoryIds.Count > 0)
                query = query.Where(p => request.CategoryIds.Contains(p.CategoryID));

            int totalRow = await query.CountAsync();
            var data = query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                        .Take(request.pageSize)
                        .Select(p => new ProductViewModel()
                        {
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            PromotionPrice = p.PromotionPrice,
                            Warranty = p.Warranty,

                        }).ToListAsync();

            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data,
            };
            return pagedResult;
        }

        public Task<List<ProductViewModel>> Search(string Keyword)
        {
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryID equals c.ID
                        select p;

            var data = query.Where(p => p.Name.Contains(Keyword)).Select(p => new ProductViewModel()
            {
                ID = p.ID,
                Name = p.Name,
                Alias = p.Alias,
                CategoryID = p.CategoryID,
                Image = p.Image != null ? p.Image : "no-image.jpg",
                Description = p.Description,
                Price = p.Price,
                PromotionPrice = p.PromotionPrice,
                Warranty = p.Warranty,
                Status = p.Status

            }).ToListAsync();

            return data;

        }

        public async Task<ProductViewModel> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            var categories = await (from c in _context.Categories
                                    join p in _context.Products on c.ID equals p.CategoryID
                                    where p.ID == productId
                                    select c.Name).ToListAsync();


            var productViewModel = new ProductViewModel()
            {
                ID = product.ID,
                Name = product.Name,
                Alias = product.Alias,
                CategoryID = product.CategoryID,
                Image = product.Image != null ? product.Image : "no-image.jpg",
                Price = product.Price,
                PromotionPrice = product.PromotionPrice != null ? product.PromotionPrice : null,
                Warranty = product.Warranty != null ? product.Warranty : null,
                Description = product.Description != null ? product.Description : null,
                Status = product.Status
            };
            return productViewModel;
        }

        public Task<List<ProductViewModel>> RecycleBin()
        {

            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryID equals c.ID
                        select p;

            var data = query.Where(p => p.Status == Status.InActive).Select(p => new ProductViewModel()
            {
                ID = p.ID,
                Name = p.Name,
                Alias = p.Alias,
                CategoryID = p.CategoryID,
                Image = p.Image != null ? p.Image : "no-image.jpg",
                Description = p.Description,
                Price = p.Price,
                PromotionPrice = p.PromotionPrice,
                Warranty = p.Warranty,
                Status = p.Status

            }).ToListAsync();
            return data;

        }

        public async Task<int> Restore(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new EShopException($"Cannot find a product: {productId}");
            else
                product.Status = Status.Active;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UploadImage(int productId, IFormFile Image)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new EShopException($"Cannot find a product: {productId}");
            else
                product.Image = await this.SaveFile(Image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.id);
            if (product == null)
                throw new EShopException($"Cannot find a product: {request.id}");
            else
            {
                product.Name = request.name;
                product.Alias = request.alias;
                product.CategoryID = request.categoryID;
                product.Image = request.Image/*await this.SaveFile(request.Image)*/;
                product.Price = request.price;
                product.PromotionPrice = request.promotionPrice;
                product.Warranty = request.warranty;
                product.Description = request.description;
                product.UpdatedDate = DateTime.Now;
                product.Status = request.status;
                return await _context.SaveChangesAsync();

            }

        }

        private async Task<string> SaveFile(IFormFile file)
        {          
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = originalFileName;
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> Delete(int productId)
        {

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find an Category: {productId}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
    }
}
