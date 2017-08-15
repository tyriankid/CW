namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyRanking : VWeiXinOAuthTemplatedWebControl
    {
        private Literal liturl;
        private VshopTemplatedRepeater rptMyRanking;
        private VshopTemplatedRepeater rptMyRankingList;
        private VshopTemplatedRepeater rptMyTeamRankingList;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }

            /*******2017-07-04，修改适用于店长和店员*******/
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
            
           
            this.liturl = (Literal) this.FindControl("liturl");
            this.rptMyRankingList = (VshopTemplatedRepeater)this.FindControl("rptMyRankingList");
            this.rptMyTeamRankingList = (VshopTemplatedRepeater)this.FindControl("rptMyTeamRankingList");
            this.rptMyRanking = (VshopTemplatedRepeater)this.FindControl("rptMyRanking");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            this.liturl.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/Default.aspx?ReferralId=" + currentMember.UserId;

            //2017-7-21店铺加入分页功能
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            DistributorsQuery query=new DistributorsQuery
            {
                PageIndex=num,
                PageSize=num2
            };
          
            DataSet userRanking = DistributorsBrower.GetUserRankingQuery(query, currentMember.UserId);
            if (userRanking.Tables.Count == 3)
            {
                this.rptMyRanking.DataSource = userRanking.Tables[0];
                this.rptMyRanking.DataBind();
                this.rptMyRankingList.DataSource = userRanking.Tables[1];
                this.rptMyRankingList.DataBind();
                if (userRanking.Tables[1].Rows.Count > 0)
                {
                    this.txtTotal.SetWhenIsNotNull(userRanking.Tables[1].Rows[0]["number"].ToString());
                }
                else
                {
                    this.txtTotal.SetWhenIsNotNull("0");
                    
                }
                this.rptMyTeamRankingList.DataSource = userRanking.Tables[2];
                this.rptMyTeamRankingList.DataBind();
            }
            PageTitle.AddSiteNameTitle("查看排名");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyRanking.html";
            }
            base.OnInit(e);
        }
    }
}

