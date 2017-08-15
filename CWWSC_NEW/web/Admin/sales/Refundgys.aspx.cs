using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public partial class Refundgys : AdminPage
	{

		private void btnAuditAcceptRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.txtAdminRemark.Text.Trim(),
				HandleTime = DateTime.Now,
				AuditTime = DateTime.Now.ToString(),
				HandleStatus = RefundInfo.Handlestatus.HasTheAudit
			};
			if (!decimal.TryParse(this.hidAuditM.Value, out num))
			{
                this.ShowMsg("����Ľ���ʽ����ȷ", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("����Ϊ����?��", false);
				return;
			}
			if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
			{
                this.ShowMsg("���ʧ�ܣ������ԡ�", false);
				return;
			}
            this.ShowMsg("��˳ɹ�", true);
			this.LoadReturnApplyData();
		}

		private void btnAuditRefuseRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.txtAdminRemark.Text.Trim(),
				HandleTime = DateTime.Now,
				HandleStatus = RefundInfo.Handlestatus.AuditNotThrough,
				Operator = Globals.GetCurrentManagerUserId().ToString()
			};
			if (!decimal.TryParse(this.hidAuditM.Value, out num))
			{
                this.ShowMsg("����Ľ���ʽ����ȷ", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("����Ϊ����2��", false);
				return;
			}
			if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
			{
                this.ShowMsg("����ʧ�ܣ������ԡ�", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				string skuId = null;
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.ProductId != int.Parse(this.hidProductId.Value))
					{
						continue;
					}
					skuId = value.SkuId;
				}
				if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, (int)orderInfo.OrderStatus))
				{
                    this.ShowMsg("�����ɹ�", true);
					this.LoadReturnApplyData();
					return;
				}
			}
		}

		private void lkbtnDeleteCheck_Click(object obj, EventArgs eventArg)
		{
			int num;
			string item = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
                item = base.Request["CheckBoxGroup"];
			}
			if (item.Length <= 0)
			{
                this.ShowMsg("��ѡҪɾ�����˿����뵥", false);
				return;
			}
            string str = "�ɹ�ɾ����{0}���˿����뵥";
			char[] chrArray = new char[] { ',' };
            str = (!RefundHelper.DelRefundApply(item.Split(chrArray), out num) ? string.Concat(string.Format(str, num), ",����������벻��ɾ��") : string.Format(str, num));
			this.LoadReturnApplyData();
			this.ShowMsg(str, true);
		}

		private void btnSearchButton_Click(object obj, EventArgs eventArg)
		{
            this.ReloadPage(true);
		}

		private void dlstRefund_ItemDataBound(object obj, DataListItemEventArgs dataListItemEventArg)
		{
			if (dataListItemEventArg.Item.ItemType == ListItemType.Item || dataListItemEventArg.Item.ItemType == ListItemType.AlternatingItem)
			{
                HtmlAnchor htmlAnchor = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckRefund");
                Label label = (Label)dataListItemEventArg.Item.FindControl("lblHandleStatus");
                if (label.Text == "4")
				{
                    label.Text = "δ���";
					return;
				}
                if (label.Text == "5")
				{
                    label.Text = "�����";
					return;
				}
                if (label.Text == "6")
				{
                    label.Text = "δ�˿�";
					return;
				}
                if (label.Text == "2")
				{
                    label.Text = "���˿�";
					return;
				}
                if (label.Text == "8")
				{
                    label.Text = "�ܾ��˿�";
					return;
				}
                label.Text = "��˲�ͨ��";
			}
		}

		private void LoadReturnApplyData()
		{
			ReturnsApplyQuery returnsApplyQuery = this.GetQuery();
			DbQueryResult returnOrderAll = RefundHelper.GetReturnOrderAll(returnsApplyQuery);
			this.dlstRefund.DataSource = returnOrderAll.Data;
			this.dlstRefund.DataBind();
			this.pager.TotalRecords = returnOrderAll.TotalRecords;
			this.pager1.TotalRecords = returnOrderAll.TotalRecords;
			this.txtOrderId.Text = returnsApplyQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			this.ddlHandleStatus.SelectedValue = returnsApplyQuery.HandleStatus.Value.ToString();
		}

        private ReturnsApplyQuery GetQuery()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
                returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				returnsApplyQuery.HandleStatus = new int?(-1);
			}
			else
			{
				int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num))
				{
					returnsApplyQuery.HandleStatus = new int?(num);
				}
			}
            //2016-08-09��֤��ǰ��½�û�����
            bool isSupplier = false;//�Ƿ�Ӧ��
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.SupplierRoleId)
            {
                isSupplier = true;//��ǰ��½Ϊ��Ӧ���û�
            }
            returnsApplyQuery.OrderSource = 2;
            if (isSupplier)
                returnsApplyQuery.SupplierId = currentManager.ClientUserId;

			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
            returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}

        private void ReloadPage(bool flag)
		{
			NameValueCollection nameValueCollection = new NameValueCollection()
			{
				{ "OrderId", this.txtOrderId.Text },
				{ "PageSize", this.pager.PageSize.ToString() }
			};
			if (!flag)
			{
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
			{
                nameValueCollection.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
			}
			base.ReloadPage(nameValueCollection);
		}

        private void returnOrderByIds(string str)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
            orderInfo.CloseReason = "�ͻ�Ҫ���˻�(��)��";
			if (RefundHelper.CloseTransaction(orderInfo))
			{
				orderInfo.OnClosed();
				MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
				Messenger.OrderClosed(member, orderInfo, orderInfo.CloseReason);
			}
		}

		private void btnRefuseRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.hidAdminRemark.Value.Trim(),
				HandleTime = DateTime.Now,
				HandleStatus = RefundInfo.Handlestatus.RefuseRefunded,
				Operator = Globals.GetCurrentManagerUserId().ToString()
			};
			if (!decimal.TryParse(this.hidRefundM.Value, out num))
			{
                this.ShowMsg("����Ľ���ʽ����ȷ", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("����Ϊ����3��", false);
				return;
			}
			if (!RefundHelper.UpdateByReturnsId(refundInfo))
			{
                this.ShowMsg("����ʧ�ܣ������ԡ�", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				string skuId = null;
				int orderItemsStatus = 0;
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.ProductId != int.Parse(this.hidProductId.Value))
					{
						continue;
					}
					skuId = value.SkuId;
					orderItemsStatus = (int)value.OrderItemsStatus;
				}
				if (orderItemsStatus == 7)
				{
					this.hidStatus.Value = 3.ToString();
				}
                /*
                //����AH�����˻��ӿڣ�  �ӿڷ��سɹ������к�������
                StringBuilder strJson = new StringBuilder();
                strJson.Append("{");
                strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);
                strJson.AppendFormat("\"HandleTime\":\"{0}\",", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                strJson.Append("\"Details\":[");
                foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                {
                    strJson.Append("{");
                    strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//��Ʒ����
                    strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//����
                    strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemListPrice.ToString("0.00"));//��Ʒһ�ڼ۸�
                    strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//��Ʒ�����
                    strJson.Append("}");
                }
                strJson.Append("]");
                strJson.Append("}");
                CwAHAPI.CwapiLog("�������ݣ�" + strJson);
                AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                string strResult = ahservice.MPFTOJL_DHD_TH(strJson.ToString());
                CwAHAPI.CwapiLog("�������ݣ�" + strResult);
                if (string.IsNullOrEmpty(strResult))
                {
                    this.ShowMsg("����ʧ�ܣ�����AH�˻��ӿڷ������ݴ���", true);
                    return;
                }
                string message = "";
                string orderid = "";
                if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                {
                    this.ShowMsg(string.Format("����ʧ�ܣ�ԭ��{0}", message), true);
                    return;
                }
                else
                {
                    if (orderid != orderInfo.OrderId)
                    {
                        this.ShowMsg("����ʧ�ܣ�����AH�ӿڷ��صĶ�������������Ķ������벻��ͬ��", true);
                        return;
                    }
                }
                 * */
                //��ʼִ���˿����   (int.Parse(this.hidStatus.Value) == 6 ? 2 : 3)
                if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, (int)orderInfo.OrderStatus))
				{
                    //֪ͨ�û��˿���
                    MemberInfo memberInfo = MemberHelper.GetMember(orderInfo.UserId);
                    if (memberInfo != null)
                    {
                        //ͨ��
                        Messenger.OrderReturnOverSendManage(orderInfo, memberInfo, false);
                    }

                    this.ShowMsg("�����ɹ�", true);
					this.LoadReturnApplyData();
					return;
				}
			}
		}

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            decimal num = new decimal(0);
            int num1 = 0;
            RefundInfo refundInfo = new RefundInfo()
            {
                RefundId = int.Parse(this.hidReturnsId.Value),
                AdminRemark = this.hidAdminRemark.Value.Trim(),
                HandleTime = DateTime.Now,
                RefundTime = DateTime.Now.ToString(),
                HandleStatus = RefundInfo.Handlestatus.Refunded,
                Operator = Globals.GetCurrentManagerUserId().ToString()
            };
            if (!decimal.TryParse(this.hidRefundM.Value, out num))
            {
                this.ShowMsg("����Ľ���ʽ����ȷ", false);
                return;
            }
            refundInfo.RefundMoney = num;
            if (num < new decimal(0))
            {
                this.ShowMsg("����Ϊ����4��", false);
                return;
            }
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            //��Ӧ�̶��������ǿῪ��Ʒ��������ÿῪ�ӿڡ�
            if (orderInfo.OrderSource == 2)
            {
                //δ����ǰ��������ý����˿�ӿڣ�  ��������ˣ������˻����У�����Ŀǰ��֧�ַ�����Ķ����ڵ����˿
                if (string.IsNullOrEmpty(orderInfo.ShipOrderNumber))
                {
                    //���ý����˿�ӿ�
                    //2017-04-11
                    StringBuilder strJsonallhere = new StringBuilder();
                    strJsonallhere.Append("{");
                    strJsonallhere.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//������
                    //strJsonallhere.AppendFormat("\"FinishDate\":\"{0}\"", orderInfo.FinishDate);//�������ʱ��
                    //strJsonallhere.AppendFormat("\"FinishDate\":\"{0}\"", DateTime.Now);//�������ʱ��
                    strJsonallhere.AppendFormat("\"Date\":\"{0}\",", DateTime.Now);//�������ʱ��
                    strJsonallhere.AppendFormat("\"OrderState\":\"{0}\"", "Refund");//�������ʱ��
                    strJsonallhere.Append("}");
                    CwAHAPI.CwapiLog("**************�������ݣ�" + strJsonallhere.ToString());
                    AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                    try
                    {
                        string strResult = ahservice.MPFTOJL_DHD_QS(strJsonallhere.ToString());
                        CwAHAPI.CwapiLog("**************�������ݣ�" + strResult);
                        string orderid = string.Empty;
                        string message = string.Empty;
                        if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                        {
                            this.ShowMsg(string.Format("����AH�����˿�ӿ�ʧ�ܣ�ԭ��Ϊ��{0}", message), false);
                            return;
                        }
                        if (orderid != orderInfo.OrderId)
                        {
                            this.ShowMsg("����AH�����˿�ӿ�ʧ�ܣ�ԭ��Ϊ���ӿڷ��صĶ��������뷢��ʱ��һ�¡�", false);
                            return;
                        }
                    }
                    catch
                    {
                        this.ShowMsg("����ʧ�ܣ�ԭ�򣺷��ʽ����˿�ӿ�����Ӧ��", false);
                        return;
                    }
                }

                ///�Ὺ�ӿ����
                string strkukaicode = string.Empty;
                foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                {
                    if (!string.IsNullOrEmpty(iteminfo.KukaiCode))
                        strkukaicode = iteminfo.KukaiCode;
                }
                //����ĿῪ��Ʒ����ÿῪ�˿�ӿڣ�ֻ��δ����ǰ���˿��������ÿῪ�ӿڣ� ��Ϊ�Ὺ�ӿڲ�֧�ַ�������˿�
                if (!string.IsNullOrEmpty(strkukaicode) && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    //��ʼ����AH�����ӿ�
                    StringBuilder strJson = new StringBuilder();
                    strJson.Append("{");
                    strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);
                    strJson.AppendFormat("\"RefundDate\":\"{0}\"", refundInfo.RefundTime);
                    strJson.Append("}");
                    CwAHAPI.KkapiLog("�����˿���ÿῪ�ӿڷ������ݣ�" + strJson);

                    //ʵ�����ӿڶ���
                    KuKaiServiceReferenceReturn.StreetReturnReceiveWebInforClient kukaireturn = new KuKaiServiceReferenceReturn.StreetReturnReceiveWebInforClient();
                    //ʵ�������ݲ�������
                    KuKaiServiceReferenceReturn.sendReturnOrderToNC sendjsondata = new KuKaiServiceReferenceReturn.sendReturnOrderToNC();
                    sendjsondata.vo = strJson.ToString();
                    //ʵ�����ص��������� ���ҵ��ýӿڵõ��ص�����
                    KuKaiServiceReferenceReturn.sendReturnOrderToNCResponse resultModel = kukaireturn.sendReturnOrderToNC(sendjsondata);
                    if (resultModel != null)
                    {
                        string strResult = resultModel.returnArg;
                        CwAHAPI.KkapiLog("�����˿���ÿῪ�ӿڷ������ݣ�" + strResult);
                        string message = "";
                        string orderid = "";
                        if (!string.IsNullOrEmpty(strResult))
                        {
                            if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                            {
                                this.ShowMsg("�˿�ʧ�ܣ��Ὺ�ӿڷ���ʧ�ܡ�", false);
                                return;
                            }
                            else
                            {
                                if (orderid != orderInfo.OrderId)
                                {
                                    this.ShowMsg("�˿�ʧ�ܣ��Ὺ�ӿڷ���ʧ�ܡ�", false);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            /*
            //Ϊ��Ӧ����Ʒ����������ִ��AH�ӿ�
            if (orderInfo.OrderSource == 2)
            {
                #region ����֤AHͬ���ӿ�
                string id = TradeHelper.GetReturnsIdByOrderId(orderInfo.OrderId);
                StringBuilder strJson = new StringBuilder();
                strJson.Append("{");
                strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);
                //strJson.AppendFormat("\"HandleTime\":\"{0}\",", HandleTime);
                strJson.AppendFormat("\"HandleTime\":\"{0}\",", DateTime.Now);
                strJson.AppendFormat("\"NumBer\":\"{0}\",", id);
                strJson.Append("\"Details\":[");
                foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                {
                    strJson.Append("{");
                    strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//��Ʒ����
                    strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//����
                    strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemListPrice.ToString("0.00"));//��Ʒһ�ڼ۸�
                    strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemAdjustedPrice.ToString("0.00"));//��Ʒ�����
                    strJson.Append("}");
                }
                strJson.Append("]");
                strJson.Append("}");
                CwAHAPI.CwapiLog("�������ݣ�" + strJson);
                AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                string strResult = ahservice.MPFTOJL_DHD_TH(strJson.ToString());
                CwAHAPI.CwapiLog("�������ݣ�" + strResult);
                string message = "";
                string orderid = "";
                if (!string.IsNullOrEmpty(strResult))
                {
                    if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                    {
                        this.ShowMsg("����AH�˿�ӿ�ʧ�ܣ�ԭ��" + message, false);
                        return;
                    }
                    else
                    {
                        if (orderid != orderInfo.OrderId)
                        {
                            this.ShowMsg("����AH�˿�ӿ�ʧ�ܣ�ԭ�򣺷��͵Ķ�����������յĲ�һ�£�", false);
                            return;
                        }
                    }
                }
                else
                {
                    this.ShowMsg("����AH�˿�ӿ�ʧ�ܣ�ԭ�򣺽ӿڷ���ֵ����Ϊ�գ�", false);
                    return;
                }
                #endregion ����֤AHͬ���ӿ�
            }
            */
            //��ʼִ�����ݿ��������
            if (!RefundHelper.UpdateByReturnsId(refundInfo))
            {
                this.ShowMsg("�˿�ʧ�ܣ������ԡ�", false);
            }
            else
            {
                string skuId = null;
                string str = null;
                foreach (LineItemInfo value in orderInfo.LineItems.Values)
                {
                    if (value.ProductId != int.Parse(this.hidProductId.Value))
                    {
                        continue;
                    }
                    skuId = value.SkuId;
                    str = value.Quantity.ToString();
                }
                if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, 9))
                {
                    RefundHelper.UpdateRefundOrderStock(str, skuId);
                    foreach (LineItemInfo lineItemInfo in OrderHelper.GetOrderInfo(this.hidOrderId.Value).LineItems.Values)
                    {
                        if (lineItemInfo.OrderItemsStatus.ToString() != OrderStatus.Refunded.ToString())
                        {
                            continue;
                        }
                        num1++;
                    }
                    if (orderInfo.LineItems.Values.Count == num1)
                    {
                        this.returnOrderByIds(this.hidOrderId.Value);
                    }

                    //�޸Ļ���
                    MemberInfo memberInfo = MemberHelper.GetMember(orderInfo.UserId);
                    //�����˿������ϸ���¼
                    if (OrderHelper.ReducedPoint(orderInfo, memberInfo))
                    {
                        //�����û��ܻ���
                        memberInfo.Points = memberInfo.Points - orderInfo.Points;
                        MemberHelper.Update(memberInfo);
                        //PointDetailDao pdd = new PointDetailDao();
                        //pdd.Delete(orderInfo.UserId, orderInfo.OrderId);
                    }
                    //֪ͨ�û��˿���
                    if (memberInfo != null)
                    {
                        //ͨ��
                        Messenger.OrderReturnOverSendManage(orderInfo, memberInfo, true);
                    }

                    this.ShowMsg("�ɹ��˿�", true);
                    this.LoadReturnApplyData();
                    return;
                }
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dlstRefund.ItemDataBound += new DataListItemEventHandler(this.dlstRefund_ItemDataBound);
			this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
			this.btnAuditAcceptRefund.Click += new EventHandler(this.btnAuditAcceptRefund_Click);
			this.btnAuditRefuseRefund.Click += new EventHandler(this.btnAuditRefuseRefund_Click);
			if (!base.IsPostBack)
			{
				this.LoadReturnApplyData();
			}
		}
	}
}