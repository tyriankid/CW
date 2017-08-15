using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
    public class OrderVirtualInfoHelper
    {
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="virtualid">主键ID</param>
        /// <returns></returns>
        public static bool DeleteOrderVirtualInfo(int ovid)
        {
            return new OrderVirtualInfoDao().DeleteOrderVirtualInfo(ovid);
        }

        /// <summary>
        /// 修改订单虚拟码关系表
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateOrderVirtualInfo(OrderVirtualInfo ordervirtual)
        {
            return new OrderVirtualInfoDao().UpdateOrderVirtualInfo(ordervirtual);
        }

        /// <summary>
        /// 根据条件查询订单虚拟码关系表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectOrderVirtualByWhere(string where)
        {
            return new OrderVirtualInfoDao().SelectOrderVirtualByWhere(where);
        }

        /// <summary>
        /// 根据订单Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectOrderVirtualByOrderId(string orderid)
        {
            return new OrderVirtualInfoDao().SelectOrderVirtualByOrderId(orderid);
        }

        /// <summary>
        /// 添加订单虚拟码关系表
        /// </summary>
        /// <param name="salesnfo">实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddOrderVirtualInfo(OrderVirtualInfo ordervirtual)
        {
            return new OrderVirtualInfoDao().AddOrderVirtualInfo(ordervirtual);
        }

        /// <summary>
        /// 根据主键ID得到订单虚拟码实体对象
        /// </summary>
        /// <param name="ovid">主键ID</param>
        /// <returns>实体类对象</returns>
        public static OrderVirtualInfo GetOrderVirtualByOvId(int ovid)
        {
            return new OrderVirtualInfoDao().GetOrderVirtualByOvId(ovid);
        }

        /// <summary>
        /// 虚拟订单分配虚拟码
        /// </summary>
        /// <param name="orderid">订单编码</param>
        /// <param name="productid">商品编码</param>
        /// <param name="skuid">规格编码</param>
        /// <param name="num">购买数量</param>
        /// <returns>bool，是否成功</returns>
        public static void AddOrderVirtualInfo(string orderid, int productid, string skuid, int num)
        {
            new OrderVirtualInfoDao().AddOrderVirtualInfo(orderid, productid, skuid, num);
            new OrderDao().UpdateItemsStatus(orderid, 5, "'" + skuid + "'");
        }

        /// <summary>
        /// 根据订单Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectOvAndPvByOrderId(string orderid)
        {
            return new OrderVirtualInfoDao().SelectOvAndPvByOrderId(orderid);
        }

    }

}
