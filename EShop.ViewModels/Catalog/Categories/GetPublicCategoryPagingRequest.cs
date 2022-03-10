using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Catalog.Categories
{
    public class GetPublicCategoryPagingRequest : PaggingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
