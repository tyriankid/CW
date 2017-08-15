using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
    public class ProductVirtualInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 分页获取虚拟码信息表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListProductVirtualInfo(ListProductVirtualInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("1=1");
            if (!string.IsNullOrEmpty(query.keyword))
            {
                builder.AppendFormat(" and (VirtualCode like '%{0}%' or SkuId = '%{0}%' )", query.keyword);
            }
            if (query.ProductId > 0)
            {
                builder.AppendFormat(" and ProductId='{0}'", query.ProductId);
            }
            if (query.states.HasValue)
            {
                builder.AppendFormat(" and VirtualState= {0}", query.states.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.vw_Hishop_ProductVirtualInfo", "VirtualId", builder.ToString(), "*");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualid">主键ID</param>
        /// <returns></returns>
        public bool DeleteProductVirtualInfo(int virtualid, int virtualState, string skuid)
        {
            string strSql = "DELETE FROM CW_ProductVirtualInfo WHERE VirtualId = @VirtualId;";
            if (virtualState == 0)
                strSql += "UPDATE dbo.Hishop_SKUs SET Stock = Stock-1 WHERE SkuId = @SkuId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            if (virtualState == 0)
            {
                this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, virtualid);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, virtualid);
                this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuid);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改虚拟码信息
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateProductVirtualInfo(ProductVirtualInfo virtualinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_ProductVirtualInfo SET VirtualCode = @VirtualCode,VirtualState=@VirtualState,ProductId=@ProductId,SkuId=@SkuId WHERE VirtualId = @VirtualId");
            this.database.AddInParameter(sqlStringCommand, "VirtualCode", DbType.String, virtualinfo.VirtualCode);
            this.database.AddInParameter(sqlStringCommand, "VirtualState", DbType.Int32, virtualinfo.VirtualState);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, virtualinfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, virtualinfo.SkuId);
            this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, virtualinfo.VirtualId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查询虚拟码信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectProductVirtualByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_ProductVirtualInfo ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据商品Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectProductVirtualByProductId(int productid)
        {
            string strSql = string.Format("select * from dbo.CW_ProductVirtualInfo where ProductId = {0}", productid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 添加虚拟码
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddProductVirtualInfo(ProductVirtualInfo virtualinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_ProductVirtualInfo (VirtualCode,VirtualState,CreateDate,ProductId,SkuId) VALUES (@VirtualCode,@VirtualState,@CreateDate,@ProductId,@SkuId); UPDATE dbo.Hishop_SKUs SET Stock = Stock+1 WHERE SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "VirtualCode", DbType.String, virtualinfo.VirtualCode);
            this.database.AddInParameter(sqlStringCommand, "VirtualState", DbType.Int32, virtualinfo.VirtualState);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, virtualinfo.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, virtualinfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, virtualinfo.SkuId);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据主键ID得到虚拟码实体对象
        /// </summary>
        /// <param name="dsid">主键ID</param>
        /// <returns>实体类对象</returns>
        public ProductVirtualInfo GetProductVirtualByDsID(int virtualid)
        {
            ProductVirtualInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.CW_ProductVirtualInfo WHERE VirtualId = @VirtualId");
            this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, virtualid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ProductVirtualInfo>(reader);
                reader.NextResult();
            }
            return info;
        }

        
    }
}
