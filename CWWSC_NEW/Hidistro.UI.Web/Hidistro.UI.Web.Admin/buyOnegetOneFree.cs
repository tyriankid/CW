namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public class buyOnegetOneFree : AdminPage
    {
        protected Button btnSearch;
        protected TextBox txtSearchText;
        protected Grid grdReservePrice;
        protected Pager pager;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdReservePrice.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdReservePrice_RowDeleting);
            if (!IsPostBack)
            {
                bind();
            }
        }
        protected void bind()
        {
            StringBuilder builder = new StringBuilder();
            BuyOneGetOnrFreeQuery query = LoadParameters();

            builder.Append("1=1");
            if (!string.IsNullOrEmpty(query.productName))
            {
                builder.AppendFormat(" and pd.ProductName  like '%{0}%' or p.ProductName like  '%{0}%'", query.productName);
            }

            DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_BuyOneGetOneFree b left join Hishop_Products pd on b.productId=pd.ProductId left join  Hishop_Products p on b.GetProductId=p.ProductId", "buyoneId", builder.ToString(), "pd.ProductName,pd.ImageUrl1,p.ProductName as GetProductName,b.*");
            this.grdReservePrice.DataSource = dtmemberInfo.Data;
            this.grdReservePrice.DataBind();
            this.pager.TotalRecords = dtmemberInfo.TotalRecords;
            this.txtSearchText.Text = query.productName;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }
        private BuyOneGetOnrFreeQuery LoadParameters()
        {
            BuyOneGetOnrFreeQuery query = new BuyOneGetOnrFreeQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "buyoneId";
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                query.productName = base.Server.UrlDecode(this.Page.Request.QueryString["productName"]);
            }

            return query;
        }
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();

            queryStrings.Add("productName", this.txtSearchText.Text);
            base.ReloadPage(queryStrings);
        }
        private void grdReservePrice_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string Id = this.grdReservePrice.DataKeys[e.RowIndex].Value.ToString();

            if (BuyOneGetOnrFreeHelper.DeleteBuyOneGetOne(int.Parse(Id)))
            {
                this.bind();
                this.ShowMsg("成功删除了一个价格预约项", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }
    }
}

