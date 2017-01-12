using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Search
{
    public class PageableSearch : IPageable
    {
        #region IPageable
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }
        #endregion
    }
}
