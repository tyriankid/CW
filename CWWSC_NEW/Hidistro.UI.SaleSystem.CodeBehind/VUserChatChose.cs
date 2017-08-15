using System;
using System.Web.UI;
using Hidistro.UI.Common.Controls;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System.Web.UI.HtmlControls;
using Hidistro.Core.Enums;
using Hidistro.Core.Entities;
using Hidistro.Core;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true), WeiXinOAuth(Common.Controls.WeiXinOAuthPage.VMemberCenter)]
    public class VUserChatChose : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptDzList;
        private VshopTemplatedRepeater rptDyList;
        private HtmlInputHidden txtTotal;
        private HtmlInputHidden currentUserId;
        private HtmlInputHidden currentDomain;
        public int userid;

        protected override void AttachChildControls()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            userid = currentMember.UserId;
            
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            this.currentUserId = (HtmlInputHidden)this.FindControl("currentUserId");
            this.currentUserId.SetWhenIsNotNull(currentMember.UserId.ToString());
            this.rptDzList = (VshopTemplatedRepeater)this.FindControl("rptDzList");
            this.rptDyList = (VshopTemplatedRepeater)this.FindControl("rptDyList");
            this.currentDomain = (HtmlInputHidden)this.FindControl("currentDomain");

            if (currentMember.DistributorUserId != currentMember.UserId) //表店员
            {
                DbQueryResult dtDz = DataHelper.PagingByRownumber(1, 100, "userid", SortAction.Asc, false, "aspnet_Members", "userid", " userid = " + currentMember.DistributorUserId, "*,'店长' as roleInfo");
                this.rptDzList.DataSource = dtDz.Data;
                this.rptDzList.DataBind();

                DbQueryResult dtDy = DataHelper.PagingByRownumber(1, 100, "userid", SortAction.Asc, false, "aspnet_Members dyAm left join aspnet_DistributorSales dyD on dyD.SaleUserId = dyAm.UserId", "dyAm.userid", " dyD.IsRz = 1 and dyD.DisUserId = " + currentMember.DistributorUserId, "*,'店员' as roleInfo");
                this.rptDyList.DataSource = dtDy.Data;
                this.rptDyList.DataBind();
            }
            else
            {
                DbQueryResult dtDy = DataHelper.PagingByRownumber(1, 100, "userid", SortAction.Asc, false, "aspnet_Members dyAm left join aspnet_DistributorSales dyD on dyD.SaleUserId = dyAm.UserId", "dyAm.userid", " dyD.IsRz = 1 and dyD.DisUserId = " + currentMember.DistributorUserId, "*,'店员' as roleInfo");
                this.rptDzList.DataSource = dtDy.Data;
                this.rptDzList.DataBind();
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.currentDomain.SetWhenIsNotNull(masterSettings.CurrentDomain);

            PageTitle.AddSiteNameTitle("售后服务");
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VUserChatChose.html";
            }
            base.OnInit(e);
        }
    }
}

