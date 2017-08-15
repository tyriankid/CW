using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
namespace Hidistro.UI.Web.Pay
{
	public class wx_Pay : System.Web.UI.Page
	{
		protected OrderInfo Order;
		protected string OrderId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            WxPayAPI.ResultNotify resultNotify = new WxPayAPI.ResultNotify(this);
            string[] strValues = resultNotify.ProcessNotify();
            if (strValues.Length == 2 && !string.IsNullOrEmpty(strValues[0]) && !string.IsNullOrEmpty(strValues[1]))
            {
                CwAHAPI.CwapiLog("**************开始支付回调***************");
                this.OrderId = strValues[0];
                CwAHAPI.CwapiLog("**************订单号：" + this.OrderId);
                this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
                if (this.Order == null)
                {
                    CwAHAPI.CwapiLog("**************根据支付回调的订单号得到订单实体失败***************");
                    ResponseWrite(true, "OK");
                }
                else
                {
                    CwAHAPI.CwapiLog("**************根据支付回调的订单号得到订单实体成功***************");
                    this.Order.GatewayOrderId = strValues[1];
                    CwAHAPI.CwapiLog("**************交易流水号：" + this.Order.GatewayOrderId);
                    this.UserPayOrder();
                }
            }
            else
                ResponseWrite(false, "NO");
            /*
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			PayNotify payNotify = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).GetPayNotify(base.Request.InputStream);
			if (payNotify != null)
			{
                WriteLog("**************开始支付回调***************");
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.nonce_str);
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.PayInfo.OutTradeNo);
				this.OrderId = payNotify.PayInfo.OutTradeNo;
                //this.SerialNum = payNotify.PayInfo.TransactionId;//记录流水号
                WriteLog("**************订单号：" + this.OrderId);
				this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
				if (this.Order == null)
				{
                    WriteLog("**************根据支付回调的订单号得到订单实体失败***************");
					base.Response.Write("success");
				}
				else
				{
                    WriteLog("**************根据支付回调的订单号得到订单实体成功***************");
					this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
                    WriteLog("**************交易流水号：" + this.Order.GatewayOrderId);
					this.UserPayOrder();
				}
			}*/
		}

