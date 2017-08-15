namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorLocation : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litLat;
        private Literal litLng;
        private Literal litWzName;
        private Literal litWzInfo;
        private Literal litWzCity;
        private HtmlInputHidden hidmodule;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺位置信息");
            this.litLat = (Literal)this.FindControl("litLat");
            this.litLng = (Literal)this.FindControl("litLng");
            this.litWzName = (Literal)this.FindControl("litWzName");
            this.litWzInfo = (Literal)this.FindControl("litWzInfo");
            this.litWzCity = (Literal)this.FindControl("litWzCity");
            this.hidmodule = (HtmlInputHidden)this.FindControl("hidmodule");

            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            if (userIdDistributors != null)
            {
                this.hidmodule.Value = userIdDistributors.Location_module;
                this.litLat.Text = userIdDistributors.Location_lat.ToString();//纬度
                this.litLng.Text = userIdDistributors.Location_lng.ToString();//经度
                this.litWzName.Text = userIdDistributors.Location_poiname;//位置名称
                this.litWzInfo.Text = userIdDistributors.Location_poiaddress;//位置详细
                this.litWzCity.Text = userIdDistributors.Location_cityname;//城市
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorLocation.html";
            }
            base.OnInit(e);
        }
    }
}

