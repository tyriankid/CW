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
    public class OrderSalesDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        ///// <summary>
        ///// 分页获取所有服务品类
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public DbQueryResult GetListServiceClass(ListServiceClassQuery query)
        //{
        //    string sqlwhere = string.IsNullOrEmpty(query.ClassName) ? string.Empty : string.Format("ClassName LIKE '%{0}%')", DataHelper.CleanSearchString(query.ClassName));
        //    return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.CW_ServiceClass", "ScID", sqlwhere, "*");
        //}

        /// <summary>
        /// 根据主键删除订单服务人员关系表数据
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public bool DeleteOrderSales(int OsId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_OrderSales WHERE OsId = @OsId");
            this.database.AddInParameter(sqlStringCommand, "OsId", DbType.Int32, OsId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 修改订单服务人员关系表数据
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public bool UpdateOrderSales(OrderSales osalesinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_OrderSales SET OrderId = @OrderId, State = @State, ServiceSalesId = @ServiceSalesId, CreateDate = @CreateDate, RefuseRemark = @RefuseRemark WHERE OsId = @OsId");
            this.database.AddInParameter(sqlStringCommand, "OsId", DbType.Int32, osalesinfo.OsId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, osalesinfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, osalesinfo.State);
            this.database.AddInParameter(sqlStringCommand, "ServiceSalesId", DbType.Guid, osalesinfo.ServiceSalesId);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, osalesinfo.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "RefuseRemark", DbType.String, osalesinfo.RefuseRemark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查询订单服务人员关系表数据
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectClassByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_OrderSales ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 添加订单服务人员关系表数据
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddOrderSales(OrderSales osalesinfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_OrderSales (OrderId,State,ServiceSalesId,CreateDate,RefuseRemark) VALUES (@OrderId,@State,@ServiceSalesId,@CreateDate,@RefuseRemark); UPDATE ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, osalesinfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, osalesinfo.State);
            this.database.AddInParameter(sqlStringCommand, "ServiceSalesId", DbType.Guid, osalesinfo.ServiceSalesId);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, osalesinfo.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "RefuseRemark", DbType.String, osalesinfo.RefuseRemark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 添加订单服务人员关系表数据
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddOrderSales(OrderSales osalesinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_OrderSales (OrderId,State,ServiceSalesId,CreateDate,RefuseRemark) VALUES (@OrderId,@State,@ServiceSalesId,@CreateDate,@RefuseRemark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, osalesinfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, osalesinfo.State);
            this.database.AddInParameter(sqlStringCommand, "ServiceSalesId", DbType.Guid, osalesinfo.ServiceSalesId);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, osalesinfo.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "RefuseRemark", DbType.String, osalesinfo.RefuseRemark);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        /// <summary>
        /// 根据主键ID得到订单服务人员关系表实体对象
        /// </summary>
        /// <param name="osid">主键ID</param>
        /// <returns>实体</returns>
        public OrderSales GetOrderSalesByOsId(int osid)
        {
            OrderSales info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM dbo.CW_OrderSales WHERE OsId = @OsId");
            this.database.AddInParameter(sqlStringCommand, "OsId", DbType.Int32, osid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<OrderSales>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 查看订单服务人员情况
        /// </summary>
        /// <param name="orderid">订单编码</param>
        /// <returns>数据表</returns>
        public DataTable SelectServiceOrderSales(string orderid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"select os.*,ds.*,m.UserName,m.UserHead,d.StoreName,d.Location_poiaddress,d.Location_poiname,d.Location_cityname,o.serviceCode from dbo.CW_OrderSales as os 
                    left join dbo.aspnet_DistributorSales as ds on os.ServiceSalesId = ds.DsID 
                    left join dbo.aspnet_Members as m on ds.SaleUserId = m.UserId 
                    left join dbo.Hishop_Orders as o on os.OrderId = o.OrderId 
                    left join dbo.aspnet_Distributors as d on d.UserId = o.serviceUserId  where os.OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 修改服务订单状态为已配单
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="orderstatus"></param>
        /// <param name="opdatetime"></param>
        /// <param name="dbTran"></param>
        /// <returns></returns>
        public bool UpdateServiceOrderSales(string orderid, int orderstatus, Guid servicesalesid, DateTime opdatetime, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_Orders Set OrderStatus = @OrderStatus,ShippingDate = @ShippingDate, serviceSalesId = @serviceSalesId Where OrderId = @OrderId;Update Hishop_OrderItems Set OrderItemsStatus=@OrderStatus Where OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, orderstatus);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, opdatetime);
            this.database.AddInParameter(sqlStringCommand, "serviceSalesId", DbType.Guid, servicesalesid);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

    }
}
