using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_AddmemberTags : AdminPage
{
    public string TagID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        TagID = this.Page.Request.QueryString["TagID"];
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(TagID))
            {
                bind();
            }
        }
    }
    protected void bind()
    {
        memberTagsInfo info = memberTagsHelper.GetMemberTagsInfo(int.Parse(TagID));
        this.txtTagsName.Text = info.TagName;
        this.txtTagsSort.Text = info.Scode;
        this.dropMemberTagsType.SelectedValue = info.TagType.ToString();
    }
    protected void btnSaveClientSettings_Click(object sender, EventArgs e)
    {
        //编辑保存
        if (!string.IsNullOrEmpty(TagID))
        {
            memberTagsInfo info = memberTagsHelper.GetMemberTagsInfo(int.Parse(TagID));
            info.TagName = this.txtTagsName.Text;
            info.Scode = this.txtTagsSort.Text;
            info.TagType = int.Parse(this.dropMemberTagsType.SelectedValue);
            if (memberTagsHelper.UpdateMemberTags(info))
            {
                this.ShowMsg("编辑成功", true);
                return;
            }
            else
            {
                this.ShowMsg("编辑失败", true);
            }
        }
        //添加保存
        else
        {
            memberTagsInfo info = new memberTagsInfo
            {
                TagName = this.txtTagsName.Text,
                Scode = this.txtTagsSort.Text,
                TagType = int.Parse(this.dropMemberTagsType.SelectedValue)

            };
            if (memberTagsHelper.AddmemberTags(info))
            {
                this.ShowMsg("添加成功", true);
                return;
            }
            else
            {
                this.ShowMsg("添加失败", true);
            }
        }
    }
}