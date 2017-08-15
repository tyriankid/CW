﻿namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Commodities;
    using Hidistro.ControlPanel.Commodities;

    [ParseChildren(true)]
    public class VReceeiptDz : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor aLinkToAdd;
        private VshopTemplatedRepeater rptvReceipt;

        protected override void AttachChildControls()
        {
            this.rptvReceipt = (VshopTemplatedRepeater)this.FindControl("rptvReceipt");
            this.aLinkToAdd = (HtmlAnchor) this.FindControl("aLinkToAdd");
            this.aLinkToAdd.HRef = Globals.ApplicationPath + "/Vshop/AddReceiptDz.aspx";
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
            {
                this.aLinkToAdd.HRef = this.aLinkToAdd.HRef + "?returnUrl=" + Globals.UrlEncode(this.Page.Request.QueryString["returnUrl"]);
            }
            IList<UserReceiptInfo> userreceipt = UserReceiptInfoHelper.GetUserReceiptInfo("0");
            if (userreceipt != null)
            {
                this.rptvReceipt.DataSource = userreceipt;
                this.rptvReceipt.DataBind();
            }
            PageTitle.AddSiteNameTitle("电子发票");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vreceeiptdz.html";
            }
            base.OnInit(e);
        }
    }
}
