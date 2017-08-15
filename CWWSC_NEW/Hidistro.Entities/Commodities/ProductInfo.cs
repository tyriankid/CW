namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ProductInfo
    {
        private SKUItem defaultSku;
        private Dictionary<string, SKUItem> skus;

        public DateTime AddedDate { get; set; }

        public int? BrandId { get; set; }

        public int CategoryId { get; set; }

        public decimal CostPrice
        {
            get
            {
                return this.DefaultSku.CostPrice;
            }
        }

        public SKUItem DefaultSku
        {
            get
            {
                return (this.defaultSku ?? (this.defaultSku = (this.Skus.Count > 0 ? this.Skus.Values.First<SKUItem>() : null)));
            }
        }

        public string Description { get; set; }

        public string specification { get; set; }

        public int DisplaySequence { get; set; }

        public string ExtendCategoryPath { get; set; }

        public bool HasSKU { get; set; }

        public string ImageUrl1 { get; set; }

        public string ImageUrl2 { get; set; }

        public string ImageUrl3 { get; set; }

        public string ImageUrl4 { get; set; }

        public string ImageUrl5 { get; set; }

        public bool IsfreeShipping { get; set; }

        public string MainCategoryPath { get; set; }

        public decimal? MarketPrice { get; set; }

        public int RestrictNeigouNum { get; set; }//限购数量

        public decimal MaxSalePrice
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in from sku in this.Skus.Values
                    where sku.SalePrice > maxSalePrice[0]
                    select sku)
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
            }
        }

        public decimal MinSalePrice
        {
            get
            {
                decimal[] minSalePrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in from sku in this.Skus.Values
                    where sku.SalePrice < minSalePrice[0]
                    select sku)
                {
                    minSalePrice[0] = item.SalePrice;
                }
                return minSalePrice[0];
            }
        }

        /// <summary>
        /// 内购最高价
        /// </summary>
        public decimal MaxNeigouPrice
        {
            get
            {
                decimal[] maxNeigouPrice = new decimal[1];
                foreach (SKUItem item in from sku in this.Skus.Values
                                         where sku.NeigouPrice > maxNeigouPrice[0]
                                         select sku)
                {
                    maxNeigouPrice[0] = item.NeigouPrice;
                }
                return maxNeigouPrice[0];
            }
        }

        /// <summary>
        /// 内购最低价
        /// </summary>
        public decimal MinNeigouPrice
        {
            get
            {
                decimal[] minNeigouPrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in from sku in this.Skus.Values
                                         where sku.NeigouPrice < minNeigouPrice[0]
                                         select sku)
                {
                    minNeigouPrice[0] = item.NeigouPrice;
                }
                return minNeigouPrice[0];
            }
        }

        public decimal MinCostPrice
        {
            get
            {
                decimal[] minCostPrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in from sku in this.Skus.Values
                                         where sku.CostPrice < minCostPrice[0]
                                         select sku)
                {
                    minCostPrice[0] = item.CostPrice;
                }
                return minCostPrice[0];
            }
        }

        public string ProductCode { get; set; }

        /// <summary>
        /// 酷开商品编码,2017-03-02添加
        /// </summary>
        public string KukaiCode { get; set; }

        public int ProductId { get; set; }

        [HtmlCoding]
        public string ProductName { get; set; }

        public int SaleCounts { get; set; }

        public decimal SalePrice { get; set; }

        public ProductSaleStatus SaleStatus { get; set; }

        [HtmlCoding]
        public string ShortDescription { get; set; }

        public int ShowSaleCounts { get; set; }

        public string SKU
        {
            get
            {
                //return this.DefaultSku.SKU;
                if (this.DefaultSku != null)
                    return this.DefaultSku.SKU;
                else
                    return null;
            }
        }

        public string SkuId
        {
            get
            {
                return this.DefaultSku.SkuId;
            }
        }

        public Dictionary<string, SKUItem> Skus
        {
            get
            {
                return (this.skus ?? (this.skus = new Dictionary<string, SKUItem>()));
            }
        }

        public int Stock
        {
            get
            {
                return this.Skus.Values.Sum<SKUItem>(((Func<SKUItem, int>) (sku => sku.Stock)));
            }
        }

        public long TaobaoProductId { get; set; }

        public string ThumbnailUrl100 { get; set; }

        public string ThumbnailUrl160 { get; set; }

        public string ThumbnailUrl180 { get; set; }

        public string ThumbnailUrl220 { get; set; }

        public string ThumbnailUrl310 { get; set; }

        public string ThumbnailUrl40 { get; set; }

        public string ThumbnailUrl410 { get; set; }

        public string ThumbnailUrl60 { get; set; }

        public int? TypeId { get; set; }

        public string Unit { get; set; }

        public int VistiCounts { get; set; }

        public decimal Weight
        {
            get
            {
                return this.DefaultSku.Weight;
            }
        }

        public int Range { get; set; }

        //商品来源
        public int ProductSource { get; set; }

        //供应商id
        public int SupplierId { get; set; }

        /// <summary>
        /// 所属服务品类
        /// </summary>
        public int? ClassId { get; set; }

        /// <summary>
        /// 产品Top枚举
        /// </summary>
        public enum ProductTop {
            /// <summary>
            /// 上新
            /// </summary>
            New,
            /// <summary>
            /// 热门
            /// </summary>
            Hot,
            /// <summary>
            /// 优惠
            /// </summary>
            Discount,
            /// <summary>
            /// 最喜欢
            /// </summary>
            MostLike,
            /// <summary>
            /// 活动商品
            /// </summary>
            Activity,
            /// <summary>
            /// 分类
            /// </summary>
            Category,
            /// <summary>
            /// 无
            /// </summary>
            No,
        }

        /// <summary>
        /// 产品限定范围枚举
        /// </summary>
        public enum ProductRanage
        {
            /// <summary>
            /// 正常显示店铺已上架的商品
            /// </summary>
            NormalSelect,
            /// <summary>
            /// 正常显示店铺未上架的商品
            /// </summary>
            NormalUnSelect,
            /// <summary>
            /// 显示所有出售状态的商品
            /// </summary>
            All,
            /// <summary>
            /// 根据上架范围显示已上架的商品
            /// </summary>
            RangeSelect,
            /// <summary>
            /// 根据上架范围显示未上架的商品
            /// </summary>
            RangeUnSelect,
        }

        //分公司Id（商品来源为4线下，此字段才生效）
        public int FilialeId { get; set; }

        /// <summary>
        /// 分公司的销售区域ID
        /// </summary>
        public int?RegionId { get; set; }

        public string RegionName { get; set; }

        /// <summary>
        /// 一口价返佣类型，0为返佣比例，1为返佣固定金额
        /// </summary>
        public int SaleCommisionType { get; set; }

        /// <summary>
        /// 一口价返佣比例
        /// </summary>
        public decimal SaleCommisionRatio { get; set; }

        /// <summary>
        /// 一口价返佣金额
        /// </summary>
        public decimal SaleCommisionMoney { get; set; }

        /// <summary>
        /// 内购价返佣类型，0为返佣比例，1为返佣固定金额
        /// </summary>
        public int NeigouCommisionType { get; set; }

        /// <summary>
        /// 内购价返佣比例
        /// </summary>
        public decimal NeigouCommisionRatio { get; set; }

        /// <summary>
        /// 内购价返佣金额
        /// </summary>
        public decimal NeigouCommisionMoney { get; set; }

        /// <summary>
        /// 批发价返佣类型，0为返佣比例，1为返佣固定金额
        /// </summary>
        public int wholesaleCommisionType { get; set; }

        /// <summary>
        /// 批发价返佣比例
        /// </summary>
        public decimal wholesaleCommisionRatio { get; set; }

        /// <summary>
        /// 批发价返佣金额
        /// </summary>
        public decimal wholesaleCommisionMoney { get; set; }

    }
}

