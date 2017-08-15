namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Function;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyOrderSeller : VWeiXinOAuthTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string strOrderid = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(strOrderid))
            {
                base.GotoResourceNotFound("");
            }

            Literal litStoreName = this.FindControl("litStoreName") as Literal;
            Literal litStoreManager = this.FindControl("litStoreManager") as Literal;
            Literal litStoreNumber = this.FindControl("litStoreNumber") as Literal;
            Literal litSaleService = this.FindControl("litSaleService") as Literal;
            Literal litGysName = this.FindControl("litGysName") as Literal;
            Literal litGysPhone = this.FindControl("litGysPhone") as Literal;
            Literal litGysAddress = this.FindControl("litGysAddress") as Literal;

            HtmlInputHidden litType1 = (HtmlInputHidden)this.FindControl("litType1");
            HtmlInputHidden litType2 = (HtmlInputHidden)this.FindControl("litType2");

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(strOrderid);
            if (orderInfo.ReferralUserId > 0)
            {
                //通过当前用户id查找分销商信息,从而来判断用户类型
                DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(orderInfo.ReferralUserId);
                if (currentDistributor.StoreId > 0)
                {
                    StoreInfo storeinfo = StoreInfoHelper.GetStoreInfo(currentDistributor.StoreId);
                    litStoreName.SetWhenIsNotNull(storeinfo.storeName);
                    litStoreManager.SetWhenIsNotNull(storeinfo.storeRelationPerson);
                    litStoreNumber.SetWhenIsNotNull(storeinfo.storeRelationCell);
                }
                litType1.Value = "1";//显示所属门店信息
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                litSaleService.Text = masterSettings.SaleService;
                litType1.Value = "0";//显示总店信息
            }
            if (orderInfo.SupplierId > 0)
            {
                SupplierInfo suppinfo = SupplierHelper.GetSupplier(orderInfo.SupplierId);
                if (suppinfo != null && suppinfo.Id > 0)
                {
                    litGysName.SetWhenIsNotNull(suppinfo.gysName);
                    litGysPhone.SetWhenIsNotNull(suppinfo.gysPhone);
                    litGysAddress.SetWhenIsNotNull(suppinfo.gysAddress);
                }
                litType2.Value = "1";//显示所属供应商信息
            }

            PageTitle.AddSiteNameTitle("订单售后");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyOrderSeller.html";
            }
            base.OnInit(e);
        }
    }
}

