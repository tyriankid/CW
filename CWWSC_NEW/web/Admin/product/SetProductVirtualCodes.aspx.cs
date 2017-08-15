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

public partial class Admin_product_SetProductVirtualCodes : AdminPage
{
    public int productid = 0;
    public int? state = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.grdProductVirtual.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProductVirtual_RowDeleting);
            BindProductVirtualCode();
        }
        if(!string.IsNullOrEmpty(this.hiproductid.Value))
            productid = Convert.ToInt32(this.hiproductid.Value);
    }
    protected void BindProductVirtualCode()
    {
        ListProductVirtualInfoQuery query = LoadParameters();
        DbQueryResult dtProductVirtual = ProductVirtualInfoHelper.GetListProductVirtualInfo(query);
        this.grdProductVirtual.DataSource = dtProductVirtual.Data;
        this.grdProductVirtual.DataBind();
        this.pager.TotalRecords = dtProductVirtual.TotalRecords;
        this.txtSearchText.Text = query.keyword;
        productid = query.ProductId;
        this.hiproductid.Value = productid.ToString();
        state = query.states;
        this.states.SelectedValue = state.ToString();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ReBind(true);
    }
    private ListProductVirtualInfoQuery LoadParameters()
    {
        ListProductVirtualInfoQuery query = new ListProductVirtualInfoQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "CreateDate";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
        {
            query.keyword = base.Server.UrlDecode(this.Page.Request.QueryString["keyword"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productId"]))
        {
            query.ProductId = Convert.ToInt32(this.Page.Request.QueryString["productId"].ToString());
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["states"]))
        {
            query.states = Convert.ToInt32(this.Page.Request.QueryString["states"].ToString());
        }
        return query;
    }

    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("keyword", this.txtSearchText.Text);
        queryStrings.Add("productId", productid.ToString());
        if (!string.IsNullOrEmpty(this.states.SelectedValue))
        {
            queryStrings.Add("states", this.states.SelectedValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        base.ReloadPage(queryStrings);
    }


    protected void grdProductVirtual_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int virtualid = (int)this.grdProductVirtual.DataKeys[e.RowIndex].Value;
        ProductVirtualInfo virtualinfo = ProductVirtualInfoHelper.GetProductVirtualByDsID(virtualid);
        if (virtualinfo != null)
        {
            if (virtualinfo.VirtualState == 0)
            {
                if (ProductVirtualInfoHelper.DeleteProductVirtualInfo(virtualid, virtualinfo.VirtualState, virtualinfo.SkuId))
                {
                    this.ShowMsg("删除服务品类成功。", true);
                }
                else
                {
                    this.ShowMsg("删除服务品类失败。", false);
                }
                this.BindProductVirtualCode();
            }
            else
                this.ShowMsg("虚拟码已经使用，无法删除。", false);
        }
        else
        {
            this.ShowMsg("删除失败，参数错误。", false);
        }
    }

}