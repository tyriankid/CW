using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class ProductConsultations : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSearch;
		private int? categoryId;
		protected ProductCategoriesDropDownList dropCategories;
		protected Grid grdConsultation;
		protected PageSize hrefPageSize;
		private string keywords = string.Empty;
		protected Pager pager;
		protected Pager pager1;
		private string productCode;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		private void BindConsultation()
		{
            //2016-08-08验证当前登陆用户类型
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }

			ProductConsultationAndReplyQuery entity = new ProductConsultationAndReplyQuery
			{
				Keywords = this.keywords,
				CategoryId = this.categoryId,
				ProductCode = this.productCode,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
                SortBy = "ConsultationDate",
				Type = ConsultationReplyType.NoReply,
                DistributorsUserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : currentManager.ClientUserId.ToString()
			};
			Globals.EntityCoding(entity, true);
			DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(entity);
			this.grdConsultation.DataSource = consultationProducts.Data;
			this.grdConsultation.DataBind();
			this.pager.TotalRecords = consultationProducts.TotalRecords;
			this.pager1.TotalRecords = consultationProducts.TotalRecords;
		}

        //得到分公司下所有门店前端用户ID
        private string getStorIds(int fgsid)
        {
            string strStorId = string.Empty;
            DataTable dtDistributors = StoreInfoHelper.SelectStoreClientUserIdByFgsId(fgsid);
            foreach (DataRow dr in dtDistributors.Rows)
            {
                strStorId += dr["UserId"].ToString() + ",";
            }
            strStorId = strStorId.TrimEnd(',');//去除最后一个 , 符号
            if (string.IsNullOrEmpty(strStorId))
                strStorId = "null";
            return strStorId;
        }

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductConsultations(true);
		}
		private void grdConsultation_ReBindData(object sender)
		{
			this.BindConsultation();
		}
		private void grdConsultation_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int consultationId = (int)this.grdConsultation.DataKeys[e.RowIndex].Value;
			if (ProductCommentHelper.DeleteProductConsultation(consultationId) > 0)
			{
				this.ShowMsg("成功删除了选择的商品咨询", true);
				this.BindConsultation();
			}
			else
			{
				this.ShowMsg("删除商品咨询失败", false);
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.SetSearchControl();
			this.grdConsultation.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdConsultation_RowDeleting);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdConsultation.ReBindData += new Grid.ReBindDataEventHandler(this.grdConsultation_ReBindData);
		}
		private void ReloadProductConsultations(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
			queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
			queryStrings.Add("productCode", this.txtSKU.Text.Trim());
			if (!isSearch)
			{
				queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
			base.ReloadPage(queryStrings);
		}
		private void SetSearchControl()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
				{
					this.keywords = this.Page.Request.QueryString["Keywords"];
				}
				int result = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
				{
					this.categoryId = new int?(result);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = this.Page.Request.QueryString["productCode"];
				}
				this.txtSearchText.Text = this.keywords;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				this.BindConsultation();
			}
			else
			{
				this.keywords = this.txtSearchText.Text.Trim();
				this.productCode = this.txtSKU.Text.Trim();
				this.categoryId = this.dropCategories.SelectedValue;
			}
		}
	}
}
