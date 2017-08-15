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
    
    /*�Ὺ���������ص� */
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
                ////���մ����ֵ
                //Stream postData = context.Request.InputStream;
                //StreamReader sRead = new StreamReader(postData);
                //string postContent = sRead.ReadToEnd();
                //if (context.Request["jsondata"] == null || string.IsNullOrEmpty(context.Request["jsondata"].ToString()))
                //{
                //    isSuccess = false;
                //    strMessage = "΢ƽ̨�����ӿڴ���Ĳ�������";
                //}
                //string postContent = context.Request["jsondata"].ToString();
                //CwAHAPI.KkapiLog("���յ��Ὺ���͵ķ������ݣ�" + postContent);
                //ͨ��jsonת����ʵ�弯��
                //ordergoods = CwAPI.JsonToModel<KukaiOrderGoods>(postContent);
                //strOrderNo = ordergoods.OrderNo;
                strOrderNo = context.Request["OrderNo"];//΢ƽ̨���۵���
                //string strDelivCode = context.Request["DelivCode"];//��ݹ�˾����
                //string strDelivName = context.Request["DelivName"];//��ݹ�˾����
                string strDelivOrderCode = context.Request["DelivOrderCode"];//��ݵ���
                CwAHAPI.KkapiLog("���յ��Ὺ���͵Ķ����ţ�" + strOrderNo);
                CwAHAPI.KkapiLog("���յ��Ὺ���͵ķ������ţ�" + strDelivOrderCode);
                if (!string.IsNullOrEmpty(strOrderNo) && !string.IsNullOrEmpty(strDelivOrderCode))
                {
                    CwAHAPI.KkapiLog("��ʼ���ݻ�ȡ����ʵ��");
                    //�õ�����ʵ�����
                    OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(strOrderNo);
                    if (orderInfo != null && !string.IsNullOrEmpty(orderInfo.OrderId) && orderInfo.OrderSource == 2)
                    {
                        //��֤�Ƿ��ǿῪ�Ķ�����ֻ�пῪ�Ķ���������ִ�С�
                        string strkukaicode = string.Empty;
                        foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                        {
                            if (!string.IsNullOrEmpty(iteminfo.KukaiCode))
                                strkukaicode += iteminfo.KukaiCode;//������ڿῪ���룬��洢�Ὺ�ڲ�����
                        }
                        if (!string.IsNullOrEmpty(strkukaicode))
                        {
                            switch (orderInfo.OrderStatus)
                            {
                                case OrderStatus.BuyerAlreadyPaid:
                                    
                                    CwAHAPI.KkapiLog("��ʼ����AH�ӿ�");
                                    #region ״̬�޸ĳɹ������ý�������ͬ���ӿڿ�ʼ
                                    //�ȷ��ʽӿ�  �ӿڳɹ�ִ�к������
                                    StringBuilder strJson = new StringBuilder();
                                    strJson.Append("{");
                                    strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderInfo.ShipTo);//�ջ�������
                                    strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderInfo.TelPhone);//�ջ����ֻ�
                                    strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderInfo.CellPhone);//�ջ��˵绰
                                    strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 2));//�ջ��˵�ַ��������ʡ�ı���
                                    strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 4));//�ջ��˵�ַ���������еı���
                                    strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderInfo.RegionId.ToString());//�ջ��˵�ַ�����������ı���
                                    strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//�ջ�����ϸ��ַ
                                    strJson.AppendFormat("\"TaxName\":\"{0}\",", "��άֱ����");//��Ʊ̧ͷ
                                    strJson.AppendFormat("\"TaxPhone\":\"{0}\",", "027-81234567");//��Ʊ�绰
                                    strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));//��Ʊ�ʼĵ�ַ
                                    strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//������
                                    strJson.AppendFormat("\"OrderNote\":\"{0}\",", orderInfo.Remark);//�������
                                    strJson.AppendFormat("\"DelivCode\":\"{0}\",", "zhaijisong");//��ݹ�˾����
                                    strJson.AppendFormat("\"DelivName\":\"{0}\",", "լ����");//��ݹ�˾����
                                    //strJson.AppendFormat("\"DelivCode\":\"{0}\",", orderInfo.ExpressCompanyAbb);//��ݹ�˾����
                                    //strJson.AppendFormat("\"DelivName\":\"{0}\",", orderInfo.ExpressCompanyName);//��ݹ�˾����
                                    strJson.AppendFormat("\"DelivOrderCode\":\"{0}\"", strDelivOrderCode);//��ݵ���
                                    strJson.Append("}");
                                    CwAHAPI.KkapiLog("���������ӿڷ�������" + strJson.ToString());
                                    AllHereServiceReference.MPFTOJLClient aa = new AllHereServiceReference.MPFTOJLClient();
                                    string strResult = aa.MPFTOJL_DHD_FH(strJson.ToString());
                                    CwAHAPI.KkapiLog("���������ӿڷ������ݣ�" + strResult);

                                    #endregion ���������ӿڵ��ý���
                                    
                                    string orderid = string.Empty;
                                    string strmessage = string.Empty;
                                    if (!string.IsNullOrEmpty(strResult) && CwAHAPI.ResolutionOrderAHResultString(strResult, out strmessage, out orderid) == 0)
                                    {
                                        if (orderInfo.OrderId.Equals(orderid))
                                        {
                                            //�޸Ķ���Ϊ����״̬
                                            if (setOrderSend(orderInfo, strDelivOrderCode))
                                            {
                                                isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                                                strMessage = "΢ƽ̨���տῪ����״̬�ɹ�";
                                            }
                                            else
                                            {
                                                isSuccess = false;
                                                strMessage = "΢ƽ̨���ö�������״̬ʧ��";
                                            }
                                        }
                                        else
                                        {
                                            isSuccess = false;
                                            strMessage = "�����ӿڷ��صĶ��������뷢�͵Ĳ�һ��";
                                        }
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        strMessage = "΢ƽ̨���ý��������ӿ�ʧ�ܣ�ԭ��" + strmessage;
                                    }
                                    break;
                                case OrderStatus.SellerAlreadySent:
                                    isSuccess = false;
                                    strMessage = "΢ƽ̨�����Ѿ������������ٷ�����";
                                    break;
                                default:
                                    isSuccess = false;
                                    strMessage = "΢ƽ̨�������Ǵ�����״̬";
                                    break;
                            }
                        }
                        else
                        {
                            isSuccess = false;
                            strMessage = "����Ķ����Ų�ѯδ���ֿῪ��Ʒ����";
                        }
                    }
                    else
                    {
                        isSuccess = false;
                        strMessage = "����Ķ�������΢ƽ̨�в����ڶ�Ӧ�Ķ���";
                    }
                }
                else
                {
                    isSuccess = false;
                    strMessage = "΢ƽ̨�����ӿڴ���Ĳ�������";
                }
            }
            catch
            {
                //�ӿ��ڲ��������
                strMessage = "΢ƽ̨�����ӿڴ���Ĳ�������";
                isSuccess = false;
            }
            //������������
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
                //֧����ʽ
                IList<ShippingModeInfo> infolist = SalesHelper.GetShippingModes();
                if (infolist == null || infolist.Count == 0) { return false; }

                IList<string> expressCompanies = ExpressHelper.GetAllExpressName();
                if (expressCompanies == null || expressCompanies.Count == 0) { return false; }

                //�õ����ͷ�ʽ��Ĭ��ȡ��һ��
                ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(infolist[0].ModeId, true);
                orderInfo.RealShippingModeId = infolist[0].ModeId;
                orderInfo.RealModeName = shippingMode.Name;

                //�õ�������˾��Ĭ��ȡ��һ��
                //ExpressCompanyInfo info4 = ExpressHelper.FindNode(expressCompanies[0]);
                //if (info4 != null)
                //{
                //    orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
                //    orderInfo.ExpressCompanyName = info4.Name;
                //}
                orderInfo.ExpressCompanyAbb = "zhaijisong";
                orderInfo.ExpressCompanyName = "լ����";
                orderInfo.ShipOrderNumber = strShipNumber;
                if (OrderHelper.SendGoods(orderInfo, false))
                {
                    //���õ�ǰ������ԱΪadmin
                    ManagerInfo currentManager = ManagerHelper.GetManager("admin");
                    //������¼
                    SendNoteInfo info5 = new SendNoteInfo();
                    info5.NoteId = Globals.GetGenerateId();
                    info5.OrderId = orderInfo.OrderId;
                    info5.Operator = currentManager.UserName;
                    info5.Remark = "��̨" + info5.Operator + "�����ɹ�";
                    OrderHelper.SaveSendNote(info5);
                    //���Ͷ��Ÿ�������
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
