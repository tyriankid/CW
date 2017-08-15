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
    public class OrderVirtualInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="virtualid">主键ID</param>
        /// <returns></returns>
        public bool DeleteOrderVirtualInfo(int ovid)
        {
            string strSql = "DELETE FROM CW_OrderVirtualInfo WHERE OVId = @OVId;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            this.database.AddInParameter(sqlStringCommand, "OVId", DbType.Int32, ovid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改订单虚拟码关系表
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateOrderVirtualInfo(OrderVirtualInfo ordervirtual)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_OrderVirtualInfo SET OrderId = @OrderId,VirtualId=@VirtualId WHERE OVId = @OVId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, ordervirtual.OrderId);
            this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, ordervirtual.VirtualId);
            this.database.AddInParameter(sqlStringCommand, "OVId", DbType.Int32, ordervirtual.OVId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查询订单虚拟码关系表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectOrderVirtualByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_OrderVirtualInfo ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据订单Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectOrderVirtualByOrderId(string orderid)
        {
            string strSql = string.Format("select * from dbo.CW_OrderVirtualInfo where OrderId = '{0}'", orderid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据订单Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectOvAndPvByOrderId(string orderid)
        {
            string strSql = string.Format("select * from dbo.CW_OrderVirtualInfo as ov left join dbo.CW_ProductVirtualInfo as pv on ov.VirtualId = pv.VirtualId  where OrderId = '{0}'", orderid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        /// <summary>
        /// 添加订单虚拟码关系表
        /// </summary>
        /// <param name="salesnfo">实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddOrderVirtualInfo(OrderVirtualInfo ordervirtual)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_OrderVirtualInfo (OrderId,VirtualId) VALUES (@OrderId,@VirtualId);");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, ordervirtual.OrderId);
            this.database.AddInParameter(sqlStringCommand, "VirtualId", DbType.Int32, ordervirtual.VirtualId);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 虚拟订单分配虚拟码
        /// </summary>
        /// <param name="orderid">订单编码</param>
        /// <param name="productid">商品编码</param>
        /// <param name="skuid">规格编码</param>
        /// <param name="num">购买数量</param>
        /// <returns>bool，是否成功</returns>
        public bool AddOrderVirtualInfo(string orderid, int productid, string skuid, int num)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_CreateOrderVirtual");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderid);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productid);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuid);
            this.database.AddInParameter(storedProcCommand, "Num", DbType.Int32, num);
            return (this.database.ExecuteNonQuery(storedProcCommand) == 1);
        }

        /// <summary>
        /// 根据主键ID得到订单虚拟码实体对象
        /// </summary>
        /// <param name="ovid">主键ID</param>
        /// <returns>实体类对象</returns>
        public OrderVirtualInfo GetOrderVirtualByOvId(int ovid)
        {
            OrderVirtualInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.CW_OrderVirtualInfo WHERE OVId = @OVId");
            this.database.AddInParameter(sqlStringCommand, "OVId", DbType.Int32, ovid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<OrderVirtualInfo>(reader);
                reader.NextResult();
            }
            return info;
        }

        
    }
}
