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
     ����Ʒ��ص���ˢ�²���
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
                    this.integrallottery(context); //���ֳ齱
                    break;
            }
        }




        /// <summary>
        /// ���ֳ齱
        /// </summary>
        /// <param name="context"></param>
        private void integrallottery(HttpContext context)
        {

            DataTable dtLottery_Rule = LotteryRuleHelper.GetSql("select * from Hishop_LotteryRule ORDER BY LotteryClass ");

            //��ȡ��̨�����н�ռ�����ܺ�
            int lotteryProportionSum = 0;
            for (int i = 0; i < dtLottery_Rule.Rows.Count; i++)
            {
                lotteryProportionSum += int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());
            }

            //��ʼ����齱
            int lotteryNum = new Random().Next(1, lotteryProportionSum);

            //������������
            int No1 = 0;
            int No2 = 0;
            int No3 = 0;
            int No4 = 0;
            int No5 = 0;
            int No6 = 0;

            //����Բ����
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

            //����Բ
            int piece = 360 / dtLottery_Rule.Rows.Count;

            //ѭ����������������
            for (int i = 0; i < dtLottery_Rule.Rows.Count; i++)
            {
                switch (dtLottery_Rule.Rows[i]["LotteryClass"].ToString())
                {
                    case "1":
                        No1 = int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());//һ�Ƚ����ֵ
                        No1Star = (int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) - 1) * piece + 1 + 3600;//һ�Ƚ�����Բ������ʼ
                        No1End = int.Parse(dtLottery_Rule.Rows[i]["LotteryClass"].ToString()) * piece + 3600;//һ�Ƚ�����Բ�������β
                        break;
                    case "2":
                        No2 = No1 + int.Parse(dtLottery_Rule.Rows[i]["LotteryProportion"].ToString());//���Ƚ����ֵ
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


            //�ж��н�����
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
        /// 2017-8-8 yk �����û�������¼
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
                    context.Response.Write("{\"success\":true,\"msg\":\"����ɹ�\"}");
                }
            }
        }
        /// <summary>
        /// ��ȡ��ǰ�ŵ�Ļ�Ա
        /// </summary>
        /// <param name="context"></param>
        private void getStoreMember(HttpContext context)
        {
            string type = context.Request["type"];
            int pagesize =10;
            string keyword = context.Request["keyword"];//��ѯ�ؼ���
            int pagenum = int.Parse(context.Request["pagenum"]);//��ҳҳ��
        
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
                    #region ��ѯ�����û��б�
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
                    #region ��ѯճ�Ի�Ա�б�
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
            //��ȡ�û���ֵ�б�ҳ��
            int pageCount = count / pagesize + (count % pagesize == 0 ? 0 : 1);
            int nextPage = (pagenum < pageCount) ? (pagenum + 1) : 0; //��һҳΪ0ʱ����ʾ�����ݿɼ��أ����ݼ�����ϣ�

            builder.Clear();
            builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"PageCount\":{2},\"NextPage\":{3},\"Data\":", "OK", count, pageCount, nextPage));
            builder.Append(JsonConvert.SerializeObject(members.Data, Newtonsoft.Json.Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
            builder.Append("}");

            context.Response.Write(builder.ToString());
        }
        /// <summary>
        /// �����û�ѡ�еķ����ַ�õ������ŵ�
        /// </summary>
        /// <param name="context"></param>
        private void serviceStoreData(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"��������\"}");
            int productid = 0;
            int region = 0;
            string strProductId = context.Request["ProductId"].ToString();//��ƷID
            string strRegion = context.Request["Region"].ToString();//����ID
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
        /// �û���ǩ����
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
                    #region ΢��Ա��ǩ
                    //���֮ǰ������û��������µı�ǩ
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
                    #region ������Ա��ǩ
                    //���֮ǰ������û��������µı�ǩ
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
                //��ǰ�û���ǩ������ʮʱ�����Ϊճ�Ի�Ա
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
                context.Response.Write("{\"success\":true,\"msg\":\"��ǩ�ɹ�\"}");
            }
        }

        /// <summary>
        /// ��Ʒ���Ǽ�����
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
                context.Response.Write("{\"success\":true,\"msg\":\"���ֳɹ�\"}");
            }

        }


        /// <summary>
        /// ǰ���ŵ��Ա���� ɾ��/�༭/���� yk
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
                        context.Response.Write("{\"success\":false,\"msg\":\"��Ա������" + name.Trim() + "���Ѿ����ڣ�\"}");
                        return;
                    }
                    salesinfo.DsID = Guid.NewGuid();
                    salesinfo.DisUserId = currentMember.UserId;
                    salesinfo.DsName = name.Trim();
                    salesinfo.DsPhone = phone.Trim();
                    salesinfo.Scode = sort.Trim();
                    salesinfo.DsType = int.Parse(DsMemberType);
                    if (DistributorSalesHelper.AddDistributorSales(salesinfo))
                        context.Response.Write("{\"success\":true,\"msg\":\"������Ա��Ϣ�ɹ�\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"������Ա��Ϣʧ�ܣ�\"}");
                    break;
                case "edit":
                    salesinfo = DistributorSalesHelper.GetSalesByDsID(Id);
                    //��ѯ��Ա�����Ƿ����
                    if (!salesinfo.DsName.Equals(name))
                    {
                        if (dtSales.Select(string.Format("DsName = '{0}'", name.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                        {
                            context.Response.Write("{\"success\":false,\"msg\":\"��Ա������" + name.Trim() + "���Ѿ����ڣ�\"}");
                            return;
                        }
                    }
                    //�Ѿ���֤�������޸�����
                    if (salesinfo.IsRz != 1)
                    {
                        //��Ա��֤���ǲ������޸���Ϣ��
                        salesinfo.DsName = name.Trim();
                        salesinfo.DsPhone = phone.Trim();
                        salesinfo.DsType = int.Parse(DsMemberType);
                    }
                    salesinfo.Scode = sort.Trim();
                    salesinfo.DsID = Id;
                    if (DistributorSalesHelper.UpdateDistributorSales(salesinfo))
                        context.Response.Write("{\"success\":true,\"msg\":\"�༭��Ա��Ϣ�ɹ���\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"�༭��Ա��Ϣʧ�ܣ�\"}");
                    break;
                case "del":
                    if (DistributorSalesHelper.DeleteStoreInfo(Id))
                    {
                        context.Response.Write("{\"success\":true,\"msg\":\"�ɹ�ɾ����һ����Ա��Ϣ\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"success\":false,\"msg\":\"ɾ����Ա��Ϣʧ��\"}");
                    }
                    break;
            }
        }

        private const double EARTH_RADIUS = 6378.137;//����뾶
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
        /// ����΢�Žӿڻ�ȡ�����ŵ���Ϣ
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
                if (dtPoiInfo.Rows.Count == 0)//���û��ͬ��,�����΢�Žӿ����»�ȡ
                {
                    //��ȡaccess_token
                    string token = Access_token.GetAccess_token(Access_token.Access_Type.weixin, true);
                    //�ŵ��б�ӿ��ύurl
                    string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + token;
                    //�ύjson��,�ŵ��б�������ʼ:begin,�ŵ��б�����������:limit
                    string json = @"{""begin"":0,""limit"":10}";
                    //����post�ύ����
                    string strPOIList = new Hishop.Weixin.MP.Util.WebUtils().DoPost(url, json);
                    //�����ص�json�ַ���ת��Ϊjson����
                    JObject obj3 = JsonConvert.DeserializeObject(strPOIList) as JObject;
                    //��json����ת��Ϊʵ�������
                    poiInfoList = JsonHelper.JsonToList<PoiInfoList>(obj3["business_list"].ToString());

                    //ͬ��΢���ŵ���Ϣ
                    if (WxPoiHelper.SyncPoiListInfo(poiInfoList))
                    {
                        dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                    }
                }


                //��ȡ�����ŵ������
                string offset = string.Empty;
                foreach (DataRow row in dtPoiInfo.Rows)
                {
                    offset += row["longitude"] + "," + row["latitude"] + ";";//���Ӿ���γ��
                }
                offset = offset.TrimEnd(';');
                //���ŵ������������
                string[] offsetList = offset.Split(';');
                /****************�������ͷ�Χ���ŵ������ѭ��ƥ���û���ǰ������,��Χ:1����*******************/
                //��������ֵ(���ͷ�Χ)
                double range = Convert.ToDouble(context.Request["range"]);
                //��ȡ�û�������
                double userLongtitude = Convert.ToDouble(context.Request["userLontitude"]);
                double userLatitude = Convert.ToDouble(context.Request["userLatitude"]);
                //ѭ���жϻ�ȡ����,�õ����ͷ�Χ�ڵ��ŵ�poi_id
                List<string> poi_id = new List<string>();
                for (int i = 0; i < offsetList.Length; i++)
                {
                    string[] oa = offsetList[i].Split(',');//��ȡ�ŵ꾭��,γ��
                    double distance = GetDistance(userLatitude, userLongtitude, Convert.ToDouble(oa[1]), Convert.ToDouble(oa[0]));
                    if (distance <= range)
                    {
                        poi_id.Add(dtPoiInfo.Rows[i]["poi_id"].ToString());
                    }
                }
                bool isUserInRange = false;
                string matchIds = "";
                if (poi_id.Count > 0)//��������ͷ�Χ�ڵ��û�,�򷵻ص�һ��ƥ�䵽���ŵ��̨id
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
                    //���ƥ�䵽��΢���ŵ껹û�а�����̨�˺�,������ʾ
                    if (matchIds.Length == 0)
                    {
                        context.Response.Write("{\"success\":false,\"errMsg\":\"ƥ�䵽��δ�󶨵��ŵ�,�����ŵ껹δͨ�����!\"}");
                        return;
                    }
                    //�����ŵ�idƥ�䵽��Ӧ�����˺�id:sender
                    matchIds = matchIds.TrimEnd(',');
                    //string[] sender = matchId.Split(',');
                    //string[] clientUserId = matchId.Split(',');

                    /*
                    //��ƥ�䵽�������ŵ����ŵ����ֽ���չʾ (Ŀǰ����Ϊ�ֵ���)
                    DataTable dtStoreName = WxPoiHelper.GetStoreName(matchIds);
                    string storeNameBtns = "";
                    foreach (DataRow row in dtStoreName.Rows)
                    {
                        storeNameBtns += "<span role='btnStreet' distributorId='" + row["userid"].ToString() + "'>" + row["storeName"].ToString() + "</span>";
                    }
                    */
                    //��ƥ�䵽�����нֵ��Խֵ����ֽ���չʾ
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
                //����
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
        /// �����ŵ�ID�õ������Ա
        /// </summary>
        /// <param name="context"></param>
        private void serviceSalesList(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"��������\"}");
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
        /// ���÷��񶩵��ķ����Ա
        /// </summary>
        /// <param name="context"></param>
        private void setServiceSalesId(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"��������\"}");
            Guid servicesalesid = Guid.Empty;
            string strSelectSalesId = context.Request["SelectSalesId"].ToString();
            string orderid = context.Request["OrderId"].ToString();
            if (Guid.TryParse(strSelectSalesId, out servicesalesid) && !string.IsNullOrEmpty(orderid))
            {
                DataTable dtOrderSales = OrderSalesHelper.SelectClassByWhere(string.Format("OrderId = '{0}' AND State = 1",orderid));
                if (dtOrderSales.Rows.Count == 0)
                {
                    OrderSales info = new OrderSales();
                    info.OrderId = orderid;//���񶩵�
                    info.State = 1;//���������Ա�ɹ�
                    info.ServiceSalesId = servicesalesid;//������ԱID
                    info.CreateDate = DateTime.Now;
                    if (OrderSalesHelper.OrderSelectSales(info))
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"����ɹ���\"}");
                    }
                    else
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"������Աʧ�ܡ�\"}");
                    }
                }
                else
                {
                    builder.Clear();
                    builder.Append("{\"Result\":\"NO\",\"Msg\":\"�����Ѿ����������Ա���޷��ظ����䡣\"}");
                }
            }
            context.Response.Write(builder.ToString());
        }


        /// <summary>
        /// �곤�ܾ����񶩵�   �˻����˿�����
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
            refundInfo.UserId = orderinfo.UserId;//������
            int num = 7;
            refundInfo.RefundType = 1;

            if (int.Parse(context.Request["OrderStatus"].ToString()) == 2)
            {
                num = 6;
                refundInfo.HandleStatus = Hidistro.Entities.RefundInfo.Handlestatus.NoRefund;
                refundInfo.RefundType = 2;
                refundInfo.AuditTime = DateTime.Now.ToString();
            }

            //���������Ա�� �ܾ�Ҳ��¼
            OrderSales osinfo = new OrderSales();
            osinfo.OrderId = refundInfo.OrderId;
            osinfo.State = 0;//�˵�
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
                //֪ͨ�û�
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
        /// ������Ա�������񶩵�
        /// </summary>
        /// <param name="context"></param>
        private void serviceOrderCheck(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{\"Result\":\"NO\",\"Msg\":\"��������\"}");
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
                        builder.Append("{\"Result\":\"NO\",\"Msg\":\"��������Ʒ���˻�(��)������ȷ���ջ�\"}");
                    }
                    //�޸Ķ���״̬
                    else if (orderInfo == null || !MemberProcessor.ConfirmOrderTakeGoods(orderInfo))
                    {
                        builder.Clear();
                        builder.Append("{\"Result\":\"NO\",\"Msg\":\"������ǰ״̬������ȷ���ջ�\"}");
                    }
                    else
                    {
                        foreach (LineItemInfo value in orderInfo.LineItems.Values)
                        {
                            if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString())
                            {
                                continue;
                            }
                            //��������ϸ״̬���Ϊȷ���ջ�
                            ShoppingProcessor.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, (int)OrderStatus.ConfirmTakeGoods);
                        }
                        builder.Clear();
                        builder.Append("{\"Result\":\"OK\",\"Msg\":\"���񶩵������ɹ�\"}");
                    }
                }
            }
            context.Response.Write(builder.ToString());
        }

        
    }
}
