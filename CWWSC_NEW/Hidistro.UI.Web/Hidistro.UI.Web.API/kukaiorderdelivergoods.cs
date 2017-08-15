using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.CWAPI;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Plugins;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    
    /*酷开订单发货回调 */
    public class kukaiorderdelivergoods : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string strMessage = string.Empty;
            string strOrderNo = string.Empty;
            //KukaiOrderGoods ordergoods;
            try
            {
                ////接收传入的值
                //Stream postData = context.Request.InputStream;
                //StreamReader sRead = new StreamReader(postData);
                //string postContent = sRead.ReadToEnd();
                //if (context.Request["jsondata"] == null || string.IsNullOrEmpty(context.Request["jsondata"].ToString()))
                //{
                //    isSuccess = false;
                //    strMessage = "微平台发货接口传入的参数错误";
                //}
                //string postContent = context.Request["jsondata"].ToString();
                //CwAHAPI.KkapiLog("接收到酷开发送的发货数据：" + postContent);
                //通过json转换的实体集合
                //ordergoods = CwAPI.JsonToModel<KukaiOrderGoods>(postContent);
                //strOrderNo = ordergoods.OrderNo;
                strOrderNo = context.Request["OrderNo"];//微平台销售单号
                //string strDelivCode = context.Request["DelivCode"];//快递公司代码
                //string strDelivName = context.Request["DelivName"];//快递公司名称
                string strDelivOrderCode = context.Request["DelivOrderCode"];//快递单号
                CwAHAPI.KkapiLog("接收到酷开发送的订单号：" + strOrderNo);
                CwAHAPI.KkapiLog("接收到酷开发送的发货单号：" + strDelivOrderCode);
                if (!string.IsNullOrEmpty(strOrderNo) && !string.IsNullOrEmpty(strDelivOrderCode))
                {
                    CwAHAPI.KkapiLog("开始根据获取订单实体");
                    //得到订单实体对象
                    OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(strOrderNo);
                    if (orderInfo != null && !string.IsNullOrEmpty(orderInfo.OrderId) && orderInfo.OrderSource == 2)
                    {
                        //验证是否是酷开的订单，只有酷开的订单才允许执行。
                        string strkukaicode = string.Empty;
                        foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                        {
                            if (!string.IsNullOrEmpty(iteminfo.KukaiCode))
                                strkukaicode += iteminfo.KukaiCode;//如果存在酷开编码，则存储酷开内部编码
                        }
                        if (!string.IsNullOrEmpty(strkukaicode))
                        {
                            switch (orderInfo.OrderStatus)
                            {
                                case OrderStatus.BuyerAlreadyPaid:
                                    
                                    CwAHAPI.KkapiLog("开始调用AH接口");
                                    #region 状态修改成功，调用金力发货同步接口开始
                                    //先访问接口  接口成功执行后面代码
                                    StringBuilder strJson = new StringBuilder();
                                    strJson.Append("{");
                                    strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderInfo.ShipTo);//收货人名称
                                    strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderInfo.TelPhone);//收货人手机
                                    strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderInfo.CellPhone);//收货人电话
                                    strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 2));//收货人地址行政区域省的编码
                                    strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 4));//收货人地址行政区域市的编码
                                    strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderInfo.RegionId.ToString());//收货人地址行政区域区的编码
                                    strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//收货人详细地址
                                    strJson.AppendFormat("\"TaxName\":\"{0}\",", "创维直销部");//发票抬头
                                    strJson.AppendFormat("\"TaxPhone\":\"{0}\",", "027-81234567");//发票电话
                                    strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//发票邮寄地址
                                    strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//订单号
                                    strJson.AppendFormat("\"OrderNote\":\"{0}\",", orderInfo.Remark);//买家留言
                                    strJson.AppendFormat("\"DelivCode\":\"{0}\",", "zhaijisong");//快递公司代码
                                    strJson.AppendFormat("\"DelivName\":\"{0}\",", "宅急送");//快递公司名称
                                    //strJson.AppendFormat("\"DelivCode\":\"{0}\",", orderInfo.ExpressCompanyAbb);//快递公司代码
                                    //strJson.AppendFormat("\"DelivName\":\"{0}\",", orderInfo.ExpressCompanyName);//快递公司名称
                                    strJson.AppendFormat("\"DelivOrderCode\":\"{0}\"", strDelivOrderCode);//快递单号
                                    strJson.Append("}");
                                    CwAHAPI.KkapiLog("金力发货接口发送数据" + strJson.ToString());
                                    AllHereServiceReference.MPFTOJLClient aa = new AllHereServiceReference.MPFTOJLClient();
                                    string strResult = aa.MPFTOJL_DHD_FH(strJson.ToString());
                                    CwAHAPI.KkapiLog("金力发货接口返回数据：" + strResult);

                                    #endregion 金力发货接口调用结束
                                    
                                    string orderid = string.Empty;
                                    string strmessage = string.Empty;
                                    if (!string.IsNullOrEmpty(strResult) && CwAHAPI.ResolutionOrderAHResultString(strResult, out strmessage, out orderid) == 0)
                                    {
                                        if (orderInfo.OrderId.Equals(orderid))
                                        {
                                            //修改订单为发货状态
                                            if (setOrderSend(orderInfo, strDelivOrderCode))
                                            {
                                                isSuccess = true;//状态已经修改，则无需接口回调
                                                strMessage = "微平台接收酷开发货状态成功";
                                            }
                                            else
                                            {
                                                isSuccess = false;
                                                strMessage = "微平台设置订单发货状态失败";
                                            }
                                        }
                                        else
                                        {
                                            isSuccess = false;
                                            strMessage = "金力接口返回的订单编码与发送的不一致";
                                        }
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        strMessage = "微平台调用金力发货接口失败，原因：" + strmessage;
                                    }
                                    break;
                                case OrderStatus.SellerAlreadySent:
                                    isSuccess = false;
                                    strMessage = "微平台订单已经发货，不能再发货。";
                                    break;
                                default:
                                    isSuccess = false;
                                    strMessage = "微平台订单不是待发货状态";
                                    break;
                            }
                        }
                        else
                        {
                            isSuccess = false;
                            strMessage = "传入的订单号查询未发现酷开商品编码";
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        strMessage = "传入的订单号在微平台中不存在对应的订单";
                    }
                }
                else
                {
                    isSuccess = false;
                    strMessage = "微平台发货接口传入的参数错误";
                }
            }
            catch
            {
                //接口内部处理错误
                strMessage = "微平台发货接口传入的参数错误";
                isSuccess = false;
            }
            //构建返回内容
            StringBuilder sbBack = new StringBuilder();
            sbBack.Append("{");
            sbBack.AppendFormat("\"RSPDESC\":\"{0}\",", strMessage);
            sbBack.AppendFormat("\"STATE\":\"{0}\",", isSuccess ? "1" : "0");
            sbBack.AppendFormat("\"ORDERID\":\"{0}\"", strOrderNo);
            sbBack.Append("}");
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
                //ExpressCompanyInfo info4 = ExpressHelper.FindNode(expressCompanies[0]);
                //if (info4 != null)
                //{
                //    orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
                //    orderInfo.ExpressCompanyName = info4.Name;
                //}
                orderInfo.ExpressCompanyAbb = "zhaijisong";
                orderInfo.ExpressCompanyName = "宅急送";
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
