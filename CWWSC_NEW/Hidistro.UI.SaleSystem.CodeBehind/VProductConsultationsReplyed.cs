namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductConsultationsReplyed : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlImage imgProductImage;
        private Literal litDescription;
        private Literal litProductTitle;
        private Literal litSalePrice;
        private Literal litShortDescription;
        private Literal litSoldCount;
        private Literal litUserName;
        private Literal litConsultationText;
        private Literal litConsultationDate;
        private int consultationId;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["consultationId"], out this.consultationId))
            {
                base.GotoResourceNotFound("");
            }

            //咨询信息
            this.litUserName = (Literal)this.FindControl("litUserName");
            this.litConsultationText = (Literal)this.FindControl("litConsultationText");
            this.litConsultationDate = (Literal)this.FindControl("litConsultationDate");
            //商品信息
            this.litProductTitle = (Literal)this.FindControl("litProductTitle");
            this.litShortDescription = (Literal)this.FindControl("litShortDescription");
            this.litSoldCount = (Literal)this.FindControl("litSoldCount");
            this.litSalePrice = (Literal)this.FindControl("litSalePrice");
            this.imgProductImage = (HtmlImage)this.FindControl("imgProductImage");


            //设置咨询信息显示值
            ProductConsultationInfo productconsultationinfo = ProductCommentHelper.GetProductConsultation(consultationId);
            if (productconsultationinfo != null && productconsultationinfo.ConsultationId > 0)
            {
                this.litUserName.Text = productconsultationinfo.UserName;
                this.litConsultationText.Text = productconsultationinfo.ConsultationText;
                this.litConsultationDate.Text = productconsultationinfo.ConsultationDate.ToString("yyyy-MM-dd HH:mm:ss");

                //设置商品信息显示值
                ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), productconsultationinfo.ProductId);
                if (product != null)
                {
                    this.litProductTitle.SetWhenIsNotNull(product.ProductName);
                    this.litShortDescription.SetWhenIsNotNull(product.ShortDescription);
                    this.litSoldCount.SetWhenIsNotNull(product.ShowSaleCounts.ToString());
                    this.litSalePrice.SetWhenIsNotNull(product.MinSalePrice.ToString("F2"));
                    this.imgProductImage.Src = product.ThumbnailUrl180;
                }
            }

            
            PageTitle.AddSiteNameTitle("商品咨询");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductConsultationsReplyed.html";
            }
            base.OnInit(e);
        }
    }
}

