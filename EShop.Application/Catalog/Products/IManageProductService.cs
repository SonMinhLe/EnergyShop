using EShop.Application.Catalog.Products.Dtos;
using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> RecycleBin(int productId);

        Task<int> Delete(int productId);


        Task<int> Restore(int productId);

        Task<int> UploadImage(int productId, IFormFile Image);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingRequest request);

        Task<ProductViewModel> GetById(int productId);

        Task<List<ProductViewModel>> RecycleBin();

        Task<List<ProductViewModel>> Search(string Keyword);

        

    }
}
