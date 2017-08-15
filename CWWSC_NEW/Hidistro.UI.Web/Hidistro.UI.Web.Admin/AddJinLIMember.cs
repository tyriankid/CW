using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class AddJinLIMember : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnCreate;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.TextBox txtProductCode;
        protected System.Web.UI.WebControls.TextBox txtProductModel;
        protected System.Web.UI.WebControls.TextBox txtPrice;
        protected System.Web.UI.WebControls.TextBox txtBuyNum;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            CWMenbersInfo manager = new CWMenbersInfo
            {
            UserName = this.txtUserName.Text.Trim(),
            accountALLHere = this.txtAccountALLHere.Text.Trim(),
            StoreName = this.txtStoreName.Text.Trim(),
            CellPhone = this.txtCellPhone.Text.Trim(),
            ProductCode = this.txtProductCode.Text.Trim(),
            ProductModel = this.txtProductModel.Text.Trim(),
            Price = this.txtPrice.Text.Trim(),
            BuyNum = this.txtBuyNum.Text.Trim(),
            Address = Globals.HtmlEncode(this.txtAddress.Text)
            };
            if (CWMembersHelper.Create(manager))
            {
                this.txtUserName.Text = string.Empty;
                this.txtAccountALLHere.Text = string.Empty;
                this.txtStoreName.Text = string.Empty;
                this.txtCellPhone.Text = string.Empty;
                this.txtProductCode.Text = string.Empty;
                this.txtProductModel.Text = string.Empty;
                this.txtPrice.Text = string.Empty;
                this.txtBuyNum.Text = string.Empty;
                this.txtAddress.Text = string.Empty;
                this.txtCellPhone.Text = string.Empty;
                this.ShowMsg("成功添加了会员", true);
            }
            else
            {
                this.ShowMsg("添加失败,改昵称已被使用!", false);
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
        }
    }
}
