using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorClass
    {
        /// <summary>
        ///
        /// </summary>
        public Guid DcID { get; set; }

        public int DisUserId { get; set; }

        public int State { get; set; }

        public DateTime ApplyDate { get; set; }

        public string ScIDs { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditRemark { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

    }
}
