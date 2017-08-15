namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrdersService : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater rptOrders;
        private Repeater rptordergifts;//礼品列表

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员订单");
            this.Page.Session["stylestatus"] = "2";//控制底部选中项

            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);


            DistributorSales salesinfo = DistributorSalesHelper.GetSalesBySaleUserId(Globals.GetCurrentMemberUserId());
            if (salesinfo != null && salesinfo.DsID != Guid.Empty)
            {
                OrderQuery query = new OrderQuery();
                switch (result)
                {
                    case 3:
                        query.Status = OrderStatus.SellerAlreadySent;
                        break;
                    case 5:
                        query.Status = OrderStatus.Finished;
                        break;
                }
                query.OrderSource = 3;

                DataSet userOrder = MemberProcessor.GetUserServiceOrder(salesinfo.DsID, query);
                this.rptOrders = (VshopTemplatedRepeater)this.FindControl("rptOrders");
                this.rptOrders.ItemDataBound += new RepeaterItemEventHandler(this.rptOrders_ItemDataBound);
                this.rptOrders.DataSource = userOrder;
                this.rptOrders.DataBind();
            }
            else
                GotoResourceNotFound("");


        }

        private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                
                /*DataSet userOrderGift = GiftProcessor.GetUserOrderGift(Globals.GetCurrentMemberUserId(),);
                this.rptordergifts = (Repeater)e.Item.Controls[0].FindControl("rptordergifts");
                this.rptordergifts.DataSource = userOrderGift;
                this.rptordergifts.DataBind();*/
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrdersService.html";
            }
            base.OnInit(e);
        }
    }
}

