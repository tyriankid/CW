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
    public class VMyOrderSellerService : VWeiXinOAuthTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string strOrderid = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(strOrderid))
            {
                base.GotoResourceNotFound("");
            }

            Literal Literal1 = this.FindControl("Literal1") as Literal;
            Literal Literal2 = this.FindControl("Literal2") as Literal;
            Literal Literal3 = this.FindControl("Literal3") as Literal;
            Literal Literal4 = this.FindControl("Literal4") as Literal;
            Literal Literal5 = this.FindControl("Literal5") as Literal;
            Literal Literal6 = this.FindControl("Literal6") as Literal;

            Literal LiteralUser1 = this.FindControl("LiteralUser1") as Literal;
            Literal LiteralUser2 = this.FindControl("LiteralUser2") as Literal;
            Literal LiteralUser3 = this.FindControl("LiteralUser3") as Literal;
            Literal LiteralUser4 = this.FindControl("LiteralUser4") as Literal;
            

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(strOrderid);
            if (orderInfo.serviceUserId > 0)
            {
                //通过当前用户id查找分销商信息,从而来判断用户类型
                DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(orderInfo.serviceUserId);
                if (currentDistributor.StoreId > 0)
                {
                    StoreInfo storeinfo = StoreInfoHelper.GetStoreInfo(currentDistributor.StoreId);
                    Literal1.SetWhenIsNotNull(storeinfo.storeName);
                    Literal2.SetWhenIsNotNull(storeinfo.storeRelationPerson);
                    Literal3.SetWhenIsNotNull(storeinfo.storeRelationCell);
                    Literal4.SetWhenIsNotNull(currentDistributor.Location_cityname);
                    Literal5.SetWhenIsNotNull(currentDistributor.Location_poiname);
                    Literal6.SetWhenIsNotNull(currentDistributor.Location_poiaddress);
                }

                //加载客信息
                LiteralUser1.SetWhenIsNotNull(orderInfo.ShippingRegion);
                LiteralUser2.SetWhenIsNotNull(orderInfo.Address);
                LiteralUser3.SetWhenIsNotNull(orderInfo.ShipTo);
                LiteralUser4.SetWhenIsNotNull(orderInfo.CellPhone);
            }
            
            PageTitle.AddSiteNameTitle("服务订单售后");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyOrderSellerService.html";
            }
            base.OnInit(e);
        }
    }
}

