using System;
using System.Collections.Generic;
using System.Text;

namespace Project.ViewModel.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}