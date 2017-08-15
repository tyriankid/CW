using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

public partial class Admin_distributor_SelectStoreToSend : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    protected void bind()
    {
        StringBuilder builder = new StringBuilder();
        ListStoreInfoQuery query = LoadParameters();

        builder.Append("1=1");
        if (!string.IsNullOrEmpty(query.storeName))
        {
            builder.AppendFormat(" and storeName like '%{0}%'", query.storeName);
        }

        DbQueryResult dtmemberInfo = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Distributors", "userid", builder.ToString(), "StoreName,UserId");
        this.grdStoreInfo.DataSource = dtmemberInfo.Data;
        this.grdStoreInfo.DataBind();
        this.pager.TotalRecords = dtmemberInfo.TotalRecords;
        this.txtSearchText.Text = query.storeName;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.ReBind(true);
    }

    protected void btnSendMsg_Click(object sender, EventArgs e)
    {
        string str = base.Request.Form["CheckBoxGroup"];
        if (string.IsNullOrEmpty(str))
        {
            this.ShowMsg("请先选择要推送消息的店长!", false);return;
        }
        if (string.IsNullOrEmpty(this.txtSendMsg.Text))
        {
            this.ShowMsg("请填写要发送的消息内容!",false);return;
        }
        try
        {
            string[] userids = str.Split(',');
            //循环发送微信推送
            for (int i = 0; i < userids.Length; i++)
            {
                MemberInfo reciver = MemberHelper.GetMember(userids[i].ToInt());
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                TemplateMessage templateMessage = new TemplateMessage();

                templateMessage.TemplateId = "kB_wQgstT4UVjI71Za0O3mzbZzsK6VKOWz0hf07WQw4";//Globals.GetMasterSettings(true).WX_Template_01;// "b1_ARggaBzbc5owqmwrZ15QPj9Ksfs0p5i64C6MzXKw";//消息模板ID
                templateMessage.Touser = reciver.OpenId;//用户OPENID

                TemplateMessage.MessagePart[] messateParts = new TemplateMessage.MessagePart[]{
                                                    new TemplateMessage.MessagePart{Name = "first",Value = "你收到了一条管理员消息！"},
                                                    new TemplateMessage.MessagePart{Name = "keyword1",Value ="直销部管理员"},
                                                    new TemplateMessage.MessagePart{Name = "keyword2",Value =DateTime.Now.ToShortTimeString()},
                                                    new TemplateMessage.MessagePart{Name = "remark",Value = "消息内容："+this.txtSendMsg.Text}};
                templateMessage.Data = messateParts;
                TemplateApi.SendMessage(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), templateMessage);
                //写入记录
                AdminWxMsgInfo msgInfo = new AdminWxMsgInfo()
                {
                    ID = Guid.NewGuid(),
                    FQManagerId = ManagerHelper.GetCurrentManager().UserId,
                    FQUserName = "直销部管理员",
                    JSUserId = reciver.UserId,
                    CreateTime = DateTime.Now,
                    MsgContent = this.txtSendMsg.Text
                };

                AdminWxMsgInfoHelper.AddAdminWxMsgInfo(msgInfo);
            }
            ShowMsg("微信消息推送成功!",true);
            CloseWindowAndRedirect();
        }
        catch (Exception ex)
        {
            ShowMsg("微信消息推送异常!"+ex.Message, false);
        }
        
    }

    private ListStoreInfoQuery LoadParameters()
    {
        ListStoreInfoQuery query = new ListStoreInfoQuery();
        query.PageIndex = this.pager.PageIndex;
        query.PageSize = this.pager.PageSize;
        query.SortOrder = SortAction.Desc;
        query.SortBy = "UserId";
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeName"]))
        {
            query.storeName = base.Server.UrlDecode(this.Page.Request.QueryString["storeName"]);
        }
        return query;
    }
    private void ReBind(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("StoreName", this.txtSearchText.Text);

        base.ReloadPage(queryStrings);
    }

}