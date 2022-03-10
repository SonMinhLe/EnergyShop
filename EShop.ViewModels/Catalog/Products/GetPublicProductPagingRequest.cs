using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.ViewModels.Common;

namespace EShop.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PaggingRequestBase
    {
        public int? CategoryId { set; get; }
    }
}
