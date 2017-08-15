using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.CWAPI
{
    public class CwStore
    {
        /// <summary>
        /// 门店名称
        /// </summary>
        public string storename { get; set; }

        /// <summary>
        /// 门店店长姓名
        /// </summary>
        public string storemanager { get; set; }

        /// <summary>
        /// 门店店长电话
        /// </summary>
        public string storemanagercell { get; set; }

        /// <summary>
        /// DZ号
        /// </summary>
        public string allherenumber { get; set; }

        /// <summary>
        /// 所属分公司编码
        /// </summary>
        public string filialecode { get; set; }

        /// <summary>
        /// AH主键ID
        /// </summary>
        public string storekeyid { get; set; }

    }
}
