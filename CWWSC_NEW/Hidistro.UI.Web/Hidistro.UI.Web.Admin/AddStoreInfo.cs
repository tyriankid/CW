using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using Hidistro.ControlPanel.Function;
using System.Text.RegularExpressions;
using ControlPanel.Commodities;
using Hidistro.Core.Entities;
using ASPNET.WebControls;
using System.Data;
namespace Hidistro.UI.Web.Admin
{
    public class AddStoreInfo : AdminPage
    {

        protected System.Web.UI.WebControls.Button btnAddProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.TextBox storeName;
        protected System.Web.UI.WebControls.TextBox storeRelationPerson;
        protected System.Web.UI.WebControls.TextBox storeRelationCell;
        protected System.Web.UI.WebControls.TextBox accountALLHere;
        protected System.Web.UI.WebControls.TextBox scode;
        protected System.Web.UI.WebControls.DropDownList ddlfgs;
        protected Pager pager;
        private string searchkey;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                BindFgs();
            }
            else
            {

            }
            this.btnAddProductType.Click += new System.EventHandler(this.btnAddProductType_Click);

        }
        protected void BindFgs()
        {
            DataSet ds = FilialeHelper.GetAllFiliale();
            this.ddlfgs.DataSource = ds;
            this.ddlfgs.DataTextField = "fgsName" ;
            this.ddlfgs.DataValueField = "id";
            this.ddlfgs.DataBind();
            this.ddlfgs.Items.Insert(0, new ListItem("——— 请选择 ———", "0"));
        }
        private void btnAddProductType_Click(object sender, System.EventArgs e)
        {
            if (this.ddlfgs.SelectedValue == "0")
            {
                this.ShowMsg("请选择分公司！", false);
                return;
            }
            StoreInfo StoreInfo = new StoreInfo 
            { 
                storeName = this.storeName.Text, 
                storeRelationCell = this.storeRelationCell.Text, 
                storeRelationPerson = this.storeRelationPerson.Text, 
                accountALLHere = this.accountALLHere.Text, 
                scode = this.scode.Text ,
                fgsid = Convert.ToInt32(this.ddlfgs.SelectedValue)
            };
            if (this.storeName.Text != "" && this.accountALLHere.Text != "" && this.storeRelationCell.Text != "" && this.storeRelationPerson.Text != "")
            {
                string patterns = @"^((([Math Processing Error]|0\d{2})[- ]?)?\d{8}|(([Math Processing Error]|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
                string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$";
                Regex regex = new Regex(pattern);
                Regex regexx = new Regex(patterns);
                if (regex.IsMatch(this.storeRelationCell.Text)||regexx.IsMatch(this.storeRelationCell.Text))
                {

                    if (StoreInfoHelper.SelectStoreInfoByWhere(string.Format(" storeName = '{0}'", this.storeName.Text.ToString().Trim())).Rows.Count > 0)
                    {
                        this.ShowMsg("门店名称已存在！", false);
                    }
                    else
                    {
                        if (this.ValidationFiliale(StoreInfo))
                        {

                            int num = StoreInfoHelper.AddStoreInfo(StoreInfo);
                            if (num > 0)
                            {

                            }
                            else
                            {
                                this.ShowMsg("添加创维门店成功", true);
                            }
                        }
                    }
                }
                else
                {
                    this.ShowMsg("请输入正确手机号或座机号！", false);
                }


            }
            else
            {
                this.ShowMsg("带“*”号的是必填字段！", false);
            }

        }
        private bool ValidationFiliale(StoreInfo StoreInfo)
        {
            ValidationResults results = Validation.Validate<StoreInfo>(StoreInfo, new string[]
			{
				"ValStoreInfo"
			});
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (System.Collections.Generic.IEnumerable<ValidationResult>)results)
                {
                    msg += Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}
