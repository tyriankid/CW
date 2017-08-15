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
    public class MemberTagsDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加会员标签
        /// </summary>
        /// <param name="tagInfo">会员标签实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddmemberTags(memberTagsInfo tagInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Tags (TagType,TagName,Scode) VALUES (@TagType,@TagName,@Scode)");
            this.database.AddInParameter(sqlStringCommand, "TagType", DbType.Int32, tagInfo.TagType);
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagInfo.TagName);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.String, tagInfo.Scode);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改会员标签
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateMemberTags(memberTagsInfo tagInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Tags SET TagType = @TagType,TagName=@TagName,Scode=@Scode where TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagInfo.TagID);
            this.database.AddInParameter(sqlStringCommand, "TagType", DbType.Int32, tagInfo.TagType);
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagInfo.TagName);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.String, tagInfo.Scode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public memberTagsInfo GetMemberTagsInfo(int TagID)
        {
            memberTagsInfo info = new memberTagsInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from aspnet_Tags where TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, TagID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<memberTagsInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="displaysequence"></param>
        /// <returns></returns>
        public bool updateSort(int TagID, int sort)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update aspnet_Tags  set Scode=@Scode where TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "@Scode", DbType.Int32, sort);
            this.database.AddInParameter(sqlStringCommand, "@TagID", DbType.Int32, TagID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetmemberTagsData(string where = "")
        {
            List<memberTagsInfo> list = new List<memberTagsInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from aspnet_Tags " + where + "");
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
        public bool DeleteMemberTags(int TagID)
        {
            List<memberTagsInfo> list = new List<memberTagsInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete aspnet_Tags where TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, TagID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
