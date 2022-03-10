using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.Common
{
    public class PaggingRequestBase
    {
        public int pageIndex { set; get; }

        public int pageSize { set; get; }
    }
}
