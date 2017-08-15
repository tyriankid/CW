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
    public class ServiceClassDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 分页获取所有服务品类
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListServiceClass(ListServiceClassQuery query)
        {
            string sqlwhere = string.IsNullOrEmpty(query.ClassName) ? string.Empty : string.Format("ClassName LIKE '%{0}%')", DataHelper.CleanSearchString(query.ClassName));
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.CW_ServiceClass", "ScID", sqlwhere, "*");
        }

        /// <summary>
        /// 根据主键删除品类信息
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public bool DeleteServiceClass(int ScID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_ServiceClass WHERE ScID = @ScID");
            this.database.AddInParameter(sqlStringCommand, "ScID", DbType.Int32, ScID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改品类信息
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateServiceClass(ServiceClass classinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_ServiceClass SET ClassName = @ClassName, Scode = @Scode WHERE ScID = @ScID");
            this.database.AddInParameter(sqlStringCommand, "ScID", DbType.Int32, classinfo.ScID);
            this.database.AddInParameter(sqlStringCommand, "ClassName", DbType.String, classinfo.ClassName);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.Int32, classinfo.Scode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据添加查询品类信息
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectClassByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_ServiceClass ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 添加新品类
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddServiceClass(ServiceClass classinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_ServiceClass (ClassName,Scode) VALUES (@ClassName,@Scode)");
            this.database.AddInParameter(sqlStringCommand, "ClassName", DbType.String, classinfo.ClassName);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.Int32, classinfo.Scode);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据品类主键ID得到品类实体对象
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns>品类实体</returns>
        public ServiceClass GetClassByDsID(int scid)
        {
            ServiceClass info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.CW_ServiceClass WHERE ScID = @ScID");
            this.database.AddInParameter(sqlStringCommand, "ScID", DbType.Int32, scid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ServiceClass>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 根据主键修改排序号
        /// </summary>
        /// <param name="scid">主键ID</param>
        /// <param name="scode">排序号</param>
        /// <returns></returns>
        public bool UpdateClassScode(int scid, int scode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update CW_ServiceClass set Scode=@Scode where ScID=@ScID");
            this.database.AddInParameter(sqlStringCommand, "@Scode", DbType.Int32, scode);
            this.database.AddInParameter(sqlStringCommand, "@ScID", DbType.Int32, scid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        ///// <summary>
        ///// 根据添加查询品类信息
        ///// </summary>
        ///// <param name="where">添加</param>
        ///// <returns>DataTable数据表</returns>
        //public DataTable SelectClassByWhere(string where)
        //{
        //    string strSql = string.Format("select * FROM CW_ServiceClass ");
        //    if (!string.IsNullOrEmpty(where))
        //    {
        //        strSql += " where " + where;
        //    }
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
        //    return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        //}

    }
}
