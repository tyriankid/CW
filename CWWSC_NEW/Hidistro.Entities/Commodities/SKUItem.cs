namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SKUItem : IComparable
    {
        private Dictionary<int, decimal> memberPrices;
        private Dictionary<int, decimal> pfPrices;
        private Dictionary<int, int> skuItems;

        public int CompareTo(object obj)
        {
            SKUItem item = obj as SKUItem;
            if (item == null)
            {
                return -1;
            }
            if (item.SkuItems.Count != this.SkuItems.Count)
            {
                return -1;
            }
            foreach (int num in item.SkuItems.Keys)
            {
                if (item.SkuItems[num] != this.SkuItems[num])
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 内购价
        /// </summary>
        public decimal NeigouPrice { get; set; }

        public decimal CostPrice { get; set; }

        public Dictionary<int, decimal> MemberPrices
        {
            get
            {
                return (this.memberPrices ?? (this.memberPrices = new Dictionary<int, decimal>()));
            }
        }

        public Dictionary<int, decimal> PFPrices
        {
            get
            {
                return (this.pfPrices ?? (this.pfPrices = new Dictionary<int, decimal>()));
            }
        }

        public int ProductId { get; set; }

        public decimal SalePrice { get; set; }

        public string SKU { get; set; }

        public string SkuId { get; set; }

        public Dictionary<int, int> SkuItems
        {
            get
            {
                return (this.skuItems ?? (this.skuItems = new Dictionary<int, int>()));
            }
        }

        public int Stock { get; set; }

        public decimal Weight { get; set; }
    }
}

