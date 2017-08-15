using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class EditFiliale : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnEditProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.TextBox txtfgsPhone;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        protected System.Web.UI.WebControls.TextBox txtfgsName;
        protected System.Web.UI.WebControls.TextBox txtfgsAddress;
        protected System.Web.UI.WebControls.TextBox txtscode;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;
        private int id;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
            {
                int.TryParse(this.Page.Request.QueryString["id"], out this.id);
            }
            this.btnEditProductType.Click += new System.EventHandler(this.btnEditProductType_Click);
            if (!this.Page.IsPostBack)
            {
                FilialeInfo filiale = FilialeHelper.GetFiliale(id);
                if (filiale == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.txtfgsName.Text = filiale.fgsName;
                    this.txtfgsAddress.Text = filiale.fgsAddress;
                    this.txtfgsPhone.Text = filiale.fgsPhone;
                    this.txtscode.Text = filiale.scode;

                }
            }
        }

        private void btnEditProductType_Click(object sender, System.EventArgs e)
        {
            FilialeInfo filiale = new FilialeInfo
            {
                Id=Convert.ToInt32( this.Page.Request.QueryString["id"]),
                fgsName = this.txtfgsName.Text,
                fgsPhone = this.txtfgsPhone.Text,
                fgsAddress = this.txtfgsAddress.Text,
                scode = this.txtscode.Text
            };



            if (this.ValidationProductType(filiale) && FilialeHelper.UpdateFiliale(filiale))
            {
                this.ShowMsg("修改成功", true);
            }

        }
        private bool ValidationProductType(FilialeInfo filiale)
        {
            ValidationResults results = Validation.Validate<FilialeInfo>(filiale, new string[]
			{
				"ValProductType"
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
