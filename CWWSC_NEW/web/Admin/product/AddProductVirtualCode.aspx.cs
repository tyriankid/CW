using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_product_AddProductVirtualCode : AdminPage
{
    //页面加载
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            int productId = 0;
            int virtualid = 0;
            if (this.Request.QueryString["productId"] != null && !string.IsNullOrEmpty(this.Request.QueryString["productId"].ToString()) && int.TryParse(this.Request.QueryString["productId"].ToString(), out productId))
            {
                ProductInfo productinfo = ProductHelper.GetProductBaseInfo(productId);
                if (productinfo != null)
                {
                    this.hiProductName.Text = productinfo.ProductName;
                    this.dropProductSku.ProductId = productId;
                    this.dropProductSku.DataBind();
                }
            }
            
            if (this.Request.QueryString["virtualId"] != null && !string.IsNullOrEmpty(this.Request.QueryString["virtualId"].ToString()) && int.TryParse(this.Request.QueryString["virtualId"].ToString(), out virtualid))
            {
                this.litPageTip.Text = "编辑虚拟码信息。";
                ShowData(virtualid);
            }
            else
            {
                this.litPageTip.Text = "添加虚拟码信息。";
            }

            
            
        }
    }

    /// <summary>
    /// 显示虚拟码
    /// </summary>
    /// <param name="virtualid"></param>
    protected void ShowData(int virtualid)
    {
        //编辑加载
        ProductVirtualInfo virtualinfo = ProductVirtualInfoHelper.GetProductVirtualByDsID(virtualid);
        if (virtualinfo != null)
        {
            this.txtVirtualCode.Text = virtualinfo.VirtualCode;
            this.dropProductSku.SelectedValue = virtualinfo.SkuId;
            this.dropProductSku.Enabled = false;
        }
    }

    //保存按钮
    protected void btnSaveClientSettings_Click(object sender, EventArgs e)
    {
        int productId = 0;
        int virtualid = 0;
        //ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
        if (this.Request.QueryString["productId"] == null || string.IsNullOrEmpty(this.Request.QueryString["productId"].ToString()) || !int.TryParse(this.Request.QueryString["productId"].ToString(), out productId))
        {
            this.ShowMsg("页面参数错误。", false);
            return;
        }
        ProductVirtualInfo virtualinfo;
        if (this.Request.QueryString["virtualId"] != null && !string.IsNullOrEmpty(this.Request.QueryString["virtualId"].ToString()) && int.TryParse(this.Request.QueryString["virtualId"].ToString(), out virtualid))
        {
            //编辑
            virtualinfo = ProductVirtualInfoHelper.GetProductVirtualByDsID(virtualid);
            virtualinfo.VirtualCode = this.txtVirtualCode.Text.Trim();
            if (ProductVirtualInfoHelper.UpdateProductVirtualInfo(virtualinfo))
            {
                this.ShowMsgAndReUrl("保存成功。", false, string.Format("SetProductVirtualCodes.aspx?productId={0}", productId));
            }
            else
            {
                this.ShowMsg("保存失败。", false);
            }
        }
        else
        {
            //新建
            virtualinfo = new ProductVirtualInfo();
            virtualinfo.VirtualCode = this.txtVirtualCode.Text.Trim();
            virtualinfo.VirtualState = 0;
            virtualinfo.CreateDate = DateTime.Now;
            virtualinfo.ProductId = productId;
            virtualinfo.SkuId = this.dropProductSku.SelectedValue;
            //累加虚拟商品库存值,增加SQL中已经执行
            if (ProductVirtualInfoHelper.AddProductVirtualInfo(virtualinfo))
            {
                this.ShowMsgAndReUrl("保存成功。", false, string.Format("SetProductVirtualCodes.aspx?productId={0}", productId));
            }
            else
            {
                this.ShowMsg("保存失败。", false);
            }
        }

    }
}