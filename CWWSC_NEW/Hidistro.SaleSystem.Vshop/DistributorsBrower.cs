namespace Hidistro.SaleSystem.Vshop
{
    using System.Linq;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.VShop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Caching;
    using Hidistro.Entities.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Comments;
    using Hidistro.SqlDal.Commodities;


    public class DistributorsBrower
    {
        public static bool AddBalanceDrawRequest(BalanceDrawRequestInfo balancerequestinfo)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if ((((currentMember != null) && !string.IsNullOrEmpty(currentMember.RealName)) && ((currentDistributors != null) && (currentDistributors.UserId > 0))) && !string.IsNullOrEmpty(currentMember.CellPhone))
            {
                if (!(string.IsNullOrEmpty(balancerequestinfo.MerchanCade) || !(currentDistributors.RequestAccount != balancerequestinfo.MerchanCade)))
                {
                    new DistributorsDao().UpdateDistributorById(balancerequestinfo.MerchanCade, currentMember.UserId);
                }
                balancerequestinfo.AccountName = currentMember.RealName;
                balancerequestinfo.UserId = currentMember.UserId;
                balancerequestinfo.UserName = currentMember.UserName;
                balancerequestinfo.CellPhone = currentMember.CellPhone;
                return new DistributorsDao().AddBalanceDrawRequest(balancerequestinfo);
            }
            return false;
        }

        public static void AddDistributorProductId(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
                foreach (int num2 in productList)
                {
                    new DistributorsDao().AddDistributorProducts(num2, userId);
                }
            }
        }

        public static bool AddDistributors(DistributorsInfo distributors)
        {
            if (IsExiteDistributorsByStoreName(distributors.StoreName) > 0)
            {
                return false;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            distributors.DistributorGradeId = DistributorGrade.OneDistributor;//分销商级别ID
            distributors.ParentUserId = new int?(currentMember.UserId);//所属上一级分销商ID,默认为自己
            distributors.UserId = currentMember.UserId;//会员ID
            return new DistributorsDao().CreateDistributor(distributors);   //新建认证门店
            /*
            DistributorsInfo currentDistributors = GetCurrentDistributors();
            if (currentDistributors != null)//如果当前访问的分销商存在
            {
                //所属上级分销商ID,多级用|分隔
                if (!(string.IsNullOrEmpty(currentDistributors.ReferralPath) || currentDistributors.ReferralPath.Contains("|")))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath + "|" + currentDistributors.UserId.ToString();
                }
                else if (!(string.IsNullOrEmpty(currentDistributors.ReferralPath) || !currentDistributors.ReferralPath.Contains("|")))
                {
                    distributors.ReferralPath = currentDistributors.ReferralPath.Split(new char[] { '|' })[1] + "|" + currentDistributors.UserId.ToString();
                }
                else
                {
                    distributors.ReferralPath = currentDistributors.UserId.ToString();
                }
                distributors.ParentUserId = new int?(currentDistributors.UserId);//所属上一级分销商ID
                if (currentDistributors.DistributorGradeId == DistributorGrade.OneDistributor)          //顶级(分销商相对于厂家的级别)
                {
                    distributors.DistributorGradeId = DistributorGrade.TowDistributor;
                }
                else if (currentDistributors.DistributorGradeId == DistributorGrade.TowDistributor)     //二级(分销商相对于厂家的级别)
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
                }
                else
                {
                    distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;                //子级(分销商相对于厂家的级别)
                }
            }
            //设置所属代理商(By jinhb 20150820)
            distributors.IsAgent = 0;
            if (!string.IsNullOrEmpty(distributors.ReferralPath))
            {
                //得到所属分销商，从最近的往上找，找到就COPY对应的代理商累加字符串联
                DistributorsInfo parentDistributors = null;
                string[] arrayReferralPath = distributors.ReferralPath.Split('|');
                parentDistributors = GetCurrentDistributors(int.Parse(arrayReferralPath[arrayReferralPath.Length - 1]));
                if (parentDistributors.IsAgent == 1)
                {
                    distributors.AgentPath = (string.IsNullOrEmpty(parentDistributors.AgentPath))
                        ? parentDistributors.UserId.ToString() : parentDistributors.AgentPath + "|" + parentDistributors.UserId.ToString();
                }
                else
                {
                    distributors.AgentPath = parentDistributors.AgentPath;
                }
            }
            if (string.IsNullOrEmpty(distributors.AgentPath))
                return new DistributorsDao().CreateDistributor(distributors);   //分销商没有所属代理商
            else
                return new DistributorsDao().CreateAgent(distributors);   //分销商有所属代理商
             * */
        }

        public static void DeleteDistributorProductIds(List<int> productList)
        {
            int userId = GetCurrentDistributors().UserId;
            if ((userId > 0) && (productList.Count > 0))
            {
                new DistributorsDao().RemoveDistributorProducts(productList, userId);
            }
        }

        public static bool FrozenCommision(int userid, string ReferralStatus)
        {
            RemoveDistributorCache(userid);
            return new DistributorsDao().FrozenCommision(userid, ReferralStatus);
        }
        /// <summary>
        /// 根据门店ID得到六项维度的实际得分
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public static DataTable getStoreSixScoreData()
        {
            return new DistributorsDao().getStoreSixScoreData();
        }
        /// <summary>
        /// 获取所有门店
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public static DataTable GetDisTrobutorData()
        {
            return new DistributorsDao().GetDisTrobutorData();
        }
        public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            return new DistributorsDao().GetBalanceDrawRequest(query);
        }

        public static bool GetBalanceDrawRequestIsCheck(int serialid)
        {
            return new DistributorsDao().GetBalanceDrawRequestIsCheck(serialid);
        }

        public static DbQueryResult GetCommissions(CommissionsQuery query)
        {
            return new DistributorsDao().GetCommissions(query);
        }

        public static DistributorsInfo GetCurrentDistributors()
        {
            return GetCurrentDistributors(Globals.GetCurrentDistributorId());
        }

        public static DistributorsInfo GetCurrentDistributors(int userId)
        {
            DistributorsInfo distributorInfo = HiCache.Get(string.Format("DataCache-Distributor-{0}", userId)) as DistributorsInfo;
            if ((distributorInfo == null) || (distributorInfo.UserId == 0))
            {
                distributorInfo = new DistributorsDao().GetDistributorInfo(userId);
                HiCache.Insert(string.Format("DataCache-Distributor-{0}", userId), distributorInfo, 360, CacheItemPriority.Normal);
            }
            return distributorInfo;
        }

        public static DataTable GetCurrentDistributorsCommosion()
        {
            return new DistributorsDao().GetDistributorsCommosion(Globals.GetCurrentDistributorId());
        }

        public static DataTable GetCurrentDistributorsCommosion(int userId)
        {
            return new DistributorsDao().GetCurrentDistributorsCommosion(userId);
        }

        public static int GetDistributorGrades(string ReferralUserId)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
            foreach (DistributorGradeInfo info2 in from item in distributorGrades
                                                   orderby item.CommissionsLimit descending
                                                   select item)
            {
                if (userIdDistributors.DistriGradeId == info2.GradeId)
                {
                    return 0;
                }
                if (info2.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                {
                    userIdDistributors.DistriGradeId = info2.GradeId;
                    return info2.GradeId;
                }
            }
            return 0;
        }

        public static DistributorsInfo GetDistributorInfo(int distributorid)
        {
            return new DistributorsDao().GetDistributorInfo(distributorid);
        }

        public static int GetDistributorNum(DistributorGrade grade)
        {
            return new DistributorsDao().GetDistributorNum(grade);
        }
        /// <summary>
        /// 获取门店所有非服务订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetDistributorOrder(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrder(query);
        }
        /// <summary>
        /// 获取门店所有服务订单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet GetDistributorOrderService(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderService(query);
        }

        /// <summary>
        /// 获取天使下面所有分销商的所有订单信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetUnderOrders(OrderQuery query)
        {
            return new OrderDao().GetUnderOrders(query);
        }

        public static int GetDistributorOrderCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderCount(query);
        }

        public static int GetDistributorOrderServiceCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderServiceCount(query);
        }

        /// <summary>
        /// 包括服务订单数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int GetDistributorOrderAndServiceCount(OrderQuery query)
        {
            return new OrderDao().GetDistributorOrderAndServiceCount(query);
        }


        public static DbQueryResult GetDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributors(query);
        }

        public static DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            return new DistributorsDao().GetDistributorsCommission(query);
        }

        public static DataTable GetDistributorsCommosion(int userId, DistributorGrade distributorgrade)
        {
            return new DistributorsDao().GetDistributorsCommosion(userId, distributorgrade);
        }

        public static int GetDownDistributorNum(string userid)
        {
            return new DistributorsDao().GetDownDistributorNum(userid);
        }

        public static DataTable GetDownDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributors(query);
        }

        public static DataTable GetDownDistributor(int distributorId, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDownDistributor(distributorId,startDate,endDate);
        }
         /// <summary>
        /// 获取当前的distributor
        /// </summary>
        public static DataTable GetDistributor(int distributorId, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDistributor(distributorId, startDate,endDate);
        }

        /// <summary>
        /// 得到所有一级代理商
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllFirstDis(string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetAllFirstDis(startDate,endDate);
        }
         //获得下级
        public static DataTable GetDownDis(int UserID, string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetDownDis(UserID,startDate,endDate);
        }

        public static DataTable GetThreeDownDistributors(DistributorsQuery query)
        {
            return new DistributorsDao().GetThreeDownDistributors(query);
        }

        public static DataTable GetDownDistributorsAndAgents(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributorsAndAgents(query);
        }

        public static DataTable GetDownDistributorsAndA(DistributorsQuery query)
        {
            return new DistributorsDao().GetDownDistributorsAndA(query);
        }

        public static DataTable GetFirstDistributors(string startDate = "", string endDate = "")
        {
            return new DistributorsDao().GetFirstDistributors(startDate,endDate);
        }

        public static int GetNotDescDistributorGrades(string ReferralUserId)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            DistributorsInfo userIdDistributors = GetUserIdDistributors(int.Parse(ReferralUserId));
            decimal num2 = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;//用于判断的变量:佣金
            decimal num3 = userIdDistributors.OrdersTotal;//用于判断的变量:销售额
            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
            //增加了升级判断类型:根据分销商的销售价来判断
            switch (masterSettings.DistributorUpgradeType)
            {
                case "byComm":
                    //根据分销商的佣金判断
                    if ((distributorGradeInfo != null) && (num2 < distributorGradeInfo.CommissionsLimit))
                    {
                        return userIdDistributors.DistriGradeId;
                    }
                    List<DistributorGradeInfo> distributorGrades = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
                    foreach (DistributorGradeInfo info3 in from item in distributorGrades
                                                           orderby item.CommissionsLimit descending
                                                           select item)
                    {
                        if (userIdDistributors.DistriGradeId == info3.GradeId)
                        {
                            return 0;
                        }
                        if (info3.CommissionsLimit <= (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance))
                        {
                            return info3.GradeId;
                        }
                    }
                    break;
                case "byOrdersTotal":
                    //根据分销商的销售额判断
                    if ((distributorGradeInfo != null) && (num3 < distributorGradeInfo.CommissionsLimit))
                    {
                        return userIdDistributors.DistriGradeId;
                    }
                    List<DistributorGradeInfo> distributorGrades2 = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
                    foreach (DistributorGradeInfo info4 in from item in distributorGrades2
                                                           orderby item.CommissionsLimit descending
                                                           select item)
                    {
                        if (userIdDistributors.DistriGradeId == info4.GradeId)
                        {
                            return 0;
                        }
                        if (info4.CommissionsLimit <= (new DistributorsDao().GetDistributorDirectOrderTotal(userIdDistributors.UserId)))
                        {
                            return info4.GradeId;
                        }
                    }
                    break;
            }

            return 0;
        }

        public static DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
        {
            return new SendRedpackRecordDao().GetNotSendRedpackRecord(balancedrawrequestid);
        }

        public static int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
        {
            return new SendRedpackRecordDao().GetRedPackTotalAmount(balancedrawrequestid, userid);
        }

        public static SendRedpackRecordInfo GetSendRedpackRecordByID(int id)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordByID(id);
        }

        public static DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
        {
            return new SendRedpackRecordDao().GetSendRedpackRecordRequest(query);
        }

        public static decimal GetUserCommissions(int userid, DateTime fromdatetime)
        {
            return new DistributorsDao().GetUserCommissions(userid, fromdatetime);
        }

        public static DistributorsInfo GetUserIdDistributors(int userid)
        {
            return new DistributorsDao().GetDistributorInfo(userid);
        }


        public static DataSet GetUserRanking(int userid)
        {
            return new DistributorsDao().GetUserRanking(userid);
        }
        public static DataSet GetUserRankingQuery(DistributorsQuery query, int userid)
        {
            return new DistributorsDao().GetUserRankingQuery(query, userid);
        }

        public static bool HasDrawRequest(int serialid)
        {
            return new SendRedpackRecordDao().HasDrawRequest(serialid);
        }

        public static int IsExiteDistributorsByStoreName(string stroname)
        {
            return new DistributorsDao().IsExiteDistributorsByStoreName(stroname);
        }

        public static bool IsExitsCommionsRequest()
        {
            return new DistributorsDao().IsExitsCommionsRequest(Globals.GetCurrentDistributorId());
        }
        public static DataTable GetCommosionByTime(string where)
        {
            return new DistributorsDao() . GetCommosionByTime(where);
        }

        public static DataTable OrderIDGetCommosion(string orderid)
        {
            return new DistributorsDao().OrderIDGetCommosion(orderid);
        }

        public static void RemoveDistributorCache(int userId)
        {
            HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
        }

        public static string SendRedPackToBalanceDrawRequest(int serialid)
        {
            return new DistributorsDao().SendRedPackToBalanceDrawRequest(serialid);
        }

        public static bool setCommission(OrderInfo order, DistributorsInfo DisInfo)
        {
            bool flag = false;
            decimal num = 0M;
            decimal num2 = 0M;
            decimal resultCommTatal = 0M;
            string userId = order.ReferralUserId.ToString();
            string orderId = order.OrderId;
            decimal orderTotal = 0M;
            ArrayList gradeIdList = new ArrayList();
            ArrayList referralUserIdList = new ArrayList();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                {
                    num2 += info.ItemsCommission;
                    if (!(string.IsNullOrEmpty(info.ItemAdjustedCommssion.ToString()) || (info.ItemAdjustedCommssion <= 0M)))
                    {
                        num += info.ItemAdjustedCommssion;
                    }
                    orderTotal += info.GetSubTotal();
                }
            }

            //供应商订单处理，2017年批发价修改， 2017-07-27修改
            //如果购买商品为供应商商品，则判断是否存在批量价格设置，如果存在则不计算佣金，也不产生佣金记录
            if (order.OrderSource == 2)
            {
                DataTable dtPfPrice = new DataTable();
                int buyQuantity = 0;
                if (order.LineItems.Values.Count > 0)
                {
                    foreach (LineItemInfo info in order.LineItems.Values)
                    {
                        dtPfPrice = ProductHelper.GetProductPfPrices(info.ProductId);
                        buyQuantity = info.Quantity;
                        break;
                    }
                }
                if (dtPfPrice.Rows.Count > 0)
                {
                    bool isPf = false;//是否享受批发价格
                    //如果IsStore = 1，则表示只有店长才可以享受批发价格
                    if (dtPfPrice.Rows[0]["IsStore"].ToString() == "1")
                    {
                        DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(order.UserId);
                        if (userIdDistributors != null && userIdDistributors.UserId > 0)
                        {
                            //当前登录为【店长】，则享受批发价格
                            isPf = true;
                        }
                        else
                        {
                            //验证是否为店员
                            DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(order.UserId);
                            if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                            {
                                //当前登录为【店员】，则享受批发价格
                                isPf = true;
                            }
                        }
                    }
                    else
                        isPf = true;//只要购买都可以享受批发价格（前提是购买数量足够）
                    if (isPf)
                    {
                        DataRow[] drs = dtPfPrice.Select(string.Format("Num <= {0}", buyQuantity), "Num desc", DataViewRowState.CurrentRows);
                        if (drs.Length > 0)
                        {
                            decimal newprice = 0;
                            if (decimal.TryParse(drs[0]["PFSalePrice"].ToString(), out newprice))
                            {
                                //如果是供应商订单并且是批发价订单，则只记录店铺订单量和订单总额，不计佣金
                                bool isOk = new DistributorsDao().UpdateDistributorOrdersTotal(order.ReferralUserId.ToString(), orderTotal);
                                return true;
                            }
                        }
                    }
                }
            }
            //服务订单处理，2017-07-27开发服务订单业务增加
            if (order.OrderSource == 3)
            {
                //如果是服务订单，则只记录店铺订单量和订单总额，不计佣金
                bool isOk = new DistributorsDao().UpdateDistributorOrdersTotal(order.serviceUserId.ToString(), orderTotal);
                return true;
            }

            
            resultCommTatal = num2 - num;
            flag = new DistributorsDao().UpdateCalculationCommission(userId, userId, orderId, orderTotal, resultCommTatal);
            int notDescDistributorGrades = GetNotDescDistributorGrades(userId);
            if (notDescDistributorGrades > 0)
            {
                gradeIdList.Add(notDescDistributorGrades);
                referralUserIdList.Add(userId);
                flag = new DistributorsDao().UpdateGradeId(gradeIdList, referralUserIdList);
            }
            return flag;
        }

        public static bool SetRedpackRecordIsUsed(int id, bool issend)
        {
            return new SendRedpackRecordDao().SetRedpackRecordIsUsed(id, issend);
        }

       
        /// <summary>
        /// wt-2016-08-04 更新返佣金额，2017-08-03修改
        /// </summary>
        public static bool UpdateCalculationCommission(OrderInfo order)
        {
            DistributorsInfo userIdDistributors = GetUserIdDistributors(order.ReferralUserId);

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            bool flag = false;
            if (userIdDistributors != null)
            {
                bool isAgent1 = (userIdDistributors.IsAgent == 1) ? true : false;
                if (!masterSettings.EnableCommission)//未启用返佣
                {
                    if (userIdDistributors.ReferralStatus == 0)//状态正常
                    {
                        flag = setCommission(order, userIdDistributors);//更新门店佣金(仅更新直接销售的门店返佣)
                    }
                }
                else//启用了返佣
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        flag = setCommission(order, userIdDistributors);//更新直接销售的门店返佣
                    }
                }
                RemoveDistributorCache(userIdDistributors.UserId);
            }
            
            OrderRedPagerBrower.CreateOrderRedPager(order.OrderId, order.GetTotal(), order.UserId);
            return flag;
        }

        public static bool UpdateDistributor(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributor(query);
        }

        public static bool UpdateDistributorMessage(DistributorsInfo query)
        {
            int num = IsExiteDistributorsByStoreName(query.StoreName);
            if ((num != 0) && (num != query.UserId))
            {
                return false;
            }
            return new DistributorsDao().UpdateDistributorMessage(query);
        }

        public static DataTable GetDistributorsByWhere(string where)
        {
            return new DistributorsDao().SelectDistributorsByWhere(where);
        }
        public static void CommitDistributors(DataTable dtData)
        {
            new DistributorsDao().CommitDistributors(dtData);
        }

        public static DataTable GetDistributorProductRangeByUserid(int userid)
        {
            return new DistributorsDao().GetDistributorProductRangeByUserid(userid);
        }
        public static void CommitDistributorProductRange(DataTable dtData)
        {
            new DistributorsDao().CommitDistributorProductRange(dtData);
        }

        /// <summary>
        /// 获取当前店铺商品限定范围的枚举
        /// </summary>
        /// <returns></returns>
        public static ProductInfo.ProductRanage GetCurrStoreProductRange()
        {
            ProductInfo.ProductRanage productRanage = ProductInfo.ProductRanage.NormalSelect;
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
            if ((currentDistributors != null) && (currentDistributors.UserId != 0))
            {
                if (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto)
                    productRanage = ProductInfo.ProductRanage.All;
                else if (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableAgentProductRange)
                    productRanage = ProductInfo.ProductRanage.RangeSelect;
                else
                    productRanage = ProductInfo.ProductRanage.NormalSelect;
            }
            else
                productRanage = ProductInfo.ProductRanage.All;
            return productRanage;
        }

        /// <summary>
        /// 累加分销商店铺访问量,触发一次访问量+1
        /// </summary>
        /// <param name="distributorId">分销商id</param>
        public static void UpdateDistributorVisitCount(int memberId, int distributorId)
        {
            new DistributorsDao().UpdateDistributorVisitCount(memberId, distributorId);
        }
        /// <summary>
        /// 获取分销商店铺访问总数
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public static int GetDistributorVisitCount(int distributorId, string date = "", int memberId = -1)
        {
            return new DistributorsDao().GetDistributorVisitCount(distributorId, date, memberId).Rows[0]["visitCount"].ToInt();
        }

        /// <summary>
        /// 获取分销商的会员数
        /// </summary>
        public static int GetDistributorMemberCount(int distributorId, string date = "")
        {
            int i = 0;
            DataTable dt = new DistributorsDao().GetDistributorMemberCount(distributorId, date);
            if (dt.Rows.Count > 0)
                i = dt.Rows[0]["memberCount"].ToInt();
            return i;
        }

        public static DataTable GetDistributorVisitInfo()
        {
            return new DistributorsDao().GetDistributorVisitInfo();
        }

        public static DataTable GetAgentDistributorsVisitInfo(int agentId, string date = "")
        {
            return new DistributorsDao().GetAgentDistributorsVisitInfo(agentId, date);
        }
        public static int Updateaspnet_DistributorsUserId(int userid) 
        { 
            return new DistributorsDao().Updateaspnet_DistributorsUserId(userid);
        }
        public static int Updateaspnet_DistributorsServiceStoreId(int userid) 
        {
            return new DistributorsDao().Updateaspnet_DistributorsServiceStoreId(userid);
        }
        public static int Updateaspnet_DistributorsServiceToreId(int userid)
        {
            return new DistributorsDao().Updateaspnet_DistributorsServiceToreId(userid);
        }

        /// <summary>
        /// 设置为内购门店
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int UpdateDistributorsSetNeiGouStore(int userid)
        {
            return new DistributorsDao().UpdateDistributorsSetNeiGouStore(userid);
        }
        /// <summary>
        /// 取消内购门店
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int UpdateDistributorsCancelNeiGouStore(int userid)
        {
            return new DistributorsDao().UpdateDistributorsCancelNeiGouStore(userid);
        }

        public static bool DeleteProduct(int CommodityID)
        {
            return new DistributorsDao().DeleteProduct(CommodityID);
        }
        public static bool DeleteChannel(Guid ChannelId)
        {
            return new DistributorsDao().DeleteChannel(ChannelId);
        }
        public static bool DeleteDistributor(int userId)
        {
            //如果该分销商有下属,则不允许删除
            //if (DistributorsBrower.GetDownDistributorNum(userId.ToString()) > 0)
            //{
            //    return false;
            //}
            bool flag = new DistributorsDao().DeleteDistributor(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
            }
            return flag;
        }

        public static DataTable Export(DataTable OldDt)
        {
            DataTable dt = OldDt.Clone();
            foreach (DataRow dr in OldDt.Rows)
            {
                dt.Rows.Add(dr.ItemArray);
                GetDownDistributorExport(dt,Convert.ToInt32(dr["UserID"]));
            }
            return dt;
        }
        public static void GetDownDistributorExport(DataTable dt,int UserID)
        {
            DataTable ChiDt = GetDownDis(UserID);
            foreach(DataRow dr in ChiDt.Rows){
                dt.Rows.Add(dr.ItemArray);
                GetDownDistributorExport(dt, Convert.ToInt32(dr["UserID"]));
            }
        }

        /// <summary>
        /// 根据子账号id获取前台绑定的分销商id(目前仅用于爽爽挝啡多店铺子账号管理)
        /// </summary>
        public static int GetSenderDistributorId(string sender)
        {
            return new DistributorsDao().GetSenderDistributorId(sender);
        }


        /// <summary>
        /// 获取店铺下的所有用户
        /// </summary>
        /// <param name="distributorId"></param>
        /// <returns></returns>
        public static int GetDistributorUserCount(int distributorId)
        {
            return new DistributorsDao().GetDistributorUserCount(distributorId);
        }

        /// <summary>
        /// 分页获取店铺的所有用户信息
        /// </summary>
        /// <param name="usersquery">查询实体</param>
        /// <returns></returns>
        public static DbQueryResult GetDistributorUserInfo(DistributorsUsersQuery usersquery)
        {
            return new DistributorsDao().GetDistributorUserInfo(usersquery);
        }

        /// <summary>
        /// 将门店定位的地址存储到，2017-06-29修改
        /// </summary>
        /// <param name="distributors"></param>
        /// <returns></returns>
        public static bool SetDistributorsLocation(DistributorsInfo distributors)
        {
            return new DistributorsDao().SetDistributorsLocation(distributors);
        }

        /// <summary>
        /// 根据条件查询门店信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectVWDistributorsByWhere(string where)
        {
            return new DistributorsDao().SelectVWDistributorsByWhere(where);
        }

        /// <summary>
        /// 根据位置信息查询附近门店信息
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>DataTable数据表</returns>
        public static DbQueryResult SelectNearByPosition(DistributorListQuery query)
        {
            return new DistributorsDao().SelectNearByPosition(query);
        }

        /// <summary>
        /// 查询访问记录分页列表
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        public static DbQueryResult GetDistributorVisit(VisitQuery query)
        {
            return new DistributorsDao().GetDistributorVisit(query);
        }
        /// <summary>
        /// 商品维护提醒 2017-8-9 yk
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetProductMaintainRemindData(string where = "")
        {
            return new ProductMaintenanceReminderDao().GetProductMaintainRemindData(where);
        }
        /// <summary>
        /// 商品价格预约
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetProductReservePriceData(string where = "")
        {
            return new ProductReservePriceDao().GetProductReservePriceData(where);
        }
    }
}

