using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{

    public class ListFiliale : AdminPage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();

            string action = Request["action"];
            switch (action)
            {
                case "judge"://判断点击打开的链接
                    judge();
                    break;
            }

            if (!this.Page.IsPostBack)
            {
                this.BindTypes();
            }
        }
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected Grid grdProductTypes;
        protected Pager pager;
        private string searchkey;
        protected System.Web.UI.WebControls.TextBox txtSearchText;

        private void judge()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            ManagerQuery query = new ManagerQuery
            {
                //分公司角色权限ID
                RoleId = masterSettings.FilialeRoleId,
                //分公司ID
                ClientUserId = Convert.ToInt32(Request["fgsid"])
            };
            DataTable dtmg = ManagerHelper.GetManagersbyClientUserId(query);
            if (dtmg.Rows.Count > 0)
            {
                int userid = Convert.ToInt32(dtmg.Rows[0]["UserId"]);
                Response.Write("/admin/store/EditManager.aspx?fgsid=" + Request["fgsid"] + "&userid=" + userid);
                Response.End();
            }
            else
            {
                Response.Write("/admin/store/AddManager.aspx?fgsid=" + Request["fgsid"]);
                Response.End();
            }
        }

        /// <summary>
        /// 绑定分公司列表
        /// </summary>
        private void BindTypes()
        {
            ListFilialeQuery quer = new ListFilialeQuery
            {
                fgsName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            };

            DbQueryResult ListFiliale = FilialeHelper.GetListFiliale(quer);

            this.grdProductTypes.DataSource = ListFiliale.Data;
            this.grdProductTypes.DataBind();
            this.pager.TotalRecords = ListFiliale.TotalRecords;
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("pageSize", "10");
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdProductTypes.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProductTypes_RowDeleting);
        }
        private void grdProductTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int Id = (int)this.grdProductTypes.DataKeys[e.RowIndex].Value;
            string strWhere = string.Format(" fgsid = '{0}'",Id);
            DataTable dtStoreInfo = StoreInfoHelper.SelectStoreInfoByWhere(strWhere);
            if (dtStoreInfo.Rows.Count == 0)
            {
                if (FilialeHelper.DeleteFiliale(Id))
                {
                    this.BindTypes();
                    this.ShowMsg("成功删除了一个分公司信息", true);
                }
                else
                {
                    this.ShowMsg("删除分公司信息失败", false);
                }
            }
            else
                this.ShowMsg("该公司存在下属门店，无法直接删除。", false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                this.txtSearchText.Text = this.searchkey;
            }
            else
            {
                this.searchkey = this.txtSearchText.Text.Trim();
            }
        }


    }
}
