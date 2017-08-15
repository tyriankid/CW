namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VRefuseServiceSales : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputHidden hiorderid;
        private HtmlInputHidden hiproductid;
        private HtmlInputHidden hiskuid;
        private HtmlInputHidden hiOrderStatus;
        private HtmlInputHidden hiDiscountAmount;
        private HtmlInputHidden hiAdjustedPrice;
        private HtmlInputHidden hiquantity;

        private string orderId;
        private string ProductId;

        protected override void AttachChildControls()
        {
            this.hiorderid = (HtmlInputHidden)this.FindControl("hiorderid");
            this.hiproductid = (HtmlInputHidden)this.FindControl("hiproductid");
            this.hiskuid = (HtmlInputHidden)this.FindControl("hiskuid");
            this.hiOrderStatus = (HtmlInputHidden)this.FindControl("hiOrderStatus");
            this.hiDiscountAmount = (HtmlInputHidden)this.FindControl("hiDiscountAmount");
            this.hiAdjustedPrice = (HtmlInputHidden)this.FindControl("hiAdjustedPrice");
            this.hiquantity = (HtmlInputHidden)this.FindControl("hiquantity");

            //参数
            this.orderId = this.Page.Request.QueryString["orderId"].Trim();
            this.ProductId = this.Page.Request.QueryString["ProductId"].Trim();
            this.hiorderid.Value = this.orderId;
            this.hiproductid.Value = this.ProductId;

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }

            this.hiOrderStatus.Value = ((int)orderInfo.OrderStatus).ToString();
            this.hiDiscountAmount.Value = orderInfo.DiscountAmount.ToString("0.00");

            bool flag = false;
            foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
            {
                if (info2.ProductId.ToString() == this.ProductId)
                {
                    this.hiAdjustedPrice.Value = info2.ItemAdjustedPrice.ToString("0.00");
                    this.hiquantity.Value = info2.Quantity.ToString();
                    this.hiskuid.Value = info2.SkuId;
                    flag = true;
                }
            }
            if (!flag)
            {
                base.GotoResourceNotFound("此订单商品不存在");
            }
            PageTitle.AddSiteNameTitle("拒绝配单");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vrefuseservicesales.html";
            }
            base.OnInit(e);
        }
    }
}


