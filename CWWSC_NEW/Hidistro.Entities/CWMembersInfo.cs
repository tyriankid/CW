namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CWMenbersInfo
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string accountALLHere { get; set; }
        public string StoreName { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public int RelationUserId { get; set; }
        public string UserCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductModel { get; set; }
        public string Price { get; set; }
        public string BuyNum { get; set; }
        public int  UserId { get; set; }
        public int  StoreId { get; set; }
    }
}
