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
    public class EditSupplier : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnEditProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected System.Web.UI.WebControls.TextBox txtgysPhone;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        protected System.Web.UI.WebControls.TextBox txtgysName;
        protected System.Web.UI.WebControls.TextBox txtgysAddress;
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
                SupplierInfo Supplier = SupplierHelper.GetSupplier(id);
                if (Supplier == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.txtgysName.Text = Supplier.gysName;
                    this.txtgysAddress.Text = Supplier.gysAddress;
                    this.txtgysPhone.Text = Supplier.gysPhone;
                    this.txtscode.Text = Supplier.scode;

                }
            }
        }
        private void btnEditProductType_Click(object sender, System.EventArgs e)
        {
            SupplierInfo Supplier = new SupplierInfo
            {
                Id = Convert.ToInt32(this.Page.Request.QueryString["id"]),
                gysName = this.txtgysName.Text,
                gysPhone = this.txtgysPhone.Text,
                gysAddress = this.txtgysAddress.Text,
                scode = this.txtscode.Text
            };




            if (this.ValidationProductType(Supplier) && SupplierHelper.UpdateSupplier(Supplier))
            {
                this.ShowMsg("修改成功", true);
            }

        }
        private bool ValidationProductType(SupplierInfo Supplier)
        {
            ValidationResults results = Validation.Validate<SupplierInfo>(Supplier, new string[]
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
