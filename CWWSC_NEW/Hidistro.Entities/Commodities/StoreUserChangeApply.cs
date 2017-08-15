using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class StoreUserChangeApply
    {
        /// <summary>
        ///店员提交更换店长申请记录表
        /// </summary>
        public Guid ID { get; set; }
        public int ApplyUserId { get; set; }
        public int StoreUserId { get; set; }
        /// <summary>
        /// 申请状态，默认值未0，0为待审核， 审核通过则为1，2为不通过。
        /// </summary>
        public int ApplyState { get; set; }
        public DateTime AuditingDate { get; set; }
        public string Reason { get; set; }
    }
}
