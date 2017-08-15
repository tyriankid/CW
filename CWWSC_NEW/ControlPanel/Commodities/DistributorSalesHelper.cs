using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
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
    public class DistributorSalesHelper
    {
        /// <summary>
        /// 获取所有店员信息，并绑定到前台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult ListDistributorSales(ListDistributorSalesQuery query)
        {
            return new DistributorSalesDao().GetListDistributorSales(query);
        }

        /// <summary>
        /// 根据主键ID删除店员
        /// </summary>
        /// <param name="dsid">店员主键ID</param>
        /// <returns>true成功，false失败</returns>
        public static bool DeleteStoreInfo(Guid dsid)
        {
            bool flag = false;
            DistributorSales info = GetSalesByDsID(dsid);
            if (info != null && info.DsID != Guid.Empty)
            {
                flag = new DistributorSalesDao().DeleteDistributorSales(dsid);
                if (flag)
                {
                    //记录操作记录
                    EventLogs.WriteOperationLog(Privilege.DeleteDistributorSales, string.Format(CultureInfo.InvariantCulture, "删除了名称为”{0}”的店员", new object[] { info.DsName }));
                }
            }
            return flag;
        }

        /// <summary>
        /// 修改店员信息
        /// </summary>
        /// <param name="salesnfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateDistributorSales(DistributorSales salesnfo)
        {
            bool flag = new DistributorSalesDao().UpdateDistributorSales(salesnfo);
            return flag;
        }

        /// <summary>
        /// 根据添加查询店员信息
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectSalesInfoByWhere(string where)
        {
            return new DistributorSalesDao().SelectSalesInfoByWhere(where);
        }

        /// <summary>
        /// 根据添加查询店员信息，关联微会员表查询
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectSalesUserInfoByWhere(string where)
        {
            return new DistributorSalesDao().SelectSalesUserInfoByWhere(where);
        }

        /// <summary>
        /// 根据门店Id查询店员信息
        /// </summary>
        /// <param name="disuserid">门店ID</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectSalesByDisUserId(int disuserid)
        {
            return new DistributorSalesDao().SelectSalesByDisUserId(disuserid);
        }

        /// <summary>
        /// 添加新店员
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddDistributorSales(DistributorSales salesnfo)
        {
            bool flag = new DistributorSalesDao().AddDistributorSales(salesnfo);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "添加了一个新的店员:”{0}”", new object[] { salesnfo.DsName }));
            }
            return flag;
        }

        /// <summary>
        /// 根据店员主键ID得到店员实体对象
        /// </summary>
        /// <param name="dsid">店员表主键ID</param>
        /// <returns>店员实体</returns>
        public static DistributorSales GetSalesByDsID(Guid dsid)
        {
            return new DistributorSalesDao().GetSalesByDsID(dsid);
        }

        /// <summary>
        /// 根据前端微信用户ID得到店员实体对象
        /// </summary>
        /// <param name="saleuserid">微信用户Id</param>
        /// <returns>店员实体</returns>
        public static DistributorSales GetSalesBySaleUserId(int saleuserid)
        {
            return new DistributorSalesDao().GetSalesBySaleUserId(saleuserid);
        }

        /// <summary>
        /// 店员认证验证
        /// </summary>
        /// <param name="salesnfo">店员实体对象</param>
        /// <returns>实体对象存在则验证成功，不存在则验证失败。</returns>
        public static DistributorSales RzSales(DistributorSales salesnfo)
        {
            return new DistributorSalesDao().RzSales(salesnfo);
        }

        /// <summary>
        /// 认证成功后绑定前端用户与店员
        /// </summary>
        /// <param name="clientUserId">前端用户userid</param>
        /// <returns>true绑定成功，false绑定失败</returns>
        public static bool RzSuccessToUpdate(DistributorSales salesnfo)
        {
            return new DistributorSalesDao().RzSuccessToUpdate(salesnfo);
        }

        /// <summary>
        /// 根据门店Id查询店员信息
        /// </summary>
        /// <param name="disuserid">门店ID</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectDistributorsInfo(int userId)
        {
            return new DistributorSalesDao().SelectDistributorsInfo(userId);
        }



    }

}
