using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Commodities
{
    public class ListProductVirtualInfoQuery : Pagination
    {
        public string  keyword { get; set; }
        public int ProductId { get; set; }
        public string SkuId { get; set; }
        public int? states { get; set; }
    }
}
