using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwFiliale
    {
        /// <summary>
        /// 分公司编码
        /// </summary>
        public string filialecode { get; set; }

        /// <summary>
        /// 分公司名称
        /// </summary>
        public string filialename { get; set; }

        /// <summary>
        /// 分公司电话
        /// </summary>
        public string filialephone { get; set; }

        /// <summary>
        /// 分公司地址
        /// </summary>
        public string filialeaddress { get; set; }
    }
}
