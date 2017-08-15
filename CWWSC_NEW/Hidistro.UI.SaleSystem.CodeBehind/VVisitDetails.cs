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
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VVisitDetails : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater vshopvisit;
        private HtmlInputHidden txtTotal;

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

            //查询条件
            string isdate = this.Page.Request.QueryString["isdate"];

            this.vshopvisit = (VshopTemplatedRepeater)this.FindControl("vshopvisit");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");

            int num;
            int num2;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }

            VisitQuery query = new VisitQuery();
            if (!string.IsNullOrEmpty(isdate) && isdate == "1")
                query.StartTime = query.EndTime = DateTime.Now.ToShortDateString();
            query.IsCount = true;
            query.PageIndex = num;
            query.PageSize = num2;
            query.SortBy = "VisitDate";
            query.SortOrder = Core.Enums.SortAction.Desc;
            query.DistributorsUserId = dzUserId;

            DbQueryResult commissions = DistributorsBrower.GetDistributorVisit(query);
            this.vshopvisit.DataSource = commissions.Data;
            this.vshopvisit.DataBind();
            this.txtTotal.SetWhenIsNotNull(commissions.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("访问记录");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VVisitDetails.html";
            }
            base.OnInit(e);
        }
    }
}

