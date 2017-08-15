using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Handler;
using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Request.Event;
using Hishop.Weixin.MP.Response;
using Hidistro.SaleSystem.Vshop;
using Hidistro.Entities.Members;
using Hidistro.Core.Entities;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;

namespace Hidistro.UI.Web.API
{

    public class CustomMsgHandler : RequestHandler
    {

        public CustomMsgHandler(System.IO.Stream inputStream) : base(inputStream) { }

        public CustomMsgHandler(string xml) : base(xml) { }


        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
            AbstractResponse result;
            if (mismatchReply == null || this.IsOpenManyService())
            {
                result = this.GotoManyCustomerService(requestMessage);
            }
            else
            {
                AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
                if (response == null)
                {
                    result = this.GotoManyCustomerService(requestMessage);
                }
                else
                {
                    response.ToUserName = requestMessage.FromUserName;
                    response.FromUserName = requestMessage.ToUserName;
                    result = response;
                }
            }
            return result;
        }


        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Topic);
            AbstractResponse result;
            if (replies != null && replies.Count > 0)
            {
                foreach (ReplyInfo info in replies)
                {
                    if (info.Keys == key)
                    {
                        TopicInfo topic = VShopHelper.Gettopic(info.ActivityId);
                        if (topic != null)
                        {
                            NewsResponse response = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article item = new Article
                            {
                                Description = topic.Title,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, topic.IconUrl),
                                Title = topic.Title,
                                Url = string.Format("http://{0}/vshop/Topics.aspx?TopicId={1}", System.Web.HttpContext.Current.Request.Url.Host, topic.TopicId)
                            };
                            response.Articles.Add(item);
                            result = response;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list2 = ReplyHelper.GetReplies(ReplyType.Vote);
            if (list2 != null && list2.Count > 0)
            {
                foreach (ReplyInfo info2 in list2)
                {
                    if (info2.Keys == key)
                    {
                        VoteInfo voteById = StoreHelper.GetVoteById((long)info2.ActivityId);
                        if (voteById != null && voteById.IsBackup)
                        {
                            NewsResponse response2 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article2 = new Article
                            {
                                Description = voteById.VoteName,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.ImageUrl),
                                Title = voteById.VoteName,
                                Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.VoteId)
                            };
                            response2.Articles.Add(article2);
                            result = response2;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list3 = ReplyHelper.GetReplies(ReplyType.Wheel);
            if (list3 != null && list3.Count > 0)
            {
                foreach (ReplyInfo info3 in list3)
                {
                    if (info3.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(info3.ActivityId);
                        if (lotteryActivityInfo != null)
                        {
                            NewsResponse response3 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article3 = new Article
                            {
                                Description = lotteryActivityInfo.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityPic),
                                Title = lotteryActivityInfo.ActivityName,
                                Url = string.Format("http://{0}/vshop/BigWheel.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityId)
                            };
                            response3.Articles.Add(article3);
                            result = response3;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list4 = ReplyHelper.GetReplies(ReplyType.Scratch);
            if (list4 != null && list4.Count > 0)
            {
                foreach (ReplyInfo info4 in list4)
                {
                    if (info4.Keys == key)
                    {
                        LotteryActivityInfo info5 = VShopHelper.GetLotteryActivityInfo(info4.ActivityId);
                        if (info5 != null)
                        {
                            NewsResponse response4 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article4 = new Article
                            {
                                Description = info5.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info5.ActivityPic),
                                Title = info5.ActivityName,
                                Url = string.Format("http://{0}/vshop/Scratch.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, info5.ActivityId)
                            };
                            response4.Articles.Add(article4);
                            result = response4;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list5 = ReplyHelper.GetReplies(ReplyType.SmashEgg);
            if (list5 != null && list5.Count > 0)
            {
                foreach (ReplyInfo info6 in list5)
                {
                    if (info6.Keys == key)
                    {
                        LotteryActivityInfo info7 = VShopHelper.GetLotteryActivityInfo(info6.ActivityId);
                        if (info7 != null)
                        {
                            NewsResponse response5 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article5 = new Article
                            {
                                Description = info7.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info7.ActivityPic),
                                Title = info7.ActivityName,
                                Url = string.Format("http://{0}/vshop/SmashEgg.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, info7.ActivityId)
                            };
                            response5.Articles.Add(article5);
                            result = response5;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list6 = ReplyHelper.GetReplies(ReplyType.SignUp);
            if (list6 != null && list6.Count > 0)
            {
                foreach (ReplyInfo info8 in list6)
                {
                    if (info8.Keys == key)
                    {
                        ActivityInfo activity = VShopHelper.GetActivity(info8.ActivityId);
                        if (activity != null)
                        {
                            NewsResponse response6 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article6 = new Article
                            {
                                Description = activity.Description,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, activity.PicUrl),
                                Title = activity.Name,
                                Url = string.Format("http://{0}/vshop/Activity.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, activity.ActivityId)
                            };
                            response6.Articles.Add(article6);
                            result = response6;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list7 = ReplyHelper.GetReplies(ReplyType.Ticket);
            if (list7 != null && list7.Count > 0)
            {
                foreach (ReplyInfo info9 in list7)
                {
                    if (info9.Keys == key)
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(info9.ActivityId);
                        if (lotteryTicket != null)
                        {
                            NewsResponse response7 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article7 = new Article
                            {
                                Description = lotteryTicket.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityPic),
                                Title = lotteryTicket.ActivityName,
                                Url = string.Format("http://{0}/vshop/SignUp.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityId)
                            };
                            response7.Articles.Add(article7);
                            result = response7;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }

        public AbstractResponse GetResponse(ReplyInfo reply, string openId)
        {
            AbstractResponse result;
            if (reply.MessageType == MessageType.Text)
            {
                TextReplyInfo info = reply as TextReplyInfo;
                TextResponse response = new TextResponse
                {
                    CreateTime = System.DateTime.Now,
                    Content = info.Text
                };
                if (reply.Keys == "登录")
                {
                    string str = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", System.Web.HttpContext.Current.Request.Url.Host, openId);
                    response.Content = response.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", str));
                }
                result = response;
            }
            else
            {
                NewsResponse response2 = new NewsResponse
                {
                    CreateTime = System.DateTime.Now,
                    Articles = new System.Collections.Generic.List<Article>()
                };
                foreach (NewsMsgInfo info2 in (reply as NewsReplyInfo).NewsMsg)
                {
                    Article item = new Article
                    {
                        Description = info2.Description,
                        PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info2.PicUrl),
                        Title = info2.Title,
                        Url = string.IsNullOrEmpty(info2.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", System.Web.HttpContext.Current.Request.Url.Host, info2.Id) : info2.Url
                    };
                    response2.Articles.Add(item);
                }
                result = response2;
            }
            return result;
        }

        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            AbstractResponse result;
            if (!this.IsOpenManyService())
            {
                result = null;
            }
            else
            {
                result = new AbstractResponse
                {
                    FromUserName = requestMessage.ToUserName,
                    ToUserName = requestMessage.FromUserName,
                    MsgType = ResponseMsgType.transfer_customer_service
                };
            }
            return result;
        }

        public bool IsOpenManyService()
        {
            return SettingsManager.GetMasterSettings(false).OpenManyService;
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            MenuInfo menu = VShopHelper.GetMenu(System.Convert.ToInt32(clickEventRequest.EventKey));
            AbstractResponse result;
            if (menu == null)
            {
                result = null;
            }
            else
            {
                ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
                if (reply == null)
                {
                    result = null;
                }
                else
                {
                    AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
                    if (keyResponse != null)
                    {
                        result = keyResponse;
                    }
                    else
                    {
                        AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
                        if (response == null)
                        {
                            this.GotoManyCustomerService(clickEventRequest);
                        }
                        response.ToUserName = clickEventRequest.FromUserName;
                        response.FromUserName = clickEventRequest.ToUserName;
                        result = response;
                    }
                }
            }
            return result;
        }

        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            WriteLog("GetSubscribeReply");

            //清除Cookies
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            System.Web.HttpCookie cookies = new System.Web.HttpCookie(cookieStr)
            {
                Value = "-1",
                Expires = System.DateTime.Now.AddHours(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookies);

            //取消关注事件
            ScanVisitDistributor(subscribeEventRequest, subscribeEventRequest.EventKey);

            //关注回复
            ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            AbstractResponse result;
            if (subscribeReply == null)
            {
                WriteLog("subscribeReply == null");
                result = null;
            }
            else
            {
                WriteLog("取消关注再关注,进入这个事件");
                subscribeReply.Keys = "登录";
                AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
                if (response == null)
                {
                    this.GotoManyCustomerService(subscribeEventRequest);
                }
                response.ToUserName = subscribeEventRequest.FromUserName;
                response.FromUserName = subscribeEventRequest.ToUserName;
                result = response;
            }
            return result;
        }
        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            AbstractResponse result;
            if (keyResponse != null)
            {
                result = keyResponse;
            }
            else
            {
                System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
                if (replies == null || (replies.Count == 0 && this.IsOpenManyService()))
                {
                    this.GotoManyCustomerService(textRequest);
                }
                foreach (ReplyInfo info in replies)
                {
                    if (info.MatchType == MatchType.Equal && info.Keys == textRequest.Content)
                    {
                        AbstractResponse response = this.GetResponse(info, textRequest.FromUserName);
                        response.ToUserName = textRequest.FromUserName;
                        response.FromUserName = textRequest.ToUserName;
                        result = response;
                        return result;
                    }
                    if (info.MatchType == MatchType.Like && info.Keys.Contains(textRequest.Content))
                    {
                        AbstractResponse response2 = this.GetResponse(info, textRequest.FromUserName);
                        response2.ToUserName = textRequest.FromUserName;
                        response2.FromUserName = textRequest.ToUserName;
                        result = response2;
                        return result;
                    }
                }
                result = this.DefaultResponse(textRequest);
            }
            return result;
        }

        /// <summary>
        /// 增加扫码事件处理,add by jhb,20151213
        /// </summary>
        /// <param name="scanEventRequest"></param>
        /// <returns></returns>
        public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
        {
            ScanVisitDistributor(scanEventRequest, scanEventRequest.EventKey);
            return null;
        }
        public override AbstractResponse OnEvent_ViewRequest(ViewEventRequest viewEventRequest)
        {
            ScanVisitDistributor(viewEventRequest, viewEventRequest.EventKey);
            return null;
        }


        /// <summary>
        /// 关注访问店铺处理
        /// </summary>
        private void ScanVisitDistributor(EventRequest eventRequest, string strEventKey)
        {
            WriteLog("1.响应微信消息事件:" + eventRequest.Event);
            string[] arrayEventKey = strEventKey.Split('_');
            string openid = eventRequest.FromUserName;
            string referralId = arrayEventKey[arrayEventKey.Length - 1];
            if (!string.IsNullOrEmpty(strEventKey) && strEventKey.Split('_').Length>1)
            {
                WriteLog("2.响应微信消息内容:" + strEventKey);
                switch (arrayEventKey[arrayEventKey.Length-2].ToLower())
                {
                    case "distributor":
                        WriteLog("3.OpenId:" + openid);
                        WriteLog("4.ReferralId:" + referralId);
                        //直接注册
                        wxReister(openid, referralId);
                        /*
                        WriteLog("3.进入枚举_访问店铺处理:" + arrayEventKey[arrayEventKey.Length - 1]);
                        string VisitDistributorID = HiCache.Get(string.Format("DataCache-VisitDistributor-{0}", arrayEventKey[arrayEventKey.Length - 1])) as string;
                        if (string.IsNullOrEmpty(VisitDistributorID))
                        {
                            WriteLog("4.存储访问店铺ID:" + VisitDistributorID);
                            HiCache.Remove(string.Format("DataCache-VisitDistributor-{0}", eventRequest.FromUserName));
                            HiCache.Insert(string.Format("DataCache-VisitDistributor-{0}", eventRequest.FromUserName), arrayEventKey[arrayEventKey.Length - 1], 360
                                , System.Web.Caching.CacheItemPriority.Normal);
                            WriteLog("5.成功存储访问店铺ID:" );
                        }
                         * */
                        break;
                }
                WriteLog("9.响应微信消息结束:" );
            }
        }

        /// <summary>
        /// 注册，数据库中不存在则注册
        /// </summary>
        /// <returns></returns>
        private void wxReister(string openId, string referralId)
        {
            WriteLog("5.开始执行注册:");
            //得到配置文件数据
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            //调用接口
            string tokenurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            string responseResult = this.GetResponseResult(string.Format(tokenurl, masterSettings.WeixinAppId, masterSettings.WeixinAppSecret));
            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;

            WriteLog("6.调用接口取得access_token:" + obj2["access_token"].ToString() + "OpengID:" + openId);

            //IList<go_memberEntity> listmemberEntity = go_memberBusiness.GetListEntity(string.Format("wxid='{0}'", openid));
            MemberInfo currentMember = MemberProcessor.GetOpenIdMember(openId);//微信用户ＯＰＥＮＩＤ
            WriteLog("******开始验证是否存在******");
            if (currentMember != null && currentMember.UserId > 0)
            {
                WriteLog("7.用户OpengID已经存在UserID为：" + currentMember.UserId);
                //存储用户Cookie值
                SetCurrUserCookie(currentMember.UserId.ToString());

                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                if (userIdDistributors != null && userIdDistributors.UserId > 0)
                {
                    //扫码用户为店长,修改用户所属门店用户ID
                    if (currentMember.DistributorUserId == 0)
                    {
                        currentMember.DistributorUserId = userIdDistributors.UserId;
                        MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                    }
                    //存储用户所属门店用户 Cookie值
                    SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                }
                else
                {
                    WriteLog("原始用户重新扫码进入，扫码门店号：" + referralId);
                    ////2017-07-04修改， 非店长用户重新扫码后重新绑定门店 得到所属ID
                    //int idisReferralId = 0;
                    //if (currentMember.DistributorUserId > 0)
                    //{
                    //    idisReferralId = currentMember.DistributorUserId;
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(referralId) && int.TryParse(referralId, out idisReferralId))
                    //    {
                    //        //验证idisReferralId 值得门店是否存在
                    //        DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(idisReferralId);
                    //        if (currDis != null && currDis.UserId > 0)
                    //        {
                    //            WriteLog("准备修改强制关系值为：" + idisReferralId);
                    //            //修改用户所属门店用户ID
                    //            currentMember.DistributorUserId = idisReferralId;
                    //            MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                    //        }
                    //    }
                    //}

                    int idisReferralId = 0;
                    //验证是否为店员
                    DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMember.UserId);
                    if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                    {
                        //扫码用户为店员
                        if (currentMember.DistributorUserId > 0)
                        {
                            idisReferralId = currentMember.DistributorUserId;
                        }
                        else
                        {
                            //店员的所属门店丢失，则强制关系
                            idisReferralId = currentMember.DistributorUserId = disSalesinfo.DisUserId;
                            MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                        }
                    }
                    else
                    {
                        //扫码用户为普通会员 得到所属ID
                        if (!string.IsNullOrEmpty(referralId) && int.TryParse(referralId, out idisReferralId))
                        {
                            //验证idisReferralId 值得门店是否存在
                            DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(idisReferralId);
                            if (currDis == null)
                                idisReferralId = 0;//扫码的门店不存在,则关联主店
                                
                            WriteLog("准备修改强制关系值为：" + idisReferralId);
                            currentMember.DistributorUserId = idisReferralId;
                            MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                        }
                    }

                    //存储用户所属门店用户 Cookie值
                    SetCurrReferralCookie(idisReferralId.ToString());
                }
            }
            else
            {
                WriteLog("7.用户OpengID不存在，准备注册新用户");
                //返回微信用户信息
                string userInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
                string wxUserInfoStr = this.GetResponseResult(string.Format(userInfoUrl, obj2["access_token"].ToString(), openId));
                #region 微信注册用户
                JObject wxUserInfo = JsonConvert.DeserializeObject(wxUserInfoStr) as JObject;
                //添加新用户
                if (this.SkipWinxinOpenId(Globals.UrlDecode(wxUserInfo["nickname"].ToString()), wxUserInfo["openid"].ToString(), wxUserInfo["headimgurl"].ToString(), referralId, 0))
                {
                    currentMember = MemberProcessor.GetOpenIdMember(wxUserInfo["openid"].ToString());//微信用户ＯＰＥＮＩＤ
                    WriteLog("8.注册新用户成功UserId为：" + currentMember.UserId);
                    //保存当前登陆用户Cookie
                    SetCurrUserCookie(currentMember.UserId.ToString());
                    //保存当前登陆用户的所属门店Cookie
                    if (currentMember.DistributorUserId > 0)
                        SetCurrReferralCookie(currentMember.DistributorUserId.ToString());
                }
                #endregion
            }
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 保存用户Cookie
        /// </summary>
        /// <param name="userid"></param>
        private void SetCurrUserCookie(string userid)
        {
            ////设置用户Cookie值
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
                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                cookie.Value = referralId;
                cookie.Expires = DateTime.Now.AddYears(10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            //}
        }

        private bool SkipWinxinOpenId(string userName, string openId, string headimgurl, string referralId, int t)
        {
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
                int idisUseerid = 0;
                int.TryParse(referralId, out idisUseerid);
                //验证idisReferralId 值得门店是否存在
                DistributorsInfo currDis = DistributorsBrower.GetUserIdDistributors(idisUseerid);
                if (currDis != null && currDis.UserId > 0)
                {
                    member.DistributorUserId = idisUseerid;
                }
                #endregion
                
                if (MemberProcessor.CreateMember(member))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                WriteLog("********异常信息：** SkipWinxinOpenId()函数调用引发的异常：" + ex.Message + "**");
            }
            return flag;
        }

        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/wx_login.txt"));

            writer.WriteLine(System.DateTime.Now);

            writer.WriteLine(log);

            writer.Flush();

            writer.Close();
        }

    }
}
