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
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VStoreRuleList : VWeiXinOAuthTemplatedWebControl
    {

        private VshopTemplatedRepeater StoreRuleRepeater;
        protected override void AttachChildControls()
        {
            this.StoreRuleRepeater = (VshopTemplatedRepeater)this.FindControl("StoreRuleRepeater");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ListDistributorSalesQuery query = new ListDistributorSalesQuery
            {   
                PageIndex=1,
                PageSize =99999,
                DisUserId = currentMember.UserId,
                SortBy = "Scode",
                SortOrder = Core.Enums.SortAction.Asc,
            };
            DbQueryResult ListStore = DistributorSalesHelper.ListDistributorSales(query);
            this.StoreRuleRepeater.DataSource = ListStore.Data;
            this.StoreRuleRepeater.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-StoreRuleList.html";
            }
            base.OnInit(e);
        }
    }
}

