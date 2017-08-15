using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Order_ItemsList : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
        protected DataList grdOrderGift;//礼品列表
        protected Label lblOrderGifts; //礼品lebel
		protected System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		protected System.Web.UI.WebControls.Literal lblAmoutPrice;
		protected System.Web.UI.WebControls.Literal lblBundlingPrice;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.Literal litWeight;
		private OrderInfo order;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
            /*
            //设置批发价格
            if (this.order.OrderSource == 2)
            {
                DataTable dtPfPrice = new DataTable();
                int buyQuantity = 0;
                if (order.LineItems.Values.Count > 0)
                {
                    foreach (LineItemInfo info in order.LineItems.Values)
                    {
                        dtPfPrice = ProductHelper.GetProductPfPrices(info.ProductId);
                        buyQuantity = info.Quantity;
                        if (dtPfPrice.Rows.Count > 0)
                        {
                            DataRow[] drs = dtPfPrice.Select(string.Format("Num <= {0}", buyQuantity), "Num desc", DataViewRowState.CurrentRows);
                            if (drs.Length > 0)
                            {
                                decimal newprice = 0;
                                if (decimal.TryParse(drs[0]["PFSalePrice"].ToString(), out newprice))
                                {
                                    info.ItemAdjustedPrice = newprice;
                                }
                            }
                        }
                    }
                }
            }*/

			this.dlstOrderItems.DataSource = this.order.LineItems.Values;
			this.dlstOrderItems.DataBind();

            if (this.order.Gifts.Count == 0)
            {
                this.grdOrderGift.Visible = false;
                this.lblOrderGifts.Visible = false;
            }
            else
            {
                this.grdOrderGift.DataSource = this.order.Gifts;
                this.grdOrderGift.DataBind();
            }

            if (this.order.LineItems.Count == 0)
            {
                dlstOrderItems.Visible = false;
            }


			this.litWeight.Text = this.order.Weight.ToString();
			if (this.order.IsReduced)
			{
				this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
				this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
				this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.ReducedPromotionId
				});
			}
			if (this.order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
			}
			this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
		}
	}
}
