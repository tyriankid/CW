using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderStatistics)]
    public class OrderStatisticsSupplier : AdminPage
	{
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected WebCalendar calendarEndDate;
		protected WebCalendar calendarStartDate;
		private System.DateTime? dateEnd;
		private System.DateTime? dateStart;
		protected System.Web.UI.WebControls.Label lblPageCount;
		protected System.Web.UI.WebControls.Label lblSearchCount;
		private string orderId;
        private string allHereCode;
        private string storeName;
		protected Pager pager;
		protected System.Web.UI.WebControls.Repeater rp_orderStatistics;
		private string shipTo;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAllHereCode;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
		private string userName;
        protected System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;
		private void BindUserOrderStatistics()
		{
			OrderQuery userOrder = new OrderQuery
			{
				UserName = this.userName,
				ShipTo = this.shipTo,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				OrderId = this.orderId,
                AllHereCode = this.allHereCode,
                StoreName = this.storeName,
                OrderSource = 2,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortBy = "OrderDate",
				SortOrder = SortAction.Desc
			};
			OrderStatisticsInfo userOrders = SalesHelper.GetUserOrders(userOrder);
            this.pager.TotalRecords = userOrders.TotalCount;
			this.rp_orderStatistics.DataSource = userOrders.OrderTbl;
			this.rp_orderStatistics.DataBind();
			
			this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">佣金共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
			this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">佣金共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));
		
        }
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			OrderQuery userOrder = new OrderQuery
			{
				UserName = this.userName,
				ShipTo = this.shipTo,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				OrderId = this.orderId,
                AllHereCode = this.allHereCode,
                StoreName = this.storeName,
                OrderSource = 2,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortBy = "OrderDate",
				SortOrder = SortAction.Desc
			};
			OrderStatisticsInfo userOrdersNoPage = SalesHelper.GetUserOrdersNoPage(userOrder);
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			builder.AppendLine("<td>订单号</td>");
            builder.AppendLine("<td>门店DZ号</td>");
            builder.AppendLine("<td>门店名称</td>");
            builder.AppendLine("<td>商品名称</td>");
            builder.AppendLine("<td>商品型号</td>");
			builder.AppendLine("<td>下单时间</td>");
            builder.AppendLine("<td>昵称</td>");
            builder.AppendLine("<td>收货人</td>");
            builder.AppendLine("<td>电话</td>");
            builder.AppendLine("<td>地址</td>");
			builder.AppendLine("<td>总订单金额</td>");
            builder.AppendLine("<td>佣金</td>");
			builder.AppendLine("</tr>");
			foreach (System.Data.DataRow row in userOrdersNoPage.OrderTbl.Rows)
			{
				builder.AppendLine("<tr>");
				builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["accountALLHere"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["storeName"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["ItemDescription"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["SKU"].ToString() + "</td>");
				builder.AppendLine("<td>" + row["OrderDate"].ToString() + "</td>");
				builder.AppendLine("<td>" + row["UserName"].ToString() + "</td>");
				builder.AppendLine("<td>" + row["ShipTo"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["CellPhone"].ToString() + "</td>");//电话
                builder.AppendLine("<td>" + row["ShippingRegion"].ToString() + " " + row["Address"].ToString() + "</td>");//地址
                builder.AppendLine("<td>" + row["Total"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["CommTotal"].ToString() + "</td>");
				builder.AppendLine("</tr>");
			}
			builder.AppendLine("<tr>");
			builder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
			builder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
			builder.AppendLine("<td>订单佣金共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
			builder.AppendLine("<td></td>");
			builder.AppendLine("</tr>");
			builder.AppendLine("</table>");
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=供应商订单统计.xls");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/ms-excel";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(builder.ToString());
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
				{
					this.shipTo = base.Server.UrlDecode(this.Page.Request.QueryString["shipTo"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.orderId = Globals.UrlDecode(this.Page.Request.QueryString["orderId"]);
				}
                //添加的查询
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["allHereCode"]))
                {
                    this.allHereCode = Globals.UrlDecode(this.Page.Request.QueryString["allHereCode"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeName"]))
                {
                    this.storeName = Globals.UrlDecode(this.Page.Request.QueryString["storeName"]);
                }
				this.txtUserName.Text = this.userName;
                this.txtAllHereCode.Text = this.allHereCode;
                this.txtStoreName.Text = this.storeName;
				this.txtShipTo.Text = this.shipTo;
				this.calendarStartDate.SelectedDate = this.dateStart;
				this.calendarEndDate.SelectedDate = this.dateEnd;
				this.txtOrderId.Text = this.orderId;
			}
			else
			{
				this.userName = this.txtUserName.Text;
                this.allHereCode = this.txtAllHereCode.Text;
                this.storeName = this.txtStoreName.Text;
				this.shipTo = this.txtShipTo.Text;
				this.dateStart = this.calendarStartDate.SelectedDate;
				this.dateEnd = this.calendarEndDate.SelectedDate;
				this.orderId = this.txtOrderId.Text;
			}
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindUserOrderStatistics();
			}
		}
		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("userName", this.txtUserName.Text);
            queryStrings.Add("allHereCode", this.txtAllHereCode.Text);
            queryStrings.Add("storeName", this.txtStoreName.Text);
			queryStrings.Add("shipTo", this.txtShipTo.Text);
			queryStrings.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			queryStrings.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
			queryStrings.Add("orderId", this.txtOrderId.Text);
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}
		protected void rp_orderStatistics_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rp_orderitem");
			string orderId = ((System.Data.DataRowView)e.Item.DataItem)["OrderId"].ToString();
			if (orderId != "")
			{
				repeater.DataSource = OrderHelper.GetOrderInfo(orderId).LineItems.Values;
				repeater.DataBind();
			}
		}
	}
}
