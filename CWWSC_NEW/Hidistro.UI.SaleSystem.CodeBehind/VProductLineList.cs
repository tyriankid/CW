﻿namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductLineList : VWeiXinOAuthTemplatedWebControl
    {
        private int categoryId;
        private string keyWord = string.Empty;
        private VshopTemplatedRepeater rpCategorys;
        private VshopTemplatedRepeater rpChooseProducts;
        private HtmlInputText txtkeywords;

        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.txtkeywords = (HtmlInputText) this.FindControl("keywords");
            this.rpChooseProducts = (VshopTemplatedRepeater) this.FindControl("rpChooseProducts");
            this.rpCategorys = (VshopTemplatedRepeater) this.FindControl("rpCategorys");
            this.DataBindSoruce();

            Literal litNext = (Literal)this.FindControl("litNext");
            if (litNext != null)
            {
                litNext.Text = (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto) ? "" : "下架所选商品";
            }
        }

        private void DataBindSoruce()
        {
            //type:0正常显示店铺已上架的商品，1正常显示店铺未上架的商品，2显示所有出售状态的商品，3根据上架范围显示已上架的商品，4根据上架范围显示未上架的商品
            ProductInfo.ProductRanage productRanage = DistributorsBrower.GetCurrStoreProductRange();
            int num;
            this.txtkeywords.Value = this.keyWord;
            this.rpCategorys.DataSource = CategoryBrowser.GetCategories();
            this.rpCategorys.DataBind();


            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
          
            int dzUserId = Globals.GetCurrentDistributorId();//店长Id
            //得到店长用户实体数据对象
            MemberInfo memberinfo = MemberProcessor.GetMember(dzUserId);

            //获取当前门店的分公司ID
            StoreInfo storeinfo = StoreInfoHelper.GetStoreInfoByUserId(Globals.GetCurrentDistributorId());
            this.rpChooseProducts.DataSource = ProductBrowser.GetProductsEx(memberinfo, null, new int?(this.categoryId), this.keyWord, 1, 0x2710, out num, "DisplaySequence", "desc", productRanage, storeinfo.fgsid.ToString());
            this.rpChooseProducts.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName="Skin-ProductLineList.html";
            }
            base.OnInit(e);
        }
    }
}

