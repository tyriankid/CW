namespace Hidistro.Core.Entities
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class SiteSettings
    {
        public SiteSettings(string siteUrl)
        {
            this.SiteUrl = siteUrl;
            this.Theme = "default";
            this.VTheme = "default";
            this.AliOHTheme = "default";//alioh
            this.Disabled = false;
            this.SiteName = "众赞移动云商平台1.5正式版";
            this.LogoUrl = "/utility/pics/logo.jpg";
            this.DefaultProductImage = "/utility/pics/none.gif";
            this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
            this.WeiXinCodeImageUrl = "/Storage/master/WeiXinCodeImageUrl.jpg";
            this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
            this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
            this.VipCardPrefix = "100000";
            this.VipRequireName = true;
            this.VipRequireMobile = true;
            this.EnablePodRequest = true;
            this.EnableProfit = false;
            this.DecimalLength = 2;
            this.PointsRate = 1M;
            this.PointsRatePt = 1M;
            this.BuyOrGive = false;
            this.DistributorUpgradeType = "byComm";//默认为按佣金升级
            this.SpecialOrderAmountType = "default";//默认为default,没有任何特殊规则
            this.SpecialValue1 = 0M;//特殊规则值1
            this.SpecialValue2 = 0M;//特殊规则值1
            this.isFlowWindowsOn = false;//主页客服和回到顶部悬浮窗
            this.isCloseStore = false;//门店是否打烊 默认不开启
            this.OrderShowDays = 7;
            this.CloseOrderDays = 3;
            this.FinishOrderDays = 14;
            this.TakeGoodsDays = 7;
            this.OpenManyService = false;
            this.EnableStoreProductAuto = false;
            this.EnableAgentProductRange = false;
            this.EnableProductReviews = true;
            this.EnableStoreInfoSet = true;
            this.EnableAliOHBankUnionPay = false;//alioh
            this.EnableQuickPay = false;//快速收银,默认不开启
            this.DistributorCutOff = "default";//分销商特殊优惠,默认为默认(无优惠)
            this.EnableOrderRemind = false;
            this.OrderRemindTime = "";
            this.SupplierRoleId = 6;//供应商角色默认为6
            this.StoreRoleId = 7;//门店角色默认为7
            this.FilialeRoleId = 8;//门店角色默认为8
            this.DefaultALLHereCode = "DZ160159";//门店默认账号（主店购买接口调用该账号）
            //创维门店评分系统
            this.OrderNum ="";
            this.StoreCommon ="";
            this.OnlineSale ="";
            this.JinliSale ="";
            this.MemberNum ="";
            this.NXmemberNum ="";
            this.CurrentDomain = "";
            this.VshopMemberCookieStr = 0;
            this.ServiceProductType = 21;//服务商品默认所属类型

        }

        public static SiteSettings FromXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("Settings");
            return new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText)
            {
                Theme = node.SelectSingleNode("Theme").InnerText,
                VTheme = node.SelectSingleNode("VTheme").InnerText,
                DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText),
                DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText,
                DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText,
                DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText,
                DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText,
                DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText,
                DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText,
                DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText,
                DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText,
                DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText,
                CheckCode = node.SelectSingleNode("CheckCode").InnerText,
                Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText),
                Footer = node.SelectSingleNode("Footer").InnerText,
                RegisterAgreement = node.SelectSingleNode("RegisterAgreement").InnerText,
                LogoUrl = node.SelectSingleNode("LogoUrl").InnerText,
                OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText),
                CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText),
                FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText),
                TakeGoodsDays = int.Parse(node.SelectSingleNode("TakeGoodsDays").InnerText),
                TaxRate = decimal.Parse(node.SelectSingleNode("TaxRate").InnerText),
                PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText),
                PointsRatePt = decimal.Parse(node.SelectSingleNode("PointsRatePt").InnerText),
                BuyOrGive = (node.SelectSingleNode("BuyOrGive")) == null ? false : bool.Parse(node.SelectSingleNode("BuyOrGive").InnerText),
                DistributorUpgradeType = (node.SelectSingleNode("DistributorUpgradeType")) == null ? "byComm" : node.SelectSingleNode("DistributorUpgradeType").InnerText,
                SpecialOrderAmountType = (node.SelectSingleNode("SpecialOrderAmountType")) == null ? "default" : node.SelectSingleNode("SpecialOrderAmountType").InnerText,
                SpecialValue1 = (node.SelectSingleNode("SpecialValue1")) == null ? 0M : decimal.Parse(node.SelectSingleNode("SpecialValue1").InnerText),
                SpecialValue2 = (node.SelectSingleNode("SpecialValue2")) == null ? 0M : decimal.Parse(node.SelectSingleNode("SpecialValue2").InnerText),
                isFlowWindowsOn = (node.SelectSingleNode("isFlowWindowsOn")) == null ? false : bool.Parse(node.SelectSingleNode("isFlowWindowsOn").InnerText),
                isCloseStore = (node.SelectSingleNode("isCloseStore")) == null ? false : bool.Parse(node.SelectSingleNode("isCloseStore").InnerText),
                SiteName = node.SelectSingleNode("SiteName").InnerText,
                SiteUrl = node.SelectSingleNode("SiteUrl").InnerText,
                YourPriceName = node.SelectSingleNode("YourPriceName").InnerText,
                EmailSender = node.SelectSingleNode("EmailSender").InnerText,
                EmailSettings = node.SelectSingleNode("EmailSettings").InnerText,
                SMSSender = node.SelectSingleNode("SMSSender").InnerText,
                SMSSettings = node.SelectSingleNode("SMSSettings").InnerText,
                EnabledCnzz = bool.Parse(node.SelectSingleNode("EnabledCnzz").InnerText),
                CnzzUsername = node.SelectSingleNode("CnzzUsername").InnerText,
                CnzzPassword = node.SelectSingleNode("CnzzPassword").InnerText,
                WeixinAppId = node.SelectSingleNode("WeixinAppId").InnerText,
                WeixinAppSecret = node.SelectSingleNode("WeixinAppSecret").InnerText,
                WeixinPaySignKey = node.SelectSingleNode("WeixinPaySignKey").InnerText,
                WeixinPartnerID = node.SelectSingleNode("WeixinPartnerID").InnerText,
                WeixinPartnerKey = node.SelectSingleNode("WeixinPartnerKey").InnerText,
                IsValidationService = bool.Parse(node.SelectSingleNode("IsValidationService").InnerText),
                WeixinToken = node.SelectSingleNode("WeixinToken").InnerText,
                WeixinNumber = node.SelectSingleNode("WeixinNumber").InnerText,
                WeixinLoginUrl = node.SelectSingleNode("WeixinLoginUrl").InnerText,
                WeiXinCodeImageUrl = node.SelectSingleNode("WeiXinCodeImageUrl").InnerText,
                VipCardLogo = node.SelectSingleNode("VipCardLogo").InnerText,
                VipCardBG = node.SelectSingleNode("VipCardBG").InnerText,
                VipCardQR = node.SelectSingleNode("VipCardQR").InnerText,
                VipCardName = node.SelectSingleNode("VipCardName").InnerText,
                VipCardPrefix = node.SelectSingleNode("VipCardPrefix").InnerText,
                VipRequireName = bool.Parse(node.SelectSingleNode("VipRequireName").InnerText),
                VipRequireMobile = bool.Parse(node.SelectSingleNode("VipRequireMobile").InnerText),
                VipRequireAdress = bool.Parse(node.SelectSingleNode("VipRequireAdress").InnerText),
                VipRequireQQ = bool.Parse(node.SelectSingleNode("VipRequireQQ").InnerText),
                VipEnableCoupon = bool.Parse(node.SelectSingleNode("VipEnableCoupon").InnerText),
                VipRemark = node.SelectSingleNode("VipRemark").InnerText,
                EnablePodRequest = bool.Parse(node.SelectSingleNode("EnablePodRequest").InnerText)
                ,
                EnableCommission = bool.Parse(node.SelectSingleNode("EnableCommission").InnerText)
                ,
                EnableProfit = bool.Parse(node.SelectSingleNode("EnableProfit").InnerText)
                ,
                EnableAlipayRequest = bool.Parse(node.SelectSingleNode("EnableAlipayRequest").InnerText),
                EnableWeiXinRequest = bool.Parse(node.SelectSingleNode("EnableWeiXinRequest").InnerText),
                EnableOffLineRequest = bool.Parse(node.SelectSingleNode("EnableOffLineRequest").InnerText),
                EnableWapShengPay = bool.Parse(node.SelectSingleNode("EnableWapShengPay").InnerText),
                OffLinePayContent = node.SelectSingleNode("OffLinePayContent").InnerText,
                DistributorDescription = node.SelectSingleNode("DistributorDescription").InnerText,
                DistributorBackgroundPic = node.SelectSingleNode("DistributorBackgroundPic").InnerText,
                DistributorLogoPic = node.SelectSingleNode("DistributorLogoPic").InnerText,
                SaleService = node.SelectSingleNode("SaleService").InnerText,
                MentionNowMoney = node.SelectSingleNode("MentionNowMoney").InnerText,
                ShopIntroduction = node.SelectSingleNode("ShopIntroduction").InnerText,
                ApplicationDescription = node.SelectSingleNode("ApplicationDescription").InnerText,
                GuidePageSet = node.SelectSingleNode("GuidePageSet").InnerText,
                ManageOpenID = node.SelectSingleNode("ManageOpenID").InnerText,
                WeixinCertPath = node.SelectSingleNode("WeixinCertPath").InnerText,
                WeixinCertPassword = node.SelectSingleNode("WeixinCertPassword").InnerText,
                GoodsPic = node.SelectSingleNode("GoodsPic").InnerText,
                GoodsName = node.SelectSingleNode("GoodsName").InnerText,
                GoodsDescription = node.SelectSingleNode("GoodsDescription").InnerText,
                ShopHomePic = node.SelectSingleNode("ShopHomePic").InnerText,
                ShopHomeName = node.SelectSingleNode("ShopHomeName").InnerText,
                ShopHomeDescription = node.SelectSingleNode("ShopHomeDescription").InnerText,
                ShopSpreadingCodePic = node.SelectSingleNode("ShopSpreadingCodePic").InnerText,
                ShopSpreadingCodeName = node.SelectSingleNode("ShopSpreadingCodeName").InnerText,
                ShopSpreadingCodeDescription = node.SelectSingleNode("ShopSpreadingCodeDescription").InnerText,
                OpenManyService = bool.Parse(node.SelectSingleNode("OpenManyService").InnerText),
                IsRequestDistributor = bool.Parse(node.SelectSingleNode("IsRequestDistributor").InnerText),
                FinishedOrderMoney = int.Parse(node.SelectSingleNode("FinishedOrderMoney").InnerText),
                RegisterDistributorsPoints = int.Parse(node.SelectSingleNode("RegisterDistributorsPoints").InnerText),
                EnableStoreProductAuto = bool.Parse(node.SelectSingleNode("OpenStoreProductAuto").InnerText),
                EnableAgentProductRange = bool.Parse(node.SelectSingleNode("OpenAgentProductRange").InnerText),
                EnableProductReviews = bool.Parse(node.SelectSingleNode("OpenProductReviews").InnerText),
                EnableStoreInfoSet = (node.SelectSingleNode("OpenStoreInfoSet")) == null ? true : (bool.Parse(node.SelectSingleNode("OpenStoreInfoSet").InnerText)),

                AliOHAppId = (node.SelectSingleNode("AliOHAppId")) == null ? "" : (node.SelectSingleNode("AliOHAppId").InnerText),
                EnableAliOHAliPay = (node.SelectSingleNode("EnableAliOHAliPay")) == null ? false : bool.Parse(node.SelectSingleNode("EnableAliOHAliPay").InnerText),
                AliOHTheme = node.SelectSingleNode("AliOHTheme") == null ? "default" : node.SelectSingleNode("AliOHTheme").InnerText,
                EnableAliOHOffLinePay = node.SelectSingleNode("EnableAliOHOffLinePay") == null ? false : bool.Parse(node.SelectSingleNode("EnableAliOHOffLinePay").InnerText),
                EnableAliOHPodPay = node.SelectSingleNode("EnableAliOHPodPay") == null ? false : bool.Parse(node.SelectSingleNode("EnableAliOHPodPay").InnerText),
                AliOHFollowRelay = node.SelectSingleNode("AliOHFollowRelay") == null ? "" : node.SelectSingleNode("AliOHFollowRelay").InnerText,
                AliOHServerUrl = node.SelectSingleNode("AliOHServerUrl") == null ? "" : node.SelectSingleNode("AliOHServerUrl").InnerText,
                EnableAliOHShengPay = node.SelectSingleNode("EnableAliOHShengPay") == null ? false : bool.Parse(node.SelectSingleNode("EnableAliOHShengPay").InnerText),
                AliOHFollowRelayTitle = node.SelectSingleNode("AliOHFollowRelayTitle") == null ? "" : node.SelectSingleNode("AliOHFollowRelayTitle").InnerText,
                EnableAliOHBankUnionPay = node.SelectSingleNode("EnableAliOHBankUnionPay") == null ? false : bool.Parse(node.SelectSingleNode("EnableAliOHBankUnionPay").InnerText),

                EnableQuickPay = (node.SelectSingleNode("EnableQuickPay")) == null ? false : (bool.Parse(node.SelectSingleNode("EnableQuickPay").InnerText)),
                DistributorCutOff = (node.SelectSingleNode("DistributorCutOff")) == null ? "default" : node.SelectSingleNode("DistributorCutOff").InnerText,

                EnableOrderRemind = (node.SelectSingleNode("EnableOrderRemind")) == null ? false : bool.Parse(node.SelectSingleNode("EnableOrderRemind").InnerText),
                OrderRemindTime = (node.SelectSingleNode("OrderRemindTime")) == null ? "" : node.SelectSingleNode("OrderRemindTime").InnerText,
                SupplierRoleId = int.Parse(node.SelectSingleNode("SupplierRoleId").InnerText),
                StoreRoleId = int.Parse(node.SelectSingleNode("StoreRoleId").InnerText),
                FilialeRoleId = int.Parse(node.SelectSingleNode("FilialeRoleId").InnerText),
                DefaultALLHereCode = (node.SelectSingleNode("DefaultALLHereCode")) == null ? "" : node.SelectSingleNode("DefaultALLHereCode").InnerText,
                //创维门店评分权重分值
                OrderNum =node.SelectSingleNode("OrderNum")==null?"":node.SelectSingleNode("OrderNum").InnerText,
                StoreCommon =node.SelectSingleNode("StoreCommon")==null?"": node.SelectSingleNode("StoreCommon").InnerText,
                OnlineSale =node.SelectSingleNode("OnlineSale")==null?"":node.SelectSingleNode("OnlineSale").InnerText,
                JinliSale =node.SelectSingleNode("JinliSale")==null?"":node.SelectSingleNode("JinliSale").InnerText,
                MemberNum =node.SelectSingleNode("MemberNum")==null?"":node.SelectSingleNode("MemberNum").InnerText,
                NXmemberNum =node.SelectSingleNode("NXmemberNum")==null?"":node.SelectSingleNode("NXmemberNum").InnerText,
                //服务商品默认所属类型
                ServiceProductType = int.Parse(node.SelectSingleNode("ServiceProductType").InnerText),
                CurrentDomain = node.SelectSingleNode("CurrentDomain").InnerText,
                VshopMemberCookieStr = int.Parse(node.SelectSingleNode("VshopMemberCookieStr").InnerText),
                

            };
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
            SetNodeValue(doc, root, "Theme", this.Theme);
            SetNodeValue(doc, root, "VTheme", this.VTheme);
            SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
            SetNodeValue(doc, root, "CheckCode", this.CheckCode);
            SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
            SetNodeValue(doc, root, "Footer", this.Footer);
            SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
            SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TakeGoodsDays", this.TakeGoodsDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
            SetNodeValue(doc, root, "PointsRatePt", this.PointsRatePt.ToString("F"));
            SetNodeValue(doc, root, "BuyOrGive", this.BuyOrGive ? "true" : "false");
            SetNodeValue(doc, root, "DistributorUpgradeType", this.DistributorUpgradeType);
            SetNodeValue(doc, root, "SpecialOrderAmountType", this.SpecialOrderAmountType.ToString());
            SetNodeValue(doc, root, "SpecialValue1", this.SpecialValue1.ToString());
            SetNodeValue(doc, root, "SpecialValue2", this.SpecialValue2.ToString());
            SetNodeValue(doc, root, "isFlowWindowsOn", this.isFlowWindowsOn.ToString());
            SetNodeValue(doc, root, "isCloseStore", this.isCloseStore.ToString());
            SetNodeValue(doc, root, "SiteName", this.SiteName);
            SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
            SetNodeValue(doc, root, "EmailSender", this.EmailSender);
            SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
            SetNodeValue(doc, root, "SMSSender", this.SMSSender);
            SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
            SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
            SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
            SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
            SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
            SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
            SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
            SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
            SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
            SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
            SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
            SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
            SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
            SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
            SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
            SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
            SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
            SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
            SetNodeValue(doc, root, "VipCardName", this.VipCardName);
            SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
            SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
            SetNodeValue(doc, root, "VipRemark", this.VipRemark);
            SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableCommission", this.EnableCommission ? "true" : "false");
            SetNodeValue(doc, root, "EnableProfit", this.EnableProfit ? "true" : "false");
            SetNodeValue(doc, root, "EnableAlipayRequest", this.EnableAlipayRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
            SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
            SetNodeValue(doc, root, "DistributorDescription", this.DistributorDescription);
            SetNodeValue(doc, root, "DistributorBackgroundPic", this.DistributorBackgroundPic);
            SetNodeValue(doc, root, "DistributorLogoPic", this.DistributorLogoPic);
            SetNodeValue(doc, root, "SaleService", this.SaleService);
            SetNodeValue(doc, root, "MentionNowMoney", this.MentionNowMoney);
            SetNodeValue(doc, root, "ShopIntroduction", this.ShopIntroduction);
            SetNodeValue(doc, root, "ApplicationDescription", this.ApplicationDescription);
            SetNodeValue(doc, root, "GuidePageSet", this.GuidePageSet);
            SetNodeValue(doc, root, "ManageOpenID", this.ManageOpenID);
            SetNodeValue(doc, root, "WeixinCertPath", this.WeixinCertPath);
            SetNodeValue(doc, root, "WeixinCertPassword", this.WeixinCertPassword);
            SetNodeValue(doc, root, "GoodsPic", this.GoodsPic);
            SetNodeValue(doc, root, "GoodsName", this.GoodsName);
            SetNodeValue(doc, root, "GoodsDescription", this.GoodsDescription);
            SetNodeValue(doc, root, "ShopHomePic", this.ShopHomePic);
            SetNodeValue(doc, root, "ShopHomeName", this.ShopHomeName);
            SetNodeValue(doc, root, "ShopHomeDescription", this.ShopHomeDescription);
            SetNodeValue(doc, root, "ShopSpreadingCodePic", this.ShopSpreadingCodePic);
            SetNodeValue(doc, root, "ShopSpreadingCodeName", this.ShopSpreadingCodeName);
            SetNodeValue(doc, root, "ShopSpreadingCodeDescription", this.ShopSpreadingCodeDescription);
            SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
            SetNodeValue(doc, root, "IsRequestDistributor", this.IsRequestDistributor ? "true" : "false");
            SetNodeValue(doc, root, "FinishedOrderMoney", this.FinishedOrderMoney.ToString());
            SetNodeValue(doc, root, "RegisterDistributorsPoints", this.RegisterDistributorsPoints.ToString());
            SetNodeValue(doc, root, "OrdersPoints", this.OrdersPoints.ToString());
            SetNodeValue(doc, root, "OpenStoreProductAuto", this.EnableStoreProductAuto.ToString());
            SetNodeValue(doc, root, "OpenAgentProductRange", this.EnableAgentProductRange.ToString());
            SetNodeValue(doc, root, "OpenProductReviews", this.EnableProductReviews.ToString());
            SetNodeValue(doc, root, "OpenStoreInfoSet", this.EnableStoreInfoSet.ToString());

            SetNodeValue(doc, root, "AliOHAppId", this.AliOHAppId);
            SetNodeValue(doc, root, "EnableAliOHOffLinePay", this.EnableAliOHOffLinePay ? "true" : "false");
            SetNodeValue(doc, root, "AliOHTheme", this.AliOHTheme);
            SetNodeValue(doc, root, "EnableAliOHAliPay", this.EnableAliOHAliPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAliOHPodPay", this.EnableAliOHPodPay ? "true" : "false");
            SetNodeValue(doc, root, "AliOHFollowRelay", this.AliOHFollowRelay);
            SetNodeValue(doc, root, "AliOHFollowRelayTitle", this.AliOHFollowRelayTitle);
            SetNodeValue(doc, root, "AliOHServerUrl", this.AliOHServerUrl);
            SetNodeValue(doc, root, "EnableAliOHShengPay", this.EnableAliOHShengPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAliOHBankUnionPay", this.EnableAliOHBankUnionPay ? "true" : "false");

            SetNodeValue(doc, root, "EnableQuickPay", this.EnableQuickPay.ToString());
            SetNodeValue(doc, root, "DistributorCutOff", this.DistributorCutOff.ToString());

            SetNodeValue(doc, root, "EnableOrderRemind", this.EnableOrderRemind ? "true" : "false");
            SetNodeValue(doc, root, "OrderRemindTime", this.OrderRemindTime);
            SetNodeValue(doc, root, "SupplierRoleId", this.SupplierRoleId.ToString());
            SetNodeValue(doc, root, "StoreRoleId", this.StoreRoleId.ToString());
            SetNodeValue(doc, root, "FilialeRoleId", this.FilialeRoleId.ToString());
            SetNodeValue(doc, root, "DefaultALLHereCode", this.DefaultALLHereCode);
            //创维门店评分权重分值
            SetNodeValue(doc, root, "OrderNum", this.OrderNum);
            SetNodeValue(doc, root, "StoreCommon", this.StoreCommon);
            SetNodeValue(doc, root, "OnlineSale", this.OnlineSale);
            SetNodeValue(doc, root, "JinliSale", this.JinliSale);
            SetNodeValue(doc, root, "MemberNum", this.MemberNum);
            SetNodeValue(doc, root, "NXmemberNum", this.NXmemberNum);
            //服务商品默认所属类型
            SetNodeValue(doc, root, "ServiceProductType", this.ServiceProductType.ToString()); 
            SetNodeValue(doc, root, "CurrentDomain", this.CurrentDomain.ToString()); 

            SetNodeValue(doc, root, "VshopMemberCookieStr", this.VshopMemberCookieStr.ToString());
        }

        public string ApplicationDescription { get; set; }

        public string CheckCode { get; set; }

        public int CloseOrderDays { get; set; }

        public string CnzzPassword { get; set; }

        public string CnzzUsername { get; set; }

        public int DecimalLength { get; set; }

        public string DefaultProductImage { get; set; }

        public string DefaultProductThumbnail1 { get; set; }

        public string DefaultProductThumbnail2 { get; set; }

        public string DefaultProductThumbnail3 { get; set; }

        public string DefaultProductThumbnail4 { get; set; }

        public string DefaultProductThumbnail5 { get; set; }

        public string DefaultProductThumbnail6 { get; set; }

        public string DefaultProductThumbnail7 { get; set; }

        public string DefaultProductThumbnail8 { get; set; }

        public bool Disabled { get; set; }

        public string DistributorBackgroundPic { get; set; }

        public string DistributorDescription { get; set; }

        public string DistributorLogoPic { get; set; }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings)) && (this.EmailSender.Trim().Length > 0)) && (this.EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender { get; set; }

        public string EmailSettings { get; set; }

        public bool EnableAlipayRequest { get; set; }

        public bool EnableCommission { get; set; }

        public bool EnableProfit { get; set; }

        public bool EnabledCnzz { get; set; }

        public bool EnableOffLineRequest { get; set; }

        public bool EnablePodRequest { get; set; }

        public bool EnableWapShengPay { get; set; }

        public bool EnableWeiXinRequest { get; set; }

        public int FinishedOrderMoney { get; set; }

        public int FinishOrderDays { get; set; }
        //收获后允许退货申请天数
        public int TakeGoodsDays { get; set; }

        public string Footer { get; set; }

        public string GoodsDescription { get; set; }

        public string GoodsName { get; set; }

        public string GoodsPic { get; set; }

        public string GuidePageSet { get; set; }

        public bool IsRequestDistributor { get; set; }

        public bool IsValidationService { get; set; }

        public string LogoUrl { get; set; }

        public string ManageOpenID { get; set; }

        public string MentionNowMoney { get; set; }

        public string OffLinePayContent { get; set; }

        public bool OpenManyService { get; set; }

        public int OrderShowDays { get; set; }

        public int OrdersPoints { get; set; }
        //店长积分比例
        public decimal PointsRate { get; set; }
        //普通会员积分比例
        public decimal PointsRatePt { get; set; }

        public bool BuyOrGive { get; set; }

        public string RegisterAgreement { get; set; }

        public int RegisterDistributorsPoints { get; set; }

        public string SaleService { get; set; }

        public string ShopHomeDescription { get; set; }

        public string ShopHomeName { get; set; }

        public string ShopHomePic { get; set; }

        public string ShopIntroduction { get; set; }

        public string ShopSpreadingCodeDescription { get; set; }

        public string ShopSpreadingCodeName { get; set; }

        public string ShopSpreadingCodePic { get; set; }

        public string SiteName { get; set; }

        public string SiteUrl { get; set; }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings)) && (this.SMSSender.Trim().Length > 0)) && (this.SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender { get; set; }

        public string SMSSettings { get; set; }

        public decimal TaxRate { get; set; }

        public string Theme { get; set; }

        public string VipCardBG { get; set; }

        public string VipCardLogo { get; set; }

        public string VipCardName { get; set; }

        public string VipCardPrefix { get; set; }

        public string VipCardQR { get; set; }

        public bool VipEnableCoupon { get; set; }

        public string VipRemark { get; set; }

        public bool VipRequireAdress { get; set; }

        public bool VipRequireMobile { get; set; }

        public bool VipRequireName { get; set; }

        public bool VipRequireQQ { get; set; }

        public string VTheme { get; set; }

        public string WeixinAppId { get; set; }

        public string WeixinAppSecret { get; set; }

        public string WeixinCertPassword { get; set; }

        public string WeixinCertPath { get; set; }

        public string WeiXinCodeImageUrl { get; set; }

        public string WeixinLoginUrl { get; set; }

        public string WeixinNumber { get; set; }

        public string WeixinPartnerID { get; set; }

        public string WeixinPartnerKey { get; set; }

        public string WeixinPaySignKey { get; set; }

        public string WeixinToken { get; set; }

        public string YourPriceName { get; set; }

        public bool EnableStoreProductAuto { get; set; }
        public bool EnableAgentProductRange { get; set; }

        public bool EnableProductReviews { get; set; }

        public bool EnableStoreInfoSet { get; set; }
        #region 服务窗
        //服务窗
        public string AliOHAppId { get; set; }

        public string AliOHFollowRelay { get; set; }

        public string AliOHFollowRelayTitle { get; set; }

        public string AliOHServerUrl { get; set; }

        public string AliOHTheme { get; set; }

        public bool EnableAliOHAliPay { get; set; }

        public bool EnableAliOHBankUnionPay { get; set; }

        public bool EnableAliOHOffLinePay { get; set; }

        public bool EnableAliOHPodPay { get; set; }

        public bool EnableAliOHShengPay { get; set; }
        #endregion

        //快速收银
        public bool EnableQuickPay { get; set; }

        //分销商特殊优惠
        public string DistributorCutOff { get; set; }

        //订单提醒设置
        public bool EnableOrderRemind { get; set; }
        public string OrderRemindTime { get; set; }

        //分销商升级规则(按佣金:byComm;按销售额:byOrdersTotal. 默认为按佣金)
        public string DistributorUpgradeType { get; set; }

        //订单邮费特殊规则
        public string SpecialOrderAmountType { get; set; }
        //特殊规则金额1
        public decimal SpecialValue1 { get; set; }
        public decimal SpecialValue2 { get; set; }
        //主页客服和回到顶部悬浮窗是否开启
        public bool isFlowWindowsOn { get; set; }
        //是否打烊
        public bool isCloseStore { get; set; }
        //供应商角色ID
        public int SupplierRoleId { get; set; }
        //门店角色ID
        public int StoreRoleId { get; set; }
        //分公司角色ID
        public int FilialeRoleId { get; set; }
        //默认门店All账号
        public string DefaultALLHereCode { get; set; }
        //创维门店评分权重分值
        public string  StoreCommon { get; set; }//门店评价
        public string OnlineSale { get; set; }//在线销售
        public string JinliSale { get; set; }//金力销售
        public string MemberNum { get; set; }//用户数量
        public string NXmemberNum { get; set; }//粘性会员数
        public string OrderNum { get; set; }//服务订单数

        public int ServiceProductType { get; set; }//服务商品默认所属类型

        public string CurrentDomain { get; set; } //当前域名

        public int VshopMemberCookieStr { get; set; } //当前member的cookie
    }
}

