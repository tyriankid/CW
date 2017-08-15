namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hishop.Weixin.MP.Util;
    using Hidistro.ControlPanel.Commodities;

    [ParseChildren(true)]
    public class VProductDetails : VWeiXinOAuthTemplatedWebControl
    {
        private Common_ExpandAttributes expandAttr;
        private HyperLink linkDescription;
        private Literal litActivities;
        private HtmlInputHidden litCategoryId;
        private Literal litConsultationsCount;
        private Literal litDescription;
        private Literal litSpecification;//*
        private HtmlInputHidden litHasCollected;
        private Literal litItemParams;
        private Literal litMarketPrice;
        private Literal litProdcutName;
        private HtmlInputHidden litproductid;
        private HtmlInputHidden litproductcode;
        private Literal litReviewsCount;
        private Literal litSalePrice;
        private Literal litMemberGradeInfo;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litStock;
        private int productId;
        private string isbackshow = "";
        private VshopTemplatedRepeater rptProductImages;
        private VshopTemplatedRepeater rptProductImages1;
        private Common_SKUSelector skuSelector;
        private Literal litFreeGetOne; //2017-8-11  买一送一 yk
        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound("");
            }
            if (this.Page.Request.QueryString["isbackshow"] != null)
                isbackshow = this.Page.Request.QueryString["isbackshow"].ToString();
            this.rptProductImages = (VshopTemplatedRepeater) this.FindControl("rptProductImages");
            this.rptProductImages1 = (VshopTemplatedRepeater)this.FindControl("rptProductImages1");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litProdcutName = (Literal) this.FindControl("litProdcutName");
            this.litActivities = (Literal) this.FindControl("litActivities");
            this.litSalePrice = (Literal) this.FindControl("litSalePrice");
            this.litMemberGradeInfo = (Literal)this.FindControl("litMemberGradeInfo");
            this.litMarketPrice = (Literal) this.FindControl("litMarketPrice");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litSpecification = (Literal)this.FindControl("litSpecification");
            this.litStock = (Literal) this.FindControl("litStock");
            this.skuSelector = (Common_SKUSelector) this.FindControl("skuSelector");
            this.linkDescription = (HyperLink) this.FindControl("linkDescription");
            this.expandAttr = (Common_ExpandAttributes)this.FindControl("ExpandAttributes");
            this.litSoldCount = (Literal) this.FindControl("litSoldCount");
            this.litConsultationsCount = (Literal) this.FindControl("litConsultationsCount");
            this.litReviewsCount = (Literal) this.FindControl("litReviewsCount");
            this.litHasCollected = (HtmlInputHidden) this.FindControl("litHasCollected");
            this.litCategoryId = (HtmlInputHidden) this.FindControl("litCategoryId");
            this.litproductid = (HtmlInputHidden) this.FindControl("litproductid");
            this.litproductcode = (HtmlInputHidden)this.FindControl("litproductcode");

            this.litFreeGetOne = (Literal)this.FindControl("litFreeGetOne");

            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ProductInfo product = ProductBrowser.GetProduct(currentMember, this.productId);
            this.litproductid.Value = this.productId.ToString();
            this.litproductcode.Value = product.ProductCode;//设置商品编码

            //取消显示用户等级， 会员等级无效
            //string currentMemberGradeName = (MemberProcessor.GetCurrentMember() == null) ? "" :
            //    string.Format("<span style='font-size:12px; background:#F90; color:#FFF; border-radius:3px; padding:3px 5px; margin-right:5px;'>{0}</span>"
            //    , Hidistro.ControlPanel.Members.MemberHelper.GetMemberGrade(MemberProcessor.GetCurrentMember().GradeId).Name);
            //this.litMemberGradeInfo.Text = currentMemberGradeName;

            //设置商品价格，先得到当前微信用户实体对象
            
            //定义价格显示值对象
            string strPrice = "¥" + product.MinSalePrice.ToString("F2");//默认商品最低一口价
            bool isneigou = false;
            //验证是否存在内购价格
            if (product.MinNeigouPrice > 0 && currentMember.DistributorUserId > 0)
            {
                //判断是否为内购门店
                if (VShopHelper.ValidateNeiGouDistributor(currentMember.DistributorUserId))
                {
                    isneigou = true;
                    if (isbackshow != "1")
                    {
                        //是内购门店则显示内购价格
                        strPrice = "¥" + product.MinNeigouPrice.ToString("F2");
                        this.litMemberGradeInfo.Text = string.Format("<span style='font-size:12px; background:#F90; color:#FFF; border-radius:3px; padding:3px 5px;margin-right:5px;position:relative;top:-3px'>{0}</span>", "内购价");
                    }
                }
            }
            
            //只有一口价才允许活动
            if (!isneigou)
            {
                //star***************yk 2017-8-11*******************
                if (BuyOneGetOnrFreeHelper.BuyOneGetOneProductExist(this.productId))
                {
                    this.litFreeGetOne.Text = "<div class=\"action\"><span class=\"purchase\">买一送一</span><div class=\"all-action\">";

                }
                //end**********************************


                //start***************满减活动控制显示******************//
                //if (!string.IsNullOrEmpty(product.MainCategoryPath))
                //DataTable allFull = ProductBrowser.GetAllFull(int.Parse(product.MainCategoryPath.Split(new char[] { '|' })[0].ToString()));
                DataTable allFull = ProductBrowser.GetAllFull(product.ProductId);
                this.litActivities.Text = "<div class=\"price clearfix\"><span class=\"title\">促销活动：</span><div class=\"all-action\">";
                if (allFull.Rows.Count > 0)
                {
                    for (int i = 0; i < allFull.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            //this.litActivities.Text = string.Concat(new object[] { text, "<div class=\"action\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                            this.litActivities.Text = string.Concat(new object[] { this.litActivities.Text, "<div class=\"action\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                        }
                        else
                        {
                            //this.litActivities.Text = string.Concat(new object[] { obj3, "<div class=\"action actionnone\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", allFull.Rows[i]["ActivitiesName"].ToString(), "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                            this.litActivities.Text = string.Concat(new object[] { this.litActivities.Text, "<div class=\"action actionnone\"><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=", allFull.Rows[i]["ActivitiesId"], "&CategoryId=", allFull.Rows[i]["ActivitiesType"], "\">", "满", decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()).ToString("0"), "减", decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString()).ToString("0"), "</a>&nbsp;&nbsp;</span></div>" });
                        }
                    }
                    //this.litActivities.Text = this.litActivities.Text + "</div><em>&nbsp;more</em></div>";
                }
                else
                {
                    this.litActivities.Text = "";
                }
                //end***************满减活动控制显示******************//
            }

            

            if (!string.IsNullOrEmpty(this.litActivities.Text) && (product == null))
            {
                base.GotoResourceNotFound("此商品已不存在");
            }
            Uri u1 = HttpContext.Current.Request.UrlReferrer;
            if (product.SaleStatus != ProductSaleStatus.OnSale && (u1 == null || u1.ToString().ToLower().IndexOf("admin")==-1))
            {
                base.GotoResourceNotFound("此商品已下架");//20160505,增加处理，后台查看商品时允许显示
            }
            if (this.rptProductImages != null)
            {
                string locationUrl = "javascript:;";
                SlideImage[] imageArray = new SlideImage[] { new SlideImage(product.ImageUrl1, locationUrl), new SlideImage(product.ImageUrl2, locationUrl), new SlideImage(product.ImageUrl3, locationUrl), new SlideImage(product.ImageUrl4, locationUrl), new SlideImage(product.ImageUrl5, locationUrl) };
                this.rptProductImages.DataSource = from item in imageArray
                    where !string.IsNullOrWhiteSpace(item.ImageUrl)
                    select item;
                this.rptProductImages.DataBind();
                this.rptProductImages1.DataSource = from item in imageArray
                                                   where !string.IsNullOrWhiteSpace(item.ImageUrl)
                                                   select item;
                this.rptProductImages1.DataBind();
            }
            string mainCategoryPath = product.MainCategoryPath;
            if (!string.IsNullOrEmpty(mainCategoryPath))
            {
                this.litCategoryId.Value = mainCategoryPath.Split(new char[] { '|' })[0];
            }
            else
            {
                this.litCategoryId.Value = "0";
            }
            this.litProdcutName.Text = product.ProductName;

            this.litSalePrice.Text = strPrice;

            if (product.MarketPrice.HasValue)
            {
                this.litMarketPrice.SetWhenIsNotNull(product.MarketPrice.GetValueOrDefault(0M).ToString("F2"));
            }
            this.litShortDescription.Text = product.ShortDescription;
            if (this.litDescription != null)
            {
                this.litDescription.Text = product.Description;
            }
            if (this.litSpecification != null)
            {
                this.litSpecification.Text = product.specification;
            }
            this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
            this.litStock.Text = product.Stock.ToString();
            this.skuSelector.ProductId = this.productId;
            if (this.expandAttr != null)
            {
                this.expandAttr.ProductId = this.productId;
            }
            if (this.linkDescription != null)
            {
                this.linkDescription.NavigateUrl = "/Vshop/ProductDescription.aspx?productId=" + this.productId;
            }
            this.litConsultationsCount.SetWhenIsNotNull(ProductBrowser.GetProductConsultationsCount(this.productId, false).ToString());
            this.litReviewsCount.SetWhenIsNotNull(ProductBrowser.GetProductReviewsCount(this.productId).ToString());

            bool flag = false;
            if (currentMember != null)
            {
                flag = ProductBrowser.CheckHasCollect(currentMember.UserId, this.productId);
            }
            this.litHasCollected.SetWhenIsNotNull(flag ? "1" : "0");
            ProductBrowser.UpdateVisitCounts(this.productId);
            PageTitle.AddSiteNameTitle("商品详情");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }

            string strUrl = HttpContext.Current.Request.Url.ToString();
            if(strUrl.IndexOf("ReferralId") < 0)
            {
                if (HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["Vshop-ReferralId"].ToString()))
                {
                    strUrl += "&ReferralId=" + HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value;
                }             
            }
            this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), product.ImageUrl1, "|", this.litProdcutName.Text, "|", product.ShortDescription, "|", strUrl });
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

