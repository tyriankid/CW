namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VAddReceiptDz : VWeiXinOAuthTemplatedWebControl
    {
        private RegionSelector dropRegions;
        protected override void AttachChildControls()
        {
            //this.dropRegions = (RegionSelector) this.FindControl("dropRegions");
            PageTitle.AddSiteNameTitle("新增电子发票信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vaddreceiptdz.html";
            }
            base.OnInit(e);
        }
    }
}

