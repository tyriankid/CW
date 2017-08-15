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
    public class ProductMaintainRemindHelper
    {

        /// <summary>
        /// 添加星级评分级别
        /// </summary>
        /// <param name="salesnfo">星级评分级别实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddProductMaintainRemind(ProductMaintainRemindInfo MrID)
        {
            bool flag = new ProductMaintenanceReminderDao().AddProductMaintainRemind(MrID);
            return flag;
        }
        public static ProductMaintainRemindInfo GetProductMaintainRemindInfo(int MrID)
        {
            return new ProductMaintenanceReminderDao().GetProductMaintainRemindInfo(MrID);
        }
        public static DataTable GetProductMaintainRemindData(string where = "")
        {
            return new ProductMaintenanceReminderDao().GetProductMaintainRemindData(where);
        }
        public static bool DeleteProductMaintainRemind(int MrID)
        {
            return new ProductMaintenanceReminderDao().DeleteProductMaintainRemind(MrID);
        }
        /// <summary>
        /// 修改星级评分级别
        /// </summary>
        /// <param name="salesnfo">星级评分级别实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateProductMaintainRemind(ProductMaintainRemindInfo MrID)
        {
            bool flag = new ProductMaintenanceReminderDao().UpdateProductMaintainRemind(MrID);
            return flag;
        }

    }

}
