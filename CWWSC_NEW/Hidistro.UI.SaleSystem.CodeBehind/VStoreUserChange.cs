namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VStoreUserChange : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlInputButton btnApply;
        private HtmlInputHidden hidApplyUserId;
        private HtmlInputHidden hidApplyStoreId;

        protected override void AttachChildControls()
        {
            btnApply = (HtmlInputButton)this.FindControl("btnApply");
            hidApplyUserId = (HtmlInputHidden)this.FindControl("hidApplyUserId");
            hidApplyStoreId = (HtmlInputHidden)this.FindControl("hidApplyStoreId");

            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            hidApplyUserId.SetWhenIsNotNull(currentMember.UserId.ToString());
            hidApplyStoreId.SetWhenIsNotNull(currentMember.DistributorUserId.ToString());

            DataTable dtApply = StoreUserChangeApplyHelper.GetStoreUserChangeApplyData(string.Format(" ApplyUserId = {0} and StoreUserId={1}", currentMember.UserId, currentMember.DistributorUserId));
            if(dtApply.Rows.Count > 0 && dtApply.Rows[0]["ApplyState"].ToString() == "0") //如果已经重复提交,则disable按钮
            {
                btnApply.Value = "审核中..";
                btnApply.Disabled = true;
            }
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-StoreUserChange.html";
            }
            base.OnInit(e);
        }
    }
}

