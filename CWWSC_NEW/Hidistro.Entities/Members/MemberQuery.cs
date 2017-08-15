namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberQuery : Pagination
    {
        public string CharSymbol { get; set; }

        public string ClientType { get; set; }

        public DateTime? EndTime { get; set; }

        public int? GradeId { get; set; }

        public bool? HasVipCard { get; set; }

        public bool? IsApproved { get; set; }

        public decimal? OrderMoney { get; set; }

        public int? OrderNumber { get; set; }

        public string Realname { get; set; }

        public DateTime? StartTime { get; set; }

        public string Username { get; set; }

        public string StoreName { get; set; }
        /// <summary>
        /// 用户类型，0为全部，1为掌柜，2为普通用户
        /// </summary>
        public string UserTypeId { get; set; }

        public string DistributorsUserIds { get; set; }

        public string CellPhone { get; set; }
        /// <summary>
        /// 是否活跃
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool? IsActivated { get; set; }
        /// <summary>
        /// 是否购机
        /// </summary>
        public bool? IsBuy { get; set; }
        /// <summary>
        /// 是否粘性会员
        /// </summary>
        public bool? IsViscidity { get; set; }
    }
}

