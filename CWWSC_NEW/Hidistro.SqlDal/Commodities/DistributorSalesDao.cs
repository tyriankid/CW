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
    public class DistributorSalesDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 分页获取所有店员
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// 修改 (查询条件错误) 2017-7-12 yk
        /// 原： sqlwhere += query.DisUserId <= 0 ? string.Empty : (string.IsNullOrEmpty(sqlwhere) ? "": "and" + string.Format("DisUserId = '{0}'", query.DisUserId));
        /// 改： sqlwhere += query.DisUserId <= 0 ? string.Empty : (string.IsNullOrEmpty(sqlwhere) ? string.Format("DisUserId = '{0}'", query.DisUserId): "and" + string.Format("DisUserId = '{0}'", query.DisUserId));
        public DbQueryResult GetListDistributorSales(ListDistributorSalesQuery query)
        {
            string sqlwhere = string.IsNullOrEmpty(query.KeyValue) ? string.Empty : string.Format("(DsName LIKE '%{0}%' or DsPhone LIKE '%{0}%')", DataHelper.CleanSearchString(query.KeyValue));
            sqlwhere += query.DisUserId <= 0 ? string.Empty : (string.IsNullOrEmpty(sqlwhere) ? string.Format("DisUserId = '{0}'", query.DisUserId): "and" + string.Format("DisUserId = '{0}'", query.DisUserId));
            //return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.vm_Hishop_DistributorSales", "DsID", sqlwhere, "*");
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.vm_Hishop_DistributorSales", "DsID", sqlwhere, "*");
        }

        /// <summary>
        /// 根据主键删除店员信息
        /// </summary>
        /// <param name="dsid">店员主键ID</param>
        /// <returns></returns>
        public bool DeleteDistributorSales(Guid dsid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_DistributorSales WHERE DsID = @DsID");
            this.database.AddInParameter(sqlStringCommand, "DsID", DbType.Guid, dsid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改店员信息
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateDistributorSales(DistributorSales salesnfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorSales SET DisUserId = @DisUserId,DsType=@DsType, SaleUserId = @SaleUserId ,DsName=@DsName,DsPhone=@DsPhone,IsStart=@IsStart,IsRz=@IsRz,RzTime=@RzTime,IsStoreManager=@IsStoreManager,Scode=@Scode WHERE DsID = @DsID");
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, salesnfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "SaleUserId", DbType.Int32, salesnfo.SaleUserId);
            this.database.AddInParameter(sqlStringCommand, "DsName", DbType.String, salesnfo.DsName);
            this.database.AddInParameter(sqlStringCommand, "DsPhone", DbType.String, salesnfo.DsPhone);
            this.database.AddInParameter(sqlStringCommand, "DsType", DbType.String, salesnfo.DsType);
            if (salesnfo.IsStart > 0)
                this.database.AddInParameter(sqlStringCommand, "IsStart", DbType.Int32, salesnfo.IsStart);
            else
                this.database.AddInParameter(sqlStringCommand, "IsStart", DbType.Int32, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "IsRz", DbType.Int32, salesnfo.IsRz);
            if (salesnfo.RzTime > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "RzTime", DbType.DateTime, salesnfo.RzTime);
            else
                this.database.AddInParameter(sqlStringCommand, "RzTime", DbType.DateTime, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "IsStoreManager", DbType.Int32, salesnfo.IsStoreManager);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.String, salesnfo.Scode);
            this.database.AddInParameter(sqlStringCommand, "DsID", DbType.Guid, salesnfo.DsID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据添加查询店员信息
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectSalesInfoByWhere(string where)
        {
            string strSql = string.Format("select * FROM aspnet_DistributorSales ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据门店Id查询店员信息
        /// </summary>
        /// <param name="disuserid">门店ID</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectSalesByDisUserId(int disuserid)
        {
            string strSql = string.Format("select * from dbo.aspnet_DistributorSales where DisUserId = {0}", disuserid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 添加新店员
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddDistributorSales(DistributorSales salesnfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorSales (DsID,DisUserId,SaleUserId,DsName,DsPhone,IsStart,IsRz,RzTime,IsStoreManager,Scode,DsType) VALUES (@DsID,@DisUserId,@SaleUserId,@DsName,@DsPhone,@IsStart,@IsRz,@RzTime,@IsStoreManager,@Scode,@DsType)");
            this.database.AddInParameter(sqlStringCommand, "DsID", DbType.Guid, salesnfo.DsID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, salesnfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "SaleUserId", DbType.Int32, salesnfo.SaleUserId);
            this.database.AddInParameter(sqlStringCommand, "DsName", DbType.String, salesnfo.DsName);
            this.database.AddInParameter(sqlStringCommand, "DsPhone", DbType.String, salesnfo.DsPhone);
            this.database.AddInParameter(sqlStringCommand, "DsType", DbType.String, salesnfo.DsType);
            if (salesnfo.IsStart > 0)
                this.database.AddInParameter(sqlStringCommand, "IsStart", DbType.Int32, salesnfo.IsStart);
            else
                this.database.AddInParameter(sqlStringCommand, "IsStart", DbType.Int32, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "IsRz", DbType.Int32, salesnfo.IsRz);
            if (salesnfo.RzTime > DateTime.MinValue)
                this.database.AddInParameter(sqlStringCommand, "RzTime", DbType.DateTime, salesnfo.RzTime);
            else
                this.database.AddInParameter(sqlStringCommand, "RzTime", DbType.DateTime, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "IsStoreManager", DbType.Int32, salesnfo.IsStoreManager);
            this.database.AddInParameter(sqlStringCommand, "Scode", DbType.String, salesnfo.Scode);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据店员主键ID得到店员实体对象
        /// </summary>
        /// <param name="dsid">店员表主键ID</param>
        /// <returns>店员实体</returns>
        public DistributorSales GetSalesByDsID(Guid dsid)
        {
            DistributorSales info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.vm_Hishop_DistributorSales WHERE DsID = @DsID");
            this.database.AddInParameter(sqlStringCommand, "DsID", DbType.Guid, dsid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<DistributorSales>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 根据前端微信用户ID得到店员实体对象
        /// </summary>
        /// <param name="saleuserid">微信用户Id</param>
        /// <returns>店员实体</returns>
        public DistributorSales GetSalesBySaleUserId(int saleuserid)
        {
            DistributorSales info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vm_Hishop_DistributorSales WHERE SaleUserId = @SaleUserId");
            this.database.AddInParameter(sqlStringCommand, "SaleUserId", DbType.Int32, saleuserid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<DistributorSales>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 店员认证验证
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>实体对象存在则验证成功，不存在则验证失败。</returns>
        public DistributorSales RzSales(DistributorSales salesnfo)
        {
            DistributorSales info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.vm_Hishop_DistributorSales WHERE accountALLHere = @accountALLHere AND DsName=@DsName AND DsPhone= @DsPhone and DsType=@DsType");
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, salesnfo.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "DsName", DbType.String, salesnfo.DsName);
            this.database.AddInParameter(sqlStringCommand, "DsPhone", DbType.String, salesnfo.DsPhone);
            this.database.AddInParameter(sqlStringCommand, "DsType", DbType.String, salesnfo.DsType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<DistributorSales>(reader);
                reader.NextResult();
            }
            return info;

            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select * from dbo.vm_Hishop_DistributorSales where accountALLHere = @accountALLHere and DsName = @DsName and DsPhone = @DsPhone");
            //this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, salesnfo.accountALLHere);
            //this.database.AddInParameter(sqlStringCommand, "DsName", DbType.String, salesnfo.DsName);
            //this.database.AddInParameter(sqlStringCommand, "DsPhone", DbType.String, salesnfo.DsPhone);
            //return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 认证成功后绑定前端用户与店员
        /// </summary>
        /// <param name="clientUserId">前端用户userid</param>
        /// <returns>true绑定成功，false绑定失败</returns>
        public bool RzSuccessToUpdate(DistributorSales salesnfo)
        {
            //修改2处， 1、修改店员信息为认证状态， 2、修改会员表，将认证的会员与认证店员所属的门店关系起来
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"UPDATE dbo.aspnet_DistributorSales SET SaleUserId = @SaleUserId,IsRz=1,RzTime=@RzTime WHERE DsID = @DsID;
                                    update dbo.aspnet_Members set DistributorUserId = @DisUserId where UserId = @SaleUserId");
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, salesnfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "SaleUserId", DbType.Int32, salesnfo.SaleUserId);
            this.database.AddInParameter(sqlStringCommand, "RzTime", DbType.DateTime, salesnfo.RzTime);
            this.database.AddInParameter(sqlStringCommand, "DsID", DbType.Guid, salesnfo.DsID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }


        /// <summary>
        /// 根据门店Id查询店员信息
        /// </summary>
        /// <param name="disuserid">门店ID</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectDistributorsInfo(int userId)
        {
            string strSql = string.Format(@"select * from dbo.aspnet_Distributors as d 
                left join dbo.CW_StoreInfo as s on d.StoreId = s.id   where UserId = {0}", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        /// <summary>
        /// 根据添加查询店员信息
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectSalesUserInfoByWhere(string where)
        {
            string strSql = string.Format("select d.*,m.UserName,m.UserHead FROM aspnet_DistributorSales as d left join dbo.aspnet_Members as m on d.SaleUserId = m.UserId");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

    }
}
