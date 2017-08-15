using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using Hidistro.Core.Entities;
using Hidistro.Core;
using Hidistro.Core.Enums;
using System.Collections.Specialized;



namespace Hidistro.UI.Web.Admin.product
{
    public partial class ProductReservePrice : AdminPage
    {
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
            ProductMaintenanceReminderQuery query = LoadParameters();

            builder.Append("1=1");
            if (!string.IsNullOrEmpty(query.productName))
            {
                builder.AppendFormat(" and ProductName like '%{0}%'", query.productName);
            }

            DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_ProductReservePrice pr left join Hishop_Products pd on pr.ProductId=pd.ProductId left join Hishop_SKUs sk on pr.SkuId=sk.SkuId ", "ReserveId", builder.ToString(), "sk.SKU,pd.ProductName,pd.ImageUrl1,pr.*");
            this.grdReservePrice.DataSource = dtmemberInfo.Data;
            this.grdReservePrice.DataBind();
            this.pager.TotalRecords = dtmemberInfo.TotalRecords;
            this.txtSearchText.Text = query.productName;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }
        private ProductMaintenanceReminderQuery LoadParameters()
        {
            ProductMaintenanceReminderQuery query = new ProductMaintenanceReminderQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "ReserveId";
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

            if (ProductReservePriceHelper.DeleteProductReservePrice(int.Parse(Id)))
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

