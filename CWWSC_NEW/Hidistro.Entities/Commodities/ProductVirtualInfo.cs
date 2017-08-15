using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductVirtualInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int VirtualId { get; set; }
        public string VirtualCode { get; set; }
        public int VirtualState { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }

    }
}
