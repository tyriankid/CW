using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductPfPriceInfo
    {
        //主键
        public int PfId { get; set; }

        //商品ID
        public int ProductId { get; set; }

        //数量
        public int Num { get; set; }

        //价格
        public decimal PFSalePrice { get; set; }

        //是否只适用于门店
        public int IsStore { get; set; }
    }
}
