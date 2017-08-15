namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VCommissionDetails : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater vshopcommssion;

        protected override void AttachChildControls()
        {
            int dzUserId = Globals.GetCurrentMemberUserId();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(dzUserId);
            if (userIdDistributors == null)
            {
                DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(dzUserId);
                if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                {
                    dzUserId = disSalesinfo.DisUserId;
                }
            }

            CommissionsQuery query;
            this.vshopcommssion = (VshopTemplatedRepeater)this.FindControl("vshopcommssion");
            query = new CommissionsQuery();
            query.StartTime = query.EndTime = "";
            query.PageIndex = 1;
            query.PageSize = 0x186a0;
            query.UserId = dzUserId;

            DbQueryResult commissions = DistributorsBrower.GetCommissions(query);
            if (commissions.TotalRecords > 0)
            {
                this.vshopcommssion.DataSource = commissions.Data;
                this.vshopcommssion.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCommissionDetails.html";
            }
            base.OnInit(e);
        }
    }
}

