using EShop.Data.EF;
using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Categories
{
    public class PublicCategoryService : IPublicCategoryService
    {
        private readonly EShopDbContext _context;

        public PublicCategoryService(EShopDbContext context)
        {
            _context = context;

        }
        public async Task<List<CategoryViewModel>> GetAll()
        {
            //1. Select join
            var query = from p in _context.Categories
                        select new { p };


            var data = await query.Select(x => new CategoryViewModel()
            {
                ID = x.p.ID,
                Name = x.p.Name,
                Alias = x.p.Alias,
                Description = x.p.Description,
            }).ToListAsync();

            return data;
        }

        public async Task<PagedResult<CategoryViewModel>> GetAllByCategoryId(GetPublicCategoryPagingRequest request)
        {

            //1. Select join
            var query = from p in _context.Categories
                        select new { p };

            //2.filter

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                // query = query.Where(p => p.CategoryId == request.CategoryId);

            }

            //3.Paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.pageSize - 1) * request.pageSize).Take(request.pageSize)
            .Take(request.pageSize)
            .Select(x => new CategoryViewModel()
            {
                ID = x.p.ID,
                Name = x.p.Name,
                Alias = x.p.Alias,
                Description = x.p.Description,


            }).ToListAsync();
            //4. Select and projection
            var pageResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecord = totalRow,
                // Items = data
            };
            return pageResult;


        }

    }
}
