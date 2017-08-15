namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public class AddbuyOnegetOneFree : AdminPage
    {
        protected Button btnAddBuyOneGetOne;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected ProductCategoriesDropDownList dropCategories;
        protected BuyProductDropDownList buyProduct;
        //protected BuyProductDropDownList GetProduct;
        protected HourDropDownList dropStartHours;
        protected HourDropDownList dropEndHours;
        protected MinuteDropDownList dropStartMinute;
        protected MinuteDropDownList dropEndMinute;
        protected TextBox txtMaxCount;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;
        protected string buyoneId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            buyoneId = this.Page.Request.QueryString["buyoneId"];
            this.btnAddBuyOneGetOne.Click += new EventHandler(this.btnAddBuyOneGetOne_Click);
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
           
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropStartHours.DataBind();
                this.dropEndHours.DataBind();
                this.dropStartMinute.DataBind();
                this.dropEndMinute.DataBind();
                this.buyProduct.DataBind();
                if (!string.IsNullOrEmpty(buyoneId))
                {   
                    bind();//编辑加载                
                }
            }
        }
        protected void bind()
        {
            BuyOneGetOneFreeInfo info = BuyOneGetOnrFreeHelper.GetBuyOneGetOnrFreeInfo(int.Parse(buyoneId));
            this.buyProduct.SelectedValue = new int?(info.productId); ;

            this.calendarEndDate.SelectedDate = new DateTime?(info.endtime.Date);
            this.dropEndHours.SelectedValue = new int?(info.endtime.Hour);
            this.dropEndMinute.SelectedValue = new int?(info.endtime.Minute);

            this.calendarStartDate.SelectedDate = new DateTime?(info.startime.Date);
            this.dropStartHours.SelectedValue = new int?(info.startime.Hour);
            this.dropStartMinute.SelectedValue = new int?(info.startime.Minute);

            this.txtMaxCount.Text = Convert.ToString(info.getNum);
        }
        private void btnAddBuyOneGetOne_Click(object sender, EventArgs e)
        {
            BuyOneGetOneFreeInfo FreeInfo = new BuyOneGetOneFreeInfo();
            string str = string.Empty;
            if (this.buyProduct.SelectedValue > 0)
            {
                if (!string.IsNullOrEmpty(buyoneId))
                {   
                     BuyOneGetOneFreeInfo Freeinfo = BuyOneGetOnrFreeHelper.GetBuyOneGetOnrFreeInfo(int.Parse(buyoneId));
                     if (Freeinfo.productId != this.buyProduct.SelectedValue.Value && BuyOneGetOnrFreeHelper.BuyOneGetOneProductExist(this.buyProduct.SelectedValue.Value))
                     {
                         this.ShowMsg("已经存在此商品的买一送一活动", false);
                         return;
                     }
                }
                else if(BuyOneGetOnrFreeHelper.BuyOneGetOneProductExist(this.buyProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的买一送一活动", false);
                    return;
                }
                FreeInfo.productId = this.buyProduct.SelectedValue.Value;
                FreeInfo.GetProductId = this.buyProduct.SelectedValue.Value;
            }
            FreeInfo.endtime = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.dropEndHours.SelectedValue.Value).AddMinutes((double)this.dropEndMinute.SelectedValue.Value);
            FreeInfo.startime = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value);
            FreeInfo.getNum = int.Parse(this.txtMaxCount.Text);
            if (!string.IsNullOrEmpty(buyoneId))
            {
                FreeInfo.buyoneId = int.Parse(buyoneId);
                if (BuyOneGetOnrFreeHelper.UpdateBuyOneGetOne(FreeInfo))
                {
                    this.ShowMsgAndReUrl("编辑买一送一活动成功", true, "buyOnegetOneFree.aspx");
                }
                else
                {
                    this.ShowMsg("编辑买一送一活动失败", true);
                } 
                
            }
            else
            {
                if (BuyOneGetOnrFreeHelper.AddBuyOneGetOne(FreeInfo))
                {
                    this.ShowMsgAndReUrl("添加买一送一活动成功", true, "buyOnegetOneFree.aspx");
                }
                else
                {
                    this.ShowMsg("添加买一送一活动失败", true);
                }
            }
        }


        #region******** 商品查询************
        private void DoCallback()
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            string str = base.Request.QueryString["action"];
            if (str.Equals("BuyGetOneProducts"))
            {
                ProductQuery query = new ProductQuery();
                if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                {
                    int num;
                    int.TryParse(base.Request.QueryString["categoryId"], out num);
                    if (num > 0)
                    {
                        query.CategoryId = new int?(num);
                        query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
                    }
                }
                string str2 = base.Request.QueryString["sku"];
                string str3 = base.Request.QueryString["productName"];
                query.Keywords = str3;
                query.ProductCode = str2;
                query.SaleStatus = ProductSaleStatus.OnSale;
                DataTable Products = BuyOneGetOnrFreeHelper.GetBuyOneGetOneProducts(query);
                if ((Products == null) || (Products.Rows.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{\"Status\":\"OK\",");
                    builder.AppendFormat("\"Product\":[{0}]", this.GenerateBrandString(Products));
                    builder.Append("}");
                    base.Response.Write(builder.ToString());
                }
            }
            base.Response.End();
        }
        private string GenerateBrandString(DataTable tb)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in tb.Rows)
            {
                builder.Append("{");
                builder.AppendFormat("\"ProductId\":\"{0}\",\"ProductName\":\"{1}\"", row["ProductId"], Uri.EscapeDataString(row["ProductName"].ToString()));
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        #endregion
    }
}

