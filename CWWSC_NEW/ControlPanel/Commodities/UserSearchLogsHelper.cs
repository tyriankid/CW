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
    public class UserSearchLogsHelper
    {

        /// <summary>
        /// 添加用户搜索记录
        /// </summary>
        /// <param name="salesnfo">用户搜索记录实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddUserSearch(UserSearchLogsInfo SearchLogsInfo)
        {
            bool flag = new UserSearchLogsDao().AddUserSearch(SearchLogsInfo);
            return flag;
        }
        /// <summary>
        /// 根据主键查询实体
        /// </summary>
        /// <param name="SearchId"></param>
        /// <returns></returns>
        public static UserSearchLogsInfo GetUserSearchInfo(string SearchId)
        {
            return new UserSearchLogsDao().GetUserSearchInfo(SearchId);
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetUserSearchData(string where = "")
        {
            return new UserSearchLogsDao().GetUserSearchData(where);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="SearchId"></param>
        /// <returns></returns>
        public static bool DeleteUserSearch(string SearchId)
        {
            return new UserSearchLogsDao().DeleteUserSearch(SearchId);
        }
        /// <summary>
        /// 根据用户删除
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static bool DeleteUserSearchUserId(string UserId)
        {
            return new UserSearchLogsDao().DeleteUserSearchUserId(UserId);
        }
        /// <summary>
        /// 修改用户搜索记录
        /// </summary>
        /// <param name="salesnfo">用户搜索记录实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateUserSearch(UserSearchLogsInfo SearchLogsInfo)
        {
            bool flag = new UserSearchLogsDao().UpdateUserSearch(SearchLogsInfo);
            return flag;
        }
        public static bool DeleteHotSearch()
        {
            return new UserSearchLogsDao().DeleteHotSearch();
        }
        
    }

}
