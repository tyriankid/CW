namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    [ParseChildren(true)]
    //pc端匿名点餐页面
    public class PCProductSearchBuy : VshopTemplatedWebControl
    {
        private Literal litBuyToGive;//买一送一是否开启
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private System.Web.UI.HtmlControls.HtmlInputHidden litCategoryId;
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;
        private List<ShoppingCartInfo> cart;
        private Literal litOrderList;//左侧订单页面,页面载入时会根据购物车内的信息动态绑定添加

        protected override void AttachChildControls()
        {
            if (!Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AnonymousOrder)
            {
                GotoResourceNotFound("pc点餐功能暂未开启!");
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Id"]))
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("Vshop-Manager")
                {
                    Value = this.Page.Request.QueryString["Id"].ToString(),
                    Expires = System.DateTime.Now.AddDays(1.0)
                };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.litCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litCategoryId");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litBuyToGive = (Literal)this.FindControl("litBuyToGive");
            this.litOrderList = (Literal)this.FindControl("litOrderList");
            this.litCategoryId.SetWhenIsNotNull(this.categoryId.ToString());
            this.Page.Session["stylestatus"] = "4";
            switch (Hidistro.Core.SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                case "common":
                case "hotel":
                    DataTable dtProducts = new DataTable();//商品dt
                    DataTable dt = CategoryBrowser.GetCategoriesByRange(1);//CategoryBrowser.GetCategoriesRange(ProductInfo.ProductRanage.All);
                    int total = 0;
                    int num;
                    int num2;
                    if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
                    {
                        num = 1;
                    }
                    if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
                    {
                        num2 = 10;
                    }
                    if (categoryId == 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            //获取第一个分类id
                            int FirstCategoryID = Convert.ToInt32(dt.Rows[0]["categoryId"]);
                            this.litCategoryId.SetWhenIsNotNull(FirstCategoryID.ToString());
                            //匿名点餐情况下,商品为总店的所有商品,用户则是匿名用户(无需登录)
                            dtProducts = ProductBrowser.GetProducts(MemberProcessor.GetAnonymousMember(this.Page.Request.QueryString["type"].ToString()), null, FirstCategoryID, null, this.keyWord, num, num2, out total, "ShowSaleCounts", "desc", "", 1);

                        }
                    }
                    else
                    {
                        dtProducts = ProductBrowser.GetProducts(MemberProcessor.GetAnonymousMember(this.Page.Request.QueryString["type"].ToString()), null, categoryId, null, this.keyWord, num, num2, out total, "ShowSaleCounts", "desc", "", 1);
                    }
                    //绑定购物车的信息
                    this.cart = ShoppingCartProcessor.GetShoppingCartAviti(Globals.GetCurrentManagerUserId());

                    //根据商品id判断是否包含其余规格,如果有,则新增一个字段存入规格号
                    dtProducts.Columns.Add("skuCounts");
                    foreach (DataRow row in dtProducts.Rows)
                    {
                        DataTable skus = ProductBrowser.GetSkus(Convert.ToInt32(row["ProductId"]));
                        row["skuCounts"] = skus.Rows.Count;

                    }

                    this.rptProducts.DataSource = dtProducts;
                    this.rptProducts.DataBind();

                    if (!dt.Columns.Contains("PType"))
                        dt.Columns.Add("PType",typeof(string));
                    foreach (DataRow dr in dt.Rows)
                        dr["PType"] = this.Page.Request.QueryString["type"];
                    this.rptCategories.DataSource = dt;
                    this.rptCategories.DataBind();



                    if (cart != null)
                    {
                        this.litOrderList.Text = "";
                        //根据购物车的信息绑定左侧点单列表的信息
                        foreach (ShoppingCartInfo cartInfo in cart)
                        {
                            foreach (ShoppingCartItemInfo info in cartInfo.LineItems)
                            {
                                this.litOrderList.Text += string.Format(@"<li><span>{4}</span><div class='guige'><d type='skuName'>{7}</d><b style='display:none'>{3}</b>{6}</div>
                                                <span>
                                                    <a id='spSub_{0}' class='shopcart-minus'>-</a>
                                                    <input type='tel' id='buyNum_{0}' class='form-control' value='{2}' disabled='disabled' />
                                                    <input type='hidden' id='skuid_{1}' value='{1}'/>
                                                    <a id='spAdd_{0}' class='shopcart-add'>+</a>
                                                    <input type='hidden' id='giveNum' value='{5}'/>
                                                </span></li>
                                                ", info.ProductId, info.SkuId, info.Quantity, info.AdjustedPrice.ToString("F2"),info.Name,info.GiveQuantity, info.GiveQuantity <= 0 ? "" : "(送" + info.GiveQuantity+")",skuContentFormat(info.SkuContent));
                            }
                        }
                    }
                    this.txtTotal.SetWhenIsNotNull(total.ToString());
                    break;
                default:
                    this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
                    if (this.Page.Request.QueryString["TypeId"] != null)
                    {
                        this.rptCategories.DataSource = CategoryBrowser.GetCategoriesByPruductType(100, Convert.ToInt32(this.Page.Request.QueryString["TypeId"]));
                        this.rptCategories.DataBind();
                    }
                    else
                    {
                        IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8, DistributorsBrower.GetCurrStoreProductRange());
                        this.rptCategories.DataSource = maxSubCategories;
                        this.rptCategories.DataBind();
                    }

                    PageTitle.AddSiteNameTitle("电脑点餐");
                    break;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (masterSettings.BuyOrGive)
            {
                this.litBuyToGive.Text = "<a id=\"btnGiveBuy\" class=\"account-all\" onclick=\"BuyGive()\">买一送一</a>";
            }
        }
        /// <summary>
        /// 页面加载时,整理购物车中商品的规格内容,保持与前端一致
        /// </summary>
        /// <param name="skuContent">商品的原规格字符串</param>
        /// <returns>整理后的字符串</returns>
        private string skuContentFormat(string skuContent)
        {
            skuContent = skuContent.Trim().TrimEnd(';');
            IList<string> skus = skuContent.Split(';');
            string result = string.Empty;
            for (int i = skus.Count-1; i >= 0; i--)
            {
                skus[i] = skus[i].Substring(skus[i].IndexOf("：")+1);
                result += skus[i]+",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-PCProductSearchBuy.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal.Text))
                {
                    literal.Text = "<img src='" + literal.Text + "'></img>";
                }
                else
                {
                    literal.Text = "<img src='/Storage/master/default.png'></img>";
                }
            }
        }
        //private void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "fl")
        //    {
        //        int CategoryId = Convert.ToInt32(e.CommandArgument);
        //        this.Page.Response.Redirect("/Vshop/ProductSearch.aspx?categoryId=" + CategoryId);
        //    }
        //}



    }
}

