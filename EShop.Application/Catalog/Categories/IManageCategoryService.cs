using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Categories
{
    public interface IManageCategoryService
    {
        Task<int> Create(CategoryCreateRequest request);

        Task<int> Update(CategoryUpdateRequest request);

        Task<int> Delete(int productId);

        Task<CategoryViewModel> GetById(int productId);



        Task AddViewcount(int productId);


        Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageCategoryPagingReuest request);


    }

}
