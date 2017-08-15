using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using Hidistro.Core.Entities;
using Hidistro.Core;
using Hidistro.Core.Enums;
using System.Collections.Specialized;



namespace Hidistro.UI.Web.Admin.product
{
    public partial class ProductReservePrice : AdminPage
    {
        string ReserveId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSaveClientSettings_Click);

            ReserveId = this.Page.Request.QueryString["ReserveId"];
            DataTable dt = ProductHelper.GetStoreProductBaseInfo();
            ReProduct.DataSource = dt;
            ReProduct.DataBind();
            if (!IsPostBack)
            {
                this.dropStartHours.DataBind();
                if (!string.IsNullOrEmpty(ReserveId))
                {
                    Bind();
                }
            }

        }
        protected void Bind()
        {
            //编辑加载
            ProductReservePriceInfo reserveInfo = ProductReservePriceHelper.GetProductReservePriceInfo(int.Parse(ReserveId));
            if (reserveInfo != null)
            {
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(ReserveId))
                {
                    dt = ProductHelper.GetStoreProductBaseInfo(" where productId=" + reserveInfo.ProductId + "");
                }
                this.hidProduct.Value = reserveInfo.ProductId.ToString();
                if (dt.Rows.Count > 0)
                {
                    this.product.Text = dt.Rows[0]["ProductName"].ToString();
                }
                this.dropProductSku.ProductId = reserveInfo.ProductId;
                this.dropProductSku.DataBind();
                this.dropProductSku.SelectedValue = reserveInfo.SkuId;
                this.StartDate.SelectedDate = new DateTime?(reserveInfo.StartDate.Date);
                this.dropStartHours.SelectedValue = new int?(reserveInfo.StartDate.Hour);
                this.CostPrice.Text = reserveInfo.CostPrice.ToString();
                this.SalePrice.Text = reserveInfo.SalePrice.ToString();
                this.NeigouPrice.Text = reserveInfo.NeigouPrice.ToString();
            }

        }
        protected void btnSaveClientSettings_Click(object sender, EventArgs e)
        {
            //编辑保存
            if (!string.IsNullOrEmpty(ReserveId))
            {
                ProductReservePriceInfo reserveInfo = ProductReservePriceHelper.GetProductReservePriceInfo(int.Parse(ReserveId));
                reserveInfo.ProductId = int.Parse(this.hidProduct.Value);
                reserveInfo.SkuId = this.dropProductSku.SelectedValue;
                reserveInfo.StartDate = this.StartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value);
                reserveInfo.CostPrice = decimal.Parse(this.CostPrice.Text);
                reserveInfo.SalePrice = decimal.Parse(this.SalePrice.Text);
                reserveInfo.NeigouPrice = decimal.Parse(this.NeigouPrice.Text);
                if (ProductReservePriceHelper.UpdateProductReservePrice(reserveInfo))
                {
                    this.ShowMsgAndReUrl("修改成功", true, "ProductReservePrice.aspx");
                }
            }
            //添加保存
            else
            {
                ProductReservePriceInfo reserveInfo = new ProductReservePriceInfo
                {
                    ProductId = int.Parse(this.hidProduct.Value),
                    SkuId = this.dropProductSku.SelectedValue,
                    StartDate = this.StartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value),
                    CostPrice = decimal.Parse(this.CostPrice.Text),
                    SalePrice = decimal.Parse(this.SalePrice.Text),
                    NeigouPrice = decimal.Parse(this.NeigouPrice.Text),
                };
                if (ProductReservePriceHelper.AddProductReservePrice(reserveInfo))
                {
                    this.ShowMsgAndReUrl("新增成功", true, "ProductReservePrice.aspx");
                }
            }
        }
        //关联规格
        protected void ReProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "click")
            {
                string productId = e.CommandArgument.ToString();
                this.hidProduct.Value = productId;
                this.dropProductSku.ProductId = new int?(int.Parse(productId));
                this.dropProductSku.DataBind();
            }
        }
    }
}

