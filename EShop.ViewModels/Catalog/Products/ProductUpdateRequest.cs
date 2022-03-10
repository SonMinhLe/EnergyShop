using EShop.Data.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Catalog.Products
{
    public class ProductUpdateRequest
    {
        public int id { set; get; }
        public string name { set; get; }

        public string alias { set; get; }

        public int categoryID { set; get; }

        public string Image { set; get; }

        public decimal price { set; get; }

        public decimal? promotionPrice { set; get; }
        public int? warranty { set; get; }

        public string description { set; get; }

        public Status status { set; get; }

    }
}
