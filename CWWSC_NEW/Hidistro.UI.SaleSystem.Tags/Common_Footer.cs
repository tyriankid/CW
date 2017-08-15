namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Common_Footer : VshopTemplatedWebControl
    {
        private HyperLink hyperindex;
        private Literal lblStyle;
        private Literal litDistrbutorTitle;
        private Literal litDistrbutorUrl;
        private Panel paneldistributor;
        private Literal litAllProduct;
        private Literal litProductUrl;
        private HtmlInputHidden isTypeButtonHide;

        protected override void AttachChildControls()
        {
            this.hyperindex = (HyperLink) this.FindControl("hyperindex");
            this.litDistrbutorTitle = (Literal) this.FindControl("litDistrbutorTitle");
            this.litDistrbutorUrl = (Literal) this.FindControl("litDistrbutorUrl");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            //this.litDistrbutorUrl.Text = "ApplicationDescription.aspx";
            this.litDistrbutorUrl.Text = "StoreIdentification.aspx";
            this.lblStyle = (Literal) this.FindControl("lblStyle");
            this.paneldistributor = (Panel) this.FindControl("paneldistributor");
            this.litAllProduct = (Literal)this.FindControl("litAllProduct");
            this.litProductUrl = (Literal)this.FindControl("litProductUrl");
            this.isTypeButtonHide = (HtmlInputHidden)this.FindControl("isTypeButtonHide");

            if (this.Page.Session["stylestatus"] != null)
            {
                this.lblStyle.Text = this.Page.Session["stylestatus"].ToString();
            }
            this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_Showfootapply;
            DistributorsInfo userIdDistributors = null;
            if (currentMember != null)
            {
                userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_Showfootmanage;
                    //this.litDistrbutorUrl.Text = "DistributorValid.aspx";
                    this.litDistrbutorUrl.Text = "DistributorCenter.aspx";
                }
                else
                {
                    /*************验证店员,2017-06-27修改**************/
                    DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMember.UserId);
                    if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                    {
                        //userIdDistributors = DistributorsBrower.GetUserIdDistributors(disSalesinfo.DisUserId);
                        if (disSalesinfo.DsType == 0)
                        {
                            this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_Showfootmanage;
                            //this.litDistrbutorUrl.Text = "DistributorValid.aspx";
                            this.litDistrbutorUrl.Text = "DistributorCenter.aspx";
                        }
                        else
                        {
                            this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_ShowfootService;
                            //this.litDistrbutorUrl.Text = "DistributorValid.aspx";
                            this.litDistrbutorUrl.Text = "MemberOrdersService.aspx?status=0";
                        }
                    }
                    /***************************/
                }
            }

            this.litAllProduct.Text = "分类";
            this.litProductUrl.Text = "ProductSearch.aspx";

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            decimal expenditure = 0M;
            if (currentMember != null && currentMember.UserId >0)
            {
                expenditure = currentMember.Expenditure;
                int currentDistributorId = currentMember.DistributorUserId;
                if ((this.hyperindex != null) && (currentDistributorId > 0))
                {
                    this.hyperindex.NavigateUrl = "Default.aspx?ReferralId=" + currentDistributorId;
                }
            }
            //this.paneldistributor.Visible = (masterSettings.IsRequestDistributor && (expenditure >= masterSettings.FinishedOrderMoney)) || ((userIdDistributors != null) && (userIdDistributors.UserId > 0));
            this.paneldistributor.Visible = true;
            //int currentDistributorId = Globals.GetCurrentDistributorId();

            


            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //MemberInfo info3 = MemberProcessor.GetCurrentMember();
            //decimal expenditure = 0M;
            //DistributorsInfo info4 = null;
            //if ((info3 != null) && (info3.UserId > 0))
            //{
            //    info4 = DistributorsBrower.GetUserIdDistributors(info3.UserId);
            //    expenditure = info3.Expenditure;
            //}
            //this.paneldistributor.Visible = (masterSettings.IsRequestDistributor && (expenditure >= masterSettings.FinishedOrderMoney)) || ((info4 != null) && (info4.UserId > 0));
            //int currentDistributorId = Globals.GetCurrentDistributorId();
            //if ((this.hyperindex != null) && (currentDistributorId > 0))
            //{
            //    this.hyperindex.NavigateUrl = "Default.aspx?ReferralId=" + currentDistributorId;
            //}
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Footer.html";
            }
            base.OnInit(e);
        }
    }
}

