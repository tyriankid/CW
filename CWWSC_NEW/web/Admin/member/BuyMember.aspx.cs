using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_BuyMember :AdminPage
{
    private string realName;
    private string searchKey;
    private int? rankId;
    private int? vipCard = null;
    protected void BindData()
    {
        MemberQuery query = new MemberQuery
        {
            Username = this.searchKey,
            Realname = this.realName,
            GradeId = this.rankId,
            OrderNumber=1,
            PageIndex = this.pager.PageIndex,
            SortBy = this.grdMemberList.SortOrderBy,
            PageSize = this.pager.PageSize
        };
        if (this.grdMemberList.SortOrder.ToLower() == "desc")
        {
            query.SortOrder = SortAction.Desc;
        }
        if (this.vipCard.HasValue && this.vipCard.Value != 0)
        {
            query.HasVipCard = new bool?(this.vipCard.Value == 1);
        }
        DbQueryResult members = MemberHelper.GetMembers(query);
        this.grdMemberList.DataSource = members.Data;
        this.grdMemberList.DataBind();
        this.pager.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
    }
    private void btnSearchButton_Click(object sender, System.EventArgs e)
    {
        this.ReBind(true);
    }
    private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        this.ReBind(false);
    }
    private void grdMemberList_ReBindData(object sender)
    {
        this.ReBind(false);
    }
    private void LoadParameters()
    {
        if (!this.Page.IsPostBack)
        {
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["rankId"], out result))
            {
                this.rankId = new int?(result);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
            {
                this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
            {
                this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
            }
            this.rankList.SelectedValue = this.rankId;
            this.txtSearchText.Text = this.searchKey;
            this.txtRealName.Text = this.realName;
        }
        else
        {
            this.rankId = this.rankList.SelectedValue;
            this.searchKey = this.txtSearchText.Text;
            this.realName = this.txtRealName.Text.Trim();
        }
    }
    protected override void OnInitComplete(System.EventArgs e)
    {
        base.OnInitComplete(e);
        this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
        this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);;
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.LoadParameters();
        if (!this.Page.IsPostBack)
        {
            this.rankList.DataBind();
            this.rankList.SelectedValue = this.rankId;
            this.BindData();
        }
        CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        if (this.rankList.SelectedValue.HasValue)
        {
            queryStrings.Add("rankId", this.rankList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("searchKey", this.txtSearchText.Text);
        queryStrings.Add("realName", this.txtRealName.Text);
        queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (!isSearch)
        {
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        base.ReloadPage(queryStrings);
    }
}