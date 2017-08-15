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
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyConsultReplyed : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;

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



            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.litfinishnum = (Literal)this.FindControl("litfinishnum");
            this.litallnum = (Literal)this.FindControl("litallnum");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            ProductConsultationAndReplyQuery consultationQuery = new ProductConsultationAndReplyQuery
            {
                DistributorsUserIds = currentMember.UserId.ToString(),
                Type = ConsultationReplyType.Replyed,
                IsCount = true,
                PageIndex = num,
                PageSize = num2,
                SortBy = "ReplyDate",
                SortOrder = SortAction.Desc
            };
            //DataTable productConsultations = (DataTable)ProductBrowser.GetProductConsultations(consultationQuery).Data;
            DbQueryResult dbqueryReesult = ProductBrowser.GetProductConsultations(consultationQuery);
            DataTable productConsultations = (DataTable)dbqueryReesult.Data;
            //为查询出来的问答列表增加商品图
            productConsultations.Columns.Add("ThumbnailsUrl");//图
            for (int i = 0; i < productConsultations.Rows.Count; i++)
            {
                int productId = Convert.ToInt32(productConsultations.Rows[i]["ProductId"]);
                Hidistro.Entities.Commodities.ProductInfo info = ProductBrowser.GetProduct(currentMember, productId);
                productConsultations.Rows[i]["ThumbnailsUrl"] = info.ThumbnailUrl60;
            }
            this.rptProducts.DataSource = productConsultations;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(dbqueryReesult.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("商品咨询");

            this.litallnum.Text = ProductBrowser.FindDistributorUserNoReply(currentMember.UserId).ToString();
            this.litfinishnum.Text = ProductBrowser.FindDistributorUserReply(currentMember.UserId).ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vMyConsultReplyed.html";
            }
            base.OnInit(e);
        }
    }
}

