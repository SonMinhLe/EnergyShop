using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.Products.Dtos
{
    public class GetProductPaggingRequest : PaggingRequestBase
    {
        public string Keyword { set; get; }

        public List<int> CategoryIds { set; get; }
    }
}
