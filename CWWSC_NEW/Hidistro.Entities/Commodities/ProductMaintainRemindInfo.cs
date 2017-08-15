using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class ProductMaintainRemindInfo
    {
        /// <summary>
        ///商品维护提醒等级实体
        /// </summary>
        public int MrID { get; set; }
        public int ProductId { get; set; }
        public int RemindCycle { get; set; }
        public int RemindNum { get; set; }
        public string RemindRemark { get; set; }
    }
}
