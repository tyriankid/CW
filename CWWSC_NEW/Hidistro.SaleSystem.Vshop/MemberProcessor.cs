namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Messages;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Linq;
    using Hidistro.Core.Entities;

    public static class MemberProcessor
    {
        public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            ShippingAddressDao dao = new ShippingAddressDao();
            int shippingId = dao.AddShippingAddress(shippingAddress);
            if (dao.SetDefaultShippingAddress(shippingId, Globals.GetCurrentMemberUserId()))
            {
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 增加代理商的收货地址
        /// </summary>
        /// <param name="shippingAddress"></param>
        /// <returns></returns>
        public static int AddAgentShippingAddress(ShippingAddressInfo shippingAddress)
        {
            ShippingAddressDao dao = new ShippingAddressDao();
            int shippingId = dao.AddShippingAddress(shippingAddress);
            if (dao.SetDefaultShippingAddress(shippingId, shippingAddress.UserId))
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = new DateTime?(DateTime.Now);
                flag = new OrderDao().UpdateOrder(order, null);
                HiCache.Remove(string.Format("DataCache-Member-{0}", order.UserId));
            }
            return flag;
        }
        
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool ConfirmOrderTakeGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                order.OrderStatus = OrderStatus.ConfirmTakeGoods;//将订单状态设置为用户确认收货
                order.TakeGoodsDate = new DateTime?(DateTime.Now);//确认收货时间
                flag = new OrderDao().UpdateOrder(order, null);
                HiCache.Remove(string.Format("DataCache-Member-{0}", order.UserId));
            }
            return flag;
        }

        public static bool CreateMember(MemberInfo member)
        {
            MemberDao dao = new MemberDao();
            if (dao.GetOpenIdMember(member.OpenId) != null)
                return true;
            else
                return dao.CreateMember(member);
        }

        public static MemberInfo GetAnonymousMember(string adminType = "normal")
        {
            /*
            MemberInfo anonymousMember =GetMemberByUserName("[匿名用户]");
            if (anonymousMember != null && anonymousMember.OpenId == null)//如果查得到匿名用户,则返回
            {
                return anonymousMember;
            }
            else//否则增加一个匿名用户
            {
                anonymousMember = new MemberInfo
                {
                    UserName="[匿名用户]",
                    RealName="[匿名用户]",
                    Password="yihuikeji888",
                };
                if (CreateMember(anonymousMember))
                {
                    anonymousMember = GetMemberByUserName("[匿名用户]");
                }
            }
            return anonymousMember;
             */

            MemberInfo anonymousMember = new MemberInfo();
            if (adminType == "admin")
            {
                anonymousMember = MemberProcessor.GetusernameMember("[堂食用户]");
                if (anonymousMember == null)//如果没有匿名用户,新建一个
                {
                    MemberInfo member = new MemberInfo();
                    string generateId = Globals.GetGenerateId();
                    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    member.UserName = "[堂食用户]";
                    member.RealName = "[堂食用户]";
                    member.CreateDate = System.DateTime.Now;
                    member.SessionId = generateId;
                    member.SessionEndTime = System.DateTime.Now.AddYears(10);
                    member.Password = HiCryptographer.Md5Encrypt("yihuikeji888");
                    MemberProcessor.CreateMember(member);
                    anonymousMember = MemberProcessor.GetMember(generateId);
                }
            }
            else if (adminType == "activity")
            {
                anonymousMember = MemberProcessor.GetusernameMember("[活动用户]");
                if (anonymousMember == null)//如果没有活动用户,新建一个
                {
                    MemberInfo member = new MemberInfo();
                    string generateId = Globals.GetGenerateId();
                    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    member.UserName = "[活动用户]";
                    member.RealName = "[活动用户]";
                    member.CreateDate = System.DateTime.Now;
                    member.SessionId = generateId;
                    member.SessionEndTime = System.DateTime.Now.AddYears(10);
                    member.Password = HiCryptographer.Md5Encrypt("yihuikeji888");
                    MemberProcessor.CreateMember(member);
                    anonymousMember = MemberProcessor.GetMember(generateId);
                }
            }

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string cookieStr = masterSettings.VshopMemberCookieStr <= 0 ? "Vshop-Member" : "Vshop-Member" + masterSettings.VshopMemberCookieStr.ToString();
            if (System.Web.HttpContext.Current.Request.Cookies[cookieStr] != null)
            {
                System.Web.HttpContext.Current.Response.Cookies[cookieStr].Expires = System.DateTime.Now.AddDays(-1.0);
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(cookieStr)
                {
                    Value = anonymousMember.UserId.ToString(),
                    Expires = System.DateTime.Now.AddYears(10)
                };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(cookieStr)
                {
                    Value = anonymousMember.UserId.ToString(),
                    Expires = System.DateTime.Now.AddYears(10)
                };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie2);
            }
            return anonymousMember;
        }

        public static bool Delete(int userId)
        {
            bool flag = new MemberDao().Delete(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
            }
            return flag;
        }

        public static bool DelShippingAddress(int shippingid, int userid)
        {
            return new ShippingAddressDao().DelShippingAddress(shippingid, userid);
        }

        public static MemberInfo GetCurrentMember()
        {
            return GetMember(Globals.GetCurrentMemberUserId());
        }

        public static int GetDefaultMemberGrade()
        {
            return new MemberGradeDao().GetDefaultMemberGrade();
        }

        public static ShippingAddressInfo GetDefaultShippingAddress()
        {
            IList<ShippingAddressInfo> shippingAddresses = new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId());
            foreach (ShippingAddressInfo info in shippingAddresses)
            {
                if (info.IsDefault)
                {
                    return info;
                }
            }
            return null;
        }

        public static MemberInfo GetMember()
        {
            return GetMember(Globals.GetCurrentMemberUserId());
        }

        public static MemberInfo GetMember(int userId)
        {
            /**
            MemberInfo member = HiCache.Get(string.Format("DataCache-Member-{0}", userId)) as MemberInfo;
            if (member == null)
            {
                member = new MemberDao().GetMember(userId);
                HiCache.Insert(string.Format("DataCache-Member-{0}", userId), member, 360, CacheItemPriority.Normal);
            }**/
            MemberInfo member = new MemberDao().GetMember(userId);
            return member;
        }

        public static MemberInfo GetMember(string sessionId)
        {
            return new MemberDao().GetMember(sessionId);
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return new MemberGradeDao().GetMemberGrade(gradeId);
        }

        public static MemberInfo GetOpenIdMember(string OpenId)
        {
            MemberDao dao = new MemberDao();
            return dao.GetOpenIdMember(OpenId);
        }

        public static ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            return new ShippingAddressDao().GetShippingAddress(shippingId, Globals.GetCurrentMemberUserId());
        }

        public static int GetShippingAddressCount()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId()).Count;
        }

        public static IList<ShippingAddressInfo> GetShippingAddresses()
        {
            return new ShippingAddressDao().GetShippingAddresses(Globals.GetCurrentMemberUserId());
        }

        /// <summary>
        /// 根据前台用户得到送货地址
        /// </summary>
        /// <param name="userId">前台用户Id</param>
        /// <returns></returns>
        public static IList<ShippingAddressInfo> GetShippingAddresses(int userId)
        {
            return new ShippingAddressDao().GetShippingAddresses(userId);
        }

        /// <summary>
        /// 根据前台用户得到送货地址
        /// </summary>
        /// <param name="shippingId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ShippingAddressInfo GetShippingAddress(int shippingId, int userId)
        {
            return new ShippingAddressDao().GetShippingAddress(shippingId, userId);
        }


        public static DataTable GetUserCoupons(int userId, int useType = 0)
        {
            return new CouponDao().GetUserCoupons(userId, useType);
        }

        public static int GetUserHistoryPoint(int userId)
        {
            return new PointDetailDao().GetHistoryPoint(userId);
        }

        public static MemberInfo GetusernameMember(string username)
        {
            return new MemberDao().GetusernameMember(username);
        }

        public static DataSet GetUserOrder(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrder(userId, query);
        }

        /// <summary>
        /// 2017-07-28，服务商品流程修改，  用于前端服务人员查看订单
        /// </summary>
        /// <param name="salesuserId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetUserServiceOrder(Guid salesuserId, OrderQuery query)
        {
            return new OrderDao().GetUserServiceOrder(salesuserId, query);
        }

        public static int GetUserOrderCount(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrderCount(userId, query);
        }

        public static DataSet GetUserOrderReturn(int userId, OrderQuery query)
        {
            return new OrderDao().GetUserOrderReturn(userId, query);
        }

        public static int GetUserOrderReturnCount(int userId)
        {
            return new OrderDao().GetUserOrderReturnCount(userId);
        }

        public static bool SetDefaultShippingAddress(int shippingId, int UserId)
        {
            return new ShippingAddressDao().SetDefaultShippingAddress(shippingId, UserId);
        }

        public static bool SetMemberSessionId(MemberInfo member)
        {
            MemberDao dao = new MemberDao();
            return dao.SetMemberSessionId(member.SessionId, member.SessionEndTime, member.OpenId);
        }

        public static bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
        {
            return new MemberDao().SetMemberSessionId(sessionId, sessionEndTime, openId);
        }

        /// <summary>
        /// 修改用户所属门店强制关系
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="distributorUserId"></param>
        /// <returns></returns>
        public static bool SetUserDistributorUserId(int userid, int distributorUserId)
        {
            HiCache.Remove(string.Format("DataCache-Member-{0}", userid));
            return new MemberDao().SetUserDistributorUserId(userid, distributorUserId);
        }

        public static bool SetPwd(string userid, string pwd)
        {
            return new MemberDao().SetPwd(userid, pwd);
        }

        public static bool UpdateMember(MemberInfo member)
        {
            HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
            return new MemberDao().Update(member);
        }

        public static bool UpdateOpenid(MemberInfo member)
        {
            return new MemberDao().UpdateOpenid(member);
        }

        public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            return new ShippingAddressDao().UpdateShippingAddress(shippingAddress);
        }
        /// <summary>
        /// 微信支付成功
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool UserPayOrder(OrderInfo order)
        {
            OrderDao dao = new OrderDao();
            order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
            order.PayDate = new DateTime?(DateTime.Now);
            bool flag = dao.UpdateOrder(order, null);
            string str = "";
            if (flag)
            {
                dao.UpdatePayOrderStock(order.OrderId);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    str = str + "'" + info.SkuId + "',";
                    ProductInfo productDetails = dao2.GetProductDetails(info.ProductId);
                    productDetails.SaleCounts += info.Quantity;
                    productDetails.ShowSaleCounts += info.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dao.UpdateItemsStatus(order.OrderId, 2, str.Substring(0, str.Length - 1));
                }
                if (!string.IsNullOrEmpty(order.ActivitiesId))
                {
                    new ActivitiesDao().UpdateActivitiesTakeEffect(order.ActivitiesId);
                }
                MemberInfo member = GetMember(order.UserId);
                if (member == null)
                {
                    return flag;
                }
                MemberDao dao4 = new MemberDao();
                PointDetailInfo point = new PointDetailInfo
                {
                    OrderId = order.OrderId,
                    UserId = member.UserId,
                    TradeDate = DateTime.Now,
                    TradeType = PointTradeType.Bounty,
                    Increased = new int?(order.Points),
                    Points = order.Points + member.Points
                };
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                PointDetailDao dao5 = new PointDetailDao();
                if (dao5.AddPointDetail(point))
                {
                    member.Expenditure += order.GetTotal();
                    member.OrderNumber++;
                    member.Points = point.Points;
                    dao4.Update(member);
                }
                Messenger.OrderPayment(member, order.OrderId, order.GetTotal());
                int historyPoint = dao5.GetHistoryPoint(member.UserId);
                MemberGradeInfo memberGrade = GetMemberGrade(member.GradeId);
                if ((memberGrade != null) && (memberGrade.Points > historyPoint))
                {
                    return flag;
                }
                List<MemberGradeInfo> memberGrades = new MemberGradeDao().GetMemberGrades() as List<MemberGradeInfo>;
                foreach (MemberGradeInfo info6 in from item in memberGrades
                                                  orderby item.Points descending
                                                  select item)
                {
                    if (member.GradeId == info6.GradeId)
                    {
                        return flag;
                    }
                    if (info6.Points <= historyPoint)
                    {
                        member.GradeId = info6.GradeId;
                        dao4.Update(member);
                        return flag;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 微信支付成功
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool UserPayOrderVirtual(OrderInfo order)
        {
            OrderDao dao = new OrderDao();
            order.OrderStatus = OrderStatus.Finished;
            order.PayDate = new DateTime?(DateTime.Now);
            order.FinishDate = order.PayDate;
            bool flag = dao.UpdateOrder(order, null);
            string str = "";
            if (flag)
            {
                dao.UpdatePayOrderStock(order.OrderId);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    str = str + "'" + info.SkuId + "',";
                    ProductInfo productDetails = dao2.GetProductDetails(info.ProductId);
                    productDetails.SaleCounts += info.Quantity;
                    productDetails.ShowSaleCounts += info.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    //虚拟订单项，默认为已完成
                    dao.UpdateItemsStatus(order.OrderId, 5, str.Substring(0, str.Length - 1));
                }
                if (!string.IsNullOrEmpty(order.ActivitiesId))
                {
                    new ActivitiesDao().UpdateActivitiesTakeEffect(order.ActivitiesId);
                }
                MemberInfo member = GetMember(order.UserId);
                if (member == null)
                {
                    return flag;
                }
                MemberDao dao4 = new MemberDao();
                PointDetailInfo point = new PointDetailInfo
                {
                    OrderId = order.OrderId,
                    UserId = member.UserId,
                    TradeDate = DateTime.Now,
                    TradeType = PointTradeType.Bounty,
                    Increased = new int?(order.Points),
                    Points = order.Points + member.Points
                };
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                PointDetailDao dao5 = new PointDetailDao();
                if (dao5.AddPointDetail(point))
                {
                    member.Expenditure += order.GetTotal();
                    member.OrderNumber++;
                    member.Points = point.Points;
                    dao4.Update(member);
                }
                Messenger.OrderPayment(member, order.OrderId, order.GetTotal());
                int historyPoint = dao5.GetHistoryPoint(member.UserId);
                MemberGradeInfo memberGrade = GetMemberGrade(member.GradeId);
                if ((memberGrade != null) && (memberGrade.Points > historyPoint))
                {
                    return flag;
                }
                List<MemberGradeInfo> memberGrades = new MemberGradeDao().GetMemberGrades() as List<MemberGradeInfo>;
                foreach (MemberGradeInfo info6 in from item in memberGrades
                                                  orderby item.Points descending
                                                  select item)
                {
                    if (member.GradeId == info6.GradeId)
                    {
                        return flag;
                    }
                    if (info6.Points <= historyPoint)
                    {
                        member.GradeId = info6.GradeId;
                        dao4.Update(member);
                        return flag;
                    }
                }
            }
            return flag;
        }

        public static MemberInfo GetMemberByUserName(string UserName)
        {
            return new MemberDao().GetMemberByUserName(UserName);
        }

        public static DataTable getUserInfoByIds(string ids)
        {
            return new MemberDao().getUserInfoByIds(ids);
        }
    }
}

