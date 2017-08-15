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
    public class UserNotReadDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加用户消息记录列表
        /// </summary>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddUserNotRead(UserNotRead info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into CW_UserNotRead(ID,FQUserId,JSUserId,UpdateTime,NotReadMsgCount) values (@ID,@FQUserId,@JSUserId,@UpdateTime,@NotReadMsgCount)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "FQUserId", DbType.Int32, info.FQUserId);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, info.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "NotReadMsgCount", DbType.Int32, info.NotReadMsgCount);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改用户消息记录列表
        /// </summary>
        public bool UpdateUserNotRead(UserNotRead info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update CW_UserNotRead set FQUserId=@FQUserId,JSUserId=@JSUserId,UpdateTime=@UpdateTime,NotReadMsgCount=@NotReadMsgCount where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "FQUserId", DbType.Int32, info.FQUserId);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "UpdateTime", DbType.DateTime, info.UpdateTime);
            this.database.AddInParameter(sqlStringCommand, "NotReadMsgCount", DbType.Int32, info.NotReadMsgCount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        public UserNotRead GetUserNotRead(Guid ID)
        {
            UserNotRead info = new UserNotRead();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_UserNotRead where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<UserNotRead>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        public DataTable GetUserNotReadData(string where = "",int top=0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select "+(top>0?"top "+top:"")+ " * from CW_UserNotRead " + where + " order by updatetime desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        public bool DeleteUserNotRead(Guid ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from CW_UserNotRead where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
