namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrders : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptOrders;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员订单");
            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
            OrderQuery query = new OrderQuery();
            switch (result)
            {
                case 3:
                    query.Status = OrderStatus.WaitBuyerPay;
                    break;

                case 5:
                    query.Status = OrderStatus.SellerAlreadySent;
                    break;
            }

            DataSet userOrder = MemberProcessor.GetUserOrder(Globals.GetCurrentMemberUserId(), query);
            this.rptOrders = (VshopTemplatedRepeater) this.FindControl("rptOrders");
            this.rptOrders.ItemDataBound += new RepeaterItemEventHandler(this.rptOrders_ItemDataBound);
            this.rptOrders.DataSource = userOrder;
            this.rptOrders.DataBind();
            /*
            string orderids=string.Empty;
            foreach(DataRow dr in userOrder.Tables[0].Rows)
                orderids+=string.Format("{0},",dr["orderid"].ToString());
            orderids=orderids.TrimEnd(',');
            */
            


        }

        private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            //{
                /*DataSet userOrderGift = GiftProcessor.GetUserOrderGift(Globals.GetCurrentMemberUserId(),);
                this.rptordergifts = (Repeater)e.Item.Controls[0].FindControl("rptordergifts");
                this.rptordergifts.DataSource = userOrderGift;
                this.rptordergifts.DataBind();*/
            //}

            DataTable table = ((DataView)DataBinder.Eval(e.Item.DataItem, "OrderItems")).ToTable();
            //增加判断,如果table有数据,才会开始转换
            if (table.Rows.Count > 0)
            {
                DataRow drOrder = ((DataRowView)e.Item.DataItem).Row;
                //decimal num = (decimal)table.Compute("sum(ItemAdjustedCommssion)", "true");
                //decimal num2 = (decimal)table.Compute("sum(itemsCommission)", "true");
                //FormatedMoneyLabel label = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lbladjustsum");
                //DataRow drOrder = ((DataRowView)e.Item.DataItem).Row;
                //label.Text = num.ToString("F2");
                //Literal literal = (Literal)e.Item.Controls[0].FindControl("litCommission");
                //if (CustomConfigHelper.Instance.SelectServerAgent)
                //{
                //    literal.Text = (10 / 100M * Convert.ToDecimal(drOrder["Amount"].ToString())).ToString("F2");
                //}
                //else
                //{
                //    literal.Text = (num2 - num).ToString("F2");
                //}

                Literal litButtonsHtml = (Literal)e.Item.Controls[0].FindControl("litButtonsHtml");
                StringBuilder stringhtml = new StringBuilder();
                if (drOrder["OrderSource"].ToString() == "3")
                { 
                    stringhtml.AppendFormat("<em class=\"red\">{0}</em>",OrderInfo.GetOrderStatusNameService((OrderStatus)drOrder["OrderStatus"]));
                    stringhtml.AppendFormat("<a href='{0}/Vshop/MyOrderSellerService.aspx?OrderId={1}' class='link {2}'>售后</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) > 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='javascript:void(0)'onclick=\"CloseOrder('{0}')\" class='link {1}'>关闭订单</a>", drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='{0}/Vshop/LookServiceSales.aspx?OrderId={1}&OrderStatus={2}' class='link {3}'>查看配单信息</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), drOrder["OrderStatus"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 3 || int.Parse(drOrder["OrderStatus"].ToString()) == 88 || int.Parse(drOrder["OrderStatus"].ToString()) == 5 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='javascript:void(0)' onclick=\"FinishServiceOrder('{0}')\" class='link {1}'>确认上门</a>", drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 88 ? "link link-color" : "hide");
                }
                else if (drOrder["OrderSource"].ToString() == "5")
                {
                    stringhtml.AppendFormat("<em class=\"red\">{0}</em>", OrderInfo.GetOrderStatusName((OrderStatus)drOrder["OrderStatus"]));
                    stringhtml.AppendFormat("<a href='{0}/Vshop/MyOrderSeller.aspx?OrderId={1}' class='link {2}'>售后</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) > 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='javascript:void(0)'onclick=\"CloseOrder('{0}')\" class='link {1}'>关闭订单</a>", drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='{0}/Vshop/LookVirtualCode.aspx?OrderId={1}&OrderStatus={2}' class='link {3}'>查看虚拟码</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), drOrder["OrderStatus"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 3 || int.Parse(drOrder["OrderStatus"].ToString()) == 88 || int.Parse(drOrder["OrderStatus"].ToString()) == 5 ? "link" : "hide");
                }
                else
                {
                    stringhtml.AppendFormat("<em class=\"red\">{0}</em>", OrderInfo.GetOrderStatusName((OrderStatus)drOrder["OrderStatus"]));
                    stringhtml.AppendFormat("<a href='{0}/Vshop/MyOrderSeller.aspx?OrderId={1}' class='link {2}'>售后</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) > 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='javascript:void(0)'onclick=\"CloseOrder('{0}')\" class='link {1}'>关闭订单</a>", drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 1 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='{0}/Vshop/MyLogistics.aspx?OrderId={1}' class='link {2}'>查看物流</a>", Globals.ApplicationPath, drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 3 || int.Parse(drOrder["OrderStatus"].ToString()) == 5 ? "link" : "hide");
                    stringhtml.AppendFormat("<a href='javascript:void(0)' onclick=\"FinishOrder('{0}')\" class='link {1}'>确认收货</a>", drOrder["OrderId"].ToString(), int.Parse(drOrder["OrderStatus"].ToString()) == 3 ? "link link-color" : "hide");
                }
                litButtonsHtml.Text = stringhtml.ToString();
                //if (drOrder["OrderStatus"].ToString() == "2")
                //{
                //    if (table.Rows[0]["OrderItemsStatus"].ToString() == "2")
                //    {
                //        litSendOrderGoods.Visible = true;
                //        litSendOrderGoods.Text = string.Format("<a class='link link-color' role='button' onclick=ServiceOrderToSales('{0}','{1}','{2}','{3}','{4}','{5}')>配单</a>",
                //            Globals.ApplicationPath, drOrder["OrderId"].ToString(), drOrder["serviceUserId"].ToString(), drOrder["UserName"].ToString(), drOrder["OrderTotal"].ToString(), table.Rows[0]["ProductId"].ToString());
                //    }
                //    else if (table.Rows[0]["OrderItemsStatus"].ToString() == "6")
                //    {
                //        litSendOrderGoods.Visible = true;
                //        litSendOrderGoods.Text = string.Format("<span><a class='link link-color' role='button' href='{0}/Vshop/LookServiceSales.aspx?OrderId={1}'>正在退款</a></span>", Globals.ApplicationPath, drOrder["OrderId"].ToString());
                //    }
                //}
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrders.html";
            }
            base.OnInit(e);
        }
    }
}

