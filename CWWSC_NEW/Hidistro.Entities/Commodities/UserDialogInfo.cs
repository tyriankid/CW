using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class UserDialogInfo
    {
        /// <summary>
        ///用户聊天状态记录表
        /// </summary>
        public Guid DialogID { get; set; }
        public int FQUserId { get; set; }
        public int JSUserId { get; set; }
        public string DialogURL { get; set; }
        public string RoomNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int RoomType { get; set; }
    }
}
