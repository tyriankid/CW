namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VStoreRuleOperat : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputText txtName;
        private HtmlInputText txtPhone;
        private HtmlInputText txtSort;
        private HtmlSelect selectDsType;
        protected override void AttachChildControls()
        {
            if (!string.IsNullOrEmpty(this.Page.Request["DsId"]))
            {
                Guid DsId = new Guid(this.Page.Request["DsId"]);
                this.txtName = (HtmlInputText)this.FindControl("txtName");
                this.txtPhone = (HtmlInputText)this.FindControl("txtPhone");
                this.txtSort = (HtmlInputText)this.FindControl("txtSort");
                this.selectDsType = (HtmlSelect)this.FindControl("selectDsType");

                DistributorSales info = DistributorSalesHelper.GetSalesByDsID(DsId);
                if (info != null)
                {
                    this.txtName.Value = info.DsName;
                    this.txtPhone.Value = info.DsPhone;
                    this.txtSort.Value = info.Scode;
                    this.selectDsType.Value =info.DsType.ToString();
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-StoreRuleOperat.html";
            }
            base.OnInit(e);
        }
    }
}

