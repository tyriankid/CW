using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductReminDetailInfo
    {
        /// <summary>
        ///商品维护提醒记录实体
        /// </summary>
        public int MrDetailID { get; set; }
        public int MrID { get; set; }
        public string OrderId { get; set; }
        public DateTime RemindDate { get; set; }
        public int RemindNum { get; set; }
        public string RemindRemark { get; set; }
        
    }
}
