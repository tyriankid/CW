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
    public class AdminWxMsgInfoHelper
    {

        public static bool AddAdminWxMsgInfo(AdminWxMsgInfo info)
        {
            bool flag = new AdminWxMsgInfoDao().AddAdminWxMsgInfo(info);
            return flag;
        }
        public static AdminWxMsgInfo GetAdminWxMsgInfo(Guid ID)
        {
            return new AdminWxMsgInfoDao().GetAdminWxMsgInfo(ID);
        }
        public static DataTable GetAdminWxMsgInfoData(string where = "",int top =0)
        {
            return new AdminWxMsgInfoDao().GetAdminWxMsgInfoData(where,top);
        }
        public static bool DeleteAdminWxMsgInfo(Guid ID)
        {
            return new AdminWxMsgInfoDao().DeleteAdminWxMsgInfo(ID);
        }
        public static bool UpdateAdminWxMsgInfo(AdminWxMsgInfo info)
        {
            bool flag = new AdminWxMsgInfoDao().UpdateAdminWxMsgInfo(info);
            return flag;
        }

    }

}
