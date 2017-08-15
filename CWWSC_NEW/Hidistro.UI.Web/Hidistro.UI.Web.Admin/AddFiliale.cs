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

namespace Hidistro.UI.Web.Admin
{


    public class AddFiliale : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnAddProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.TextBox txtfgsName;
        protected System.Web.UI.WebControls.TextBox txtfgsPhone;
        protected System.Web.UI.WebControls.TextBox txtfgsAddress;
        protected System.Web.UI.WebControls.TextBox txtscode;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
               
            }
            else
            {

            }
            this.btnAddProductType.Click += new System.EventHandler(this.btnAddProductType_Click);
        }
        private void btnAddProductType_Click(object sender, System.EventArgs e)
        {
            FilialeInfo Filiale = new FilialeInfo { fgsAddress = this.txtfgsAddress.Text, fgsName = this.txtfgsName.Text, fgsPhone = this.txtfgsPhone.Text, scode = this.txtscode.Text };

            if ( this.txtfgsName.Text != "" && this.txtfgsPhone.Text != "")
            {
                string patterns = @"^((([Math Processing Error]|0\d{2})[- ]?)?\d{8}|(([Math Processing Error]|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
                string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$";
                Regex regex = new Regex(pattern);
                Regex regexx = new Regex(patterns);
                if (regex.IsMatch(this.txtfgsPhone.Text) || regexx.IsMatch(this.txtfgsPhone.Text))
                {

                    if (FilialeHelper.SelectFilialeByName(this.txtfgsName.Text.ToString()).Tables[0].Rows.Count>0)
                    {
                        this.ShowMsg("分公司名称已存在！", false);
                    }
                    else
                    {
                        if (this.ValidationFiliale(Filiale))
                        {

                            int num = FilialeHelper.AddFiliale(Filiale);
                            if (num > 0)
                            {

                            }
                            else
                            {
                                this.ShowMsg("添加分公司成功", true);
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
                this.ShowMsg("分公司名称和电话不能为空！", false);
            }

        }
        private bool ValidationFiliale(FilialeInfo Filiale)
        {
            ValidationResults results = Validation.Validate<FilialeInfo>(Filiale, new string[]
			{
				"ValFiliale"
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
