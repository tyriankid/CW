using Hidistro.Core.Entities;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hidistro.Entities.Lottery;
using Hidistro.Entities.Store;
using Hidistro.ControlPanel.Store;
using System.Globalization;
using System.Data;
using Hidistro.SqlDal.Lottery;
//using Hidistro.SqlDal.Lottery;

namespace ControlPanel.Lottery
{
    public class LotteryRuleHelper
    {

        /// <summary>
        /// 根据规则Id得到对应实体
        /// </summary>
        /// <param name="dsid">乐透规则表主键ID</param>
        /// <returns>乐透规则实体</returns>
        public static LotteryRuleInfo LoadInfo(Guid dsid)
        {
            return new LotteryRuleDao().LoadInfo(dsid);
        }



        /// <summary>
        /// 执行自定义sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetSql(string strSql)
        {
            return new LotteryRuleDao().GetSql(strSql);
        }



        /// <summary>
        /// 根据主键删除乐透规则
        /// </summary>
        /// <param name="dsid">乐透规则主键ID</param>
        /// <returns></returns>
        public static bool DeleteLotteryRule(Guid ruleId)
        {
            bool flag = false;
            LotteryRuleInfo info = LoadInfo(ruleId);
            if (info != null && info.RuleId != Guid.Empty)
            {
                flag = new LotteryRuleDao().DeleteLotteryRule(ruleId);
                if (flag)
                {
                    //记录操作记录
                    EventLogs.WriteOperationLog(Privilege.DeleteDistributorSales, string.Format(CultureInfo.InvariantCulture, "”{0}”已删除", new object[] { info.LotteryItem }));
                }
                else
                {
                    EventLogs.WriteOperationLog(Privilege.DeleteDistributorSales, string.Format(CultureInfo.InvariantCulture, "”{0}”删除失败", new object[] { info.LotteryItem }));
                }
            }
            return flag;
        }




        /// <summary>
        /// 修改乐透规则信息
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateLotteryRule(LotteryRuleInfo ruleinfo)
        {
            bool flag = new LotteryRuleDao().UpdateLotteryRule(ruleinfo);
            LotteryRuleInfo info = LoadInfo(ruleinfo.RuleId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "”{0}”成功", new object[] { ruleinfo.LotteryItem }));
            }
            else
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "”{0}”添加成功", new object[] { ruleinfo.LotteryItem }));
            }
            return flag;
        }


        /// <summary>
        /// 添加新店员
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddLotteryRule(LotteryRuleInfo ruleinfo)
        {
            bool flag = new LotteryRuleDao().AddLotteryRule(ruleinfo);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "”{0}”添加成功", new object[] { ruleinfo.LotteryItem }));
            }
            else
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "“{0}”添加失败", new object[] { ruleinfo.LotteryItem }));
            }
            return flag;
        }




        /// <summary>
        /// 分页获取所有乐透规则
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetListLotteryRuleList(LotteryRuleQuery query)
        {
            return new LotteryRuleDao().GetListLotteryRuleList(query);
        }


    }
}
