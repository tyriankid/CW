using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Products)]
    public class ProductOnAuditing : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnCancelFreeShip;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected WebCalendar calendarEndDate;
		protected WebCalendar calendarStartDate;
		private int? categoryId;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
        protected ProductSourceDownList dropSource;
		private System.DateTime? endDate;
		protected Grid grdProducts;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Pager pager1;
		private string productCode;
		private string productName;
		private ProductSaleStatus saleStatus = ProductSaleStatus.OnAuditing;
		private System.DateTime? startDate;
		private int? tagId;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		private int? typeId;
        private int? sourceId;
        protected string LocalUrl = string.Empty;
        protected HiddenField txtProductIds;//隐藏域,传递的商品ID集合

		private void BindProducts()
		{
            //加载参数
			this.LoadParameters();
            //查询条件
            ProductQuery entity = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.typeId,
                SourceId = this.sourceId,
                SaleStatus = this.saleStatus,
                EndDate = this.endDate,
                ProductSource = 2,//商品来源，1为创维、2为供应商，只有供应商的上传的商品才会审核
                SupplierId = 0 //审核页面与商品所属供应商无关
            };
			if (this.categoryId.HasValue && this.categoryId > 0)
			{
				entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			Globals.EntityCoding(entity, true);
			DbQueryResult products = ProductHelper.GetProducts(entity);
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
			this.txtSearchText.Text = entity.Keywords;
			this.txtSKU.Text = entity.ProductCode;
			this.dropCategories.SelectedValue = entity.CategoryId;
			this.dropType.SelectedValue = entity.TypeId;
            this.dropSource.SelectedValue = entity.SourceId;
			this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
                System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litRegionName");

                switch (literal.Text)
                { 
                    case "1":
                        literal.Text = "出售中";
                        break;
                    case "2":
                        literal.Text = "下架区";
                        break;
                    case "3":
                        literal.Text = "仓库中";
                        break;
                    case "-2":
                        literal.Text = "未通过";
                        break;
                    case "-1":
                        literal.Text = "待审核";
                        break;
                }
				if (string.IsNullOrEmpty(literal2.Text))
				{
					literal2.Text = "-";
				}

			}
		}
		private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (ProductHelper.RemoveProduct(this.grdProducts.DataKeys[e.RowIndex].Value.ToString()) > 0)
			{
				this.ShowMsg("删除商品成功", true);
				this.ReloadProductOnSales(false);
			}
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int result = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
			{
				this.categoryId = new int?(result);
			}
			int num2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out num2))
			{
				this.dropBrandList.SelectedValue = new int?(num2);
			}
			int num3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out num3))
			{
				this.tagId = new int?(num3);
			}
			int num4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out num4))
			{
				this.typeId = new int?(num4);
			}
            int num5 = 0;
            if (int.TryParse(this.Page.Request.QueryString["sourceId"], out num5))
            {
                this.sourceId = new int?(num5);
            }
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			this.txtSearchText.Text = this.productName;
			this.txtSKU.Text = this.productCode;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropTagList.DataBind();
			this.dropTagList.SelectedValue = this.tagId;
			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
			this.dropType.SelectedValue = this.typeId;
            this.dropSource.SelectedValue = this.sourceId;
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
                DataTable dtProductRegion = ManagerHelper.GetProductRegion(string.Empty);
                ViewState["dtProductRegion"] = dtProductRegion;//存储到全局值

				this.dropCategories.IsUnclassified = true;
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
                this.dropSource.DataBind();
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			queryStrings.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
			queryStrings.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			if (this.dropTagList.SelectedValue.HasValue)
			{
				queryStrings.Add("tagId", this.dropTagList.SelectedValue.ToString());
			}
			if (this.dropType.SelectedValue.HasValue)
			{
				queryStrings.Add("typeId", this.dropType.SelectedValue.ToString());
			}
            if (this.dropSource.SelectedValue.HasValue)
            {
                queryStrings.Add("sourceId", this.dropSource.SelectedValue.ToString());
            }
			base.ReloadPage(queryStrings);
		}
	}
}
