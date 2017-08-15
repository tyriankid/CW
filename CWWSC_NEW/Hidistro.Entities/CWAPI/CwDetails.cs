using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwDetails
    {
        /// <summary>
        /// 商品内码
        /// </summary>
        public string GoodsCode { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int OrderQuantity { get; set; }

        /// <summary>
        /// 商品原单价
        /// </summary>
        public decimal OrderFprice { get; set; }

        /// <summary>
        /// 商品内部结算价
        /// </summary>
        public decimal OrderRePrice { get; set; }

    }
}
