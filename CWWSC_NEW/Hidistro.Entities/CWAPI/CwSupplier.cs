using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwSupplier
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string suppliercode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string suppliername { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        public string supplierphone { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public string supplieraddress { get; set; }
    }
}