        /// <summary>
        /// 改变订单信息
        /// </summary>
		private void UserPayOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
                ResponseWrite(true, "OK");
			}
			else
			{
                CwAHAPI.CwapiLog("**************准备修改订单状态***************");
                //订单付款成功修改内容
                if (this.Order.CheckAction(OrderActions.BUYER_PAY))
                {
                    CwAHAPI.CwapiLog("**************订单状态为：" + this.Order.OrderStatus + "**********开始修改订单为已支付状态");
                    //付款成功修改订单状态
                    if (this.Order.OrderSource == 5)
                    {
                        //支付完自动完成
                        if (MemberProcessor.UserPayOrderVirtual(this.Order))
                        {
                            foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                            {
                                //分配虚拟码
                                OrderVirtualInfoHelper.AddOrderVirtualInfo(this.Order.OrderId, iteminfo.ProductId, iteminfo.SkuId, iteminfo.Quantity);
                            }
                        }
                    }
                    else
                    {
                        #region  除虚拟商品外的订单处理
                        if (MemberProcessor.UserPayOrder(this.Order))
                        {
                            CwAHAPI.CwapiLog("**************订单状态修改成功，准备通知门店**********");

                            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                            if (this.Order.ReferralUserId > 0)
                            {
                                //通知门店
                                MemberInfo member = MemberProcessor.GetMember(this.Order.ReferralUserId);
                                if (member != null)
                                {
                                    Messenger.OrderPaymentToManager(member, this.OrderId, this.Order.GetTotal());
                                }
                            }
                            else
                            {
                                //通知主店
                                MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                                if (openIdMember != null)
                                {
                                    Messenger.OrderPaymentToManager(openIdMember, this.OrderId, this.Order.GetTotal());
                                }
                            }
                            this.Order.OnPayment();
                            CwAHAPI.CwapiLog("**************已经通知门店**********");
                            switch (Order.OrderSource)
                            {
                                case 1:
                                    #region 开始同步港口系统
                                    CwAHAPI.CwapiLog("**************开始调用港口接口**********");

                                    string store = string.Empty;
                                    if (this.Order.ReferralUserId > 0)
                                    {
                                        StoreInfo storeinfo = StoreInfoHelper.GetStoreInfoByUserId(this.Order.ReferralUserId);

                                        if (storeinfo != null && !string.IsNullOrEmpty(storeinfo.accountALLHere))
                                        {
                                            store = storeinfo.accountALLHere;
                                            addJinliMember(storeinfo.accountALLHere, storeinfo.storeName, storeinfo.Id);
                                        }
                                    }
                                    else
                                    {
                                        //如果是主店购买，不存在accountALLHere值，则取配置文件中的主店默认AllHere账号值
                                        if (!string.IsNullOrEmpty(masterSettings.DefaultALLHereCode))
                                        {
                                            store = masterSettings.DefaultALLHereCode;
                                            addJinliMember(store, "总店");
                                        }
                                    }
                                    string strmessage = string.Empty;
                                    if (!string.IsNullOrEmpty(store))
                                    {
                                        int iresult = CwAPI.SendOrderToGankKou(this.Order, store, out strmessage);
                                        CwAHAPI.CwapiLog("*******结束港口订单同步接口********** 门店号：" + store + "—流水号：" + this.Order.GatewayOrderId + "—接口调用提示：" + strmessage);
                                        if (iresult == 0)
                                        {
                                            this.Order.submitgk = 1;
                                            //修改同步状态
                                            OrderHelper.UpdateSubmitgk(this.Order.OrderId, this.Order.submitgk);
                                            ResponseWrite(true, "OK");
                                        }
                                        else
                                        {
                                            //接口调用错误也保存交易流水
                                            OrderHelper.UpdateSerialNum(this.Order.OrderId, this.Order.GatewayOrderId);
                                            ResponseWrite(false, "NO");
                                        }
                                    }
                                    else
                                    {
                                        CwAHAPI.CwapiLog("**************下单用户所属门店对应的基础门店数据不存在！！！**********");
                                        ResponseWrite(false, "NO");
                                    }
                                    #endregion 结束同步港口系统
                                    break;
                                case 2:
                                    CwAHAPI.CwapiLog("**************开始调用金力订单同步接口**********");
                                    string stordekeyid = string.Empty;
                                    if (Order.ReferralUserId == 0)
                                    {
                                        if (!string.IsNullOrEmpty(masterSettings.DefaultALLHereCode))
                                        {
                                            stordekeyid = masterSettings.DefaultALLHereCode;
                                        }
                                    }
                                    else
                                    {
                                        stordekeyid = VShopHelper.GetOrderStoreKeyId(Order.ReferralUserId.ToString());
                                    }
                                    if (string.IsNullOrEmpty(stordekeyid))
                                    {
                                        ResponseWrite(false, "NO");
                                        return;
                                    }
                                    CwAHAPI.CwapiLog("店铺同步编码为：" + stordekeyid);

                                    #region 开始同步金力系统
                                    //开始调用AH订单接口
                                    StringBuilder strJson = new StringBuilder();
                                    strJson.Append("{");
                                    strJson.AppendFormat("\"ReceivName\":\"{0}\",", Order.ShipTo);
                                    strJson.AppendFormat("\"ReceivTel\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", Order.RegionId.ToString().Substring(0, 2));
                                    strJson.AppendFormat("\"ReceivCity\":\"{0}\",", Order.RegionId.ToString().Substring(0, 4));
                                    strJson.AppendFormat("\"ReceivArea\":\"{0}\",", Order.RegionId.ToString());
                                    ///地址省/市/区中文名称
                                    string[] strname;
                                    if (!string.IsNullOrEmpty(Order.ShippingRegion))
                                    {
                                        strname = Order.ShippingRegion.Split('，');
                                        if (strname.Length == 3)
                                        {
                                            strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strname[0]);//吉林省 松原市 乾安县
                                            strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strname[1]);
                                            strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strname[2]);
                                        }
                                        else
                                        {
                                            strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//吉林省 松原市 乾安县
                                            strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", "");
                                            strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", "");
                                        }
                                    }
                                    strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                    //2017-04-26,与金力沟通后统一增加字段发票类型
                                    strJson.AppendFormat("\"TaxType\":\"{0}\",", Order.receiptId > 0 ? "1" : "2");//0为普通发票，1为增值税发票，2为电子发票
                                    strJson.AppendFormat("\"TaxName\":\"{0}\",", string.IsNullOrEmpty(Order.TelPhone) ? Order.ShipTo : Order.TelPhone);
                                    strJson.AppendFormat("\"TaxPhone\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                    strJson.AppendFormat("\"OrderNo\":\"{0}\",", Order.OrderId);
                                    strJson.AppendFormat("\"OrderTime\":\"{0}\",", Order.OrderDate);
                                    strJson.AppendFormat("\"FValue\":\"{0}\",", Order.GetAmount().ToString("0.00"));//商品总价格
                                    strJson.AppendFormat("\"ReValue\":\"{0}\",", Order.GetTotal().ToString("0.00"));//订单实际支付价格
                                    strJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(Order.Remark));
                                    strJson.AppendFormat("\"SerialNum\":\"{0}\",", Order.GatewayOrderId);//serialNum 交易流水号
                                    //根据orderInfo.ReferralUserId 的值 在 aspnet_Distributors 中查询 —— StoreId —— 然后在CW_StoreInfo中查询DZ号
                                    strJson.AppendFormat("\"BMBM\":\"{0}\",", stordekeyid);
                                    //根据orderInfo.SupplierId查询供应商的Code值在传入
                                    SupplierInfo supplierinfo = SupplierHelper.GetSupplier(Order.SupplierId);
                                    if (supplierinfo != null && !string.IsNullOrEmpty(supplierinfo.gysCode))
                                        strJson.AppendFormat("\"WLDW\":\"{0}\",", supplierinfo.gysCode);
                                    else
                                        strJson.AppendFormat("\"WLDW\":\"{0}\",", "");
                                    strJson.Append("\"Details\":[");
                                    string strkukaicode = string.Empty;//存储酷开编码
                                    foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                                    {
                                        if (!string.IsNullOrEmpty(iteminfo.KukaiCode))
                                            strkukaicode += iteminfo.KukaiCode;//如果存在酷开编码，则存储酷开内部编码

                                        strJson.Append("{");
                                        strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//商品内码
                                        strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                                        strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品成交价格
                                        strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//商品结算价
                                        strJson.Append("}");
                                    }
                                    strJson.Append("]");
                                    strJson.Append("}");
                                    CwAHAPI.CwapiLog("支付成功订单接口：发送数据：" + strJson);
                                    AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                                    string strResult = ahservice.MPFTOJL_DHD_CJ(strJson.ToString());
                                    CwAHAPI.CwapiLog("支付成功订单接口：返回内容：" + strResult);
                                    string message = "";
                                    string orderid = "";
                                    if (!string.IsNullOrEmpty(strResult))
                                    {
                                        if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                                        {
                                            OrderHelper.UpdateSerialNum(this.Order.OrderId, Order.GatewayOrderId);
                                            ResponseWrite(false, "NO");
                                        }
                                        else
                                        {
                                            if (orderid == Order.OrderId)
                                            {
                                                //更新金力同步状态
                                                Order.submitgk = 1;
                                                //修改金力同步状态
                                                OrderHelper.UpdateSubmitgk(Order.OrderId, Order.submitgk);

                                                #region  开始调用酷开接口
                                                CwAHAPI.KkapiLog("*************金力接口返回成功，开始调用酷开接口，准备数据************");
                                                if (!string.IsNullOrEmpty(strkukaicode))
                                                {
                                                    CwAHAPI.KkapiLog("*************酷开商品编码存在值为：" + strkukaicode + "************");
                                                    //开始调用AH订单接口
                                                    StringBuilder strkukaiJson = new StringBuilder();
                                                    strkukaiJson.Append("{");
                                                    strkukaiJson.AppendFormat("\"ReceivName\":\"{0}\",", Order.ShipTo);
                                                    strkukaiJson.AppendFormat("\"ReceivTel\":\"{0}\",", Order.CellPhone);
                                                    strkukaiJson.AppendFormat("\"ReceivPhone\":\"{0}\",", Order.CellPhone);

                                                    strkukaiJson.AppendFormat("\"ReceivProvice\":\"{0}\",", Order.RegionId.ToString().Substring(0, 2));
                                                    strkukaiJson.AppendFormat("\"ReceivCity\":\"{0}\",", Order.RegionId.ToString().Substring(0, 4));
                                                    strkukaiJson.AppendFormat("\"ReceivArea\":\"{0}\",", Order.RegionId.ToString());
                                                    ///地址省/市/区中文名称
                                                    string[] strnamekukai;
                                                    if (!string.IsNullOrEmpty(Order.ShippingRegion))
                                                    {
                                                        CwAHAPI.KkapiLog("城市：" + Order.ShippingRegion);
                                                        strnamekukai = Order.ShippingRegion.Split('，');
                                                        if (strnamekukai.Length == 3)
                                                        {
                                                            strkukaiJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strnamekukai[0]);//吉林省 松原市 乾安县
                                                            strkukaiJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strnamekukai[1]);
                                                            strkukaiJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strnamekukai[2]);

                                                            CwAHAPI.KkapiLog("省份为：" + strnamekukai[0]);
                                                            CwAHAPI.KkapiLog("城市为：" + strnamekukai[1]);
                                                            CwAHAPI.KkapiLog("区为：" + strnamekukai[2]);
                                                        }
                                                        else
                                                        {
                                                            strkukaiJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//吉林省 松原市 乾安县
                                                            strkukaiJson.AppendFormat("\"ReceivCityName\":\"{0}\",", "");
                                                            strkukaiJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", "");
                                                        }
                                                    }
                                                    strkukaiJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                                    strkukaiJson.AppendFormat("\"TaxName\":\"{0}\",", string.IsNullOrEmpty(Order.TelPhone) ? Order.ShipTo : Order.TelPhone);
                                                    strkukaiJson.AppendFormat("\"TaxPhone\":\"{0}\",", Order.CellPhone);
                                                    strkukaiJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                                    strkukaiJson.AppendFormat("\"OrderNo\":\"{0}\",", Order.OrderId);
                                                    strkukaiJson.AppendFormat("\"OrderTime\":\"{0}\",", Order.OrderDate);
                                                    strkukaiJson.AppendFormat("\"FValue\":\"{0}\",", Order.GetAmount().ToString("0.00"));//商品总价格
                                                    strkukaiJson.AppendFormat("\"ReValue\":\"{0}\",", Order.GetTotal().ToString("0.00"));//订单实际支付价格
                                                    strkukaiJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(Order.Remark));
                                                    strkukaiJson.AppendFormat("\"SerialNum\":\"{0}\",", Order.GatewayOrderId);//serialNum 交易流水号"45888899800000001"
                                                    strkukaiJson.Append("\"Details\":[");
                                                    foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                                                    {
                                                        strkukaiJson.Append("{");
                                                        strkukaiJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.KukaiCode);//酷开商品内码
                                                        strkukaiJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                                                        strkukaiJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品实际购买单价
                                                        strkukaiJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//商品结算价
                                                        strkukaiJson.Append("}");
                                                    }
                                                    strkukaiJson.Append("]");
                                                    strkukaiJson.Append("}");
                                                    CwAHAPI.KkapiLog("发送酷开接口数据：" + strkukaiJson);
                                                    try
                                                    {
                                                        //实例化接口对象
                                                        KuKaiServiceReference.CatchStreetOrderWebInfoClient kukaiwebinfo = new KuKaiServiceReference.CatchStreetOrderWebInfoClient();
                                                        //实例化传递参数对象
                                                        KuKaiServiceReference.sendStreetOrderToNC sendModel = new KuKaiServiceReference.sendStreetOrderToNC();
                                                        //将传递值设置vo属性上
                                                        sendModel.vo = strkukaiJson.ToString();
                                                        //实例化回掉参数对象， 并且调用接口得到回调对象
                                                        KuKaiServiceReference.sendStreetOrderToNCResponse resultModel = kukaiwebinfo.sendStreetOrderToNC(sendModel);
                                                        if (resultModel != null)
                                                        {
                                                            string strKukaiResult = resultModel.returnArg;
                                                            CwAHAPI.KkapiLog("返回酷开接口数据：" + strKukaiResult);
                                                            string kukaimessage = "";
                                                            string kukaiorderid = "";
                                                            if (!string.IsNullOrEmpty(strKukaiResult))
                                                            {
                                                                if (CwAHAPI.ResolutionOrderAHResultString(strKukaiResult, out kukaimessage, out kukaiorderid) == -1)
                                                                {
                                                                    ResponseWrite(false, "NO");
                                                                }
                                                                else
                                                                {
                                                                    if (orderid == Order.OrderId)
                                                                    {
                                                                        CwAHAPI.KkapiLog("**********订单同步成功**********");
                                                                        //更新酷开同步状态
                                                                        Order.submitkk = 1;
                                                                        //修改酷开同步状态
                                                                        OrderHelper.UpdateSubmitkk(Order.OrderId, Order.submitkk);
                                                                        ResponseWrite(true, "OK");
                                                                    }
                                                                    else
                                                                        ResponseWrite(false, "NO");
                                                                }
                                                            }
                                                            else
                                                                ResponseWrite(false, "NO");
                                                        }
                                                        else
                                                            ResponseWrite(false, "NO");
                                                    }
                                                    catch
                                                    {
                                                        ResponseWrite(false, "NO");
                                                    }
                                                }

                                                #endregion 结束调用酷开接口
                                            }
                                        }
                                    }
                                    #endregion 开始同步金力系统
                                    break;
                                case 3:
                                    //暂不同步到金力
                                    break;
                            }
                        }
                        else
                            ResponseWrite(false, "NO");
                        #endregion
                    }
                }
                else
                    ResponseWrite(false, "NO");
			}
		}
        /// <summary>
        /// 2017-7-24  yk 创维商城 前端微会员购买商品后（付款后） -  在金力会员表中添加一条记录。
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="msg"></param>
        private void addJinliMember(string storeCode,string storename,int StoreId=0)
        {
            MemberInfo info = MemberProcessor.GetCurrentMember();
            if (info != null)
            {   
                CWMenbersInfo member = new CWMenbersInfo
                {
                    UserName = info.UserName,
                    accountALLHere = storeCode,
                    StoreName = storename,
                    CellPhone = info.CellPhone,
                    Price = Order.TotalPrice.ToString("0.00"),
                    Address = Globals.HtmlEncode(Order.Address),
                    UserId = info.UserId,
                    StoreId = StoreId,
                };
                foreach (LineItemInfo infos in Order.LineItems.Values)
                {
                    member.ProductCode = infos.ProductCode;
                    member.ProductModel = infos.SKUContent;
                    member.BuyNum = infos.Quantity.ToString(); 
                }
                CWMembersHelper.Create(member);
            }
        }
        private void ResponseWrite(bool isSuccess, string msg)
        {
            WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
            res.SetValue("return_code", isSuccess ? "SUCCESS" : "FAIL");
            res.SetValue("return_msg", msg);
            base.Response.Write(res.ToXml());
            base.Response.End();
        }

        public void WriteLog(string str)
        {
            string path = HttpContext.Current.Server.MapPath("/CW_PAY.txt");
            System.IO.StreamWriter writer = System.IO.File.AppendText(path);
            writer.WriteLine(str);
            writer.WriteLine(System.DateTime.Now);
            writer.Flush();
            writer.Close();
        }
	}
}
