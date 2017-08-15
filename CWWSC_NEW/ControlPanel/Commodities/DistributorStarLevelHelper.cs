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
    public class DistributorStarLevelHelper
    {

        /// <summary>
        /// 添加星级评分级别
        /// </summary>
        /// <param name="salesnfo">星级评分级别实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddDistributorMarks(DistributorStarLevelInfo StarLevelInfo)
        {
            bool flag = new DistributorStarLevelDao().AddDistributorStarLevel(StarLevelInfo);
            return flag;
        }
        public static DistributorStarLevelInfo GetDistributorStarLevelInfo(string StarLevelID)
        {
            return new DistributorStarLevelDao().GetDistributorStarLevelInfo(StarLevelID);
        }
        public static DataTable GetDistributorStarLevelData(string where = "")
        {
            return new DistributorStarLevelDao().GetDistributorStarLevelData(where);
        }
        public static bool DeleteDistributorStarLevel(string StarLevelID)
        {
            return new DistributorStarLevelDao().DeleteDistributorStarLevel(StarLevelID);
        }
        /// <summary>
        /// 修改星级评分级别
        /// </summary>
        /// <param name="salesnfo">星级评分级别实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateDistributorStarLevel(DistributorStarLevelInfo StarLevelInfo)
        {
            bool flag = new DistributorStarLevelDao().UpdateDistributorStarLevel(StarLevelInfo);
            return flag;
        }

    }

}
