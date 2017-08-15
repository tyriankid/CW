using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
    public class StoreUserChangeApplyDao
    {
        private Database database = DatabaseFactory.CreateDatabase();


        public bool AddStoreUserChangeApply(StoreUserChangeApply info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into CW_StoreUserChangeApply(ID,ApplyUserId,StoreUserId,ApplyState,AuditingDate,Reason) values (@ID,@ApplyUserId,@StoreUserId,@ApplyState,@AuditingDate,@Reason)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "ApplyUserId", DbType.Int32, info.ApplyUserId);
            this.database.AddInParameter(sqlStringCommand, "StoreUserId", DbType.Int32, info.StoreUserId);
            this.database.AddInParameter(sqlStringCommand, "ApplyState", DbType.Int32, info.ApplyState);
            this.database.AddInParameter(sqlStringCommand, "AuditingDate", DbType.DateTime, info.AuditingDate);
            this.database.AddInParameter(sqlStringCommand, "Reason", DbType.String, info.Reason);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改用户消息记录列表
        /// </summary>
        public bool UpdateStoreUserChangeApply(StoreUserChangeApply info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update CW_StoreUserChangeApply set ApplyUserId=@ApplyUserId,StoreUserId=@StoreUserId,ApplyState=@ApplyState,AuditingDate=@AuditingDate,Reason=@Reason where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "ApplyUserId", DbType.Int32, info.ApplyUserId);
            this.database.AddInParameter(sqlStringCommand, "StoreUserId", DbType.Int32, info.StoreUserId);
            this.database.AddInParameter(sqlStringCommand, "ApplyState", DbType.Int32, info.ApplyState);
            this.database.AddInParameter(sqlStringCommand, "AuditingDate", DbType.DateTime, info.AuditingDate);
            this.database.AddInParameter(sqlStringCommand, "Reason", DbType.String, info.Reason);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        public StoreUserChangeApply GetStoreUserChangeApply(Guid ID)
        {
            StoreUserChangeApply info = new StoreUserChangeApply();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_StoreUserChangeApply where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<StoreUserChangeApply>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        public DataTable GetStoreUserChangeApplyData(string where = "", int top = 0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_StoreUserChangeApply where " + where);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        public bool DeleteStoreUserChangeApply(Guid ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from CW_UserDialogInfo where DialogID=@DialogID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        public int ChangeUserToStore(int userid, int storeid, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ChangeUserToStore");
            this.database.AddInParameter(storedProcCommand, "userid", DbType.Int32, userid);
            this.database.AddInParameter(storedProcCommand, "storeid", DbType.Int32, storeid);
            this.database.AddOutParameter(storedProcCommand, "result", DbType.Int32, 4);
            this.database.ExecuteNonQuery(storedProcCommand, dbTran);
            return (int)this.database.GetParameterValue(storedProcCommand, "result");
        }



    }
}
