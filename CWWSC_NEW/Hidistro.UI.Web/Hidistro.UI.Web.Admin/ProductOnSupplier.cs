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
    public class ProductOnSupplier : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnCancelFreeShip;
		protected ImageLinkButton btnDelete;
		protected System.Web.UI.WebControls.HyperLink btnDownTaobao;
		protected System.Web.UI.WebControls.Button btnInStock;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.Button btnSetFreeShip;
		protected System.Web.UI.WebControls.Button btnUnSale;
		protected System.Web.UI.WebControls.Button btnUpdateProductTags;
		protected System.Web.UI.WebControls.Button btnUpSale;
        protected ImageLinkButton btnRemove;
		protected WebCalendar calendarEndDate;
		protected WebCalendar calendarStartDate;
		private int? categoryId;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductCategoriesDropDownList dropCategories;
		protected SaleStatusDropDownList dropSaleStatus;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
        protected ProductSourceDownList dropSource;
		private System.DateTime? endDate;
		protected Grid grdProducts;
		protected PageSize hrefPageSize;
		public ProductTagsLiteral litralProductTag;
		protected Pager pager;
		protected Pager pager1;
		private string productCode;
		private string productName;
		private ProductSaleStatus saleStatus = ProductSaleStatus.All;
		private System.DateTime? startDate;
		private int? tagId;
		protected TrimTextBox txtProductTag;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		private int? typeId;
        private int? sourceId;
        protected string LocalUrl = string.Empty;
        protected HiddenField txtProductIds;//隐藏域,传递的商品ID集合
        protected HiddenField hidManage;//是否管理员

		private void BindProducts()
		{
            //2016-07-14验证当前登陆用户类型
            bool isSupplier = false;//是否供应商
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.SupplierRoleId)
            {
                saleStatus = ProductSaleStatus.All;
                this.dropSaleStatus.afreshBind(true);
                this.dropSource.SelectedValue = currentManager.ClientUserId;//ClientUserId值为供应商Id
                this.dropSource.Enabled = false;
                isSupplier = true;
            }
            else
            {
                //如果为创维管理员则加载已审核的商品
                //saleStatus = ProductSaleStatus.OnSale;
                saleStatus = ProductSaleStatus.All;
                //出售状态无法更改
                //this.dropSaleStatus.Enabled = false;
                this.hidManage.Value = "true";
            }

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
                ProductSource = 2,//商品来源，1为创维、2为供应商  这里如果是供应商则显示供应商商品，如果是平台管理员则显示创维与供应商的商品
                SupplierId = isSupplier ? currentManager.ClientUserId : 0
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
            if (entity.SourceId != null)
                this.dropSource.SelectedValue = entity.SourceId;
			this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
		}
		private void btnCancelFreeShip_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要删除的商品", false);
			}
			else
			{
				if (ProductHelper.RemoveProduct(str) > 0)
				{
					this.ShowMsg("成功删除了选择的商品", true);
					this.BindProducts();
				}
				else
				{
					this.ShowMsg("删除商品失败，未知错误", false);
				}
			}
		}
        //清除商品区域关系
        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            //构建条件 
            if (string.IsNullOrEmpty(this.txtProductIds.Value))
                return;//商品ID集合为空则结束
            string strSqlwhere =  string.Format(@"ProductID in ({0})", this.txtProductIds.Value);
            //得到商品区域关系表结构
            DataTable dtProductRegion = DataBaseHelper.GetDataTable("Erp_ProductRegion", strSqlwhere, null);

            //循环删除所选商品区域关系
            foreach (DataRow dr in dtProductRegion.Rows)
            {
                dr.Delete();
            }
            //将表提交到数据库
            string str = string.Empty;//定义回调提示语变量
            if (DataBaseHelper.CommitDataTable(dtProductRegion, "SELECT * from Erp_ProductRegion") != -1)
                str = string.Format("ShowMsg(\"{0}\", {1})", "清除商品区域关系生效。", "true");//成功
            else
                str = string.Format("ShowMsg(\"{0}\", {1})", "清除商品区域关系失败！", "false");//失败
            //前端（客户端）弹出提示
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
        }
		private void btnInStock_Click(object sender, System.EventArgs e)
		{
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要入库的商品", false);
			}
			else
			{
				if (ProductHelper.InStock(str) > 0)
				{
					this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
					this.BindProducts();
				}
				else
				{
					this.ShowMsg("入库商品失败，未知错误", false);
				}
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void btnSetFreeShip_Click(object sender, System.EventArgs e)
		{
			bool isFree = ((System.Web.UI.WebControls.Button)sender).ID == "btnSetFreeShip";
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要设置为包邮的商品", false);
			}
			else
			{
				if (ProductHelper.SetFreeShip(str, isFree) > 0)
				{
					this.ShowMsg("成功" + (isFree ? "设置" : "取消") + "了商品包邮状态", true);
					this.BindProducts();
				}
				else
				{
					this.ShowMsg((isFree ? "设置" : "取消") + "商品包邮状态失败，未知错误", false);
				}
			}
		}
		private void btnUnSale_Click(object sender, System.EventArgs e)
		{
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要下架的商品", false);
			}
			else
			{
				if (ProductHelper.OffShelf(str) > 0)
				{
					this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
					this.BindProducts();
				}
				else
				{
					this.ShowMsg("下架商品失败，未知错误", false);
				}
			}
		}
		private void btnUpdateProductTags_Click(object sender, System.EventArgs e)
		{
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要关联的商品", false);
			}
			else
			{
				System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
				if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
				{
					string str2 = this.txtProductTag.Text.Trim();
					string[] strArray;
					if (str2.Contains(","))
					{
						strArray = str2.Split(new char[]
						{
							','
						});
					}
					else
					{
						strArray = new string[]
						{
							str2
						};
					}
					string[] array = strArray;
					for (int j = 0; j < array.Length; j++)
					{
						string str3 = array[j];
						list.Add(System.Convert.ToInt32(str3));
					}
				}
				string[] strArray2;
				if (str.Contains(","))
				{
					strArray2 = str.Split(new char[]
					{
						','
					});
				}
				else
				{
					strArray2 = new string[]
					{
						str
					};
				}
				int num = 0;
				string[] strArray3 = strArray2;
				for (int i = 0; i < strArray3.Length; i++)
				{
					string text = strArray3[i];
				}
				if (num > 0)
				{
					this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
				}
				else
				{
					this.ShowMsg("已成功取消了商品的关联商品标签", true);
				}
				this.txtProductTag.Text = "";
			}
		}
		private void btnUpSale_Click(object sender, System.EventArgs e)
		{
			string str = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(str))
			{
				this.ShowMsg("请先选择要上架的商品", false);
			}
			else
			{
				if (ProductHelper.UpShelf(str) > 0)
				{
					this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
					this.BindProducts();
				}
				else
				{
					this.ShowMsg("上架商品失败，未知错误", false);
				}
			}
		}
		private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
                System.Web.UI.WebControls.HiddenField hidd = (System.Web.UI.WebControls.HiddenField)e.Row.FindControl("hidProductId");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
                System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litRegionName");

                switch (literal.Text)
                { 
                    case "1":
                        literal.Text = "<span style='color:green'>出售中</span>";
                        break;
                    case "2":
                        literal.Text = "下架区";
                        break;
                    case "3":
                        literal.Text = "仓库中";
                        break;
                    case "-2":
                        literal.Text = "<span style='color:red'><a href=\"javascript:void(0);\" onclick=\"javascript:ShouBackAccount('" + hidd.Value + "')\">未通过</a></span>";
                        break;
                    case "-1":
                        literal.Text = "<span style='color:orange'>待审核</span>";
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
            if (this.sourceId != null)
                this.dropSource.SelectedValue = this.sourceId;
			this.dropSaleStatus.SelectedValue = this.saleStatus;
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			this.btnUpSale.Click += new System.EventHandler(this.btnUpSale_Click);
			this.btnUnSale.Click += new System.EventHandler(this.btnUnSale_Click);
			this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
			this.btnCancelFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
			this.btnSetFreeShip.Click += new System.EventHandler(this.btnSetFreeShip_Click);
			this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.dropSaleStatus.SelectedIndexChanged += new System.EventHandler(this.dropSaleStatus_SelectedIndexChanged);
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
				this.litralProductTag.DataBind();
				this.dropSaleStatus.DataBind();
				this.btnDownTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", SettingsManager.GetMasterSettings(true).SiteUrl, Globals.ApplicationPath);
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
			queryStrings.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
			base.ReloadPage(queryStrings);
		}
	}
}
