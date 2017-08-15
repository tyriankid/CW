using System;
using System.Data;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hidistro.ControlPanel.Config;

namespace Hidistro.UI.Common.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public abstract class VWeiXinOAuthTemplatedWebControl : VshopTemplatedWebControl
    {

        static readonly bool isWxLogger = true; //微信日志开关

        protected VWeiXinOAuthTemplatedWebControl()
        {
            WxLogger("*******开始执行底层****");
            //得到Url中参数值
            string strReferralId = string.Empty;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                strReferralId = this.Page.Request.QueryString["ReferralId"].ToString();
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isbackshow"]))
            {
                return;//不在登录
            }

            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                WxLogger("*******Cookie中存在值并且Member对象不为空****");
                //验证是否是门店
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                if (userIdDistributors != null && userIdDistributors.UserId > 0)
                {
                    WxLogger("*******当前的登录用户为门店****");
                    //修改用户所属门店用户ID
                    if (currentMember.DistributorUserId == 0)
                    {
                        currentMember.DistributorUserId = userIdDistributors.UserId;
                        MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                    }
                    //将所属门店设置为自己
                    SetCurrReferralCookie(userIdDistributors.UserId.ToString());
                }
                else
                {
                    if (currentMember.DistributorUserId > 0)
                    {
                        WxLogger("*******数据库中存储的值为：******" + currentMember.DistributorUserId);
                        SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                    }
                    else
                    {
                        WxLogger("*******不是门店并且为设置过强制关系****URLReferralId参数值：" + strReferralId);
                        int iReferralId = 0;
                        WxLogger("*******不是门店并且为设置过强制关系****Cookie中参数值：" + GetCurrReferralCookie());
                        if (!string.IsNullOrEmpty(GetCurrReferralCookie()) && int.TryParse(GetCurrReferralCookie(), out iReferralId))
                        {
                            DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(iReferralId);
                            if (currDis != null && currDis.UserId > 0)
                            {
                                WxLogger("设置成功，值为:" + iReferralId);
                                //修改用户所属门店用户ID
                                currentMember.DistributorUserId = iReferralId;
                            }
                        }
                        if (!string.IsNullOrEmpty(strReferralId) && int.TryParse(strReferralId, out iReferralId))
                        {
                            DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(iReferralId);
                            if (currDis != null && currDis.UserId > 0)
                            {
                                //修改用户所属门店用户ID
                                currentMember.DistributorUserId = iReferralId;
                            }
                        }
                        if (currentMember.DistributorUserId > 0)
                        {
                            MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                            WxLogger("*******更新强制关系成功，关系值为：" + currentMember.DistributorUserId);
                            //将所属门店设置为Url参数值
                            SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                        }
                    }
                }
                return;
            }
                
            string currentRequestUrl = HttpContext.Current.Request.Url.ToString();
            currentRequestUrl = System.Text.RegularExpressions.Regex.Replace(currentRequestUrl, "[\f\n\r\t\v]", "");

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            //是否启用微信登录
            if (!masterSettings.IsValidationService)
            {
                WxLogger("********状态信息：**跳转到通用登陆接口" + masterSettings.WeixinLoginUrl + "**");
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx");
                return;
            }   
            
            WeiXinOAuthAttribute oAuth2Attr = Attribute.GetCustomAttribute(this.GetType(), typeof(WeiXinOAuthAttribute)) as WeiXinOAuthAttribute;
            string code = this.Page.Request.QueryString["code"];

            WxLogger("********Request.Url****" + currentRequestUrl);
            WxLogger("********ReferralId****" + this.Page.Request.QueryString["ReferralId"]);

            //如果未到微信网关授权，先授权获取code
            if (string.IsNullOrEmpty(code))
            {
                //WxLogger("********URL中ReferralId值参数值为：****" + state);
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="
                    + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(currentRequestUrl) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                this.Page.Response.Redirect(url, true);
                return;
            }

            if (!string.IsNullOrEmpty(code))
            {
                string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid="
                    + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + code + "&grant_type=authorization_code");

                //获取用户信息失败，跳转至首页
                if (!responseResult.Contains("access_token"))
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/index.aspx");
                    return;
                }

                //获取到用户信息，判断该用户是否已存在本系统数据库
                JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                currentMember = MemberProcessor.GetOpenIdMember(obj2["openid"].ToString());//微信用户ＯＰＥＮＩＤ
                if (currentMember != null && currentMember.UserId > 0)
                {
                    //存储用户Cookie值
                    SetCurrUserCookie(currentMember.UserId.ToString());

                    //验证是否是门店
                    DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                    if (userIdDistributors != null && userIdDistributors.UserId > 0)
                    {
                        //修改用户所属门店用户ID
                        if (currentMember.DistributorUserId == 0)
                        {
                            currentMember.DistributorUserId = userIdDistributors.UserId;
                            MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                        }
                        //将所属门店设置为自己
                        SetCurrReferralCookie(userIdDistributors.UserId.ToString());
                    }
                    else
                    {
                        /*****State_ 2017-07-03修改****/
                        //存储是否店员


                        /*****End_2017-07-03修改****/

                        if (currentMember.DistributorUserId > 0)
                        {
                            SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                        }
                        else
                        { 
                            int iReferralId = 0;
                            if (!string.IsNullOrEmpty(GetCurrReferralCookie()) && int.TryParse(GetCurrReferralCookie(), out iReferralId))
                            {
                                DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(iReferralId);
                                if (currDis != null && currDis.UserId > 0)
                                {
                                    //修改用户所属门店用户ID
                                    currentMember.DistributorUserId = iReferralId;
                                }
                            }
                            if (!string.IsNullOrEmpty(strReferralId) && int.TryParse(strReferralId, out iReferralId))
                            {
                                DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(iReferralId);
                                if (currDis != null && currDis.UserId > 0)
                                {
                                    //修改用户所属门店用户ID
                                    currentMember.DistributorUserId = iReferralId;
                                }
                            }
                            if (currentMember.DistributorUserId > 0)
                            {
                                MemberProcessor.SetUserDistributorUserId(currentMember.UserId, iReferralId);
                                //将所属门店设置为Url参数值
                                SetCurrReferralCookie(iReferralId.ToString());
                            }
                        }
                    }
                }
                else
                {
                    string wxUserInfoStr = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                    if (!wxUserInfoStr.Contains("nickname"))
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/index.aspx");//获取用户昵称失败，跳转至首页
                        return;
                    }
                    JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;
                    if (this.SkipWinxinOpenId(Globals.UrlDecode(wxUserInfo["nickname"].ToString()), wxUserInfo["openid"].ToString(), wxUserInfo["headimgurl"].ToString(), strReferralId, 0))
                    {
                        WxLogger("********状态信息：**微信绑定登录成功**" + currentRequestUrl);
                        currentMember = MemberProcessor.GetOpenIdMember(wxUserInfo["openid"].ToString());//微信用户ＯＰＥＮＩＤ

                        //保存当前登陆用户Cookie
                        SetCurrUserCookie(currentMember.UserId.ToString());
                        //保存当前登陆用户的所属门店Cookie
                        //int ireferralId = 0;
                        //int.TryParse(strReferralId, out ireferralId);
                        //if (ireferralId > 0)
                        if (currentMember.DistributorUserId > 0)
                            SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                        
                        this.Page.Response.Redirect(currentRequestUrl);
                    }
                    else
                    {
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/index.aspx");
                    }
                }
            }
        }

        /// <summary>
        /// 保存用户Cookie
        /// </summary>
        /// <param name="userid"></param>
        private void SetCurrUserCookie(string userid)
        {
            //设置用户Cookie值
            //HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-Member"];
            //if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            //{
            //    cookie.Value = userid;
            //    cookie.Expires = DateTime.Now.AddYears(10);
            //    HttpContext.Current.Response.Cookies.Set(cookie);
            //}
            //else
            //{
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            HttpCookie cookie = new HttpCookie(cookieStr);
                cookie.Value = userid;
                cookie.Expires = DateTime.Now.AddYears(10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            //}
        }

        /// <summary>
        /// 保存用户所属门店Cookie
        /// </summary>
        /// <param name="referralId"></param>
        private void SetCurrReferralCookie(string referralId)
        {
            //存储所属门店Cookie值
            //HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            //if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            //{
            //    cookie.Value = referralId;
            //    cookie.Expires = DateTime.Now.AddYears(10);
            //    HttpContext.Current.Response.Cookies.Set(cookie);
            //}
            //else
            //{
            WxLogger("********状态信息：**即将添加Cookie值" + referralId);
                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                cookie.Value = referralId;
                cookie.Expires = DateTime.Now.AddYears(10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            //}
        }

        ///// <summary>
        ///// 保存用户是否店员Cookie
        ///// </summary>
        ///// <param name="referralId"></param>
        //private void SetCurrCookie(string referralId)
        //{
        //    WxLogger("********状态信息：**即将添加Cookie值" + referralId);
        //    HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
        //    cookie.Value = referralId;
        //    cookie.Expires = DateTime.Now.AddYears(10);
        //    HttpContext.Current.Response.Cookies.Add(cookie);
        //}

        /// <summary>
        /// 从Cookie中获取所属门店信息
        /// </summary>
        /// <returns></returns>
        public string GetCurrReferralCookie()
        {
            string strResult = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if (cookie != null)
                strResult = cookie.Value;
            return strResult;
        }

        /// <summary>
        /// 获取推荐人ID
        /// </summary>
        /// <returns></returns>
        string GetReferralUserId()
        {
            string ReferralUserId = "";

            try
            {
                //跳转过来的URL
                Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;

                if (null != urlReferrer && !string.IsNullOrWhiteSpace(urlReferrer.Query))
                {
                    string querystr = "";
                    if (urlReferrer.Query.StartsWith("?")) querystr = urlReferrer.Query.Substring(1);

                    foreach (string item in querystr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.Contains("ReferralUserId"))
                        {
                            ReferralUserId = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                            break;
                        }
                    }
                }
            }
            catch { }

            return ReferralUserId;
        }

        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse)WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }


        bool SkipWinxinOpenId(string userName, string openId, string headimgurl, string referralId, int t)
        {
            WxLogger("      状态信息：**　进入　SkipWinxinOpenId　函数体 **");
            bool flag = false;
            try
            {
                string generateId = Globals.GetGenerateId();

                MemberInfo member = new MemberInfo();

                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = userName;
                member.RealName = userName;
                if (t == 0)
                {
                    member.OpenId = openId;
                    member.RegSource = 0;//微信注册
                }
                else if (t == 1)
                {
                    member.AliOpenId = openId;
                    member.RegSource = 1;//支付宝
                }
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now.AddYears(10);
                member.Email = generateId + "@localhost.com";
                member.SessionId = generateId;
                member.Password = generateId;
                member.RealName = string.Empty;
                member.Address = string.Empty;
                member.UserHead = headimgurl;//用户头像
                #region 设置所属门店用户ID
                int iReferralId = 0;
                int.TryParse(referralId, out iReferralId);
                DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(iReferralId);
                if (currDis != null && currDis.UserId > 0)
                {
                    member.DistributorUserId = iReferralId;
                }
                #endregion
                WxLogger(" 调式A **" + member.OpenId);
                if (MemberProcessor.CreateMember(member))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                WxLogger("      异常信息：** SkipWinxinOpenId()函数调用引发的异常：" + ex.Message + "**");
            }
            return flag;
        }



        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="log"></param>
        void WxLogger(string log)
        {

            if (!isWxLogger) return;

            string logFile = Page.Request.MapPath("~/wx_login.txt");

            File.AppendAllText(logFile, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));

        }

        /// <summary>
        /// 判断用户有没有关注公众号
        /// </summary>
        /// <returns></returns>
        private bool checkWxEx(string openid)
        {
            WxLogger("      checkWx：");
            HttpCookie cookie = HttpContext.Current.Request.Cookies["wx_subscribe"];
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                //读取配置信息
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                WxLogger("      checkWx：" + masterSettings.GuidePageSet);
                if (masterSettings.IsValidationService && masterSettings.GuidePageSet != "") //是否启用微信登录
                {

                    #region 取到了code,说明用户同意了授权登录

                    string responseResult = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&grant_type=client_credential");
                    WxLogger("      responseResult：" + responseResult);
                    if (responseResult.Contains("access_token"))
                    {
                        WxLogger("      得到token：");
                        JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                        //string openId = obj2["openid"].ToString();//微信用户ＯＰＥＮＩＤ
                        //WxLogger("      openId_openId：" + openId);
                        string wxUserInfoStr = this.GetResponseResult("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + obj2["access_token"].ToString() + "&openid=" + openid + "&lang=zh_CN");
                        WxLogger("      wxUserInfoStr：" + wxUserInfoStr);
                        if (wxUserInfoStr.Contains("subscribe"))
                        {
                            WxLogger("      checkWx_openId：" + openid);
                            JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;
                            WxLogger("      subscribe：" + wxUserInfo["subscribe"]);
                            if (Convert.ToInt32(wxUserInfo["subscribe"]) != 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/index.aspx");

                        }

                    }


                    #endregion

                }
                else
                {
                    return true;
                }
            }
            else
            {
                WxLogger("      cookie：" + cookie.Value);
                return true;
            }
            return false;
        }

    }
}

