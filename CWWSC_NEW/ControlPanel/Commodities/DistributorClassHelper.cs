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
    public class DistributorClassHelper
    {
        /// <summary>
        /// 分页获取所有服务门店申请
        /// </summary>
        /// <param name="query">条件对象</param>
        /// <returns>分页相关数据对象</returns>
        public static DbQueryResult GetListDistributorClass(ListDistributorClassQuery query)
        {
            return new DistributorClassDao().GetListDistributorClass(query);
        }

        /// <summary>
        /// 根据主键删除服务门店申请表
        /// </summary>
        /// <param name="dsid">申请表主键ID</param>
        /// <returns></returns>
        public static bool DeleteDistributorClass(Guid DcID)
        {
            return new DistributorClassDao().DeleteDistributorClass(DcID);
        }

        /// <summary>
        /// 根据主键删除服务门店申请表
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public static bool DeleteDistributorClassByUserId(int DisUserId)
        {
            return new DistributorClassDao().DeleteDistributorClassByUserId(DisUserId);
        }

        /// <summary>
        /// 修改服务门店申请表
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateDistributorClass(DistributorClass classinfo)
        {
            return new DistributorClassDao().UpdateDistributorClass(classinfo);
        }

        /// <summary>
        /// 修改服务门店申请表
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateDistributorClassEx(DistributorClass classinfo)
        {
            return new DistributorClassDao().UpdateDistributorClassEx(classinfo);
        }

        /// <summary>
        /// 根据条件查询服务门店申请表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectClassByWhere(string where)
        {
            return new DistributorClassDao().SelectClassByWhere(where);
        }

        /// <summary>
        /// 添加服务门店申请表
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddDistributorClass(DistributorClass classinfo)
        {
            return new DistributorClassDao().AddDistributorClass(classinfo);
        }

        /// <summary>
        /// 根据主键ID得到服务门店申请实体对象
        /// </summary>
        /// <param name="dsid">主键ID</param>
        /// <returns>服务门店申请实体</returns>
        public static DistributorClass GetDistributorClassByDcID(Guid dcid)
        {
            return new DistributorClassDao().GetDistributorClassByDcID(dcid);
        }

        /// <summary>
        /// 根据条件查询服务门店
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectDistributorAndClassByWhere(string where)
        {
            return new DistributorClassDao().SelectDistributorAndClassByWhere(where);
        }
    }

}
