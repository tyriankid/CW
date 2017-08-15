using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class KukaiOrderGoods
    {
        /// <summary>
        /// 微平台订单编码
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string DelivCode { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string DelivName { get; set; }

        /// <summary>
        /// 发货单号
        /// </summary>
        public string DelivOrderCode { get; set; }
    }
}
