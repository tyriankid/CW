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
    public class DistributorMarkHelper
    {
      
        /// <summary>
        /// 添加星级评分
        /// </summary>
        /// <param name="salesnfo">星级评分实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddDistributorMarks(DistributorMarkInfo salesnfo)
        {
            bool flag = new DistributorMarkDao().AddDistributorMarks(salesnfo);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorSales, string.Format(CultureInfo.InvariantCulture, "评分成功"));
            }
            return flag;
        }
    }

}
