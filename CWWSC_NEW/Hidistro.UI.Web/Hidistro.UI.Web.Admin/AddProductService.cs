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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddProducts)]
    public class AddProductService : ProductBasePage
	{
		protected System.Web.UI.WebControls.Button btnAdd;
		//protected System.Web.UI.WebControls.CheckBox ChkisfreeShipping;
		protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
		protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
        protected System.Web.UI.WebControls.CheckBox chkPic;//*
        protected ProductServiceClassDropDownList dropClassList;

		protected ProductTypeDownList dropProductTypes;
		protected KindeditorControl editDescription;
        protected KindeditorControl specification;//*
		protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
		protected ProductTagsLiteral litralProductTag;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
        protected System.Web.UI.WebControls.RadioButton radOnAuditing;
		protected Script Script1;
		protected Script Script2;
		protected TrimTextBox txtAttributes;
        protected TrimTextBox txtNeigouPrice;//内购价
        protected TrimTextBox txtCostPrice;
		protected TrimTextBox txtMarketPrice;
		protected TrimTextBox txtMemberPrices;
		protected TrimTextBox txtProductCode;
        //protected TrimTextBox txtKuKaiProductCode;//酷开商品内码
		protected TrimTextBox txtProductName;
		protected TrimTextBox txtProductTag;
		protected TrimTextBox txtSalePrice;
		protected TrimTextBox txtShortDescription;
		protected TrimTextBox txtSku;
		protected TrimTextBox txtSkus;
		protected TrimTextBox txtStock;
		//protected TrimTextBox txtUnit;
		//protected TrimTextBox txtWeight;
        protected ProductFlashUpload ucFlashUpload1;
        protected DropDownList DDLProductSource;//商品来源
        //protected TrimTextBox txtRestrictNeigouNum;//内购门店中同一会员最多购买数量

        //protected ImageUploader uploader1;
        //protected ImageUploader uploader2;
        //protected ImageUploader uploader3;
        //protected ImageUploader uploader4;
        //protected ImageUploader uploader5;
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			decimal num3;
            decimal? neigouprice;
			decimal? nullable;
			decimal? nullable2;
			int num4;
			decimal? nullable3;
			int num5;
            int num6;
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
				ProductInfo target = new ProductInfo
				{
					//CategoryId = this.categoryId,
					TypeId = this.dropProductTypes.SelectedValue,
					ProductName = this.txtProductName.Text,
					ProductCode = this.txtProductCode.Text,
                    //KukaiCode = this.txtKuKaiProductCode.Text,//酷开商品编码
					MarketPrice = nullable2,
                    //RestrictNeigouNum = num6,
					//Unit = this.txtUnit.Text,
                    Range = 0,
                    ProductSource = Convert.ToInt32(this.DDLProductSource.SelectedValue),
                    //SupplierId = this.DDLProductSource.SelectedValue == "2" ? currentManager.ClientUserId :0,
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
					//IsfreeShipping = this.ChkisfreeShipping.Checked,
                    IsfreeShipping = true,//服务商品默认设置包邮
					Description = (!string.IsNullOrEmpty(text) && text.Length > 0) ? text : null,//*
                    specification=(!string.IsNullOrEmpty(word)&&word.Length>0)?word:null,
					AddedDate = System.DateTime.Now,
                    ClassId = this.dropClassList.SelectedValue,//服务品类
					//MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
                    MainCategoryPath = string.Empty//服务商品所属原始分类为空
				};
				ProductSaleStatus onSale = ProductSaleStatus.OnSale;
				if (this.radInStock.Checked)
				{
					onSale = ProductSaleStatus.OnStock;
				}
				if (this.radUnSales.Checked)
				{
					onSale = ProductSaleStatus.UnSale;
				}
				if (this.radOnSales.Checked)
				{
					onSale = ProductSaleStatus.OnSale;
				}
                //添加审核状态
                if (this.radOnAuditing.Checked)
                {
                    onSale = ProductSaleStatus.OnAuditing;
                }
				target.SaleStatus = onSale;
				System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
				System.Collections.Generic.Dictionary<string, SKUItem> skus;
				if (this.chkSkuEnabled.Checked)
				{
					target.HasSKU = true;
                    skus = base.GetServiceSkus(this.txtSkus.Text);
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
                    //if (this.txtMemberPrices.Text.Length > 0)
                    //{
                    //    base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                    //}
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
                        this.ShowMsg("添加服务商品成功", true);
						//base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", this.categoryId, target.ProductId)), true);
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ProductOnService.aspx"), true);
						return;
					case ProductActionStatus.DuplicateName:
                        this.ShowMsg("添加服务商品失败，商品名称不能重复", false);
						return;
					case ProductActionStatus.DuplicateSKU:
                        this.ShowMsg("添加服务商品失败，商家内码不能重复", false);
						return;
					case ProductActionStatus.SKUError:
                        this.ShowMsg("添加服务商品失败，商家内码不能重复", false);
						return;
					case ProductActionStatus.AttributeError:
                        this.ShowMsg("添加服务商品失败，保存商品属性时出错", false);
						return;
					case ProductActionStatus.ProductTagEroor:
                        this.ShowMsg("添加服务商品失败，保存商品标签时出错", false);
						return;
					}
                    this.ShowMsg("添加服务商品失败，未知错误", false);
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
            //页面加载
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.litralProductTag.Text))
                {
                    this.l_tags.Visible = true;
                }

                this.dropClassList.DataBind();//绑定品类下拉框s.dropProductTypes.DataBind();
                this.dropProductTypes.DataBind();
                //从配置文件中获取商品类型ID
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.ServiceProductType > 0)
                {
                    this.dropProductTypes.SelectedValue = masterSettings.ServiceProductType;
                    this.dropProductTypes.Enabled = false;    
                }

                //服务商品配置
                this.DDLProductSource.SelectedValue = "3";
                this.DDLProductSource.Enabled = false;
                //this.txtProductCode.Text = this.txtSku.Text = "";//允许输入商品内码，  用户对外
                //this.txtProductCode.Enabled = false;
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
			if (this.txtProductCode.Text.Length > 20)
			{
				str += Formatter.FormatErrorMessage("商家内码的长度不能超过20个字符");
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
            //if (!string.IsNullOrEmpty(this.txtRestrictNeigouNum.Text))
            //{
            //    int inum;
            //    if (int.TryParse(this.txtRestrictNeigouNum.Text, out inum))
            //    {
            //        neigouNum = inum;
            //    }
            //    else
            //    {
            //        str += Formatter.FormatErrorMessage("请正确填写商品的内购限制数量");
            //    }
            //}
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
                //if (!string.IsNullOrEmpty(this.txtWeight.Text))
                //{
                //    decimal num3;
                //    if (decimal.TryParse(this.txtWeight.Text, out num3))
                //    {
                //        weight = new decimal?(num3);
                //    }
                //    else
                //    {
                //        str += Formatter.FormatErrorMessage("请正确填写商品的重量");
                //    }
                //}
			}
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
