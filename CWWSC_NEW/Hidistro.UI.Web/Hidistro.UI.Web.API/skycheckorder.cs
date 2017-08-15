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
    
    /*����״̬�ص� */
    public class skycheckorder : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string strStatus = context.Request["status"];//2��֣�3������5��ȷ���ջ�
            string strSendnum = context.Request["sendnum"];//����
            string strOrderid = context.Request["orderid"];//����Id�� �Խӿڴ����orderidֵΪ�������Ƕ�������
            string strGoodssn = context.Request["goodssn"];//��Ʒ����
            string strShipno = context.Request["shipno"];//�˵���

            bool isSuccess = false;
            #region �ڲ���������
            //�õ�����ʵ�����
            OrderInfo Order = ShoppingProcessor.GetOrderInfoById(strOrderid);
            switch (strStatus)
            { 
                case "2":
                    isSuccess = true;
                    break;
                case "3":
                    //����
                    //if (Order != null && !string.IsNullOrEmpty(strShipno))
                    //{
                    //    //����������Ϊ�ѷ���״̬
                    //    if (setOrderSend(Order, strShipno))
                    //    {
                    //        isSuccess = true;//ִ�гɹ�
                    //    }
                    //}
                    //if (Order.OrderStatus == OrderStatus.SellerAlreadySent)
                    //{
                    //    isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                    //}
                    if (Order != null && !string.IsNullOrEmpty(strShipno))
                    {
                        //����������Ϊ�ѷ���״̬
                        if (setOrderSend(Order, strShipno))
                        {
                        }
                    }
                    isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                    break;
                case "5":
                    //try
                    //{
                    //    //ȷ���ջ�
                    //    if (MemberProcessor.ConfirmOrderFinish(Order))
                    //    {
                    //        //��ӷ�Ӷ��¼
                    //        DistributorsBrower.UpdateCalculationCommission(Order);
                    //        isSuccess = true;//ִ�гɹ�
                    //    }
                    //    if (Order.OrderStatus == OrderStatus.Finished)
                    //    {
                    //        isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                    //    }
                    //}
                    //catch 
                    //{
                    //    isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                    //}
                    try
                    {
                        //ȷ���ջ�
                        if (MemberProcessor.ConfirmOrderFinish(Order))
                        {
                            //��ӷ�Ӷ��¼
                            DistributorsBrower.UpdateCalculationCommission(Order);
                        }
                    }
                    catch
                    {
                    }
                    isSuccess = true;//״̬�Ѿ��޸ģ�������ӿڻص�
                    break;
            }
            #endregion �ڲ���������


            //������������
            StringBuilder sbBack = new StringBuilder();
            sbBack.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbBack.Append("<Result>");
                sbBack.AppendFormat("<State>{0}</State>", isSuccess ? "0" : "99");
                sbBack.AppendFormat("<Message>{0}</Message>", isSuccess ? "�ύ�ɹ�" : "�ύʧ��");
            sbBack.Append("</Result>");
            //�ص�����
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
                ExpressCompanyInfo info4 = ExpressHelper.FindNode(expressCompanies[0]);
                if (info4 != null)
                {
                    orderInfo.ExpressCompanyAbb = info4.Kuaidi100Code;
                    orderInfo.ExpressCompanyName = info4.Name;
                }
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
