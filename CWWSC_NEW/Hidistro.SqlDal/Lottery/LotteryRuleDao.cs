using Hidistro.Core;
using Hidistro.Core.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Hidistro.Entities.Lottery;
using Hidistro.Entities;

namespace Hidistro.SqlDal.Lottery
{
    public class LotteryRuleDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 根据规则Id得到对应实体
        /// </summary>
        /// <param name="RuleId">乐透规则表主键ID</param>
        /// <returns>乐透规则实体</returns>
        public LotteryRuleInfo LoadInfo(Guid ruleId)
        {
            LotteryRuleInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LotteryRule WHERE RuleId = @RuleId");
            this.database.AddInParameter(sqlStringCommand, "RuleId", DbType.Guid, ruleId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<LotteryRuleInfo>(reader);
                reader.NextResult();
            }
            return info;
        }




        /// <summary>
        /// 执行自定义sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetSql(string strSql) {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0]; 
        }


        /// <summary>
        /// 根据主键删除乐透规则
        /// </summary>
        /// <param name="RuleId">乐透规则主键ID</param>
        /// <returns></returns>
        public bool DeleteLotteryRule(Guid ruleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_LotteryRule WHERE RuleId = @RuleId");
            this.database.AddInParameter(sqlStringCommand, "RuleId", DbType.Guid, ruleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }     






        /// <summary>
        /// 修改乐透规则
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateLotteryRule(LotteryRuleInfo ruleinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_LotteryRule SET LotteryItem=@LotteryItem, LotteryProportion = @LotteryProportion ,GiftId=@GiftId,Name=@Name,LotteryClass=@LotteryClass WHERE RuleId = @RuleId");
            this.database.AddInParameter(sqlStringCommand, "RuleId", DbType.Guid, ruleinfo.RuleId);
            this.database.AddInParameter(sqlStringCommand, "LotteryItem", DbType.String, ruleinfo.LotteryItem);
            this.database.AddInParameter(sqlStringCommand, "LotteryProportion", DbType.Int32, ruleinfo.LotteryProportion);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, ruleinfo.GiftId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, ruleinfo.Name);
            this.database.AddInParameter(sqlStringCommand, "LotteryClass", DbType.Int32, ruleinfo.LotteryClass);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }



        /// <summary>
        /// 添加乐透规则
        /// </summary>
        /// <param name="salesnfo">乐透规则实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddLotteryRule(LotteryRuleInfo ruleinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_LotteryRule (RuleId,LotteryItem,LotteryProportion,GiftId,Name,LotteryClass) VALUES (@RuleId,@LotteryItem,@LotteryProportion,@GiftId,@Name,@LotteryClass)");
            this.database.AddInParameter(sqlStringCommand, "RuleId", DbType.Guid, ruleinfo.RuleId);
            this.database.AddInParameter(sqlStringCommand, "LotteryItem", DbType.String, ruleinfo.LotteryItem);
            this.database.AddInParameter(sqlStringCommand, "LotteryProportion", DbType.Int32, ruleinfo.LotteryProportion);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, ruleinfo.GiftId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, ruleinfo.Name);
            this.database.AddInParameter(sqlStringCommand, "LotteryClass", DbType.Int32, ruleinfo.LotteryClass);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }






        /// <summary>
        /// 分页获取所有乐透规则
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListLotteryRuleList(LotteryRuleQuery query)
        {
            string sqlwhere = string.IsNullOrEmpty(query.KeyValue) ? string.Empty : string.Format("(LotteryItem LIKE '%{0}%' or LotteryProportion LIKE '%{0}%' or Name like '%{0}%')", DataHelper.CleanSearchString(query.KeyValue));
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_LotteryRule", "RuleId", sqlwhere, "*");
            
        }

       
        


    }
}
