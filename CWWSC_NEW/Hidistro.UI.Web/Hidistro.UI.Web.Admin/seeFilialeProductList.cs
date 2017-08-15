using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Products)]
    public class seeFilialeProductList : AdminPage
    {
        protected Grid grdsFilialeProductList;
        protected Pager pager;
        protected TextBox txtSearchText;
        protected RegionSelector dropRegion;
        protected Button btnSearch;
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }
       
        protected void bind()
        {
            StringBuilder builder = new StringBuilder();
            ProductQuery query = LoadParameters();
            builder.Append("1=1");
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" and PD.FilialeId>0 and productCode='{0}'", query.ProductCode);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" and fgsName like '%{0}%'", query.Keywords);
            }
            string str = string.Format("vw_Hishop_BrowseProductList PD  left join CW_Filiale CF on PD.FilialeId=CF.id " );
            DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, str, "ProductId", builder.ToString(), "ProductId,fgsName,ProductName,(SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = PD.SkuId) AS  CostPrice,ProductCode,Stock,SalePrice,MarketPrice,SaleStatus,RegionName,RegionId,ThumbnailUrl40");
            this.grdsFilialeProductList.DataSource = dtmemberInfo.Data;
            this.grdsFilialeProductList.DataBind();
            this.pager.TotalRecords = dtmemberInfo.TotalRecords;
            this.txtSearchText.Text = query.Keywords;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }
        private ProductQuery LoadParameters()
        {
            ProductQuery query = new ProductQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "ProductId";
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
            {
                query.Keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductCode"]))
            {
                query.ProductCode = base.Server.UrlDecode(this.Page.Request.QueryString["ProductCode"]);
            }
            return query;
        }
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.txtSearchText.Text))
            {
                queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            }
            queryStrings.Add("ProductCode",this.Page.Request.QueryString["ProductCode"]);
            base.ReloadPage(queryStrings);
        }
    }
}
