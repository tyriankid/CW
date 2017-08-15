using ControlPanel.Lottery;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Lottery;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_IntegralLottery_IntegralLotteryRule : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.LoadParameters();
        if (!IsPostBack)
        {
            BindPage();

        }
    }

    /// <summary>
    /// 绑定后台用户分页
    /// </summary>
    private void BindPage()
    {

        LotteryRuleQuery query = new LotteryRuleQuery
        {
            KeyValue = this.txtSearchText.Text,
            PageIndex = this.pager.PageIndex,
            PageSize = this.pager.PageSize,
            SortBy = "LotteryClass",
            SortOrder = Hidistro.Core.Enums.SortAction.Asc,
        };
        DbQueryResult ListStore = LotteryRuleHelper.GetListLotteryRuleList(query);

        this.rptRule.DataSource = ListStore.Data;
        this.rptRule.DataBind();
        this.pager.TotalRecords = ListStore.TotalRecords;
       
    }



    /// <summary>
    /// 重载
    /// </summary>
    /// <param name="isSearch">是否查询(重置起始页码)</param>
    
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


    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_Click(object sender, System.EventArgs e)
    {
        this.ReBind(true);
    }
   

    //删除
    [System.Web.Services.WebMethod]
    public static bool Del(Guid id)
    {
        return LotteryRuleHelper.DeleteLotteryRule(id);
        
    }


    private void LoadParameters()
    {
        if (!this.Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
            {
                this.txtSearchText.Text = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
            }
            this.txtSearchText.Text = this.Page.Request.QueryString["searchKey"];
        }
    }
}