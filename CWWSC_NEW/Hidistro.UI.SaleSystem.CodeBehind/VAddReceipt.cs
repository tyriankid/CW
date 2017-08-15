namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VAddReceipt : VWeiXinOAuthTemplatedWebControl
    {
        private RegionSelector dropRegions;
        protected override void AttachChildControls()
        {
            this.dropRegions = (RegionSelector) this.FindControl("dropRegions");
            PageTitle.AddSiteNameTitle("新增增值税发票信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vaddreceipt.html";
            }
            base.OnInit(e);
        }
    }
}

