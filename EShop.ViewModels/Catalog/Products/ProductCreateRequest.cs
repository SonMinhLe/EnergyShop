using EShop.Data.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public IFormFile Image { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }
        public int? Warranty { set; get; }

        public string Description { set; get; }

        public Status Status { set; get; }



    }
}
