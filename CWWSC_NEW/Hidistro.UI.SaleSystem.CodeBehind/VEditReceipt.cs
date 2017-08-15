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
    public class VEditReceipt : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputText CompanyName;
        private HtmlInputText TaxesCode;
        private HtmlInputText Bank;
        private HtmlInputText BankNumber;
        private HtmlInputText Phone;
        private HtmlTextArea Address;
        private HtmlInputHidden hdimg1;
        private HtmlInputHidden hdimg2;
        private HtmlInputHidden hdimg3;
        private HtmlInputHidden ReceiptId;
        private HtmlImage img1;
        private HtmlImage img2;
        private HtmlImage img3;
        private int receiptId;

        protected override void AttachChildControls()
        {
            this.CompanyName = (HtmlInputText)this.FindControl("CompanyName");
            this.TaxesCode = (HtmlInputText)this.FindControl("TaxesCode");
            this.Bank = (HtmlInputText)this.FindControl("Bank");
            this.BankNumber = (HtmlInputText)this.FindControl("BankNumber");
            this.Phone = (HtmlInputText)this.FindControl("Phone");
            this.Address = (HtmlTextArea)this.FindControl("Address");

            this.hdimg1 = (HtmlInputHidden)this.FindControl("hdimg1");
            this.hdimg2 = (HtmlInputHidden)this.FindControl("hdimg2");
            this.hdimg3 = (HtmlInputHidden)this.FindControl("hdimg3");
            this.ReceiptId = (HtmlInputHidden)this.FindControl("ReceiptId");

            this.img1 = (HtmlImage)this.FindControl("img1");
            this.img2 = (HtmlImage)this.FindControl("img2");
            this.img3 = (HtmlImage)this.FindControl("img3");

            UserReceiptInfo receiptinfo = UserReceiptInfoHelper.GetUserReceiptInfo(this.receiptId);
            if (receiptinfo == null)
            {
                this.Page.Response.Redirect("./Receeipt.aspx", true);
            }
            this.CompanyName.Value = receiptinfo.CompanyName;
            this.TaxesCode.Value = receiptinfo.TaxesCode;
            this.Bank.Value = receiptinfo.Bank;
            this.BankNumber.Value = receiptinfo.BankNumber;
            this.Phone.Value = receiptinfo.Phone;
            this.Address.Value = receiptinfo.Address;

            this.hdimg1.Value = receiptinfo.RegistrationImg;
            this.hdimg2.Value = receiptinfo.EmpowerEntrustImg;
            this.hdimg3.Value = receiptinfo.TaxpayerProveImg;

            this.img1.Src = receiptinfo.RegistrationImg;
            this.img2.Src = receiptinfo.EmpowerEntrustImg;
            this.img3.Src = receiptinfo.TaxpayerProveImg;

            //设置主键ID给前端值
            this.ReceiptId.Value = this.receiptId.ToString();

            PageTitle.AddSiteNameTitle("编辑增值税发票信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ReceiptId"], out this.receiptId))
            {
                this.Page.Response.Redirect("./Receeipt.aspx", true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-veditreceipt.html";
            }
            base.OnInit(e);
        }
    }
}

