using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Catalog.Categories
{
    public class CategoryViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }


        public string Alias { set; get; }


        public string Description { set; get; }


        public IFormFile Image { set; get; }

    }
}
