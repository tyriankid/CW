using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public class editFilialeProductLine : ProductBasePage
    {
        protected string ReUrl = "productonsales.aspx";
        protected System.Web.UI.WebControls.Button btnSave;
        private int categoryId;
        protected System.Web.UI.WebControls.CheckBox ChkisfreeShipping;
        protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
        protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
        protected System.Web.UI.WebControls.CheckBox chkIsDownPic;//*
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckDescription;
        protected KindeditorControl editSpecification;//*
        protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
        protected System.Web.UI.WebControls.Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
        private int productId;
        protected Script Script1;
        protected Script Script2;
        private string toline = "";
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtDisplaySequence;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox showCounts;//显示出售数量
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
        protected DropDownList DDLProductSource;//商品来源
        protected TrimTextBox txtRestrictNeigouNum;//内购门店中同一会员最多购买数量
        protected TrimTextBox txtNeigouPrice;//内购价
        protected TrimTextBox txtWeight;
        protected ProductFlashUpload ucFlashUpload1;
        protected System.Web.UI.WebControls.RadioButton radInStock;
        protected System.Web.UI.WebControls.RadioButton radOnSales;
        /**start佣金设置**/
        protected RadioButton RadioSaleA;//一口价佣金类型
        protected RadioButton RadioSaleB;//一口价佣金类型
        protected TrimTextBox txtSaleCommisionRatio;//返佣比例值
        protected TrimTextBox txtSaleCommisionMoney;//返佣比例值
        protected RadioButton RadioNeigouA;//内购价佣金类型
        protected RadioButton RadioNeigouB;//内购价佣金类型
        protected TrimTextBox txtNeigouCommisionRatio;//返佣比例值
        protected TrimTextBox txtNeigouCommisionMoney;//返佣比例值
        /**end佣金设置**/
        protected RegionSelector dropRegion;//地址控件

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.categoryId == 0)
            {
                this.categoryId = (int)this.ViewState["ProductCategoryId"];
            }
            int num;
            decimal num2;
            decimal? nullable;
            decimal? nullable2;
            int num3;
            decimal? nullable3;
            decimal? neigouprice;//内购价
            int neigouNum;//限购数量
            //设置返佣
            decimal? commvalue1;
            decimal? commvalue2;
            decimal? commvalue3;
            decimal? commvalue4;
            //设置返佣
            if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num, out num2, out neigouprice, out nullable, out nullable2, out neigouNum, out num3, out nullable3, out commvalue1, out commvalue2, out commvalue3, out commvalue4))
            {
                if (!this.chkSkuEnabled.Checked)
                {
                    if (num2 <= 0m)
                    {
                        this.ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (neigouprice.HasValue && neigouprice.Value >= num2)
                    {
                        this.ShowMsg("商品内购价必须小于商品一口价", false);
                        return;
                    }
                    if (nullable.HasValue && nullable.Value >= num2)
                    {
                        this.ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                }
                string text = this.fckDescription.Text;
                if (this.ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                string word = this.editSpecification.Text;
                if (this.chkIsDownPic.Checked)
                {
                    word = base.DownRemotePic(word);
                }
                string str1 = this.ucFlashUpload1.Value.Trim();
                //this.ucFlashUpload1.Value = str1;
                string[] strArrays = str1.Split(new char[] { ',' });
                string[] strArrays1 = new string[] { "", "", "", "", "" };
                string[] strArrays2 = strArrays1;
                for (int i = 0; i < (int)strArrays.Length && i < 5; i++)
                {
                    strArrays2[i] = strArrays[i];
                }
                //当前后台登陆用户
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                //修改一个商品
                //1.从数据库获取填充到界面,有一个数据对象
                ProductInfo target = ProductHelper.GetProductBaseInfo(this.productId);
                //2.根据界面输入修改对象
                target.ProductId = this.productId;
                target.CategoryId = this.categoryId;
                target.TypeId = this.dropProductTypes.SelectedValue;
                target.ProductName = this.txtProductName.Text;
                target.ProductCode = this.txtProductCode.Text;
                target.DisplaySequence = num;
                target.MarketPrice = nullable2;
                target.RestrictNeigouNum = neigouNum;//限购数量
                target.Unit = this.txtUnit.Text;
                //target.Range = Convert.ToInt32(this.DDLRange.SelectedValue);
                target.Range = 0;
                target.ShowSaleCounts = int.Parse(this.showCounts.Text);
                target.ImageUrl1 = strArrays2[0];
                target.ImageUrl2 = strArrays2[1];
                target.ImageUrl3 = strArrays2[2];
                target.ImageUrl4 = strArrays2[3];
                target.ImageUrl5 = strArrays2[4];
                target.ThumbnailUrl40 = strArrays2[0].Replace("/images/", "/thumbs40/40_");
                target.ThumbnailUrl60 = strArrays2[0].Replace("/images/", "/thumbs60/60_");
                target.ThumbnailUrl100 = strArrays2[0].Replace("/images/", "/thumbs100/100_");
                target.ThumbnailUrl160 = strArrays2[0].Replace("/images/", "/thumbs160/160_");
                target.ThumbnailUrl180 = strArrays2[0].Replace("/images/", "/thumbs180/180_");
                target.ThumbnailUrl220 = strArrays2[0].Replace("/images/", "/thumbs220/220_");
                target.ThumbnailUrl310 = strArrays2[0].Replace("/images/", "/thumbs310/310_");
                target.ThumbnailUrl410 = strArrays2[0].Replace("/images/", "/thumbs410/410_");
                target.ShortDescription = this.txtShortDescription.Text;
                target.IsfreeShipping = this.ChkisfreeShipping.Checked;
                target.Description = (!string.IsNullOrEmpty(text) && text.Length > 0) ? text : null;
                target.specification = (!string.IsNullOrEmpty(word) && word.Length > 0) ? word : null;
                target.AddedDate = System.DateTime.Now;
                target.BrandId = this.dropBrandCategories.SelectedValue;
                //来源及供应商
                target.ProductSource = Convert.ToInt32(this.DDLProductSource.SelectedValue);
                target.FilialeId = currentManager.ClientUserId;
                /****start佣金配置*****/
                target.SaleCommisionType = this.RadioSaleA.Checked ? 0 : 1;
                target.SaleCommisionRatio = commvalue1.HasValue ? commvalue1.Value : 0m;
                target.SaleCommisionMoney = commvalue2.HasValue ? commvalue2.Value : 0m;
                target.NeigouCommisionType = this.RadioNeigouA.Checked ? 0 : 1;
                target.NeigouCommisionRatio = commvalue3.HasValue ? commvalue3.Value : 0m;
                target.NeigouCommisionMoney = commvalue4.HasValue ? commvalue4.Value : 0m;
                /****end佣金配置*****/
                //分公司修改必须选择商品销售区域 yk
                if (!this.dropRegion.GetSelectedRegionId().HasValue)
                {
                    this.ShowMsg("请选择商品销售区域", false);
                    return;
                }
                else
                {
                    int num4;
                    if (int.TryParse(this.dropRegion.GetSelectedRegionId().Value.ToString(), out num4))
                    {
                        target.RegionId = new int?(num4);
                        target.RegionName = this.dropRegion.GetSelectedRegionName();
                    }
                }
                ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                if (this.radInStock.Checked)
                {
                    onSale = ProductSaleStatus.OnStock;
                }
                if (this.radOnSales.Checked)
                {
                    onSale = ProductSaleStatus.OnSale;
                }
                target.SaleStatus = onSale;
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
                if (category != null)
                {
                    target.MainCategoryPath = category.Path + "|";
                }
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
                        SalePrice = num2,
                        NeigouPrice = neigouprice.HasValue ? neigouprice.Value : 0m,
                        CostPrice = nullable.HasValue ? nullable.Value : 0m,
                        Stock = num3,
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
                ValidationResults validateResults = Validation.Validate<ProductInfo>(target);
                if (!validateResults.IsValid)
                {
                    this.ShowMsg(validateResults);
                }
                else
                {
                    System.Collections.Generic.IList<int> tagIds = new System.Collections.Generic.List<int>();
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
                            tagIds.Add(System.Convert.ToInt32(str3));
                        }
                    }
                    switch (ProductHelper.UpdateProduct(target, skus, attrs, tagIds))
                    {
                        case ProductActionStatus.Success:
                            this.litralProductTag.SelectedValue = tagIds;
                            if (base.Request.QueryString["reurl"] != null)
                            {
                                this.ReUrl = base.Request.QueryString["reurl"].ToString();
                            }
                            if (this.DDLProductSource.SelectedValue == "4")
                                this.ReUrl = "FilialeProductLineList.aspx";
                            this.ShowMsgAndReUrl("修改商品成功", true, this.ReUrl);
                            this.ShowMsg("修改商品成功", true);
                            break;
                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("修改商品失败，商品名称不能重复", false);
                            break;
                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            break;
                        case ProductActionStatus.SKUError:
                            this.ShowMsg("修改商品失败，商家编码不能重复", false);
                            break;
                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("修改商品失败，保存商品属性时出错", false);
                            break;
                        case ProductActionStatus.OffShelfError:
                            this.ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
                            break;
                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("修改商品失败，保存商品标签时出错", false);
                            break;
                        default:
                            this.ShowMsg("修改商品失败，未知错误", false);
                            break;

                    }
                }
            }
        }
        private void LoadProduct(ProductInfo product, System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropBrandCategories.SelectedValue = product.BrandId;
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtProductCode.Text = product.ProductCode;
            this.txtUnit.Text = product.Unit;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else
            {
                this.radInStock.Checked = true;
            }
            //当前登录身份为分公司时禁止修改商品名称和商品码
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                //禁止非总部操作商品项
                this.txtProductName.Enabled = false;
                this.txtProductCode.Enabled = false;
            }
            this.DDLProductSource.SelectedValue = product.ProductSource.ToString();
            this.DDLProductSource.Enabled = false;
            //end
            /*****Start2017-08-02设置佣金*******/
            if (product.SaleCommisionType == 0)
            {
                this.RadioSaleA.Checked = true;
            }
            else
            {
                this.RadioSaleB.Checked = true;
            }
            if (product.SaleCommisionRatio > 0)
                this.txtSaleCommisionRatio.Text = product.SaleCommisionRatio.ToString("F2");
            if (product.SaleCommisionMoney > 0)
                this.txtSaleCommisionMoney.Text = product.SaleCommisionMoney.ToString("F2");

            if (product.NeigouCommisionType == 0)
            {
                this.RadioNeigouA.Checked = true;
            }
            else
            {
                this.RadioNeigouB.Checked = true;
            }
            if (product.NeigouCommisionRatio > 0)
                this.txtNeigouCommisionRatio.Text = product.NeigouCommisionRatio.ToString("F2");
            if (product.NeigouCommisionMoney > 0)
                this.txtNeigouCommisionMoney.Text = product.NeigouCommisionMoney.ToString("F2");

            /*****End设置佣金*******/
            this.showCounts.Text = product.ShowSaleCounts.ToString();
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            //限购数量
            if (product.RestrictNeigouNum > 0)
                this.txtRestrictNeigouNum.Text = product.RestrictNeigouNum.ToString();//限购数量
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            this.editSpecification.Text = product.specification;
            this.ChkisfreeShipping.Checked = product.IsfreeShipping;
            this.dropRegion.SetSelectedRegionId(product.RegionId);//地址
            string[] imageUrl1 = new string[] { product.ImageUrl1, ",", product.ImageUrl2, ",", product.ImageUrl3, ",", product.ImageUrl4, ",", product.ImageUrl5 };
            string str4 = string.Concat(imageUrl1);
            ProductFlashUpload productFlashUpload = this.ucFlashUpload1;
            string str5 = str4.Replace(",,", ",").Replace(",,", ",");
            char[] chrArray = new char[] { ',' };
            productFlashUpload.Value = str5.Trim(chrArray);

            if (attrs != null && attrs.Count > 0)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                this.txtAttributes.Text = builder.ToString();
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                System.Text.StringBuilder builder2 = new System.Text.StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str];
                    string str2 = string.Concat(new string[]
					{
						"<item skuCode=\"",
						item.SKU,
						"\" salePrice=\"",
						item.SalePrice.ToString("F2"),
                        "\" neigouPrice=\"",
						(item.NeigouPrice > 0m) ? item.NeigouPrice.ToString("F2") : "",
						"\" costPrice=\"",
						(item.CostPrice > 0m) ? item.CostPrice.ToString("F2") : "",
						"\" qty=\"",
						item.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture),
						"\" weight=\"",
						(item.Weight > 0m) ? item.Weight.ToString("F2") : "",
						"\"><skuFields>"
					});
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray2 = new string[]
						{
							"<sku attributeId=\"",
							num3.ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" valueId=\"",
							item.SkuItems[num3].ToString(System.Globalization.CultureInfo.InvariantCulture),
							"\" />"
						};
                        string str3 = string.Concat(strArray2);
                        str2 += str3;
                    }
                    str2 += "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str2 += "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num5 = item.MemberPrices[num4];
                            str2 += string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(System.Globalization.CultureInfo.InvariantCulture), num5.ToString("F2"));
                        }
                        str2 += "</memberPrices>";
                    }
                    str2 += "</item>";
                    builder2.Append(str2);
                }
                builder2.Append("</productSkus></xml>");
                this.txtSkus.Text = builder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            this.txtNeigouPrice.Text = ((defaultSku.NeigouPrice > 0m) ? defaultSku.NeigouPrice.ToString("F2") : "");
            this.txtCostPrice.Text = ((defaultSku.CostPrice > 0m) ? defaultSku.CostPrice.ToString("F2") : "");
            this.txtStock.Text = defaultSku.Stock.ToString(System.Globalization.CultureInfo.InvariantCulture);
            this.txtWeight.Text = ((defaultSku.Weight > 0m) ? defaultSku.Weight.ToString("F2") : "");
          
            if (defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num6 in defaultSku.MemberPrices.Keys)
                {
                    decimal num7 = defaultSku.MemberPrices[num6];
                    this.txtMemberPrices.Text = this.txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num6.ToString(System.Globalization.CultureInfo.InvariantCulture), num7.ToString("F2"));
                }
                this.txtMemberPrices.Text = this.txtMemberPrices.Text + "</gradePrices></xml>";
            }
        }
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
            if (!this.Page.IsPostBack)
            {
                System.Collections.Generic.IList<int> tagsId = null;
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                ProductInfo product = ProductHelper.GetProductDetails(this.productId, out dictionary, out tagsId);
                if (product == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
                        this.ViewState["ProductCategoryId"] = this.categoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                        this.ViewState["ProductCategoryId"] = product.CategoryId;
                        this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    this.lnkEditCategory.NavigateUrl = this.lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    this.litralProductTag.SelectedValue = tagsId;
                    if (tagsId.Count > 0)
                    {
                        foreach (int num in tagsId)
                        {
                            this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                        }
                        this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                    }
                    this.dropProductTypes.DataBind();
                    this.dropBrandCategories.DataBind();
                    this.LoadProduct(product, dictionary);
                }
            }
        }
        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? neigouprice, out decimal? costPrice, out decimal? marketPrice, out int neigouNum, out int stock, out decimal? weight, out decimal? saleRatio, out decimal? saleMoney, out decimal? neigouRatio, out decimal? neigouMoney)
        {
            string str = string.Empty;
            neigouprice = new decimal?(0m);
            costPrice = new decimal?(0m);
            marketPrice = new decimal?(0m);
            weight = new decimal?(0m);
            displaySequence = (stock = 0);
            salePrice = 0m;
            neigouNum = 0;
            /***Start设置返佣***/
            saleRatio = new decimal?(0m);
            saleMoney = new decimal?(0m);
            neigouRatio = new decimal?(0m);
            neigouMoney = new decimal?(0m);
            /***End设置返佣***/
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                str += Formatter.FormatErrorMessage("请正确填写商品排序");
            }
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
                int inum = 0;
                if (int.TryParse(this.txtRestrictNeigouNum.Text, out inum))
                {
                    neigouNum = inum;
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写商品在内购门店中的限购数量");
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
                    decimal num1;
                    if (decimal.TryParse(this.txtNeigouPrice.Text, out num1))
                    {
                        neigouprice = new decimal?(num1);
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
            if (this.RadioSaleA.Checked)//一口价返佣
            {
                decimal inum;
                if (decimal.TryParse(this.txtSaleCommisionRatio.Text, out inum))
                {
                    saleRatio = new decimal?(inum);
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写一口价返佣比例");
                }
                decimal inum1;
                if (decimal.TryParse(this.txtSaleCommisionMoney.Text, out inum1))
                {
                    saleMoney = new decimal?(inum1);
                }
            }
            else
            {
                decimal inum;
                if (decimal.TryParse(this.txtSaleCommisionMoney.Text, out inum))
                {
                    saleMoney = new decimal?(inum);
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写一口价返佣金额");
                }
                decimal inum1;
                if (decimal.TryParse(this.txtSaleCommisionRatio.Text, out inum1))
                {
                    saleRatio = new decimal?(inum1);
                }
            }

            if (this.RadioNeigouA.Checked)//内购价返佣
            {
                decimal inum;
                if (decimal.TryParse(this.txtNeigouCommisionRatio.Text, out inum))
                {
                    neigouRatio = new decimal?(inum);
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写内购价返佣比例");
                }
                decimal inum1;
                if (decimal.TryParse(this.txtNeigouCommisionMoney.Text, out inum1))
                {
                    neigouMoney = new decimal?(inum1);
                }
            }
            else
            {
                decimal inum;
                if (decimal.TryParse(this.txtNeigouCommisionMoney.Text, out inum))
                {
                    neigouMoney = new decimal?(inum);
                }
                else
                {
                    str += Formatter.FormatErrorMessage("请正确填写内购价返佣金额");
                }
                decimal inum1;
                if (decimal.TryParse(this.txtNeigouCommisionRatio.Text, out inum1))
                {
                    neigouRatio = new decimal?(inum1);
                }
            }

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
