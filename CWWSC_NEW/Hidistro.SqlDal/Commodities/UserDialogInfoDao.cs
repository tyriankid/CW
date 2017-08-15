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
    public class UserDialogInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加用户消息记录列表
        /// </summary>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddUserDialogInfo(UserDialogInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into CW_UserDialogInfo(DialogID,FQUserId,JSUserId,CreateTime,DialogURL,RoomNum,RoomType) values (@DialogID,@FQUserId,@JSUserId,@CreateTime,@DialogURL,@RoomNum,@RoomType)");
            this.database.AddInParameter(sqlStringCommand, "DialogID", DbType.Guid, info.DialogID);
            this.database.AddInParameter(sqlStringCommand, "FQUserId", DbType.Int32, info.FQUserId);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "DialogURL", DbType.String, info.DialogURL);
            this.database.AddInParameter(sqlStringCommand, "RoomNum", DbType.String, info.RoomNum);
            this.database.AddInParameter(sqlStringCommand, "RoomType", DbType.Int32, info.RoomType);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改用户消息记录列表
        /// </summary>
        public bool UpdateUserDialogInfo(UserDialogInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update CW_UserDialogInfo set FQUserId=@FQUserId,JSUserId=@JSUserId,CreateTime=@CreateTime,DialogURL=@DialogURL,RoomNum=@RoomNum,RoomType=@RoomType where DialogID=@DialogID");
            this.database.AddInParameter(sqlStringCommand, "DialogID", DbType.Guid, info.DialogID);
            this.database.AddInParameter(sqlStringCommand, "FQUserId", DbType.Int32, info.FQUserId);
            this.database.AddInParameter(sqlStringCommand, "JSUserId", DbType.Int32, info.JSUserId);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, info.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "DialogURL", DbType.String, info.DialogURL);
            this.database.AddInParameter(sqlStringCommand, "RoomNum", DbType.String, info.RoomNum);
            this.database.AddInParameter(sqlStringCommand, "RoomType", DbType.Int32, info.RoomType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        public UserDialogInfo GetUserDialogInfo(Guid DialogID)
        {
            UserDialogInfo info = new UserDialogInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from CW_UserDialogInfo where DialogID=@DialogID");
            this.database.AddInParameter(sqlStringCommand, "DialogID", DbType.Guid, DialogID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<UserDialogInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        public DataTable GetUserDialogInfoData(string where = "",int top=0)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select "+(top>0?"top "+top:"")+ "  (case amf.DistributorUserId  when amf.UserId then '店长' else '用户' end)as FQRoleInfo, (case amj.DistributorUserId  when amj.UserId then '店长' else '用户' end)as JSRoleInfo,ud.*,amf.username as FQUserName,amf.userhead as FQUserHead,amf.userid as FQQUserID,amj.username as JSUserName,amj.userhead as JSUserHead,amj.userid as JSSUserId from CW_UserDialogInfo  ud left join aspnet_members amf on ud.FQUserid=amf.userid left join aspnet_members amj on ud.JSUserId = amj.userid " + where + " order by ud.createtime asc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        public bool DeleteGetUserDialogInfo(Guid DialogID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from CW_UserDialogInfo where DialogID=@DialogID");
            this.database.AddInParameter(sqlStringCommand, "DialogID", DbType.Guid, DialogID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
