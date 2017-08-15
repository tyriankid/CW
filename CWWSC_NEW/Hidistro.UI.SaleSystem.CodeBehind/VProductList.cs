namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Members;

    [ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private int valueId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotalPages;
        private VshopTemplatedRepeater ReUserSearch;
        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int num3;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            int.TryParse(this.Page.Request.QueryString["valueId"], out this.valueId);

            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.ReUserSearch = (VshopTemplatedRepeater)this.FindControl("ReUserSearch");
            this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
            this.Page.Session["stylestatus"] = "4";
            string str = this.Page.Request.QueryString["sort"];

            if (string.IsNullOrWhiteSpace(str))
            {
                str = "DisplaySequence";
            }
            string str2 = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();
            string swr = "";
            DataTable dt = null;
            /*****start2017-08-03,门店过滤分公司商品的显示******/
            string fgsId = "";
            //获取当前门店的分公司ID
            StoreInfo storeinfo = StoreInfoHelper.GetStoreInfoByUserId(Globals.GetCurrentDistributorId());
            if (storeinfo != null && !string.IsNullOrEmpty(storeinfo.fgsid.ToString()))
            {
                fgsId = storeinfo.fgsid.ToString();
            }
            /*****end2017-08-03,门店过滤分公司商品的显示******/
            //*****************star 获取当前模块的搜索记录 2017-8-8 yk********************************
            DataTable dtUserSearch = UserSearchLogsHelper.GetUserSearchData(" where FunctionType='List' order by searchDate desc");
            if (dtUserSearch.Rows.Count > 0)
            {
                this.ReUserSearch.DataSource = dtUserSearch;
                this.ReUserSearch.DataBind();
            }
            //*******************end*********************************************************
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["TypeId"]))
            {
                int typeID = Convert.ToInt32(this.Page.Request.QueryString["TypeId"]);
                if (typeID == 1)
                {
                    swr = " CategoryId in (1,2,3,4)";
                    dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, null, null, this.keyWord, num, num2, out num3, str, str2, swr, 0, fgsId);
                }
                else
                {
                    swr = " CategoryId in (select CategoryId from Hishop_Categories where AssociatedProductType=" + typeID + ")";
                    dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, null, null, this.keyWord, num, num2, out num3, str, str2, swr, 0, fgsId);
                }
            }
            else
            {
                dt = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), new int?(this.valueId), this.keyWord, num, num2, out num3, str, str2, "", 0, fgsId);
            }
            this.rptProducts.DataSource = dt;
            this.rptProducts.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(num3.ToString());
            PageTitle.AddSiteNameTitle("分类搜索页");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }
    }
}

