using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class OrderVirtualInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int OVId { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 虚拟编码
        /// </summary>
        public int VirtualId { get; set; }

    }
}
