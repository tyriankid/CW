using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class UserSearchLogsInfo
    {
        /// <summary>
        ///用户搜索实体
        /// </summary>
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string FunctionType { get; set; }
        public string SearchText { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
