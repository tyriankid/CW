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
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VServiceApply : VWeiXinOAuthTemplatedWebControl
    {

        private VshopTemplatedRepeater ServiceApplyRepeater;
        protected override void AttachChildControls()
        {
            this.ServiceApplyRepeater = (VshopTemplatedRepeater)this.FindControl("ServiceApplyRepeater");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ListDistributorClassQuery query = new ListDistributorClassQuery();
            query.DisUserId = currentMember.UserId;
            query.PageIndex = 1;
            query.PageSize = 10;
            query.SortBy = "ApplyDate";
            query.SortOrder = SortAction.Desc;
            DbQueryResult ListStore = DistributorClassHelper.GetListDistributorClass(query);

            //DataTable dtResult = DistributorClassHelper.SelectClassByWhere(string.Format("DisUserId = '{0}'", currentMember.UserId));
            this.ServiceApplyRepeater.DataSource = ListStore.Data;
            this.ServiceApplyRepeater.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ServiceApply.html";
            }
            base.OnInit(e);
        }
    }
}

