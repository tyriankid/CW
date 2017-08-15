namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSaleService : VshopTemplatedWebControl
    {
        private Literal litSaleService;
        private Literal litDistributors;

        protected override void AttachChildControls()
        {
            this.litSaleService = (Literal) this.FindControl("litSaleService");
            this.litDistributors = (Literal) this.FindControl("litDistributors");

            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.litSaleService.Text = masterSettings.SaleService;

                int distrId = Globals.GetCurrentDistributorId();
                if (distrId > 0)
                {
                    //通过当前用户id查找分销商信息,从而来判断用户类型
                    DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(distrId);
                    if (currentDistributor != null && currentDistributor.StoreId > 0)
                    {
                        StoreInfo storeinfo = StoreInfoHelper.GetStoreInfo(currentDistributor.StoreId);
                        if (storeinfo != null && storeinfo.Id > 0)
                        {
                            string strTitle = "<br />当前店铺名称：" + storeinfo.storeName + "<br />";
                            strTitle += "当前店铺联系人：" + storeinfo.storeRelationPerson + "<br />";
                            strTitle += "当前店铺电话：" + storeinfo.storeRelationCell;
                            this.litDistributors.Text = strTitle;
                        }
                    }
                }
            }
            PageTitle.AddSiteNameTitle("售后服务");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VSaleService.html";
            }
            base.OnInit(e);
        }
    }
}

