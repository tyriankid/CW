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
                CwAHAPI.CwapiLog("**************��ʼ֧���ص�***************");
                this.OrderId = strValues[0];
                CwAHAPI.CwapiLog("**************�����ţ�" + this.OrderId);
                this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
                if (this.Order == null)
                {
                    CwAHAPI.CwapiLog("**************����֧���ص��Ķ����ŵõ�����ʵ��ʧ��***************");
                    ResponseWrite(true, "OK");
                }
                else
                {
                    CwAHAPI.CwapiLog("**************����֧���ص��Ķ����ŵõ�����ʵ��ɹ�***************");
                    this.Order.GatewayOrderId = strValues[1];
                    CwAHAPI.CwapiLog("**************������ˮ�ţ�" + this.Order.GatewayOrderId);
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
                WriteLog("**************��ʼ֧���ص�***************");
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.nonce_str);
                //WxPayAPI.Log.Debug(this.GetType().ToString(), payNotify.PayInfo.OutTradeNo);
				this.OrderId = payNotify.PayInfo.OutTradeNo;
                //this.SerialNum = payNotify.PayInfo.TransactionId;//��¼��ˮ��
                WriteLog("**************�����ţ�" + this.OrderId);
				this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
				if (this.Order == null)
				{
                    WriteLog("**************����֧���ص��Ķ����ŵõ�����ʵ��ʧ��***************");
					base.Response.Write("success");
				}
				else
				{
                    WriteLog("**************����֧���ص��Ķ����ŵõ�����ʵ��ɹ�***************");
					this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
                    WriteLog("**************������ˮ�ţ�" + this.Order.GatewayOrderId);
					this.UserPayOrder();
				}
			}*/
		}

        /// <summary>
        /// �ı䶩����Ϣ
        /// </summary>
		private void UserPayOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
                ResponseWrite(true, "OK");
			}
			else
			{
                CwAHAPI.CwapiLog("**************׼���޸Ķ���״̬***************");
                //��������ɹ��޸�����
                if (this.Order.CheckAction(OrderActions.BUYER_PAY))
                {
                    CwAHAPI.CwapiLog("**************����״̬Ϊ��" + this.Order.OrderStatus + "**********��ʼ�޸Ķ���Ϊ��֧��״̬");
                    //����ɹ��޸Ķ���״̬
                    if (this.Order.OrderSource == 5)
                    {
                        //֧�����Զ����
                        if (MemberProcessor.UserPayOrderVirtual(this.Order))
                        {
                            foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                            {
                                //����������
                                OrderVirtualInfoHelper.AddOrderVirtualInfo(this.Order.OrderId, iteminfo.ProductId, iteminfo.SkuId, iteminfo.Quantity);
                            }
                        }
                    }
                    else
                    {
                        #region  ��������Ʒ��Ķ�������
                        if (MemberProcessor.UserPayOrder(this.Order))
                        {
                            CwAHAPI.CwapiLog("**************����״̬�޸ĳɹ���׼��֪ͨ�ŵ�**********");

                            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                            if (this.Order.ReferralUserId > 0)
                            {
                                //֪ͨ�ŵ�
                                MemberInfo member = MemberProcessor.GetMember(this.Order.ReferralUserId);
                                if (member != null)
                                {
                                    Messenger.OrderPaymentToManager(member, this.OrderId, this.Order.GetTotal());
                                }
                            }
                            else
                            {
                                //֪ͨ����
                                MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                                if (openIdMember != null)
                                {
                                    Messenger.OrderPaymentToManager(openIdMember, this.OrderId, this.Order.GetTotal());
                                }
                            }
                            this.Order.OnPayment();
                            CwAHAPI.CwapiLog("**************�Ѿ�֪ͨ�ŵ�**********");
                            switch (Order.OrderSource)
                            {
                                case 1:
                                    #region ��ʼͬ���ۿ�ϵͳ
                                    CwAHAPI.CwapiLog("**************��ʼ���øۿڽӿ�**********");

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
                                        //��������깺�򣬲�����accountALLHereֵ����ȡ�����ļ��е�����Ĭ��AllHere�˺�ֵ
                                        if (!string.IsNullOrEmpty(masterSettings.DefaultALLHereCode))
                                        {
                                            store = masterSettings.DefaultALLHereCode;
                                            addJinliMember(store, "�ܵ�");
                                        }
                                    }
                                    string strmessage = string.Empty;
                                    if (!string.IsNullOrEmpty(store))
                                    {
                                        int iresult = CwAPI.SendOrderToGankKou(this.Order, store, out strmessage);
                                        CwAHAPI.CwapiLog("*******�����ۿڶ���ͬ���ӿ�********** �ŵ�ţ�" + store + "����ˮ�ţ�" + this.Order.GatewayOrderId + "���ӿڵ�����ʾ��" + strmessage);
                                        if (iresult == 0)
                                        {
                                            this.Order.submitgk = 1;
                                            //�޸�ͬ��״̬
                                            OrderHelper.UpdateSubmitgk(this.Order.OrderId, this.Order.submitgk);
                                            ResponseWrite(true, "OK");
                                        }
                                        else
                                        {
                                            //�ӿڵ��ô���Ҳ���潻����ˮ
                                            OrderHelper.UpdateSerialNum(this.Order.OrderId, this.Order.GatewayOrderId);
                                            ResponseWrite(false, "NO");
                                        }
                                    }
                                    else
                                    {
                                        CwAHAPI.CwapiLog("**************�µ��û������ŵ��Ӧ�Ļ����ŵ����ݲ����ڣ�����**********");
                                        ResponseWrite(false, "NO");
                                    }
                                    #endregion ����ͬ���ۿ�ϵͳ
                                    break;
                                case 2:
                                    CwAHAPI.CwapiLog("**************��ʼ���ý�������ͬ���ӿ�**********");
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
                                    CwAHAPI.CwapiLog("����ͬ������Ϊ��" + stordekeyid);

                                    #region ��ʼͬ������ϵͳ
                                    //��ʼ����AH�����ӿ�
                                    StringBuilder strJson = new StringBuilder();
                                    strJson.Append("{");
                                    strJson.AppendFormat("\"ReceivName\":\"{0}\",", Order.ShipTo);
                                    strJson.AppendFormat("\"ReceivTel\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", Order.RegionId.ToString().Substring(0, 2));
                                    strJson.AppendFormat("\"ReceivCity\":\"{0}\",", Order.RegionId.ToString().Substring(0, 4));
                                    strJson.AppendFormat("\"ReceivArea\":\"{0}\",", Order.RegionId.ToString());
                                    ///��ַʡ/��/����������
                                    string[] strname;
                                    if (!string.IsNullOrEmpty(Order.ShippingRegion))
                                    {
                                        strname = Order.ShippingRegion.Split('��');
                                        if (strname.Length == 3)
                                        {
                                            strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strname[0]);//����ʡ ��ԭ�� Ǭ����
                                            strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strname[1]);
                                            strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strname[2]);
                                        }
                                        else
                                        {
                                            strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//����ʡ ��ԭ�� Ǭ����
                                            strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", "");
                                            strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", "");
                                        }
                                    }
                                    strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                    //2017-04-26,�������ͨ��ͳһ�����ֶη�Ʊ����
                                    strJson.AppendFormat("\"TaxType\":\"{0}\",", Order.receiptId > 0 ? "1" : "2");//0Ϊ��ͨ��Ʊ��1Ϊ��ֵ˰��Ʊ��2Ϊ���ӷ�Ʊ
                                    strJson.AppendFormat("\"TaxName\":\"{0}\",", string.IsNullOrEmpty(Order.TelPhone) ? Order.ShipTo : Order.TelPhone);
                                    strJson.AppendFormat("\"TaxPhone\":\"{0}\",", Order.CellPhone);
                                    strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(Order.Address));
                                    strJson.AppendFormat("\"OrderNo\":\"{0}\",", Order.OrderId);
                                    strJson.AppendFormat("\"OrderTime\":\"{0}\",", Order.OrderDate);
                                    strJson.AppendFormat("\"FValue\":\"{0}\",", Order.GetAmount().ToString("0.00"));//��Ʒ�ܼ۸�
                                    strJson.AppendFormat("\"ReValue\":\"{0}\",", Order.GetTotal().ToString("0.00"));//����ʵ��֧���۸�
                                    strJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(Order.Remark));
                                    strJson.AppendFormat("\"SerialNum\":\"{0}\",", Order.GatewayOrderId);//serialNum ������ˮ��
                                    //����orderInfo.ReferralUserId ��ֵ �� aspnet_Distributors �в�ѯ ���� StoreId ���� Ȼ����CW_StoreInfo�в�ѯDZ��
                                    strJson.AppendFormat("\"BMBM\":\"{0}\",", stordekeyid);
                                    //����orderInfo.SupplierId��ѯ��Ӧ�̵�Codeֵ�ڴ���
                                    SupplierInfo supplierinfo = SupplierHelper.GetSupplier(Order.SupplierId);
                                    if (supplierinfo != null && !string.IsNullOrEmpty(supplierinfo.gysCode))
                                        strJson.AppendFormat("\"WLDW\":\"{0}\",", supplierinfo.gysCode);
                                    else
                                        strJson.AppendFormat("\"WLDW\":\"{0}\",", "");
                                    strJson.Append("\"Details\":[");
                                    string strkukaicode = string.Empty;//�洢�Ὺ����
                                    foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                                    {
                                        if (!string.IsNullOrEmpty(iteminfo.KukaiCode))
                                            strkukaicode += iteminfo.KukaiCode;//������ڿῪ���룬��洢�Ὺ�ڲ�����

                                        strJson.Append("{");
                                        strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//��Ʒ����
                                        strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//����
                                        strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//��Ʒ�ɽ��۸�
                                        strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//��Ʒ�����
                                        strJson.Append("}");
                                    }
                                    strJson.Append("]");
                                    strJson.Append("}");
                                    CwAHAPI.CwapiLog("֧���ɹ������ӿڣ��������ݣ�" + strJson);
                                    AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                                    string strResult = ahservice.MPFTOJL_DHD_CJ(strJson.ToString());
                                    CwAHAPI.CwapiLog("֧���ɹ������ӿڣ��������ݣ�" + strResult);
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
                                                //���½���ͬ��״̬
                                                Order.submitgk = 1;
                                                //�޸Ľ���ͬ��״̬
                                                OrderHelper.UpdateSubmitgk(Order.OrderId, Order.submitgk);

                                                #region  ��ʼ���ÿῪ�ӿ�
                                                CwAHAPI.KkapiLog("*************�����ӿڷ��سɹ�����ʼ���ÿῪ�ӿڣ�׼������************");
                                                if (!string.IsNullOrEmpty(strkukaicode))
                                                {
                                                    CwAHAPI.KkapiLog("*************�Ὺ��Ʒ�������ֵΪ��" + strkukaicode + "************");
                                                    //��ʼ����AH�����ӿ�
                                                    StringBuilder strkukaiJson = new StringBuilder();
                                                    strkukaiJson.Append("{");
                                                    strkukaiJson.AppendFormat("\"ReceivName\":\"{0}\",", Order.ShipTo);
                                                    strkukaiJson.AppendFormat("\"ReceivTel\":\"{0}\",", Order.CellPhone);
                                                    strkukaiJson.AppendFormat("\"ReceivPhone\":\"{0}\",", Order.CellPhone);

                                                    strkukaiJson.AppendFormat("\"ReceivProvice\":\"{0}\",", Order.RegionId.ToString().Substring(0, 2));
                                                    strkukaiJson.AppendFormat("\"ReceivCity\":\"{0}\",", Order.RegionId.ToString().Substring(0, 4));
                                                    strkukaiJson.AppendFormat("\"ReceivArea\":\"{0}\",", Order.RegionId.ToString());
                                                    ///��ַʡ/��/����������
                                                    string[] strnamekukai;
                                                    if (!string.IsNullOrEmpty(Order.ShippingRegion))
                                                    {
                                                        CwAHAPI.KkapiLog("���У�" + Order.ShippingRegion);
                                                        strnamekukai = Order.ShippingRegion.Split('��');
                                                        if (strnamekukai.Length == 3)
                                                        {
                                                            strkukaiJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strnamekukai[0]);//����ʡ ��ԭ�� Ǭ����
                                                            strkukaiJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strnamekukai[1]);
                                                            strkukaiJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strnamekukai[2]);

                                                            CwAHAPI.KkapiLog("ʡ��Ϊ��" + strnamekukai[0]);
                                                            CwAHAPI.KkapiLog("����Ϊ��" + strnamekukai[1]);
                                                            CwAHAPI.KkapiLog("��Ϊ��" + strnamekukai[2]);
                                                        }
                                                        else
                                                        {
                                                            strkukaiJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//����ʡ ��ԭ�� Ǭ����
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
                                                    strkukaiJson.AppendFormat("\"FValue\":\"{0}\",", Order.GetAmount().ToString("0.00"));//��Ʒ�ܼ۸�
                                                    strkukaiJson.AppendFormat("\"ReValue\":\"{0}\",", Order.GetTotal().ToString("0.00"));//����ʵ��֧���۸�
                                                    strkukaiJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(Order.Remark));
                                                    strkukaiJson.AppendFormat("\"SerialNum\":\"{0}\",", Order.GatewayOrderId);//serialNum ������ˮ��"45888899800000001"
                                                    strkukaiJson.Append("\"Details\":[");
                                                    foreach (LineItemInfo iteminfo in Order.LineItems.Values)
                                                    {
                                                        strkukaiJson.Append("{");
                                                        strkukaiJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.KukaiCode);//�Ὺ��Ʒ����
                                                        strkukaiJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//����
                                                        strkukaiJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//��Ʒʵ�ʹ��򵥼�
                                                        strkukaiJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//��Ʒ�����
                                                        strkukaiJson.Append("}");
                                                    }
                                                    strkukaiJson.Append("]");
                                                    strkukaiJson.Append("}");
                                                    CwAHAPI.KkapiLog("���ͿῪ�ӿ����ݣ�" + strkukaiJson);
                                                    try
                                                    {
                                                        //ʵ�����ӿڶ���
                                                        KuKaiServiceReference.CatchStreetOrderWebInfoClient kukaiwebinfo = new KuKaiServiceReference.CatchStreetOrderWebInfoClient();
                                                        //ʵ�������ݲ�������
                                                        KuKaiServiceReference.sendStreetOrderToNC sendModel = new KuKaiServiceReference.sendStreetOrderToNC();
                                                        //������ֵ����vo������
                                                        sendModel.vo = strkukaiJson.ToString();
                                                        //ʵ�����ص��������� ���ҵ��ýӿڵõ��ص�����
                                                        KuKaiServiceReference.sendStreetOrderToNCResponse resultModel = kukaiwebinfo.sendStreetOrderToNC(sendModel);
                                                        if (resultModel != null)
                                                        {
                                                            string strKukaiResult = resultModel.returnArg;
                                                            CwAHAPI.KkapiLog("���ؿῪ�ӿ����ݣ�" + strKukaiResult);
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
                                                                        CwAHAPI.KkapiLog("**********����ͬ���ɹ�**********");
                                                                        //���¿Ὺͬ��״̬
                                                                        Order.submitkk = 1;
                                                                        //�޸ĿῪͬ��״̬
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

                                                #endregion �������ÿῪ�ӿ�
                                            }
                                        }
                                    }
                                    #endregion ��ʼͬ������ϵͳ
                                    break;
                                case 3:
                                    //�ݲ�ͬ��������
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
        /// 2017-7-24  yk ��ά�̳� ǰ��΢��Ա������Ʒ�󣨸���� -  �ڽ�����Ա�������һ����¼��
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
