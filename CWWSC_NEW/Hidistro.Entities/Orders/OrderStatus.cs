namespace Hidistro.Entities.Orders
{
    using System;

    public enum OrderStatus
    {
        All = 0,
        WaitBuyerPay = 1,//等待买家付款
        BuyerAlreadyPaid = 2,//已付款,等待发货
        SellerAlreadySent = 3,//已发货
        Closed = 4,//已关闭
        Finished = 5,//订单已完成
        ApplyForRefund = 6,//申请退款
        ApplyForReturns = 7,//申请退货
        ApplyForReplacement = 8,//申请换货
        Refunded = 9,//已退款
        Returned = 10,//已退货
        Today = 11,//
        Replaced = 11,//
        ConfirmTakeGoods = 88,//用户确认收货
        History = 99,//历史订单

        MerchantsAgreedFoReturns = 12,
        DeliveryingForReturns = 13,
        GetGoodsForReturns = 14,
        MerchantsAgreedForReplace = 15,
        UserDeliveryForReplace = 16,
        MerchantsDeliveryForReplace = 17,
        RefundRefused = 18,
        ReturnRefused = 19,
        ReplaceRefused = 20
    }
}

