namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
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
    public class VApplyOperat : VWeiXinOAuthTemplatedWebControl
    {
        private CommonTagsLiteral serviceClassLiteral;
        private HtmlInputHidden region;
        private HtmlInputHidden regionText;

        protected override void AttachChildControls()
        {
            this.serviceClassLiteral = (CommonTagsLiteral)this.FindControl("serviceClassLiteral");
            this.regionText = (HtmlInputHidden)this.FindControl("regionText");
            this.region = (HtmlInputHidden)this.FindControl("region");

            if (!string.IsNullOrEmpty(this.Page.Request["DcID"]))
            {
                Guid DsId = new Guid(this.Page.Request["DcID"]);
                DistributorClass disclass = DistributorClassHelper.GetDistributorClassByDcID(DsId);
                if (disclass != null && disclass.DcID != Guid.Empty)
                {
                    this.serviceClassLiteral.SelectedValue = disclass.ScIDs;

                    string fullRegion = RegionHelper.GetFullRegion(disclass.RegionId, " ");
                    this.regionText.SetWhenIsNotNull(fullRegion);
                    this.region.SetWhenIsNotNull(disclass.RegionId.ToString());
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ApplyOperat.html";
            }
            base.OnInit(e);
        }
    }
}

