using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwReceipt
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// PDF发票下载地址
        /// </summary>
        public string TaxPath { get; set; }
    }
}
