using ASPNET.WebControls;
using ControlPanel.Commodities;
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
    public class ListStoreInfo : AdminPage
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindTypes();
            }
        }
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button ExportFgs;
        protected Grid grdProductTypes;
        protected Pager pager;
        private string searchkey;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        /// <summary>
        /// 绑定分公司列表
        /// </summary>
        private void BindTypes()
        {
            ListStoreInfoQuery query = new ListStoreInfoQuery
            {
                storeName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "id",
                SortOrder = Core.Enums.SortAction.Asc,
            };
            //DataTable dt = FilialeHelper.GetAllFiliale this.searchkey
            DbQueryResult ListStore = StoreInfoHelper.ListStoreInfo(query);

            this.grdProductTypes.DataSource = ListStore.Data;
            this.grdProductTypes.DataBind();
            this.pager.TotalRecords = ListStore.TotalRecords;
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        /// <summary>
        /// 导出所有分公司信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportFgs_Click(object sender, System.EventArgs e)
        {
            //获取所有门店信息
            DataTable dt = StoreInfoHelper.GetAllStoreInfo();

            DataTable exportdata = new DataTable();
            exportdata.Columns.Add("所属分公司");
            exportdata.Columns.Add("门店名称");
            exportdata.Columns.Add("门店负责人");
            exportdata.Columns.Add("联系电话");
            exportdata.Columns.Add("金力账号");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                exportdata.Rows.Add(
                    dt.Rows[i]["fgsName"].ToString(),
                    dt.Rows[i]["StoreName"].ToString(),
                    dt.Rows[i]["storeRelationPerson"],
                    dt.Rows[i]["storeRelationCell"].ToString(),
                    dt.Rows[i]["accountAllHere"].ToString()
                    );
            }

            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            fields.Add("所属分公司");
            list2.Add("所属分公司");
            fields.Add("门店名称");
            list2.Add("门店名称");
            fields.Add("门店负责人");
            list2.Add("门店负责人");
            fields.Add("联系电话");
            list2.Add("联系电话");
            fields.Add("金力账号");
            list2.Add("金力账号");
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
            foreach (System.Data.DataRow row in exportdata.Rows)
            {
                foreach (string str2 in fields)
                {
                    builder.Append(row[str2]).Append(",");
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
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=门店信息.csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
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
            this.ExportFgs.Click += new System.EventHandler(this.ExportFgs_Click);
            this.grdProductTypes.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProductTypes_RowDeleting);
            this.grdProductTypes.RowCommand += new GridViewCommandEventHandler(this.grdProductTypes_RowCommand);
        }

        protected void grdProductTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Auditing")
            {
                int id = 0;
                if (int.TryParse(e.CommandArgument.ToString(), out id))
                {
                    StoreInfo storeinfo = StoreInfoHelper.GetStoreInfo(id);
                    if (!string.IsNullOrEmpty(storeinfo.storekeyid))
                    {
                        if (storeinfo.Auditing != 1)
                        {
                            storeinfo.Auditing = 1;
                            if (StoreInfoHelper.UpdateStoreInfo(storeinfo))
                            {
                                this.BindTypes();
                                this.ShowMsg("审核通过。", true);
                            }
                            else
                                this.ShowMsg("审核未通过，修改出现错误。", false);
                        }
                        else
                            this.ShowMsg("该门店已经审核，请不要重复审核。", false);
                    }
                    else
                        this.ShowMsg("审核未通过，因为该门店未进过金力同步没有同步编码。", false);
                }
            }
        }

        private void grdProductTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int Id = (int)this.grdProductTypes.DataKeys[e.RowIndex].Value;

            if (StoreInfoHelper.DeleteStoreInfo(Id))
            {
                this.BindTypes();
                this.ShowMsg("成功删除了一个创维门店信息", true);
            }
            else
            {
                this.ShowMsg("删除创维门店信息失败", false);
            }
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
