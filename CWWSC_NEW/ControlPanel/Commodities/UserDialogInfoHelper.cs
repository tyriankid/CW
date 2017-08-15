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
    public class UserDialogInfoHelper
    {

        public static bool AddUserDialogInfo(UserDialogInfo info)
        {
            bool flag = new UserDialogInfoDao().AddUserDialogInfo(info);
            return flag;
        }
        public static UserDialogInfo GetUserDialogInfo(Guid DialogID)
        {
            return new UserDialogInfoDao().GetUserDialogInfo(DialogID);
        }
        public static DataTable GetUserDialogInfoData(string where = "",int top =0)
        {
            return new UserDialogInfoDao().GetUserDialogInfoData(where,top);
        }
        public static bool DeleteGetUserDialogInfo(Guid DialogID)
        {
            return new UserDialogInfoDao().DeleteGetUserDialogInfo(DialogID);
        }
        public static bool UpdateUserDialogInfo(UserDialogInfo info)
        {
            bool flag = new UserDialogInfoDao().UpdateUserDialogInfo(info);
            return flag;
        }

    }

}
