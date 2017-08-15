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
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class EditJinLiMember : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnEditUser;
        //protected System.Web.UI.WebControls.Literal lblLoginNameValue;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.TextBox txtProductCode;
        protected System.Web.UI.WebControls.TextBox txtProductModel;
        protected System.Web.UI.WebControls.TextBox txtPrice;
        protected System.Web.UI.WebControls.TextBox txtBuyNum;
        protected System.Web.UI.WebControls.TextBox txtAddress;
        private int Id;
        protected void btnEditUser_Click(object sender, System.EventArgs e)
        {
            CWMenbersInfo member = CWMembersHelper.GetMemberId(this.Id);
            member.UserName = this.txtUserName.Text.Trim();
            member.accountALLHere = this.txtAccountALLHere.Text.Trim();
            member.StoreName = this.txtStoreName.Text.Trim();
            member.CellPhone = this.txtCellPhone.Text.Trim();
            member.ProductCode = this.txtProductCode.Text.Trim();
            member.ProductModel = this.txtProductModel.Text.Trim();
            member.Price = this.txtPrice.Text.Trim();
            member.BuyNum = this.txtBuyNum.Text.Trim();
            member.Address = Globals.HtmlEncode(this.txtAddress.Text);
            member.CellPhone = this.txtCellPhone.Text.Trim();
            if (this.ValidationMember(member))
            {
                if (CWMembersHelper.UpdateMembersr(member))
                {
                    this.ShowMsg("成功修改了当前会员的个人资料", true);
                }
                else
                {
                    this.ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
        }
        private void LoadMemberInfo()
        {
            CWMenbersInfo member = CWMembersHelper.GetMemberId(this.Id);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                //this.lblLoginNameValue.Text = member.UserName;
                //this.txtRealName.Text = member.RealName;
                //this.txtAddress.Text = Globals.HtmlDecode(member.Address);
                //this.txtQQ.Text = member.QQ;
                //this.txtCellPhone.Text = member.CellPhone;
                //this.txtprivateEmail.Text = member.Email;

                this.txtUserName.Text = member.UserName;
                this.txtAccountALLHere.Text=member.accountALLHere;
                this.txtStoreName.Text= member.StoreName;
                this.txtCellPhone.Text=member.CellPhone;
                this.txtProductCode.Text= member.ProductCode;
                this.txtProductModel.Text=member.ProductModel;
                this.txtPrice.Text=member.Price;
                this.txtBuyNum.Text=member.BuyNum;
                this.txtAddress.Text = Globals.HtmlEncode(member.Address);
                this.txtCellPhone.Text=member.CellPhone;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["Id"], out this.Id))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
                if (!this.Page.IsPostBack)
                {
                    this.LoadMemberInfo();
                }
            }
        }
        private bool ValidationMember(CWMenbersInfo member)
        {
            ValidationResults results = Validation.Validate<CWMenbersInfo>(member, new string[]
			{
				"ValMember"
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
