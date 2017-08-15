using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Text;

public partial class Admin_distributor_StoreUserChangeApplyManage : AdminPage
{
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
        StoreUserChangeApplyQuery query = LoadParameters();

        builder.Append("1=1");
        if (!string.IsNullOrEmpty(query.StoreName))
        {
            builder.AppendFormat(" and StoreName like '%{0}%'", query.StoreName);
        }
        if (!string.IsNullOrEmpty(query.StartTime.ToString()))
        {
            builder.AppendFormat(" and AuditingDate >= '{0}'", query.StartTime);
        }
        if (!string.IsNullOrEmpty(query.EndTime.ToString()))
        {
            builder.AppendFormat(" and AuditingDate <= '{0}'", query.EndTime);
        }
        if (!string.IsNullOrEmpty(query.ApplyState))
        {
            builder.AppendFormat(" and ApplyState={0}", query.ApplyState);
        }
        DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_StoreUserChangeApply sa left join aspnet_distributors ad on sa.StoreUserId = ad.userid left join aspnet_members am on am.userid = sa.applyuserid", "ID", builder.ToString(), "sa.*,ad.storename,am.username");
        this.grdApplyList.DataSource = dtmemberInfo.Data;
        this.grdApplyList.DataBind();
        this.pager.TotalRecords = dtmemberInfo.TotalRecords;
        this.txtSearchText.Text = query.StoreName;
        this.states.SelectedValue = query.ApplyState;
        this.calendarStartDate.SelectedDate = query.StartTime;
        this.calendarEndDate.SelectedDate = query.EndTime;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ReBind(true);
    }
    private StoreUserChangeApplyQuery LoadParameters()
    {
        StoreUserChangeApplyQuery query = new StoreUserChangeApplyQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "AuditingDate";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
        {
            query.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ApplyState"]))
        {
            query.ApplyState = this.Page.Request.QueryString["ApplyState"];
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
        {
            query.StartTime = this.Page.Request.QueryString["StartTime"].ToDateTime();
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
        {
            query.EndTime = this.Page.Request.QueryString["EndTime"].ToDateTime();
        }
        return query;
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        if (!string.IsNullOrEmpty(this.states.SelectedValue))
        {
            queryStrings.Add("ApplyState", this.states.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
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

}