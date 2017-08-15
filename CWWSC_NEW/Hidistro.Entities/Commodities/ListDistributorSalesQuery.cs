using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Commodities
{
    public class ListDistributorSalesQuery : Pagination
    {
        /// <summary>
        /// 所属门店ID
        /// </summary>
        public int DisUserId { get; set; }

        public string DsName { get; set; }

        public string DsPhone { get; set; }

        public string KeyValue { get; set; }
    }
}
