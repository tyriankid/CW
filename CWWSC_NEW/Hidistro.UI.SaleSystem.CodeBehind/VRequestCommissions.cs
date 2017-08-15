namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System . Data;

    [ParseChildren(true)]
    public class VRequestCommissions : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputHidden hidmoney;
        private Literal litmaxmoney;
        private HtmlAnchor requestcommission;
        private HtmlAnchor requestcommission1;
        private HtmlInputText txtaccount;
        private HtmlInputText txtmoney;
        private HtmlInputText txtmoneyweixin;
        private FormatedMoneyLabel lblsurpluscommission;

        private HtmlInputText txtaccountname;//用户姓名
        private HtmlInputText txtaccountbank;//开户行

        private HtmlControl quickPayArea;//快捷支付区域

        protected override void AttachChildControls()
        {
            string time="";
            DateTime dtts = DateTime.Now;
            //if (dtts.Day < 28)
            if (dtts.Day < 26)
            {
                if ((Convert.ToInt32(dtts.Month) - 1) == 0)
                {
                    //time = (Convert.ToInt32(dtts.Year) - 1).ToString() + "-" + (Convert.ToInt32(dtts.Month) - 1 + 12).ToString() + "-" + "27";
                    time = (Convert.ToInt32(dtts.Year) - 1).ToString() + "-12-" + "26";
                }
                else
                {
                    //time = dtts.Year + "-" + (Convert.ToInt32(dtts.Month) - 1).ToString() + "-" + "27";
                    time = dtts.Year + "-" + (Convert.ToInt32(dtts.Month) - 1).ToString() + "-" + "26";
                }
            }
            else
            {
                //time = dtts.Year + "-" + dtts.Month + "-" + "27";
                time = dtts.Year + "-" + dtts.Month + "-" + "26";
            }
            int currentMemberUserId = Globals . GetCurrentMemberUserId();
            PageTitle.AddSiteNameTitle("申请提现");
            this.litmaxmoney = (Literal) this.FindControl("litmaxmoney");
            this.txtaccount = (HtmlInputText) this.FindControl("txtaccount");
            this.txtmoney = (HtmlInputText) this.FindControl("txtmoney");
            this.txtmoneyweixin = (HtmlInputText) this.FindControl("txtmoneyweixin");
            this.hidmoney = (HtmlInputHidden) this.FindControl("hidmoney");
            this.lblsurpluscommission = (FormatedMoneyLabel)this.FindControl("lblsurpluscommission");
            this.requestcommission = (HtmlAnchor) this.FindControl("requestcommission");
            this.requestcommission1 = (HtmlAnchor) this.FindControl("requestcommission1");
            this.txtaccountname = (HtmlInputText)this.FindControl("txtaccountname");//用户姓名
            this.txtaccountbank = (HtmlInputText)this.FindControl("txtaccountbank");//开户行
            this.quickPayArea = (HtmlControl)this.FindControl("quickPayArea");//快捷支付区域
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            DataTable dttx = DistributorsBrower.GetCommosionByTime("hc.UserId='" + currentMemberUserId + "' and hc.TradeTime<'" + time + "'");
            if (dttx.Rows.Count > 0)
            {
                this.lblsurpluscommission.Money = Convert.ToDecimal(dttx.Rows[0]["ktx"]);
                this.litmaxmoney.Text = Convert.ToDecimal(dttx.Rows[0]["ktx"]).ToString("0.00");
            }
            else
            {
                this.lblsurpluscommission.Money = null;
                this.litmaxmoney.Text = "0.00";
            }
            #region 为用户信息赋值
            string[] accountInfo=userIdDistributors.RequestAccount.Split(',');//根据逗号分隔符获取相应的信息
            switch(accountInfo.Length)
            {
                case 1:
                    this.txtaccount.Value = accountInfo[0].Substring(accountInfo[0].IndexOf(':')+1);//账户名
                    break;
                case 2:
                    this.txtaccount.Value = accountInfo[0].Substring(accountInfo[0].IndexOf(':')+1);//账户名
                    this.txtaccountname.Value = accountInfo[1].Substring(accountInfo[1].IndexOf(':')+1);//姓名
                    break;
                case 3:
                    this.txtaccount.Value = accountInfo[0].Substring(accountInfo[0].IndexOf(':')+1);//账户名
                    this.txtaccountname.Value = accountInfo[1].Substring(accountInfo[1].IndexOf(':')+1);//姓名
                    this.txtaccountbank.Value = accountInfo[2].Substring(accountInfo[2].IndexOf(':')+1);//开户行
                    break;
            }
            #endregion
            
            
            decimal result = 0M;
            if (decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out result) && (result > 0M))
            {
                if (dttx.Rows.Count > 0)
                    this.litmaxmoney.Text = Convert.ToDecimal(dttx.Rows[0]["ktx"]).ToString("0.00");
                else
                    this.litmaxmoney.Text = "0.00";
                this.txtmoney.Attributes["placeholder"] = "请输入大于等于" + result + "元的金额,并且是整数";
                this.txtmoneyweixin.Attributes["placeholder"] = "请输入大于等于" + result + "元的金额,并且是整数";
                this.hidmoney.Value = result.ToString();
            }
            if (DistributorsBrower.IsExitsCommionsRequest())
            {
                this.requestcommission.Disabled = true;
                this.requestcommission.InnerText = "您的申请正在审核当中";
                this.requestcommission1.Disabled = true;
                this.requestcommission1.InnerText = "您的申请正在审核当中";
            }
            if (!Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsQuickGetCashOn && quickPayArea != null)
                quickPayArea.Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RequestCommissions.html";
            }
            base.OnInit(e);
        }
    }
}

