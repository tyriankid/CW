using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorMarkInfo
    {
        /// <summary>
        ///星际评分实体对象
        /// </summary>
        public Guid ID { get; set; }
        public int DisUserId { get; set; }
        public int UserId { get; set; }
        public decimal Mark1 { get; set; }
        public decimal Mark2 { get; set; }
        public decimal Mark3 { get; set; }
        public decimal Mark4 { get; set; }
        public decimal Mark5 { get; set; }
        public decimal Total { get; set; }
    }
}
