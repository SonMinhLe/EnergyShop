using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Categories
{
    public interface IPublicCategoryService
    {
        Task<PagedResult<CategoryViewModel>> GetAllByCategoryId(GetPublicCategoryPagingRequest request);

        Task<List<CategoryViewModel>> GetAll();

    }
}
