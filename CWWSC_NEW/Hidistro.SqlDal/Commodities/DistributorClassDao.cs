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
    public class DistributorClassDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 获取服务门店申请表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListDistributorClass(ListDistributorClassQuery query)
        {
            string sqlwhere = query.DisUserId > 0 ? string.Format("DisUserId = {0} ", query.DisUserId) : string.Empty;
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.aspnet_DistributorClass", "DcID", sqlwhere, "*");
        }

        /// <summary>
        /// 根据主键删除服务门店申请表
        /// </summary>
        /// <param name="dsid">申请表主键ID</param>
        /// <returns></returns>
        public bool DeleteDistributorClass(Guid DcID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_DistributorClass WHERE DcID = @DcID");
            this.database.AddInParameter(sqlStringCommand, "DcID", DbType.Guid, DcID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据主键删除服务门店申请表
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public bool DeleteDistributorClassByUserId(int DisUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_DistributorClass WHERE DisUserId = @DisUserId");
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, DisUserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改服务门店申请表
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateDistributorClass(DistributorClass classinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorClass SET DisUserId = @DisUserId, State = @State, ApplyDate = @ApplyDate, ScIDs = @ScIDs, AuditDate = @AuditDate, AuditRemark = @AuditRemark, RegionId=@RegionId, RegionName=@RegionName WHERE DcID = @DcID");
            this.database.AddInParameter(sqlStringCommand, "DcID", DbType.Guid, classinfo.DcID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, classinfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, classinfo.State);
            if (classinfo.ApplyDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, classinfo.ApplyDate);
            else
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "ScIDs", DbType.String, classinfo.ScIDs);
            if (classinfo.AuditDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, classinfo.AuditDate);
            else
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "AuditRemark", DbType.String, classinfo.AuditRemark);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, classinfo.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, classinfo.RegionName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改服务门店申请表
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateDistributorClassEx(DistributorClass classinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorClass set State = 2,AuditRemark='审核通过新申请其它申请自动不通过' WHERE DisUserId = @DisUserId;UPDATE aspnet_DistributorClass SET DisUserId = @DisUserId, State = @State, ApplyDate = @ApplyDate, ScIDs = @ScIDs, AuditDate = @AuditDate, AuditRemark = @AuditRemark, RegionId=@RegionId, RegionName=@RegionName WHERE DcID = @DcID");
            this.database.AddInParameter(sqlStringCommand, "DcID", DbType.Guid, classinfo.DcID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, classinfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, classinfo.State);
            if (classinfo.ApplyDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, classinfo.ApplyDate);
            else
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "ScIDs", DbType.String, classinfo.ScIDs);
            if (classinfo.AuditDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, classinfo.AuditDate);
            else
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "AuditRemark", DbType.String, classinfo.AuditRemark);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, classinfo.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, classinfo.RegionName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查询服务门店申请表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectClassByWhere(string where)
        {
            string strSql = string.Format("select * FROM aspnet_DistributorClass ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 添加服务门店申请表
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddDistributorClass(DistributorClass classinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorClass (DcID,DisUserId,State,ApplyDate,ScIDs,AuditDate,AuditRemark,RegionId,RegionName) VALUES (@DcID,@DisUserId,@State,@ApplyDate,@ScIDs,@AuditDate,@AuditRemark,@RegionId,@RegionName)");
            this.database.AddInParameter(sqlStringCommand, "DcID", DbType.Guid, classinfo.DcID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, classinfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, classinfo.State);
            if (classinfo.ApplyDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, classinfo.ApplyDate);
            else
                this.database.AddInParameter(sqlStringCommand, "ApplyDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "ScIDs", DbType.String, classinfo.ScIDs);
            if(classinfo.AuditDate > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, classinfo.AuditDate);
            else
                this.database.AddInParameter(sqlStringCommand, "AuditDate", DbType.Date, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "AuditRemark", DbType.String, classinfo.AuditRemark);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, classinfo.RegionId);
            this.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, classinfo.RegionName);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据主键ID得到服务门店申请实体对象
        /// </summary>
        /// <param name="dsid">主键ID</param>
        /// <returns>服务门店申请实体</returns>
        public DistributorClass GetDistributorClassByDcID(Guid dcid)
        {
            DistributorClass info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.aspnet_DistributorClass WHERE DcID = @DcID");
            this.database.AddInParameter(sqlStringCommand, "DcID", DbType.Guid, dcid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<DistributorClass>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 根据条件查询服务门店
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectDistributorAndClassByWhere(string where)
        {
            string strSql = string.Format("select * FROM dbo.vm_Hishop_DistributorClassAndRegion ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

    }
}
