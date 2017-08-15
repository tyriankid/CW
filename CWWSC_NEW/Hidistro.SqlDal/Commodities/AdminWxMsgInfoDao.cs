using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
    public class AdminWxMsgInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加用户消息记录列表
        /// </summary>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddAdminWxMsgInfo(AdminWxMsgInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into CW_AdminWxMsgInfo(ID,FQManagerId,FQUserName,JSUserId,CreateTime,MsgContent) values (@ID,@FQManagerId,@FQUserName,@JSUserId,@CreateTime,@MsgContent)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "FQManagerId", DbType.Int32, info.FQManagerId);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "FQUserName", DbType.String, info.FQUserName);
            this.database.AddInParameter(sqlStringCommand, "MsgContent", DbType.String, info.MsgContent);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改用户消息记录列表
        /// </summary>
        public bool UpdateAdminWxMsgInfo(AdminWxMsgInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update CW_AdminWxMsgInfo set FQManagerId=@FQManagerId,FQUserName=@FQUserName,JSUserId=@JSUserId,CreateTime=@CreateTime,MsgContent=@MsgContent where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, info.ID);
            this.database.AddInParameter(sqlStringCommand, "FQUserName", DbType.String, info.FQUserName);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "MsgContent", DbType.String, info.MsgContent);
            this.database.AddInParameter(sqlStringCommand, "FQManagerId", DbType.Int32, info.FQManagerId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        public AdminWxMsgInfo GetAdminWxMsgInfo(Guid ID)
        {
            AdminWxMsgInfo info = new AdminWxMsgInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_AdminWxMsgInfo where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<AdminWxMsgInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        public DataTable GetAdminWxMsgInfoData(string where = "",int top=0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select "+(top>0?"top "+top:"")+ "  * from CW_AdminWxMsgInfo " + where + " order by createtime desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        public bool DeleteAdminWxMsgInfo(Guid ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from CW_AdminWxMsgInfo where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
