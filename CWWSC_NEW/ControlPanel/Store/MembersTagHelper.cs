using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Store
{
    public static class MembersTagHelper
    {
        /// <summary>
        ///  获取当前用户所有标签，并绑定到前台
        /// </summary>
        /// <param name="MembersTagInfo"></param>
        /// <returns>返回一个DataTable</returns>
        public static DataTable GetMembersTagInfo(MembersTagInfo MembersTagInfo)
        {
            return new MembersTagDao().GetMembersTag(MembersTagInfo);
        }
        /// <summary>
        /// 根据当前用户ID，删除其所有的标签
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="isDeleteMembersTag"></param>
        /// <returns>返回删除的行数</returns>
        public static int DeleteMembersTagInfoByUserID(string userid, bool isDeleteMembersTag)
        {
            int num = new MembersTagDao().DeleteMembersTagInfoByUserID(userid);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteMembersTagInfoByUserID, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 条标签", new object[] { num }));
                if (!isDeleteMembersTag)
                {
                    return num;
                }
            }
            return num;
        }
    }
}
