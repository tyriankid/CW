using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class EditStoreInfo : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnEditProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;

        protected System.Web.UI.WebControls.TextBox storeName;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        protected System.Web.UI.WebControls.TextBox storeRelationPerson;
        protected System.Web.UI.WebControls.TextBox storeRelationCell;
        protected System.Web.UI.WebControls.TextBox accountALLHere;
        protected System.Web.UI.WebControls.TextBox scode;
        protected System.Web.UI.WebControls.DropDownList ddlfgs;

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
                StoreInfo StoreInfo = StoreInfoHelper.GetStoreInfo(id);

                if (StoreInfo == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    DataSet ds = DataBaseHelper.GetDataSet("select * from CW_Filiale where id='" + StoreInfo.fgsid + "'");
                    this.accountALLHere.Enabled = false;
                    this.ddlfgs.Enabled = false;
                    this.storeName.Text = StoreInfo.storeName;
                    this.storeRelationPerson.Text = StoreInfo.storeRelationPerson;
                    this.storeRelationCell.Text = StoreInfo.storeRelationCell;
                    this.accountALLHere.Text = StoreInfo.accountALLHere;
                    this.scode.Text = StoreInfo.scode;
                    this.ddlfgs.DataSource = ds;
                    this.ddlfgs.DataTextField = "fgsName";
                    this.ddlfgs.DataValueField = "id";
                    this.ddlfgs.DataBind();
                }
            }
        }

        private void btnEditProductType_Click(object sender, System.EventArgs e)
        {
            int id = Convert.ToInt32(this.Page.Request.QueryString["id"]);
            StoreInfo storeInfo = StoreInfoHelper.GetStoreInfo(id);
            storeInfo.storeName = this.storeName.Text;
            storeInfo.storeRelationPerson = this.storeRelationPerson.Text;
            storeInfo.storeRelationCell = this.storeRelationCell.Text;
            storeInfo.accountALLHere = this.accountALLHere.Text;
            storeInfo.scode = this.scode.Text;

            if (this.ValidationProductType(storeInfo) && StoreInfoHelper.UpdateStoreInfo(storeInfo))
            {
                this.ShowMsg("修改成功", true);
            }

        }
        private bool ValidationProductType(StoreInfo filiale)
        {
            ValidationResults results = Validation.Validate<StoreInfo>(filiale, new string[]
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
