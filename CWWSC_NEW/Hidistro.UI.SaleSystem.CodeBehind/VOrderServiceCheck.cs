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
    public class VOrderServiceCheck : VWeiXinOAuthTemplatedWebControl
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
            HtmlInputHidden litOrderId = (HtmlInputHidden)this.FindControl("litOrderId");
            

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(strOrderid);
            if (orderInfo.serviceUserId > 0)
            {
                //加载显示信息
                Literal1.SetWhenIsNotNull(orderInfo.OrderId);
                Literal2.SetWhenIsNotNull(orderInfo.ShipTo);
                Literal3.SetWhenIsNotNull(orderInfo.CellPhone);
                litOrderId.Value = orderInfo.OrderId;//订单编号
            }
            
            PageTitle.AddSiteNameTitle("服务订单核销");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vOrderServiceCheck.html";
            }
            base.OnInit(e);
        }
    }
}

