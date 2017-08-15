using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
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
    public class nianxingMembersHelper
    {
        //（新增）
        public static int InsertCwMembers(nianxingMembersInfo member)
        {
            if (member == null)
            {
                return 0;
            }
            Globals.EntityCoding(member, true);
            int id = new nianxingMembersDao().InsertCwMembers(member);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "新增加了一个会员:”{0}”", new object[] { member.UserName }));
            }
            return id;
        }
        //（查询）
        public static DbQueryResult GetListMember(nianxingMemberQuery query)
        {
            return new nianxingMembersDao().GetListMember(query);
        }
        public static DbQueryResult GetActivateMember(nianxingMemberQuery query)
        {
            return new nianxingMembersDao().GetActivateMember(query);
        }
        //（删除）
        public static bool DeleteMembers(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new nianxingMembersDao().DeleteMembers(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的会员", new object[] { Id }));
            }
            return flag;
        }
        //（修改）
        public static bool UpdateMembersr(nianxingMembersInfo member)
        {
            return new nianxingMembersDao().UpdateMembersr(member);
        }
        //（获取所有用户）
        public static IList<nianxingMembersInfo> GetMemberModelList()
        {
            return new nianxingMembersDao().GetMemberModelList();
        }
        //（判断用户昵称是否重复）
        public static bool Create(nianxingMembersInfo manager)
        {
            //新增对重复用户昵称的判断
            if (GetMember(manager.UserName) != null)
            {
                return false;
            }
            return new nianxingMembersDao().insertMember(manager);
        }
        public static DataTable GetMemberwhere(string where="")
        {
            return new nianxingMembersDao().GetMemberwhere(where);
        }
        public static DataTable GetAllFiliale()
        {
            return new nianxingMembersDao().GetAllFiliale();
        }
        public static nianxingMembersInfo GetMember(string userName)
        {
            return new nianxingMembersDao().GetMember(userName);
        }
        public static nianxingMembersInfo GetMemberId(int Id)
        {
            return new nianxingMembersDao().GetMemberId(Id);
        }

        /// <summary>
        /// 根据电话查询CwMember表
        /// </summary>
        /// <param name="cellphone">电话</param>
        /// <returns></returns>
        public static DataTable GetCwMemberByCellPhone(string cellphone)
        {
            return new nianxingMembersDao().GetCwMemberByCellPhone(cellphone);
        }

        /// <summary>
        /// 粘性会员导出
        /// </summary>
        /// <param name="query">条件</param>
        /// <param name="fields">导出字段</param>
        /// <returns></returns>
        public static DataTable GetExportCwMemberByQuery(nianxingMemberQuery query, IList<string> fields)
        {
            return new nianxingMembersDao().GetExportCwMemberByQuery(query, fields);
        }

    }
}
