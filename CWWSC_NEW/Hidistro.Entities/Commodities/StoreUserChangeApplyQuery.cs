using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Commodities
{
    public class StoreUserChangeApplyQuery : Pagination
    {
        public string StoreName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 申请状态，默认值未0，0为待审核， 审核通过则为1，2为不通过。
        /// </summary>
        public string ApplyState { get; set; }
    }
}
