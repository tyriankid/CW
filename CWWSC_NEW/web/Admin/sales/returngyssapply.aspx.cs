using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Entities.Store;
using Hidistro.ControlPanel.Store;
using Hidistro.SaleSystem.Vshop;
using System.Text;
using Hishop.Weixin.MP.Util;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class returngyssapply : AdminPage
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
                HandleStatus = RefundInfo.Handlestatus.NoRefund
            };
            if (!decimal.TryParse(this.hidAuditM.Value, out num))
            {
                this.ShowMsg("输入的金额格式不正确", false);
                return;
            }
            refundInfo.RefundMoney = num;
            if (num < new decimal(0))
            {
                this.ShowMsg("不能为负数！", false);
                return;
            }
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);

            //为供应商商品订单才允许执行AH接口
            if (orderInfo.OrderSource == 2)
            {
                #region 先验证AH同步接口
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
                    strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//商品内码
                    strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                    strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品成交价格
                    strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//商品结算价
                    strJson.Append("}");
                }
                strJson.Append("]");
                strJson.Append("}");
                CwAHAPI.CwapiLog("**************发送数据：" + strJson.ToString());
                AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                string strResult = ahservice.MPFTOJL_DHD_TH(strJson.ToString());
                CwAHAPI.CwapiLog("**************返回内容：" + strResult);
                string message = "";
                string orderid = "";
                if (!string.IsNullOrEmpty(strResult))
                {
                    if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                    {
                        this.ShowMsg("调用AH退款接口失败，原因：" + message, false);
                        return;
                    }
                    else
                    {
                        if (orderid != orderInfo.OrderId)
                        {
                            this.ShowMsg("调用AH退款接口失败，原因：发送的订单编码与接收的不一致！", false);
                            return;
                        }
                    }
                }
                else
                {
                    this.ShowMsg("调用AH退款接口失败，原因：接口返回值集合为空！", false);
                    return;
                }
                #endregion 先验证AH同步接口
            }
            //执行退货代码
            if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
            {
                this.ShowMsg("审核失败，请重试。", false);
                return;
            }
            MemberInfo memberInfo = MemberHelper.GetMember(orderInfo.UserId);
            if (memberInfo != null)
            {
                Messenger.OrderReturnGoodsOverSendManage(orderInfo, memberInfo, true);
            }
            this.ShowMsg("审核成功！", true);
            this.LoadReturnsApplyData();
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
                this.ShowMsg("输入的金额格式不正确", false);
                return;
            }
            refundInfo.RefundMoney = num;
            if (num < new decimal(0))
            {
                this.ShowMsg("不能为负数！", false);
                return;
            }
            if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
            {
                this.ShowMsg("操作失败，请重试。", false);
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
                //RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, 3)
                if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, (int)orderInfo.OrderStatus))
                {
                    MemberInfo memberInfo = MemberHelper.GetMember(orderInfo.UserId);
                    if (memberInfo != null)
                    {
                        Messenger.OrderReturnGoodsOverSendManage(orderInfo, memberInfo, false);
                    }

                    this.ShowMsg("操作成功", true);
                    this.LoadReturnsApplyData();
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
                this.ShowMsg("请选要删除的退款申请单", false);
                return;
            }
            string str = "成功删除了{0}个退款申请单";
            char[] chrArray = new char[] { ',' };
            str = (!RefundHelper.DelRefundApply(item.Split(chrArray), out num) ? string.Concat(string.Format(str, num), ",待处理的申请不能删除") : string.Format(str, num));
            this.LoadReturnsApplyData();
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
                    label.Text = "未审核";
                    return;
                }
                if (label.Text == "5")
                {
                    label.Text = "已审核";
                    return;
                }
                if (label.Text == "6")
                {
                    label.Text = "未退款";
                    return;
                }
                if (label.Text == "2")
                {
                    label.Text = "已退款";
                    return;
                }
                if (label.Text == "8")
                {
                    label.Text = "拒绝退款";
                    return;
                }
                label.Text = "审核不通过";
            }
        }

        private void LoadReturnsApplyData()
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
                returnsApplyQuery.HandleStatus = new int?(-2);
            }
            else
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num))
                {
                    returnsApplyQuery.HandleStatus = new int?(num);
                }
            }
            //2016-08-09验证当前登陆用户类型
            bool isSupplier = false;//是否供应商
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.SupplierRoleId)
            {
                isSupplier = true;//当前登陆为供应商用户
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
            orderInfo.CloseReason = "客户要求退货(款)！";
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
                this.ShowMsg("输入的金额格式不正确", false);
                return;
            }
            if (num < new decimal(0))
            {
                this.ShowMsg("不能为负数！", false);
                return;
            }
            refundInfo.RefundMoney = num;
            if (!RefundHelper.UpdateByReturnsId(refundInfo))
            {
                this.ShowMsg("操作失败，请重试。", false);
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
                //(int.Parse(this.hidStatus.Value) == 6 ? 2 : 3)
                if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, (int)orderInfo.OrderStatus))
                {
                    this.ShowMsg("操作成功", true);
                    this.LoadReturnsApplyData();
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
                this.ShowMsg("输入的金额格式不正确", false);
                return;
            }
            refundInfo.RefundMoney = num;
            if (num < new decimal(0))
            {
                this.ShowMsg("不能为负数！", false);
                return;
            }
            if (!RefundHelper.UpdateByReturnsId(refundInfo))
            {
                this.ShowMsg("退款失败，请重试。", false);
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
                if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, 9))
                {
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




                    this.ShowMsg("成功退款", true);
                    this.LoadReturnsApplyData();
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
                this.LoadReturnsApplyData();
            }
        }
    }
}