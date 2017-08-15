using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ControlPanel.Commodities
{
    public class O2OAttributeHelper
    {
        //（删除）
        public static bool DeleteO2OAttribute(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new CW_O2OMembersAttribute().DeleteO2OAttribute(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的会员", new object[] { Id }));
            }
            return flag;
        }
    }
}
