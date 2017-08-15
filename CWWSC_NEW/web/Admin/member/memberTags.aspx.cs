using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_memberTags :AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.grdmemberTags.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdmemberTags_RowCommand);
        if (!IsPostBack)
        {
            bind();
        }
    }
    protected void bind()
    {
        StringBuilder builder = new StringBuilder();
        MemberTagsQuery query = LoadParameters();

        builder.Append("1=1");
        if (!string.IsNullOrEmpty(query.keyword))
        {
            builder.AppendFormat(" and TagName like '%{0}%'", query.keyword);
        }
        if (!string.IsNullOrEmpty(query.tagstype))
        {
            builder.AppendFormat(" and TagType='{0}'", query.tagstype);

        }
        DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Tags", "TagID", builder.ToString(), "*");
        this.grdmemberTags.DataSource = dtmemberInfo.Data;
        this.grdmemberTags.DataBind();
        this.pager.TotalRecords = dtmemberInfo.TotalRecords;
        this.txtSearchText.Text = query.keyword;
        this.dropMemberTagsType.SelectedValue = query.tagstype;

    }
    public void btnSearchButton_Click(object sender, System.EventArgs e)
    {
        this.ReBind(true);
    }
    private MemberTagsQuery LoadParameters()
    {

        MemberTagsQuery query = new MemberTagsQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "Scode";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
        {
            query.keyword = base.Server.UrlDecode(this.Page.Request.QueryString["keyword"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tagstype"]))
        {
            query.tagstype = this.Page.Request.QueryString["tagstype"];
        }
        return query;
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        if (!string.IsNullOrEmpty(this.dropMemberTagsType.SelectedValue))
        {
            queryStrings.Add("tagstype", this.dropMemberTagsType.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("keyword", this.txtSearchText.Text);
        base.ReloadPage(queryStrings);
    }
     private void grdmemberTags_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {   
            if (memberTagsHelper.DeleteMemberTags(int.Parse(e.CommandArgument.ToString())))
            {
                this.ShowMsgAndReUrl("删除成功", true, "memberTags.aspx");
                return;
            }
            else
            {
                this.ShowMsg("删除失败", false);
                return;
            }
        }

    }
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnOrder_Click(object sender, System.EventArgs e)
    {
        foreach (System.Web.UI.WebControls.GridViewRow row in this.grdmemberTags.Rows)
        {
            int result = 0;
            System.Web.UI.WebControls.TextBox box = (System.Web.UI.WebControls.TextBox)row.FindControl("txtSort");
            if (int.TryParse(box.Text.Trim(), out result))
            {
                int TagID = (int)this.grdmemberTags.DataKeys[row.RowIndex].Value;
                if (memberTagsHelper.GetMemberTagsInfo(TagID).Scode != result.ToString())
                {
                    memberTagsHelper.updateSort(TagID, result);
                }
            }
        }
        this.ReBind(true);
    }
   
}