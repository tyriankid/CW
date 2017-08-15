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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_distributor_distributorServiceAuditing :AdminPage
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
        ListDistributorClassQuery query = LoadParameters();

        builder.Append("1=1");
        if (!string.IsNullOrEmpty(query.keyword))
        {
            builder.AppendFormat(" and StoreName like '%{0}%'", query.keyword);
        }
        if (!string.IsNullOrEmpty(query.RealName))
        {
            builder.AppendFormat(" and RealName like '%{0}%'", query.RealName);
        }
        if (!string.IsNullOrEmpty(query.accountALLHere))
        {
            builder.AppendFormat(" and accountALLHere='{0}'", query.accountALLHere);
        }
        if (!string.IsNullOrEmpty(query.states))
        {
            builder.AppendFormat(" and State={0}",query.states);
        }
        DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_StoreService", "DcID", builder.ToString(), "*");
        this.grdDistributor.DataSource = dtmemberInfo.Data;
        this.grdDistributor.DataBind();
        this.pager.TotalRecords = dtmemberInfo.TotalRecords;
        this.txtSearchText.Text = query.keyword;
        this.states.SelectedValue = query.states;
        this.txtRealName.Text = query.RealName;
        this.txtAllHere.Text = query.accountALLHere;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ReBind(true);
    }
    private ListDistributorClassQuery LoadParameters()
    {
        ListDistributorClassQuery query = new ListDistributorClassQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "ApplyDate";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
        {
            query.keyword = base.Server.UrlDecode(this.Page.Request.QueryString["keyword"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["states"]))
        {
            query.states =this.Page.Request.QueryString["states"];
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
        {
            query.RealName = this.Page.Request.QueryString["RealName"];
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["accountALLHere"]))
        {
            query.accountALLHere = this.Page.Request.QueryString["accountALLHere"];
        }
        return query;
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        if (!string.IsNullOrEmpty(this.states.SelectedValue))
        {
            queryStrings.Add("states", this.states.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("keyword", this.txtSearchText.Text);
        queryStrings.Add("RealName", this.txtRealName.Text);
        queryStrings.Add("accountALLHere", this.txtAllHere.Text);

        base.ReloadPage(queryStrings);
    }
    public string serviceName(string ScIDs)
    {
        string cate = "";
        if (!string.IsNullOrEmpty(ScIDs))
        {
          DataTable dtcate= ServiceClassHelper.SelectClassByWhere(" ScID in (" + ScIDs.TrimEnd(',') + ")");
          foreach (DataRow row in dtcate.Rows)
          {
              cate += row["ClassName"] + ",";
          }
        }
        return cate.TrimEnd(',');
    }
}