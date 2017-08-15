namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public class AddCountDown : AdminPage
    {
        protected Button btnAddCountDown;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected ProductCategoriesDropDownList dropCategories;
        protected GroupBuyProductDropDownList dropProduct;
        protected HourDropDownList dropStartHours;
        protected HourDropDownList dropEndHours;
        protected MinuteDropDownList dropStartMinute;
        protected MinuteDropDownList dropEndMinute;
        protected Label lblPrice;
        protected TextBox txtContent;
        protected TextBox txtMaxCount;
        protected TextBox txtPrice;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void btnbtnAddCountDown_Click(object sender, EventArgs e)
        {
            int num;
            CountDownInfo countDownInfo = new CountDownInfo();
            string str = string.Empty;
            if (this.dropProduct.SelectedValue > 0)
            {
                if (PromoteHelper.ProductCountDownExist(this.dropProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的限时抢购活动", false);
                    return;
                }
                countDownInfo.ProductId = this.dropProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择限时抢购商品");
            }
            if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.dropEndHours.SelectedValue.Value).AddMinutes((double)this.dropEndMinute.SelectedValue.Value);
                if (DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value), countDownInfo.EndDate) >= 0)
                {
                    str = str + Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                }
                else
                {
                    countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value);
                }
            }
            if (int.TryParse(this.txtMaxCount.Text.Trim(), out num))
            {
                countDownInfo.MaxCount = num;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
            }
            if (!string.IsNullOrEmpty(this.txtPrice.Text))
            {
                decimal num2;
                if (decimal.TryParse(this.txtPrice.Text.Trim(), out num2))
                {
                    countDownInfo.CountDownPrice = num2;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("价格填写格式不正确");
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
            }
            else
            {
                countDownInfo.Content = Globals.HtmlEncode(this.txtContent.Text);
                if (PromoteHelper.AddCountDown(countDownInfo))
                {
                    this.ShowMsg("添加限时抢购活动成功", true);
                }
                else
                {
                    this.ShowMsg("添加限时抢购活动失败", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddCountDown.Click += new EventHandler(this.btnbtnAddCountDown_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropProduct.DataBind();
                this.dropStartHours.DataBind();
                this.dropEndHours.DataBind();
                this.dropStartMinute.DataBind();
                this.dropEndMinute.DataBind();
            }
        }
    }
}

