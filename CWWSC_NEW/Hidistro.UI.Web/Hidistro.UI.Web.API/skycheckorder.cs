using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    
    /*订单状态回调 */
    public class skycheckorder : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string strStatus = context.Request["status"];//2配仓，3发货，5，确认收货
            string strSendnum = context.Request["sendnum"];//数量
            string strOrderid = context.Request["orderid"];//订单Id， 以接口传入的orderid值为主，不是订单编码
            string strGoodssn = context.Request["goodssn"];//商品内码
            string strShipno = context.Request["shipno"];//运单号

            bool isSuccess = false;
            #region 内部处理内容
            //得到订单实体对象
            OrderInfo Order = ShoppingProcessor.GetOrderInfoById(strOrderid);
            switch (strStatus)
            { 
                case "2":
                    isSuccess = true;
                    break;
                case "3":
                    //发货
                    //if (Order != null && !string.IsNullOrEmpty(strShipno))
                    //{
                    //    //将订单设置为已发货状态
                    //    if (setOrderSend(Order, strShipno))
                    //    {
                    //        isSuccess = true;//执行成功
                    //    }
                    //}
                    //if (Order.OrderStatus == OrderStatus.SellerAlreadySent)
                    //{
                    //    isSuccess = true;//状态已经修改，则无需接口回调
                    //}
                    if (Order != null && !string.IsNullOrEmpty(strShipno))
                    {
                        //将订单设置为已发货状态
                        if (setOrderSend(Order, strShipno))
                        {
                        }
                    }
                    isSuccess = true;//状态已经修改，则无需接口回调
                    break;
                case "5":
                    //try
                    //{
                    //    //确认收货
                    //    if (MemberProcessor.ConfirmOrderFinish(Order))
                    //    {
                    //        //添加返佣记录
                    //        DistributorsBrower.UpdateCalculationCommission(Order);
                    //        isSuccess = true;//执行成功
                    //    }
                    //    if (Order.OrderStatus == OrderStatus.Finished)
                    //    {
                    //        isSuccess = true;//状态已经修改，则无需接口回调
                    //    }
                    //}
                    //catch 
                    //{
                    //    isSuccess = true;//状态已经修改，则无需接口回调
                    //}
                    try
                    {
                        //确认收货
                        if (MemberProcessor.ConfirmOrderFinish(Order))
                        {
                            //添加返佣记录
                            DistributorsBrower.UpdateCalculationCommission(Order);
                        }
                    }
                    catch
                    {
                    }
                    isSuccess = true;//状态已经修改，则无需接口回调
                    break;
            }
            #endregion 内部处理内容


            //构建返回内容
            StringBuilder sbBack = new StringBuilder();
            sbBack.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbBack.Append("<Result>");
                sbBack.AppendFormat("<State>{0}</State>", isSuccess ? "0" : "99");
                sbBack.AppendFormat("<Message>{0}</Message>", isSuccess ? "提交成功" : "提交失败");
            sbBack.Append("</Result>");
            //回掉请求方
            context.Response.Write(sbBack.ToString());
        }

        private bool setOrderSend(OrderInfo orderInfo, string strShipNumber)
        {
            try
            {
                //支付方式
                IList<ShippingModeInfo> infolist = SalesHelper.GetShippingModes();
                if (infolist == null || infolist.Count == 0) { return false; }

                IList<string> expressCompanies = ExpressHelper.GetAllExpressName();
                if (expressCompanies == null || expressCompanies.Count == 0) { return false; }

                //得到配送方式，默认取第一条
                ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(infolist[0].ModeId, true);
                orderInfo.RealShippingModeId = infolist[0].ModeId;
                orderInfo.RealModeName = shippingMode.Name;

                //得到物流公司，默认取第一条
                ExpressCompanyInfo info4 = ExpressHelper.FindNode(expressCompanies[0]);
                if (info4 != null)
                {
                    orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
                    orderInfo.ExpressCompanyName = info4.Name;
                }
                orderInfo.ShipOrderNumber = strShipNumber;
                if (OrderHelper.SendGoods(orderInfo, false))
                {
                    //设置当前操作人员为admin
                    ManagerInfo currentManager = ManagerHelper.GetManager("admin");
                    //发货记录
                    SendNoteInfo info5 = new SendNoteInfo();
                    info5.NoteId = Globals.GetGenerateId();
                    info5.OrderId = orderInfo.OrderId;
                    info5.Operator = currentManager.UserName;
                    info5.Remark = "后台" + info5.Operator + "发货成功";
                    OrderHelper.SaveSendNote(info5);
                    //发送短信给购买者
                    MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
                    Messenger.OrderShipping(orderInfo, member);

                    orderInfo.OnDeliver();
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}
