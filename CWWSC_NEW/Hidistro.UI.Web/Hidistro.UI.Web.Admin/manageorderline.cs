using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Hidistro.Entities.Sales;
using System.Xml;
using Hidistro.Entities.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hishop.Weixin.MP.Util;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SupplierOrders)]//0xfa7
    public class manageorderline : AdminPage
    {
        protected string Reurl = string.Empty;

        protected HyperLink hlinkAllOrder;

        protected HyperLink hlinkNotPay;

        protected HyperLink hlinkYetPay;

        protected HyperLink hlinkSendGoods;

        protected HyperLink hlinkTradeFinished;

        protected HyperLink hlinkClose;

        protected HyperLink hlinkHistory;

        protected WebCalendar calendarStartDate;

        protected WebCalendar calendarEndDate;

        //start代理商处理相关添加内容
        protected DropDownList ddlOrderAgent;

        protected TextBox txtRealName;

        protected TextBox txtAllHereCode;

        protected TextBox txtUserName;

        protected TextBox txtOrderId;

        protected Label lblStatus;

        protected TextBox txtProductName;

        protected TextBox txtShopTo;

        protected DropDownList ddlIsPrinted;

        protected ShippingModeDropDownList shippingModeDropDownList;

        protected RegionSelector dropRegion;

        protected DropDownList OrderFromList;

        protected Button btnSearchButton;

        protected PageSize hrefPageSize;

        protected Pager pager1;

        protected ImageLinkButton lkbtnDeleteCheck;

        protected HtmlInputHidden hidOrderId;

        protected DataList dlstOrders;

        protected Pager pager;

        protected CloseTranReasonDropDownList ddlCloseReason;

        protected FormatedMoneyLabel lblOrderTotalForRemark;

        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

        protected TextBox txtRemark;

        protected Label lblOrderId;

        protected Label lblOrderTotal;

        protected Label lblRefundType;

        protected Label lblRefundRemark;

        protected Label lblContacts;

        protected Label lblEmail;

        protected Label lblTelephone;

        protected Label lblAddress;

        protected TextBox txtAdminRemark;

        protected Label return_lblOrderId;

        protected Label return_lblOrderTotal;

        protected Label return_lblRefundType;

        protected Label return_lblReturnRemark;

        protected Label return_lblContacts;

        protected Label return_lblEmail;

        protected Label return_lblTelephone;

        protected Label return_lblAddress;

        protected TextBox return_txtRefundMoney;

        protected TextBox return_txtAdminRemark;

        protected Label replace_lblOrderId;

        protected Label replace_lblOrderTotal;

        protected Label replace_lblComments;

        protected Label replace_lblContacts;

        protected Label replace_lblEmail;

        protected Label replace_lblTelephone;

        protected Label replace_lblAddress;

        protected Label replace_lblPostCode;

        protected TextBox replace_txtAdminRemark;

        protected HtmlInputHidden hidOrderTotal;

        protected HtmlInputHidden hidRefundType;

        protected HtmlInputHidden hidRefundMoney;

        protected HtmlInputHidden hidAdminRemark;

        protected Button btnCloseOrder;

        protected Button btnAcceptRefund;

        protected Button btnRefuseRefund;

        protected Button btnAcceptReturn;

        protected Button btnRefuseReturn;

        protected Button btnAcceptReplace;

        protected Button btnRefuseReplace;

        protected Button btnRemark;

        protected Button btnOrderGoods;

        protected Button btnProductGoods;

        protected Button btnExport;

        protected HtmlControl agentPpurchase;//代理商采购相关功能区域

        protected System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;//判断是不是爽爽挝啡的隐藏域

        protected DropDownList ddlStoreType;

        //订单来源
        //protected DropDownList ddlOrderSource;

        protected Button btnPack;
        protected Button btnUnPack;

        protected FileUpload fileOrderInfoPack;
        protected DropDownList dropExpress;

        //分公司绑定(线下商品)
        protected Repeater ReFilialeIdOrder;
        protected TextBox txtFiliale;
        protected HtmlInputHidden hidFilialeId;
        //分公司与总部操作权限区分
        protected HtmlInputHidden hidFilialeOrder;
        private void bindOrderType()
        {
            int result = 0;
            int.TryParse(base.Request.QueryString["orderType"], out result);
            this.OrderFromList.SelectedIndex = result;
        }

        private void btnRefuseRefund_Click(object obj, EventArgs eventArg)
        {
            string userName = ManagerHelper.GetCurrentManager().UserName;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(hidOrderId.Value);
            RefundHelper.EnsureRefund(orderInfo.OrderId, userName, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退款", true);
        }

        private void btnCloseOrder_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(orderInfo))
            {
                orderInfo.OnClosed();
                this.BindOrders();
                Messenger.OrderClosed(MemberHelper.GetMember(orderInfo.UserId), orderInfo, orderInfo.CloseReason);
                this.ShowMsg("关闭订单成功", true);
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        private void btnOrderGoods_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                string[] array = str.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Length; i++)
                {
                    string str2 = array[i];
                    list.Add("'" + str2 + "'");
                }
                System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>订单单号</td>");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("<td>备注</td>");
                builder.AppendLine("</tr>");
                foreach (System.Data.DataRow row in orderGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"] + "</td>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("<td>" + row["Remark"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }
        private void btnProductGoods_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                string[] array = str.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Length; i++)
                {
                    string str2 = array[i];
                    list.Add("'" + str2 + "'");
                }
                System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("</tr>");
                foreach (System.Data.DataRow row in productGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void dlstOrders_ItemDataBound(object obj, DataListItemEventArgs dataListItemEventArg)
        {
            bool flag;
            if (dataListItemEventArg.Item.ItemType == ListItemType.Item || dataListItemEventArg.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderStatus orderStatu = (OrderStatus)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "OrderStatus");
                string str = "";
                if (!(DataBinder.Eval(dataListItemEventArg.Item.DataItem, "Gateway") is DBNull))
                {
                    str = (string)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "Gateway");
                }
                int num = (DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyId")) != DBNull.Value ? (int)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyId") : 0;
                HyperLink hyperLink = (HyperLink)dataListItemEventArg.Item.FindControl("lkbtnEditPrice");
                Label label = (Label)dataListItemEventArg.Item.FindControl("lkbtnSendGoods");

                //如果是自提方式，则由门店发货
                DataRowView drv = (DataRowView)dataListItemEventArg.Item.DataItem;
                string strModeName = (drv["ModeName"] != DBNull.Value) ? drv["ModeName"].ToString() : "";
                string strRealModeName = (drv.Row.Table.Columns.Contains("RealModeName") && drv["RealModeName"] != DBNull.Value) ? drv["RealModeName"].ToString() : "";
                if (strRealModeName.IndexOf("自提") > -1 || (strRealModeName == "" && strModeName.IndexOf("自提") > -1))
                {
                    if (drv["ReferralUserId"].ToString() != "0")
                    {
                        label.Enabled = false;
                        label.ToolTip = "该订单为自提方式，由门店发货！";
                        label.Text = "<a style='color:Red;display:block;' href='javascript:ShowMsg(\"该订单为自提方式，由门店发货\",true)'>发货</a>";
                    }
                }

                ImageLinkButton imageLinkButton = (ImageLinkButton)dataListItemEventArg.Item.FindControl("lkbtnPayOrder");
                ImageLinkButton imageLinkButton1 = (ImageLinkButton)dataListItemEventArg.Item.FindControl("lkbtnConfirmOrder");
                Literal literal = (Literal)dataListItemEventArg.Item.FindControl("litCloseOrder");
                HtmlAnchor lkbtnsendgk = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnsendgk");
                HtmlAnchor htmlAnchor = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckRefund");
                HtmlAnchor htmlAnchor1 = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckReturn");
                HtmlAnchor htmlAnchor2 = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckReplace");
                if (orderStatu == OrderStatus.WaitBuyerPay)
                {
                    hyperLink.Visible = true;
                    literal.Visible = true;
                    //if (str != "hishop.plugins.payment.podrequest")
                    if (str == "hishop.plugins.payment.offlinerequest")
                    {
                        imageLinkButton.Visible = true;
                    }
                }
                if (orderStatu == OrderStatus.ApplyForRefund)
                {
                    lkbtnsendgk.Visible = true;
                    htmlAnchor.Visible = true;
                }
                if (orderStatu == OrderStatus.ApplyForReturns)
                {
                    htmlAnchor1.Visible = true;
                }
                if (orderStatu == OrderStatus.ApplyForReplacement)
                {
                    htmlAnchor2.Visible = true;
                }
                if (num <= 0)
                {
                    Label label1 = label;
                    if (orderStatu == OrderStatus.BuyerAlreadyPaid)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = (orderStatu != OrderStatus.WaitBuyerPay ? false : str == "hishop.plugins.payment.podrequest");
                    }
                    label1.Visible = flag;
                }
                else
                {
                    GroupBuyStatus groupBuyStatu = (GroupBuyStatus)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyStatus");
                    label.Visible = (orderStatu != OrderStatus.BuyerAlreadyPaid ? false : groupBuyStatu == GroupBuyStatus.Success);
                }
                imageLinkButton1.Visible = orderStatu == OrderStatus.SellerAlreadySent;
            }
        }

        private void btnSendGoods_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + str));
            }
        }
        private void BindOrders()
        {
            //2016-08-08验证当前登陆用户类型
            bool isStoreId = false;//是否门店
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.StoreRoleId)
            {
                isStoreId = true;//当前登陆为门店用户
            }
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                this.hidFilialeOrder.Value="分公司";
                isFiliale = true;//当前登陆为分公司用户
            }

            OrderQuery orderQuery = this.GetOrderQuery();
            orderQuery.OrderSource = 4;
            orderQuery.referralUserId = isStoreId ? currentManager.ClientUserId : 0;//门店订单筛选，只显示门店相关订单
            orderQuery.FilialeId = isFiliale ? currentManager.ClientUserId : 0;//分公司订单筛选，分公司相关线下订单
            DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            this.dlstOrders.DataSource = orders.Data;
            this.dlstOrders.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;
            this.txtAllHereCode.Text = orderQuery.AllHereCode;
            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;
            this.dropExpress.SelectedValue = orderQuery.ExpressCompanyName;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.lblStatus.Text = orderQuery.Status.ToString();
            this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
            
            if (orderQuery.IsPrinted.HasValue)
            {
                this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
            }
            if (orderQuery.RegionId.HasValue)
            {
                this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
            }

            //代理商采购添加
            if (orderQuery.OrderAgent > 0)
                this.ddlOrderAgent.SelectedValue = orderQuery.OrderAgent.ToString();
            this.txtRealName.Text = orderQuery.RealName;
            this.ddlStoreType.SelectedValue = orderQuery.StoreType;
            //this.ddlOrderSource.SelectedValue = orderQuery.OrderSource.ToString();
            this.hidFilialeId.Value = orderQuery.FilialeId.ToString();
            this.txtFiliale.Text = orderQuery.fgsName;
        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AllHereCode"]))
            {
                query.AllHereCode = Globals.UrlDecode(this.Page.Request.QueryString["AllHereCode"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out num))
                {
                    query.Status = (OrderStatus)num;
                }
                else
                {
                    switch (this.Page.Request.QueryString["OrderStatus"].ToString())
                    {
                        case "All":
                            query.Status = OrderStatus.All;
                            break;
                        case "WaitBuyerPay":
                            query.Status = OrderStatus.WaitBuyerPay;
                            break;
                        case "BuyerAlreadyPaid":
                            query.Status = OrderStatus.BuyerAlreadyPaid;
                            break;
                        case "SellerAlreadySent":
                            query.Status = OrderStatus.SellerAlreadySent;
                            break;
                        case "Finished":
                            query.Status = OrderStatus.Finished;
                            break;
                        case "Closed":
                            query.Status = OrderStatus.Closed;
                            break;
                        case "History":
                            query.Status = OrderStatus.History;
                            break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int num3 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            int num4;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out num4))
            {
                query.RegionId = new int?(num4);
            }
            int num5;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && int.TryParse(this.Page.Request.QueryString["UserId"], out num5))
            {
                query.UserId = new int?(num5);
            }
            //下属信息浏览添加
            int num7;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]) && int.TryParse(this.Page.Request.QueryString["ReferralUserId"], out num7))
            {
                query.referralUserId = new int?(num7);
            }
            int result = 0;
            if (int.TryParse(base.Request.QueryString["orderType"], out result) && result > 0)
            {
                query.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)result);
            }
            //代理商采购添加
            int num6;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderAgent"]) && int.TryParse(this.Page.Request.QueryString["OrderAgent"], out num6))
            {
                query.OrderAgent = new int?(num6);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
            {
                query.RealName = Globals.UrlDecode(this.Page.Request.QueryString["RealName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreType"]))
            {
                query.StoreType = Globals.UrlDecode(this.Page.Request.QueryString["StoreType"]);
            }
            int num8;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderSource"]) && int.TryParse(this.Page.Request.QueryString["OrderSource"], out num8))
            {
                query.OrderSource = num8;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ExpressCompanyName"]))
            {
                query.ExpressCompanyName = Globals.UrlDecode(this.Page.Request.QueryString["ExpressCompanyName"]);
            }

            int num9;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FilialeId"]) && int.TryParse(this.Page.Request.QueryString["FilialeId"], out num9))
            {
                query.FilialeId = new int?(num9);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["txtFiliale"]))
            {
                query.fgsName= Globals.UrlDecode(this.Page.Request.QueryString["txtFiliale"]);
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        private void ReloadOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("AllHereCode", this.txtAllHereCode.Text);
            queryStrings.Add("UserName", this.txtUserName.Text);
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("ProductName", this.txtProductName.Text);
            queryStrings.Add("ShipTo", this.txtShopTo.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("OrderType", this.OrderFromList.SelectedValue);
            queryStrings.Add("OrderStatus", this.lblStatus.Text);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                queryStrings.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                queryStrings.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
            }
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                queryStrings.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlOrderAgent.SelectedValue))
            {
                queryStrings.Add("OrderAgent", this.ddlOrderAgent.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.ddlStoreType.SelectedValue))
            {
                queryStrings.Add("StoreType", this.ddlStoreType.SelectedValue);
            }
            //if (!string.IsNullOrEmpty(this.ddlOrderSource.SelectedValue))
            //{
            //    queryStrings.Add("OrderSource", this.ddlOrderSource.SelectedValue);
            //}
            if (!string.IsNullOrEmpty(this.dropExpress.SelectedValue))
            {
                queryStrings.Add("ExpressCompanyName", this.dropExpress.SelectedValue);
            }
            queryStrings.Add("RealName", this.txtRealName.Text);
            if (!string.IsNullOrEmpty(this.ddlCloseReason.SelectedValue))
            {
                queryStrings.Add("OrderAgent", this.ddlOrderAgent.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.hidFilialeId.Value))
            {
                queryStrings.Add("FilialeId", this.hidFilialeId.Value);
            }
            if (!string.IsNullOrEmpty(this.txtFiliale.Text))
            {
                queryStrings.Add("txtFiliale", this.txtFiliale.Text);
            }
            base.ReloadPage(queryStrings);
        }
        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/sales/manageorderline.aspx?orderStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            this.hlinkHistory.NavigateUrl = string.Format(format, 99);
        }

        private void btnRemark_Click(object sender, System.EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtRemark.Text))
                {
                    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                    orderInfo.OrderId = this.hidOrderId.Value;
                    if (this.orderRemarkImageForRemark.SelectedItem != null)
                    {
                        orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                    }
                    orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                    if (OrderHelper.SaveRemark(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMsg("保存备忘录成功", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败", false);
                    }
                }
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            string userName = ManagerHelper.GetCurrentManager().UserName;
            if (RefundHelper.EnsureRefund(OrderHelper.GetOrderInfo(this.hidOrderId.Value).OrderId, userName, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindOrders();
                this.ShowMsg("成功的确认了订单退款", true);
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadOrders(true);
        }

        protected void dlstOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            bool flag = false;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    int groupBuyId = orderInfo.GroupBuyId;
                    if (!OrderHelper.ConfirmPay(orderInfo))
                    {
                        this.ShowMsg("确认订单收款失败", false);
                        return;
                    }
                    DebitNoteInfo debitNoteInfo = new DebitNoteInfo();

                    debitNoteInfo.NoteId = Globals.GetGenerateId();
                    debitNoteInfo.OrderId = e.CommandArgument.ToString();
                    debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
                    debitNoteInfo.Remark = string.Concat("后台", debitNoteInfo.Operator, "收款成功");

                    OrderHelper.SaveDebitNote(debitNoteInfo);
                    if (orderInfo.GroupBuyId > 0)
                    {
                        PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                    }
                    this.BindOrders();
                    orderInfo.OnPayment();
                    this.ShowMsg("成功的确认了订单收款", true);
                    return;
                }
                if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
                    LineItemInfo lineItemInfo = new LineItemInfo();
                    foreach (KeyValuePair<string, LineItemInfo> lineItem in lineItems)
                    {
                        lineItemInfo = lineItem.Value;
                        if (lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForRefund && lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForReturns)
                        {
                            continue;
                        }
                        flag = true;
                    }
                    if (!flag)
                    {
                       
                        //2016-11-07 开始调用完成订单接口
                        StringBuilder strJson = new StringBuilder();
                        strJson.Append("{");
                        strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//订单号
                        //strJson.AppendFormat("\"FinishDate\":\"{0}\"", orderInfo.FinishDate);//订单完成时间
                        //strJson.AppendFormat("\"FinishDate\":\"{0}\"", DateTime.Now);//订单完成时间
                        strJson.AppendFormat("\"Date\":\"{0}\",", DateTime.Now);//订单完成时间
                        strJson.AppendFormat("\"OrderState\":\"{0}\"", "Finish");//订单完成时间
                        strJson.Append("}");
                        CwAHAPI.CwapiLog("**************发送数据：" + strJson.ToString());

                        AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                        try
                        {
                            string strResult = ahservice.MPFTOJL_DHD_QS(strJson.ToString());
                            CwAHAPI.CwapiLog("**************返回内容：" + strResult);
                            string orderid = string.Empty;
                            string message = string.Empty;
                            if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                            {
                                this.ShowMsg(string.Format("调用AH订单完成接口失败，原因为：{0}", message), false);
                                return;
                            }
                            if (orderid != orderInfo.OrderId)
                            {
                                this.ShowMsg("调用AH订单完成接口失败，原因为：接口返回的订单编码与发送时不一致。", false);
                                return;
                            }
                        }
                        catch
                        {
                            this.ShowMsg("操作失败，原因：访问金力完成接口无响应。", false);
                            return;
                        }
                        //开始执行订单完成代码
                        if (!OrderHelper.ConfirmOrderFinish(orderInfo))
                        {
                            this.ShowMsg("完成订单失败", false);
                            return;
                        }
                        this.BindOrders();
                        DistributorsBrower.UpdateCalculationCommission(orderInfo);
                        foreach (LineItemInfo value in orderInfo.LineItems.Values)
                        {
                            if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString())
                            {
                                continue;
                            }
                            RefundHelper.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, 5);
                        }
                        //遍历订单车中包含[会员充值]的商品,生成等价的优惠券给用户
                        if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.CouponRecharge)
                        {
                            //找到所有会员充值的商品
                            List<LineItemInfo> couponGoodList = lineItems.Values.Where(n => Hidistro.ControlPanel.Commodities.ProductHelper.GetProductBaseInfo(n.ProductId).ProductName == "会员充值").ToList();
                            //创建后台couponInfo
                            foreach (LineItemInfo info in couponGoodList)
                            {
                                for (int i = 0; i < info.Quantity; i++)
                                {
                                    CouponInfo target = new CouponInfo
                                    {
                                        Name = "会员充值" + info.ItemAdjustedPrice.ToString("F2") + "元",
                                        ClosingTime = DateTime.MaxValue,
                                        StartTime = DateTime.Now,
                                        Amount = 0M,
                                        DiscountValue = info.ItemAdjustedPrice,
                                        NeedPoint = 0
                                    };
                                    string lotNumber = string.Empty;

                                    int resultCouponId = CouponHelper.CreateCoupon(target, 0);
                                    IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
                                    if (resultCouponId > 0) //如果后台新建成功或者已经存在,开始对用户进行发送
                                    {
                                        CouponItemInfo item = new CouponItemInfo();
                                        string claimCode = string.Empty;
                                        claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                        item = new CouponItemInfo(resultCouponId, claimCode, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now);
                                        listCouponItem.Add(item);
                                    }
                                    CouponHelper.SendClaimCodes(resultCouponId, listCouponItem);
                                }
                            }
                        }
                        this.ShowMsg("成功的完成了该订单", true);
                        return;
                    }
                    this.ShowMsg("订单中商品有退货(款)不允许完成!", false);

                }
            }
        }

        protected void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的订单", false);
            }
            else
            {
                int num = OrderHelper.DeleteOrders("'" + str.Replace(",", "','") + "'");
                this.BindOrders();
                this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
            }
        }
        /// <summary>
        /// 整理购物车中商品的规格内容,保持与前端一致
        /// </summary>
        /// <param name="skuContent">商品的原规格字符串</param>
        /// <returns>整理后的字符串</returns>ss
        private static string skuContentFormat(string skuContent)
        {
            skuContent = skuContent.Trim().TrimEnd(';');
            IList<string> skus = skuContent.Split(';');
            if (skus.Count <= 0 || skus[0] == "") return "默认";
            string result = string.Empty;
            for (int i = skus.Count - 1; i >= 0; i--)
            {
                skus[i] = skus[i].Substring(skus[i].IndexOf("：") + 1);
                result += skus[i] + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }
        /// <summary>
        /// 打印订单信息(sswf)
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string PrintOrderInfo(string OrderIds)
        {
            string backJson = string.Empty;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };

                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                    builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/login_logo2.png' /><h3>SALES MEMO</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>顾客名称： </span>{3}</div><div style='text-align:left;padding-bottom:5px;'><span >联系电话： </span>{5}</div><div style='text-align:left;padding-bottom:5px;'><span>联系地址： </span>{4}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId, order.Username, order.Address, order.CellPhone);
                    builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:50%;background:#fff;border:none;text-align:left;'>菜名</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", info2.GetSubShowTotal().ToString("F2"));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    if (reducedPromotionAmount1 > 0m)
                    {
                        builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2));
                    }
                    decimal payCharge1 = order.PayCharge;
                    if (payCharge1 > 0m)
                    {
                        builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                    }
                    if (!string.IsNullOrEmpty(order.CouponCode))
                    {
                        decimal couponValue = order.CouponValue;
                        if (couponValue > 0m)
                        {
                            builder.AppendFormat("<div><span>优惠抵扣： </span>-￥{0}</div>", System.Math.Round(couponValue, 2));
                        }
                    }
                    //计算买一送一减免
                    decimal giveBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {
                        if (itemInfo.GiveQuantity > 0)
                        {
                            giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));

                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                    builder.Append("</div>");
                }
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                //return "success,str=123";
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
        }
        /// <summary>
        /// 三作咖啡发货打印订单
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string StampOrderInfo(string OrderIds)
        {
            string backJson = string.Empty;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                    builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/三作咖啡.png' /><h3>SanZuo Coffee</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>顾客名称： </span>{3}</div><div style='text-align:left;padding-bottom:5px;'><span >联系电话： </span>{5}</div><div style='text-align:left;padding-bottom:5px;'><span>联系地址： </span>{4}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId, order.Username, order.Address, order.CellPhone);
                    builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:50%;background:#fff;border:none;text-align:left;'>菜名</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", info2.GetSubShowTotal().ToString("F2"));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    //计算买一送一减免
                    decimal giveBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {
                        if (itemInfo.GiveQuantity > 0)
                        {
                            giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));

                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>三作咖啡外卖送货单</div>");
                    builder.Append("</div>");
                }
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        public void packOrderInfos(object sender, EventArgs e)
        {
            string backJson = string.Empty;
            OrderQuery orderQuery = this.GetOrderQuery();
            string path = OrderHelper.PackOrderInfos(orderQuery.StartDate.ToString(), orderQuery.EndDate.ToString());
            Hidistro.Entities.FilePlus.DownFile(this.Page, path);
        }
        /// <summary>
        /// 导入订单
        /// </summary>
        public void UnPackOrderInfos(object sender, EventArgs e)
        {
            //反序列化订单文件并整表提交
            if (!string.IsNullOrEmpty(fileOrderInfoPack.FileName) && fileOrderInfoPack.FileContent.Length > 0)
            {
                string filePath = MapPath("/Storage/temp/") + "unpack_" + fileOrderInfoPack.FileName;
                fileOrderInfoPack.SaveAs(filePath);
                DataSet dsOrdersInfo = ObjectSerializerHelper.DataSetDeserialize(filePath);
                //拼接主键id,用于得出ds差别
                string orderIds = string.Empty;
                foreach (DataRow row in dsOrdersInfo.Tables[0].Rows)
                {
                    orderIds = orderIds + "'" + row["OrderId"] + "'" + ",";
                }
                orderIds = orderIds.TrimEnd(',');
                string selectSql = "select * from Hishop_Orders where OrderId in (" + orderIds + ")" + ";" + "select * from Hishop_OrderItems where OrderId in (" + orderIds + ")";
                DataSet dsOrdersInfoCurrent = DataBaseHelper.GetDataSet(selectSql);
                dsOrdersInfo.Tables[0].PrimaryKey = new DataColumn[] { dsOrdersInfo.Tables[0].Columns["OrderId"] };
                dsOrdersInfo.Tables[1].PrimaryKey = new DataColumn[] { dsOrdersInfo.Tables[1].Columns["OrderId"] };
                dsOrdersInfoCurrent.Tables[0].PrimaryKey = new DataColumn[] { dsOrdersInfoCurrent.Tables[0].Columns["OrderId"] };
                dsOrdersInfoCurrent.Tables[1].PrimaryKey = new DataColumn[] { dsOrdersInfoCurrent.Tables[1].Columns["OrderId"] };


                dsOrdersInfoCurrent = DataBaseHelper.GetDsDifferent(dsOrdersInfoCurrent, dsOrdersInfo, false);

                string[] selectSqls = selectSql.Split(';');


                if (dsOrdersInfo.Tables.Count > 0)
                {
                    int count = DataBaseHelper.CommitDataSet(dsOrdersInfoCurrent, selectSqls);
                    if (count > 0)
                    {
                        this.ShowMsg("导入成功!", true);
                    }
                }
            }
            else
            {
                this.ShowMsg("请上传数据文件", false);
            }

        }
        /// <summary>
        /// 打印日结单(sswf)
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string todayOrderPrint(string startDate, string endDate)
        {
            int couponCount = 0;
            decimal couponTotalPrice = 0m;//优惠券总价
            int giveCount = 0;
            decimal givePrice = 0m;//买一送一总价
            int orderCount = 0;//订单总数
            decimal orderTotalPrice = 0m;//订单总价
            int pcOrderCount = 0;//店内订单总数
            decimal pcOrderTotalPrice = 0m;//店内订单总价
            int mobileOrderCount = 0;//移动端订单总数
            int microPayOrderCount = 0;//店内微信扫码支付总数
            decimal microPayOrderTotalPrice = 0m;//店内微信扫码支付总价
            decimal mobileOrderTotalPrice = 0m;//移动端订单总价
            decimal totalPriceGot = 0m;//实收总价

            string backJson = string.Empty;
            try
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                int senderId = 0;
                string storeName = "";
                if (ManagerHelper.GetRole(currentManager.RoleId).RoleName != "超级管理员") //如果当前后台账号不是超级管理员,则日结单根据当前账号id结合sender进行过滤
                {
                    senderId = currentManager.UserId;
                    storeName = DistributorsBrower.GetUserIdDistributors(DistributorsBrower.GetSenderDistributorId(senderId.ToString())).StoreName;
                }
                IList<string> orderIds = OrderHelper.GetTodayOrders(startDate, endDate, senderId, storeName); //将今天的orderid拆分成数组
                string rrr = "";
                foreach (string a in orderIds)
                {
                    rrr += a + ",";
                }
                DataTable dtProducts = OrderHelper.GetTodayProducts(startDate, endDate, senderId, storeName);//将今天的所有卖出的商品存在DataTable里,下面循环会往里面填值
                dtProducts.PrimaryKey = new DataColumn[] { dtProducts.Columns["ProductId"] };

                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                //头部,开始时间,结束时间,制单时间
                builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><h3>门店统计日结报表</h3><div style='text-align:left;padding-bottom:5px;'><span>开始时间： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>结束时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>制单时间： </span>{2}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", startDate.ToString(), endDate.ToString(), DateTime.Now.ToString());
                //列表table
                builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:65%;background:#fff;border:none;text-align:left;'>项目</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    //统计优惠券总价
                    if (!string.IsNullOrEmpty(order.CouponCode))
                    {
                        couponCount++;
                        couponTotalPrice += System.Math.Round(order.CouponValue);
                    }
                    //统计堂食总价和数量
                    if ((order.Username == "[堂食用户]" || order.Username == "[匿名用户]" || order.Username == "[活动用户]") && order.RealModeName != "微信扫码支付")
                    {
                        pcOrderCount++;
                        pcOrderTotalPrice += order.GetTotal();//实际消费(减去了买一送一和优惠券的金额)
                        totalPriceGot += order.GetTotal();
                    }
                    //统计移动端的总价和数量
                    if (order.Username != "[堂食用户]" && order.Username != "[匿名用户]" && order.Username != "[活动用户]")
                    {
                        mobileOrderCount++;
                        mobileOrderTotalPrice += order.GetTotal();
                    }
                    //统计店内微信扫码支付总价和数量
                    if ((order.Username == "[堂食用户]" || order.Username == "[匿名用户]" || order.Username == "[活动用户]") && order.RealModeName == "微信扫码支付")
                    {
                        microPayOrderCount++;
                        microPayOrderTotalPrice += order.GetTotal();
                    }
                    //订单数量和总价统计
                    orderCount++;
                    orderTotalPrice += order.GetShowAmount();

                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            //统计买一送一赠送总价
                            giveCount += info2.GiveQuantity;
                            givePrice += (info2.GetSubShowTotal() - info2.GetSubTotal());//计算出买一送一的价格
                            //查找商品表,根据productid来刷新相应的商品的数量和金额
                            DataRow drProduct = dtProducts.Rows.Find(info2.ProductId);
                            if (drProduct != null)
                            {
                                drProduct["quantity"] = Convert.ToInt32(drProduct["quantity"]) + info2.Quantity;
                                drProduct["Price"] = Convert.ToDecimal(drProduct["Price"]) + info2.GetSubShowTotal();
                            }
                        }
                    }

                }


                foreach (DataRow row in dtProducts.Rows)
                {
                    builder.AppendFormat("<td>{0}</td>", row["ProductName"].ToString());
                    builder.AppendFormat("<td style='text-align:center;'>{0}</td>", row["quantity"].ToString());
                    builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", Convert.ToDecimal(row["price"]).ToString("F2"));
                }



                builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");
                //底部
                builder.AppendFormat("<div style='width:100%;float:left;font-size:14px;padding-bottom:3px;'><span>订单总数：</span>{0}</div>", orderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>实收金额：</span>{0}</div>", Convert.ToDouble(totalPriceGot));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>消费金额：</span>{0}</div>", Convert.ToDouble(orderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>微信订单：</span>{0}</div>", mobileOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>微信消费：</span>{0}</div>", Convert.ToDouble(mobileOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>店内订单：</span>{0}</div>", pcOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>店内消费：</span>{0}</div>", Convert.ToDouble(pcOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>扫码支付订单：</span>{0}</div>", microPayOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>扫码支付消费：</span>{0}</div>", Convert.ToDouble(microPayOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>买送数量：</span>{0}</div>", giveCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>买送减免：</span>{0}</div>", Convert.ToDouble(givePrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>优惠券数量：</span>{0}</div>", couponCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>优惠券减免：</span>{0}</div>", Convert.ToDouble(couponTotalPrice));



                //builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                builder.Append("</div>");

                /*
                    
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    
                    
                    


                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    if (reducedPromotionAmount1 > 0m)
                    {
                        builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2));
                    }
                    decimal payCharge1 = order.PayCharge;
                    if (payCharge1 > 0m)
                    {
                        builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                    }

                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));


                
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                //return "success,str=123";
                return builder.ToString();               */
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            string str;
            string str1;

            /*star***********线下订单绑定分公司查询列表 2017-8-4 yk************/
            DataTable dtFiliale = FilialeHelper.GetFilialeBaseInfo();
            ReFilialeIdOrder.DataSource = dtFiliale;
            ReFilialeIdOrder.DataBind();
            /*end*************************/
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction == false)//如果区域功能没有打开,则隐藏相关功能
                agentPpurchase.Visible = false;
            this.Reurl = base.Request.Url.ToString();

            if (!this.Reurl.Contains("?"))
            {
                manageorderline manageorderline = this;
                manageorderline.Reurl = string.Concat(manageorderline.Reurl, "?pageindex=1");
            }
            this.Reurl = Regex.Replace(this.Reurl, @"&t=(\d+)", "");
            this.Reurl = Regex.Replace(this.Reurl, @"(\?)t=(\d+)", "?");
            if ((string.IsNullOrEmpty(base.Request["isCallback"]) ? false : base.Request["isCallback"] == "true"))
            {
                if (string.IsNullOrEmpty(base.Request["ReturnsId"]))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
                StringBuilder stringBuilder = new StringBuilder();
                if (base.Request["type"] != "refund")
                {
                    num = 0;
                    str = "";
                }
                else
                {
                    RefundHelper.GetRefundType(base.Request["orderId"], out num, out str);
                }
                str1 = (num != 1 ? "银行转帐" : "退到预存款");
                stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
                if (base.Request["type"] != "replace")
                {
                    stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                    stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", str1);
                }
                stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.ShipTo);
                stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
                stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", string.Concat(orderInfo.TelPhone, " "), orderInfo.CellPhone.Trim());
                stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
                stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", str.Replace(",", ""));
                stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
                stringBuilder.AppendFormat(",\"GroupBuyId\":\"{0}\"", (orderInfo.GroupBuyId > 0 ? orderInfo.GroupBuyId : 0));
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write(string.Concat("{\"Status\":\"1\"", stringBuilder.ToString(), "}"));
                base.Response.End();
            }
            this.btnUnPack.Click += new EventHandler(this.UnPackOrderInfos);
            this.btnPack.Click += new EventHandler(this.packOrderInfos);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            this.dlstOrders.ItemDataBound += new DataListItemEventHandler(this.dlstOrders_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.dlstOrders.ItemCommand += new DataListCommandEventHandler(this.dlstOrders_ItemCommand);
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnProductGoods.Click += new EventHandler(this.btnProductGoods_Click);
            this.btnOrderGoods.Click += new EventHandler(this.btnOrderGoods_Click);
            this.btnExport.Click += new EventHandler(this.btnExPortButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.shippingModeDropDownList.DataBind();
                this.ddlIsPrinted.Items.Clear();
                this.ddlIsPrinted.Items.Add(new ListItem("全部", string.Empty));
                this.ddlIsPrinted.Items.Add(new ListItem("已打印", "1"));
                this.ddlIsPrinted.Items.Add(new ListItem("未打印", "0"));
                this.SetOrderStatusLink();
                this.bindOrderType();
                this.BindOrders();

                ViewState["StoreFiter"] = (this.specialHideShow.Value == "sswk") ? "1" : "0";
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        protected void btnExPortButton_Click(object sender, EventArgs e)
        {
            //2016-08-08验证当前登陆用户类型
            bool isSupplier = false;//是否供应商
            //bool isStoreId = false;//是否门店
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.SupplierRoleId)
            {
                isSupplier = true;//当前登陆为供应商用户
            }
            //else if (currentManager.RoleId == masterSettings.StoreRoleId)
            //{
            //    isStoreId = true;//当前登陆为门店用户
            //}

            //得到条件对象
            OrderQuery query = new OrderQuery();
            query.AllHereCode = this.txtAllHereCode.Text;
            query.UserName = this.txtUserName.Text;
            query.OrderId = this.txtOrderId.Text;
            query.ProductName = this.txtProductName.Text;
            query.ShipTo = txtShopTo.Text;
            int num = 0;
            if (!string.IsNullOrEmpty(this.lblStatus.Text))
            {
                if (int.TryParse(this.lblStatus.Text, out num))
                {
                    query.Status = (OrderStatus)num;
                }
                else
                {
                    switch (this.lblStatus.Text)
                    {
                        case "All":
                            query.Status = OrderStatus.All;
                            break;
                        case "WaitBuyerPay":
                            query.Status = OrderStatus.WaitBuyerPay;
                            break;
                        case "BuyerAlreadyPaid":
                            query.Status = OrderStatus.BuyerAlreadyPaid;
                            break;
                        case "SellerAlreadySent":
                            query.Status = OrderStatus.SellerAlreadySent;
                            break;
                        case "Finished":
                            query.Status = OrderStatus.Finished;
                            break;
                        case "Closed":
                            query.Status = OrderStatus.Closed;
                            break;
                        case "History":
                            query.Status = OrderStatus.History;
                            break;
                    }
                }
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                query.StartDate = new System.DateTime?(System.DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToString()));
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                query.EndDate = new System.DateTime?(System.DateTime.Parse(this.calendarEndDate.SelectedDate.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                int num3 = 0;
                if (int.TryParse(this.shippingModeDropDownList.SelectedValue.Value.ToString(), out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                int num2 = 0;
                if (int.TryParse(this.ddlIsPrinted.SelectedValue, out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            int num4;
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                if (int.TryParse(this.dropRegion.GetSelectedRegionId().Value.ToString(), out num4))
                {
                    query.RegionId = new int?(num4);
                }
            }
            int result = 0;
            if (int.TryParse(this.OrderFromList.SelectedValue, out result) && result > 0)
            {
                query.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)result);
            }
            //设置订单归属（供应商，与总部管理员区别显示）
            query.OrderSource = 2;
            if (isSupplier)
                query.SupplierId = currentManager.ClientUserId;
            query.referralUserId = 0;
            //query.SupplierId = isSupplier ? ManagerHelper.GetCurrentManager().ClientUserId : 0;
            //query.referralUserId = isStoreId ? ManagerHelper.GetCurrentManager().ClientUserId : 0;

            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            DataTable dt = new DataTable();
            //if (CustomConfigHelper.Instance.IsSanzuo)
            //    dt = OrderHelper.GetOrderSanZuo(query);
            //else
            dt = OrderHelper.GetOrder(query);
            fields.Add("ShipOrderNumber");
            list2.Add("物流单号");
            fields.Add("PaymentType");
            list2.Add("支付方式");
            fields.Add("ShipTo");
            list2.Add("收货人");
            fields.Add("CellPhone");
            list2.Add("联系电话");
            fields.Add("ShippingRegion");
            list2.Add("区域");
            fields.Add("Address");
            list2.Add("收货地址");
            fields.Add("OrderId");
            list2.Add("订单编号");
            fields.Add("OrderStatus");
            list2.Add("订单状态");
            fields.Add("OrderDate");
            list2.Add("提交时间");
            fields.Add("ProductName");
            list2.Add("商品名称");
            fields.Add("Quantity");
            list2.Add("商品数量");
            fields.Add("ItemAdjustedPrice");
            list2.Add("商品单价");
            fields.Add("OrderTotal");
            list2.Add("订单实收款");
            fields.Add("ItemsCommission");
            list2.Add("佣金(订单完成)");
            fields.Add("accountALLHere");
            list2.Add("DZ号");
            fields.Add("fgsName");
            list2.Add("分公司");
            fields.Add("ReferralUserId");
            list2.Add("门店名称");
            fields.Add("SKU");
            list2.Add("商品型号");
            fields.Add("BuyType");
            list2.Add("价格类型");
            fields.Add("DiscountAmount");
            list2.Add("满减减免");
            fields.Add("CouponValue");
            list2.Add("优惠价减免");
            fields.Add("Remark");
            list2.Add("买家留言");
            fields.Add("gysName");
            list2.Add("供应商");

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (System.Data.DataRow row in dt.Rows)
            {
                foreach (string str2 in fields)
                {
                    if (str2 == "IsPrinted")
                    {
                        builder.Append(row[str2] != DBNull.Value && (bool)row[str2] ? "已打印" : "未打印").Append(",");
                    }
                    else if (str2 == "ReferralUserId")
                    {
                        builder.Append(row[str2].ToString() == "" ? "主站" : row[str2].ToString() == "0" ? "主站" : row["StoreName"]).Append(",");
                    }
                    else if (str2 == "OrderStatus")
                    {
                        builder.Append((OrderInfo.GetOrderStatusName((OrderStatus)row["OrderStatus"])).Replace(",", "，")).Append(",");
                    }
                    else if (str2 == "accountALLHere")
                    {
                        builder.Append(row[str2].ToString() == "" ? "主站" : row["accountALLHere"]).Append(",");
                    }
                    else if (str2 == "BuyType")
                    {
                        switch (row[str2].ToString())
                        { 
                            case "1" :
                                builder.Append("团购").Append(",");
                                break;
                            case "2":
                                builder.Append("限时抢购").Append(",");
                                break;
                            case "3":
                                builder.Append("砍价").Append(",");
                                break;
                            case "4":
                                builder.Append("内购").Append(",");
                                break;
                            case "5":
                                builder.Append("批发").Append(",");
                                break;
                            case "6":
                                builder.Append("满减").Append(",");
                                break;
                            default :
                                builder.Append("一口价").Append(",");
                                break;
                        }
                    }
                    else
                    {
                        builder.Append(row[str2]).Append(",");
                    }
                    if (str2 == fields[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=订单信息.csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }
        //电商ID       
        private static string EBusinessID = "1259918";//"请到快递鸟官网申请http://www.kdniao.com/ServiceApply.aspx";
        //电商加密私钥，快递鸟提供，注意保管，不要泄漏
        private static string AppKey = "8822d37a-ea4a-4d83-bfe5-08c878ff53a5";//"请到快递鸟官网申请http://www.kdniao.com/ServiceApply.aspx";
        //请求url, 正式环境地址：http://api.kdniao.cc/api/Eorderservice
        //测试地址  http://testapi.kdniao.cc:8081/api/EOrderService  
        private static string ReqURL = "http://api.kdniao.cc/api/Eorderservice";
        /// <summary>
        /// Json方式  电子面单
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string orderTracesSubByJson(string OrderIds)
        {
            #region
            string printHtml = string.Empty;
            string productName = "";
            string requestData = "";
            int shipmentQuantity = 0;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    ShippersInfo shipper = SalesHelper.GetIsDefault(1);
                    if (shipper == null)
                    {
                        return ("请添加发货人信息！");

                    }
                    string[] regionAddress = RegionHelper.GetFullRegion(shipper.RegionId, ",").Split(',');
                    string[] regionId = RegionHelper.GetFullRegion(order.RegionId, ",").Split(',');
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str in lineItems1.Keys)
                        {
                            LineItemInfo info = lineItems1[str];
                            productName += info.ItemDescription;
                            shipmentQuantity = info.ShipmentQuantity;
                        }
                    }
                    string phone = order.TelPhone;
                    string shipperPhone = shipper.TelPhone;
                    if (order.TelPhone == null)
                    {
                        phone = order.CellPhone;
                    }
                    if (shipper.TelPhone == null)
                    {
                        shipperPhone = shipper.CellPhone;
                    }
                    if (order.ExpressCompanyName == "韵达快运" || order.ExpressCompanyName == "圆通速递")
                    {
                        if (order.ExpressCompanyName == "韵达快运")
                        {
                            #region

                            requestData = "{'OrderCode': '" + order.OrderId + "'," +
                                    "'ShipperCode':'YD'," +
                                    "'PayType':1," +
                                    "'ExpType':1," +
                                    "'Cost':" + order.Freight + "," +
                                    "'OtherCost':1.0," +
                                    "'Sender':" +
                                    "{" +
                                    "'Company':' ','Name':'" + shipper.ShipperName + "','Mobile':'" + shipperPhone + "','ProvinceName':'" + regionAddress.GetValue(0) + "','CityName':'" + regionAddress.GetValue(1) + "','ExpAreaName':'" + regionAddress.GetValue(2) + "','Address':'" + shipper.Address + "'}," +
                                    "'Receiver':" +
                                    "{" +
                                    "'Company':' ','Name':'" + order.ShipTo + "','Mobile':'" + phone + "','ProvinceName':'" + regionId.GetValue(0) + "','CityName':'" + regionId.GetValue(1) + "','ExpAreaName':'" + regionId.GetValue(2) + "','Address':'" + order.Address + "'}," +
                                    "'Commodity':" +
                                    "[{" +
                                    "'GoodsName':'" + productName + "','Goodsquantity':" + shipmentQuantity + ",'GoodsWeight':" + order.Weight + "}]," +
                                //"'AddService':" +
                                //"[{" +
                                //"'Name':'COD','Value':'1020'}]," +
                                   "'CustomerPwd':'kQcITjsnJPaURrDdFvXHZA3wxMNhGi'," +
                                   "'CustomerName':'43012088820'," +
                                    "'Weight':1.0," +
                                    "'Quantity':1," +
                                    "'Volume':0.0," +
                                    "'Remark':'小心轻放'," +
                                    "'IsReturnPrintTemplate':1}";
                            #endregion
                        }
                        if (order.ExpressCompanyName == "圆通速递")
                        {
                            #region
                            requestData = "{'OrderCode': '" + order.OrderId + "'," +
                                    "'ShipperCode':'YTO'," +
                                    "'PayType':1," +
                                    "'ExpType':1," +
                                    "'Cost':" + order.Freight + "," +
                                    "'OtherCost':1.0," +
                                    "'Sender':" +
                                    "{" +
                                    "'Company':' ','Name':'" + shipper.ShipperName + "','Mobile':'" + shipperPhone + "','ProvinceName':'" + regionAddress.GetValue(0) + "','CityName':'" + regionAddress.GetValue(1) + "','ExpAreaName':'" + regionAddress.GetValue(2) + "','Address':'" + shipper.Address + "'}," +
                                    "'Receiver':" +
                                    "{" +
                                    "'Company':' ','Name':'" + order.ShipTo + "','Mobile':'" + phone + "','ProvinceName':'" + regionId.GetValue(0) + "','CityName':'" + regionId.GetValue(1) + "','ExpAreaName':'" + regionId.GetValue(2) + "','Address':'" + order.Address + "'}," +
                                    "'Commodity':" +
                                    "[{" +
                                    "'GoodsName':'" + productName + "','Goodsquantity':" + shipmentQuantity + ",'GoodsWeight':" + order.Weight + "}]," +
                                //"'AddService':" +
                                //"[{" +
                                //"'Name':'COD','Value':'1020'}]," +
                                   "'MonthCode':'QXACQh8Y'," +
                                   "'CustomerName':'k11108881'," +
                                    "'Weight':1.0," +
                                    "'Quantity':1," +
                                    "'Volume':0.0," +
                                    "'Remark':'小心轻放'," +
                                    "'IsReturnPrintTemplate':1}";
                            #endregion
                        }
                    }
                    else
                    {
                        return ("快递公司命名不规范或为空！正确命名:'韵达快运'或'圆通速递'。");
                    }
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8));
                    param.Add("EBusinessID", EBusinessID);
                    param.Add("RequestType", "1007");
                    string dataSign = encrypt(requestData, AppKey, "UTF-8");
                    param.Add("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
                    param.Add("DataType", "2");

                    string result = sendPost(ReqURL, param);

                    JObject aaa = JsonToObject<JObject>(result);
                    printHtml = aaa["PrintTemplate"].ToString();
                }
                //根据公司业务处理返回的信息......
                return printHtml;
            }
            catch (Exception ex)
            {
                return "{" + ex.Message + "}";
            }
            #endregion
        }
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>        
        private static string sendPost(string url, Dictionary<string, string> param)
        {
            string result = "";
            StringBuilder postData = new StringBuilder();
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    if (postData.Length > 0)
                    {
                        postData.Append("&");
                    }
                    postData.Append(p.Key);
                    postData.Append("=");
                    postData.Append(p.Value);
                }
            }
            byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = url;
                request.Accept = "*/*";
                request.Timeout = 30 * 1000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.Method = "POST";
                request.ContentLength = byteData.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Flush();
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream backStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                result = sr.ReadToEnd();
                sr.Close();
                backStream.Close();
                response.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        ///<summary>
        ///电商Sign签名
        ///</summary>
        ///<param name="content">内容</param>
        ///<param name="keyValue">Appkey</param>
        ///<param name="charset">URL编码 </param>
        ///<returns>DataSign签名</returns>        
        private static string encrypt(String content, String keyValue, String charset)
        {
            if (keyValue != null)
            {
                return base64(MD5(content + keyValue, charset), charset);
            }
            return base64(MD5(content, charset), charset);
        }
        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>      
        private static string MD5(string str, string charset)
        {
            byte[] buffer = System.Text.Encoding.GetEncoding(charset).GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] somme = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte a in somme)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("X");
                    else
                        ret += a.ToString("X");
                }
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>      
        private static string base64(String str, String charset)
        {
            return Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(str));
        }
        public static T JsonToObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

    }

}