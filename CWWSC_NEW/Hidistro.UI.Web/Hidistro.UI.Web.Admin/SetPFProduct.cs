using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
    public class SetPFProduct : AdminPage
	{
        private int productid;
        public string strUrl;

        protected System.Web.UI.WebControls.Label labTitle;
        protected System.Web.UI.WebControls.TextBox txtNum1;
        protected System.Web.UI.WebControls.TextBox txtNum2;
        protected System.Web.UI.WebControls.TextBox txtNum3;
        protected System.Web.UI.WebControls.TextBox txtNum4;
        protected System.Web.UI.WebControls.TextBox txtNum5;
        protected System.Web.UI.WebControls.TextBox txtPrice1;
        protected System.Web.UI.WebControls.TextBox txtPrice2;
        protected System.Web.UI.WebControls.TextBox txtPrice3;
        protected System.Web.UI.WebControls.TextBox txtPrice4;
        protected System.Web.UI.WebControls.TextBox txtPrice5;
        protected System.Web.UI.WebControls.CheckBox cboxIsStore;

        protected System.Web.UI.WebControls.Button btnReturn;
        protected System.Web.UI.WebControls.Button btnSetPFPrices;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productid))
			{
				base.GotoResourceNotFound();
			}
			else
			{
                this.btnSetPFPrices.Click += new System.EventHandler(this.btnSetPFPrices_Click);
                this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
                //设置返回地址参数
                if (this.Page.Request.QueryString["reurl"] != null)
                    strUrl = this.Page.Request.QueryString["reurl"].ToString();
				if (!this.Page.IsPostBack)
				{
					this.loadData();
				}
			}
		}

        /// <summary>
        /// 加载配置值
        /// </summary>
        private void loadData() 
        { 
            if(this.productid >0)
            {
                //得到商品详细信息
                System.Collections.Generic.IList<int> tagsId = null;
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                ProductInfo proinfo = ProductHelper.GetProductDetails(this.productid, out dictionary, out tagsId);
                if (proinfo != null && proinfo.ProductId > 0)
                {
                    string strSalePrices = string.Empty;
                    foreach (string str in proinfo.Skus.Keys)
                    {
                        SKUItem item = proinfo.Skus[str];
                        if (item != null && !string.IsNullOrEmpty(item.SkuId))
                        {
                            strSalePrices += item.SalePrice.ToString("0.00") + "&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                    }

                    this.labTitle.Text = "商品名称：" + proinfo.ProductName + "&nbsp;&nbsp;&nbsp;&nbsp;商品一口价：" + strSalePrices;
                    DataTable dt = ProductHelper.GetProductPfPrices(this.productid);
                    if (dt.Rows.Count > 0)
                    {
                        this.cboxIsStore.Checked = dt.Rows[0]["IsStore"].ToString() == "1" ? true : false;
                        switch (dt.Rows.Count)
                        {
                            case 1:
                                this.txtNum1.Text = dt.Rows[0]["Num"].ToString();
                                this.txtPrice1.Text = Convert.ToDecimal(dt.Rows[0]["PFSalePrice"].ToString()).ToString("0.00");
                                break;
                            case 2:
                                this.txtNum1.Text = dt.Rows[0]["Num"].ToString();
                                this.txtNum2.Text = dt.Rows[1]["Num"].ToString();
                                this.txtPrice1.Text = Convert.ToDecimal(dt.Rows[0]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice2.Text = Convert.ToDecimal(dt.Rows[1]["PFSalePrice"].ToString()).ToString("0.00");
                                break;
                            case 3:
                                this.txtNum1.Text = dt.Rows[0]["Num"].ToString();
                                this.txtNum2.Text = dt.Rows[1]["Num"].ToString();
                                this.txtNum3.Text = dt.Rows[2]["Num"].ToString();
                                this.txtPrice1.Text = Convert.ToDecimal(dt.Rows[0]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice2.Text = Convert.ToDecimal(dt.Rows[1]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice3.Text = Convert.ToDecimal(dt.Rows[2]["PFSalePrice"].ToString()).ToString("0.00");
                                break;
                            case 4:
                                this.txtNum1.Text = dt.Rows[0]["Num"].ToString();
                                this.txtNum2.Text = dt.Rows[1]["Num"].ToString();
                                this.txtNum3.Text = dt.Rows[2]["Num"].ToString();
                                this.txtNum4.Text = dt.Rows[3]["Num"].ToString();
                                this.txtPrice1.Text = Convert.ToDecimal(dt.Rows[0]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice2.Text = Convert.ToDecimal(dt.Rows[1]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice3.Text = Convert.ToDecimal(dt.Rows[2]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice4.Text = Convert.ToDecimal(dt.Rows[3]["PFSalePrice"].ToString()).ToString("0.00");
                                break;
                            default:
                                this.txtNum1.Text = dt.Rows[0]["Num"].ToString();
                                this.txtNum2.Text = dt.Rows[1]["Num"].ToString();
                                this.txtNum3.Text = dt.Rows[2]["Num"].ToString();
                                this.txtNum4.Text = dt.Rows[3]["Num"].ToString();
                                this.txtNum5.Text = dt.Rows[4]["Num"].ToString();
                                this.txtPrice1.Text = Convert.ToDecimal(dt.Rows[0]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice2.Text = Convert.ToDecimal(dt.Rows[1]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice3.Text = Convert.ToDecimal(dt.Rows[2]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice4.Text = Convert.ToDecimal(dt.Rows[3]["PFSalePrice"].ToString()).ToString("0.00");
                                this.txtPrice5.Text = Convert.ToDecimal(dt.Rows[4]["PFSalePrice"].ToString()).ToString("0.00");
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSetPFPrices_Click(object sender, System.EventArgs e)
        {
            List<ProductPfPriceInfo> infolist = new List<ProductPfPriceInfo>();
            int num1=0;
            int num2=0;
            int num3=0;
            int num4=0;
            int num5=0;
            decimal price1 = 0;
            decimal price2 = 0;
            decimal price3 = 0;
            decimal price4 = 0;
            decimal price5 = 0;
            int IsStore = this.cboxIsStore.Checked ? 1 : 0;
            if (!string.IsNullOrEmpty(this.txtNum1.Text) && int.TryParse(this.txtNum1.Text, out num1) && !string.IsNullOrEmpty(this.txtPrice1.Text) && decimal.TryParse(this.txtPrice1.Text, out price1))
            {
                ProductPfPriceInfo info = new ProductPfPriceInfo();
                info.ProductId = this.productid;
                info.Num = num1;
                info.PFSalePrice = price1;
                info.IsStore = IsStore;
                infolist.Add(info);
            }
            if (!string.IsNullOrEmpty(this.txtNum2.Text) && int.TryParse(this.txtNum2.Text, out num2) && !string.IsNullOrEmpty(this.txtPrice2.Text) && decimal.TryParse(this.txtPrice2.Text, out price2))
            {
                ProductPfPriceInfo info = new ProductPfPriceInfo();
                info.ProductId = this.productid;
                info.Num = num2;
                info.PFSalePrice = price2;
                info.IsStore = IsStore;
                infolist.Add(info);
            }
            if (!string.IsNullOrEmpty(this.txtNum3.Text) && int.TryParse(this.txtNum3.Text, out num3) && !string.IsNullOrEmpty(this.txtPrice3.Text) && decimal.TryParse(this.txtPrice3.Text, out price3))
            {
                ProductPfPriceInfo info = new ProductPfPriceInfo();
                info.ProductId = this.productid;
                info.Num = num3;
                info.PFSalePrice = price3;
                info.IsStore = IsStore;
                infolist.Add(info);
            }
            if (!string.IsNullOrEmpty(this.txtNum4.Text) && int.TryParse(this.txtNum4.Text, out num4) && !string.IsNullOrEmpty(this.txtPrice4.Text) && decimal.TryParse(this.txtPrice4.Text, out price4))
            {
                ProductPfPriceInfo info = new ProductPfPriceInfo();
                info.ProductId = this.productid;
                info.Num = num4;
                info.PFSalePrice = price4;
                info.IsStore = IsStore;
                infolist.Add(info);
            }
            if (!string.IsNullOrEmpty(this.txtNum5.Text) && int.TryParse(this.txtNum5.Text, out num5) && !string.IsNullOrEmpty(this.txtPrice5.Text) && decimal.TryParse(this.txtPrice5.Text, out price5))
            {
                ProductPfPriceInfo info = new ProductPfPriceInfo();
                info.ProductId = this.productid;
                info.Num = num5;
                info.PFSalePrice = price5;
                info.IsStore = IsStore;
                infolist.Add(info);
            }

            //先删除
            ProductHelper.DeltePFPrices(this.productid);
            if (infolist.Count > 0)
            {
                //保存
                if (ProductHelper.InsertPFPrices(infolist))
                {
                    this.ShowMsgAndReUrl("保存成功！", true, this.strUrl);
                }
                else
                    this.ShowMsg("保存失败。", false);
            }
            this.ShowMsg("保存成功！", false);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, System.EventArgs e)
        {
            this.Page.Response.Redirect(this.strUrl, true);
        }

	}
}
