using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_distributor_ApplyUserChange : AdminPage
{
    protected string ID = "";
    protected string applyuserid = "";
    protected string storeuserid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ID = this.Page.Request.QueryString["ID"];
        applyuserid = this.Page.Request.QueryString["applyuserid"];
        storeuserid = this.Page.Request.QueryString["storeuserid"];
    }
    protected void btnPass_Click(object sender, EventArgs e)
    {
        StoreUserChangeApply info = StoreUserChangeApplyHelper.GetStoreUserChangeApply(new Guid(ID));
        if (info != null)
        {
            //首先调用交换用户身份存储过程
            if (StoreUserChangeApplyHelper.ChangeUserToStore(applyuserid.ToInt(), storeuserid.ToInt()) == 1)
            {
                info.ApplyState = 1;
                info.AuditingDate = DateTime.Now;
                if (StoreUserChangeApplyHelper.UpdateStoreUserChangeApply(info))
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    masterSettings.VshopMemberCookieStr = masterSettings.VshopMemberCookieStr + 1;
                    SettingsManager.Save(masterSettings);
                    this.ShowMsg("审核成功", true);
                }
            }
            else
            {
                info.ApplyState = 2;
                info.AuditingDate = DateTime.Now;
                if (StoreUserChangeApplyHelper.UpdateStoreUserChangeApply(info))
                {
                    this.ShowMsg("审核失败，交换身份过程出现错误！", false);
                }
            }

        }
        else
        {
            this.ShowMsg("审核记录不存在", false);
        }
    }
    protected void btnNoPass_Click(object sender, EventArgs e)
    {
        StoreUserChangeApply info = StoreUserChangeApplyHelper.GetStoreUserChangeApply(new Guid(ID));
        if (info != null)
        {
            info.ApplyState = 2;
            info.AuditingDate = DateTime.Now;
            info.Reason = this.txtAccount.Text;
            if (StoreUserChangeApplyHelper.UpdateStoreUserChangeApply(info))
            {
                this.ShowMsg("审核成功", true);
            }
        }
    }
}