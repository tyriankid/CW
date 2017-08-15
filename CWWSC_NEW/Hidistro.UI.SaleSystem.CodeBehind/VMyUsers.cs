namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMyUsers : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptUsers;
        private HtmlInputHidden txtTotal;
        private HtmlInputHidden currentUserId;
        private HtmlInputHidden currentDomain;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            /*******Start 2017-07-04，修改适用于店长和店员*******/
            MemberInfo currentMember = null;
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            if (userIdDistributors != null)
                currentMember = MemberProcessor.GetCurrentMember();
            else
            {
                userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentDistributorId());
                if (userIdDistributors != null)
                    currentMember = MemberProcessor.GetMember(Globals.GetCurrentDistributorId());
            }
            /****End******/
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            this.rptUsers = (VshopTemplatedRepeater)this.FindControl("rptUsers");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            this.currentUserId = (HtmlInputHidden)this.FindControl("currentUserId");
            this.currentDomain = (HtmlInputHidden)this.FindControl("currentDomain");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            //查询条件
            string keyword = this.Page.Request.QueryString["keyWord"];
            string isdate = this.Page.Request.QueryString["isdate"];
            //排序字段与规则
            string strsort = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(strsort))
            {
                strsort = "CreateDate";
            }
            string strorder = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(strorder))
            {
                strorder = "desc";
            }

            DistributorsUsersQuery query = new DistributorsUsersQuery();
            query.DistributorUserId = currentMember.UserId;
            query.KeyWord = keyword;
            if (!string.IsNullOrEmpty(isdate) && isdate == "1")
                query.StartDate = DateTime.Now;
            query.IsCount = true;
            query.PageIndex = num;
            query.PageSize = num2;
            query.SortBy = strsort;
            query.SortOrder = strorder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc;

            DbQueryResult dbqueryReesult = DistributorsBrower.GetDistributorUserInfo(query);
            this.rptUsers.DataSource = dbqueryReesult.Data;
            this.rptUsers.DataBind();
            this.txtTotal.SetWhenIsNotNull(dbqueryReesult.TotalRecords.ToString());
            this.currentUserId.SetWhenIsNotNull(currentMember.UserId.ToString());
            this.currentDomain.SetWhenIsNotNull(masterSettings.CurrentDomain);
            PageTitle.AddSiteNameTitle("店铺用户");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyUsers.html";
            }
            base.OnInit(e);
        }
    }
}

