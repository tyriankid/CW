namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductQuery : Pagination
    {
        public int? BrandId { get; set; }

        //服务品类
        public int? ClassId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsIncludeHomeProduct { get; set; }

        public bool? IsIncludePromotionProduct { get; set; }

        public int? IsMakeTaobao { get; set; }

        [HtmlCoding]
        public string Keywords { get; set; }

        public string MaiCategoryPath { get; set; }

        public decimal? MaxSalePrice { get; set; }

        public decimal? MinSalePrice { get; set; }

        [HtmlCoding]
        public string ProductCode { get; set; }

        public ProductSaleStatus SaleStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public int? TagId { get; set; }

        public int? TopicId { get; set; }

        public int? TypeId { get; set; }

        public int? SourceId { get; set; }

        public string ProductIds { get; set; }

        public string SkuIds { get; set; }

        public string CommoditySource { get; set; }

        public string CommodityCode { get; set; }

        public string ProductName { get; set; }

        /// <summary>
        /// 商品来源（商品类型）1-为创维商品；2-供应商商品；3-服务商品；4-虚拟商品；5-线下商品
        /// </summary>
        public int ProductSource { get; set; }

        public int SupplierId { get; set; }
        /// <summary>
        /// 商品所属门店
        /// </summary>
        public int? FilialeId { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public int? RegionId { get; set; }
    }
}

