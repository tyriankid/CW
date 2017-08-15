using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
    public class memberTagsRelationDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加会员标签关系
        /// </summary>
        /// <param name="tagInfo">会员标签实体关系对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddmemberTags(memberTagsRelationInfo tagInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_MemberTags (UserType,UserId,TagID,CreateTime) VALUES (@UserType,@UserId,@TagID,@CreateTime)");
            this.database.AddInParameter(sqlStringCommand, "UserType", DbType.Int32, tagInfo.UserType);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, tagInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagInfo.TagID);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, tagInfo.CreateTime);

            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改会员标签关系
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateMemberTags(memberTagsRelationInfo tagInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberTags SET UserType = @UserType,UserId=@UserId,TagID=@TagID,CreateTime=@CreateTime where MtID=@MtID");
            this.database.AddInParameter(sqlStringCommand, "MtID", DbType.Int32, tagInfo.MtID);
            this.database.AddInParameter(sqlStringCommand, "UserType", DbType.Int32, tagInfo.UserType);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, tagInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagInfo.TagID);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, tagInfo.CreateTime);

            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public memberTagsRelationInfo GetMemberTagsInfo(int MtID)
        {
            memberTagsRelationInfo info = new memberTagsRelationInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from aspnet_MemberTags where MtID=@MtID");
            this.database.AddInParameter(sqlStringCommand, "MtID", DbType.Int32, MtID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<memberTagsRelationInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetmemberTagsData(string where)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from aspnet_MemberTags " + where + "");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int  GetmemberTagsNum(string userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select count(UserId) from aspnet_MemberTags  where userId={0}", userId));
            return (int)this.database.ExecuteScalar(sqlStringCommand);
          
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteMemberTags(int MtID)
        {
            List<memberTagsRelationInfo> list = new List<memberTagsRelationInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete aspnet_MemberTags where MtID=@MtID");
            this.database.AddInParameter(sqlStringCommand, "MtID", DbType.Int32, MtID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteMemberTagsWhere(string where)
        {
            List<memberTagsRelationInfo> list = new List<memberTagsRelationInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete aspnet_MemberTags "+where+"");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
