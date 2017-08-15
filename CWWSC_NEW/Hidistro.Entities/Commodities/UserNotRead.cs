using System;

namespace Hidistro.Entities.Commodities
{
    public class UserNotRead
    {
        /// <summary>
        ///用户聊天状态记录表
        /// </summary>
        public Guid ID { get; set; }
        public int FQUserId { get; set; }
        public int JSUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int NotReadMsgCount { get; set; }
    }
}
