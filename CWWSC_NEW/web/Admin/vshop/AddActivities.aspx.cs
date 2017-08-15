using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;
using Hidistro.ControlPanel.Promotions;

namespace Hidistro.UI.Web.Admin.vshop
{
	public partial class AddActivities : AdminPage
	{

		private void AAbiuZJB(object obj, EventArgs eventArg)
		{
			int num = 0;

            //if (this.dropProduct.SelectedValue > 0)
            //{
            //    if (PromoteHelper.ProductCountDownExist(this.dropProduct.SelectedValue.Value))
            //    {
            //        this.ShowMsg("�Ѿ����ڴ���Ʒ�������", false);
            //        return;
            //    }
            //    countDownInfo.ProductId = this.dropProduct.SelectedValue.Value;
            //}
            //else
            //{
            //    str = str + Formatter.FormatErrorMessage("��ѡ����ʱ������Ʒ");
            //}
            //if (!this.calendarEndDate.SelectedDate.HasValue)
            //{
            //    str = str + Formatter.FormatErrorMessage("��ѡ���������");
            //}
            //else
            //{
            //    countDownInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.dropEndHours.SelectedValue.Value).AddMinutes((double)this.dropEndMinute.SelectedValue.Value);
            //    if (DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value), countDownInfo.EndDate) >= 0)
            //    {
            //        str = str + Formatter.FormatErrorMessage("��ʼ���ڱ���Ҫ���ڽ�������");
            //    }
            //    else
            //    {
            //        countDownInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value);
            //    }
            //}
            //if (int.TryParse(this.txtMaxCount.Text.Trim(), out num))
            //{
            //    countDownInfo.MaxCount = num;
            //}
            //else
            //{
            //    str = str + Formatter.FormatErrorMessage("�޹���������Ϊ�գ�ֻ��Ϊ����");
            //}

            if (this.dropProduct.SelectedValue.HasValue)
            {
                if (VShopHelper.SelectActivitiesByProductId(this.dropProduct.SelectedValue.Value).Rows.Count > 0)
                {
                    this.ShowMsg("�Ѿ����ڴ���Ʒ�������", false);
                    return;
                }
            }
			if (!this.txtStartDate.SelectedDate.HasValue)
			{
                this.ShowMsg("��ѡ��ʼ���ڣ�", false);
				return;
			}
			if (!this.txtEndDate.SelectedDate.HasValue)
			{
                this.ShowMsg("��ѡ��������ڣ�", false);
				return;
			}
            DateTime datetimestart =this.txtStartDate.SelectedDate.Value.AddHours((double)this.dropStartHours.SelectedValue.Value).AddMinutes((double)this.dropStartMinute.SelectedValue.Value);
            DateTime datetimeend =this.txtEndDate.SelectedDate.Value.AddHours((double)this.dropEndHours.SelectedValue.Value).AddMinutes((double)this.dropEndMinute.SelectedValue.Value);
            if (datetimestart.CompareTo(datetimeend) > 0)
            {
                this.ShowMsg("��ʼ���ڲ������ڽ������ڣ�", false);
                return;
            }
			if (this.txtReductionMoney.Text.Trim() == "" || !int.TryParse(this.txtReductionMoney.Text.Trim(), out num))
			{
                this.ShowMsg("������������������", false);
				return;
			}
			if (this.txtMeetMoney.Text.Trim() == "" || !int.TryParse(this.txtMeetMoney.Text.Trim(), out num))
			{
                this.ShowMsg("������������������", false);
				return;
			}
			if (int.Parse(this.txtReductionMoney.Text.Trim()) >= int.Parse(this.txtMeetMoney.Text.Trim()))
			{
                this.ShowMsg("������ܴ��ڵ��������", false);
				return;
			}
            ActivitiesInfo activitiesInfo = new ActivitiesInfo()
            {
                ActivitiesName = this.dropProduct.SelectedItem.Text,
                ActivitiesDescription = this.txtDescription.Text.Trim(),
                StartTime = datetimestart,
                EndTIme = datetimeend,
                MeetMoney = decimal.Parse(this.txtMeetMoney.Text.Trim()),
                ReductionMoney = decimal.Parse(this.txtReductionMoney.Text.Trim())
            };
            activitiesInfo.ActivitiesType = this.dropProduct.SelectedValue.Value;
            activitiesInfo.Type = 0;
            
            if (VShopHelper.AddActivities(activitiesInfo) > 0)
            {
                this.ShowMsgAndReUrl("��ӳɹ���", true, "ActivitiesList.aspx");
                return;
            }
            this.ShowMsg("���ʧ�ܣ�", false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
                //this.dropCategories.IsTopCategory = true;
                //this.dropCategories.IsUnclassified = false;
                //this.dropCategories.DataBind();

                this.dropCategories.DataBind();
                this.dropProduct.DataBind();
                this.dropStartHours.DataBind();
                this.dropEndHours.DataBind();
                this.dropStartMinute.DataBind();
                this.dropEndMinute.DataBind();
			}
			this.btnAddActivity.Click += new EventHandler(this.AAbiuZJB);
		}
	}
}