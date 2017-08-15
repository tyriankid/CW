namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VQRcode : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litimage;
        private Literal litItemParams;
        private Literal litstorename;
        private Literal liturl;
        private Image litimgorcode;
        private Image image;


        protected override void AttachChildControls()
        {
            this.litimage = (Literal)this.FindControl("litimage");
            this.liturl = (Literal) this.FindControl("liturl");
            this.litstorename = (Literal) this.FindControl("litstorename");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litimgorcode = (Image)this.FindControl("litimgorcode");
            this.image = (Image)this.FindControl("image");
            //头像
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            //获取信息
            DistributorsInfo distributorsInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
            string rzUserId = rzUserId = currentMember.UserId.ToString();//this.Page.Request.QueryString["ReferralId"];//认证门店Id
            if (distributorsInfo != null)//如果当前是分销商或分销商以上的用户
            {
                if (!string.IsNullOrEmpty(distributorsInfo.Logo) && this.image != null)
                {
                    this.image.ImageUrl = Globals.ApplicationPath + distributorsInfo.Logo;
                }
            }
            else
            {
                ///2017-07-03修改,满足店员前端拥有店长功能
                DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMember.UserId);
                if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                {
                    rzUserId = disSalesinfo.DisUserId.ToString();
                }
                else
                    this.Page.Response.Redirect("Index.aspx");
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";

            if (!string.IsNullOrEmpty(rzUserId))//认证门店Id
            {
                //店铺推广码: 设置带参数的固定二维码图片 (作为背景) 
                string savepath = HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("distributor_{0}", rzUserId) + ".jpg";
                if (!File.Exists(savepath))
                {
                    TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("distributor_{0}", rzUserId), false);
                }
                qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("distributor_{0}", rzUserId) + ".jpg";
                litimgorcode.ImageUrl = qrCodeBackImgUrl;

                //快速开店码: 条码背景
                distributorsInfo = DistributorsBrower.GetCurrentDistributors(int.Parse(rzUserId));
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sfsq"]) && this.Page.Request.QueryString["sfsq"] == "1")
                {
                    if (distributorsInfo != null && distributorsInfo.IsAgent == 1)//代理商
                    {
                        qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/ApplicationDescription.aspx?ReferralId=" + rzUserId + "&sfsq=1";
                        litimgorcode.ImageUrl = "/API/GetQRCode.ashx?code=" + qrCodeBackImgUrl;
                    }
                }
            }
            else//总店
            {
                qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/Default.aspx";
                litimgorcode.ImageUrl = "/API/GetQRCode.ashx?code=" + qrCodeBackImgUrl;  
            }

            this.litstorename.Text = (distributorsInfo == null) ? "总店" : distributorsInfo.StoreName;
            PageTitle.AddSiteNameTitle(this.litstorename.Text + "店铺二维码");


            //微信分享的宣传内容
            string str = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopSpreadingCodePic))
            {
                str = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopSpreadingCodePic;
            }
            this.litItemParams.Text = str + "|" + masterSettings.ShopSpreadingCodeName + "|" + masterSettings.ShopSpreadingCodeDescription;
            //URL设置
            if (liturl != null)
            {
                var id = System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if (string.IsNullOrEmpty(rzUserId) && id != null && !string.IsNullOrEmpty(id.ToString()))
                    rzUserId = id.ToString();
                liturl.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/QRcode.aspx?ReferralId=" + rzUserId;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

