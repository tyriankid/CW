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
    public class CWMembersHelper
    {
        //（新增）
        public static int InsertCwMembers(CWMenbersInfo member)
        {
            if (member == null)
            {
                return 0;
            }
            Globals.EntityCoding(member, true);
            int id = new CWMembersDao().InsertCwMembers(member);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "新增加了一个会员:”{0}”", new object[] { member.UserName }));
            }
            return id;
        }
        //（查询）
        public static DbQueryResult GetListMember(ListMembersQuery query)
        {
            return new CWMembersDao().GetListMember(query);
        }
        public static DbQueryResult GetActivateMember(ListMembersQuery query)
        {
            return new CWMembersDao().GetActivateMember(query);
        }
        //（删除）
        public static bool DeleteMembers(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new CWMembersDao().DeleteMembers(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的会员", new object[] { Id }));
            }
            return flag;
        }
        //（修改）
        public static bool UpdateMembersr(CWMenbersInfo member)
        {
            if (member == null)
            {
                return false;
            }
            Globals.EntityCoding(member, true);
            bool flag = new CWMembersDao().UpdateMembersr(member);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的会员信息", new object[] { member.id }));
            }
            return flag;
        }
        //（获取所有用户）
        public static IList<CWMenbersInfo> GetMemberModelList()
        {
            return new CWMembersDao().GetMemberModelList();
        }
        //（判断用户昵称是否重复）
        public static bool Create(CWMenbersInfo manager)
        {
            //新增对重复用户昵称的判断
            if (GetMember(manager.UserName) != null)
            {
                return false;
            }
            return new CWMembersDao().insertMember(manager);
        }
        public static DataTable GetAllMember()
        {
            return new CWMembersDao().GetAllMember();
        }
        public static DataTable GetMemberwhere(string where="")
        {
            return new CWMembersDao().GetMemberwhere(where);
        }
        public static DataTable GetAllFiliale()
        {
            return new CWMembersDao().GetAllFiliale();
        }
        public static CWMenbersInfo GetMember(string userName)
        {
            return new CWMembersDao().GetMember(userName);
        }
        public static CWMenbersInfo GetMemberId(int Id)
        {
            return new CWMembersDao().GetMemberId(Id);
        }

        /// <summary>
        /// 根据电话查询CwMember表
        /// </summary>
        /// <param name="cellphone">电话</param>
        /// <returns></returns>
        public static DataTable GetCwMemberByCellPhone(string cellphone)
        {
            return new CWMembersDao().GetCwMemberByCellPhone(cellphone);
        }

        /// <summary>
        /// 金立会员导出
        /// </summary>
        /// <param name="query">条件</param>
        /// <param name="fields">导出字段</param>
        /// <returns></returns>
        public static DataTable GetExportCwMemberByQuery(ListMembersQuery query, IList<string> fields)
        {
            return new CWMembersDao().GetExportCwMemberByQuery(query, fields);
        }

    }
}
