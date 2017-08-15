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
    public class UserNotReadHelper
    {
        public static bool AddUserNotRead(UserNotRead info)
        {
            bool flag = new UserNotReadDao().AddUserNotRead(info);
            return flag;
        }
        public static UserNotRead GetUserNotRead(Guid ID)
        {
            return new UserNotReadDao().GetUserNotRead(ID);
        }
        public static DataTable GetUserNotReadData(string where = "",int top =0)
        {
            return new UserNotReadDao().GetUserNotReadData(where,top);
        }
        public static bool DeleteUserNotRead(Guid ID)
        {
            return new UserNotReadDao().DeleteUserNotRead(ID);
        }
        public static bool UpdateUserNotRead(UserNotRead info)
        {
            bool flag = new UserNotReadDao().UpdateUserNotRead(info);
            return flag;
        }

    }

}
