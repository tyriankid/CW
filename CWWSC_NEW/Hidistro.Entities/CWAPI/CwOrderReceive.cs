using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwOrderReceive
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string ORDERID { get; set; }

        /// <summary>
        /// 1：同步成功
        /// 0：同步失败
        /// </summary>
        public int STATE { get; set; }

        /// <summary>
        /// 应答 或错误描述
        /// </summary>
        public string RSPDESC { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderState { get; set; }
    }
}
