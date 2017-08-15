using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{

    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CW_O2OMemberQuery : Pagination
    {
        public string name { get; set; }

        public string cell { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        /// 是否粘性会员查询
        /// </summary>
        public bool isNxMember { get; set; }

        /// <summary>
        /// 门店DZ号
        /// </summary>
        public string storeCode { get; set; }

        /// <summary>
        /// 门店Code集合
        /// </summary>
        public string storeCodes { get; set; }
    }

}
