using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwProduct
    {
        /// <summary>
        /// 微平台商品编码
        /// </summary>
        public int MPF_SP {get; set;}

        /// <summary>
        /// 商品内码
        /// </summary>
        public int SPNM {get; set;}

        /// <summary>
        /// 商品名称
        /// </summary>
        public string SPNAME { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        public string WEIGHT { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string METERING { get; set; }
           
    }
}
