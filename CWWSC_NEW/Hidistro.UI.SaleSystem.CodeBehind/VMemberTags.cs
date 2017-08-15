namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMemberTags : VWeiXinOAuthTemplatedWebControl
    {
        private MemberTagsLiteral memberLiteral;
        private JINLIMemberTagsLiteral JINLIMemberTagsLiteral;
        private HtmlInputHidden hidJinLi;
        protected override void AttachChildControls()
        {
            this.memberLiteral = (MemberTagsLiteral)this.FindControl("memberLiteral");
            this.JINLIMemberTagsLiteral = (JINLIMemberTagsLiteral)this.FindControl("JINLIMemberTagsLiteral");
            this.hidJinLi = (HtmlInputHidden)this.FindControl("hidJinLi");

            string userId = this.Page.Request.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                //绑定微会员与标签的关系
                DataTable dtusertags = memberTagsRelationHelper.GetmemberTagsData(" where UserType=0 and UserId=" + userId + "");
                string select = "";
                string selectJinLi = "";
                if (dtusertags.Rows.Count > 0)
                {
                    foreach (DataRow row in dtusertags.Rows)
                    {
                        select += row["TagID"] + ",";
                    }
                    this.memberLiteral.SelectedValue=select.TrimEnd(',');
                }
                //验证当前用户是否是金立会员
                DataTable dt = CWMembersHelper.GetMemberwhere(" where UserId=" + userId + "");
                if (dt.Rows.Count>0)
                {
                    //绑定金立会员与标签的关系
                    DataTable dtJinusertags = memberTagsRelationHelper.GetmemberTagsData(" where UserType=1 and UserId=" + userId + "");
                    if (dtJinusertags.Rows.Count > 0)
                    {
                        foreach (DataRow rows in dtJinusertags.Rows)
                        {
                            selectJinLi+=rows["TagID"] + ",";
                        }
                        this.JINLIMemberTagsLiteral.SelectedValue = selectJinLi.TrimEnd(',');
                    }
                }
                else
                {
                    this.hidJinLi.Value="false";
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vMemberTags.html";
            }
            base.OnInit(e);
        }
    }
}

