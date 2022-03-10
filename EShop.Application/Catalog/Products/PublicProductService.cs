using EShop.Data.EF;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Data.Enum;
using Microsoft.EntityFrameworkCore;
using EShop.ViewModels.Common;
using EShop.ViewModels.Catalog.Products;

namespace EShop.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;
      
       public PublicProductService(EShopDbContext context)
        {
            _context = context;
       
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryID equals c.ID
                        select p;
            
            var data = query.Where(p => p.Status == Status.Active).Select(p => new ProductViewModel()
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
       
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryID equals c.ID
                        select p;

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryID == request.CategoryId);
            }

            int totalRow = await query.CountAsync();
            var data = query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize)
                        .Take(request.pageSize)
                        .Select(p => new ProductViewModel()
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

            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data,
            };
            return pagedResult;
        }

        
    }
}
