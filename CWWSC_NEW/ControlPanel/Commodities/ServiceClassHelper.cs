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
    public class ServiceClassHelper
    {
        /// <summary>
        /// 分页获取所有服务品类
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetListServiceClass(ListServiceClassQuery query)
        {
            return new ServiceClassDao().GetListServiceClass(query);
        }

        /// <summary>
        /// 根据主键删除品类信息
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns></returns>
        public static bool DeleteServiceClass(int ScID)
        {
            return new ServiceClassDao().DeleteServiceClass(ScID);
        }

        /// <summary>
        /// 修改品类信息
        /// </summary>
        /// <param name="classinfo">数据实体对象</param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateServiceClass(ServiceClass classinfo)
        {
            return new ServiceClassDao().UpdateServiceClass(classinfo);
        }

        /// <summary>
        /// 根据添加查询品类信息
        /// </summary>
        /// <param name="where">添加</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectClassByWhere(string where = "")
        {
            return new ServiceClassDao().SelectClassByWhere(where);
        }

        /// <summary>
        /// 添加新品类
        /// </summary>
        /// <param name="salesnfo">品类实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddServiceClass(ServiceClass classinfo)
        {
            return new ServiceClassDao().AddServiceClass(classinfo);
        }

        /// <summary>
        /// 根据品类主键ID得到品类实体对象
        /// </summary>
        /// <param name="dsid">品类主键ID</param>
        /// <returns>品类实体</returns>
        public static ServiceClass GetClassByDsID(int scid)
        {
            return new ServiceClassDao().GetClassByDsID(scid);
        }
        
        
        /// <summary>
        /// 根据主键修改排序号
        /// </summary>
        /// <param name="scid">主键ID</param>
        /// <param name="scode">排序号</param>
        /// <returns></returns>
        public static bool UpdateClassScode(int scid, int scode)
        {
            return new ServiceClassDao().UpdateClassScode(scid, scode);
        }

        public static string GetClassNameById(string scids)
        {
            DataTable dt = SelectClassByWhere(string.Format("ScID in ({0})", scids.TrimEnd(',')));
            string strResult = string.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    strResult += dr["ClassName"].ToString() + ",";
                }
            }
            return strResult.TrimEnd(',');
        }

    }

}
