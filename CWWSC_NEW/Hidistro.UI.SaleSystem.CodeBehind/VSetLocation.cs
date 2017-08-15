namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSetLocation : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputHidden hdbackimg;
        private HtmlInputHidden hdlogo;
        private HtmlImage imglogo;
        private Literal litBackImg;
        private HtmlInputText txtacctount;
        private HtmlTextArea txtdescription;
        private HtmlInputText txtstorename;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺消息");
            this.imglogo = (HtmlImage) this.FindControl("imglogo");
            this.litBackImg = (Literal) this.FindControl("litBackImg");
            this.hdbackimg = (HtmlInputHidden) this.FindControl("hdbackimg");
            this.hdlogo = (HtmlInputHidden) this.FindControl("hdlogo");
            this.txtstorename = (HtmlInputText) this.FindControl("txtstorename");
            this.txtdescription = (HtmlTextArea) this.FindControl("txtdescription");
            this.txtacctount = (HtmlInputText) this.FindControl("txtacctount");

            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            //重置功能
            if (this.Page.Request.QueryString["reset"] == "1")
            {
                userIdDistributors.BackImage = "";
                DistributorsBrower.UpdateDistributor(userIdDistributors);
            }
            if (userIdDistributors != null)
            {
                
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SetLocation.html";
            }
            base.OnInit(e);
        }
    }
}

