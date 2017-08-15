namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using Hidistro.ControlPanel.Commodities;
    using System.Linq;
    using Hidistro.Core;
    using System.Data;
    [ParseChildren(true)]
    public class VProductSearch : VshopTemplatedWebControl
    {
        private int typeID;
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptTypes;
        private VshopTemplatedRepeater ReUserSearch;
        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["TypeID"], out this.typeID);
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.rptTypes = (VshopTemplatedRepeater)this.FindControl("rptTypes");
            List<ProductTypeInfo> listProductTypeInfo = (List<ProductTypeInfo>)ProductTypeHelper.GetProductTypes();
            if (rptTypes != null)
            {
                listProductTypeInfo.Add(new ProductTypeInfo() { TypeId = -1, TypeName = "品牌", PTCode = "9999" });
                this.rptTypes.DataSource = listProductTypeInfo;
                this.rptTypes.DataBind();
            }

            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);

            if (this.typeID == 0 && listProductTypeInfo.Count > 0) this.typeID = listProductTypeInfo.First().TypeId;
            if (this.typeID > 0)
            {
                rptCategories.TemplateFile = "/Tags/skin-Common_Categories.ascx";
                IList<CategoryInfo> listCategoryInfo = CatalogHelper.GetSequenceCategories();
                this.rptCategories.DataSource = listCategoryInfo.Where(p => p.AssociatedProductType == this.typeID);
                this.rptCategories.DataBind();
            }
            else
            {
                rptCategories.TemplateFile = "/Tags/skin-Common_Brands.ascx";
                rptCategories.DataSource = CatalogHelper.GetBrandCategories();
                rptCategories.DataBind();
            }
            //*****************star 获取当前模块的搜索记录 2017-8-8 yk********************************
            this.ReUserSearch = (VshopTemplatedRepeater)this.FindControl("ReUserSearch");
            DataTable dtUserSearch = UserSearchLogsHelper.GetUserSearchData(" where FunctionType='Search' order by searchDate desc");
            if (dtUserSearch.Rows.Count > 0)
            {
                this.ReUserSearch.DataSource = dtUserSearch;
                this.ReUserSearch.DataBind();
            }
            //*******************end*********************************************************
            this.Page.Session["stylestatus"] = "4";

            //IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8, DistributorsBrower.GetCurrStoreProductRange());
            //this.rptCategories.DataSource = maxSubCategories;
            //this.rptCategories.DataBind();

            PageTitle.AddSiteNameTitle("分类搜索页");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vProductSearch.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                if (literal != null)
                {
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
        }
    }
}

