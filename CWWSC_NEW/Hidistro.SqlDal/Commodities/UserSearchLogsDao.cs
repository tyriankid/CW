using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
    public class UserSearchLogsDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加搜索记录
        /// </summary>
        /// <param name="UserSearchInfo">搜索记录实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddUserSearch(UserSearchLogsInfo UserSearchInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_UserSearchLogs (UserId,FunctionType,SearchText,SearchDate) VALUES (@UserId,@FunctionType,@SearchText,@SearchDate)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserSearchInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "FunctionType", DbType.String, UserSearchInfo.FunctionType);
            this.database.AddInParameter(sqlStringCommand, "SearchText", DbType.String, UserSearchInfo.SearchText);
            this.database.AddInParameter(sqlStringCommand, "SearchDate", DbType.DateTime, UserSearchInfo.SearchDate);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改搜索记录
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateUserSearch(UserSearchLogsInfo UserSearchInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_UserSearchLogs SET UserId = @UserId,FunctionType=@FunctionType,SearchText=@SearchText，SearchDate=@SearchDate  where SearchId=@SearchId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserSearchInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "FunctionType", DbType.String, UserSearchInfo.FunctionType);
            this.database.AddInParameter(sqlStringCommand, "SearchText", DbType.String, UserSearchInfo.SearchText);
            this.database.AddInParameter(sqlStringCommand, "SearchDate", DbType.DateTime, UserSearchInfo.SearchDate);
            this.database.AddInParameter(sqlStringCommand, "SearchId", DbType.Int32, UserSearchInfo.SearchId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public UserSearchLogsInfo GetUserSearchInfo(string SearchId)
        {
            UserSearchLogsInfo info = new UserSearchLogsInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_UserSearchLogs where SearchId=@SearchId");
            this.database.AddInParameter(sqlStringCommand, "SearchId", DbType.String, SearchId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<UserSearchLogsInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetUserSearchData(string where = "")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select Top 10 * from CW_UserSearchLogs " + where + "");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteUserSearch(string SearchId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_UserSearchLogs where SearchId=@SearchId");
            this.database.AddInParameter(sqlStringCommand, "SearchId", DbType.String, SearchId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据用户删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteUserSearchUserId(string userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_UserSearchLogs where UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 删除热搜
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteHotSearch()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_userHotSearch");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
