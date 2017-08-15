using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.CWAPI;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hishop.Plugins;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    public class VshopProcess : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private IDictionary<string, string> jsondict = new Dictionary<string, string>();

        private void AddCommissions(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            string str = "";


            if (Convert.ToInt32(DateTime.Now.Day) <= 31)
            {
                //输入的金额大于等于 查询汇总上月27之前（）
                string time = "";
                DateTime dtts = DateTime.Now;
                decimal num = decimal.Parse(httpContext.Request["commissionmoney"].Trim());
                //if (dtts.Day < 28)
                if (dtts.Day < 26)
                {
                    if ((Convert.ToInt32(dtts.Month) - 1) == 0)
                    {
                        //time =(Convert.ToInt32(dtts.Year)-1).ToString() + "-" + (Convert.ToInt32(dtts.Month) - 1+12).ToString() + "-" + "27";
                        time = (Convert.ToInt32(dtts.Year) - 1).ToString() + "-12-" + "26";
                    }
                    else
                    {
                        //time = dtts.Year + "-" + (Convert.ToInt32(dtts.Month) - 1).ToString() + "-" + "27";
                        time = dtts.Year + "-" + (Convert.ToInt32(dtts.Month) - 1).ToString() + "-" + "26";
                    }
                }
                else
                {
                    //time = dtts.Year + "-" + dtts.Month + "-" + "27";
                    time = dtts.Year + "-" + dtts.Month + "-" + "26";
                }
                int currentMemberUserId = Globals.GetCurrentMemberUserId();
                DataTable dttx = DistributorsBrower.GetCommosionByTime("hc.UserId='" + currentMemberUserId + "' and hc.TradeTime<'" + time + "'");
                if (num <= Convert.ToDecimal(dttx.Rows[0]["ktx"]))
                {
                    if (DistributorsBrower.IsExitsCommionsRequest())
                    {
                        str = "{\"success\":false,\"msg\":\"您的申请正在审核中！\"}";
                    }
                    else if (this.CheckAddCommissions(httpContext, ref str))
                    {
                        string str1 = httpContext.Request["account"].Trim();
                        int num1 = 0;
                        int.TryParse(httpContext.Request["requesttype"].Trim(), out num1);
                        BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo()
                        {
                            MerchanCade = str1,
                            Amount = num,
                            RequesType = num1
                        };
                        str = (!DistributorsBrower.AddBalanceDrawRequest(balanceDrawRequestInfo) ? "{\"success\":false,\"msg\":\"真实姓名或手机号未填写！\"}" : "{\"success\":true,\"msg\":\"申请成功！\"}");
                    }
                }
                else
                {
                    str = "{\"success\":false,\"msg\":\"提现金额不能大于可提现金额！\"}";
                }
            }
            else
            {
                str = "{\"success\":false,\"msg\":\"请在每个月1～5号进行提现！\"}";
            }

            httpContext.Response.Write(str);
            httpContext.Response.End();
        }

        private void SendOrderGoods(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            httpContext.Response.Write(btnSendGoods_Click(httpContext.Request["orderId"].Trim(), httpContext.Request["shippingModeId"].Trim()));
            httpContext.Response.End();
        }

        private string btnSendGoods_Click(string orderId, string shippingModeId)
        {
            string str = "{\"success\":false,\"msg\":\"发货失败\"}";
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember != null)
                {
                    if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
                    {
                        str = "{\"success\":false,\"msg\":\"当前订单为团购订单，团购活动还未成功结束，所以不能发货\"}";
                    }
                    else
                    {
                        if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
                        {
                            str = "{\"success\":false,\"msg\":\"当前订单状态没有付款或不是等待发货的订单，所以不能发货\"}";
                        }
                        else
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(int.Parse(shippingModeId), true);
                            orderInfo.RealShippingModeId = shippingMode.ModeId; //配送方式
                            orderInfo.RealModeName = shippingMode.Name;
                            orderInfo.ShipOrderNumber = "";         //物流单号

                            if (OrderHelper.SendGoodsEx(orderInfo, MemberProcessor.GetCurrentMember().UserName))
                            {
                                #region bb
                                SendNoteInfo info5 = new SendNoteInfo();
                                info5.NoteId = Globals.GetGenerateId();
                                info5.OrderId = orderId;
                                info5.Operator = currentMember.UserName;
                                info5.Remark = "门店" + info5.Operator + "发货成功";
                                OrderHelper.SaveSendNote(info5);
                                MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
                                Messenger.OrderShipping(orderInfo, member);
                                if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
                                {
                                    #region cc
                                    if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                                    {
                                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                        if (paymentMode != null)
                                        {
                                            PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
														{
															paymentMode.Gateway
														})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
														{
															paymentMode.Gateway
														})), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                        }
                                    }
                                    if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
                                    {
                                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                        PayClient client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                                        DeliverInfo deliver = new DeliverInfo
                                        {
                                            TransId = orderInfo.GatewayOrderId,
                                            OutTradeNo = orderInfo.OrderId,
                                            OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                                        };
                                        client.DeliverNotify(deliver);
                                    }
                                    #endregion
                                }
                                orderInfo.OnDeliver();
                                str = "{\"success\":true,\"msg\":\"发货成功\"}";
                                #endregion
                            }
                            else
                            {
                                str = "{\"success\":false,\"msg\":\"发货失败\"}";
                            }
                        }
                    }
                }
            }
            return str;
        }



        private bool CheckRequestDistributors(HttpContext httpContext, StringBuilder stringBuilder)
        {

            if (string.IsNullOrEmpty(httpContext.Request["stroename"]))
            {
                stringBuilder.AppendFormat("请输入店铺名称", new object[0]);
                return false;
            }

            if (httpContext.Request["stroename"].Length > 20)
            {
                stringBuilder.AppendFormat("请输入店铺名称字符不多于20个字符", new object[0]);
                return false;
            }

            if (string.IsNullOrEmpty(httpContext.Request["descriptions"]) || httpContext.Request["descriptions"].Trim().Length <= 30)
            {
                return true;
            }

            stringBuilder.AppendFormat("店铺描述字不能多于30个字", new object[0]);

            return false;

        }



        public void AddDistributor(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            StringBuilder stringBuilder = new StringBuilder();
            if (!this.CheckRequestDistributors(context, stringBuilder))
            {
                context.Response.Write(string.Concat("{\"success\":false,\"msg\":\"", stringBuilder.ToString(), "\"}"));
                return;
            }
            //验证后台账户是否存在
            string username = context.Request["allherecode"].Trim();
            if (ManagerHelper.GetManager(username) != null)
            {
                context.Response.Write(string.Concat("{\"success\":false,\"msg\":\"", "此金力账号已经在管理端中存在。", "\"}"));
                return;
            }

            DistributorsInfo distributorsInfo = new DistributorsInfo()
            {
                StoreId = Convert.ToInt32(context.Request["stroeid"].Trim()),
                RequestAccount = context.Request["acctount"].Trim(),
                StoreName = context.Request["stroename"].Trim(),
                StoreDescription = context.Request["descriptions"].Trim(),
                BackImage = context.Request["BackImage"].Trim(),
                Logo = context.Request["logo"].Trim(),
                DistriGradeId = DistributorGradeBrower.GetIsDefaultDistributorGradeInfo().GradeId
            };
            if (!DistributorsBrower.AddDistributors(distributorsInfo))
            {
                context.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新输入店铺名称\"}");
                return;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            if (HttpContext.Current.Request.Cookies[cookieStr] != null)
            {
                int iuserId = 0;
                string userId = Globals.GetCurrentMemberUserId().ToString();
                if (int.TryParse(userId, out iuserId))
                {
                    //修改数据库所属门店值
                    MemberProcessor.SetUserDistributorUserId(iuserId, iuserId);
                }

                //设置cookies值
                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                cookie.Value = userId;
                cookie.Expires = DateTime.Now.AddYears(10);
                HttpContext.Current.Response.Cookies.Add(cookie);

                //建立后台账号
                ManagerInfo manager = new ManagerInfo();
                if (masterSettings.StoreRoleId > 0)
                {
                    manager.RoleId = masterSettings.StoreRoleId;
                    manager.UserName = context.Request["allherecode"].Trim();
                    manager.Password = HiCryptographer.Md5Encrypt("888888");
                    manager.Email = "";
                    manager.CreateDate = DateTime.Now;
                    //manager.ClientUserId = distributorsInfo.StoreId;
                    manager.ClientUserId = distributorsInfo.UserId;
                    manager.AgentName = distributorsInfo.StoreName;
                    if (ManagerHelper.Create(manager)) { }
                }
            }
            context.Response.Write("{\"success\":true}");
        }


        //分销商下加商品
        private void DeleteDistributorProducts(System.Web.HttpContext context)
        {

            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {
                string json = context.Request["Params"];

                JObject source = JObject.Parse(json);

                if (source.Count > 0)
                {
                    List<int> productIds = (from s in source.Values() select (int)s).ToList<int>();
                    DistributorsBrower.DeleteDistributorProductIds(productIds);
                    // DistributorsBrower.DeleteDistributorProductIds((
                    //    from s in source.Values()
                    //   select System.Convert.ToInt32(s)).ToList<int>());
                }

            }

            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");

            context.Response.End();

        }

        //分销商上架商品
        private void AddDistributorProducts(System.Web.HttpContext context)
        {
            //无法将类型为“Newtonsoft.Json.Linq.JValue”的对象强制转换为类型“System.IConvertible”。

            if (!string.IsNullOrEmpty(context.Request["Params"]))
            {

                string json = context.Request["Params"];

                JObject source = JObject.Parse(json);

                if (source.Count > 0)
                {

                    List<int> productIds = (from s in source.Values() select (int)s).ToList<int>();

                    DistributorsBrower.AddDistributorProductId(productIds);

                }

            }

            context.Response.Write("{\"success\":\"true\",\"msg\":\"保存成功\"}");
            context.Response.End();
        }
        private void AddFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"请先登录才可以收藏商品\"}");
            }
            else
            {
                if (ProductBrowser.AddProductToFavorite(System.Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                }
            }
        }
        private void AddProductConsultations(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (ProductBrowser.FindUserNoReply(currentMember.UserId, System.Convert.ToInt32(context.Request["ProductId"])) < 3)
            {
                ProductConsultationInfo productConsultation = new ProductConsultationInfo
                {
                    ConsultationDate = System.DateTime.Now,
                    ConsultationText = context.Request["ConsultationText"],
                    ProductId = System.Convert.ToInt32(context.Request["ProductId"]),
                    UserEmail = currentMember.Email,
                    UserId = currentMember.UserId,
                    UserName = currentMember.UserName,
                    DistributorUserId = currentMember.DistributorUserId
                };
                if (ProductBrowser.InsertProductConsultation(productConsultation))
                {
                    //咨询提醒门店或管理员
                    MemberInfo disMember = null;
                    string strUrl = string.Empty;
                    if (currentMember.DistributorUserId > 0)
                    {
                        disMember = MemberProcessor.GetMember(currentMember.DistributorUserId);
                        strUrl = context.Request.Url.Host + "/Vshop/MyConsult.aspx";
                    }
                    else
                    {
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                        disMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                    }
                    Messenger.UserConsultation(strUrl, disMember, currentMember, productConsultation);

                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                }
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的咨询次数过多，请等待管理员回复以后再进行咨询\"}");
            }
        }

        /// <summary>
        /// 门店回答咨询
        /// </summary>
        /// <param name="context"></param>
        private void ProductConsultationsReplyed(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int consultationId = 0;
            if (context.Request["ConsultationId"] != null && int.TryParse(context.Request["ConsultationId"].ToString(), out consultationId) && context.Request["ReplyText"] != null)
            {
                //获取咨询实体信息
                ProductConsultationInfo productconsultationinfo = ProductCommentHelper.GetProductConsultation(consultationId);
                if (productconsultationinfo != null && productconsultationinfo.ConsultationId > 0)
                {
                    //构建回答内容
                    productconsultationinfo.ReplyText = context.Request["ReplyText"].ToString();
                    productconsultationinfo.ReplyDate = DateTime.Now;
                    productconsultationinfo.ReplyUserId = currentMember.UserId;//当前操作的门店Id，当前操作人就是门店
                    //提交回答
                    if (ProductCommentHelper.ReplyProductConsultation(productconsultationinfo))
                    {
                        if (productconsultationinfo.DistributorUserId > 0)
                        {
                            string storeName = string.Empty;
                            string strUrl = context.Request.Url.Host + "/Vshop/MyConsultations.aspx";
                            //咨询回复提醒用户
                            DistributorsInfo disinfo = DistributorsBrower.GetDistributorInfo(productconsultationinfo.DistributorUserId);
                            if (disinfo != null)
                                storeName = disinfo.StoreName;

                            MemberInfo userMember = MemberProcessor.GetMember(productconsultationinfo.UserId);
                            Messenger.UserConsultationReplyed(strUrl, userMember, storeName, productconsultationinfo);
                        }

                        context.Response.Write("{\"success\":true}");
                    }
                    else
                        context.Response.Write("{\"success\":false, \"msg\":\"保存数据发送错误\"}");
                }
                else
                    context.Response.Write("{\"success\":false, \"msg\":\"咨询ID查询出错\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"参数传递错误\"}");
            }
        }

        private void AddProductReview(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int productId = System.Convert.ToInt32(context.Request["ProductId"]);
            int num2;
            int num3;
            ProductBrowser.LoadProductReview(productId, currentMember.UserId, out num2, out num3);
            if (num2 == 0)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您没有购买此商品(或此商品的订单尚未完成)，因此不能进行评论\"}");
            }
            else
            {
                if (num3 >= num2)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"您已经对此商品进行过评论(或此商品的订单尚未完成)，因此不能再次进行评论\"}");
                }
                else
                {
                    ProductReviewInfo review = new ProductReviewInfo
                    {
                        ReviewDate = System.DateTime.Now,
                        ReviewText = context.Request["ReviewText"],
                        ProductId = productId,
                        UserEmail = currentMember.Email,
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName
                    };
                    if (ProductBrowser.InsertProductReview(review))
                    {
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"提交失败\"}");
                    }
                }
            }
        }
        private void AddShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    TelPhone = context.Request.Form["telphone"],
                    Zipcode = "",
                    IsDefault = true,
                    UserId = currentMember.UserId,
                    RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.AddShippingAddress(shippingAddress) > 0)
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        /// <summary>
        /// 新增增值税发票信息
        /// </summary>
        /// <param name="context"></param>
        private void AddReceipt(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                context.Response.Write("{\"success\":false}");
            else
            {
                UserReceiptInfo receiptinfo = new UserReceiptInfo
                {
                    UserId = currentMember.UserId,
                    Type = 1,
                    CompanyName = context.Request.Form["companyName"],
                    TaxesCode = context.Request.Form["taxesCode"],
                    Address = context.Request.Form["address"],
                    Phone = context.Request.Form["phone"],
                    Bank = context.Request.Form["bank"],
                    BankNumber = context.Request.Form["bankNumber"],
                    RegistrationImg = context.Request.Form["registrationImg"],
                    EmpowerEntrustImg = context.Request.Form["empowerEntrustImg"],
                    TaxpayerProveImg = context.Request.Form["taxpayerProveImg"],
                    IsDefault = 1
                };
                if (UserReceiptInfoHelper.AddUserReceiptInfo(receiptinfo))
                    context.Response.Write("{\"success\":true}");
                else
                    context.Response.Write("{\"success\":false}");
            }
        }

        /// <summary>
        /// 新增电子发票信息
        /// </summary>
        /// <param name="context"></param>
        private void AddReceiptDz(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                context.Response.Write("{\"success\":false}");
            else
            {
                int type1 = 0;
                if (context.Request.Form["type1"] != null && int.TryParse(context.Request.Form["type1"], out type1))
                {
                    UserReceiptInfo receiptinfo = new UserReceiptInfo
                    {
                        UserId = currentMember.UserId,
                        Type = 0,
                        Type1 = type1,
                        CompanyName = context.Request.Form["companyName"],
                        TaxesCode = context.Request.Form["taxesCode"],
                        Address = context.Request.Form["address"],
                        Phone = context.Request.Form["phone"],
                        Bank = context.Request.Form["bank"],
                        BankNumber = context.Request.Form["bankNumber"],
                        RegistrationImg = context.Request.Form["registrationImg"],
                        EmpowerEntrustImg = context.Request.Form["empowerEntrustImg"],
                        TaxpayerProveImg = context.Request.Form["taxpayerProveImg"],
                        IsDefault = 1
                    };
                    if (UserReceiptInfoHelper.AddUserReceiptInfo(receiptinfo))
                        context.Response.Write("{\"success\":true}");
                    else
                        context.Response.Write("{\"success\":false}");
                }
                else
                    context.Response.Write("{\"success\":false}");
            }
        }


        /// <summary>
        /// 编辑增值税发票信息
        /// </summary>
        /// <param name="context"></param>
        private void EditReceipt(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int receiptId = 0;
            if (context.Request.Form["receiptId"] == null || string.IsNullOrEmpty(context.Request.Form["receiptId"].ToString()))
            {
                context.Response.Write("{\"success\":false}");
            }
            if (!int.TryParse(context.Request.Form["receiptId"].ToString(), out receiptId))
            {
                context.Response.Write("{\"success\":false}");
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                UserReceiptInfo receiptinfo = UserReceiptInfoHelper.GetUserReceiptInfo(receiptId);
                receiptinfo.Type = 1;
                receiptinfo.CompanyName = context.Request.Form["companyName"].ToString();
                receiptinfo.TaxesCode = context.Request.Form["taxesCode"].ToString();
                receiptinfo.Address = context.Request.Form["address"].ToString();
                receiptinfo.Phone = context.Request.Form["phone"].ToString();
                receiptinfo.Bank = context.Request.Form["bank"].ToString();
                receiptinfo.BankNumber = context.Request.Form["bankNumber"].ToString();
                receiptinfo.RegistrationImg = context.Request.Form["registrationImg"].ToString();
                receiptinfo.EmpowerEntrustImg = context.Request.Form["empowerEntrustImg"].ToString();
                receiptinfo.TaxpayerProveImg = context.Request.Form["taxpayerProveImg"].ToString();
                receiptinfo.IsDefault = 1;
                if (UserReceiptInfoHelper.UpdateUserReceiptInfo(receiptinfo))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        /// <summary>
        /// 编辑电子发票信息
        /// </summary>
        /// <param name="context"></param>
        private void EditReceiptDz(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int receiptId = 0;
            int type1 = 0;
            if (context.Request.Form["receiptId"] == null || string.IsNullOrEmpty(context.Request.Form["receiptId"].ToString()) || context.Request.Form["type1"] == null || string.IsNullOrEmpty(context.Request.Form["type1"].ToString()))
            {
                context.Response.Write("{\"success\":false}");
            }
            if (!int.TryParse(context.Request.Form["receiptId"].ToString(), out receiptId))
            {
                context.Response.Write("{\"success\":false}");
            }
            if (!int.TryParse(context.Request.Form["type1"], out type1))
            {
                context.Response.Write("{\"success\":false}");
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                UserReceiptInfo receiptinfo = UserReceiptInfoHelper.GetUserReceiptInfo(receiptId);
                receiptinfo.Type = 0;
                receiptinfo.Type1 = type1;
                receiptinfo.CompanyName = context.Request.Form["companyName"].ToString();
                receiptinfo.TaxesCode = context.Request.Form["taxesCode"].ToString();
                receiptinfo.IsDefault = 1;
                if (UserReceiptInfoHelper.UpdateUserReceiptInfo(receiptinfo))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }


        private void AddSignUp(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = System.Convert.ToInt32(context.Request["id"]);
            string str = System.Convert.ToString(context.Request["code"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            if (!string.IsNullOrEmpty(lotteryTicket.InvitationCode) && lotteryTicket.InvitationCode != str)
            {
                context.Response.Write("{\"success\":false, \"msg\":\"邀请码不正确\"}");
            }
            else
            {
                if (lotteryTicket.EndTime < System.DateTime.Now)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
                }
                else
                {
                    if (lotteryTicket.OpenTime < System.DateTime.Now)
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"报名已结束\"}");
                    }
                    else
                    {
                        if (VshopBrowser.GetUserPrizeRecord(activityid) == null)
                        {
                            PrizeRecordInfo model = new PrizeRecordInfo
                            {
                                ActivityID = activityid
                            };
                            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                            model.UserID = currentMember.UserId;
                            model.UserName = currentMember.UserName;
                            model.IsPrize = true;
                            model.Prizelevel = "已报名";
                            model.PrizeTime = new System.DateTime?(System.DateTime.Now);
                            VshopBrowser.AddPrizeRecord(model);
                            context.Response.Write("{\"success\":true, \"msg\":\"报名成功\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"success\":false, \"msg\":\"你已经报名了，请不要重复报名！\"}");
                        }
                    }
                }
            }
        }
        private void AddTicket(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int activityid = System.Convert.ToInt32(context.Request["activityid"]);
            LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(activityid);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null && !lotteryTicket.GradeIds.Contains(currentMember.GradeId.ToString()))
            {
                context.Response.Write("{\"success\":false, \"msg\":\"您的会员等级不在此活动范内\"}");
            }
            else
            {
                if (lotteryTicket.EndTime < System.DateTime.Now)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"活动已结束\"}");
                }
                else
                {
                    if (System.DateTime.Now < lotteryTicket.OpenTime)
                    {
                        context.Response.Write("{\"success\":false, \"msg\":\"抽奖还未开始\"}");
                    }
                    else
                    {
                        if (VshopBrowser.GetCountBySignUp(activityid) < lotteryTicket.MinValue)
                        {
                            context.Response.Write("{\"success\":false, \"msg\":\"还未达到人数下限\"}");
                        }
                        else
                        {
                            PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                            try
                            {
                                if (!lotteryTicket.IsOpened)
                                {
                                    VshopBrowser.OpenTicket(activityid);
                                    userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(userPrizeRecord.RealName) && !string.IsNullOrWhiteSpace(userPrizeRecord.CellPhone))
                                    {
                                        context.Response.Write("{\"success\":false, \"msg\":\"您已经抽过奖了\"}");
                                        return;
                                    }
                                }
                                if (userPrizeRecord == null || string.IsNullOrEmpty(userPrizeRecord.PrizeName))
                                {
                                    context.Response.Write("{\"success\":false, \"msg\":\"很可惜,你未中奖\"}");
                                    return;
                                }
                                if (!userPrizeRecord.PrizeTime.HasValue)
                                {
                                    userPrizeRecord.PrizeTime = new System.DateTime?(System.DateTime.Now);
                                    VshopBrowser.UpdatePrizeRecord(userPrizeRecord);
                                }
                            }
                            catch (System.Exception exception)
                            {
                                context.Response.Write("{\"success\":false, \"msg\":\"" + exception.Message + "\"}");
                                return;
                            }
                            context.Response.Write("{\"success\":true, \"msg\":\"恭喜你获得" + userPrizeRecord.Prizelevel + "\"}");
                        }
                    }
                }
            }
        }
        private void AddUserPrize(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["activityid"], out result);
            string str = context.Request["prize"];
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            PrizeRecordInfo model = new PrizeRecordInfo
            {
                PrizeTime = new System.DateTime?(System.DateTime.Now),
                UserID = Globals.GetCurrentMemberUserId(),
                ActivityName = lotteryActivity.ActivityName,
                ActivityID = result,
                Prizelevel = str
            };
            string text = str;
            switch (text)
            {
                case "一等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[0].PrizeName;
                    model.IsPrize = true;
                    goto IL_216;
                case "二等奖":
                    model.PrizeName = (model.PrizeName = lotteryActivity.PrizeSettingList[1].PrizeName);
                    model.IsPrize = true;
                    goto IL_216;
                case "三等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[2].PrizeName;
                    model.IsPrize = true;
                    goto IL_216;
                case "四等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[3].PrizeName;
                    model.IsPrize = true;
                    goto IL_216;
                case "五等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[4].PrizeName;
                    model.IsPrize = true;
                    goto IL_216;
                case "六等奖":
                    model.PrizeName = lotteryActivity.PrizeSettingList[5].PrizeName;
                    model.IsPrize = true;
                    goto IL_216;
            }
            model.IsPrize = false;
        IL_216:
            VshopBrowser.AddPrizeRecord(model);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.Write(builder);
        }

        private bool CheckAddCommissions(HttpContext httpContext, ref string strPointers)
        {
            int num = 0;
            int.TryParse(httpContext.Request["requesttype"].Trim(), out num);
            if (num != 0)
            {
                num = 1;
            }
            if (num == 1 && string.IsNullOrEmpty(httpContext.Request["account"].Trim()))
            {
                strPointers = "{\"success\":false,\"msg\":\"支付宝账号不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(httpContext.Request["commissionmoney"].Trim()))
            {
                strPointers = "{\"success\":false,\"msg\":\"提现金额不允许为空！\"}";
                return false;
            }
            if (decimal.Parse(httpContext.Request["commissionmoney"].Trim()) <= new decimal(0))
            {
                strPointers = "{\"success\":false,\"msg\":\"提现金额必须大于0的纯数字！\"}";
                return false;
            }
            if (!(new System.Text.RegularExpressions.Regex("^[0-9]*[1-9][0-9]*$").IsMatch(httpContext.Request["commissionmoney"].Trim())))
            {
                strPointers = "{\"success\":false,\"msg\":\"请输入正整数！\"}";
                return false;
            }
            decimal num1 = new decimal(0);
            decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out num1);
            if (num1 > new decimal(0) && decimal.Parse(httpContext.Request["commissionmoney"].Trim()) < new decimal(0))
            {
                strPointers = string.Concat("{\"success\":false,\"msg\":\"提现金额必须大于等于", num1.ToString(), "元！\"}");
                return false;
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if (decimal.Parse(httpContext.Request["commissionmoney"].Trim()) <= currentDistributors.ReferralBlance)
            {
                return true;
            }
            strPointers = "{\"success\":false,\"msg\":\"提现金额必须为小于现有佣金余额！\"}";
            return false;
        }


        private void CheckFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                if (ProductBrowser.ExistsProduct(System.Convert.ToInt32(context.Request["ProductId"]), currentMember.UserId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        /// <summary>
        /// 得到库存信息
        /// </summary>
        /// <param name="context"></param>
        private void GetStock(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int istock = 0;
            string productid = context.Request["ProductId"];
            if (!string.IsNullOrEmpty(productid))
            {
                DataTable dtproduct = ProductBrowser.GetProducts(string.Format("ProductId = '{0}'", productid));
                if (dtproduct.Rows.Count > 0)
                {
                    istock = CwAPI.GetStock(dtproduct.Rows[0]["ProductCode"].ToString());
                    context.Response.Write("{\"success\":true,\"istock\":\"" + istock + "\"}");
                }
                else
                    context.Response.Write("{\"success\":false,\"istock\":\"" + istock + "\"}");
            }
            else
                context.Response.Write("{\"success\":false,\"istock\":\"" + istock + "\"}");
        }

        private bool CheckUpdateDistributors(HttpContext httpContext, StringBuilder stringBuilder)
        {

            if (string.IsNullOrEmpty(httpContext.Request["stroename"]))
            {
                stringBuilder.Append("请输入店铺名称");
                return false;
            }

            if (httpContext.Request["stroename"].Length > 20)
            {
                stringBuilder.Append("请输入店铺名称字符不多于20个字符");
                return false;
            }

            if (string.IsNullOrEmpty(httpContext.Request["descriptions"]) || httpContext.Request["descriptions"].Trim().Length <= 30)
            {
                return true;
            }

            stringBuilder.Append("店铺描述字不能多于30个字");

            return false;

        }

        private void DelFavorite(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (ProductBrowser.DeleteFavorite(System.Convert.ToInt32(context.Request["favoriteId"])) == 1)
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false, \"msg\":\"取消失败\"}");
            }
        }
        private void DelShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.DelShippingAddress(System.Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }

        private void DelReceipt(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (context.Request.Form["receiptId"] != null && !string.IsNullOrEmpty(context.Request.Form["receiptId"].ToString()))
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember == null)
                {
                    context.Response.Write("{\"success\":false}");
                }
                else
                {
                    int userId = currentMember.UserId;
                    if (UserReceiptInfoHelper.DeleteReceipt(System.Convert.ToInt32(context.Request.Form["receiptId"])))
                    {
                        context.Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false}");
                    }
                }
            }
            else
                context.Response.Write("{\"success\":false}");
        }

        //确认收货(先版本确认收货后，在7天内可以发起退换货)
        private void ConfirmTakeGoodsOrder(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            string orderId = Convert.ToString(httpContext.Request["orderId"]);
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            LineItemInfo lineItemInfo = new LineItemInfo();
            foreach (KeyValuePair<string, LineItemInfo> lineItem in lineItems)
            {
                lineItemInfo = lineItem.Value;
                if (lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForRefund && lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForReturns)
                {
                    continue;
                }
                flag = true;
            }
            if (flag)
            {
                httpContext.Response.Write("{\"success\":false, \"msg\":\"订单中商品有退货(款)不允许确认收货\"}");
                return;
            }
            //修改订单状态
            if (orderInfo == null || !MemberProcessor.ConfirmOrderTakeGoods(orderInfo))
            {
                httpContext.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许确认收货\"}");
                return;
            }
            foreach (LineItemInfo value in orderInfo.LineItems.Values)
            {
                if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString() && !CustomConfigHelper.Instance.AnonymousOrder)
                {
                    continue;
                }
                //将订单明细状态标记为确认收货
                ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, (int)OrderStatus.ConfirmTakeGoods);
            }
            httpContext.Response.Write(string.Concat("{\"success\":true}"));
        }

        //确认收货(原始确认收货就是完成状态）
        private void FinishOrder(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            bool flag = false;
            string str = Convert.ToString(httpContext.Request["orderId"]);
            bool isMicroPay = Convert.ToBoolean(httpContext.Request["isMicroPay"]);//是否微信扫码支付

            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str);
            if (isMicroPay)
            {
                orderInfo.RealModeName = "微信扫码支付";
            }
            Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
            LineItemInfo lineItemInfo = new LineItemInfo();
            foreach (KeyValuePair<string, LineItemInfo> lineItem in lineItems)
            {
                lineItemInfo = lineItem.Value;
                if (lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForRefund && lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForReturns)
                {
                    continue;
                }
                flag = true;
            }
            if (flag)
            {
                httpContext.Response.Write("{\"success\":false, \"msg\":\"订单中商品有退货(款)不允许完成\"}");
                return;
            }

            #region 供应商商品订单才运行调用AH接口 ——先调用订单完成接口
            if (orderInfo.OrderSource == 2)
            {
                //2016-11-15 开始调用完成订单接口
                StringBuilder strJson = new StringBuilder();
                strJson.Append("{");
                strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//订单号
                //strJson.AppendFormat("\"FinishDate\":\"{0}\"", orderInfo.FinishDate);//订单完成时间
                strJson.AppendFormat("\"FinishDate\":\"{0}\",", DateTime.Now);//订单完成时间
                strJson.Append("}");
                CwAHAPI.CwapiLog("发送数据：" + strJson);
                AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                string strResult = ahservice.MPFTOJL_DHD_QS(strJson.ToString());
                CwAHAPI.CwapiLog("返回内容：" + strResult);
                string orderid = string.Empty;
                string message = string.Empty;
                if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                {
                    httpContext.Response.Write("{\"success\":false, \"msg\":\"" + string.Format("订单完成接口调用失败，原因为：{0}", message) + "\"}");
                    return;
                }
                if (orderid != orderInfo.OrderId)
                {
                    httpContext.Response.Write("{\"success\":false, \"msg\":\"订单完成接口调用失败，原因为：接口返回的订单编码与发送时不一致。\"}");
                    return;
                }
            }
            #endregion 先调用订单完成接口

            if (orderInfo == null || !MemberProcessor.ConfirmOrderFinish(orderInfo))
            {
                httpContext.Response.Write("{\"success\":false, \"msg\":\"订单当前状态不允许完成\"}");
                return;
            }
            DistributorsBrower.UpdateCalculationCommission(orderInfo);//增加佣金记录、更新分销商的有效推广佣金和订单总额
            foreach (LineItemInfo value in orderInfo.LineItems.Values)
            {
                if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString() && !CustomConfigHelper.Instance.AnonymousOrder)
                {
                    continue;
                }
                ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, 5);
            }

            //遍历订单车中包含[会员充值]的商品,生成等价的优惠券给用户
            if (CustomConfigHelper.Instance.CouponRecharge)
            {
                //找到所有会员充值的商品
                List<LineItemInfo> couponGoodList = lineItems.Values.Where(n => ProductHelper.GetProductBaseInfo(n.ProductId).ProductName == "会员充值").ToList();
                //创建后台couponInfo
                foreach (LineItemInfo info in couponGoodList)
                {
                    for (int i = 0; i < info.Quantity; i++)
                    {
                        CouponInfo target = new CouponInfo
                        {
                            Name = "会员充值" + info.ItemAdjustedPrice.ToString("F2") + "元",
                            ClosingTime = DateTime.MaxValue,
                            StartTime = DateTime.Now,
                            Amount = 0M,
                            DiscountValue = info.ItemAdjustedPrice,
                            NeedPoint = 0
                        };
                        string lotNumber = string.Empty;

                        int resultCouponId = CouponHelper.CreateCoupon(target, 0);
                        IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
                        if (resultCouponId > 0) //如果后台新建成功或者已经存在,开始对用户进行发送
                        {
                            CouponItemInfo item = new CouponItemInfo();
                            string claimCode = string.Empty;
                            claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                            item = new CouponItemInfo(resultCouponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
                            listCouponItem.Add(item);
                        }
                        CouponHelper.SendClaimCodes(resultCouponId, listCouponItem);
                    }
                }
            }

            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            int num = 0;
            //if (masterSettings.IsRequestDistributor && !string.IsNullOrEmpty(masterSettings.FinishedOrderMoney.ToString()) && currentMember.Expenditure >= masterSettings.FinishedOrderMoney)
            //{
            //    num = 1;
            //}
            //DistributorsInfo distributorsInfo = new DistributorsInfo();
            //distributorsInfo = DistributorsBrower.GetUserIdDistributors(orderInfo.UserId);
            //if (distributorsInfo != null && distributorsInfo.UserId > 0)
            //{
            //    num = 0;
            //}
            httpContext.Response.Write(string.Concat("{\"success\":true,\"isapply\":", num, "}"));
        }

        /// <summary>
        /// 得到订单编码
        /// </summary>
        /// <returns></returns>
        private string GenerateOrderId()
        {
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + str;
        }

        /// <summary>
        /// 得到服务商品核销码
        /// </summary>
        /// <returns></returns>
        private string GenerateServiceCode()
        {
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 4; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return str;
        }

        //private string GenerateOrderIdByOrder(int randomLength=7)
        //{
        //    //订单id由前八位日期,后七位随机素组成
        //    //新增参数randomLength,如果传来的参数为7,则后面7位全部为随机数,若是3,则后3位是当天生成的订单数补0
        //    string str = string.Empty;
        //    System.Random random = new System.Random();
        //    for (int i = 0; i < randomLength; i++)
        //    {
        //        int num = random.Next();
        //        str += ((char)(48 + (ushort)(num % 10))).ToString();
        //    }
        //    int leftNumCount = 7-randomLength;
        //    if (leftNumCount > 0)
        //    {
        //        string currentOrderNum = (OrderHelper.GetTodayOrderNum()+1).ToString();
        //        string leftNumStr = currentOrderNum.PadLeft(leftNumCount, '0');
        //        str += leftNumStr;
        //    }
        //    return System.DateTime.Now.ToString("yyyyMMdd") + str;
        //}

        private void GetPrize(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            int result = 1;
            //string name = context.Request["RealName"].ToString();
            //string phone = context.Request["CellPhone"].ToString();
            //string address = context.Request["Address"].ToString();
            int.TryParse(context.Request["activityid"], out result);
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(result);
            int userPrizeCount = VshopBrowser.GetUserPrizeCount(result);
            MemberInfo currMemberInfo = null;
            if (MemberProcessor.GetCurrentMember() == null)
            {
                currMemberInfo = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                currMemberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
                currMemberInfo.UserName = "";
                currMemberInfo.OpenId = "";
                currMemberInfo.CreateDate = System.DateTime.Now;
                currMemberInfo.SessionId = generateId;
                currMemberInfo.SessionEndTime = System.DateTime.Now;
                MemberProcessor.CreateMember(currMemberInfo);
                currMemberInfo = MemberProcessor.GetMember(generateId);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();

                System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                {
                    Value = currMemberInfo.UserId.ToString(),
                    Expires = System.DateTime.Now.AddYears(10)
                };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                //if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(address))
                //{
                currMemberInfo = MemberProcessor.GetCurrentMember();
                //currMemberInfo.RealName = name;
                //currMemberInfo.CellPhone = phone;
                //currMemberInfo.Address = address;
                //MemberHelper.UpdateNoLog(currMemberInfo);
                //}
            }
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (userPrizeCount >= lotteryActivity.MaxNum)
            {
                builder.Append("\"No\":\"-1\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                if (System.DateTime.Now < lotteryActivity.StartTime)
                {
                    builder.Append("\"No\":\"-3\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                }
                else if (System.DateTime.Now > lotteryActivity.EndTime)
                {
                    builder.Append("\"No\":\"-4\"");
                    builder.Append("}");
                    context.Response.Write(builder.ToString());
                }
                else
                {
                    PrizeQuery page = new PrizeQuery
                    {
                        ActivityId = result
                    };
                    System.Collections.Generic.List<PrizeRecordInfo> prizeList = VshopBrowser.GetPrizeList(page);
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    if (prizeList != null && prizeList.Count > 0)
                    {
                        num3 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "一等奖");
                        num4 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "二等奖");
                        num5 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "三等奖");
                    }
                    PrizeRecordInfo model = new PrizeRecordInfo
                    {
                        PrizeTime = new System.DateTime?(System.DateTime.Now),
                        UserID = Globals.GetCurrentMemberUserId(),
                        //RealName = name,
                        //CellPhone = phone,
                        ActivityName = lotteryActivity.ActivityName,
                        ActivityID = result,
                        IsPrize = true
                    };

                    //是否中的积分
                    bool isIntegral = false;
                    System.Collections.Generic.List<PrizeSetting> prizeSettingList = lotteryActivity.PrizeSettingList;
                    decimal num6 = prizeSettingList[0].Probability * 100m;
                    decimal num7 = prizeSettingList[1].Probability * 100m;
                    decimal num8 = prizeSettingList[2].Probability * 100m;
                    int num9 = new System.Random(System.Guid.NewGuid().GetHashCode()).Next(1, 10001);
                    if (prizeSettingList.Count > 3)
                    {
                        decimal num10 = prizeSettingList[3].Probability * 100m;
                        decimal num11 = prizeSettingList[4].Probability * 100m;
                        decimal num12 = prizeSettingList[5].Probability * 100m;
                        int num13 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "四等奖");
                        int num14 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "五等奖");
                        int num15 = prizeList.Count((PrizeRecordInfo a) => a.Prizelevel == "六等奖");
                        if (num9 < num6 && prizeSettingList[0].PrizeNum > num3)
                        {
                            model.Prizelevel = "一等奖";
                            model.PrizeName = prizeSettingList[0].PrizeName;
                            builder.Append("\"No\":\"9\",\"JE\":\"" + model.PrizeName + "\"");
                        }
                        else
                        {
                            if (num9 < num7 && prizeSettingList[1].PrizeNum > num4)
                            {
                                model.Prizelevel = "二等奖";
                                model.PrizeName = prizeSettingList[1].PrizeName;
                                builder.Append("\"No\":\"11\",\"JE\":\"" + model.PrizeName + "\"");
                            }
                            else
                            {
                                if (num9 < num8 && prizeSettingList[2].PrizeNum > num5)
                                {
                                    model.Prizelevel = "三等奖";
                                    model.PrizeName = prizeSettingList[2].PrizeName;
                                    builder.Append("\"No\":\"1\",\"JE\":\"" + model.PrizeName + "\"");
                                }
                                else
                                {
                                    if (num9 < num10 && prizeSettingList[3].PrizeNum > num13)
                                    {
                                        model.Prizelevel = "四等奖";
                                        model.PrizeName = prizeSettingList[3].PrizeName;
                                        //判断是否中奖是积分
                                        string strSendMsg = "";
                                        if (prizeSettingList[3].Integral > 0)
                                        {
                                            isIntegral = true;//中奖积分
                                            int? isendIntegral = prizeSettingList[3].Integral;
                                            //是这增加积分
                                            //实例化积分实体类
                                            PointDetailInfo point = new PointDetailInfo
                                            {
                                                OrderId = "拆礼盒抽奖送积分",
                                                UserId = currMemberInfo.UserId,
                                                TradeDate = DateTime.Now,
                                                TradeType = PointTradeType.Bounty,
                                                Increased = isendIntegral,
                                                Points = currMemberInfo.Points + isendIntegral.Value
                                            };
                                            if ((point.Points > 0x7fffffff) || (point.Points < 0))
                                            {
                                                point.Points = 0;
                                            }
                                            //送积分
                                            try
                                            {
                                                new PointDetailDao().AddPointDetail(point);//积分detail表
                                                currMemberInfo.Points = currMemberInfo.Points + isendIntegral.Value;
                                                MemberHelper.UpdateNoLog(currMemberInfo);//积分主表
                                                strSendMsg = "积分赠送成功";
                                            }
                                            catch (Exception ex)
                                            {
                                                strSendMsg = "积分赠送失败";
                                            }
                                            HiCache.Remove(string.Format("DataCache-Member-{0}", currMemberInfo.UserId));
                                        }
                                        builder.Append("\"No\":\"3\",\"JE\":\"" + model.PrizeName + "\",\"Msg\":\"" + strSendMsg + "\"");
                                    }
                                    else
                                    {
                                        if (num9 < num11 && prizeSettingList[4].PrizeNum > num14)
                                        {
                                            model.Prizelevel = "五等奖";
                                            model.PrizeName = prizeSettingList[4].PrizeName;

                                            //判断是否中奖是积分
                                            string strSendMsg = "";
                                            if (prizeSettingList[4].Integral > 0)
                                            {
                                                isIntegral = true;//中奖积分
                                                int? isendIntegral = prizeSettingList[4].Integral;
                                                //是这增加积分
                                                //实例化积分实体类
                                                PointDetailInfo point = new PointDetailInfo
                                                {
                                                    OrderId = "拆礼盒抽奖送积分",
                                                    UserId = currMemberInfo.UserId,
                                                    TradeDate = DateTime.Now,
                                                    TradeType = PointTradeType.Bounty,
                                                    Increased = isendIntegral,
                                                    Points = currMemberInfo.Points + isendIntegral.Value
                                                };
                                                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                                                {
                                                    point.Points = 0;
                                                }
                                                //送积分
                                                try
                                                {
                                                    new PointDetailDao().AddPointDetail(point);//积分detail表
                                                    currMemberInfo.Points = currMemberInfo.Points + isendIntegral.Value;
                                                    MemberHelper.UpdateNoLog(currMemberInfo);//积分主表
                                                    strSendMsg = "积分赠送成功";
                                                }
                                                catch (Exception ex)
                                                {
                                                    strSendMsg = "积分赠送失败";
                                                }
                                                HiCache.Remove(string.Format("DataCache-Member-{0}", currMemberInfo.UserId));
                                            }
                                            builder.Append("\"No\":\"5\",\"JE\":\"" + model.PrizeName + "\",\"Msg\":\"" + strSendMsg + "\"");
                                        }
                                        else
                                        {
                                            if (num9 < num12 && prizeSettingList[5].PrizeNum > num15)
                                            {
                                                model.Prizelevel = "六等奖";
                                                model.PrizeName = prizeSettingList[5].PrizeName;

                                                //判断是否中奖是积分
                                                string strSendMsg = "";
                                                if (prizeSettingList[5].Integral > 0)
                                                {
                                                    isIntegral = true;//中奖积分
                                                    int? isendIntegral = prizeSettingList[4].Integral;
                                                    //是这增加积分
                                                    //实例化积分实体类
                                                    PointDetailInfo point = new PointDetailInfo
                                                    {
                                                        OrderId = "拆礼盒抽奖送积分",
                                                        UserId = currMemberInfo.UserId,
                                                        TradeDate = DateTime.Now,
                                                        TradeType = PointTradeType.Bounty,
                                                        Increased = isendIntegral,
                                                        Points = currMemberInfo.Points + isendIntegral.Value
                                                    };
                                                    if ((point.Points > 0x7fffffff) || (point.Points < 0))
                                                    {
                                                        point.Points = 0;
                                                    }
                                                    //送积分
                                                    try
                                                    {
                                                        new PointDetailDao().AddPointDetail(point);//积分detail表
                                                        currMemberInfo.Points = currMemberInfo.Points + isendIntegral.Value;
                                                        MemberHelper.UpdateNoLog(currMemberInfo);//积分主表
                                                        strSendMsg = "积分赠送成功";
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        strSendMsg = "积分赠送失败";
                                                    }
                                                    HiCache.Remove(string.Format("DataCache-Member-{0}", currMemberInfo.UserId));
                                                }
                                                builder.Append("\"No\":\"7\",\"JE\":\"" + model.PrizeName + "\",\"Msg\":\"" + strSendMsg + "\"");
                                            }
                                            else
                                            {
                                                model.IsPrize = false;
                                                builder.Append("\"No\":\"0\",\"JE\":\"0\"");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (num9 < num6 && prizeSettingList[0].PrizeNum > num3)
                        {
                            model.Prizelevel = "一等奖";
                            model.PrizeName = prizeSettingList[0].PrizeName;
                            builder.Append("\"No\":\"9\",\"JE\":\"" + model.PrizeName + "\"");
                        }
                        else
                        {
                            if (num9 < num7 && prizeSettingList[1].PrizeNum > num4)
                            {
                                model.Prizelevel = "二等奖";
                                model.PrizeName = prizeSettingList[1].PrizeName;
                                builder.Append("\"No\":\"11\",\"JE\":\"" + model.PrizeName + "\"");
                            }
                            else
                            {
                                if (num9 < num8 && prizeSettingList[2].PrizeNum > num5)
                                {
                                    model.Prizelevel = "三等奖";
                                    model.PrizeName = prizeSettingList[2].PrizeName;
                                    builder.Append("\"No\":\"1\",\"JE\":\"" + model.PrizeName + "\"");
                                }
                                else
                                {
                                    model.IsPrize = false;
                                    builder.Append("\"No\":\"0\",\"JE\":\"0\"");
                                }
                            }
                        }
                    }
                    if (context.Request["activitytype"] != "scratch")
                    {
                        //VshopBrowser.AddPrizeRecord(model);

                        #region ActivityType=9,微信红包
                        //___________增加红包领取,2016-02-01,type=9
                        string strRedMessage = "1";
                        int amount = 0;
                        int.TryParse(model.PrizeName, out amount);
                        if (amount > 0)
                        {
                            switch (lotteryActivity.ActivityType)
                            {
                                case 9:
                                    currMemberInfo = MemberProcessor.GetCurrentMember();
                                    strRedMessage = SendRedPack(currMemberInfo.OpenId, "恭喜您领取微信红包成功!", "参与微信红包活动成功", "微信红包活动", amount * 100, 0);
                                    break;
                            }

                        }
                        if (strRedMessage == "1")
                        {
                            int RecordId = VshopBrowser.AddPrizeRecord(model);
                            if (RecordId > 0 && model.IsPrize && !isIntegral)
                            {
                                builder.Append(",\"RecordId\":\"" + RecordId + "\"");
                            }
                            builder.Append("}");
                        }
                        else if (strRedMessage.IndexOf("余额不足") > -1)
                        {
                            model.IsPrize = false;
                            model.RealName = model.CellPhone = model.Prizelevel = model.PrizeName = null;
                            model.PrizeName = "余额不足";
                            VshopBrowser.AddPrizeRecord(model);
                            builder = new System.Text.StringBuilder();
                            builder.Append("{");
                            builder.Append("\"No\":\"0\",\"Msg\":\"" + strRedMessage + "\"");
                            builder.Append("}");
                        }

                        //红包领取失败，提示
                        if (strRedMessage != "1" && strRedMessage.IndexOf("余额不足") == -1)
                        {
                            builder = new System.Text.StringBuilder();
                            builder.Append("{");
                            builder.Append("\"No\":\"-9\",\"Msg\":\"" + strRedMessage + "\"");
                            builder.Append("}");
                        }
                        #endregion

                    }

                    context.Response.Write(builder.ToString());
                }
            }
        }

        /// <summary>
        /// 修改中奖用户的信息
        /// </summary>
        /// <param name="context"></param>
        private void SetPrizeRecord(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            builder.Append("\"Result\":\"NO\"");
            builder.Append("}");
            int recordid = 0;
            string name = context.Request["Name"].ToString();
            string phone = context.Request["Phone"].ToString();
            string address = context.Request["Address"].ToString();
            string editmember = context.Request["EditMember"].ToString();
            int.TryParse(context.Request["RecordId"], out recordid);
            if (recordid > 0 && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(editmember))
            {
                PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecordById(recordid);
                userPrizeRecord.RealName = name;
                userPrizeRecord.CellPhone = phone;
                if (VshopBrowser.UpdatePrizeRecordById(userPrizeRecord))
                {
                    if (editmember == "1")
                    {
                        MemberInfo currMemberInfo = MemberProcessor.GetCurrentMember();
                        currMemberInfo.RealName = name;
                        currMemberInfo.CellPhone = phone;
                        currMemberInfo.Address = address;
                        MemberHelper.UpdateNoLog(currMemberInfo);
                    }
                    builder.Clear();
                    builder.Append("{");
                    builder.Append("\"Result\":\"OK\"");
                    builder.Append("}");
                }
            }
            context.Response.Write(builder.ToString());
        }


        /// <summary>
        /// 修改门店绑定的地址信息
        /// </summary>
        /// <param name="context"></param>
        private void BindStoreLocation(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            string module = context.Request["module"].ToString();
            string lat = context.Request["lat"].ToString();
            string lng = context.Request["lng"].ToString();
            string poiaddress = context.Request["poiaddress"].ToString();
            string poiname = context.Request["poiname"].ToString();
            string cityname = context.Request["cityname"].ToString();

            if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lng) && !string.IsNullOrEmpty(poiname) && !string.IsNullOrEmpty(poiaddress))
            {
                DistributorsInfo disinfo = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
                if (disinfo != null && disinfo.UserId > 0)
                {
                    disinfo.Location_module = module;
                    disinfo.Location_lat = Convert.ToDouble(lat);
                    disinfo.Location_lng = Convert.ToDouble(lng);
                    disinfo.Location_poiaddress = poiaddress;
                    disinfo.Location_poiname = poiname;
                    disinfo.Location_cityname = cityname;
                    if (DistributorsBrower.SetDistributorsLocation(disinfo))
                        builder.Append("\"Result\":\"OK\"");
                    else
                        builder.Append("\"Result\":\"NO\",\"Msg\":\"绑定地址出错！\"");
                }
                else
                    builder.Append("\"Result\":\"NO\",\"Msg\":\"当前不是店长操作！\"");
            }
            else
                builder.Append("\"Result\":\"NO\",\"Msg\":\"数据传递错误！\"");
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 发送红包
        /// </summary>
        public string SendRedPack(string re_openid, string wishing, string act_name, string remark, int amount, int sendredpackrecordid)
        {
            string empty = string.Empty;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!masterSettings.EnableWeiXinRequest)
            {
                empty = "管理员后台未开启微信付款！";
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime dateTime = DateTime.Parse(string.Concat(now.ToString("yyyy-MM-dd"), " 00:00:01"));
                DateTime dateTime1 = DateTime.Parse(string.Concat(now.ToString("yyyy-MM-dd"), " 08:00:00"));
                if (now > dateTime && now < dateTime1)
                {
                    empty = "北京时间0：00-8：00不触发红包赠送";
                }
                else if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinPartnerID) || string.IsNullOrEmpty(masterSettings.WeixinPartnerKey) || string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword))
                {
                    empty = "系统微信发红包配置接口未配置好";
                }
                else if (!string.IsNullOrEmpty(re_openid))
                {
                    string siteName = masterSettings.SiteName;
                    string str = masterSettings.SiteName;
                    RedPackClient redPackClient = new RedPackClient();
                    empty = redPackClient.SendRedpack(masterSettings.WeixinAppId, masterSettings.WeixinPartnerID, "", siteName, str, re_openid, wishing, Globals.IPAddress, act_name, remark, amount, masterSettings.WeixinPartnerKey, masterSettings.WeixinCertPath, masterSettings.WeixinCertPassword, sendredpackrecordid);
                }
                else
                {
                    empty = "用户未绑定微信";
                }
            }
            return empty;
        }

        public void GetShippingTypes(System.Web.HttpContext context)
        {
            int regionId = System.Convert.ToInt32(context.Request["regionId"]);
            int groupbuyId = (!string.IsNullOrWhiteSpace(context.Request["groupBuyId"])) ? System.Convert.ToInt32(context.Request["groupBuyId"]) : 0;
            int countdownId = (!string.IsNullOrWhiteSpace(context.Request["countDownId"])) ? System.Convert.ToInt32(context.Request["countDownId"]) : 0;
            int cutdownId = (!string.IsNullOrWhiteSpace(context.Request["cutdownId"])) ? System.Convert.ToInt32(context.Request["cutdownId"]) : 0;
            int num2;
            ShoppingCartInfo shoppingCart;
            Hidistro.Core.Entities.SiteSettings masterSettings = Hidistro.Core.SettingsManager.GetMasterSettings(false);
            if (int.TryParse(context.Request["buyAmount"], out num2) && !string.IsNullOrWhiteSpace(context.Request["productSku"]))
            {
                string productSkuId = System.Convert.ToString(context.Request["productSku"]);
                if (groupbuyId > 0)
                {
                    shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(groupbuyId, productSkuId, num2);
                }
                else if (countdownId > 0)//抢购
                {
                    shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(productSkuId, num2);
                }
                else if (cutdownId > 0)//砍价
                {
                    shoppingCart = ShoppingCartProcessor.GetCutDownShoppingCart(productSkuId, num2, cutdownId);
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, num2);
                }
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }

            /*******star******2017-8-4 线下商品加入配送地址区域验证验证 yk******/
            if (shoppingCart.LineItems[0].ProductSource == 4 && regionId != 0)
            {

                switch (shoppingCart.LineItems[0].RegionId.Length)
                {
                    #region  根据分公司线下商品的配送区域来适配用户收获地址
                    case 2:
                        if (shoppingCart.LineItems[0].RegionId != regionId.ToString().Substring(0, 2))
                        {
                            context.Response.Write("{\"Status\":-1}");
                            return;
                        }
                        break;
                    case 4:
                        if (shoppingCart.LineItems[0].RegionId != regionId.ToString().Substring(0, 4))
                        {
                            context.Response.Write("{\"Status\":-1}");
                            return;
                        }
                        break;
                    case 6:
                        if (shoppingCart.LineItems[0].RegionId != regionId.ToString())
                        {
                            context.Response.Write("{\"Status\":-1}");
                            return;
                        }
                        break;

                    #endregion
                }
            }
            /*******end************/
            System.Collections.Generic.IEnumerable<int> source =
                from item in ShoppingProcessor.GetShippingModes()
                select item.ModeId;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            bool isAllFreeShipping = false;
            bool isAddShipping = false;
            if (masterSettings.SpecialOrderAmountType == "freeShipping")
                isAllFreeShipping = shoppingCart.GetAmount() >= masterSettings.SpecialValue1 && masterSettings.SpecialValue1 > 0M;
            else if (masterSettings.SpecialOrderAmountType == "addShipping")
                isAddShipping = shoppingCart.GetAmount() < masterSettings.SpecialValue1 && masterSettings.SpecialValue1 > 0M;

            if (source != null && source.Count<int>() > 0)
            {
                foreach (int num3 in source)
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(num3, true);
                    decimal num4 = 0m;
                    decimal numFree = 0m;
                    if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping))
                    {
                        num4 = ShoppingProcessor.CalcFreight(regionId, shoppingCart.Weight, shippingMode);
                        //if (isAllFreeShipping)//如果达到了设置好的包邮金额,那么邮费全免
                        //{
                        //    num4 = 0M;
                        //}
                    }
                    //else
                    //    num4 = ShoppingProcessor.CalcFreight(regionId, shoppingCart.Weight, shippingMode);
                    if (isAllFreeShipping)//满额包邮
                    {
                        builder.Append(string.Concat(new string[]
					    {
						    ",{\"modelId\":\"",
						    shippingMode.ModeId.ToString(),
						    "\",\"text\":\"",
						    shippingMode.Name,
						    "： ￥",
						    ("<k style='text-decoration:line-through;'>"+num4.ToString("F2")+"</k>")+("(满"+masterSettings.SpecialValue1+"包邮)"),
						    "\",\"freight\":\"",
						    numFree.ToString("F2"),
						    "\"}"
					    }));
                    }
                    else if (isAddShipping)//不满额加价
                    {
                        builder.Append(string.Concat(new string[]
					    {
						    ",{\"modelId\":\"",
						    shippingMode.ModeId.ToString(),
						    "\",\"text\":\"",
						    shippingMode.Name,
						    "： ￥",
						    (num4+masterSettings.SpecialValue2).ToString("F2"),//+("(不满"+masterSettings.SpecialValue1+"加收"+masterSettings.SpecialValue2+"元)"),
						    "\",\"freight\":\"",
						    (num4+masterSettings.SpecialValue2).ToString("F2"),
						    "\"}"
					    }));
                    }
                    else//正常
                    {
                        builder.Append(string.Concat(new string[]
					    {
						    ",{\"modelId\":\"",
						    shippingMode.ModeId.ToString(),
						    "\",\"text\":\"",
						    shippingMode.Name,
						    "： ￥",
						    num4 == 0 ? num4.ToString("F2") + "(包邮)" : num4.ToString("F2"),
						    "\",\"freight\":\"",
						    num4.ToString("F2"),
						    "\"}"
					    }));
                    }

                }
                if (builder.Length > 0)
                {
                    builder.Remove(0, 1);
                }
            }
            builder.Insert(0, "{\"data\":[").Append("]}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }
        private void ProcessAddToCartBySkus(System.Web.HttpContext context)
        {

            if (MemberProcessor.GetCurrentMember() == null && ManagerHelper.GetCurrentManager() == null)
            {
                context.Response.Write("{\"Status\":-1}");
            }
            else
            {
                context.Response.ContentType = "application/json";
                int quantity = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
                string skuId = context.Request["productSkuId"];
                int categoryId = int.Parse(context.Request["categoryid"], NumberStyles.None);
                ShoppingCartProcessor.AddLineItem(skuId, quantity, categoryId);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write(string.Concat(new string[]
				{
					"{\"Status\":\"OK\",\"TotalMoney\":\"",
					shoppingCart.GetTotal().ToString(".00"),
					"\",\"Quantity\":\"",
					shoppingCart.GetQuantity().ToString(),
					"\"}"
				}));
            }
        }
        private void ProcessChageQuantity(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int result = 1;
            int.TryParse(context.Request["quantity"], out result);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            //20150921新增判断:如果取不到skuID,并且取到了GiftId表示这是一个积分兑换礼品.
            int skuStock;
            int giftId = Convert.ToInt32(context.Request["giftID"]);
            if (skuId == null && giftId != null)
            {
                skuStock = GiftProcessor.GetGiftStock(giftId);
                if (result > skuStock)
                {
                    builder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                    result = skuStock;
                }
                else
                {
                    builder.Append("\"Status\":\"OK\",");
                    ShoppingCartProcessor.UpdateGiftItemQuantity(giftId, (result > 0) ? result : 1);
                    builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart().GetAmount());
                }
            }
            else
            {
                skuStock = ShoppingCartProcessor.GetSkuStock(skuId);
                if (result > skuStock)
                {
                    builder.AppendFormat("\"Status\":\"{0}\"", skuStock);
                    result = skuStock;
                }
                else
                {
                    builder.Append("\"Status\":\"OK\",");
                    ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (result > 0) ? result : 1);
                    ////增加后台订单的购物车处理
                    //int pcUserid = 0;
                    //if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && ManagerHelper.GetCurrentManager() != null &&context.Request.UrlReferrer.AbsoluteUri.IndexOf("admin")>-1)
                    //{
                    //    pcUserid = ManagerHelper.GetCurrentManager().UserId;
                    //    ShoppingCartProcessor.UpdateLineItemQuantityPC(skuId, (result > 0) ? result : 1, pcUserid);
                    //}
                    //else
                    //{
                    //    
                    //}
                    //builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart(pcUserid).GetAmount());
                }
            }


            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }
        /// <summary>
        /// 删除购物车中的商品和礼品
        /// update@20150923 by hj,新增了对购物车中礼品的删除
        /// </summary>
        /// <param name="context"></param>
        private void ProcessDeleteCartProduct(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"];
            int giftId = Convert.ToInt32(context.Request["giftId"]);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(skuId))
            {
                //增加后台订单的购物车处理
                if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && ManagerHelper.GetCurrentManager() != null)
                {
                    ShoppingCartProcessor.RemoveLineItemPC(skuId, ManagerHelper.GetCurrentManager().UserId);
                }
                else
                {
                    ShoppingCartProcessor.RemoveLineItem(skuId);
                }
            }
            if (giftId != null)
                ShoppingCartProcessor.RemoveGiftItem(giftId);

            builder.Append("{");
            builder.Append("\"Status\":\"OK\"");
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }
        /// <summary>
        /// 买一送一，操作购物车数据库表
        /// </summary>
        /// <param name="context"></param>
        private void BuyToGive(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            double TopPrice = 0.00;//购物车中最高价格值
            //double AvgPrice = 0.00;//购物车商品单价的平均价
            //double SumPrice = 0.00;//购物车商品单价的综合
            int SumQuantity = 0;
            int SumGiveQuantity = 0;
            int ReadQuantity = 0;
            int ReadGiveQuantity = 0;

            double price = 0;//变量送商品的价格值
            string strSkuId = string.Empty;
            //得到当前用户购物车数据信息
            int pcUserid = 0;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && ManagerHelper.GetCurrentManager() != null)
            {
                pcUserid = ManagerHelper.GetCurrentManager().UserId;
            }
            DataTable dtCart = ShoppingCartProcessor.GetCartShopping(pcUserid);
            //得到购物车中最高金额值
            if (dtCart.Rows.Count > 0)
            {
                double.TryParse(dtCart.Rows[0]["SalePrice"].ToString(), out TopPrice);
            }
            //循环取得,总购买数量，已送总数量，商品单价总和
            foreach (DataRow dr in dtCart.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Quantity"].ToString()))
                {
                    SumQuantity += int.Parse(dr["Quantity"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["GiveQuantity"].ToString()))
                {
                    SumGiveQuantity += int.Parse(dr["GiveQuantity"].ToString());
                }
            }
            //验证是否允许送
            if ((SumQuantity / 2 - SumGiveQuantity) >= 1)
            {
                //循环得到买一送一的SkuID
                foreach (DataRow dr in dtCart.Rows)
                {
                    int quantity = 0;
                    int GiveQuantity = 0;
                    //得到数量
                    int.TryParse(dr["Quantity"].ToString(), out quantity);
                    //得到赠送数量
                    int.TryParse(dr["GiveQuantity"].ToString(), out GiveQuantity);
                    double value = 0;
                    if (double.TryParse(dr["SalePrice"].ToString(), out value))
                    {
                        //累加已计算的
                        ReadQuantity += quantity;
                        ReadGiveQuantity += GiveQuantity;

                        if ((ReadQuantity / 2 - ReadGiveQuantity) >= 1)
                        {
                            //从第二高价并且还可以送的商品开始TopPrice > value
                            if (quantity > GiveQuantity)
                            {
                                strSkuId = dr["SkuId"].ToString();
                                double.TryParse(dr["SalePrice"].ToString(), out price);
                                break;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(strSkuId))
                {
                    //循环得到买一送一的SkuID
                    foreach (DataRow dr in dtCart.Rows)
                    {
                        int quantity = 0;
                        int GiveQuantity = 0;
                        //得到数量
                        int.TryParse(dr["Quantity"].ToString(), out quantity);
                        //得到赠送数量
                        int.TryParse(dr["GiveQuantity"].ToString(), out GiveQuantity);
                        //当前循环的商品是否还允许送一
                        if ((quantity / 2 - GiveQuantity) >= 1)
                        {
                            strSkuId = dr["SkuId"].ToString();
                            double.TryParse(dr["SalePrice"].ToString(), out price);
                            break;
                        }
                    }
                }
            }
            //原始取购物车方法
            //List<ShoppingCartInfo> cartlist = ShoppingCartProcessor.GetShoppingCartAviti();
            //foreach (ShoppingCartInfo cartinfo in cartlist)
            //{
            //    foreach (ShoppingCartItemInfo info in cartinfo.LineItems)
            //    {
            //        decimal value = info.AdjustedPrice ;
            //        if (value > price)
            //        {
            //            price = value;
            //        }
            //    }
            //}
            //根据SkuId设置赠送值
            if (!string.IsNullOrEmpty(strSkuId))
            {
                ShoppingCartProcessor.UpdateLineItemBuyToGive(strSkuId, pcUserid);
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                builder.AppendFormat("\"SkuId\":\"{0}\",", strSkuId);
                //builder.AppendFormat("\"TotalPrice\":\"{0}\"", ShoppingCartProcessor.GetShoppingCart().GetAmount());
                builder.AppendFormat("\"Price\":\"{0}\"", price);
                builder.Append("}");

            }
            else
            {
                builder.Append("{");
                builder.Append("\"Status\":\"NO\"");
                builder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }

        private void ProcessGetSkuByOptions(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
            bool isMemberPrice = Convert.ToBoolean(context.Request["IsMemberPrice"]);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                SKUItem item = isMemberPrice ? ShoppingProcessor.GetProductAndSku(currentMember, productId, str) : ShoppingProcessor.GetProductAndSku(currentMember, productId, str, false);
                if (item == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else
                {

                    System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", item.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", item.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", item.Weight);
                    builder.AppendFormat("\"Stock\":\"{0}\",", item.Stock);

                    //设置商品价格
                    //定义价格显示值对象
                    string strPrice = "¥" + item.SalePrice.ToString("F2");
                    //验证是否存在内购价格
                    if (item.NeigouPrice > 0)
                    {
                        //判断是否为内购门店
                        if (currentMember.DistributorUserId > 0)
                        {
                            DistributorsInfo info = VShopHelper.GetUserIdDistributors(currentMember.DistributorUserId);
                            if (info != null && info.UserId > 0 && info.IsNeiGouStore == 1)
                            {
                                //是内购门店则显示内购价格
                                strPrice = "¥" + item.NeigouPrice.ToString("F2");
                            }
                        }
                    }
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", strPrice);
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }
        public void ProcessRequest(System.Web.HttpContext context)
        {
            string text = context.Request["action"];
            switch (text)
            {
                case "AddToCartBySkus":
                    this.ProcessAddToCartBySkus(context);
                    break;
                case "GetSkuByOptions":
                    this.ProcessGetSkuByOptions(context);
                    break;
                case "DeleteCartProduct":
                    this.ProcessDeleteCartProduct(context);
                    break;
                case "ChageQuantity":
                    this.ProcessChageQuantity(context);
                    break;
                case "Submmitorder":
                    this.ProcessSubmmitorder(context);
                    break;
                case "Submmitordervirtual":
                    this.ProcessSubmmitorderVirtual(context);
                    break;
                case "SubmitMemberCard":
                    this.ProcessSubmitMemberCard(context);
                    break;
                case "AddShippingAddress":
                    this.AddShippingAddress(context);
                    break;
                case "AddReceipt":
                    this.AddReceipt(context);
                    break;
                case "AddReceiptDz":
                    this.AddReceiptDz(context);
                    break;
                case "EditReceipt":
                    this.EditReceipt(context);
                    break;
                case "EditReceiptDz":
                    this.EditReceiptDz(context);
                    break;
                case "DelShippingAddress":
                    this.DelShippingAddress(context);
                    break;
                case "DelReceipt":
                    this.DelReceipt(context);
                    break;
                case "SetDefaultShippingAddress":
                    this.SetDefaultShippingAddress(context);
                    break;
                case "UpdateShippingAddress":
                    this.UpdateShippingAddress(context);
                    break;
                case "GetPrize":
                    this.GetPrize(context);
                    break;
                case "Vote":
                    this.Vote(context);
                    break;
                case "SubmitActivity":
                    this.SubmitActivity(context);
                    break;
                case "AddSignUp":
                    this.AddSignUp(context);
                    break;
                case "AddTicket":
                    this.AddTicket(context);
                    break;
                case "FinishOrder":
                    this.FinishOrder(context);
                    break;
                case "ConfirmTakeGoodsOrder":
                    this.ConfirmTakeGoodsOrder(context);
                    break;
                case "AddUserPrize":
                    this.AddUserPrize(context);
                    break;
                case "SubmitWinnerInfo":
                    this.SubmitWinnerInfo(context);
                    break;
                case "SetUserName":
                    this.SetUserName(context);
                    break;
                case "AddProductConsultations":
                    this.AddProductConsultations(context);
                    break;
                case "AddProductReview":
                    this.AddProductReview(context);
                    break;
                case "AddFavorite":
                    this.AddFavorite(context);
                    break;
                case "DelFavorite":
                    this.DelFavorite(context);
                    break;
                case "CheckFavorite":
                    this.CheckFavorite(context);
                    break;
                case "GetStock":
                    this.GetStock(context);
                    break;
                case "Logistic":
                    this.SearchExpressData(context);
                    break;
                case "GetShippingTypes":
                    this.GetShippingTypes(context);
                    break;
                case "UserLogin":
                    this.UserLogin(context);
                    break;
                case "RegisterUser":
                    this.RegisterUser(context);
                    break;
                case "AddDistributor":
                    this.AddDistributor(context);
                    break;
                case "SetDistributorMsg":
                    this.SetDistributorMsg(context);
                    break;
                case "DeleteProducts":
                    this.DeleteDistributorProducts(context);
                    break;
                case "AddDistributorProducts":
                    this.AddDistributorProducts(context);
                    break;
                case "UpdateDistributor":
                    this.UpdateDistributor(context);
                    break;
                case "AddCommissions":
                    this.AddCommissions(context);
                    break;
                case "RequestReturn":
                    this.RequestReturn(context);
                    break;
                case "EditPassword":
                    this.EditPassword(context);
                    break;
                case "GetOrderRedPager":
                    this.GetOrderRedPager(context);
                    break;
                case "AdjustCommissions":
                    this.AdjustCommssions(context);
                    break;
                case "SendOrderGoods":
                    this.SendOrderGoods(context);
                    break;
                case "IsOrderStateChange":
                    this.IsOrderStateChange(context);
                    break;
                case "IsShippingModeOnly":
                    this.IsShippingModeOnly(context);
                    break;
                case "IsPaymentModeOnly":
                    this.IsPaymentModeOnly(context);
                    break;
                case "orderRemind":
                    this.OrderRemind(context);
                    break;
                case "NoYesOrderRemind":
                    this.IsOrderRemind(context);
                    break;
                case "CloseOrder":
                    this.CloseOrder(context);
                    break;
                case "getCoupon":
                    this.getCoupon(context);
                    break;
                case "cutDown":
                    this.cutDown(context);
                    break;
                case "reFreshCutDownInfo":
                    this.reFreshCutDownInfo(context);
                    break;
                case "GetDistributorByStreetId":
                    this.getDistributorByStreetId(context);
                    break;
                case "PrintOrderInfo":
                    this.PrintOrderInfo(context);
                    break;
                case "CheckCouponUseable":
                    this.CheckCouponUseable(context);
                    break;
                case "GetSKUSelector":
                    this.GetSKUSelector(context);
                    break;
                case "GetNumberInShoppingcart":
                    this.GetNumberInShoppingcart(context);
                    break;
                case "GetTotalNumAndPrice":
                    this.GetTotalNumAndPrice(context);
                    break;
                case "GoMicroPay":
                    this.GoMicroPay(context);
                    break;
                case "BuyToGive":
                    this.BuyToGive(context);//买一送一设置
                    break;
                case "ClearShoppingCart":
                    this.ClearShoppingCart(context);
                    break;
                case "SubmmitWKMHInfo":
                    this.SubmmitWKMHInfo(context);
                    break;
                case "getGuestListByHosterId":
                    this.getGuestList(context);
                    break;
                case "getGuestMatchRate":
                    this.getGuestMatchInfo(context);
                    break;
                case "GetMemberInfo":
                    this.getMemberInfo(context);
                    break;
                case "SubmitVote":
                    this.SubmitVote(context);
                    break;
                case "ValidateExistVote":
                    this.ValidateExistVote(context);
                    break;
                case "GetActivityNumCount":
                    this.GetActivityNumCount(context);
                    break;
                case "sendgk":
                    this.sendgk(context);
                    break;
                case "sendah":
                    this.sendah(context);
                    break;
                case "sendkk":
                    this.sendkk(context);
                    break;
                case "showordersource":
                    this.showordersource(context);
                    break;
                case "showordersourceservice":
                    this.showordersourceservice(context);
                    break;
                case "ProductConsultationsReplyed":
                    this.ProductConsultationsReplyed(context);
                    break;
                case "requestFp":
                    this.requestFp(context);
                    break;
                case "ValidateBuyNum":
                    this.ValidateBuyNum(context);
                    break;
                case "donwloadFp":
                    this.donwloadFp(context);
                    break;
                case "wxrefund":
                    this.wxrefund(context);
                    break;
                case "validatePoints":
                    this.validatePoints(context);
                    break;
                case "SetPrizeRecord":
                    this.SetPrizeRecord(context);
                    break;
                case "BindStoreLocation":
                    //2017-06-30 创维二期修改
                    this.BindStoreLocation(context);
                    break;
                case "StoreData":
                    //门店列表
                    this.StoreData(context);
                    break;
                case "BindUserStore":
                    //用户绑定门店
                    this.BindUserStore(context);
                    break;
            }
        }

        private void BindUserStore(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            int storeid = 0;
            string strStoreId = context.Request["storeid"].ToString();//门店Id
            if (int.TryParse(strStoreId, out storeid) && storeid > 0)
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                currentMember.DistributorUserId = storeid;
                MemberProcessor.SetUserDistributorUserId(currentMember.UserId, currentMember.DistributorUserId);
                //将所属门店设置为自己
                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                cookie.Value = storeid.ToString();
                cookie.Expires = DateTime.Now.AddYears(10);
                context.Response.Cookies.Add(cookie);

                builder.Clear();
                builder.Append("{\"Result\":\"OK\"}");
            }
            context.Response.Write(builder.ToString());
        }



        /// <summary>
        /// 根据经纬度获取门店信息
        /// </summary>
        /// <param name="context"></param>
        private void StoreData(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            double lng = 0;
            double lat = 0;
            int pagenum = 1;
            int pagesize = 0;
            string strlng = context.Request["lng"].ToString();//经度
            string strlat = context.Request["lat"].ToString();//纬度
            string keyword = context.Request["keyword"].ToString();//查询关键字
            string strpagenum = context.Request["pagenum"].ToString();//分页页码
            string strpagesize = context.Request["pagesize"].ToString();//每页数量
            if (double.TryParse(strlng, out lng) && double.TryParse(strlat, out lat) && int.TryParse(strpagenum, out pagenum) && int.TryParse(strpagesize, out pagesize))
            {
                DistributorListQuery query = new DistributorListQuery();
                query.lng = lng;
                query.lat = lat;
                if (lng > 0 && lat > 0)
                {
                    query.NearByValue = 5;
                    query.SortBy = "distance";//距离
                    query.SortOrder = SortAction.Asc;
                }
                else
                {
                    query.SortBy = "StarTotal";//星级分数
                    query.SortOrder = SortAction.Desc;
                }
                query.keyword = keyword;
                query.PageSize = pagesize;
                query.PageIndex = pagenum;

                DbQueryResult distributor = DistributorsBrower.SelectNearByPosition(query);
                int count = distributor.TotalRecords;
                //获取用户充值列表及页码
                int pageCount = count / pagesize + (count % pagesize == 0 ? 0 : 1);
                int nextPage = (pagenum < pageCount) ? (pagenum + 1) : 0; //下一页为0时，表示无数据可加载（数据加载完毕）

                builder.Clear();
                builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"PageCount\":{2},\"NextPage\":{3},\"Data\":", "OK", count, pageCount, nextPage));
                builder.Append(JsonConvert.SerializeObject(distributor.Data, Newtonsoft.Json.Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
                builder.Append("}");
            }
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 验证商品兑换的积分
        /// </summary>
        /// <param name="context"></param>
        private void validatePoints(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Status\":\"ERR\",\"Msg\":\"参数错误。\"}");
            int giftId = 0;
            int buyNum = 0;
            if (context.Request["giftId"] != null && context.Request["buyNum"] != null)
            {
                if (int.TryParse(context.Request["giftId"].ToString(), out giftId) && int.TryParse(context.Request["buyNum"].ToString(), out buyNum))
                {
                    if (giftId > 0 && buyNum > 0)
                    {
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        if (currentMember != null && currentMember.Points > 0)
                        {
                            GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
                            if (giftDetails != null && giftDetails.NeedPoint > 0)
                            {
                                int neadPoint = giftDetails.NeedPoint * buyNum;
                                if (currentMember.Points >= neadPoint)
                                {
                                    builder.Clear();
                                    builder.Append("{\"Status\":\"YES\",\"Msg\":\"允许执行。\"}");
                                }
                                else
                                {
                                    builder.Clear();
                                    builder.Append("{\"Status\":\"ERR\",\"Msg\":\"您的积分不够。\"}");
                                }
                            }
                        }
                        else
                        {
                            builder.Clear();
                            builder.Append("{\"Status\":\"ERR\",\"Msg\":\"您的积分不够。\"}");
                        }
                    }
                }
            }
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 微信退款
        /// </summary>
        /// <param name="context"></param>
        private void wxrefund(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            string refundMoney = context.Request["refundMoney"].ToString();
            decimal money = 0;

            if (!string.IsNullOrEmpty(orderId) && decimal.TryParse(refundMoney, out money))
            {
                OrderInfo orderinfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderinfo != null && !string.IsNullOrEmpty(orderinfo.GatewayOrderId))
                {
                    WxPayData data = new WxPayData();
                    data.SetValue("transaction_id", orderinfo.GatewayOrderId);
                    //data.SetValue("out_trade_no", orderinfo.OrderId);//存在一个就行
                    data.SetValue("total_fee", int.Parse((orderinfo.GetTotal() * 100).ToString()));//订单总金额
                    data.SetValue("refund_fee", int.Parse((money * 100).ToString()));//退款金额
                    data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
                    data.SetValue("op_user_id", WxPayConfig.MCHID);//操作员，默认为商户号

                    WxPayData result = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据
                    if (result.IsSet("result_code"))
                    {
                        if (result.GetValue("result_code").ToString().Equals("SUCCESS"))
                        {
                            //成功
                            builder.Append("{\"success\":\"YES\",\"Msg\":\"退款成功！\"}");
                        }
                        else
                            builder.Append("{\"success\":\"NO\",\"Msg\":\"" + (result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "") + "\"}");
                    }
                    else
                        builder.Append("{\"success\":\"NO\",\"Msg\":\"退款接口返回的报文错误。\"}");
                }
                else
                    builder.Append("{\"success\":\"NO\",\"Msg\":\"订单不存在交易流水。\"}");
            }
            else
                builder.Append("{\"success\":\"NO\",\"Msg\":\"参数错误。\"}");
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 验证购买商品是否超过限购数量
        /// </summary>
        /// <param name="context"></param>
        private void ValidateBuyNum(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int productId = 0;
            int quantity = 0;
            if (context.Request["from"] != null && context.Request["from"].ToString() == "signBuy")
            {
                if (context.Request["productId"] != null && context.Request["quantity"] != null && context.Request["productSkuId"] != null
                    && int.TryParse(context.Request["productId"].ToString(), out productId) && int.TryParse(context.Request["quantity"].ToString(), out quantity))
                {
                    string productSkuId = context.Request["productSkuId"].ToString();
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    ProductInfo productinfo = ProductBrowser.GetProduct(currentMember, productId);
                    if (productinfo != null && productinfo.ProductId > 0)
                    {
                        builder.Append("{\"Status\":\"YES\",\"Msg\":\"允许执行。\",\"ProSource\":\"" + productinfo.ProductSource + "\"}");
                        if (currentMember.DistributorUserId > 0)
                        {
                            DistributorsInfo disinfo = VShopHelper.GetUserIdDistributors(currentMember.DistributorUserId);
                            if (disinfo != null && disinfo.UserId > 0 && disinfo.IsNeiGouStore == 1)
                            {
                                //是内购门店
                                if (productinfo.RestrictNeigouNum > 0)
                                {
                                    DataTable dt = OrderHelper.GetOrderItemByUserIdAndProId(currentMember.UserId, productId);
                                    int icount = dt.Rows.Count + quantity;
                                    if (productinfo.RestrictNeigouNum < icount)
                                    {
                                        builder.Clear();
                                        builder.Append("{\"Status\":\"NO\",\"Num\":\"" + productinfo.RestrictNeigouNum.ToString() + "\",\"ProSource\":\"" + productinfo.ProductSource + "\"}");
                                        //strResult = "\"Status\":\"NO\",\"Num\":\"" + productinfo.RestrictNeigouNum.ToString() + "\"";
                                    }
                                }
                            }
                        }
                    }
                    else
                        builder.Append("{\"Status\":\"ERR\",\"Msg\":\"参数错误。\"}");
                }
                else
                    builder.Append("{\"Status\":\"ERR\",\"Msg\":\"参数错误。\"}");
            }
            else
                builder.Append("{\"Status\":\"ERR\",\"Msg\":\"参数错误。\"}");

            context.Response.Write(builder.ToString());

            //context.Response.ContentType = "application/json";
            //System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //builder.Append("{\"Status\":\"YES\",\"Msg\":\"允许执行。\"}");
            //int productId = 0;
            //int quantity = 0;
            //if (context.Request["from"] != null && context.Request["from"].ToString() == "signBuy")
            //{
            //    if (context.Request["productId"] != null && context.Request["quantity"] != null && context.Request["productSkuId"] != null
            //        && int.TryParse(context.Request["productId"].ToString(), out productId) && int.TryParse(context.Request["quantity"].ToString(), out quantity))
            //    {
            //        string productSkuId = context.Request["productSkuId"].ToString();
            //        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            //        if (currentMember.DistributorUserId > 0)
            //        {
            //            DistributorsInfo disinfo = VShopHelper.GetUserIdDistributors(currentMember.DistributorUserId);
            //            if (disinfo != null && disinfo.UserId > 0 && disinfo.IsNeiGouStore == 1)
            //            {
            //                //是内购门店
            //                ProductInfo productinfo = ProductBrowser.GetProduct(currentMember, productId);
            //                if (productinfo.RestrictNeigouNum > 0)
            //                {
            //                    DataTable dt = OrderHelper.GetOrderItemByUserIdAndProId(currentMember.UserId, productId);
            //                    int icount = dt.Rows.Count + quantity;
            //                    if (productinfo.RestrictNeigouNum < icount)
            //                    {
            //                        builder.Clear();
            //                        builder.Append("{\"Status\":\"NO\",\"Num\":\"" + productinfo.RestrictNeigouNum.ToString() + "\"}");
            //                        //builder.AppendFormat("{\"Status\":\"NO\",\"Num\":\"{0}\"}", productinfo.RestrictNeigouNum.ToString());
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        builder.Clear();
            //        builder.Append("{\"Status\":\"ERR\",\"Msg\":\"参数错误。\"}");
            //    }
            //}
            //context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 重新发送到港口
        /// </summary>
        /// <param name="context"></param>
        private void sendgk(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                OrderInfo orderinfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (!string.IsNullOrEmpty(orderinfo.GatewayOrderId))
                {
                    #region 开始同步港口系统
                    string store = string.Empty;
                    if (orderinfo.ReferralUserId > 0)
                    {
                        StoreInfo storeinfo = StoreInfoHelper.GetStoreInfoByUserId(orderinfo.ReferralUserId);
                        if (storeinfo != null && !string.IsNullOrEmpty(storeinfo.accountALLHere))
                            store = storeinfo.accountALLHere;
                    }
                    else
                    {
                        //如果是主店购买，不存在accountALLHere值，则取配置文件中的主店默认AllHere账号值
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                        if (!string.IsNullOrEmpty(masterSettings.DefaultALLHereCode))
                        {
                            store = masterSettings.DefaultALLHereCode;
                        }
                    }
                    string strmessage = string.Empty;
                    if (!string.IsNullOrEmpty(store))
                    {
                        int iresult = CwAPI.SendOrderToGankKou(orderinfo, store, out strmessage);
                        if (iresult == 0)
                        {
                            orderinfo.submitgk = 1;
                            //修改同步状态
                            OrderHelper.UpdateSubmitgk(orderinfo.OrderId, orderinfo.submitgk);
                            builder.Append("{\"success\":\"true\",\"Msg\":\"推送成功！\"}");
                        }
                        else
                        {
                            builder.Append("{\"success\":\"false\",\"Msg\":\"" + strmessage + "\"}");
                        }
                    }
                    else
                        builder.Append("{\"success\":\"false\",\"Msg\":\"订单用户所属门店对应的基础门店数据不存在。\"}");
                    #endregion
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"订单不存在交易流水。\"}");
            }
            else
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");

            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 重新发送到AH
        /// </summary>
        /// <param name="context"></param>
        private void sendah(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            if (string.IsNullOrEmpty(orderId))
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号错误，无法查询订单信息。\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.GatewayOrderId))//上线时在开启判断
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单不存在交易流水。\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            string stordekeyid = string.Empty;
            if (orderInfo.ReferralUserId == 0)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.DefaultALLHereCode))
                {
                    stordekeyid = masterSettings.DefaultALLHereCode;
                }
            }
            else
            {
                stordekeyid = VShopHelper.GetOrderStoreKeyId(orderInfo.ReferralUserId.ToString());
            }
            if (string.IsNullOrEmpty(stordekeyid))
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单所属门店KeyID不存在，无法与AH对接订单信息。\"}");
                context.Response.Write(builder.ToString());
                return;
            }

            #region 开始同步AH系统
            //开始调用AH订单接口
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderInfo.ShipTo);
            strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderInfo.CellPhone);
            strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderInfo.CellPhone);

            strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 2));
            strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 4));
            strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderInfo.RegionId.ToString());
            ///地址省/市/区中文名称
            string[] strname;
            if (!string.IsNullOrEmpty(orderInfo.ShippingRegion))
            {
                CwAHAPI.CwapiLog("城市：" + orderInfo.ShippingRegion);
                strname = orderInfo.ShippingRegion.Split('，');
                if (strname.Length == 3)
                {
                    strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strname[0]);//吉林省 松原市 乾安县
                    strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strname[1]);
                    strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strname[2]);

                    CwAHAPI.CwapiLog("省份为：" + strname[0]);
                    CwAHAPI.CwapiLog("城市为：" + strname[1]);
                    CwAHAPI.CwapiLog("区为：" + strname[2]);
                }
                else
                {
                    strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//吉林省 松原市 乾安县
                    strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", "");
                    strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", "");
                }
            }
            strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));
            //2017-04-26,与金力沟通后统一增加字段发票类型
            strJson.AppendFormat("\"TaxType\":\"{0}\",", orderInfo.receiptId > 0 ? "1" : "2");//0为普通发票，1为增值税发票，2为电子发票
            strJson.AppendFormat("\"TaxName\":\"{0}\",", string.IsNullOrEmpty(orderInfo.TelPhone) ? orderInfo.ShipTo : orderInfo.TelPhone);
            strJson.AppendFormat("\"TaxPhone\":\"{0}\",", orderInfo.CellPhone);
            strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);
            strJson.AppendFormat("\"OrderTime\":\"{0}\",", orderInfo.OrderDate);
            strJson.AppendFormat("\"FValue\":\"{0}\",", orderInfo.GetAmount().ToString("0.00"));//商品总价格
            strJson.AppendFormat("\"ReValue\":\"{0}\",", orderInfo.GetTotal().ToString("0.00"));//订单实际支付价格
            strJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Remark));
            strJson.AppendFormat("\"SerialNum\":\"{0}\",", orderInfo.GatewayOrderId);//serialNum 交易流水号"45888899800000001"
            //strJson.AppendFormat("\"SerialNum\":\"{0}\",", 45888899800000001);
            //根据orderInfo.ReferralUserId 的值 在 aspnet_Distributors 中查询 —— StoreId —— 然后在CW_StoreInfo中查询DZ号
            strJson.AppendFormat("\"BMBM\":\"{0}\",", stordekeyid);
            //根据orderInfo.SupplierId查询供应商的Code值在传入
            SupplierInfo supplierinfo = SupplierHelper.GetSupplier(orderInfo.SupplierId);
            if (supplierinfo != null && !string.IsNullOrEmpty(supplierinfo.gysCode))
                strJson.AppendFormat("\"WLDW\":\"{0}\",", supplierinfo.gysCode);
            else
                strJson.AppendFormat("\"WLDW\":\"{0}\",", "");
            strJson.Append("\"Details\":[");
            foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
            {
                if (!string.IsNullOrEmpty(iteminfo.ProductCode) && iteminfo.ProductCode != "0")
                {
                    strJson.Append("{");
                    strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//商品内码
                    strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                    strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品实际购买单价
                    strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//商品结算价
                    strJson.Append("}");
                }
                else
                {
                    builder.Append("{\"success\":\"false\",\"Msg\":\"商品内码不能为空\"}");
                    context.Response.Write(builder.ToString());
                    break;
                }
            }
            strJson.Append("]");
            strJson.Append("}");
            CwAHAPI.CwapiLog("发送数据：" + strJson);
            AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
            try
            {
                string strResult = ahservice.MPFTOJL_DHD_CJ(strJson.ToString());
                CwAHAPI.CwapiLog("返回内容：" + strResult);
                string message = "";
                string orderid = "";
                if (!string.IsNullOrEmpty(strResult))
                {
                    if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                    {
                        int iIndex = message.IndexOf(":");
                        if (iIndex > 0)
                            message = message.Substring(0, (iIndex + 1));
                        builder.Append("{\"success\":\"false\",\"Msg\":\"" + message + "\"}");
                    }
                    else
                    {
                        if (orderid == orderInfo.OrderId)
                        {
                            //更新金力同步状态
                            orderInfo.submitgk = 1;
                            //修改同步状态
                            OrderHelper.UpdateSubmitgk(orderInfo.OrderId, orderInfo.submitgk);
                            builder.Append("{\"success\":\"true\",\"Msg\":\"推送成功！\"}");
                        }
                    }
                }
            }
            catch
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"操作失败，原因：接口调用无响应。\"}");
            }
            #endregion

            context.Response.Write(builder.ToString());
        }


        /// <summary>
        /// 重新发送到酷开
        /// </summary>
        /// <param name="context"></param>
        private void sendkk(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            if (string.IsNullOrEmpty(orderId))
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号错误，无法查询订单信息。\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.GatewayOrderId))//上线时在开启判断
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单不存在交易流水。\"}");
                context.Response.Write(builder.ToString());
                return;
            }

            #region 开始同步酷开系统
            //开始调用AH订单接口
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderInfo.ShipTo);
            strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderInfo.CellPhone);
            strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderInfo.CellPhone);

            strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 2));
            strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderInfo.RegionId.ToString().Substring(0, 4));
            strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderInfo.RegionId.ToString());
            ///地址省/市/区中文名称
            string[] strname;
            if (!string.IsNullOrEmpty(orderInfo.ShippingRegion))
            {
                CwAHAPI.KkapiLog("城市：" + orderInfo.ShippingRegion);
                strname = orderInfo.ShippingRegion.Split('，');
                if (strname.Length == 3)
                {
                    strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", strname[0]);//吉林省 松原市 乾安县
                    strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", strname[1]);
                    strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", strname[2]);

                    CwAHAPI.KkapiLog("省份为：" + strname[0]);
                    CwAHAPI.KkapiLog("城市为：" + strname[1]);
                    CwAHAPI.KkapiLog("区为：" + strname[2]);
                }
                else
                {
                    strJson.AppendFormat("\"ReceivProviceName\":\"{0}\",", "");//吉林省 松原市 乾安县
                    strJson.AppendFormat("\"ReceivCityName\":\"{0}\",", "");
                    strJson.AppendFormat("\"ReceivAreaName\":\"{0}\",", "");
                }
            }
            strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));
            strJson.AppendFormat("\"TaxName\":\"{0}\",", string.IsNullOrEmpty(orderInfo.TelPhone) ? orderInfo.ShipTo : orderInfo.TelPhone);
            strJson.AppendFormat("\"TaxPhone\":\"{0}\",", orderInfo.CellPhone);
            strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Address));
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);
            strJson.AppendFormat("\"OrderTime\":\"{0}\",", orderInfo.OrderDate);
            strJson.AppendFormat("\"FValue\":\"{0}\",", orderInfo.GetAmount().ToString("0.00"));//商品总价格
            strJson.AppendFormat("\"ReValue\":\"{0}\",", orderInfo.GetTotal().ToString("0.00"));//订单实际支付价格
            strJson.AppendFormat("\"OrderNote\":\"{0}\",", CwAHAPI.cleanString(orderInfo.Remark));
            strJson.AppendFormat("\"SerialNum\":\"{0}\",", orderInfo.GatewayOrderId);//serialNum 交易流水号"45888899800000001"
            //strJson.AppendFormat("\"SerialNum\":\"{0}\",", 45888899800000001);
            strJson.Append("\"Details\":[");
            foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
            {
                if (!string.IsNullOrEmpty(iteminfo.KukaiCode) && iteminfo.KukaiCode != "0")
                {
                    strJson.Append("{");
                    strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.KukaiCode);//酷开商品内码
                    strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                    strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品实际购买单价
                    strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemCostPrice.ToString("0.00"));//商品结算价
                    strJson.Append("}");
                }
                else
                {
                    builder.Append("{\"success\":\"false\",\"Msg\":\"酷开商品内码不能为空\"}");
                    context.Response.Write(builder.ToString());
                    break;
                }
            }
            strJson.Append("]");
            strJson.Append("}");
            CwAHAPI.KkapiLog("发送数据：" + strJson);

            try
            {
                //实例化接口对象
                KuKaiServiceReference.CatchStreetOrderWebInfoClient kukaiwebinfo = new KuKaiServiceReference.CatchStreetOrderWebInfoClient();
                //实例化传递参数对象
                KuKaiServiceReference.sendStreetOrderToNC sendModel = new KuKaiServiceReference.sendStreetOrderToNC();
                //将传递值设置vo属性上
                sendModel.vo = strJson.ToString();
                //实例化回掉参数对象， 并且调用接口得到回调对象
                KuKaiServiceReference.sendStreetOrderToNCResponse resultModel = kukaiwebinfo.sendStreetOrderToNC(sendModel);
                if (resultModel != null)
                {
                    string strResult = resultModel.returnArg;
                    CwAHAPI.KkapiLog("返回内容：" + strResult);
                    string message = "";
                    string orderid = "";
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        if (CwAHAPI.ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                        {
                            if (message != null)
                            {
                                int iIndex = message.IndexOf(":");
                                if (iIndex > 0)
                                    message = message.Substring(0, (iIndex + 1));
                            }
                            builder.Append("{\"success\":\"false\",\"Msg\":\"" + message + "\"}");
                        }
                        else
                        {
                            if (orderid == orderInfo.OrderId)
                            {
                                //更新酷开同步状态
                                orderInfo.submitkk = 1;
                                //修改同步状态
                                OrderHelper.UpdateSubmitkk(orderInfo.OrderId, orderInfo.submitkk);
                                builder.Append("{\"success\":\"true\",\"Msg\":\"推送成功！\"}");
                            }
                        }
                    }
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"操作失败，原因：酷开接口调用返回对象不存在值。\"}");
            }
            catch
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"操作失败，原因：酷开接口调用无响应。\"}");
            }
            #endregion

            context.Response.Write(builder.ToString());
        }

        //订单服务门店
        private void showordersourceservice(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                DataTable dt = StoreInfoHelper.GetOrderSourceServiceByOrderId(orderId);
                if (dt.Rows.Count > 0)
                {
                    builder.Append("{\"success\":\"true\"");
                    builder.AppendFormat(",\"FgsName\":\"{0}\"", dt.Rows[0]["fgsName"].ToString());
                    builder.AppendFormat(",\"FgsPhone\":\"{0}\"", dt.Rows[0]["fgsPhone"].ToString());
                    builder.AppendFormat(",\"StoreName\":\"{0}\"", dt.Rows[0]["storeName"].ToString());
                    builder.AppendFormat(",\"StoreRelationPerson\":\"{0}\"", dt.Rows[0]["storeRelationPerson"].ToString());
                    builder.AppendFormat(",\"StoreRelationCell\":\"{0}\"", dt.Rows[0]["storeRelationCell"].ToString());
                    builder.AppendFormat(",\"AccountALLHere\":\"{0}\"", dt.Rows[0]["accountALLHere"].ToString());
                    builder.Append("}");
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"订单来源不存在。\"}");
            }
            else
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");

            context.Response.Write(builder.ToString());
        }

        private void showordersource(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                DataTable dt = StoreInfoHelper.GetOrderSourceByOrderId(orderId);
                if (dt.Rows.Count > 0)
                {
                    builder.Append("{\"success\":\"true\"");
                    builder.AppendFormat(",\"FgsName\":\"{0}\"", dt.Rows[0]["fgsName"].ToString());
                    builder.AppendFormat(",\"FgsPhone\":\"{0}\"", dt.Rows[0]["fgsPhone"].ToString());
                    builder.AppendFormat(",\"StoreName\":\"{0}\"", dt.Rows[0]["storeName"].ToString());
                    builder.AppendFormat(",\"StoreRelationPerson\":\"{0}\"", dt.Rows[0]["storeRelationPerson"].ToString());
                    builder.AppendFormat(",\"StoreRelationCell\":\"{0}\"", dt.Rows[0]["storeRelationCell"].ToString());
                    builder.AppendFormat(",\"AccountALLHere\":\"{0}\"", dt.Rows[0]["accountALLHere"].ToString());
                    builder.Append("}");
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"订单来源不存在。\"}");
            }
            else
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");

            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 获取发票信息
        /// </summary>
        /// <param name="context"></param>
        private void donwloadFp(System.Web.HttpContext context)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderid"].ToString();
            //orderId = "201612166650346";//测试才打开
            if (!string.IsNullOrEmpty(orderId))
            {
                string strPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/ReceiptPdf/{0}.PDF", orderId));
                if (File.Exists(strPath))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(strPath);
                    long fileSize = file.Length;
                    HttpContext.Current.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
                    context.Response.AddHeader("Content-Length", fileSize.ToString());
                    context.Response.TransmitFile(strPath, 0, fileSize);
                    context.Response.Flush();
                    context.Response.End();

                }
            }
        }


        /// <summary>
        /// 获取发票信息
        /// </summary>
        /// <param name="context"></param>
        private void requestFp(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string orderId = context.Request["orderid"].ToString();
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (!string.IsNullOrEmpty(orderId) && orderInfo != null)
            {
                if (orderInfo.OrderStatus == OrderStatus.Finished)
                {
                    //得到orderid开始下载到服务器
                    //orderId = "201611114268025";//测试使用//orderId = "201612166650346";
                    StringBuilder strJson = new StringBuilder();
                    strJson.Append("{");
                    strJson.AppendFormat("\"OrderNo\":\"{0}\"", orderId);//订单号
                    strJson.Append("}");
                    CwAHAPI.CwapiLog("发送数据：" + strJson);
                    AllHereServiceReference.MPFTOJLClient ahservice = new AllHereServiceReference.MPFTOJLClient();
                    try
                    {
                        string strResult = ahservice.MPFTOJL_FP(strJson.ToString());
                        CwAHAPI.CwapiLog("返回内容：" + strResult);
                        string strOutOrderid = string.Empty;
                        string strPath = CwAHAPI.ResolutioSendReceiptAHResultString(strResult, out strOutOrderid);
                        if (strOutOrderid == orderId)
                        {
                            if (!string.IsNullOrEmpty(strPath))
                            {
                                //builder.Append("{\"success\":\"true\",\"Msg\":\"" + string.Format(@"{0}", strPath.Replace("\\", "\\\\")) + "\"}");
                                builder.Append("{\"success\":\"true\",\"Msg\":\"" + string.Format(@"{0}", strPath) + "\"}");
                            }
                            else
                                builder.Append("{\"success\":\"false\",\"Msg\":\"发票信息需要人工处理，还未生成发票信息，请耐心等待。\"}");
                        }
                        else
                        {
                            builder.Append("{\"success\":\"false\",\"Msg\":\"发票接口返回的订单编号与发送的不一致。\"}");
                        }
                    }
                    catch
                    {
                        builder.Append("{\"success\":\"false\",\"Msg\":\"操作失败，原因：接口调用无响应。\"}");
                    }
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"订单不是完结状态无法调用获取发票。\"}");
            }
            else
                builder.Append("{\"success\":\"false\",\"Msg\":\"订单编号不存在。\"}");

            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 获取用户姓名、电话
        /// </summary>
        /// <param name="context"></param>
        private void getMemberInfo(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            MemberInfo currMemberInfo = MemberProcessor.GetCurrentMember();
            if (currMemberInfo.RealName != null && currMemberInfo.CellPhone != null && currMemberInfo.Address != null &&
                !string.IsNullOrEmpty(currMemberInfo.RealName.Trim()) && !string.IsNullOrEmpty(currMemberInfo.CellPhone.Trim()) && !string.IsNullOrEmpty(currMemberInfo.Address.Trim()))
            {
                builder.Append("{\"IExist\":\"1\",\"RealName\":\"" + currMemberInfo.RealName.Trim() + "\",\"CellPhone\": \"" + currMemberInfo.CellPhone.Trim() + "\",\"Address\": \"" + currMemberInfo.Address.Trim() + "\"}");
            }
            else
            {
                builder.Append("{\"IExist\":\"0\",\"RealName\":\"1\",\"CellPhone\": \"1\",\"Address\": \"1\"}");//返回空值
            }
            context.Response.Write(builder.ToString());
        }

        private void GetActivityNumCount(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int activityid = 0;
            int.TryParse(context.Request["activityid"].ToString(), out activityid);
            int currNumCount = 0;
            int.TryParse(context.Request["currNumCount"].ToString(), out currNumCount);
            if (activityid > 0 && currNumCount > 0)
            {
                currNumCount = VshopBrowser.GetNumCount(activityid, currNumCount);
            }
            builder.Append("{\"success\":\"true\",\"Msg\":\"" + currNumCount + "\"}");
            context.Response.Write(builder.ToString());
        }

        //验证是否参与过投票
        private void ValidateExistVote(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            long voteid;
            long.TryParse(context.Request["VoteId"].ToString(), out voteid);
            try
            {
                //得到当前登录用户信息
                MemberInfo currMemberInfo = null;
                if (MemberProcessor.GetCurrentMember() == null)
                {
                    currMemberInfo = new MemberInfo();
                    string generateId = Globals.GetGenerateId();
                    currMemberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    currMemberInfo.UserName = "";
                    currMemberInfo.OpenId = "";
                    currMemberInfo.CreateDate = System.DateTime.Now;
                    currMemberInfo.SessionId = generateId;
                    currMemberInfo.SessionEndTime = System.DateTime.Now;
                    MemberProcessor.CreateMember(currMemberInfo);
                    currMemberInfo = MemberProcessor.GetMember(generateId);
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
                    System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                    {
                        Value = currMemberInfo.UserId.ToString(),
                        Expires = System.DateTime.Now.AddYears(10)
                    };
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    currMemberInfo = MemberProcessor.GetCurrentMember();
                }

                //验证是否在活动时间范围内
                VoteInfo voteById = StoreHelper.GetVoteById(voteid);
                if (voteById != null)
                {
                    DateTime date = DateTime.Now;
                    if (voteById.StartDate <= date)
                    {
                        if (date < voteById.EndDate)
                        {
                            //验证是否已经参与
                            DataSet dsResult = DataBaseHelper.GetDataSet(string.Format("select * from YiHui_Votes_Model_Result where VoteId = {0} and UserId = {1}", voteid, currMemberInfo.UserId));
                            if (dsResult.Tables[0].Rows.Count == 0)
                            {
                                builder.Append("{\"success\":\"true\",\"Msg\":\"未参与可以添加。\"}");//返回空值
                            }
                            else
                            {
                                builder.Append("{\"success\":\"false\",\"Msg\":\"您已经参与过本次活动。\"}");//返回空值
                            }
                        }
                        else
                            builder.Append("{\"success\":\"false\",\"Msg\":\"当前活动已经结束。\"}");//返回空值
                    }
                    else
                        builder.Append("{\"success\":\"false\",\"Msg\":\"当前活动未开启。\"}");//返回空值
                }
                else
                    builder.Append("{\"success\":\"false\",\"Msg\":\"当前活动不存在或已经被删除。\"}");//返回空值
            }
            catch
            {
                builder.Append("{\"success\":\"false\",\"Msg\":\"内部错误。\"}");//返回空值
            }
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 提交投票信息
        /// </summary>
        /// <param name="context"></param>
        private void SubmitVote(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string voteid = context.Request["VoteId"].ToString();
            string strData = context.Request["FormData"].ToString();
            if (!string.IsNullOrEmpty(strData))
            {
                try
                {
                    //得到当前登录用户信息
                    MemberInfo currMemberInfo = null;
                    if (MemberProcessor.GetCurrentMember() == null)
                    {
                        currMemberInfo = new MemberInfo();
                        string generateId = Globals.GetGenerateId();
                        currMemberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
                        currMemberInfo.UserName = "";
                        currMemberInfo.OpenId = "";
                        currMemberInfo.CreateDate = System.DateTime.Now;
                        currMemberInfo.SessionId = generateId;
                        currMemberInfo.SessionEndTime = System.DateTime.Now;
                        MemberProcessor.CreateMember(currMemberInfo);
                        currMemberInfo = MemberProcessor.GetMember(generateId);
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                        string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
                        System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                        {
                            Value = currMemberInfo.UserId.ToString(),
                            Expires = System.DateTime.Now.AddYears(10)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        currMemberInfo = MemberProcessor.GetCurrentMember();
                    }
                    //得到结果表结构
                    string selectSql = string.Format("Select * From YiHui_Votes_Model_Result Where 1=2");
                    DataSet dsData = DataBaseHelper.GetDataSet(selectSql);
                    //构建待提交的数据表
                    string[] strModels = strData.Split('●');
                    foreach (string strmodel in strModels)
                    {
                        if (!string.IsNullOrEmpty(strmodel))
                        {
                            string[] strValues = strmodel.Split('♦');
                            if (strValues.Length == 2)
                            {
                                DataRow resultDr = dsData.Tables[0].NewRow();
                                resultDr["PMRID"] = Guid.NewGuid();
                                resultDr["VoteId"] = voteid;
                                resultDr["PMID"] = new Guid(strValues[0]);
                                resultDr["UserId"] = currMemberInfo.UserId;
                                resultDr["Result"] = strValues[1];
                                dsData.Tables[0].Rows.Add(resultDr);
                            }
                        }
                    }
                    //整表提交
                    DataBaseHelper.CommitDataSet(dsData, selectSql.Split(';'));
                    builder.Append("{\"success\":\"true\",\"Msg\":\"操作成功\"}");//返回空值
                }
                catch
                {
                    builder.Append("{\"success\":\"false\",\"Msg\":\"内部错误\"}");//返回空值
                }
            }
            else
                builder.Append("{\"success\":\"false\",\"Msg\":\"参数错误\"}");//返回空值
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 获取对hoster的匹配度
        /// </summary>
        private void getGuestMatchInfo(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int hosterId = Convert.ToInt32(context.Request["hosterId"]);//获取hosterId
            int guestId = Convert.ToInt32(context.Request["guestId"]);//guestId
            Guid activityId = new Guid(context.Request["activityId"]);

            int matchRate = PromoteHelper.getMatchRate(activityId, hosterId, guestId);
            string matchDescription = PromoteHelper.GetMatchDescription(matchRate, activityId);
            string hosterName = MemberHelper.GetMember(hosterId).UserName;

            context.Response.Write("{\"success\":true,\"matchValue\":" + matchRate + ",\"matchDes\":\"" + matchDescription + "\",\"hosterName\":\"" + hosterName + "\"}");
        }

        /// <summary>
        /// 获取hoster的匹配度列表
        /// </summary>
        private void getGuestList(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int hosterId = Convert.ToInt32(context.Request["hosterId"]);//获取hosterId
            Guid activityId = new Guid(context.Request["activityId"]);
            string backJson = string.Empty;
            string detailList = string.Empty;
            //拼凑成html传回前端
            try
            {
                DataTable matchList = PromoteHelper.GetMatchInfoList(hosterId, activityId);
                if (matchList.Rows.Count == 0)
                {
                    detailList = "<h5>暂时没有人回答问题哦!</h5>";
                }
                else
                {
                    foreach (DataRow row in matchList.Rows)
                    {
                        string guestName = row["UserName"].ToString();
                        int matchRate = Convert.ToInt32(row["MatchRate"]);
                        string imgUserHead = "<img class='img40' src='" + row["UserHead"].ToString() + "'/>";
                        string matchRateDescription = PromoteHelper.GetMatchDescription(matchRate, activityId);//"赶紧约起来吧!";
                        detailList += string.Format("<tr><td>{0}</td><td><h2>{1}</h2><p>{2}</p></td><td>{3}%</td></tr>", imgUserHead, guestName, matchRateDescription, matchRate);
                    }
                }

                backJson += string.Format("\"success\":true,\"detailList\":\"{0}\"", detailList);
                context.Response.Write("{" + backJson + "}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":\"" + ex.Message + "\"}");
            }

        }

        /// <summary>
        /// 根据typeId设置hoster答题详情,或者是设置guest答题详情
        /// </summary>
        private void SubmmitWKMHInfo(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool flag = false;
            int matchValue = -1;//匹配度
            int typeId = Convert.ToInt32(context.Request["typeId"]);//保存类型
            int currentId = Convert.ToInt32(context.Request["memberId"]);//获取当前id
            int hosterId = 0;
            int.TryParse((context.Request["HosterId"]), out hosterId);//hosterId
            Guid activityId = new Guid(context.Request["activityId"]);
            IList<string> sbjIdList = context.Request["SBJIds"].Split(',');//获取题目id
            IList<string> optIdList = context.Request["OPTIds"].Split(',');//获取对应的答案id

            string matchDescription = string.Empty;
            string hosterName = string.Empty;
            try
            {


                switch (typeId)
                {
                    case 1://hoster出题
                        if (PromoteHelper.SetHosterDetail(currentId, sbjIdList, optIdList, activityId))//插入hoster信息至数据库
                            flag = true;
                        break;
                    case 4://guest答题
                        switch (PromoteHelper.SetGuestDetail(currentId, hosterId, optIdList, activityId))
                        {
                            case 1:
                                matchValue = PromoteHelper.getMatchInfo(activityId, hosterId, currentId);
                                matchDescription = PromoteHelper.GetMatchDescription(matchValue, activityId);
                                hosterName = MemberHelper.GetMember(hosterId).UserName;
                                flag = true;
                                break;
                            case -1:
                                flag = false;
                                break;
                            case 0:
                                flag = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            if (flag)
            {
                context.Response.Write("{\"success\":true,\"matchValue\":" + matchValue + ",\"matchDes\":\"" + matchDescription + "\",\"hosterName\":\"" + hosterName + "\"}");
            }

            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        public void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/wx_.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// 清除购物车
        /// </summary>
        private void ClearShoppingCart(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && ManagerHelper.GetCurrentManager() != null)
                ShoppingCartProcessor.ClearShoppingCartPC();
            else
                ShoppingCartProcessor.ClearShoppingCart();
            context.Response.Write("{\"success\":true}");
        }

        /// <summary>
        /// 刷卡支付
        /// </summary>
        private void GoMicroPay(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string payCode = context.Request["PayCode"];
            string orderId = context.Request["OrderId"];

            string des = "爽爽挝啡：";//描述
            OrderInfo info = OrderHelper.GetOrderInfo(orderId);

            string itemDescription = info.OrderId;
            if (info.LineItems.Count() > 0)
            {
                itemDescription = ((LineItemInfo)info.LineItems.First().Value).ItemDescription;
                if (itemDescription.Length > 30)
                    itemDescription = itemDescription.Substring(0, 30) + "...";
                else if (info.LineItems.Count() > 1)
                    itemDescription = itemDescription + "...";
            }
            //foreach (LineItemInfo itemInfo in info.LineItems.Values)
            //{
            //    des += ProductHelper.GetProductBaseInfo(itemInfo.ProductId).ProductName + ",";
            //}
            //des.TrimEnd(',');
            string tPrice = context.Request["tPrice"];//必须为整数,单位是[分]
            string result = string.Empty;
            try
            {
                result = MicroPay.Run(orderId, itemDescription, tPrice, payCode);
            }
            catch (Exception ex)
            { result = ex.Message; }
            finally
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("{");
                builder.Append("\"success\":true,");
                builder.AppendFormat("\"result\":\"{0}\"", result);
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }

        }

        /// <summary>
        /// 动态获取购物车中的总价和总数量
        /// </summary>
        private void GetTotalNumAndPrice(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            List<ShoppingCartInfo> cart = ShoppingCartProcessor.GetShoppingCartList();
            int totalNum = 0;
            decimal totalPrice = 0m;
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].LineItems.Count == 0)
                    continue;
                totalNum += cart[i].GetQuantity();
                totalPrice += cart[i].GetAmount();
            }
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            builder.Append("\"success\":true,");
            builder.AppendFormat("\"tNum\":\"{0}\",", totalNum);
            builder.AppendFormat("\"tPrice\":\"{0}\"", totalPrice.ToString("F2"));
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public List<MemberShoppingCartInfo> currMemberShoppingCartInfo;
        public static VshopProcess _instanceShoppingcart;
        public static VshopProcess InstanceShoppingcart
        {
            get
            {

                if (_instanceShoppingcart == null
                    || _instanceShoppingcart.currMemberShoppingCartInfo.Count(p => p.CurrentMember == MemberProcessor.GetCurrentMember()) == 0)
                {
                    if (_instanceShoppingcart == null)
                    {
                        _instanceShoppingcart = new VshopProcess();
                    }
                    _instanceShoppingcart.currMemberShoppingCartInfo.Add(
                        new MemberShoppingCartInfo(MemberProcessor.GetCurrentMember(), ShoppingCartProcessor.GetShoppingCartList()));

                }
                return _instanceShoppingcart;
            }
        }


        /// <summary>
        /// 通过skuid动态获取购物车中的已选数量
        /// </summary>
        private void GetNumberInShoppingcart(System.Web.HttpContext context)
        {
            /*
            List<ShoppingCartInfo> currMemberShoppingCartInfo = InstanceShoppingcart.currMemberShoppingCartInfo.Where(p => p.CurrentMember == MemberProcessor.GetCurrentMember()).First().Carts;
            string userSelectdSkuId="T01";
            if (currMemberShoppingCartInfo.Where(p => p.LineItems.Where(T => T.SkuId == userSelectdSkuId)).Count() == 0)
            {
                //DB->Add
            }
            else
            { 
                //Update
            }
            */
            context.Response.ContentType = "application/json";
            string skuId = context.Request["skuId"].ToString();
            /*
            if (currMemberShoppingCartInfo == null)
            {
                currMemberShoppingCartInfo = ShoppingCartProcessor.GetShoppingCartList();
            }
             */
            List<ShoppingCartInfo> cart = ShoppingCartProcessor.GetShoppingCartList();
            int num = 0;
            foreach (ShoppingCartInfo info in cart)
            {
                foreach (ShoppingCartItemInfo itemInfo in info.LineItems)
                {
                    if (skuId == itemInfo.SkuId)
                    {
                        num = itemInfo.Quantity;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            builder.Append("\"success\":true,");
            builder.AppendFormat("\"number\":\"{0}\"", num);
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }


        /// <summary>
        /// 动态获取规格选择器
        /// </summary>
        private void GetSKUSelector(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = Convert.ToInt32(context.Request["ProductId"]);
            DataTable skus = ProductBrowser.GetSkus(productId);
            StringBuilder builder = new StringBuilder();
            //builder.AppendFormat("<input type='hidden' id='hiddenSkuId' value='{0}_0'  />", productId);
            if ((skus != null) && (skus.Rows.Count > 0))
            {
                IList<string> list = new List<string>();
                //builder.AppendFormat("<input type='hidden' id='hiddenProductId' value='{0}' />", productId);
                builder.Append("<div class='specification'>");
                foreach (DataRow row in skus.Rows)
                {
                    if (!list.Contains((string)row["AttributeName"]))
                    {
                        list.Add((string)row["AttributeName"]);
                        builder.AppendFormat("<div class='title text-muted'>{0}：</div><input type='hidden' name='skuCountname' AttributeName='{0}' id='skuContent_{1}' />", row["AttributeName"], row["AttributeId"]);
                        builder.AppendFormat("<div class='list clearfix' id='skuRow_{0}'>", row["AttributeId"]);
                        IList<string> list2 = new List<string>();
                        foreach (DataRow row2 in skus.Rows)
                        {
                            if ((string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0) && !list2.Contains((string)row2["ValueStr"]))
                            {
                                string str = string.Concat(new object[] { "skuValueId_", row["AttributeId"], "_", row2["ValueId"] });
                                list2.Add((string)row2["ValueStr"]);
                                builder.AppendFormat("<div class='SKUValueClass' id='{0}' AttributeId='{1}' ValueId='{2}'>{3}</div>", new object[] { str, row["AttributeId"], row2["ValueId"], (row2["ImageUrl"].ToString() != "") ? ("<img src='" + row2["ImageUrl"] + "' width='50px' height='35px'></img>") : row2["ValueStr"] });
                            }
                        }
                        builder.Append("</div>");
                    }
                }
                builder.Append("</div>");
                string backAttrs = string.Format("\"success\":true,\"selector\":\"{0}\"", builder);
                context.Response.Write("{" + backAttrs + "}");
            }
            else
            {
                context.Response.Write("{\"success\":false\"}");
            }
            //writer.Write(builder.ToString());
        }

        /// <summary>
        /// 检查pc端点餐优惠券是否可用
        /// </summary>
        private void CheckCouponUseable(System.Web.HttpContext context)
        {
            string msg = "ok";
            decimal? reachPrice = 0m;
            decimal cutPrice = 0m;
            context.Response.ContentType = "application/json";
            string showCode = context.Request["showCode"];
            DataTable dtCoupon = CouponHelper.DeCodeShowCode(showCode);
            if (dtCoupon != null)//优惠券代码不存在
            {
                CouponInfo coupon = ShoppingProcessor.GetCoupon(dtCoupon.Rows[0]["ClaimCode"].ToString());
                if (coupon != null)//未使用判断
                {
                    //过期判断
                    if (coupon.ClosingTime < DateTime.Now)
                    {
                        msg = "该优惠券已过期!";
                    }
                    else//通过验证,传回数据
                    {
                        reachPrice = coupon.Amount;
                        cutPrice = coupon.DiscountValue;
                    }
                }
                else
                {
                    msg = "该优惠券已使用!";
                }
            }
            else
            {
                msg = "优惠券不存在!";
            }
            string backAttrs = string.Format(",\"msg\":\"{0}\",\"reachPrice\":\"{1}\",\"cutPrice\":\"{2}\"", msg, reachPrice, cutPrice);
            context.Response.Write("{\"success\":true" + backAttrs + "}");
        }
        /// <summary>
        /// 整理购物车中商品的规格内容,保持与前端一致
        /// </summary>
        /// <param name="skuContent">商品的原规格字符串</param>
        /// <returns>整理后的字符串</returns>
        private string skuContentFormat(string skuContent)
        {
            skuContent = skuContent.Trim().TrimEnd(';');
            IList<string> skus = skuContent.Split(';');
            if (skus.Count <= 0 || skus[0] == "") return "默认";
            string result = string.Empty;
            for (int i = skus.Count - 1; i >= 0; i--)
            {
                skus[i] = skus[i].Substring(skus[i].IndexOf("：") + 1);
                result += skus[i] + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }
        /// <summary>
        /// 打印pc端点餐小票
        /// </summary>
        private void PrintOrderInfo(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                IList<string> orderIds = context.Request["OrderId"].ToString().Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    //Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    //根据订单的sender字段获取当前店铺的前端分销商id
                    int distributorId = DistributorsBrower.GetSenderDistributorId(order.Sender);
                    DistributorsInfo currentSenderDistributor = DistributorsBrower.GetCurrentDistributors(distributorId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:0 10px;' >");
                    builder.AppendFormat("<div style='font-size:12px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/login_logo2.png' /><h3 style='margin:5px auto'>SALES MEMO</h3><div style='text-align:left;padding-bottom:2px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:2px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:2px;'><span>消费客户： </span>{3}</div><div style='text-align:left;padding-bottom:2px;'><span>订单编号： </span><span style='text-align:center;font-size:48px;line-height:48px;vertical-align: top;'>{2}</span></div></div><div style='border-bottom:1px dashed #000; margin:5px 0'></div>", currentSenderDistributor != null ? currentSenderDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId.Substring(order.OrderId.Length - CustomConfigHelper.Instance.OrderIdSortNumCount), "堂食客户");
                    builder.Append("<table style='width:100%;background:#fff;font-size:12px'><thead><tr><th style='width:50%;background:#fff;border:none;text-align:left;'>名称</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", System.Math.Round(info2.GetSubShowTotal(), 2));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:5px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    decimal reducedPromotionAmount2 = order.DiscountAmount;
                    if (reducedPromotionAmount1 > 0m || reducedPromotionAmount2 > 0m)
                    {
                        builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2) + System.Math.Round(reducedPromotionAmount2, 2));
                    }
                    decimal payCharge1 = order.PayCharge;
                    if (payCharge1 > 0m)
                    {
                        builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                    }
                    if (!string.IsNullOrEmpty(order.CouponCode))
                    {
                        decimal couponValue = order.CouponValue;
                        if (couponValue > 0m)
                        {
                            builder.AppendFormat("<div><span>优惠抵扣： </span>-￥{0}</div>", System.Math.Round(couponValue, 2));
                        }
                    }
                    //计算买一送一减免
                    decimal giveBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {
                        if (itemInfo.GiveQuantity > 0)
                        {
                            giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:12px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue - order.DiscountAmount, 2));
                    //插入二维码start-----------
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string savepath = System.Web.HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("distributor_{0}", currentSenderDistributor) + ".jpg";
                    if (!File.Exists(savepath))
                    {
                        Hishop.Weixin.MP.Api.TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("distributor_{0}", currentSenderDistributor), false);
                    }
                    string qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("distributor_{0}", currentSenderDistributor) + ".jpg";
                    builder.AppendFormat("<div style='text-align:center;width:100%;margin-top:10px;'><img src='{0}' style='width:30%' /></div>", qrCodeBackImgUrl);
                    //插入二维码end------------

                    //谢谢光临
                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:12px;'>全国加盟热线：400-043-0311</div>");

                    builder.Append("</div>");
                }
                string backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                context.Response.Write("{" + backJson + "}");
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                context.Response.Write("{" + backjson + "}");
            }
        }

        /// <summary>
        /// 根据点击的街道获取配送范围包括此街道的分销商信息,并拼接成下拉菜单传递回去
        /// </summary>
        /// <param name="context"></param>
        private void getDistributorByStreetId(System.Web.HttpContext context)
        {

            string backJson = "";
            string dropDistributorHtml = "<button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>请选择配送店铺<span class='caret'></span></button>";
            context.Response.ContentType = "application/json";
            try
            {
                dropDistributorHtml += "<ul id='distributorUl' class='dropdown-menu' role='menu'>";
                DataTable distributors = SalesHelper.GetDistributorRegions(context.Request["StreetId"]);
                foreach (DataRow row in distributors.Rows)
                {
                    string distributorId = row["distributorId"].ToString();
                    string storeName = row["storeName"].ToString();
                    dropDistributorHtml += string.Format("<li><a href='#' name='{1}' value='{1}'>{0}</a></li>", storeName, distributorId);
                }
                dropDistributorHtml += "</ul>";
                backJson += string.Format("\"success\":true,\"inHtml\":\"{0}\"", dropDistributorHtml);
                context.Response.Write("{" + backJson + "}");
            }
            catch
            {
                context.Response.Write("{\"success\":false}");
            }

        }

        /// <summary>
        /// 获取最新砍价页面信息并传回html代码
        /// </summary>
        private void reFreshCutDownInfo(System.Web.HttpContext context)
        {
            string backJson = "";
            context.Response.ContentType = "application/json";
            try
            {
                CutDownInfo cutDown = PromoteHelper.GetCutDown(Convert.ToInt32(context.Request["cutDownId"]));
                string currentPrice = "￥" + cutDown.CurrentPrice.ToString("F2");//当前价
                string cutDownPriceTotal = "￥" + PromoteHelper.GetCutDownTotalPrice(cutDown.CutDownId).ToString("F2");//当前被砍总价
                CutDownDetailsQuery query = new CutDownDetailsQuery()
                {
                    PageSize = 30,
                    PageIndex = 1,
                    CutDownId = cutDown.CutDownId,
                    SortBy = "cutTime",
                    SortOrder = Hidistro.Core.Enums.SortAction.Desc,
                };
                DataTable dtCutDownDetailList = (DataTable)PromoteHelper.GetCutDownDetailList(query).Data;//获取砍价详情
                string detailList = "";
                MemberInfo rowMember = new MemberInfo();
                foreach (DataRow row in dtCutDownDetailList.Rows)
                {
                    rowMember = MemberHelper.GetMember(Convert.ToInt32(row["memberId"]));
                    string MemberName = rowMember.UserName;
                    string cutTime = DateTime.Parse(row["cutTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    string imgUserHead = "<img src='" + rowMember.UserHead + "'/>";
                    string cutPrice = ((decimal)row["cutDownPrice"]).ToString("F2");
                    detailList += string.Format("<li><b>{0}</b><span>{1} 帮你砍了<i>{2}元</i></span></li>", imgUserHead, MemberName, cutPrice);
                }
                backJson += string.Format("\"success\":true,\"currentPrice\":\"{0}\",\"cutDownPriceTotal\":\"{1}\",\"detailList\":\"{2}\""
                    , currentPrice, cutDownPriceTotal, detailList);
                context.Response.Write("{" + backJson + "}");
            }
            catch
            {
                context.Response.Write("{\"success\":false}");
            }


        }
        /// <summary>
        /// 砍价
        /// </summary>
        private void cutDown(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            CutDownInfo cutDown = PromoteHelper.GetCutDown(Convert.ToInt32(context.Request["cutDownId"]));
            CutDownDetailInfo info = new CutDownDetailInfo()
            {
                CutDownId = cutDown.CutDownId,
                MemberId = Convert.ToInt32(context.Request["memberId"]),
                CutDownPrice = cutDown.PerCutPrice
            };
            string result = "";
            result = PromoteHelper.goCutDown(info);
            if (result == "success")//如果砍价成功
            {
                context.Response.Write("{\"success\":true}");
            }
            else//返回错误信息
            {
                context.Response.Write("{\"success\":\"" + result + "\"}");
            }
        }

        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="context"></param>
        private void getCoupon(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            DataTable allCoupons = CouponHelper.GetAllCoupons();//优惠券列表
            DataTable allCouponItemsClaimCode = CouponHelper.GetAllCouponItemsClaimCode();//所有已发送优惠券的claimcode等信息
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                return;//如果当前没有登录则返回

            int cs = allCouponItemsClaimCode.Select(string.Format("CouponId={0} and userId={1}", context.Request["CouponId"], currentMember.UserId)).Length;
            if (cs > 0)
            {
                context.Response.Write("{\"success\":false}");//如果领取过,返回false
                return;
            }

            //发送优惠券
            int couponId = Convert.ToInt32(context.Request["CouponId"]);
            string claimCode = (context.Request["CouponId"].ToString() + "|" + currentMember.UserId.ToString()).PadLeft(15, 'b');//代码格式为:bbbbbbbbbb[couponid]|[userid]  15位
            CouponItemInfo item = new CouponItemInfo();
            System.Collections.Generic.IList<CouponItemInfo> listCouponItem = new System.Collections.Generic.List<CouponItemInfo>();
            item = new CouponItemInfo(couponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
            listCouponItem.Add(item);
            CouponHelper.SendClaimCodes(couponId, listCouponItem);
            context.Response.Write("{\"success\":true}");
        }



        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="context"></param>
        private void CloseOrder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                OrderHelper.SetOrderState(context.Request["orderId"].ToString(), OrderStatus.Closed);
                context.Response.Write("{\"success\":true}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        /// <summary>
        /// 判断配送方式是不是唯一,并且是包含SN的
        /// </summary>
        /// <param name="context"></param>
        private void IsShippingModeOnly(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            IList<ShippingModeInfo> shippingModes = SalesHelper.GetShippingModes();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();

            if (shippingModes.Count == 1 && shippingModes[0].Name.IndexOf("SN") > -1)//如果有且只有一条数据,并且包含SN码支付内的字样,传送回true
            {
                context.Response.Write("{\"success\":true,\"name\":\"" + currentMember.UserName + "\",\"tel\":\"" + currentMember.CellPhone + "\",\"shippingmodeid\":" + shippingModes[0].ModeId + "}");
            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        /// <summary>
        /// 判断配送方式是不是唯一的
        /// </summary>
        /// <param name="context"></param>
        private void IsPaymentModeOnly(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes();
            //如判断支付方式是不是只有一种
            int paymentmodeid = -1;
            int paymentModeCount = 0;
            if (SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
            {
                paymentModeCount++;
                paymentmodeid = 88;
            }
            if (SettingsManager.GetMasterSettings(false).EnableOffLineRequest)
            {
                paymentModeCount++;
                paymentmodeid = 99;
            }
            if (SettingsManager.GetMasterSettings(false).EnablePodRequest)
            {
                paymentModeCount++;
                paymentmodeid = 0;
            }
            paymentModeCount += paymentModes.Count;
            if (paymentModeCount == 1)
            {
                if (paymentModes.Count == 1)
                {
                    paymentmodeid = paymentModes[0].ModeId;
                }
                context.Response.Write("{\"success\":true,\"paymentmodeid\":" + paymentmodeid + "}");

            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        /// <summary>
        /// 判断订单状态是否变更,并且为已完成
        /// </summary>
        /// <param name="context"></param>
        private void IsOrderStateChange(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string latestStatus = OrderHelper.GetSimpleOrderInfo(context.Request["orderid"]).Rows[0]["OrderStatus"].ToString();
            if (latestStatus != context.Request["orderstatus"] && latestStatus == "5")//当订单编号变更了,并且为完成状态时,传递true
            {
                context.Response.Write("{\"success\":true}");
            }
            else
            {
                context.Response.Write("{\"success\":false}");
            }
        }

        private void ProcessSubmitMemberCard(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                currentMember.Address = context.Request.Form.Get("address");
                currentMember.RealName = context.Request.Form.Get("name");
                currentMember.CellPhone = context.Request.Form.Get("phone");
                currentMember.QQ = context.Request.Form.Get("qq");
                if (!string.IsNullOrEmpty(currentMember.QQ))
                {
                    currentMember.Email = currentMember.QQ + "@qq.com";
                }
                currentMember.VipCardNumber = SettingsManager.GetMasterSettings(true).VipCardPrefix + currentMember.UserId.ToString();
                currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
                string s = MemberProcessor.UpdateMember(currentMember) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.Write(s);
            }
        }

        //生成订单
        private void ProcessSubmmitorder(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            int result = 0;
            int PaymentTypeId = 0;
            string str = context.Request["couponCode"];                             //优惠券代码
            int shippingId = int.Parse(context.Request["shippingId"]);              //收货地址ID
            int receiptId = 0;
            if (context.Request["receiptId"] != null && !string.IsNullOrEmpty(context.Request["receiptId"].ToString()))
            {
                receiptId = int.Parse(context.Request["receiptId"]);              //发票信息
            }
            /******start服务商品修改，提供服务的门店ID*****/
            int serviceUserId = 0;
            if (context.Request["serviceUserId"] != null && !string.IsNullOrEmpty(context.Request["serviceUserId"].ToString()))
            {
                serviceUserId = int.Parse(context.Request["serviceUserId"]);              //服务门店ID
            }
            /******end服务商品修改，提供服务的门店ID*****/

            int groupbuyId;//团购id
            int countdownId;//限时抢购id
            int cutdownId;//砍价id

            bool isFreeShipping = Convert.ToBoolean(context.Request["isFreeShipping"]);//订单是否包邮
            bool flagGroupBuy = (int.TryParse(context.Request["groupbuyId"], out groupbuyId) && groupbuyId != 0);
            bool flagCountDown = (int.TryParse(context.Request["countDownId"], out countdownId) && countdownId != 0);
            bool flagCutDown = (int.TryParse(context.Request["cutdownId"], out cutdownId) && cutdownId != 0);
            string remark = context.Request["remark"];                               //订单备注
            int buyAmount = 0;                                                       //购买数量
            ShoppingCartInfo shoppingCart;
            string str5 = "";
            if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(context.Request["productSku"]) && !string.IsNullOrEmpty(context.Request["from"]) && (context.Request["from"] == "signBuy" || context.Request["from"] == "groupBuy" || context.Request["from"] == "countDown" || context.Request["from"] == "cutDown"))
            {
                string productSkuId = context.Request["productSku"];
                if (context.Request["from"] == "signBuy")
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount);//得到购物车信息()
                }
                else if (context.Request["from"] == "countDown")//抢购
                {
                    shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(productSkuId, buyAmount);
                }
                else if (context.Request["from"] == "cutDown")//砍价
                {
                    shoppingCart = ShoppingCartProcessor.GetCutDownShoppingCart(productSkuId, buyAmount, cutdownId);
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(groupbuyId, productSkuId, buyAmount);
                }
            }
            else
            {
                //取当前登录会员的购物车
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            //得到订单
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();

            //2016-12-19修改，是否享受批发价格
            bool isPf = false;//是否享受批发价格，如果改商品有其它活动则无法享受到批发价与内购价优惠
            bool isNg = false;//是否内购价
            if (shoppingCart.LineItems.Count > 0 && !flagGroupBuy && !flagCountDown && !flagCutDown)
            {
                //2017-08-14-wt 验证批发价格、内购价格，  如果购买是批发价格、内购价格则不享有其他的价格优惠
                VshopBrowser.ValidatePriceAndSetValue(currentMember, shoppingCart.LineItems[0], out isPf, out isNg);
            }

            #region *************Star   2017-8-11 yk  买一送一商品修改***********************
            if (!isPf && !isNg && !flagGroupBuy && !flagCountDown && !flagCutDown)
            {
                foreach (ShoppingCartItemInfo infoFree in shoppingCart.LineItems)
                {
                    #region 如果该商品是买一送一活动商品并且在活动时间内并且该用户拥有该活动资格，则添加赠送商品
                    //得到该商品活动的实体
                    BuyOneGetOneFreeInfo buyOneInfo = BuyOneGetOnrFreeHelper.GetProductBuyOneGetOneFreeInfo(infoFree.ProductId);
                    if (buyOneInfo != null)
                    {
                        //得到该用户参与该商品活动的【总次数】
                        int getNum = BuyOneGetOnrFreeHelper.getUserGetNum(currentMember.UserId, buyOneInfo.buyoneId);
                        //当前用户对于活动的【剩余赠送次数】
                        int Surplus = buyOneInfo.getNum - getNum;

                        if (buyOneInfo != null && DateTime.Now > buyOneInfo.startime && DateTime.Now < buyOneInfo.endtime)//活动时间验证
                        {
                            if (Surplus > 0)//表示当前用户还有资格参与活动
                            {

                                if (infoFree.Quantity > Surplus)//如果【购买数量】大于【剩余赠送次数】则赠送最大数量为【剩余赠送次数】
                                {
                                    infoFree.Quantity = infoFree.Quantity + buyOneInfo.getNum;
                                    infoFree.GiveQuantity = buyOneInfo.getNum;
                                }
                                else
                                {
                                    infoFree.GiveQuantity = infoFree.Quantity;//赠送数量 
                                    infoFree.Quantity = infoFree.Quantity * 2;//实际数量
                                }
                            }
                        }

                        #region  正式上线后添加到wx_pay中  若当前用户成功参与当前买一送一活动 则在活动表中增加一条记录
                        BuyOneGetOneDetailFreeInfo infoDetail = new BuyOneGetOneDetailFreeInfo
                        {
                            buyoneId = buyOneInfo.buyoneId,
                            buyDate = DateTime.Now,
                            userId = currentMember.UserId,
                        };
                        BuyOneGetOnrFreeHelper.AddBuyOneGetOneDetail(infoDetail);
                        #endregion 
                    }
                    #endregion

                    
                }
            }
            #endregion *************End       2017-8-11 yk  买一送一商品修改***********************


            //生成订单对象
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCart, currentMember, flagCountDown, flagGroupBuy);
            if (orderInfo == null)
            {
                //builder.Append("\"Status\":\"None\"");
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", str5);
            }
            else
            {
                //orderInfo.OrderId = isAgent ? this.GenerateOrderId(managerinfo.UserId) : this.GenerateOrderIdByOrder(4);//代理商采购修改
                orderInfo.OrderId = this.GenerateOrderId();
                orderInfo.OrderDate = System.DateTime.Now;
                currentMember = MemberProcessor.GetCurrentMember();
                orderInfo.UserId = currentMember.UserId;
                orderInfo.Username = currentMember.UserName;
                orderInfo.EmailAddress = currentMember.Email;
                orderInfo.RealName = currentMember.RealName;
                orderInfo.QQ = currentMember.QQ;
                orderInfo.Remark = remark;
                orderInfo.buyType = 0;//默认为正常一口价
                //满减配置
                if (!isPf && !isNg && !flagGroupBuy && !flagCountDown && !flagCutDown)
                {
                    string activitiesId = "";
                    string activitiesName = "";
                    orderInfo.DiscountAmount = this.DiscountMoney(orderInfo, out activitiesId, out activitiesName);
                    orderInfo.ActivitiesId = activitiesId;
                    orderInfo.ActivitiesName = activitiesName;
                    if (!string.IsNullOrEmpty(activitiesId))
                        orderInfo.buyType = 6;//满减
                }
                //得到价格类型
                if (isPf && !flagGroupBuy && !flagCountDown && !flagCutDown)
                {
                    orderInfo.buyType = 5;//批发价
                }
                if (isNg && !flagGroupBuy && !flagCountDown && !flagCutDown)
                {
                    orderInfo.buyType = 4;//内购价
                }
                if (flagGroupBuy)
                {
                    GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
                    orderInfo.GroupBuyId = groupbuyId;
                    orderInfo.NeedPrice = groupBuy.NeedPrice;
                    orderInfo.GroupBuyStatus = groupBuy.Status;
                    orderInfo.buyType = 1;//团购
                }
                if (flagCountDown)//限时抢购判断
                {
                    CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(shoppingCart.LineItems[0].ProductId);
                    if (countDownInfo != null)
                    {
                        if (countDownInfo.EndDate < DateTime.Now)
                        {
                            str5 = "此订单为抢购订单，但抢购时间已到！";
                        }
                        if (shoppingCart.LineItems[0].Quantity > countDownInfo.MaxCount)
                        {
                            str5 = "你购买的数量超过限购数量:" + countDownInfo.MaxCount.ToString();
                        }
                        int num9 = ShoppingProcessor.CountDownOrderCount(shoppingCart.LineItems[0].ProductId);
                        if ((num9 + shoppingCart.LineItems[0].Quantity) > countDownInfo.MaxCount)
                        {
                            str5 = string.Format("你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！", num9, countDownInfo.MaxCount);
                        }
                        orderInfo.CountDownBuyId = countDownInfo.CountDownId;
                        orderInfo.buyType = 2;//限时抢购
                    }
                    else
                    {
                        str5 = "抢购信息不存在或者已过期！";
                    }
                }
                if (flagCutDown)//砍价判断
                {
                    CutDownInfo cutDownInfo = PromoteHelper.GetCutDown(cutdownId);//GetCutDownInfo(shoppingCart.LineItems[0].ProductId);
                    if (cutDownInfo != null)
                    {
                        if (cutDownInfo.EndDate < DateTime.Now)
                        {
                            str5 = "此订单为抢购订单，但抢购时间已到！";
                        }
                        if (shoppingCart.LineItems[0].Quantity > cutDownInfo.Count)
                        {
                            str5 = "你购买的数量超过限购数量:" + cutDownInfo.Count.ToString();
                        }
                        int num9 = cutDownInfo.SoldCount;//ShoppingProcessor.CountDownOrderCount(shoppingCart.LineItems[0].ProductId);
                        if ((num9 + shoppingCart.LineItems[0].Quantity) > cutDownInfo.Count)
                        {
                            str5 = string.Format("此商品已经被抢购一空！你已经抢购过该商品{0}件，如果你有未付款的抢购单，请及时支付！", num9);
                        }
                        if (cutDownInfo.SoldCount >= cutDownInfo.Count || cutDownInfo.EndDate <= DateTime.Now)//如果已达购买量上限或者活动时间结束了,自动结束活动
                        {
                            PromoteHelper.CloseCutDown(cutdownId);
                        }
                        orderInfo.CutDownBuyId = cutDownInfo.CutDownId;
                        orderInfo.buyType = 3;//砍价
                    }
                    else
                    {
                        str5 = "砍价信息不存在或者已过期！";
                    }
                }
                foreach (ShoppingCartItemInfo info5 in shoppingCart.LineItems)
                {
                    if (ShoppingCartProcessor.GetSkuStock(info5.SkuId) < info5.Quantity)
                    {
                        str5 = string.Format("订单中有商品({0}{1})库存不足！", info5.Name, info5.SkuContent);
                        break;
                    }
                }

                if (str5 != "")
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", str5);
                    HiCache.Remove("DataCache-Member-" + currentMember.UserId);
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                    return;
                    //this.jsondict.Add("Status", "Error");
                    //this.jsondict.Add("ErrorMsg", str5);
                    //this.WriteJson(context, 0);
                }
                //if (orderInfo.OrderSource == 3)//原始3为礼品订单， 现在为服务商品订单
                if (orderInfo.OrderSource == 99)
                    orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;//默认为已付款
                else
                    orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;//默认为待付款
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = context.Request["shiptoDate"];

                if (currentMember.DistributorUserId > 0)
                {
                    orderInfo.ReferralUserId = currentMember.DistributorUserId;
                }
                else
                {
                    orderInfo.ReferralUserId = 0;
                }
                /*
                if (System.Web.HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null)
                {
                    orderInfo.ReferralUserId = int.Parse(System.Web.HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value);
                }
                else
                {
                    orderInfo.ReferralUserId = 0;
                }*/

                ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
                if (shippingAddress != null)
                {
                    if (!string.IsNullOrEmpty(shippingAddress.TelPhone))
                    {
                        orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                        orderInfo.RegionId = shippingAddress.RegionId;
                        orderInfo.Address = shippingAddress.Address;
                        orderInfo.ZipCode = shippingAddress.Zipcode;
                        orderInfo.ShipTo = shippingAddress.ShipTo;
                        orderInfo.TelPhone = shippingAddress.TelPhone;
                        orderInfo.CellPhone = shippingAddress.CellPhone;
                        MemberProcessor.SetDefaultShippingAddress(shippingId, MemberProcessor.GetCurrentMember().UserId);
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                        builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", "请在收货信息中填写发票抬头信息，您可以新增一个收货信息或者在（我-收货地址)中去修改。");
                        HiCache.Remove("DataCache-Member-" + currentMember.UserId);
                        builder.Append("}");
                        context.Response.ContentType = "application/json";
                        context.Response.Write(builder.ToString());
                        return;
                    }
                }

                //发票信息//2016-09-28
                orderInfo.receiptId = receiptId;
                /***2017-07-26修改服务订单核销码，提供服务的门店ID****/
                if (orderInfo.OrderSource == 3)
                {
                    orderInfo.serviceCode = this.GenerateServiceCode();
                    orderInfo.serviceUserId = serviceUserId;
                }
                /***服务订单核销码，提供服务的门店ID****/

                if (int.TryParse(context.Request["shippingType"], out result) && result != 99)//根据选择的配送方式id获取配送方式信息
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(result, true);
                    if (shippingMode != null)
                    {
                        orderInfo.ShippingModeId = shippingMode.ModeId;
                        orderInfo.ModeName = shippingMode.Name;
                        if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping))
                        {
                            orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCart.Weight, shippingMode));
                        }
                        else
                        {
                            orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
                        }
                        //如果达到了包邮金额,则包邮.
                        Hidistro.Core.Entities.SiteSettings masterSettings = Hidistro.Core.SettingsManager.GetMasterSettings(false);
                        bool isAllFreeShipping = false;
                        bool isAddShipping = false;
                        if (masterSettings.SpecialOrderAmountType == "freeShipping")
                            isAllFreeShipping = orderInfo.GetAmount() >= masterSettings.SpecialValue1 && masterSettings.SpecialValue1 > 0M;
                        else if (masterSettings.SpecialOrderAmountType == "addShipping")
                            isAddShipping = orderInfo.GetAmount() < masterSettings.SpecialValue1 && masterSettings.SpecialValue1 > 0M;
                        if (isAllFreeShipping)
                            orderInfo.AdjustedFreight = 0M;
                        else if (isAddShipping)
                            orderInfo.AdjustedFreight += masterSettings.SpecialValue2;
                    }
                }
                if (int.TryParse(context.Request["paymentType"], out PaymentTypeId))
                {
                    orderInfo.PaymentTypeId = PaymentTypeId;
                    switch (PaymentTypeId)
                    {
                        case -1:
                        case 0:
                            {
                                orderInfo.PaymentType = "货到付款";
                                orderInfo.Gateway = "hishop.plugins.payment.podrequest";
                                break;
                            }
                        case 88:
                            {
                                orderInfo.PaymentType = "微信支付";
                                orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
                                break;
                            }
                        case 99:
                            {
                                orderInfo.PaymentType = "线下付款";
                                orderInfo.Gateway = "hishop.plugins.payment.offlinerequest";
                                break;
                            }
                        default:
                            {
                                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(PaymentTypeId);
                                if (paymentMode != null)
                                {
                                    orderInfo.PaymentTypeId = paymentMode.ModeId;
                                    orderInfo.PaymentType = paymentMode.Name;
                                    orderInfo.Gateway = paymentMode.Gateway;
                                }
                                break;
                            }
                    }
                }
                else
                {
                    //礼品支付类型默认积分支付
                    if (orderInfo.OrderSource == 99)
                    {
                        orderInfo.PaymentType = "积分支付";
                    }
                }
                //优惠券
                if (!string.IsNullOrEmpty(str))
                {
                    DataTable dt = CouponHelper.DeCodeShowCode(str);
                    if (dt != null)//如果是用的showCode提交的优惠券,则获取claimCode
                    {
                        str = dt.Rows[0]["ClaimCode"].ToString();
                    }
                    CouponInfo couponInfo = ShoppingProcessor.UseCoupon(shoppingCart.GetTotal(), str);
                    orderInfo.CouponName = couponInfo.Name;
                    if (couponInfo.Amount.HasValue)
                    {
                        orderInfo.CouponAmount = couponInfo.Amount.Value;
                    }
                    orderInfo.CouponCode = str;
                    orderInfo.CouponValue = couponInfo.DiscountValue;
                }
                try
                {
                    this.SetOrderItemStatus(orderInfo);
                    //this.InsertCalculationCommission(orderInfo.OrderId, orderInfo, selectAgentId);//计算返佣
                    //没有享受批发价格才可能计算返佣 , 服务商品不记录返佣
                    if (orderInfo.OrderSource != 3)
                    {
                        if (orderInfo.OrderSource == 1 || !isPf)
                            this.InsertCalculationCommission(orderInfo.OrderId, orderInfo);//计算返佣
                    }
                    if (ShoppingProcessor.CreatOrder(orderInfo))
                    {
                        ///设置复购会员，根据收获人/电话，验证是否金立会员
                        if (!string.IsNullOrEmpty(orderInfo.ShipTo.Trim()) && !string.IsNullOrEmpty(orderInfo.CellPhone.Trim()))
                        {
                            try
                            {
                                DataTable dtCwMember = CWMembersHelper.GetCwMemberByCellPhone(orderInfo.CellPhone.Trim());
                                foreach (DataRow drCw in dtCwMember.Rows)
                                {
                                    drCw["RelationUserId"] = currentMember.UserId;
                                }
                                string sql = "select * from dbo.CW_Members";
                                DataBaseHelper.CommitDataTable(dtCwMember.GetChanges(), sql);
                            }
                            catch { }
                        }


                        //if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && ManagerHelper.GetCurrentManager() != null && context.Request.UrlReferrer.AbsoluteUri.IndexOf("admin") > -1)
                        //    ShoppingCartProcessor.ClearShoppingCartPC();
                        //else
                        ShoppingCartProcessor.ClearShoppingCart();

                        if (orderInfo.Gifts.Count > 0)
                        {
                            ShoppingCartProcessor.ClearGiftShoppingCart();
                            ShoppingProcessor.CutNeedPoint(shoppingCart.GetTotalNeedPoint(), orderInfo.OrderId);
                        }
                        //除去礼品兑换以外，创维及供应商商品订单都发送通知
                        if (orderInfo.OrderSource != 99)
                        {
                            //Messenger.OrderCreated(orderInfo, currentMember);通知购买者，这里不需要通知购买者
                            MemberInfo tzMember = null;
                            if (orderInfo.OrderSource == 3)
                            {
                                if (orderInfo.serviceUserId > 0)
                                    tzMember = MemberProcessor.GetMember(orderInfo.serviceUserId);
                            }
                            else
                            {
                                if (orderInfo.ReferralUserId > 0)
                                {
                                    //通知门店
                                    tzMember = MemberProcessor.GetMember(orderInfo.ReferralUserId);
                                }
                                else
                                {
                                    //通知主店对应管理员
                                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                    if (!string.IsNullOrEmpty(masterSettings.ManageOpenID))
                                    {
                                        tzMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                                    }
                                }
                            }
                            if (tzMember != null)
                                Messenger.OrderCreatedSendManage(orderInfo, tzMember);
                        }

                        builder.Append("\"Status\":\"OK\",");
                        builder.AppendFormat("\"OrderId\":\"{0}\"", orderInfo.OrderId);
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                        builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", "订单创建失败！");
                    }
                }
                catch (OrderException exception)
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", exception.Message);
                }
            }
            HiCache.Remove("DataCache-Member-" + currentMember.UserId);
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());

        }


        //生成订单,虚拟订单专用
        private void ProcessSubmmitorderVirtual(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            int result = 0;
            int PaymentTypeId = 0;
            string str = context.Request["couponCode"];                             //优惠券代码
            //int shippingId = 0;
            //if(context.Request["shippingId"] != null && !string.IsNullOrEmpty(context.Request["shippingId"].ToString()))
            //{
            //    shippingId = int.Parse(context.Request["shippingId"]);              //收货地址ID
            //}
            int receiptId = 0;
            if (context.Request["receiptId"] != null && !string.IsNullOrEmpty(context.Request["receiptId"].ToString()))
            {
                receiptId = int.Parse(context.Request["receiptId"]);              //发票信息
            }

            int groupbuyId;//团购id
            int countdownId;//限时抢购id
            int cutdownId;//砍价id

            //bool isFreeShipping = Convert.ToBoolean(context.Request["isFreeShipping"]);//订单是否包邮
            bool isFreeShipping = true;
            bool flagGroupBuy = (int.TryParse(context.Request["groupbuyId"], out groupbuyId) && groupbuyId != 0);
            bool flagCountDown = (int.TryParse(context.Request["countDownId"], out countdownId) && countdownId != 0);
            bool flagCutDown = (int.TryParse(context.Request["cutdownId"], out cutdownId) && cutdownId != 0);
            string remark = context.Request["remark"];                               //订单备注
            int buyAmount = 0;                                                       //购买数量
            ShoppingCartInfo shoppingCart;
            string str5 = "";
            if (int.TryParse(context.Request["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(context.Request["productSku"]) && !string.IsNullOrEmpty(context.Request["from"]) && (context.Request["from"] == "signBuy" || context.Request["from"] == "groupBuy" || context.Request["from"] == "countDown" || context.Request["from"] == "cutDown"))
            {
                string productSkuId = context.Request["productSku"];
                if (context.Request["from"] == "signBuy")
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSkuId, buyAmount);//得到购物车信息()
                }
                else if (context.Request["from"] == "countDown")//抢购
                {
                    shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(productSkuId, buyAmount);
                }
                else if (context.Request["from"] == "cutDown")//砍价
                {
                    shoppingCart = ShoppingCartProcessor.GetCutDownShoppingCart(productSkuId, buyAmount, cutdownId);
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(groupbuyId, productSkuId, buyAmount);
                }
            }
            else
            {
                //取当前登录会员的购物车
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            //得到订单
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();

            //2016-12-19修改，是否享受批发价格
            bool isPf = false;//是否享受批发价格，如果改商品有其它活动则无法享受到批发价与内购价优惠
            bool isNg = false;//是否内购价
            if (shoppingCart.LineItems.Count > 0 && !flagGroupBuy && !flagCountDown && !flagCutDown)
            {
                //2017-08-14-wt 验证批发价格、内购价格，  如果购买是批发价格、内购价格则不享有其他的价格优惠
                VshopBrowser.ValidatePriceAndSetValue(currentMember, shoppingCart.LineItems[0], out isPf, out isNg);
            }

            #region *************Star   2017-8-11 yk  买一送一商品修改***********************
            if (!isPf && !isNg && !flagGroupBuy && !flagCountDown && !flagCutDown)
            {
                foreach (ShoppingCartItemInfo infoFree in shoppingCart.LineItems)
                {
                    #region 如果该商品是买一送一活动商品并且在活动时间内并且该用户拥有该活动资格，则添加赠送商品
                    //得到该商品活动的实体
                    BuyOneGetOneFreeInfo buyOneInfo = BuyOneGetOnrFreeHelper.GetProductBuyOneGetOneFreeInfo(infoFree.ProductId);
                    if (buyOneInfo != null)
                    {
                        //得到该用户参与该商品活动的【总次数】
                        int getNum = BuyOneGetOnrFreeHelper.getUserGetNum(currentMember.UserId, buyOneInfo.buyoneId);
                        //当前用户对于活动的【剩余赠送次数】
                        int Surplus = buyOneInfo.getNum - getNum;

                        if (buyOneInfo != null && DateTime.Now > buyOneInfo.startime && DateTime.Now < buyOneInfo.endtime)//活动时间验证
                        {
                            if (Surplus > 0)//表示当前用户还有资格参与活动
                            {

                                if (infoFree.Quantity > Surplus)//如果【购买数量】大于【剩余赠送次数】则赠送最大数量为【剩余赠送次数】
                                {
                                    infoFree.Quantity = infoFree.Quantity + buyOneInfo.getNum;
                                    infoFree.GiveQuantity = buyOneInfo.getNum;
                                }
                                else
                                {
                                    infoFree.GiveQuantity = infoFree.Quantity;//赠送数量 
                                    infoFree.Quantity = infoFree.Quantity * 2;//实际数量
                                }
                            }
                        }

                        #region  若当前用户成功参与当前买一送一活动 则在活动表中增加一条记录
                        BuyOneGetOneDetailFreeInfo infoDetail = new BuyOneGetOneDetailFreeInfo
                        {
                            buyoneId = buyOneInfo.buyoneId,
                            buyDate = DateTime.Now,
                            userId = currentMember.UserId,
                        };
                        BuyOneGetOnrFreeHelper.AddBuyOneGetOneDetail(infoDetail);
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion *************End       2017-8-11 yk  买一送一商品修改***********************

            //生成订单对象
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCart, currentMember, flagCountDown, flagGroupBuy);
            if (orderInfo == null)
            {
                //builder.Append("\"Status\":\"None\"");
                builder.Append("\"Status\":\"Error\"");
                builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", str5);
            }
            else
            {
                //orderInfo.OrderId = isAgent ? this.GenerateOrderId(managerinfo.UserId) : this.GenerateOrderIdByOrder(4);//代理商采购修改
                orderInfo.OrderId = this.GenerateOrderId();
                orderInfo.OrderDate = System.DateTime.Now;
                currentMember = MemberProcessor.GetCurrentMember();
                orderInfo.UserId = currentMember.UserId;
                orderInfo.Username = currentMember.UserName;
                orderInfo.EmailAddress = currentMember.Email;
                orderInfo.RealName = currentMember.RealName;
                orderInfo.QQ = currentMember.QQ;
                orderInfo.Remark = remark;
                orderInfo.buyType = 0;//默认为正常一口价
                if (!isPf && !isNg && !flagGroupBuy && !flagCountDown && !flagCutDown)
                {
                    //满减配置
                    string activitiesId = "";
                    string activitiesName = "";
                    orderInfo.DiscountAmount = this.DiscountMoney(orderInfo, out activitiesId, out activitiesName);
                    orderInfo.ActivitiesId = activitiesId;
                    orderInfo.ActivitiesName = activitiesName;
                    if (!string.IsNullOrEmpty(activitiesId))
                        orderInfo.buyType = 6;//满减
                }

                //得到价格类型
                if (isPf)
                {
                    orderInfo.buyType = 5;//批发价
                }
                if (isNg)
                {
                    orderInfo.buyType = 4;//内购价
                }
                if (flagGroupBuy)
                {
                    GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
                    orderInfo.GroupBuyId = groupbuyId;
                    orderInfo.NeedPrice = groupBuy.NeedPrice;
                    orderInfo.GroupBuyStatus = groupBuy.Status;
                    orderInfo.buyType = 1;//团购
                }
                if (flagCountDown)//限时抢购判断
                {
                    CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(shoppingCart.LineItems[0].ProductId);
                    if (countDownInfo != null)
                    {
                        if (countDownInfo.EndDate < DateTime.Now)
                        {
                            str5 = "此订单为抢购订单，但抢购时间已到！";
                        }
                        if (shoppingCart.LineItems[0].Quantity > countDownInfo.MaxCount)
                        {
                            str5 = "你购买的数量超过限购数量:" + countDownInfo.MaxCount.ToString();
                        }
                        int num9 = ShoppingProcessor.CountDownOrderCount(shoppingCart.LineItems[0].ProductId);
                        if ((num9 + shoppingCart.LineItems[0].Quantity) > countDownInfo.MaxCount)
                        {
                            str5 = string.Format("你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！", num9, countDownInfo.MaxCount);
                        }
                        orderInfo.CountDownBuyId = countDownInfo.CountDownId;
                        orderInfo.buyType = 2;//限时抢购
                    }
                    else
                    {
                        str5 = "抢购信息不存在或者已过期！";
                    }
                }
                if (flagCutDown)//砍价判断
                {
                    CutDownInfo cutDownInfo = PromoteHelper.GetCutDown(cutdownId);//GetCutDownInfo(shoppingCart.LineItems[0].ProductId);
                    if (cutDownInfo != null)
                    {
                        if (cutDownInfo.EndDate < DateTime.Now)
                        {
                            str5 = "此订单为抢购订单，但抢购时间已到！";
                        }
                        if (shoppingCart.LineItems[0].Quantity > cutDownInfo.Count)
                        {
                            str5 = "你购买的数量超过限购数量:" + cutDownInfo.Count.ToString();
                        }
                        int num9 = cutDownInfo.SoldCount;//ShoppingProcessor.CountDownOrderCount(shoppingCart.LineItems[0].ProductId);
                        if ((num9 + shoppingCart.LineItems[0].Quantity) > cutDownInfo.Count)
                        {
                            str5 = string.Format("此商品已经被抢购一空！你已经抢购过该商品{0}件，如果你有未付款的抢购单，请及时支付！", num9);
                        }
                        if (cutDownInfo.SoldCount >= cutDownInfo.Count || cutDownInfo.EndDate <= DateTime.Now)//如果已达购买量上限或者活动时间结束了,自动结束活动
                        {
                            PromoteHelper.CloseCutDown(cutdownId);
                        }
                        orderInfo.CutDownBuyId = cutDownInfo.CutDownId;
                        orderInfo.buyType = 3;//砍价
                    }
                    else
                    {
                        str5 = "砍价信息不存在或者已过期！";
                    }
                }
                foreach (ShoppingCartItemInfo info5 in shoppingCart.LineItems)
                {
                    if (ShoppingCartProcessor.GetSkuStock(info5.SkuId) < info5.Quantity)
                    {
                        str5 = string.Format("订单中有商品({0}{1})库存不足！", info5.Name, info5.SkuContent);
                        break;
                    }

                    //**********Start 2017-08-15-wt 验证虚拟码数量是否满足**************/
                    string strwhere = string.Format("ProductId = {0} and SkuId = '{1}' and VirtualState = 0", info5.ProductId, info5.SkuId);
                    int virtualcodeCount = ProductVirtualInfoHelper.SelectProductVirtualByWhere(strwhere).Rows.Count;
                    if (virtualcodeCount < info5.Quantity)
                    {
                        str5 = string.Format("订单中有商品({0}{1})库存不足！", info5.Name, info5.SkuContent);
                        break;
                    }
                    //**********End 2017-08-15-wt 验证虚拟码数量是否满足**************/
                }

                if (str5 != "")
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", str5);
                    HiCache.Remove("DataCache-Member-" + currentMember.UserId);
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                    return;
                    //this.jsondict.Add("Status", "Error");
                    //this.jsondict.Add("ErrorMsg", str5);
                    //this.WriteJson(context, 0);
                }
                
                /***start虚拟订单自动，正式上线屏蔽打开***/

                //orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;//默认为待付款
                orderInfo.OrderStatus = OrderStatus.Finished;
                orderInfo.PayDate = new DateTime?(DateTime.Now);
                orderInfo.FinishDate = new DateTime?(DateTime.Now);

                /***end虚拟订单自动，正式上线屏蔽打开***/

                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = context.Request["shiptoDate"];

                if (currentMember.DistributorUserId > 0)
                {
                    orderInfo.ReferralUserId = currentMember.DistributorUserId;
                }
                else
                {
                    orderInfo.ReferralUserId = 0;
                }
                //发票信息//2016-09-28
                orderInfo.receiptId = receiptId;
                //支付方式
                if (int.TryParse(context.Request["paymentType"], out PaymentTypeId))
                {
                    orderInfo.PaymentTypeId = PaymentTypeId;
                    switch (PaymentTypeId)
                    {
                        case -1:
                        case 0:
                            {
                                orderInfo.PaymentType = "货到付款";
                                orderInfo.Gateway = "hishop.plugins.payment.podrequest";
                                break;
                            }
                        case 88:
                            {
                                orderInfo.PaymentType = "微信支付";
                                orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
                                break;
                            }
                        case 99:
                            {
                                orderInfo.PaymentType = "线下付款";
                                orderInfo.Gateway = "hishop.plugins.payment.offlinerequest";
                                break;
                            }
                        default:
                            {
                                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(PaymentTypeId);
                                if (paymentMode != null)
                                {
                                    orderInfo.PaymentTypeId = paymentMode.ModeId;
                                    orderInfo.PaymentType = paymentMode.Name;
                                    orderInfo.Gateway = paymentMode.Gateway;
                                }
                                break;
                            }
                    }
                }
                else
                {
                    //礼品支付类型默认积分支付
                    if (orderInfo.OrderSource == 99)
                    {
                        orderInfo.PaymentType = "积分支付";
                    }
                }
                //优惠券
                if (!string.IsNullOrEmpty(str))
                {
                    DataTable dt = CouponHelper.DeCodeShowCode(str);
                    if (dt != null)//如果是用的showCode提交的优惠券,则获取claimCode
                    {
                        str = dt.Rows[0]["ClaimCode"].ToString();
                    }
                    CouponInfo couponInfo = ShoppingProcessor.UseCoupon(shoppingCart.GetTotal(), str);
                    orderInfo.CouponName = couponInfo.Name;
                    if (couponInfo.Amount.HasValue)
                    {
                        orderInfo.CouponAmount = couponInfo.Amount.Value;
                    }
                    orderInfo.CouponCode = str;
                    orderInfo.CouponValue = couponInfo.DiscountValue;
                }
                try
                {
                    this.SetOrderItemStatus(orderInfo);
                    //this.InsertCalculationCommission(orderInfo.OrderId, orderInfo, selectAgentId);//计算返佣
                    //没有享受批发价格才可能计算返佣 , 服务商品不记录返佣
                    if (orderInfo.OrderSource != 3)
                    {
                        if (orderInfo.OrderSource == 1 || !isPf)
                            this.InsertCalculationCommission(orderInfo.OrderId, orderInfo);//计算返佣
                    }
                    if (ShoppingProcessor.CreatOrder(orderInfo))
                    {
                        /**正式上线前屏蔽****/
                        foreach (LineItemInfo iteminfo in orderInfo.LineItems.Values)
                        {
                            //分配虚拟码
                            OrderVirtualInfoHelper.AddOrderVirtualInfo(orderInfo.OrderId, iteminfo.ProductId, iteminfo.SkuId, iteminfo.Quantity);
                        }
                        /**正式上线前屏蔽****/

                        ShoppingCartProcessor.ClearShoppingCart();

                        if (orderInfo.Gifts.Count > 0)
                        {
                            ShoppingCartProcessor.ClearGiftShoppingCart();
                            ShoppingProcessor.CutNeedPoint(shoppingCart.GetTotalNeedPoint(), orderInfo.OrderId);
                        }
                        //除去礼品兑换以外，创维及供应商商品订单都发送通知
                        if (orderInfo.OrderSource != 99)
                        {
                            //Messenger.OrderCreated(orderInfo, currentMember);通知购买者，这里不需要通知购买者
                            MemberInfo tzMember = null;
                            if (orderInfo.ReferralUserId > 0)
                            {
                                //通知门店
                                tzMember = MemberProcessor.GetMember(orderInfo.ReferralUserId);
                            }
                            else
                            {
                                //通知主店对应管理员
                                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                if (!string.IsNullOrEmpty(masterSettings.ManageOpenID))
                                {
                                    tzMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                                }
                            }
                            if (tzMember != null)
                                Messenger.OrderCreatedSendManage(orderInfo, tzMember);
                        }

                        builder.Append("\"Status\":\"OK\",");
                        builder.AppendFormat("\"OrderId\":\"{0}\"", orderInfo.OrderId);
                    }
                    else
                    {
                        builder.Append("\"Status\":\"Error\"");
                        builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", "订单创建失败！");
                    }
                }
                catch (OrderException exception)
                {
                    builder.Append("\"Status\":\"Error\"");
                    builder.AppendFormat(",\"ErrorMsg\":\"{0}\"", exception.Message);
                }
            }
            HiCache.Remove("DataCache-Member-" + currentMember.UserId);
            builder.Append("}");
            context.Response.ContentType = "application/json";
            context.Response.Write(builder.ToString());
        }


        public decimal DiscountMoney(OrderInfo order, out string ActivitiesId, out string ActivitiesName)
        {
            decimal num = new decimal(0);
            decimal num1 = new decimal(0);
            decimal num2 = new decimal(0);
            LineItemInfo lineItemInfo = new LineItemInfo();
            ActivitiesId = "";
            ActivitiesName = "";
            System.Data.DataTable type = ProductBrowser.GetType();
            for (int i = 0; i < type.Rows.Count; i++)
            {
                string str = "";
                string str1 = "";
                decimal subTotal = new decimal(0);
                foreach (KeyValuePair<string, LineItemInfo> lineItem in order.LineItems)
                {
                    lineItemInfo = lineItem.Value;
                    if (lineItemInfo.ProductId == 0 || string.IsNullOrEmpty(type.Rows[i]["ActivitiesType"].ToString()))
                    {
                        continue;
                    }
                    int activitiesproductid = int.Parse(type.Rows[i]["ActivitiesType"].ToString());
                    if (activitiesproductid == lineItemInfo.ProductId)
                    {
                        subTotal = subTotal + lineItemInfo.GetSubTotal();
                    }
                    //if (int.Parse(type.Rows[i]["ActivitiesType"].ToString()) != 0)
                    //{
                    //    continue;
                    //}
                    //subTotal = subTotal + lineItemInfo.GetSubTotal();

                    //if (string.IsNullOrEmpty(lineItemInfo.MainCategoryPath))
                    //{
                    //    continue;
                    //}
                    //int num3 = int.Parse(type.Rows[i]["ActivitiesType"].ToString());
                    //string mainCategoryPath = lineItemInfo.MainCategoryPath;
                    //char[] chrArray = new char[] { '|' };
                    //if (num3 == int.Parse(mainCategoryPath.Split(chrArray)[0].ToString()))
                    //{
                    //    subTotal = subTotal + lineItemInfo.GetSubTotal();
                    //}
                    //if (int.Parse(type.Rows[i]["ActivitiesType"].ToString()) != 0)
                    //{
                    //    continue;
                    //}
                    //subTotal = subTotal + lineItemInfo.GetSubTotal();
                }
                if (subTotal != new decimal(0))
                {
                    System.Data.DataTable allFull = ProductBrowser.GetAllFull(int.Parse(type.Rows[i]["ActivitiesType"].ToString()));
                    if (allFull.Rows.Count > 0)
                    {
                        int num4 = 0;
                        while (true)
                        {
                            if (num4 >= allFull.Rows.Count)
                            {
                                break;
                            }
                            else if (subTotal >= decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString()))
                            {
                                num1 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString());
                                num = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["ReductionMoney"].ToString());
                                str = string.Concat(allFull.Rows[allFull.Rows.Count - 1]["ActivitiesId"].ToString(), ",");
                                str1 = string.Concat(allFull.Rows[allFull.Rows.Count - 1]["ActivitiesName"].ToString(), ",");
                                break;
                            }
                            else if (subTotal <= decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString()))
                            {
                                num1 = decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString());
                                num = num + decimal.Parse(allFull.Rows[0]["ReductionMoney"].ToString());
                                str = string.Concat(allFull.Rows[0]["ActivitiesId"].ToString(), ",");
                                str1 = string.Concat(allFull.Rows[0]["ActivitiesName"].ToString(), ",");
                                break;
                            }
                            else
                            {
                                if (subTotal >= decimal.Parse(allFull.Rows[num4]["MeetMoney"].ToString()))
                                {
                                    num1 = decimal.Parse(allFull.Rows[num4]["MeetMoney"].ToString());
                                    num = decimal.Parse(allFull.Rows[num4]["ReductionMoney"].ToString());
                                    str = string.Concat(allFull.Rows[num4]["ActivitiesId"].ToString(), ",");
                                    str1 = string.Concat(allFull.Rows[num4]["ActivitiesName"].ToString(), ",");
                                }
                                num4++;
                            }
                        }
                        if (subTotal >= num1)
                        {
                            ActivitiesId = string.Concat(ActivitiesId, str);
                            ActivitiesName = string.Concat(ActivitiesName, str1);
                            num2 = num2 + num;
                            foreach (KeyValuePair<string, LineItemInfo> keyValuePair in order.LineItems)
                            {
                                LineItemInfo value = keyValuePair.Value;
                                if (string.IsNullOrEmpty(value.MainCategoryPath))
                                {
                                    continue;
                                }
                                int num5 = int.Parse(type.Rows[i]["ActivitiesType"].ToString());
                                string mainCategoryPath1 = value.MainCategoryPath;
                                char[] chrArray1 = new char[] { '|' };
                                if (num5 != int.Parse(mainCategoryPath1.Split(chrArray1)[0].ToString()) && int.Parse(type.Rows[i]["ActivitiesType"].ToString()) != 0)
                                {
                                    continue;
                                }
                                value.PromotionName = str1.Substring(0, str1.Length - 1);
                                value.PromotionId = int.Parse(str.Substring(0, str.Length - 1));
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(ActivitiesName))
            {
                ActivitiesName = ActivitiesName.Substring(0, ActivitiesName.Length - 1);
                ActivitiesId = ActivitiesId.Substring(0, ActivitiesId.Length - 1);
            }
            return num2;
        }


        public void SetOrderItemStatus(OrderInfo order)
        {
            System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
            LineItemInfo lineItemInfo = new LineItemInfo();
            foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
            {
                lineItemInfo = current.Value;
                lineItemInfo.OrderItemsStatus = OrderStatus.WaitBuyerPay;
            }
        }

        /// <summary>
        /// 计算订单明细中的一级返佣信息(存在分销商信息才计算)
        /// 分销商等级明细中的上浮百分比数值 + 商品分类明细中的佣金百分比数值
        /// wt计算返佣金额值
        /// </summary>
        public bool InsertCalculationCommission(string orderid, OrderInfo order)
        {
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(order.ReferralUserId);
            bool result = false;
            if (userIdDistributors != null)
            {
                //decimal d = 0m;//FirstCommissionRise 直接佣金上浮(百分比)
                //System.Data.DataView defaultView = DistributorGradeBrower.GetAllDistributorGrade().DefaultView; //当前系统中所有分销商等级
                //#region 直接佣金上浮，这里取门店等级佣金上浮值
                //if (userIdDistributors.DistriGradeId.ToString() != "0")
                //{
                //    defaultView.RowFilter = " GradeId=" + userIdDistributors.DistriGradeId;
                //    d = decimal.Parse(defaultView[0]["FirstCommissionRise"].ToString());
                //}
                //#endregion 直接佣金上浮   

                #region 直接佣金上浮，这里取门店星级佣金上浮值
                decimal ratio = 0m;
                decimal money = 0m;
                if (userIdDistributors.StarLevelID != Guid.Empty)
                {
                    DistributorStarLevelInfo starLevalInfo = DistributorStarLevelHelper.GetDistributorStarLevelInfo(userIdDistributors.StarLevelID.ToString());
                    if (starLevalInfo != null && starLevalInfo.CommissionType > 0)
                    {
                        if (starLevalInfo.CommissionType == 1)
                            ratio = starLevalInfo.CommissionRise;
                        else
                            money = starLevalInfo.CommissionMoney;
                    }
                }
                #endregion

                #region 是否利润计算
                bool isProfit = false;
                //根据配置文件来处理(按销售价或利润计算返佣)
                if (SettingsManager.GetMasterSettings(false).EnableProfit)
                {
                    isProfit = true;
                }
                #endregion

                System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
                LineItemInfo lineItemInfo = new LineItemInfo();
                foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
                {
                    #region 计算订单明细中的一级返佣信息
                    lineItemInfo = current.Value;

                    /**新佣金算法**/
                    System.Collections.Generic.IList<int> tagsId = null;
                    System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                    ProductInfo product = ProductHelper.GetProductDetails(lineItemInfo.ProductId, out dictionary, out tagsId);
                    if (product != null && product.ProductId > 0)
                    {
                        decimal commision = 0;//佣金值变量
                        if (order.buyType == 4)
                        {
                            //内购价
                            if (product.NeigouCommisionType == 0)
                            {
                                if (product.NeigouCommisionRatio > 0)
                                {
                                    commision = product.NeigouCommisionRatio + ratio;
                                    if (!isProfit)
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotal();
                                    else
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotalProfit();
                                }
                            }
                            else
                            {
                                if (product.NeigouCommisionMoney > 0 && lineItemInfo.GetSubTotal() >= product.NeigouCommisionMoney)
                                {
                                    commision = product.NeigouCommisionMoney + money;
                                    //如果佣金金额大于利润，则直接取利润值
                                    if (lineItemInfo.GetSubTotalProfit() >= commision)
                                        lineItemInfo.ItemsCommission = commision;
                                    else
                                        lineItemInfo.ItemsCommission = lineItemInfo.GetSubTotalProfit();
                                }
                            }
                        }
                        else if (order.buyType == 5)
                        {
                            if (product.wholesaleCommisionType == 0)
                            {
                                if (product.wholesaleCommisionRatio > 0)
                                {
                                    commision = product.wholesaleCommisionRatio + ratio;
                                    if (!isProfit)
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotal();
                                    else
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotalProfit();
                                }
                            }
                            else
                            {
                                if (product.wholesaleCommisionMoney > 0 && lineItemInfo.GetSubTotal() >= product.wholesaleCommisionMoney)
                                {
                                    commision = product.wholesaleCommisionMoney + money;
                                    //如果佣金金额大于利润，则直接取利润值
                                    if (lineItemInfo.GetSubTotalProfit() >= commision)
                                        lineItemInfo.ItemsCommission = commision;
                                    else
                                        lineItemInfo.ItemsCommission = lineItemInfo.GetSubTotalProfit();
                                }
                            }
                        }
                        else
                        {
                            //一口价
                            if (product.SaleCommisionType == 0)
                            {
                                if (product.SaleCommisionRatio > 0)
                                {
                                    commision = product.SaleCommisionRatio + ratio;
                                    if (!isProfit)
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotal();
                                    else
                                        lineItemInfo.ItemsCommission = commision / 100m * lineItemInfo.GetSubTotalProfit();
                                }
                            }
                            else
                            {
                                if (product.SaleCommisionMoney > 0 && lineItemInfo.GetSubTotal() >= product.SaleCommisionMoney)
                                {
                                    commision = product.SaleCommisionMoney + money;
                                    //如果佣金金额大于利润，则直接取利润值
                                    if (lineItemInfo.GetSubTotalProfit() >= commision)
                                        lineItemInfo.ItemsCommission = commision;
                                    else
                                        lineItemInfo.ItemsCommission = lineItemInfo.GetSubTotalProfit();
                                }
                            }
                        }
                    }
                    //屏蔽原始佣金算法
                    //System.Data.DataView defaultView2 = CategoryBrowser.GetAllCategories().DefaultView;     //当前系统中所有商品分类
                    //System.Data.DataTable productCategories = ProductBrowser.GetProductCategories(lineItemInfo.ProductId);  //获取当前商品分类
                    //if (productCategories.Rows.Count > 0 && productCategories.Rows[0][0].ToString() != "0")
                    //{
                    //    defaultView2.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
                    //    string text = defaultView2[0]["FirstCommission"].ToString();    //直接销售佣金(百分比)
                    //    if (!string.IsNullOrEmpty(text))
                    //    {
                    //        //根据配置文件来处理(按销售价或利润计算返佣)
                    //        if (!SettingsManager.GetMasterSettings(false).EnableProfit)
                    //        {
                    //            //计算订单明细中的返佣金额: (佣金上浮+销售佣金)/100.00*实际单价*数量
                    //            lineItemInfo.ItemsCommission = (decimal.Parse(text) + d) / 100m * lineItemInfo.GetSubTotal();
                    //        }
                    //        else
                    //        {
                    //            //计算订单明细中的返佣金额: (佣金上浮+销售佣金)/100.00*实际利润*数量
                    //            lineItemInfo.ItemsCommission = (decimal.Parse(text) + d) / 100m * lineItemInfo.GetSubTotalProfit();
                    //        }
                    //    }
                    //}
                    //屏蔽原始佣金算法

                    #endregion 计算订单明细中的一级返佣信息
                }
            }
            return result;
        }


        public void RegisterUser(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string username = context.Request["userName"];
            string sourceData = context.Request["password"];
            string str3 = context.Request["passagain"];
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (sourceData == str3)
            {
                MemberInfo info = new MemberInfo();
                if (MemberProcessor.GetusernameMember(username) == null)
                {
                    MemberInfo member = new MemberInfo();
                    string generateId = Globals.GetGenerateId();
                    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    member.UserName = username;
                    member.CreateDate = System.DateTime.Now;
                    member.SessionId = generateId;
                    member.SessionEndTime = System.DateTime.Now.AddYears(10);
                    member.Password = HiCryptographer.Md5Encrypt(sourceData);
                    MemberProcessor.CreateMember(member);
                    MemberInfo info2 = MemberProcessor.GetMember(generateId);
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
                    if (System.Web.HttpContext.Current.Request.Cookies[cookieStr] != null)
                    {
                        System.Web.HttpContext.Current.Response.Cookies[cookieStr].Expires = System.DateTime.Now.AddDays(-1.0);
                        System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                        {
                            Value = info2.UserId.ToString(),
                            Expires = System.DateTime.Now.AddYears(10)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(cookieStr)
                        {
                            Value = info2.UserId.ToString(),
                            Expires = System.DateTime.Now.AddYears(10)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie2);
                    }
                    context.Session["userid"] = info2.UserId.ToString();
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-1\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"-2\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }
        private void SearchExpressData(System.Web.HttpContext context)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    s = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber, 0);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(s);
            context.Response.End();
        }
        private void SetDefaultShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int userId = currentMember.UserId;
                if (MemberProcessor.SetDefaultShippingAddress(System.Convert.ToInt32(context.Request.Form["shippingid"]), userId))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }
        public void SetDistributorMsg(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.MicroSignal = context.Request["MicroSignal"];
            currentMember.RealName = context.Request["RealName"];
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }
        public void SetUserName(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            currentMember.UserName = context.Request["userName"];
            currentMember.VipCardDate = new System.DateTime?(System.DateTime.Now);
            currentMember.CellPhone = context.Request["CellPhone"];
            currentMember.QQ = context.Request["QQ"];
            if (!string.IsNullOrEmpty(currentMember.QQ))
            {
                currentMember.Email = currentMember.QQ + "@qq.com";
            }
            currentMember.RealName = context.Request["RealName"];
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (MemberProcessor.UpdateMember(currentMember))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }
        private void SubmitActivity(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ActivityInfo activity = VshopBrowser.GetActivity(System.Convert.ToInt32(context.Request.Form.Get("id")));
                if (System.DateTime.Now < activity.StartDate || System.DateTime.Now > activity.EndDate)
                {
                    context.Response.Write("{\"success\":false, \"msg\":\"报名还未开始或已结束\"}");
                }
                else
                {
                    ActivitySignUpInfo info = new ActivitySignUpInfo
                    {
                        ActivityId = System.Convert.ToInt32(context.Request.Form.Get("id")),
                        Item1 = context.Request.Form.Get("item1"),
                        Item2 = context.Request.Form.Get("item2"),
                        Item3 = context.Request.Form.Get("item3"),
                        Item4 = context.Request.Form.Get("item4"),
                        Item5 = context.Request.Form.Get("item5"),
                        RealName = currentMember.RealName,
                        SignUpDate = System.DateTime.Now,
                        UserId = currentMember.UserId,
                        UserName = currentMember.UserName
                    };
                    string s = string.IsNullOrEmpty(VshopBrowser.SaveActivitySignUp(info)) ? "{\"success\":true}" : "{\"success\":false, \"msg\":\"你已经报过名了,请勿重复报名\"}";
                    context.Response.Write(s);
                }
            }
        }
        private void SubmitWinnerInfo(System.Web.HttpContext context)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                int activityId = System.Convert.ToInt32(context.Request.Form.Get("id"));
                string realName = context.Request.Form.Get("name");
                string cellPhone = context.Request.Form.Get("phone");
                string s = VshopBrowser.UpdatePrizeRecord(activityId, currentMember.UserId, realName, cellPhone) ? "{\"success\":true}" : "{\"success\":false}";
                context.Response.ContentType = "application/json";
                context.Response.Write(s);
            }
        }

        private void UpdateDistributor(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            StringBuilder stringBuilder = new StringBuilder();
            if (!this.CheckUpdateDistributors(httpContext, stringBuilder))
            {
                httpContext.Response.Write(string.Concat("{\"success\":false,\"msg\":\"", stringBuilder.ToString(), "\"}"));
                return;
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
            currentDistributors.StoreName = httpContext.Request["stroename"].Trim();
            currentDistributors.StoreDescription = httpContext.Request["descriptions"].Trim();
            currentDistributors.BackImage = httpContext.Request["backimg"].Trim();
            currentDistributors.RequestAccount = httpContext.Request["accountname"].Trim();
            currentDistributors.Logo = httpContext.Request["logo"].Trim();
            if (DistributorsBrower.UpdateDistributorMessage(currentDistributors))
            {
                httpContext.Response.Write("{\"success\":true}");
                return;
            }
            httpContext.Response.Write("{\"success\":false,\"msg\":\"店铺名称已存在，请重新命名!\"}");
        }

        private void UpdateShippingAddress(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                context.Response.Write("{\"success\":false}");
            }
            else
            {
                ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                {
                    Address = context.Request.Form["address"],
                    CellPhone = context.Request.Form["cellphone"],
                    ShipTo = context.Request.Form["shipTo"],
                    TelPhone = context.Request.Form["telphone"],
                    Zipcode = "12345",
                    UserId = currentMember.UserId,
                    ShippingId = System.Convert.ToInt32(context.Request.Form["shippingid"]),
                    RegionId = System.Convert.ToInt32(context.Request.Form["regionSelectorValue"])
                };
                if (MemberProcessor.UpdateShippingAddress(shippingAddress))
                {
                    context.Response.Write("{\"success\":true}");
                }
                else
                {
                    context.Response.Write("{\"success\":false}");
                }
            }
        }
        private string UploadFileImages(System.Web.HttpContext context, System.Web.HttpPostedFile file)
        {
            string virtualPath = string.Empty;
            string result;
            if (file != null && !string.IsNullOrEmpty(file.FileName))
            {
                string str2 = Globals.GetStoragePath() + "/Logo";
                string str3 = System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + System.IO.Path.GetExtension(file.FileName);
                virtualPath = str2 + "/" + str3;
                string str4 = System.IO.Path.GetExtension(file.FileName).ToLower();
                if (!str4.Equals(".gif") && !str4.Equals(".jpg") && !str4.Equals(".png") && !str4.Equals(".bmp"))
                {
                    context.Response.Write("你上传的文件格式不正确！上传格式有(.gif、.jpg、.png、.bmp)");
                    context.Response.End();
                }
                if (file.ContentLength > 1048576)
                {
                    context.Response.Write("你上传的文件不能大于1048576KB!请重新上传！");
                    context.Response.End();
                }
                file.SaveAs(context.Request.MapPath(virtualPath));
                result = virtualPath;
            }
            else
            {
                context.Response.Write("图片上传失败!");
                context.Response.End();
                result = virtualPath;
            }
            return result;
        }
        public void UserLogin(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo usernameMember = new MemberInfo();
            string username = context.Request["userName"];
            string sourceData = context.Request["password"];
            usernameMember = MemberProcessor.GetusernameMember(username);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (usernameMember == null)
            {
                builder.Append("\"Status\":\"-1\"");
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                if (usernameMember.Password == HiCryptographer.Md5Encrypt(sourceData))
                {
                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(usernameMember.UserId);
                    if (userIdDistributors != null && userIdDistributors.UserId > 0)
                    {
                        System.Web.HttpContext.Current.Response.Cookies["Vshop-ReferralId"].Expires = System.DateTime.Now.AddDays(-1.0);
                        System.Web.HttpCookie cookie = new System.Web.HttpCookie("Vshop-ReferralId")
                        {
                            Value = Globals.UrlEncode(userIdDistributors.UserId.ToString()),
                            Expires = System.DateTime.Now.AddYears(1)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    }
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
                    if (System.Web.HttpContext.Current.Request.Cookies[cookieStr] != null)
                    {
                        System.Web.HttpContext.Current.Response.Cookies[cookieStr].Expires = System.DateTime.Now.AddDays(-1.0);
                        System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(cookieStr)
                        {
                            Value = usernameMember.UserId.ToString(),
                            Expires = System.DateTime.Now.AddYears(10)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie2);
                    }
                    else
                    {
                        System.Web.HttpCookie cookie3 = new System.Web.HttpCookie(cookieStr)
                        {
                            Value = Globals.UrlEncode(usernameMember.UserId.ToString()),
                            Expires = System.DateTime.Now.AddYears(1)
                        };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie3);
                    }
                    context.Session["userid"] = usernameMember.UserId.ToString();
                    builder.Append("\"Status\":\"OK\"");
                }
                else
                {
                    builder.Append("\"Status\":\"-2\"");
                }
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
        }
        private void Vote(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 1;
            int.TryParse(context.Request["voteId"], out result);
            string itemIds = context.Request["itemIds"];
            itemIds = itemIds.Remove(itemIds.Length - 1);
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = System.DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = System.DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                {
                    Value = member.UserId.ToString(),
                    Expires = System.DateTime.Now.AddYears(10)
                };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            if (VshopBrowser.Vote(result, itemIds))
            {
                builder.Append("\"Status\":\"OK\"");
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 退货，退款请求
        /// </summary>
        /// <param name="context"></param>
        public void RequestReturn(HttpContext context)
        {

            context.Response.ContentType = "application/json";

            Hidistro.Entities.RefundInfo refundInfo = new Hidistro.Entities.RefundInfo();

            refundInfo.OrderId = context.Request["orderid"];
            refundInfo.ApplyForTime = DateTime.Now;
            refundInfo.RefundRemark = context.Request["Reason"];
            refundInfo.HandleStatus = Hidistro.Entities.RefundInfo.Handlestatus.NoneAudit;
            refundInfo.Account = context.Request["Account"];
            refundInfo.RefundMoney = decimal.Parse(context.Request["Money"]);
            refundInfo.ProductId = int.Parse(context.Request["productid"]);

            StringBuilder stringBuilder = new StringBuilder();

            OrderInfo orderinfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);


            refundInfo.UserId = MemberProcessor.GetCurrentMember().UserId;

            int num = 7;

            refundInfo.RefundType = 1;

            if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
            {
                num = 6;
                refundInfo.HandleStatus = Hidistro.Entities.RefundInfo.Handlestatus.NoRefund;
                refundInfo.RefundType = 2;
                refundInfo.AuditTime = DateTime.Now.ToString();
            }
            stringBuilder.Append("{");
            //if (string.IsNullOrEmpty(refundInfo.Account.Trim()))
            //{
            //    stringBuilder.Append("\"Status\":\"Mesg\"");
            //}else
            if (ShoppingProcessor.GetReturnMes(refundInfo.UserId, refundInfo.OrderId, refundInfo.ProductId, (int)refundInfo.HandleStatus))
            {
                stringBuilder.Append("\"Status\":\"Repeat\"");
            }
            else if (!ShoppingProcessor.InsertOrderRefund(refundInfo))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
            }
            else if (!ShoppingProcessor.UpdateOrderGoodStatu(refundInfo.OrderId, context.Request["skuid"], num))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"OK\"");

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (refundInfo.RefundType == 2)
                {
                    //退款
                    if (orderinfo.ReferralUserId > 0)
                    {
                        //通知门店
                        MemberInfo member = MemberProcessor.GetMember(orderinfo.ReferralUserId);
                        if (member != null)
                        {
                            Messenger.OrderReturnSendManage(orderinfo, member);
                        }
                    }
                    else
                    {
                        //通知主店
                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                        if (openIdMember != null)
                        {
                            Messenger.OrderReturnSendManage(orderinfo, openIdMember);
                        }
                    }
                }
                else
                {
                    //退货
                    if (orderinfo.ReferralUserId > 0)
                    {
                        //通知门店
                        MemberInfo member = MemberProcessor.GetMember(orderinfo.ReferralUserId);
                        if (member != null)
                        {
                            Messenger.OrderReturnGoodsSendManage(orderinfo, member);
                        }
                    }
                    else
                    {
                        //通知主店
                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(masterSettings.ManageOpenID);
                        if (openIdMember != null)
                        {
                            Messenger.OrderReturnGoodsSendManage(orderinfo, openIdMember);
                        }
                    }
                }
            }
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="context"></param>
        public void EditPassword(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string item = context.Request["oldPwd"];
            string str = context.Request["password"];
            string item1 = context.Request["passagain"];
            MemberInfo memberInfo = new MemberInfo();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[cookieStr];
            if (httpCookie == null)
            {
                context.Response.Write("{\"Status\":\"-1\"}");
                return;
            }
            int num = int.Parse(httpCookie.Value);
            memberInfo = MemberProcessor.GetMember(num);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            if (memberInfo.Password != HiCryptographer.Md5Encrypt(item))
            {
                stringBuilder.Append("\"Status\":\"-4\"");
            }
            else if (str != item1)
            {
                stringBuilder.Append("\"Status\":\"-2\"");
            }
            else if (!MemberProcessor.SetPwd(num.ToString(), HiCryptographer.Md5Encrypt(str)))
            {
                stringBuilder.Append("\"Status\":\"-3\"");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"OK\"");
            }
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        public void GetOrderRedPager(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string item = context.Request["orderid"];
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[cookieStr];
            if (httpCookie == null)
            {
                context.Response.Write("{\"status\":\"-1\",\"tips\":\"用户未登录！\"}");
                return;
            }
            int num = int.Parse(httpCookie.Value);
            OrderRedPagerInfo orderRedPagerInfo = OrderRedPagerBrower.GetOrderRedPagerInfo(item);
            if (orderRedPagerInfo == null)
            {
                context.Response.Write("{\"status\":\"-2\",\"tips\":\"订单红包不存在！\"}");
                return;
            }
            int maxGetTimes = orderRedPagerInfo.MaxGetTimes;
            if (orderRedPagerInfo.AlreadyGetTimes >= maxGetTimes)
            {
                context.Response.Write("{\"status\":\"-3\",\"tips\":\"订单红包已经领完了！\"}");
                return;
            }
            int num1 = 1;
            int num2 = (int)Math.Floor(orderRedPagerInfo.ItemAmountLimit);
            if (num2 < 2)
            {
                num2 = num1 + 1;
            }

            int num3 = (new Random()).Next(num1, num2);

            DateTime now = DateTime.Now;

            UserRedPagerInfo userRedPagerInfo = new UserRedPagerInfo();

            userRedPagerInfo.Amount = decimal.Parse(num3.ToString());
            userRedPagerInfo.UserID = num;
            userRedPagerInfo.OrderID = orderRedPagerInfo.OrderID;
            userRedPagerInfo.RedPagerActivityName = orderRedPagerInfo.RedPagerActivityName;
            userRedPagerInfo.OrderAmountCanUse = orderRedPagerInfo.OrderAmountCanUse;
            userRedPagerInfo.CreateTime = now;
            userRedPagerInfo.ExpiryTime = now.AddDays((double)orderRedPagerInfo.ExpiryDays);
            userRedPagerInfo.IsUsed = false;

            string str = UserRedPagerBrower.CreateUserRedPager(userRedPagerInfo);
            if (str == "-1")
            {
                context.Response.Write(string.Concat("{\"status\":\"-5\",\"tips\":\"", num.ToString(), "\"}"));
                return;
            }
            if (str != "1")
            {
                context.Response.Write("{\"status\":\"-4\",\"tips\":\"红包领取失败！\"}");
                return;
            }
            context.Response.Write(string.Concat("{\"status\":\"0\",\"tips\":\"", num.ToString(), "\"}"));
        }


        private bool CheckAdjustCommssions(HttpContext httpContext, ref string textRef1)
        {
            if (string.IsNullOrEmpty(httpContext.Request["orderId"]))
            {
                textRef1 = "{\"success\":false,\"msg\":\"订单号不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(httpContext.Request["skuId"]))
            {
                textRef1 = "{\"success\":false,\"msg\":\"商品规格不允许为空！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(httpContext.Request["adjustprice"]))
            {
                textRef1 = "{\"success\":false,\"msg\":\"请输入要调整的价格！\"}";
                return false;
            }
            if (string.IsNullOrEmpty(httpContext.Request["commssionprice"]))
            {
                textRef1 = "{\"success\":false,\"msg\":\"佣金金额值不对！\"}";
                return false;
            }
            if ((Convert.ToDecimal(httpContext.Request["adjustprice"]) >= 0M) && (Convert.ToDecimal(httpContext.Request["ajustprice"]) <= Convert.ToDecimal(httpContext.Request["commssionprice"])))
            {
                return true;
            }
            textRef1 = "{\"success\":false,\"msg\":\"输入金额必须在0~" + httpContext.Request["commssionprice"].ToString() + "之间！\"}";
            return false;
        }

        private void AdjustCommssions(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            string s = "";
            if (this.CheckAdjustCommssions(httpContext, ref s))
            {
                decimal result = 0M;
                decimal num2 = 0M;
                decimal.TryParse(httpContext.Request["adjustprice"], out result);
                decimal.TryParse(httpContext.Request["commssionprice"], out num2);
                if (ShoppingProcessor.UpdateAdjustCommssions(httpContext.Request["orderId"], httpContext.Request["skuId"], num2, result))
                {
                    s = "{\"success\":true,\"msg\":\"修改金额成功！\"}";
                }
                else
                {
                    s = "{\"success\":false,\"msg\":\"优惠金额修改失败！\"}";
                }
            }
            httpContext.Response.Write(s);
            httpContext.Response.End();
        }
        //获得订单未付款提示配置
        private void OrderRemind(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = "{\"EnableOrderRemind\":\"" + masterSettings.EnableOrderRemind + "\",";
            str += "\"OrderRemindTime\":\"" + masterSettings.OrderRemindTime + "\"}";
            httpContext.Response.Write(str);
            httpContext.Response.End();
        }

        public void WriteJson(HttpContext context, int status = 0)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            if (this.jsondict.Count > 0)
            {
                int num = 0;
                foreach (string str in this.jsondict.Keys)
                {
                    if (num == 0)
                    {
                        builder.AppendFormat("\"{0}\":\"{1}\"", str, this.jsondict[str]);
                    }
                    else
                    {
                        builder.AppendFormat(",\"{0}\":\"{1}\"", str, this.jsondict[str]);
                    }
                    num++;
                }
            }
            else
            {
                builder.AppendFormat("\"{0}\":\"{1}\"", "status", status);
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
            context.Response.End();
        }

        //是否弹出订单提示
        public void IsOrderRemind(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/json";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string t = masterSettings.OrderRemindTime;
            int num = 0;
            DateTime dateTime = DateTime.Now;
            string str = "{\"success\":\"False\"}";
            string[] time = t.Split(',');
            bool f = false;
            if (System.Web.HttpContext.Current.Request.Cookies["OrderShowNum"] != null)
            {
                try
                {
                    string[] nt = System.Web.HttpContext.Current.Request.Cookies.Get("OrderShowNum").Value.Split(',');
                    num = int.Parse(nt[0]);
                    //dateTime = DateTime.Parse(nt[1]);
                    dateTime = Convert.ToDateTime(nt[1]);
                }
                catch { };
            }
            else
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("OrderShowNum")
                {
                    Value = "0," + DateTime.Now,
                    Expires = System.DateTime.Now.AddHours(2)
                };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (num < time.Length)
            {
                if (num == 0)
                {
                    if (ShoppingProcessor.IsOrderRemind(Globals.GetCurrentMemberUserId(), Convert.ToInt32(time[num])))
                    {
                        num += 1;
                        HttpCookie cok = HttpContext.Current.Request.Cookies["OrderShowNum"];
                        cok.Value = num + "," + DateTime.Now;
                        HttpContext.Current.Response.AppendCookie(cok);
                        str = "{\"success\":\"True\"}";
                    }
                }
                else
                {
                    if (DateTime.Now >= dateTime.AddMinutes(int.Parse(time[num]) - int.Parse(time[num - 1])))
                    {
                        if (ShoppingProcessor.IsOrderRemind(Globals.GetCurrentMemberUserId(), Convert.ToInt32(time[num])))
                        {
                            num += 1;
                            HttpCookie cok = HttpContext.Current.Request.Cookies["OrderShowNum"];
                            cok.Value = num + "," + DateTime.Now;
                            HttpContext.Current.Response.AppendCookie(cok);
                            str = "{\"success\":\"True\"}";
                        }
                    }
                }
            }
            httpContext.Response.Write(str);
            httpContext.Response.End();
        }
    }
}
