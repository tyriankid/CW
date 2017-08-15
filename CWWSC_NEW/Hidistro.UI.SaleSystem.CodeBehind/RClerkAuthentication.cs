namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Data;
    using System.Collections;
    using Hidistro.Entities.Members;
using System.Web.UI.HtmlControls;
    [ParseChildren(true)]
    public class RClerkAuthentication : VWeiXinOAuthTemplatedWebControl
    {
        //private DropDownList FgsName;
        //private Literal litButton;
        //private Literal customInputs;//自定义控件
        private HtmlInputHidden hidIsStoreManage;
        protected override void AttachChildControls()
        {

            this.hidIsStoreManage = (HtmlInputHidden)this.FindControl("hidIsStoreManage");

            if (Hidistro.SaleSystem.Vshop.MemberProcessor.GetCurrentMember() == null)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx");
                return;
            }
            //新增判断:如果当前会员已经是门店,则直接跳转到店铺管理页面
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            DistributorsInfo disinfo = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetUserIdDistributors(currentMemberUserId);

            if (disinfo != null && disinfo.UserId > 0)
            {
                //当前用户是店长是禁止店长认证，而非链接跳转 2017-7-31 yk
                this.hidIsStoreManage.Value = "storeManage";
                //this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/DistributorCenter.aspx");
                return;
            }
            else
            {
                DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMemberUserId);
                if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                {
                    //当前用户是店长是禁止店长认证，而非链接跳转 2017-7-31 yk
                    this.hidIsStoreManage.Value = "storeManage";
                    //this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/DistributorCenter.aspx");
                    return;
                }
                else
                { 
                    //普通用户
                    
                }
            }

            //this.FgsName = (DropDownList)this.FindControl("FgsName");

            //DataTable dt = FilialeHelper.GetAllFiliale().Tables[0];
            //this.FgsName.DataSource = dt.DefaultView;
            //this.FgsName.DataTextField="fgsName";
            //this.FgsName.DataValueField = "id";
            //this.FgsName.DataBind();


            //this.litApplicationDescription = (Literal)this.FindControl("litApplicationDescription");
            //this.litButton = (Literal)this.FindControl("litButton");
            //this.customInputs = (Literal)this.FindControl("customInputs");//自定义控件

            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            ////根据自定义xml判断是否显示分销描述
            //if (CustomConfigHelper.Instance.IsDistributorDescriptionOn == "true" && litApplicationDescription != null)
            //    this.litApplicationDescription.Text = masterSettings.ApplicationDescription;
            ////自定义控件的插入
            //if (CustomConfigHelper.Instance.CustomInputs != "" && this.customInputs != null)
            //    this.customInputs.Text = CustomConfigHelper.Instance.CustomInputs;

            //if (litButton != null)
            //    litButton.Text = "<a type=\"button\" class=\"btn btn-danger btn-block btn-apply\" href=\"DistributorValid.aspx?action=" + CustomConfigHelper.Instance.DistributorType_Name + "\">" + CustomConfigHelper.Instance.DistributorType_Showbutton + "</a>";
            ////自定义标题的赋值
            //if (CustomConfigHelper.Instance.ApplicationDescriptionTitle != "分销商描述")
            //    PageTitle.AddSiteNameTitle(CustomConfigHelper.Instance.ApplicationDescriptionTitle);

            this.Page.Session["stylestatus"] = "2";

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sfsq"]) && this.Page.Request.QueryString["sfsq"] == "1")
            {
                this.Page.Response.Redirect("DistributorValid.aspx?sfsq=1&ReferralId=" + this.Page.Request.QueryString["ReferralId"]);
            }

        }



        protected override void OnInit(EventArgs e)
        {
           
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RClerkAuthentication.html";
            }
            base.OnInit(e);
        }
    }
}

