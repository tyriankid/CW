using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class AddProductLine : ProductBasePage
    {
        protected System.Web.UI.WebControls.Button btnAdd;
        private int categoryId;
        protected System.Web.UI.WebControls.CheckBox ChkisfreeShipping;
        protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
        protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
        protected System.Web.UI.WebControls.CheckBox chkPic;//*
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl editDescription;
        protected KindeditorControl specification;//*
        protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
        protected System.Web.UI.WebControls.Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
        protected Script Script1;
        protected Script Script2;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtNeigouPrice;//内购价
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtProductCode;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtShortDescription;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtUnit;
        protected TrimTextBox txtWeight;

        /**start佣金设置**/
        //protected RadioButton RadioSaleA;//一口价佣金类型
        //protected RadioButton RadioSaleB;//一口价佣金类型
        //protected TrimTextBox txtSaleCommisionRatio;//返佣比例值
        //protected TrimTextBox txtSaleCommisionMoney;//返佣比例值
        //protected RadioButton RadioNeigouA;//内购价佣金类型
        //protected RadioButton RadioNeigouB;//内购价佣金类型
        //protected TrimTextBox txtNeigouCommisionRatio;//返佣比例值
        //protected TrimTextBox txtNeigouCommisionMoney;//返佣比例值
        /**end佣金设置**/

        protected ProductFlashUpload ucFlashUpload1;
        protected DropDownList DDLProductSource;//商品来源
        protected TrimTextBox txtRestrictNeigouNum;//内购门店中同一会员最多购买数量
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            decimal num3;
            decimal? neigouprice;
            decimal? nullable;
            decimal? nullable2;
            int num4;
            decimal? nullable3;
            int num5;
            int num6;//限购数量

            ////设置返佣
            //decimal? commvalue1;
            //decimal? commvalue2;
            //decimal? commvalue3;
            //decimal? commvalue4;
            ////设置返佣

            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num3, out neigouprice, out nullable, out nullable2, out num4, out nullable3, out num5, out num6))
            {
                if (!this.chkSkuEnabled.Checked)
                {
                    if (num3 <= 0m)
                    {
                        this.ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (neigouprice.HasValue && neigouprice.Value >= num3)
                    {
                        this.ShowMsg("商品内购价必须小于商品一口价", false);
                        return;
                    }
                    if (nullable.HasValue && nullable.Value >= num3)
                    {
                        this.ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                }
                string text = this.editDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                string word = this.specification.Text;//*
                if (this.chkPic.Checked)
                {

                    word = base.DownRemotePic(word);
                }
                string str1 = this.ucFlashUpload1.Value.Trim();
                string[] strArrays = str1.Split(new char[] { ',' });
                string[] strArrays1 = new string[] { "", "", "", "", "" };
                string[] strArrays2 = strArrays1;
                for (int i = 0; i < (int)strArrays.Length && i < 5; i++)
                {
                    strArrays2[i] = strArrays[i];
                }
                //当前后台登陆用户
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                ProductInfo target = new ProductInfo
                {
                    CategoryId = this.categoryId,
                    TypeId = this.dropProductTypes.SelectedValue,
                    ProductName = this.txtProductName.Text,
                    ProductCode = this.txtProductCode.Text,
                    MarketPrice = nullable2,
                    RestrictNeigouNum = num6,
                    Unit = this.txtUnit.Text,
                    Range = 0,
                    ProductSource = Convert.ToInt32(this.DDLProductSource.SelectedValue),
                    ImageUrl1 = strArrays2[0],
                    ImageUrl2 = strArrays2[1],
                    ImageUrl3 = strArrays2[2],
                    ImageUrl4 = strArrays2[3],
                    ImageUrl5 = strArrays2[4],
                    ThumbnailUrl40 = strArrays2[0].Replace("/images/", "/thumbs40/40_"),
                    ThumbnailUrl60 = strArrays2[0].Replace("/images/", "/thumbs60/60_"),
                    ThumbnailUrl100 = strArrays2[0].Replace("/images/", "/thumbs100/100_"),
                    ThumbnailUrl160 = strArrays2[0].Replace("/images/", "/thumbs160/160_"),
                    ThumbnailUrl180 = strArrays2[0].Replace("/images/", "/thumbs180/180_"),
                    ThumbnailUrl220 = strArrays2[0].Replace("/images/", "/thumbs220/220_"),
                    ThumbnailUrl310 = strArrays2[0].Replace("/images/", "/thumbs310/310_"),
                    ThumbnailUrl410 = strArrays2[0].Replace("/images/", "/thumbs410/410_"),
                    ShortDescription = this.txtShortDescription.Text,
                    IsfreeShipping = this.ChkisfreeShipping.Checked,
                    Description = (!string.IsNullOrEmpty(text) && text.Length > 0) ? text : null,//*
                    specification = (!string.IsNullOrEmpty(word) && word.Length > 0) ? word : null,
                    AddedDate = System.DateTime.Now,
                    BrandId = this.dropBrandCategories.SelectedValue,
                    MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|",
                    FilialeId =0
                };

                target.SaleStatus = ProductSaleStatus.line;//线下商品状态

                /****start佣金配置*****/
                //target.SaleCommisionType = this.RadioSaleA.Checked ? 0 : 1;
                //target.SaleCommisionRatio = commvalue1.HasValue ? commvalue1.Value : 0m;
                //target.SaleCommisionMoney = commvalue2.HasValue ? commvalue2.Value : 0m;
                //target.NeigouCommisionType = this.RadioNeigouA.Checked ? 0 : 1;
                //target.NeigouCommisionRatio = commvalue3.HasValue ? commvalue3.Value : 0m;
                //target.NeigouCommisionMoney = commvalue4.HasValue ? commvalue4.Value : 0m;
                /****end佣金配置*****/

                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
                System.Collections.Generic.Dictionary<string, SKUItem> skus;
                if (this.chkSkuEnabled.Checked)
                {
                    target.HasSKU = true;
                    skus = base.GetSkus(this.txtSkus.Text);
                }
                else
                {
                    System.Collections.Generic.Dictionary<string, SKUItem> dictionary3 = new System.Collections.Generic.Dictionary<string, SKUItem>();
                    SKUItem item = new SKUItem
                    {
                        SkuId = "0",
                        SKU = this.txtSku.Text,
                        SalePrice = num3,
                        NeigouPrice = neigouprice.HasValue ? neigouprice.Value : 0m,
                        CostPrice = nullable.HasValue ? nullable.Value : 0m,
                        Stock = num4,
                        Weight = nullable3.HasValue ? nullable3.Value : 0m
                    };
                    dictionary3.Add("0", item);
                    skus = dictionary3;
                    if (this.txtMemberPrices.Text.Length > 0)
                    {
                        base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                    }
                }
                if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
                {
                    attrs = base.GetAttributes(this.txtAttributes.Text);
                }
                ValidationResults validateResults = Validation.Validate<ProductInfo>(target, new string[]
				{
					"AddProduct"
				});
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    System.Collections.Generic.IList<int> tagsId = new System.Collections.Generic.List<int>();
                    if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                    {
                        string str2 = this.txtProductTag.Text.Trim();
                        string[] strArray;
                        if (str2.Contains(","))
                        {
                            strArray = str2.Split(new char[]
							{
								','
							});
                        }
                        else
                        {
                            strArray = new string[]
							{
								str2
							};
                        }
                        string[] array = strArray;
                        for (int i = 0; i < array.Length; i++)
                        {
                            string str3 = array[i];
                            tagsId.Add(System.Convert.ToInt32(str3));
                        }
                    }
                    switch (ProductHelper.AddProduct(target, skus, attrs, tagsId))
                    {
                        case ProductActionStatus.Success:
                            this.ShowMsg("添加商品成功", true);
                            base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ProductLineList.aspx"), true);
                            return;
                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("添加商品失败，商品名称不能重复", false);
                            return;
                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;
                        case ProductActionStatus.SKUError:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;
                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("添加商品失败，保存商品属性时出错", false);
                            return;
                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("添加商品失败，保存商品标签时出错", false);
                            return;
                    }
                    this.ShowMsg("添加商品失败，未知错误", false);
                }
            }
        }
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
            {
                base.DoCallback();
            }
            else
            {
                if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!this.Page.IsPostBack)
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                        CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                        if (category == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.litralProductTag.Text))
                            {
                                this.l_tags.Visible = true;
                            }
                            this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            this.dropProductTypes.DataBind();
                            this.dropProductTypes.SelectedValue = category.AssociatedProductType;
                            this.dropBrandCategories.DataBind();
                            this.DDLProductSource.SelectedValue = "4";
                            this.DDLProductSource.Enabled = false;
                        }
                    }
                }
            }
        }
        private bool ValidateConverts(bool skuEnabled, out decimal salePrice, out decimal? neigouPrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight, out int lineId, out int neigouNum)
        {
            string str = string.Empty;
            neigouPrice = new decimal?(0m);
            costPrice = new decimal?(0m);
            marketPrice = new decimal?(0m);
            weight = new decimal?(0m);
            stock = (lineId = 0);
            salePrice = 0m;
            neigouNum = 0;

            ///***Start设置返佣***/
            //saleRatio = new decimal?(0m);
            //saleMoney = new decimal?(0m);
            //neigouRatio = new decimal?(0m);
            //neigouMoney = new decimal?(0m);
            ///***End设置返佣***/

            if (this.txtProductCode.Text.Length > 20)
            {
                str += Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
            }
            if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtMarketPrice.Text, out num))
                {
                    marketPrice = new decimal?(num);
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (!string.IsNullOrEmpty(this.txtRestrictNeigouNum.Text))
            {
                int inum;
                if (int.TryParse(this.txtRestrictNeigouNum.Text, out inum))
                {
                    neigouNum = inum;
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写商品的内购限制数量");
                }
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    str += Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(this.txtNeigouPrice.Text))
                {
                    decimal num7;
                    if (decimal.TryParse(this.txtNeigouPrice.Text, out num7))
                    {
                        neigouPrice = new decimal?(num7);
                    }
                    else
                    {
                        str += Formatter.FormatErrorMessage("请正确填写商品的内购价");
                    }
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out num2))
                    {
                        costPrice = new decimal?(num2);
                    }
                    else
                    {
                        str += Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    str += Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    decimal num3;
                    if (decimal.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new decimal?(num3);
                    }
                    else
                    {
                        str += Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }

            /**设置佣金**/
            //if (this.RadioSaleA.Checked)//一口价
            //{
            //    decimal inum;
            //    if (decimal.TryParse(this.txtSaleCommisionRatio.Text, out inum))
            //    {
            //        saleRatio = new decimal?(inum);
            //    }
            //    else
            //    {
            //        str += Formatter.FormatErrorMessage("请正确填写一口价返佣比例");
            //    }
            //    decimal inum1;
            //    if (decimal.TryParse(this.txtSaleCommisionMoney.Text, out inum1))
            //    {
            //        saleMoney = new decimal?(inum1);
            //    }
            //}
            //else
            //{
            //    decimal inum;
            //    if (decimal.TryParse(this.txtSaleCommisionMoney.Text, out inum))
            //    {
            //        saleMoney = new decimal?(inum);
            //    }
            //    else
            //    {
            //        str += Formatter.FormatErrorMessage("请正确填写一口价返佣金额");
            //    }
            //    decimal inum1;
            //    if (decimal.TryParse(this.txtSaleCommisionRatio.Text, out inum1))
            //    {
            //        saleRatio = new decimal?(inum1);
            //    }
            //}

            //if (this.RadioNeigouA.Checked)//内购价
            //{
            //    decimal inum;
            //    if (decimal.TryParse(this.txtNeigouCommisionRatio.Text, out inum))
            //    {
            //        neigouRatio = new decimal?(inum);
            //    }
            //    else
            //    {
            //        str += Formatter.FormatErrorMessage("请正确填写内购价返佣比例");
            //    }
            //    decimal inum1;
            //    if (decimal.TryParse(this.txtNeigouCommisionMoney.Text, out inum1))
            //    {
            //        neigouMoney = new decimal?(inum1);
            //    }
            //}
            //else
            //{
            //    decimal inum;
            //    if (decimal.TryParse(this.txtNeigouCommisionMoney.Text, out inum))
            //    {
            //        neigouMoney = new decimal?(inum);
            //    }
            //    else
            //    {
            //        str += Formatter.FormatErrorMessage("请正确填写内购价返佣金额");
            //    }
            //    decimal inum1;
            //    if (decimal.TryParse(this.txtNeigouCommisionRatio.Text, out inum1))
            //    {
            //        neigouRatio = new decimal?(inum1);
            //    }
            //}

            /**设置佣金**/

            bool result;
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

    }
}
