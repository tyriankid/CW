namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.SqlDal.Comments;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Store;
    using Hidistro.SqlDal.VShop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class VshopBrowser
    {
        public static int AddPrizeRecord(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().AddPrizeRecord(model);
        }

        public static bool UpdatePrizeRecordPrize(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().UpdatePrizeRecordPrize(model);
        }

        public static DbQueryResult FriendExtensionList(FriendExtensionQuery Query)
        {
            return new FriendExtensionDao().FriendExtensionList(Query);
        }

        public static ActivityInfo GetActivity(int activityId)
        {
            return new ActivityDao().GetActivity(activityId);
        }

        public static IList<BannerInfo> GetAllBanners()
        {
            return new BannerDao().GetAllBanners();
        }

        /// <summary>
        /// 获取所有大图
        /// </summary>
        /// <returns></returns>
        public static IList<BannerInfo> GetAllImgBanners()
        {
            return new BannerDao().GetAllImgBanners();
        }

        public static IList<NavigateInfo> GetAllNavigate()
        {
            return new BannerDao().GetAllNavigate();
        }

        public static int GetCountBySignUp(int activityId)
        {
            return new PrizeRecordDao().GetCountBySignUp(activityId);
        }

        public static DataTable GetHomeProducts()
        {
            return new HomeProductDao().GetHomeProducts();
        }

        public static LotteryActivityInfo GetLotteryActivity(int activityid)
        {
            LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().GetLotteryActivityInfo(activityid);
            if (lotteryActivityInfo != null)
            {
                lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
            }
            return lotteryActivityInfo;
        }

        public static LotteryTicketInfo GetLotteryTicket(int activityid)
        {
            LotteryTicketInfo lotteryTicket = new LotteryActivityDao().GetLotteryTicket(activityid);
            if (lotteryTicket != null)
            {
                lotteryTicket.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicket.PrizeSetting);
            }
            return lotteryTicket;
        }

        public static MessageInfo GetMessage(int messageId)
        {
            return new ReplyDao().GetMessage(messageId);
        }

        public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
        {
            return new PrizeRecordDao().GetPrizeList(page);
        }

        public static TopicInfo GetTopic(int topicId)
        {
            return new TopicDao().GetTopic(topicId);
        }

        public static DataTable GetTopics()
        {
            return new HomeTopicDao().GetHomeTopics();
        }

        public static int GetUserPrizeCount(int activityid)
        {
            return new PrizeRecordDao().GetUserPrizeCount(activityid);
        }

        public static PrizeRecordInfo GetUserPrizeRecord(int activityid)
        {
            return new PrizeRecordDao().GetUserPrizeRecord(activityid);
        }

        public static PrizeRecordInfo GetUserPrizeRecordById(int recordid)
        {
            return new PrizeRecordDao().GetUserPrizeRecordById(recordid);
        }

        public static DataTable GetVote(int voteId, out string voteName, out int checkNum, out int voteNum)
        {
            return new VoteDao().LoadVote(voteId, out voteName, out checkNum, out voteNum);
        }

        public static bool HasSignUp(int activityId, int userId)
        {
            return new PrizeRecordDao().HasSignUp(activityId, userId);
        }

        public static bool IsVote(int voteId)
        {
            return new VoteDao().IsVote(voteId);
        }

        public static bool OpenTicket(int ticketId)
        {
            LotteryTicketInfo lotteryTicket = GetLotteryTicket(ticketId);
            if (new PrizeRecordDao().OpenTicket(ticketId, lotteryTicket.PrizeSettingList))
            {
                lotteryTicket.IsOpened = true;
                return new LotteryActivityDao().UpdateLotteryTicket(lotteryTicket);
            }
            return false;
        }

        public static string SaveActivitySignUp(ActivitySignUpInfo info)
        {
            return new ActivitySignUpDao().SaveActivitySignUp(info);
        }

        public static bool UpdatePrizeRecord(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().UpdatePrizeRecord(model);
        }

        public static bool UpdatePrizeRecordById(PrizeRecordInfo model)
        {
            return new PrizeRecordDao().UpdatePrizeRecordById(model);
        }

        public static bool UpdatePrizeRecord(int activityId, int userId, string realName, string cellPhone)
        {
            PrizeRecordDao dao = new PrizeRecordDao();
            PrizeRecordInfo userPrizeRecord = dao.GetUserPrizeRecord(activityId);
            if (userPrizeRecord == null)
            {
                return false;
            }
            userPrizeRecord.UserID = userId;
            userPrizeRecord.RealName = realName;
            userPrizeRecord.CellPhone = cellPhone;
            return dao.UpdatePrizeRecord(userPrizeRecord);
        }

        public static bool Vote(int voteId, string itemIds)
        {
            return new VoteDao().Vote(voteId, itemIds);
        }

        /// <summary>
        /// 获取有效期内未参与的活动
        /// </summary>
        public static DataTable GetLotteryActivity_Valid(int activitytype, int userid)
        {
            return new LotteryActivityDao().SelectLotteryActivity_Valid(activitytype, userid);
        }

        /// <summary>
        /// 获取某活动的最新营销次数
        /// </summary>
        public static int GetNumCount(int ActivityID, int currNumCount)
        {
            return new LotteryActivityDao().SelectNumCount(ActivityID, currNumCount);
        }

        /// <summary>
        /// 验证是否启用了批发价、内购价
        /// </summary>
        /// <param name="currentMember">当前前端登录用户</param>
        /// <param name="iteminfo"></param>
        /// <param name="isPf"></param>
        /// <param name="neigouprice"></param>
        public static void ValidatePriceAndSetValue(MemberInfo currentMember, ShoppingCartItemInfo iteminfo, out bool isPf, out bool isNeigou)
        {
            //赋初始值
            isPf = false;
            isNeigou = false;

            if (iteminfo.ProductSource == 2)
            {
                #region  验证批发价格，暂时虚拟商品无批发价格

                //设置批发价格
                DataTable dtPfPrice = ProductHelper.GetProductPfPrices(iteminfo.ProductId);
                if (dtPfPrice.Rows.Count > 0)
                {
                    DataRow[] drs = dtPfPrice.Select(string.Format("Num <= {0}", iteminfo.Quantity), "Num desc", DataViewRowState.CurrentRows);
                    if (drs.Length > 0)
                    {
                        //如果IsStore = 1，则表示只有店长才可以享受批发价格
                        if (dtPfPrice.Rows[0]["IsStore"].ToString() == "1")
                        {
                            if (currentMember != null)
                            {
                                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                                if (userIdDistributors != null && userIdDistributors.UserId > 0)
                                    isPf = true;//是店长则享受批发价格
                                else
                                {
                                    //2017-07-04修改  验证是否店员
                                    DistributorSales disSalesinfo = DistributorSalesHelper.GetSalesBySaleUserId(currentMember.UserId);
                                    if (disSalesinfo != null && disSalesinfo.DsID != Guid.Empty)
                                        isPf = true;//是店员则享受批发价格
                                }
                            }
                        }
                        else
                            isPf = true;//只要购买都可以享受批发价格（前提是购买数量足够）
                        //是享受批发价格则修改销售价格
                        if (isPf)
                        {
                            decimal newprice = 0;
                            if (decimal.TryParse(drs[0]["PFSalePrice"].ToString(), out newprice))
                                iteminfo.AdjustedPrice = newprice;
                            else
                                isPf = false;
                        }
                    }
                }
                #endregion  验证批发价格，暂时虚拟商品无批发价格
            }
            if (iteminfo.ProductSource == 1 || iteminfo.ProductSource == 2 || iteminfo.ProductSource == 3 || iteminfo.ProductSource == 4 || iteminfo.ProductSource == 5)
            {
                #region  验证内购门店价格
                //判断是否为内购门店
                if (VShopHelper.ValidateNeiGouDistributor(currentMember.DistributorUserId))
                {
                    ProductInfo productinfo = ProductBrowser.GetProduct(currentMember, iteminfo.ProductId);
                    //2017-08-03，判断内购价购买的数量是否在规定数据以内，如果是则显示内购价， 如果不是则显示一口价
                    DataTable dt = OrderHelper.GetOrderItemByUserIdAndProId(currentMember.UserId, iteminfo.ProductId);
                    foreach (SKUItem item in productinfo.Skus.Values)
                    {
                        int icount = dt.Rows.Count + iteminfo.Quantity;
                        if (productinfo.RestrictNeigouNum >= icount)
                        {
                            if (item.SkuId == iteminfo.SkuId && item.NeigouPrice > 0)
                            {
                                isNeigou = true;
                                iteminfo.AdjustedPrice = iteminfo.NeigouPrice;
                                break;
                            }
                        }
                    }
                    //if (iteminfo.NeigouPrice > 0)
                    //{
                    //    isNeigou = true;
                    //    iteminfo.AdjustedPrice = iteminfo.NeigouPrice;
                    //}
                }
                #endregion 验证内购门店价格
            }
        }


    }
}

