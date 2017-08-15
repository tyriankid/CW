using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorSales
    {
        /// <summary>
        ///
        /// </summary>
        public Guid DsID { get; set; }
        public int DisUserId { get; set; }
        public int SaleUserId { get; set; }
        public string DsName { get; set; }
        public string DsPhone { get; set; }
        public int IsStart { get; set; }
        public int IsRz { get; set; }
        public DateTime RzTime { get; set; }
        public int IsStoreManager { get; set; }
        public string Scode { get; set; }
        public int DsType { get; set; }

        //关联显示值
        public string accountALLHere { get; set; }
        public string StoreName { get; set; }
        public string storeRelationPerson { get; set; }

    }
}
