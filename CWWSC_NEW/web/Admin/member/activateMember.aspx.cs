using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_activateMember : AdminPage
{
    private void BindData()
    {
        ListMembersQuery memberQuery = (ListMembersQuery)this.GetMemberQuery();
        DbQueryResult members = CWMembersHelper.GetActivateMember(memberQuery);
        this.grdMember.DataSource = members.Data;
        this.grdMember.DataBind();
        this.txtSearchText.Text = memberQuery.UserName;
        this.pager1.TotalRecords = members.TotalRecords;
    }
    private void btnSearchButton_Click(object sender, System.EventArgs e)
    {
        this.ReloadMemberLogs(true);
    }
    private ListMembersQuery GetMemberQuery()
    {
        ListMembersQuery query = new ListMembersQuery();
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
        {
            query.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
        }
        query.PageSize = this.pager1.PageSize;
        query.PageIndex = this.pager1.PageIndex;
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
        {
            query.SortBy = this.Page.Request.QueryString["SortBy"];
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
        {
            query.SortOrder = SortAction.Desc;
        }
        return query;
    }
    private void grdMember_ReBindData(object sender)
    {
        this.ReloadMemberLogs(false);
    }
    private void grdMember_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int userId = (int)this.grdMember.DataKeys[e.RowIndex].Value;
        CWMembersHelper.DeleteMembers(userId);
        this.BindData();
        this.ShowMsg("成功删除了一个会员", true);
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.grdMember.ReBindData += new Grid.ReBindDataEventHandler(this.grdMember_ReBindData);
        this.grdMember.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMember_RowDeleting);
        this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
        if (!this.Page.IsPostBack)
        {
            this.BindData();
        }
    }
    private void ReloadMemberLogs(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("UserName", System.Convert.ToString(this.txtSearchText.Text));
        if (!isSearch)
        {
            queryStrings.Add("PageIndex", this.pager1.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("SortBy", this.grdMember.SortOrderBy);
        queryStrings.Add("SortOrder", SortAction.Desc.ToString());
        base.ReloadPage(queryStrings);
    }
}