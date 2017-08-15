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
    public class DistributorStarChangeHelper
    {
      
        /// <summary>
        /// 添加修改星级评分记录
        /// </summary>
        /// <param name="salesnfo">修改星级评分记录实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddDistributorMarks(DistributorStarChangeInfo ChangeInfo)
        {
            bool flag = new DistributorStarChangeDao().AddDistributorStarChange(ChangeInfo);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "修改成功"));
            }
            return flag;
        }
    }

}
