using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductReservePriceInfo
    {
        /// <summary>
        ///商品价格预约实体
        /// </summary>
        public int ReserveId { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public DateTime StartDate { get; set; }
        public int State { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal NeigouPrice { get; set; }
        
    }
}
