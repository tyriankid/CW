using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwProductReceive
    {
        /// <summary>
        /// 微平台商品编码
        /// </summary>
        public int MPF_SP { get; set; }

        /// <summary>
        /// 同步状态
        /// </summary>
        public string STATE { get; set; }

        /// <summary>
        /// 应答/错误描述
        /// </summary>
        public string RSPDESC { get; set; }

        /// <summary>
        /// 返回AH商品内码
        /// </summary>
        public int SPNM { get; set; }

    }
}
