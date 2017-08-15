namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnsApplyQuery : Pagination
    {
        public int? HandleStatus { get; set; }

        public string OrderId { get; set; }

        public string ReturnsId { get; set; }
        /// <summary>
        /// 订单来源,1为创维、2为其它
        /// </summary>
        public int OrderSource { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }
        /// <summary>
        /// 门店前端用户ID
        /// </summary>
        public int ReferralUserId { get; set; }


        /// <summary>
        /// 服务门店前端用户ID
        /// </summary>
        public int ServiceUserId { get; set; }

        /// <summary>
        /// 分公司ID
        /// </summary>
        public int FilialeId { get; set; }

    }
}

