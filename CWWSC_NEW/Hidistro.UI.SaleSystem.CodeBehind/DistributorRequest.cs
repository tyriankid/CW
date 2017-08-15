namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using Hidistro.Entities.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using System.Data;
    using ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;

    [ParseChildren(true)]
    public class DistributorRequest : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litBackImg;
        private System.Web.UI.HtmlControls.HtmlInputHidden storeName;
        private System.Web.UI.HtmlControls.HtmlInputHidden stroeid;
        private System.Web.UI.HtmlControls.HtmlInputHidden allherecode;
        private HtmlInputText txtstorename;
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请门店");
            this.Page.Session["stylestatus"] = "2";

            MemberInfo memberinfo = MemberProcessor.GetCurrentMember();
            if (memberinfo != null)
            {
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(memberinfo.UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    this.Page.Response.Redirect("DistributorCenter.aspx", true);
                }
            }

            Literal litNext = (Literal)this.FindControl("litNext");
            //增加判断,如果选了快速开店,上架商品页面也必须要跳过.
            if(litNext!=null)
            {
                if ((Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreInfoSet))//如果开启了店铺配置(非快速开店)
                {
                    litNext.Text = (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto) ? "" : "，上架商品";
                }
                else//如果没开店铺配置(快速开店)
                {
                    storeName = (HtmlInputHidden)this.FindControl("storeName");
                    if (storeName != null) //附一个默认值
                    {
                        int i = 1;
                        storeName.Value = MemberProcessor.GetCurrentMember().UserName.ToString() + "的小店";
                        while (DistributorsBrower.IsExiteDistributorsByStoreName(storeName.Value) > 0)
                        {
                            i++;
                            storeName.Value += i.ToString();
                        }
                    }
                    litNext.Text = "";
                }
            }

            //处理认证成功后门店,
            if(Page.Request.QueryString["storeId"] != null)
            {
                this.txtstorename = (HtmlInputText)this.FindControl("txtstorename");
                this.stroeid = (HtmlInputHidden)this.FindControl("stroeid");
                this.allherecode = (HtmlInputHidden)this.FindControl("allherecode");
                int storeId = 0;
                if(!string.IsNullOrEmpty(Page.Request.QueryString["storeId"]))
                    int.TryParse(Page.Request.QueryString["storeId"].ToString(),out storeId);
                this.stroeid.Value = storeId.ToString();
                //根据ID得到门店信息
                StoreInfo storeinfo = StoreInfoHelper.GetStoreInfo(storeId);
                this.txtstorename.Value = storeinfo.storeName;
                this.allherecode.Value = storeinfo.accountALLHere;
                //this.txtstorename.Value = StoreInfoHelper.GetStoreInfoByfgsid(Convert.ToInt32(Page.Request.QueryString["storeId"])).Tables[0].Rows[0]["storeName"].ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRequest.html";
            }
            base.OnInit(e);
        }
    }
}

