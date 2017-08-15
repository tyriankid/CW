using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class AdminWxMsgInfo
    {
        /// <summary>
        /// 管理员微信消息推送记录
        /// </summary>
        public Guid ID { get; set; }
        public int FQManagerId { get; set; }
        public string FQUserName { get; set; }
        public int JSUserId { get; set; }
        public string MsgContent { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
