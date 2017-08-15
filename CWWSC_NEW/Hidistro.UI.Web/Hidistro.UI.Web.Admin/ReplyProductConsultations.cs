using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class ReplyProductConsultations : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnReplyProductConsultation;
		private int consultationId;
		protected KindeditorControl fckReplyText;
		protected FormatedTimeLabel lblTime;
		protected System.Web.UI.WebControls.Literal litConsultationText;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected void btnReplyProductConsultation_Click(object sender, System.EventArgs e)
		{
			ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
			if (string.IsNullOrEmpty(this.fckReplyText.Text))
			{
				productConsultation.ReplyText = null;
			}
			else
			{
				productConsultation.ReplyText = this.fckReplyText.Text;
			}
			productConsultation.ReplyUserId = new int?(Globals.GetCurrentManagerUserId());
			productConsultation.ReplyDate = new System.DateTime?(System.DateTime.Now);
			ValidationResults results = Validation.Validate<ProductConsultationInfo>(productConsultation, new string[]
			{
				"Reply"
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
			else
			{
				if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
				{
                    //提醒用户
                    MemberInfo userMember = MemberProcessor.GetMember(productConsultation.UserId);
                    if (userMember != null && !string.IsNullOrEmpty(userMember.OpenId))
                    {
                        string storeName = "创维主店";
                        string strUrl = Request.Url.Host + "/Vshop/MyConsultations.aspx";
                        Messenger.UserConsultationReplyed(strUrl, userMember, storeName, productConsultation);
                    }

					this.fckReplyText.Text = string.Empty;
					this.CloseWindow();
				}
				else
				{
					this.ShowMsg("回复商品咨询失败", false);
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnReplyProductConsultation.Click += new System.EventHandler(this.btnReplyProductConsultation_Click);
				if (!this.Page.IsPostBack)
				{
					ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
					if (productConsultation == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.litUserName.Text = productConsultation.UserName;
						this.litConsultationText.Text = productConsultation.ConsultationText;
						this.lblTime.Time = productConsultation.ConsultationDate;
					}
				}
			}
		}
	}
}
