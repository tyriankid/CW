using ControlPanel.Commodities;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_O2Osubsidiary : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.grdSubsidiary.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMember_RowDeleting);
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        CW_O2OSubsidiaryQuery SubsidiaryQuery = (CW_O2OSubsidiaryQuery)this.GetSubsidiaryQuery();
        DbQueryResult listInfo = O2OSubsidiaryHelper.GetListO2OSubsidiary(SubsidiaryQuery);
        this.grdSubsidiary.DataSource = listInfo.Data;
        this.grdSubsidiary.DataBind();
        this.txtSearchText.Text = SubsidiaryQuery.ColName;
        this.pager1.TotalRecords = listInfo.TotalRecords;
        this.pager1.TotalRecords = listInfo.TotalRecords;
    }
    private CW_O2OSubsidiaryQuery GetSubsidiaryQuery()
    {
        CW_O2OSubsidiaryQuery query = new CW_O2OSubsidiaryQuery();
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ColName"]))
        {
            query.ColName = base.Server.UrlDecode(this.Page.Request.QueryString["ColName"]);
        }
        query.PageSize = this.pager1.PageSize;
        query.PageIndex = this.pager1.PageIndex;
        query.SortBy = "type";
        query.SortOrder = SortAction.Asc;
        return query;
    }
    private void grdMember_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int userId = (int)this.grdSubsidiary.DataKeys[e.RowIndex].Value;
        O2OSubsidiaryHelper.DeleteO2OSubsidiary(userId);
        this.BindData();
        this.ShowMsg("成功删除了一个会员", true);
    }
    private void ReloadListLogs(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("ColName", System.Convert.ToString(this.txtSearchText.Text));
        if (!isSearch)
        {
            queryStrings.Add("PageIndex", this.pager1.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("SortBy", this.grdSubsidiary.SortOrderBy);
        queryStrings.Add("SortOrder", SortAction.Desc.ToString());
        base.ReloadPage(queryStrings);
    }
    private void grdMember_ReBindData(object sender)
    {
        this.ReloadListLogs(false);
    }
    protected void btnSearchButton_Click(object sender, EventArgs e)
    {
        this.ReloadListLogs(true);
    }
}