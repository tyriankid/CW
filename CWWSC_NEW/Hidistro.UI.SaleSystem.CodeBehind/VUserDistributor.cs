namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VUserDistributor : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litFgsName;//所属分公司
        private Literal litStoreName;//门店名称
        private Literal litStoreDzName;//店长姓名
        private Literal litStoreStar;//门店星级
        private Literal litLocation_cityname;//所在位置
        private Literal litLocation_poiname;//位置
        private Literal litLocation_poiaddress;//门店地址
        private HtmlInputHidden hidValueId;//门店ID

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("用户绑定门店");
            this.litFgsName = (Literal)this.FindControl("litFgsName");
            this.litStoreName = (Literal)this.FindControl("litStoreName");
            this.litStoreDzName = (Literal)this.FindControl("litStoreDzName");
            this.litStoreStar = (Literal)this.FindControl("litStoreStar");
            this.litLocation_cityname = (Literal)this.FindControl("litLocation_cityname");
            this.litLocation_poiname = (Literal)this.FindControl("litLocation_poiname");
            this.litLocation_poiaddress = (Literal)this.FindControl("litLocation_poiaddress");

            this.hidValueId = (HtmlInputHidden)this.FindControl("hidValueId");

            MemberInfo memberinfo = MemberProcessor.GetCurrentMember();
            if (memberinfo != null && memberinfo.DistributorUserId > 0)
            {
                DataTable dtView = DistributorsBrower.SelectVWDistributorsByWhere(string.Format(" UserId = {0}", memberinfo.DistributorUserId));
                if (dtView.Rows.Count >0)
                {
                    this.hidValueId.Value = dtView.Rows[0]["UserId"].ToString();
                    this.litFgsName.Text = dtView.Rows[0]["fgsName"].ToString();//分公司名称
                    this.litStoreName.Text = dtView.Rows[0]["StoreName"].ToString();//门店名称
                    this.litStoreDzName.Text = dtView.Rows[0]["storeRelationPerson"].ToString();//店长姓名
                    this.litStoreStar.Text = dtView.Rows[0]["QQ"].ToString();//门店星级
                    this.litLocation_cityname.Text = dtView.Rows[0]["Location_cityname"].ToString();//所在城市
                    this.litLocation_poiname.Text = dtView.Rows[0]["Location_poiname"].ToString();//门店位置
                    this.litLocation_poiaddress.Text = dtView.Rows[0]["Location_poiaddress"].ToString();//门店地址
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-UserDistributor.html";
            }
            base.OnInit(e);
        }
    }
}

