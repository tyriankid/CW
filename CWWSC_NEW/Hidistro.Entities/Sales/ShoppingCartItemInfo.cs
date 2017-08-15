namespace Hidistro.Entities.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShoppingCartItemInfo
    {
        public decimal GetSubWeight()
        {
            return (this.Weight * this.Quantity);
        }



        public decimal AdjustedPrice { get; set; }

        public bool IsfreeShipping { get; set; }

        public bool IsSendGift { get; set; }

        public string MainCategoryPath { get; set; }

        public decimal MemberPrice { get; set; }

        public string Name { get; set; }

        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public int Quantity { get; set; }

        public int GiveQuantity { get; set; }

        public int HalfPriceQuantity { get; set; }

        public int ShippQuantity { get; set; }

        public string SKU { get; set; }

        public string SkuContent { get; set; }

        public string SkuId { get; set; }

        public decimal SubTotal
        {
            get
            {
                return (this.AdjustedPrice * (this.Quantity - GiveQuantity));
            }
        }

        public string ThumbnailUrl100 { get; set; }

        public string ThumbnailUrl40 { get; set; }

        public string ThumbnailUrl60 { get; set; }

        public int UserId { get; set; }

        public decimal Weight { get; set; }

        public int CategoryId { get; set; }

        //商品来源 线下商品标识  4
        public int ProductSource { get; set; }
        //供应商ID
        public int SupplierId { get; set; }

        //商品编码
        public string ProductCode { get; set; }

        //线下商品配送区域
        public string RegionId { get; set; }

        /// <summary>
        /// 分公司ID， 线下订单使用
        /// </summary>
        public int FilialeId { get; set; }

        /// <summary>
        /// 内购价格
        /// </summary>
        public decimal NeigouPrice { get; set; }
        
        /// <summary>
        /// 价格类型
        /// </summary>
        //public int PriceType { get; set; }
      
    }
}

