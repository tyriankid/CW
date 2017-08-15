using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_AddNianxingMember : AdminPage
{
    public string userId = "";
    protected void Page_Load(object sender, System.EventArgs e)
    {
        userId = this.Page.Request.QueryString["Id"];
        this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                bind();
            }
        }

    }
    private void bind()
    {
        nianxingMembersInfo member = nianxingMembersHelper.GetMemberId(int.Parse(userId));
        if (member == null)
        {
            base.GotoResourceNotFound();
        }
        else
        {
            this.txtUserName.Text = member.UserName;
            this.txtAccountALLHere.Text = member.accountALLHere;
            this.txtStoreName.Text = member.StoreName;
            this.txtCellPhone.Text = member.CellPhone;
            this.txtProductCode.Text = member.ProductCode;
            this.txtProductModel.Text = member.ProductModel;
            this.txtPrice.Text = member.Price;
            this.txtBuyNum.Text = member.BuyNum;
            this.txtAddress.Text = Globals.HtmlEncode(member.Address);
            this.txtCellPhone.Text = member.CellPhone;
        }
    }
    private void btnCreate_Click(object sender, System.EventArgs e)
    {
        if (string.IsNullOrEmpty(userId))
        {
            nianxingMembersInfo manager = new nianxingMembersInfo
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
            if (nianxingMembersHelper.Create(manager))
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
        else
        {
            nianxingMembersInfo member = nianxingMembersHelper.GetMemberId(int.Parse(userId));
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
            if (nianxingMembersHelper.UpdateMembersr(member))
            {
                this.ShowMsg("成功修改了当前会员的个人资料", true);
            }
            else
            {
                this.ShowMsg("当前会员的个人信息修改失败", false);
            }
        }
    }


}