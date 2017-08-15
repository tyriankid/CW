using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
    public class OrderSalesHelper
    {
        /// <summary>
        /// 根据主键删除订单服务人员关系表数据
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public static bool DeleteOrderSales(int OsId)
        {
            return new OrderSalesDao().DeleteOrderSales(OsId);
        }

        /// <summary>
        /// 修改订单服务人员关系表数据
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateOrderSales(OrderSales osalesinfo)
        {
            return new OrderSalesDao().UpdateOrderSales(osalesinfo);
        }

        /// <summary>
        /// 根据条件查询订单服务人员关系表数据
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectClassByWhere(string where)
        {
            return new OrderSalesDao().SelectClassByWhere(where);
        }

        /// <summary>
        /// 添加订单服务人员关系表数据
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddOrderSales(OrderSales osalesinfo)
        {
            return new OrderSalesDao().AddOrderSales(osalesinfo);
        }

        /// <summary>
        /// 添加订单服务人员关系表数据, 关联表修改,2017-07-27新建
        /// </summary>
        /// <param name="osalesinfo"></param>
        /// <returns></returns>
        public static bool OrderSelectSales(OrderSales osalesinfo)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderSalesDao salesdao = new OrderSalesDao();
                    if (!salesdao.AddOrderSales(osalesinfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    DateTime optime = DateTime.Now;
                    int status = (int)OrderStatus.SellerAlreadySent;
                    if (!salesdao.UpdateServiceOrderSales(osalesinfo.OrderId, status, osalesinfo.ServiceSalesId, optime, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        /// <summary>
        /// 根据主键ID得到订单服务人员关系表实体对象
        /// </summary>
        /// <param name="osid">主键ID</param>
        /// <returns>实体</returns>
        public static OrderSales GetOrderSalesByOsId(int osid)
        {
            return new OrderSalesDao().GetOrderSalesByOsId(osid);
        }
        
        /// <summary>
        /// 查看订单服务人员情况
        /// </summary>
        /// <param name="orderid">订单编码</param>
        /// <returns>数据表</returns>
        public static DataTable SelectServiceOrderSales(string orderid)
        {
            return new OrderSalesDao().SelectServiceOrderSales(orderid);
        }

    }

}
