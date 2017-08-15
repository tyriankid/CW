using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
    public class ProductVirtualInfoHelper
    {
        /// <summary>
        /// 分页获取虚拟码信息表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetListProductVirtualInfo(ListProductVirtualInfoQuery query)
        {
            return new ProductVirtualInfoDao().GetListProductVirtualInfo(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualid">主键ID</param>
        /// <returns></returns>
        public static bool DeleteProductVirtualInfo(int virtualid, int virtualState, string skuid)
        {
            return new ProductVirtualInfoDao().DeleteProductVirtualInfo(virtualid, virtualState, skuid);
        }

        /// <summary>
        /// 修改虚拟码信息
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateProductVirtualInfo(ProductVirtualInfo virtualinfo)
        {
            return new ProductVirtualInfoDao().UpdateProductVirtualInfo(virtualinfo);
        }

        /// <summary>
        /// 根据条件查询虚拟码信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectProductVirtualByWhere(string where)
        {
            return new ProductVirtualInfoDao().SelectProductVirtualByWhere(where);
        }

        /// <summary>
        /// 根据商品Id查询虚拟码信息
        /// </summary>
        /// <param name="disuserid">商品ID</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectProductVirtualByProductId(int productid)
        {
            return new ProductVirtualInfoDao().SelectProductVirtualByProductId(productid);
        }

        /// <summary>
        /// 添加虚拟码
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddProductVirtualInfo(ProductVirtualInfo virtualinfo)
        {
            return new ProductVirtualInfoDao().AddProductVirtualInfo(virtualinfo);
        }

        /// <summary>
        /// 根据主键ID得到虚拟码实体对象
        /// </summary>
        /// <param name="dsid">主键ID</param>
        /// <returns>实体类对象</returns>
        public static ProductVirtualInfo GetProductVirtualByDsID(int virtualid)
        {
            return new ProductVirtualInfoDao().GetProductVirtualByDsID(virtualid);
        }

    }

}
