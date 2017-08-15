using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;

namespace Hidistro.UI.Web.Admin.vshop
{
	public partial class EditActivities : AdminPage
	{
		int m_ActivitiesId;

        private void LoadActivity(int activitiesid)
		{
            IList<ActivitiesInfo> activitiesInfo = VShopHelper.GetActivitiesInfo(activitiesid.ToString());
			if (activitiesInfo.Count > 0)
			{
                this.dropProduct.SelectedValue = new int?(activitiesInfo[0].ActivitiesType);

                this.txtStartDate.SelectedDate = new DateTime?(activitiesInfo[0].StartTime.Date);
                this.dropStartHours.SelectedValue = new int?(activitiesInfo[0].StartTime.Hour);
                this.dropStartMinute.SelectedValue = new int?(activitiesInfo[0].StartTime.Minute);

                this.txtEndDate.SelectedDate = new DateTime?(activitiesInfo[0].EndTIme.Date);
                this.dropEndHours.SelectedValue = new int?(activitiesInfo[0].EndTIme.Hour);
                this.dropEndMinute.SelectedValue = new int?(activitiesInfo[0].EndTIme.Minute);

				this.txtMeetMoney.Text = activitiesInfo[0].MeetMoney.ToString("F0");
                this.txtReductionMoney.Text = activitiesInfo[0].ReductionMoney.ToString("F0");
                this.txtDescription.Text = activitiesInfo[0].ActivitiesDescription;
			}
		}

		private void btnEditActivity_Click(object obj, EventArgs eventArg)
		{
            if (this.dropProduct.SelectedValue.HasValue)
            {
                IList<ActivitiesInfo> activitiesInfolist = VShopHelper.GetActivitiesInfo(m_ActivitiesId.ToString());
                if (activitiesInfolist.Count > 0)
                {
                    if (activitiesInfolist[0].ActivitiesType != this.dropProduct.SelectedValue.Value && VShopHelper.SelectActivitiesByProductId(this.dropProduct.SelectedValue.Value).Rows.Count > 0)
                    {
                        this.ShowMsg("已经存在此商品的满减活动", false);
                        return;
                    }
                }
                else
                {
                    this.ShowMsg("页面参数错误！", false);
                    return;
                }
            }

			if (!this.txtStartDate.SelectedDate.HasValue)
			{
                this.ShowMsg("请选择开始日期！", false);
				return;
			}
			if (!this.txtEndDate.SelectedDate.HasValue)
			{
                this.ShowMsg("请选择结束日期！", false);
				return;
			}
            DateTime datetimestart = this.txtStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value);
            DateTime datetimeend = this.txtEndDate.SelectedDate.Value.AddHours((double)this.dropEndHours.SelectedValue.Value).AddMinutes((double)this.dropEndMinute.SelectedValue.Value);
            if (datetimestart.CompareTo(datetimeend) > 0)
            {
                this.ShowMsg("开始日期不能晚于结束日期！", false);
                return;
            }

            decimal num = new decimal(0);
            decimal num1 = new decimal(0);

			if (this.txtReductionMoney.Text.Trim() == "")
			{
                this.ShowMsg("减免金额请输入整数！", false);
				return;
			}
			if (!decimal.TryParse(this.txtReductionMoney.Text.Trim(), out num))
			{
                this.ShowMsg("减免金额请输入整数", false);
				return;
			}
			if (this.txtMeetMoney.Text.Trim() == "")
			{
                this.ShowMsg("满足金额请输入整数！", false);
				return;
			}
			if (!decimal.TryParse(this.txtMeetMoney.Text.Trim(), out num1))
			{
                this.ShowMsg("满足金额请输入整数", false);
				return;
			}
			if (decimal.Parse(this.txtReductionMoney.Text.Trim()) >= decimal.Parse(this.txtMeetMoney.Text.Trim()))
			{
                this.ShowMsg("减免金额不能大于等于满足金额！", false);
				return;
			}
			ActivitiesInfo activitiesInfo = new ActivitiesInfo()
			{
				//ActivitiesName = this.txtName.Text.Trim(),
                ActivitiesName = this.dropProduct.SelectedItem.Text,
				ActivitiesDescription = this.txtDescription.Text.Trim(),
				StartTime = this.txtStartDate.SelectedDate.Value,
				EndTIme = this.txtEndDate.SelectedDate.Value,
				MeetMoney = decimal.Parse(this.txtMeetMoney.Text.Trim()),
				ReductionMoney = decimal.Parse(this.txtReductionMoney.Text.Trim())
			};
            activitiesInfo.ActivitiesType = this.dropProduct.SelectedValue.Value;
            activitiesInfo.Type = 0;
			activitiesInfo.ActivitiesId = this.m_ActivitiesId;

			if (VShopHelper.UpdateActivities(activitiesInfo))
			{
                this.ShowMsgAndReUrl("修改成功", true, "ActivitiesList.aspx");
				return;
			}
            this.ShowMsg("修改失败", false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            this.m_ActivitiesId = base.GetUrlIntParam("activitiesid");
			this.btnEditActivity.Click += new EventHandler(this.btnEditActivity_Click);
			if (!this.Page.IsPostBack)
			{
				if (this.m_ActivitiesId == 0)
				{
                    this.Page.Response.Redirect("ActivitiesList.aspx");
					return;
				}
                this.dropCategories.DataBind();
                this.dropProduct.DataBind();
                this.dropStartHours.DataBind();
                this.dropEndHours.DataBind();
                this.dropStartMinute.DataBind();
                this.dropEndMinute.DataBind();

				this.LoadActivity(this.m_ActivitiesId);
			}
		}
	}
}