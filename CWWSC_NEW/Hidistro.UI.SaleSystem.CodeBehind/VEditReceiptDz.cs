namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VEditReceiptDz : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputText CompanyName;
        private HtmlInputText TaxesCode;
        private HtmlInputHidden fptype;
        private HtmlInputHidden ReceiptId;
        private int receiptId;

        protected override void AttachChildControls()
        {
            this.CompanyName = (HtmlInputText)this.FindControl("CompanyName");
            this.TaxesCode = (HtmlInputText)this.FindControl("TaxesCode");

            this.fptype = (HtmlInputHidden)this.FindControl("fptype");
            this.ReceiptId = (HtmlInputHidden)this.FindControl("ReceiptId");

            UserReceiptInfo receiptinfo = UserReceiptInfoHelper.GetUserReceiptInfo(this.receiptId);
            if (receiptinfo == null)
            {
                this.Page.Response.Redirect("./Receeipt.aspx", true);
            }
            this.CompanyName.Value = receiptinfo.CompanyName;
            this.TaxesCode.Value = receiptinfo.TaxesCode;

            this.fptype.Value = receiptinfo.Type1.ToString();

            //设置主键ID给前端值
            this.ReceiptId.Value = this.receiptId.ToString();

            PageTitle.AddSiteNameTitle("编辑增值税发票信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ReceiptId"], out this.receiptId))
            {
                this.Page.Response.Redirect("./ReceeiptDz.aspx", true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-veditreceiptdz.html";
            }
            base.OnInit(e);
        }
    }
}

