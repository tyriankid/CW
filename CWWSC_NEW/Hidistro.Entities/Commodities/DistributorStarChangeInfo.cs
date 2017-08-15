using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorStarChangeInfo
    {
        /// <summary>
        /// 星级评分后台修改记录实体
        /// </summary>
        public Guid ID { get; set; }
        public int DisUserId { get; set; }
        public bool ChangeType { get; set; }
        public decimal ChangeMark { get; set; }
        public DateTime OpDataTime { get; set; }
        public string OpReason { get; set; }
    }
}
