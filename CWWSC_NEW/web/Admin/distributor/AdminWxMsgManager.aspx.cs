using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

public partial class Admin_distributor_AdminWxMsgManager : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.grdAdminWxMsgInfo.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdAdminWxMsgInfo_RowDeleting);

        if (!IsPostBack)
        {
            bind();
        }
    }
    protected void bind()
    {
        StringBuilder builder = new StringBuilder();
        AdminWxMsgInfoQuery query = LoadParameters();

        builder.Append("1=1");
        if (!string.IsNullOrEmpty(query.StoreName))
        {
            builder.AppendFormat(" and ad.StoreName like '%{0}%'", query.StoreName);
        }
        if (!string.IsNullOrEmpty(query.StartTime.ToString()))
        {
            builder.AppendFormat(" and awm.CreateTime >= '{0}'", query.StartTime);
        }
        if (!string.IsNullOrEmpty(query.EndTime.ToString()))
        {
            builder.AppendFormat(" and awm.CreateTime <= '{0}'", query.EndTime);
        }
        DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_AdminWxMsgInfo AWM left join aspnet_members AM on AWM.JSUserId = AM.userid left join aspnet_distributors ad on ad.userid = am.userid", "ID", builder.ToString(), "AWM.*,am.username,ad.storename");
        this.grdAdminWxMsgInfo.DataSource = dtmemberInfo.Data;
        this.grdAdminWxMsgInfo.DataBind();
        this.pager.TotalRecords = dtmemberInfo.TotalRecords;
        this.txtSearchText.Text = query.StoreName;
        this.calendarStartDate.SelectedDate = query.StartTime;
        this.calendarEndDate.SelectedDate = query.EndTime;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ReBind(true);
    }
    private AdminWxMsgInfoQuery LoadParameters()
    {
        AdminWxMsgInfoQuery query = new AdminWxMsgInfoQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "awm.CreateTime";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
        {
            query.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
        {
            query.StartTime = new System.DateTime?(DateTime.Parse(this.Page.Request.QueryString["StartTime"]));
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
        {
            query.EndTime = DateTime.Parse(this.Page.Request.QueryString["EndTime"]);
        }

        return query;
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        if (!string.IsNullOrWhiteSpace(this.txtSearchText.Text))
        {
            queryStrings.Add("StoreName", this.txtSearchText.Text);
        }
       
        if (this.calendarStartDate.SelectedDate.HasValue)
        {
            queryStrings.Add("StartTime", this.calendarStartDate.SelectedDate.Value.ToString());
        }
        if (this.calendarEndDate.SelectedDate.HasValue)
        {
            queryStrings.Add("EndTime", this.calendarEndDate.SelectedDate.Value.ToString());
        }
        
        base.ReloadPage(queryStrings);
    }

    private void grdAdminWxMsgInfo_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Guid id = new Guid(this.grdAdminWxMsgInfo.DataKeys[e.RowIndex].Value.ToString());
        AdminWxMsgInfoHelper.DeleteAdminWxMsgInfo(id);
        this.bind();
        this.ShowMsg("成功删除了一个记录", true);
    }

}