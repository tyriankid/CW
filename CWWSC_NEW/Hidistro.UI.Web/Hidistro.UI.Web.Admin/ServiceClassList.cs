using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
    public class ServiceClassList : AdminPage
	{
		protected System.Web.UI.WebControls.LinkButton btnorder;
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdServiceClassList;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		private void BindServiceClass()
		{
            this.grdServiceClassList.DataSource = ServiceClassHelper.SelectClassByWhere("1=1 order by Scode");
            this.grdServiceClassList.DataBind();
		}
		protected void btnorder_Click(object sender, System.EventArgs e)
		{
			try
			{
				bool flag = true;
                for (int i = 0; i < this.grdServiceClassList.Rows.Count; i++)
				{
                    int ScID = (int)this.grdServiceClassList.DataKeys[i].Value;
                    int Scode = int.Parse((this.grdServiceClassList.Rows[i].Cells[2].Controls[1] as System.Web.UI.HtmlControls.HtmlInputText).Value);
                    if (!ServiceClassHelper.UpdateClassScode(ScID, Scode))
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.ShowMsg("批量更新排序成功！", true);
                    this.BindServiceClass();
				}
				else
				{
					this.ShowMsg("批量更新排序失败！", false);
				}
			}
			catch (System.Exception exception)
			{
				this.ShowMsg("批量更新排序失败！" + exception.Message, false);
			}
		}
		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
            string where = string.IsNullOrEmpty(this.txtSearchText.Text.Trim()) ? string.Empty : string.Format("ClassName like '%{0}%'", DataHelper.CleanSearchString(this.txtSearchText.Text.Trim()));
            this.grdServiceClassList.DataSource = ServiceClassHelper.SelectClassByWhere(where);
            this.grdServiceClassList.DataBind();
		}
        //protected void grdServiceClassList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        //{
        //    int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
        //    int brandId = (int)this.grdServiceClassList.DataKeys[rowIndex].Value;
        //    if (e.CommandName == "Rise")
        //    {
        //        if (rowIndex != this.grdServiceClassList.Rows.Count)
        //        {
        //            CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Asc);
        //            this.BindServiceClass();
        //        }
        //    }
        //    else
        //    {
        //        if (e.CommandName == "Fall")
        //        {
        //            CatalogHelper.UpdateBrandCategorieDisplaySequence(brandId, SortAction.Desc);
        //            this.BindServiceClass();
        //        }
        //    }
        //}
        protected void grdServiceClassList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
            int ScID = (int)this.grdServiceClassList.DataKeys[e.RowIndex].Value;
            if (DistributorClassHelper.SelectClassByWhere(string.Format("ScIDs like '%{0},%'", ScID)).Rows.Count > 0)
            {
                this.ShowMsg("选择的服务品类已经有门店申请服务门店，删除失败", false);
            }
            else
            {
                if (ServiceClassHelper.DeleteServiceClass(ScID))
                {
                    this.ShowMsg("删除服务品类成功。", true);
                }
                else
                {
                    this.ShowMsg("删除服务品类失败。", false);
                }
                this.BindServiceClass();
            }
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
            this.grdServiceClassList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdServiceClassList_RowDeleting);
            //this.grdServiceClassList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdServiceClassList_RowCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnorder.Click += new System.EventHandler(this.btnorder_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
                this.BindServiceClass();
			}
		}
	}
}
