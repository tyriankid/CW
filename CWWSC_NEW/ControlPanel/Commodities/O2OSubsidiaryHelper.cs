using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ControlPanel.Commodities
{
    public   class O2OSubsidiaryHelper
    {
        //（新增）
        public static int InsertO2OSubsidiary(CW_O2OSubsidiaryInfo info)
        {
            if (info == null)
            {
                return 0;
            }
            Globals.EntityCoding(info, true);
            int id = new CW_O2OSubsidiaryDao().InsertO2OSubsidiary(info);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "新增加了列名:”{0}”", new object[] { info.ColName }));
            }
            return id;
        }

        //（查询）
        public static DbQueryResult GetListO2OSubsidiary(CW_O2OSubsidiaryQuery query)
        {
            return new CW_O2OSubsidiaryDao().GetListO2OSubsidiary(query);
        }
        //（删除）
        public static bool DeleteO2OSubsidiary(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new CW_O2OSubsidiaryDao().DeleteO2OSubsidiary(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除成功"));
            }
            return flag;
        }
        //（修改）
        public static bool UpdateO2OSubsidiary(CW_O2OSubsidiaryInfo info)
        {
            if (info == null)
            {
                return false;
            }
            Globals.EntityCoding(info, true);
            bool flag = new CW_O2OSubsidiaryDao().UpdateO2OSubsidiary(info);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改成功"));
            }
            return flag;
        }
        public static CW_O2OSubsidiaryInfo GetO2OSubsidiary(int ColId)
        {
            return new CW_O2OSubsidiaryDao().GetO2OSubsidiary(ColId);
        }
    }
}
