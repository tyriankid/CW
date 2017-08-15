using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class Order_Receipt : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.RadioButtonList radioReceiptType;
        protected System.Web.UI.WebControls.Label lableCompanyName;
        protected System.Web.UI.WebControls.Label LabelTaxesCode;

        protected System.Web.UI.WebControls.Label address;
        protected System.Web.UI.WebControls.Label Name;
        protected System.Web.UI.WebControls.Label TaxesCode;
        protected System.Web.UI.WebControls.Label bank;
        protected System.Web.UI.WebControls.Label banknum;
        protected System.Web.UI.WebControls.Label phone;
        protected System.Web.UI.WebControls.Image RegistrationImg;
        protected System.Web.UI.WebControls.Image EmpowerEntrustImg;
        protected System.Web.UI.WebControls.Image TaxpayerProveImg;
        protected System.Web.UI.WebControls.HiddenField hiReceiptType;

        public void LoadControl()
        {
            if (receiptId > 0)
            {
                UserReceiptInfo receiptinfo = UserReceiptInfoHelper.GetUserReceiptInfo(receiptId);
                if (receiptinfo != null)
                {
                    this.hiReceiptType.Value = receiptinfo.Type.ToString();
                    if (receiptinfo.Type == 1)
                    {
                        this.address.Text = receiptinfo.Address;
                        this.Name.Text = receiptinfo.CompanyName;
                        this.TaxesCode.Text = receiptinfo.TaxesCode;
                        this.bank.Text = receiptinfo.Bank;
                        this.banknum.Text = receiptinfo.BankNumber;
                        this.phone.Text = receiptinfo.Phone;
                        this.RegistrationImg.ImageUrl = Globals.ApplicationPath + receiptinfo.RegistrationImg;
                        this.EmpowerEntrustImg.ImageUrl = Globals.ApplicationPath + receiptinfo.EmpowerEntrustImg;
                        this.TaxpayerProveImg.ImageUrl = Globals.ApplicationPath + receiptinfo.TaxpayerProveImg;
                    }
                    else
                    {
                        this.radioReceiptType.SelectedValue = receiptinfo.Type1.ToString();
                        if (receiptinfo.Type1 == 1)
                        {
                            this.lableCompanyName.Text = "公司名称：" + receiptinfo.CompanyName;
                            this.LabelTaxesCode.Text = "纳税识别号：" + receiptinfo.TaxesCode;
                        }
                        else
                            this.lableCompanyName.Text = "发票抬头：" + receiptinfo.CompanyName;
                    }
                }
            }
            
        }
        protected override void OnLoad(System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadControl();
            }
        }

        private int receiptId;
        /// <summary>
        /// 发票ID
        /// </summary>
        public int ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }
    }
}
