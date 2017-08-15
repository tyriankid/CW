using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System.Web.UI.HtmlControls;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Commodities;
using Hidistro.ControlPanel.Commodities;
using System.Data;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true), WeiXinOAuth(Common.Controls.WeiXinOAuthPage.VMemberCenter)]
    public class VMemberCenter : VWeiXinOAuthTemplatedWebControl
    {
        private Image image;
        private Literal litExpenditure;
        private Literal litMemberGrade;
        private Literal litUserName;
        private Literal litWaitForPayCount;
        private Literal litWaitForRecieveCount;
        private Literal litWaitForReplace;
        private Literal litVantages;//我的积分
        private Literal litGetCoupon;//领取优惠券
        private Literal litMyCouponCount;//我的优惠券数量
        private Literal litMyOfflineMsgCount;//我的未读消息数量
        private Literal litUserType;//用户类别
        private HtmlControl ReturnChangeGoodsArea;//退换货区域
        private HtmlInputHidden isSignOn;//签到活动是否开启隐藏域
        private HtmlInputHidden myOrdersCount;//签到活动是否开启隐藏域
        private HtmlInputHidden memberid;
        private HtmlInputHidden distributorId;
        private Literal litRecommend;
        private Literal litHeadline;

        //2017-07-04， 用户类型
        private HtmlInputHidden memberType;

        protected override void AttachChildControls()
        {
            this.ReturnChangeGoodsArea = (HtmlControl)this.FindControl("ReturnChangeGoodsArea");
            //退换货根据用户的特殊需求来配置
            if (this.ReturnChangeGoodsArea != null)
            {
                ReturnChangeGoodsArea.Visible = CustomConfigHelper.Instance.IsReturnChangeGoodsOn == "false"?false:true;
            }
            
            PageTitle.AddSiteNameTitle("会员中心");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                this.litUserType = (Literal)this.FindControl("litUserType");
                this.litUserName = (Literal)this.FindControl("litUserName");
                this.image = (Image)this.FindControl("image");
                this.litExpenditure = (Literal)this.FindControl("litExpenditure");
                this.litExpenditure.SetWhenIsNotNull(currentMember.Expenditure.ToString("F2"));
                this.litMemberGrade = (Literal)this.FindControl("litMemberGrade");
                this.isSignOn = (HtmlInputHidden)this.FindControl("isSignOn");
                this.myOrdersCount = (HtmlInputHidden)this.FindControl("myOrdersCount");
                this.memberid = (HtmlInputHidden)this.FindControl("memberid");
                this.distributorId = (HtmlInputHidden)this.FindControl("distributorId");
                this.litRecommend = (Literal)this.FindControl("litRecommend");
                this.litHeadline = (Literal)this.FindControl("litHeadline");
                //用户类型
                this.memberType = (HtmlInputHidden)this.FindControl("memberType");
                //通过当前用户id查找分销商信息,从而来判断用户类型
                DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
                if (this.litUserType != null)
                {
                    if (currentDistributor == null)
                    {
                        DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMember.UserId);
                        if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                        {
                            //当前登录为【店员】
                            currentDistributor = DistributorsBrower.GetUserIdDistributors(disSalesinfo.DisUserId);
                            if (disSalesinfo.DsType == 0)
                            {
                                //当前登录为【导购店员】
                                this.litUserType.Text = "导购店员";
                                this.memberType.Value = "1";
                            }
                            else
                            {
                                //当前登录为【服务店员】
                                this.litUserType.Text = "服务店员";
                                this.memberType.Value = "1";
                            }
                        }
                        else
                        {
                            this.litUserType.Text = "普通用户";
                            this.memberType.Value = "0";
                        }
                    }
                    else
                    {
                        this.litUserType.Text = "店长";
                        this.memberType.Value = "2";
                    }
                }
                //根据签到活动的状态,进行相应的功能隐藏显示
                System.Data.DataTable ruleDT = PromoteHelper.GetSignRule();
                if (ruleDT.Rows.Count > 0)
                {
                    isSignOn.Value = ruleDT.Rows[0]["State"].ToString();
                }
                else
                {
                    isSignOn.Value = "0";
                }
                this.memberid.SetWhenIsNotNull(currentMember.UserId.ToString());
                this.distributorId.SetWhenIsNotNull(currentMember.DistributorUserId.ToString());
                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                if (memberGrade != null)
                {
                    this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
                }
                this.litUserName.Text = string.IsNullOrEmpty(currentMember.RealName) ? currentMember.UserName : currentMember.RealName;
                if (!string.IsNullOrEmpty(currentMember.UserHead))
                {
                    this.image.ImageUrl = currentMember.UserHead;
                }
                this.Page.Session["stylestatus"] = "1";

                OrderQuery query = new OrderQuery
                {
                    Status = OrderStatus.All
                };
                int userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.myOrdersCount.Value = userOrderCount.ToString();
                /*
                this.litWaitForRecieveCount = (Literal)this.FindControl("litWaitForRecieveCount");
                this.litWaitForPayCount = (Literal)this.FindControl("litWaitForPayCount");
                this.litWaitForReplace = (Literal)this.FindControl("litWaitForReplace");
                OrderQuery query = new OrderQuery
                {
                    Status = OrderStatus.WaitBuyerPay
                };
                int userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.litWaitForPayCount.SetWhenIsNotNull((userOrderCount > 0) ? ("<i class=\"border-circle\">" + userOrderCount.ToString() + "<i>") : "");
                query.Status = OrderStatus.SellerAlreadySent;
                userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.litWaitForRecieveCount.SetWhenIsNotNull((userOrderCount > 0) ? ("<i class=\"border-circle\">" + userOrderCount.ToString() + "<i>") : "");
                int userOrderReturnCount = MemberProcessor.GetUserOrderReturnCount(Globals.GetCurrentMemberUserId());
                this.litWaitForReplace.SetWhenIsNotNull((userOrderReturnCount > 0) ? ("<i class=\"border-circle\">" + userOrderReturnCount.ToString() + "<i>") : "");
                */
                this.litVantages = (Literal)this.FindControl("litVantages");
                if (litVantages != null)
                {
                    string str = litVantages.Text;
                    litVantages.Text = str + "我的积分:" + currentMember.Points.ToString();
                }
                //领取优惠券
                this.litGetCoupon = (Literal)this.FindControl("litGetCoupon");
                if (this.litGetCoupon != null)
                {
                    //可领取优惠券张数
                    int gCouponsCount=CouponHelper.GetUseableCoupons(currentMember.UserId).Rows.Count;
                    string gCouponsStr=string.Format(gCouponsCount>0?"<i style=\"float: right;margin-right: 20px;background: #F87575;color: #fff;width: 25px;height: 25px;line-height: 25px;text-align: center;text-indent: 0;border-radius: 50%;font-size: 15px;position: relative;top: 50%;transform: translateY(-50%);-webkit-transform: translateY(-50%);-moz-transform: translateY(-50%); font-style:normal;\">" + gCouponsCount + "</i>":"");
                    string str = litGetCoupon.Text;
                    litGetCoupon.Text = string.Format("<div class=\"bottom-content\"><a class=\"red\" style=\"background:#fff url('" + Globals.GetVshopSkinPath() + "/images/iconfont-lingqu.png') 10px center no-repeat;\" href=\"/Vshop/getcoupons.aspx\">领取优惠券{0}</a></div>", gCouponsStr);
                }

                //我的优惠券数量
                this.litMyCouponCount = (Literal)this.FindControl("litMyCouponCount");
                if (this.litMyCouponCount != null)
                {
                    string str = litMyCouponCount.Text;
                    int number = CouponHelper.GetUserCoupons(currentMember.UserId,1).Rows.Count;

                    if(number>0)
                        litMyCouponCount.Text = "<i   style='display:none'>" + str + number + "</i>";
                }


                //我的未读消息数量
                this.litMyOfflineMsgCount = (Literal)this.FindControl("litMyOfflineMsgCount");
                if (litMyOfflineMsgCount != null)
                {
                    DataTable dtNotReadInfo = UserNotReadHelper.GetUserNotReadData(" where JSUserId = " + currentMember.UserId);
                    if(dtNotReadInfo.Rows.Count>0 && dtNotReadInfo.Rows[0]["NotReadMsgCount"].ToInt() > 0)
                    {
                        litMyOfflineMsgCount.Text = "<i style='display:none'>" + dtNotReadInfo.Rows[0]["NotReadMsgCount"] + "</i>";
                    }
                }

            }
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
    }
}

