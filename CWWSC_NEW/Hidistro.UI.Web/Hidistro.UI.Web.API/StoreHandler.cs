using ControlPanel.Lottery;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    /*
     与商品相关的无刷新操作
     */
    public class StoreHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private IDictionary<string, string> jsondict = new Dictionary<string, string>();
        public void ProcessRequest(System.Web.HttpContext context)
        {
            string text = context.Request["action"];
            switch (text)
            {
                case "getPoiList":
                    this.getPoiList(context);
                    break;
                case "getStoreName":
                    this.getStoreName(context);
                    break;
                case "StoreRule":
                    this.StoreRule(context);
                    break;
                case "CommodityReview":
                    this.CommodityReview(context);
                    break;
                case "memberTags":
                    this.memberTags(context);
                    break;
                case "serviceStoreList":
                    this.serviceStoreData(context);
                    break;
                case "getStoreMember":
                    this.getStoreMember(context);
                    break;
                case "serviceSalesList":
                    this.serviceSalesList(context);
                    break;
                case "setServiceSalesId":
                    this.setServiceSalesId(context);
                    break;
                case "refuseServiceOrder":
                    this.refuseServiceOrder(context);
                    break;
                case "serviceOrderCheck":
                    this.serviceOrderCheck(context);
                    break;
                case "userSearchLog":
                    this.userSearchLog(context);
                    break;
                case "integrallottery":
                    this.integrallottery(context); //积分抽奖
                    break;
            }
        }




        /// <summary>
        /// 积分抽奖
        /// </summary>
        /// <param name="context"></param>
        private void integrallottery(HttpContext context)
        {

            DataTable dtLottery_Rule = LotteryRuleHelper.GetSql("select * from Hishop_LotteryRule ORDER BY LotteryClass ");

            //获取后台配置中奖占比数总和
            int lotteryProportionSum = 0;
            for (int i = 0; i < dtLottery_Rule.Rows.Count; i++)
            {
                lotteryProportionSum += int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
            }

            //开始随机抽奖
            int lotteryNum = new Random().Next(1, lotteryProportionSum);

            //声明奖项区间
            int No1 = 0;
            int No2 = 0;
            int No3 = 0;
            int No4 = 0;
            int No5 = 0;
            int No6 = 0;

            //声明圆区间
            int No1Star = 0;
            int No1End = 0;
            int No2Star = 0;
            int No2End = 0;
            int No3Star = 0;
            int No3End = 0;
            int No4Star = 0;
            int No4End = 0;
            int No5Star = 0;
            int No5End = 0;
            int No6Star = 0;
            int No6End = 0;

            //均分圆
            int piece = 360 / dtLottery_Rule.Rows.Count;

            //循环各个级别奖项区间
            for (int i = 0; i < dtLottery_Rule.Rows.Count; i++)
            {
                switch (dtLottery_Rule.Rows[i]["LotteryClass"].ToString())
                {
                    case "1":
                        No1 = int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());//一等奖最大值
                        No1Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;//一等奖所在圆的区域开始
                        No1End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;//一等奖所在圆的区域结尾
                        break;
                    case "2":
                        No2 = No1 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());//二等奖最大值
                        No2Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;
                        No2End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;
                        break;
                    case "3":
                        No3 = No2 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
                        No3Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;
                        No3End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;
                        break;
                    case "4":
                        No4 = No3 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
                        No4Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;
                        No4End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;
                        break;
                    case "5":
                        No5 = No4 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
                        No5Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;
                        No5End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;
                        break;
                    case "6":
                        No6 = No5 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
                        No6Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;
                        No6End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;
                        break;
                }
            }


            //判断中奖区间
            if (lotteryNum < No1)
            {
                int place = new Random().Next(No1Star, No1End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"",  dtLottery_Rule.Rows[0]["LotteryItem"], dtLottery_Rule.Rows[0]["Name"], place, dtLottery_Rule.Rows[0]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }
            else if (lotteryNum < No2)
            {
                int place = new Random().Next(No2Star, No2End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"",  dtLottery_Rule.Rows[1]["LotteryItem"], dtLottery_Rule.Rows[1]["Name"], place, dtLottery_Rule.Rows[1]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }
            else if (lotteryNum < No3)
            {
                int place = new Random().Next(No3Star, No3End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"",  dtLottery_Rule.Rows[2]["LotteryItem"], dtLottery_Rule.Rows[2]["Name"], place, dtLottery_Rule.Rows[2]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }
            else if (lotteryNum < No4)
            {
                int place = new Random().Next(No4Star, No4End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"",  dtLottery_Rule.Rows[3]["LotteryItem"], dtLottery_Rule.Rows[3]["Name"], place, dtLottery_Rule.Rows[3]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }
            else if (lotteryNum < No5)
            {
                int place = new Random().Next(No5Star, No5End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"", dtLottery_Rule.Rows[4]["LotteryItem"], dtLottery_Rule.Rows[4]["Name"], place, dtLottery_Rule.Rows[4]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }
            else if (lotteryNum < No6)
            {
                int place = new Random().Next(No6Star, No6End);
                string result = string.Format("{{\"state\":true,\"lotteryitem\":\"{0}\",\"productname\":\"{1}\",\"place\":{2},\"giftid\":\"{3}\"", dtLottery_Rule.Rows[5]["LotteryItem"], dtLottery_Rule.Rows[5]["Name"], place, dtLottery_Rule.Rows[5]["GiftId"]);
                result += "}";
                context.Response.Write(result);
                return;
            }

        }






        /// <summary>
        /// 2017-8-8 yk 保存用户搜索记录
        /// </summary>
        /// <param name="context"></param>
        private void userSearchLog(HttpContext context)
        {
            string keyword = context.Request["keyword"];
            string FunctionType = context.Request["FunctionType"];
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null && currentMember.UserId > 0)
            {
                UserSearchLogsInfo Info = new UserSearchLogsInfo
                {
                    UserId = currentMember.UserId,
                    FunctionType=FunctionType,
                    SearchText=keyword,
                    SearchDate=DateTime.Now,
                };
                if (UserSearchLogsHelper.AddUserSearch(Info))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"保存成功\"}");
                }
            }
        }
        /// <summary>
        /// 获取当前门店的会员
        /// </summary>
        /// <param name="context"></param>
        private void getStoreMember(HttpContext context)
        {
            string type = context.Request["type"];
            int pagesize =10;
            string keyword = context.Request["keyword"];//查询关键字
            int pagenum = int.Parse(context.Request["pagenum"]);//分页页码
        
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            MemberInfo currentMember = null;
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId());
            if (userIdDistributors != null)
                currentMember = MemberProcessor.GetCurrentMember();
            else
            {
                userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentDistributorId());
                if (userIdDistributors != null)
                    currentMember = MemberProcessor.GetMember(Globals.GetCurrentDistributorId());
            }
            DbQueryResult members = null;
            switch (type)
            {
                case "JL":
                    #region 查询金立用户列表
                    ListMembersQuery query = new ListMembersQuery()
                    {
                        SortBy = "UserId",
                        SortOrder = SortAction.Desc,
                        UserName = keyword,
                        PageSize = pagesize,
                        PageIndex =pagenum,
                    };
                    if (currentMember.UserId != 0)
                    {
                        query.id = StoreInfoHelper.GetStoreInfoByUserId(currentMember.UserId).Id;
                    }
                    members = CWMembersHelper.GetListMember(query);
                        #endregion
                    break;
                case "NX":
                    #region 查询粘性会员列表
                    nianxingMemberQuery querys = new nianxingMemberQuery()
                    {
                        SortBy = "UserId",
                        SortOrder = SortAction.Desc,
                        UserName = keyword,
                        PageSize = pagesize,
                        PageIndex = pagenum,
                    };
                    if (currentMember.UserId != 0)
                    {
                        querys.id = StoreInfoHelper.GetStoreInfoByUserId(currentMember.UserId).Id;
                    }
                    members = nianxingMembersHelper.GetListMember(querys);
                        #endregion
                    break;
            }
            int count = members.TotalRecords;
            //获取用户充值列表及页码
            int pageCount = count / pagesize + (count % pagesize == 0 ? 0 : 1);
            int nextPage = (pagenum < pageCount) ? (pagenum + 1) : 0; //下一页为0时，表示无数据可加载（数据加载完毕）

            builder.Clear();
            builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"PageCount\":{2},\"NextPage\":{3},\"Data\":", "OK", count, pageCount, nextPage));
            builder.Append(JsonConvert.SerializeObject(members.Data, Newtonsoft.Json.Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
            builder.Append("}");

            context.Response.Write(builder.ToString());
        }
        /// <summary>
        /// 根据用户选中的服务地址得到服务门店
        /// </summary>
        /// <param name="context"></param>
        private void serviceStoreData(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            int productid = 0;
            int region = 0;
            string strProductId = context.Request["ProductId"].ToString();//商品ID
            string strRegion = context.Request["Region"].ToString();//区域ID
            if (int.TryParse(strProductId, out productid) && int.TryParse(strRegion, out region))
            {
                System.Collections.Generic.IList<int> tagsId = null;
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                ProductInfo productinfo = ProductHelper.GetProductDetails(productid, out dictionary, out tagsId);
                if (productinfo != null && productinfo.ClassId.HasValue)
                {
                    string strwhere = string.Format("',' + ScIDs like '%,{0},%'", productinfo.ClassId.Value);
                    strwhere += string.Format(" AND RegionId like '{0}%'", strRegion);
                    strwhere += " ORDER BY LevelNum DESC ";
                    DataTable dtstores = DistributorClassHelper.SelectDistributorAndClassByWhere(strwhere);

                    builder.Clear();
                    builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"Data\":", "OK", dtstores.Rows.Count));
                    builder.Append(JsonConvert.SerializeObject(dtstores, Newtonsoft.Json.Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
                    builder.Append("}");
                }
            }
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 用户标签保存
        /// </summary>
        /// <param name="context"></param>
        private void memberTags(HttpContext context)
        {
            int userId = int.Parse(context.Request["userId"]);
            string selectIds = context.Request["selectIds"].TrimEnd(',');
            string JinLiIds = context.Request["JinLiIds"].TrimEnd(',');
            bool flag = false;
            try
            {
                MemberInfo currentMember = MemberProcessor.GetMember(userId);
                StoreInfo storeinfo = StoreInfoHelper.GetStoreInfoByUserId(Globals.GetCurrentDistributorId());
                if (!string.IsNullOrEmpty(selectIds))
                {
                    #region 微会员标签
                    //添加之前清除该用户该类型下的标签
                    memberTagsRelationHelper.DeleteMemberTagsWhere(" where userId=" + userId + " and UserType=0");
                    string[] id = selectIds.Split(',');
                    for (int m = 0; m < id.Length; m++)
                    {
                        memberTagsRelationInfo info = new memberTagsRelationInfo
                        {
                            UserType = 0,
                            UserId = userId,
                            TagID = int.Parse(id.GetValue(m).ToString()),
                            CreateTime = DateTime.Now,
                        };
                        if (memberTagsRelationHelper.AddmemberTags(info))
                        {
                            flag = true;
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(JinLiIds))
                {
                    #region 金立会员标签
                    //添加之前清除该用户该类型下的标签
                    memberTagsRelationHelper.DeleteMemberTagsWhere(" where userId=" + userId + " and UserType=1");
                    string[] idJ = JinLiIds.Split(',');
                    for (int m = 0; m < idJ.Length; m++)
                    {
                        memberTagsRelationInfo infos = new memberTagsRelationInfo
                        {
                            UserType = 1,
                            UserId = userId,
                            TagID = int.Parse(idJ.GetValue(m).ToString()),
                            CreateTime = DateTime.Now,
                        };
                        if (memberTagsRelationHelper.AddmemberTags(infos))
                        {
                            flag = true;
                        }
                    }
                    #endregion
                }
                //当前用户标签数大于十时，添加为粘性会员
                if (memberTagsRelationHelper.GetmemberTagsNum(userId.ToString()) > 10)
                {
                    if (storeinfo != null)
                    {
                        nianxingMembersInfo infoNX = new nianxingMembersInfo
                        {
                            UserName = currentMember.UserName,
                            accountALLHere = storeinfo.accountALLHere,
                            StoreName = storeinfo.storeName,
                            CellPhone = currentMember.CellPhone,
                            Address = Globals.HtmlEncode(currentMember.Address),
                            StoreId = storeinfo.Id,
                            UserId = userId,

                        };
                        nianxingMembersHelper.Create(infoNX);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":true,\"msg\":\"" + ex.Message + "\"}");
            }
            if (flag)
            {
                context.Response.Write("{\"success\":true,\"msg\":\"标签成功\"}");
            }
        }

        /// <summary>
        /// 商品的星级评价
        /// </summary>
        /// <param name="context"></param>
        private void CommodityReview(HttpContext context)
        {
            int oneReview = int.Parse(string.IsNullOrEmpty(context.Request["review1"]) ? "0" : context.Request["review1"]);
            int twoReview = int.Parse(string.IsNullOrEmpty(context.Request["review2"]) ? "0" : context.Request["review2"]);
            int threeReview = int.Parse(string.IsNullOrEmpty(context.Request["review3"]) ? "0" : context.Request["review3"]);
            int fourReview = int.Parse(string.IsNullOrEmpty(context.Request["review4"]) ? "0" : context.Request["review4"]);
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            DistributorMarkInfo info = new DistributorMarkInfo
            {
                ID = Guid.NewGuid(),
                DisUserId = currentManager.ClientUserId,
                UserId = currentMember.UserId,
                Mark1 = oneReview * decimal.Parse("0.25"),
                Mark2 = twoReview * decimal.Parse("0.25"),
                Mark3 = threeReview * decimal.Parse("0.25"),
                Mark4 = fourReview * decimal.Parse("0.25"),
                Mark5 = 0,
            };
            info.Total = (info.Mark1 + info.Mark2 + info.Mark3 + info.Mark4 + info.Mark5);
            if (DistributorMarkHelper.AddDistributorMarks(info))
            {
                context.Response.Write("{\"success\":true,\"msg\":\"评分成功\"}");
            }

        }


        /// <summary>
        /// 前端门店会员管理 删除/编辑/新增 yk
        /// </summary>
        /// <param name="context"></param>
        private void StoreRule(HttpContext context)
        {
            string type = context.Request["StoreRuleType"];
            string name = context.Request["name"];
            string phone = context.Request["phone"];
            string sort = context.Request["sort"];
            string DsMemberType = context.Request["type"];
            Guid Id = new Guid();
            if (!string.IsNullOrEmpty(context.Request["DsID"]))
            {
                Id = new Guid(context.Request["DsID"]);
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DataTable dtSales = DistributorSalesHelper.SelectSalesByDisUserId(currentMember.UserId);
            DistributorSales salesinfo = new DistributorSales();
            switch (type)
            {
                case "add":
                    if (dtSales.Select(string.Format("DsName = '{0}'", name.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"店员姓名【" + name.Trim() + "】已经存在！\"}");
                        return;
                    }
                    salesinfo.DsID = Guid.NewGuid();
                    salesinfo.DisUserId = currentMember.UserId;
                    salesinfo.DsName = name.Trim();
                    salesinfo.DsPhone = phone.Trim();
                    salesinfo.Scode = sort.Trim();
                    salesinfo.DsType = int.Parse(DsMemberType);
                    if (DistributorSalesHelper.AddDistributorSales(salesinfo))
                        context.Response.Write("{\"success\":true,\"msg\":\"新增店员信息成功\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"新增店员信息失败！\"}");
                    break;
                case "edit":
                    salesinfo = DistributorSalesHelper.GetSalesByDsID(Id);
                    //查询店员姓名是否存在
                    if (!salesinfo.DsName.Equals(name))
                    {
                        if (dtSales.Select(string.Format("DsName = '{0}'", name.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                        {
                            context.Response.Write("{\"success\":false,\"msg\":\"店员姓名【" + name.Trim() + "】已经存在！\"}");
                            return;
                        }
                    }
                    //已经认证后不允许修改名称
                    if (salesinfo.IsRz != 1)
                    {
                        //店员认证后是不允许修改信息的
                        salesinfo.DsName = name.Trim();
                        salesinfo.DsPhone = phone.Trim();
                        salesinfo.DsType = int.Parse(DsMemberType);
                    }
                    salesinfo.Scode = sort.Trim();
                    salesinfo.DsID = Id;
                    if (DistributorSalesHelper.UpdateDistributorSales(salesinfo))
                        context.Response.Write("{\"success\":true,\"msg\":\"编辑店员信息成功！\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"编辑店员信息失败！\"}");
                    break;
                case "del":
                    if (DistributorSalesHelper.DeleteStoreInfo(Id))
                    {
                        context.Response.Write("{\"success\":true,\"msg\":\"成功删除了一个店员信息\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"删除店员信息失败\"}");
                    }
                    break;
            }
        }

        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        private void getStoreName(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string storeId = context.Request["storeId"].ToString();
            DataTable dtNames = WxPoiHelper.GetStoreName(storeId);
            string storeNames = "";
            foreach (DataRow row in dtNames.Rows)
            {
                storeNames += row["storeName"];
            }

            context.Response.Write("{\"success\":true,\"storeName\":\"" + storeNames + "\"}");
        }

        /// <summary>
        /// 调用微信接口获取所有门店信息
        /// </summary>
        /// <param name="context"></param>
        private void getPoiList(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                Thread.Sleep(1000);
                DataTable dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                List<PoiInfoList> poiInfoList = new List<PoiInfoList>();
                if (dtPoiInfo.Rows.Count == 0)//如果没有同步,则调用微信接口重新获取
                {
                    //获取access_token
                    string token = Access_token.GetAccess_token(Access_token.Access_Type.weixin, true);
                    //门店列表接口提交url
                    string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + token;
                    //提交json串,门店列表索引开始:begin,门店列表返回数量限制:limit
                    string json = @"{""begin"":0,""limit"":10}";
                    //调用post提交方法
                    string strPOIList = new Hishop.Weixin.MP.Util.WebUtils().DoPost(url, json);
                    //将传回的json字符串转换为json对象
                    JObject obj3 = JsonConvert.DeserializeObject(strPOIList) as JObject;
                    //将json对象转换为实体类对象
                    poiInfoList = JsonHelper.JsonToList<PoiInfoList>(obj3["business_list"].ToString());

                    //同步微信门店信息
                    if (WxPoiHelper.SyncPoiListInfo(poiInfoList))
                    {
                        dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                    }
                }


                //获取所有门店的坐标
                string offset = string.Empty;
                foreach (DataRow row in dtPoiInfo.Rows)
                {
                    offset += row["longitude"] + "," + row["latitude"] + ";";//增加精度纬度
                }
                offset = offset.TrimEnd(';');
                //将门店坐标放入数组
                string[] offsetList = offset.Split(';');
                /****************根据配送范围将门店的坐标循环匹配用户当前的坐标,误差范围:1公里*******************/
                //允许的误差值(配送范围)
                double range = Convert.ToDouble(context.Request["range"]);
                //获取用户的坐标
                double userLongtitude = Convert.ToDouble(context.Request["userLontitude"]);
                double userLatitude = Convert.ToDouble(context.Request["userLatitude"]);
                //循环判断获取距离,得到配送范围内的门店poi_id
                List<string> poi_id = new List<string>();
                for (int i = 0; i < offsetList.Length; i++)
                {
                    string[] oa = offsetList[i].Split(',');//获取门店经度,纬度
                    double distance = GetDistance(userLatitude, userLongtitude, Convert.ToDouble(oa[1]), Convert.ToDouble(oa[0]));
                    if (distance <= range)
                    {
                        poi_id.Add(dtPoiInfo.Rows[i]["poi_id"].ToString());
                    }
                }
                bool isUserInRange = false;
                string matchIds = "";
                if (poi_id.Count > 0)//如果有配送范围内的用户,则返回第一个匹配到的门店后台id
                {
                    for (int i = 0; i < poi_id.Count; i++)
                    {
                        DataTable dtSender = WxPoiHelper.GetSenderByPoiId(poi_id[i]);
                        foreach (DataRow row in dtSender.Rows)
                        {
                            if (row["clientUserId"].ToString() != "")
                            {
                                matchIds += row["clientUserId"] + ",";

                            }
                        }
                    }
                    isUserInRange = true;
                    //如果匹配到的微信门店还没有绑定至后台账号,给出提示
                    if (matchIds.Length == 0)
                    {
                        context.Response.Write("{\"success\":false,\"errMsg\":\"匹配到了未绑定的门店,或者门店还未通过审核!\"}");
                        return;
                    }
                    //根据门店id匹配到对应的子账号id:sender
                    matchIds = matchIds.TrimEnd(',');
                    //string[] sender = matchId.Split(',');
                    //string[] clientUserId = matchId.Split(',');

                    /*
                    //将匹配到的所有门店以门店名字进行展示 (目前更换为街道名)
                    DataTable dtStoreName = WxPoiHelper.GetStoreName(matchIds);
                    string storeNameBtns = "";
                    foreach (DataRow row in dtStoreName.Rows)
                    {
                        storeNameBtns += "<span role='btnStreet' distributorId='" + row["userid"].ToString() + "'>" + row["storeName"].ToString() + "</span>";
                    }
                    */
                    //将匹配到的所有街道以街道名字进行展示
                    DataTable dtStreetName = WxPoiHelper.GetStoreStreets(matchIds);
                    string streetNameBtns = "";
                    foreach (DataRow row in dtStreetName.Rows)
                    {
                        streetNameBtns += "<span role='btnStreet' distributorId='" + row["distributorid"].ToString() + "'>" + row["regionName"].ToString() + "</span>";
                    }


                    context.Response.Write("{\"success\":true,\"isUserInRange\":\"" + isUserInRange + "\",\"distributorId\":\"" + streetNameBtns + "\"}");

                }

                else
                {
                    context.Response.Write("{\"success\":true,\"isUserInRange\":\"" + isUserInRange + "\"}");
                }

                /*
                //调试
                string[] la0 = offsetList[0].Split(',');
                double distance = GetDistance(userLatitude,userLongtitude,Convert.ToDouble(la0[1]),Convert.ToDouble(la0[0]));
                context.Response.Write("{\"success\":true,\"userLo\":" + userLongtitude + ",\"userLa\":" + userLatitude + ",\"poiLA\":\"" + offsetList[0] + "\",\"distance\":"+distance+"}");
                 */
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"errMsg\":\"" + ex.Message + "\"}");
            }

        }

        /// <summary>
        /// 根据门店ID得到服务店员
        /// </summary>
        /// <param name="context"></param>
        private void serviceSalesList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            int serviceuserid = 0;
            string strServiceUserId = context.Request["ServiceUserId"].ToString();
            if (int.TryParse(strServiceUserId, out serviceuserid))
            {
                string strWhere = string.Format(" DsType = 1 AND DisUserId = {0} AND IsRz = 1", serviceuserid);
                DataTable dtServiceSales = DistributorSalesHelper.SelectSalesUserInfoByWhere(strWhere);

                builder.Clear();
                builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"Data\":", "OK", dtServiceSales.Rows.Count));
                builder.Append(JsonConvert.SerializeObject(dtServiceSales, Newtonsoft.Json.Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
                builder.Append("}");
            }
            context.Response.Write(builder.ToString());
        }
        
        /// <summary>
        /// 设置服务订单的服务店员
        /// </summary>
        /// <param name="context"></param>
        private void setServiceSalesId(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            Guid servicesalesid = Guid.Empty;
            string strSelectSalesId = context.Request["SelectSalesId"].ToString();
            string orderid = context.Request["OrderId"].ToString();
            if (Guid.TryParse(strSelectSalesId, out servicesalesid) && !string.IsNullOrEmpty(orderid))
            {
                DataTable dtOrderSales = OrderSalesHelper.SelectClassByWhere(string.Format("OrderId = '{0}' AND State = 1",orderid));
                if (dtOrderSales.Rows.Count == 0)
                {
                    OrderSales info = new OrderSales();
                    info.OrderId = orderid;//服务订单
                    info.State = 1;//分配服务人员成功
                    info.ServiceSalesId = servicesalesid;//服务人员ID
                    info.CreateDate = DateTime.Now;
                    if (OrderSalesHelper.OrderSelectSales(info))
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"分配成功。\"}");
                    }
                    else
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"分配人员失败。\"}");
                    }
                }
                else
                {
                    builder.Clear();
                    builder.Append("{\"Result\":\"NO\",\"Msg\":\"订单已经分配服务人员，无法重复分配。\"}");
                }
            }
            context.Response.Write(builder.ToString());
        }


        /// <summary>
        /// 店长拒绝服务订单   退货，退款请求
        /// </summary>
        /// <param name="context"></param>
        public void refuseServiceOrder(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Hidistro.Entities.RefundInfo refundInfo = new Hidistro.Entities.RefundInfo();
            refundInfo.OrderId = context.Request["OrderId"];
            refundInfo.ApplyForTime = DateTime.Now;
            refundInfo.RefundRemark = context.Request["RefuseMsg"];
            refundInfo.HandleStatus = Hidistro.Entities.RefundInfo.Handlestatus.NoneAudit;
            //refundInfo.Account = context.Request["Account"];
            refundInfo.RefundMoney = decimal.Parse(context.Request["Money"]);
            refundInfo.ProductId = int.Parse(context.Request["ProductId"]);

            StringBuilder stringBuilder = new StringBuilder();
            OrderInfo orderinfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);
            refundInfo.UserId = orderinfo.UserId;//购买者
            int num = 7;
            refundInfo.RefundType = 1;

            if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
            {
                num = 6;
                refundInfo.HandleStatus = Hidistro.Entities.RefundInfo.Handlestatus.NoRefund;
                refundInfo.RefundType = 2;
                refundInfo.AuditTime = DateTime.Now.ToString();
            }

            //分配服务人员， 拒绝也记录
            OrderSales osinfo = new OrderSales();
            osinfo.OrderId = refundInfo.OrderId;
            osinfo.State = 0;//退单
            osinfo.CreateDate = DateTime.Now;
            osinfo.RefuseRemark = refundInfo.RefundRemark;

            stringBuilder.Append("{");
            if (ShoppingProcessor.GetReturnMes(refundInfo.UserId, refundInfo.OrderId, refundInfo.ProductId, (int)refundInfo.HandleStatus))
            {
                stringBuilder.Append("\"Status\":\"Repeat\"");
            }
            else if (!ShoppingProcessor.InsertOrderRefund(refundInfo))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
            }
            else if (!ShoppingProcessor.UpdateOrderGoodStatu(refundInfo.OrderId, context.Request["SkuId"], num))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
            }
            else if (!OrderSalesHelper.AddOrderSales(osinfo))
            {
                stringBuilder.Append("\"Status\":\"Error\"");
            }
            else
            {
                stringBuilder.Append("\"Status\":\"OK\"");
                //通知用户
                MemberInfo member = MemberProcessor.GetMember(orderinfo.UserId);
                if (member != null)
                {
                    Messenger.OrderReturnSendManage(orderinfo, member);
                }
            }
            stringBuilder.Append("}");
            context.Response.Write(stringBuilder.ToString());
        }

        /// <summary>
        /// 服务人员核销服务订单
        /// </summary>
        /// <param name="context"></param>
        private void serviceOrderCheck(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"参数错误。\"}");
            string serviceOrderId = context.Request["OrderId"].ToString();
            string serviceCode = context.Request["ServiceCode"].ToString();
            if (!string.IsNullOrEmpty(serviceOrderId) && !string.IsNullOrEmpty(serviceCode))
            {
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                bool flag = false;
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(serviceOrderId);
                if (orderInfo.serviceCode.Equals(serviceCode))
                {
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
                        builder.Clear();
                        builder.Append("{\"Result\":\"NO\",\"Msg\":\"订单中商品有退货(款)不允许确认收货\"}");
                    }
                    //修改订单状态
                    else if (orderInfo == null || !MemberProcessor.ConfirmOrderTakeGoods(orderInfo))
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"NO\",\"Msg\":\"订单当前状态不允许确认收货\"}");
                    }
                    else
                    {
                        foreach (LineItemInfo value in orderInfo.LineItems.Values)
                        {
                            if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString())
                            {
                                continue;
                            }
                            //将订单明细状态标记为确认收货
                            ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, (int)OrderStatus.ConfirmTakeGoods);
                        }
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"服务订单核销成功\"}");
                    }
                }
            }
            context.Response.Write(builder.ToString());
        }

        
    }
}
