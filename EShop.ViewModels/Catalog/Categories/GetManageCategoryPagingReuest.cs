using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Catalog.Categories
{
    public class GetManageCategoryPagingReuest
    {
        public string Keyword { get; set; }

        public List<int> CategoryIds { get; set; }
        public int PageSize { get; set; }

    }
}
