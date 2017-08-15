using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ControlPanel.Commodities
{
    public class O2OMemberHelper
    {
        //（新增）
        public static int InsertO2OCwMembers(CW_O2OMenbersInfo member)
        {
            if (member == null)
            {
                return 0;
            }
            Globals.EntityCoding(member, true);
            int id = new CW_O2OMemberDao().InsertO2OCwMembers(member);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "新增加了一个会员:”{0}”", new object[] { member.name }));
            }
            return id;
        }

        //（查询录入会员）
        public static DbQueryResult GetListO2OMember(CW_O2OMemberQuery query)
        {
            return new CW_O2OMemberDao().GetListO2OMember(query);
        }
        //（查询粘性会员）
        public static DbQueryResult GetListNxMember(CW_O2OMemberQuery query)
        {
            return new CW_O2OMemberDao().GetListNxMember(query);
        }

        //（删除）
        public static bool DeleteO2OMembers(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new CW_O2OMemberDao().DeleteO2OMembers(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的会员", new object[] { Id }));
            }
            return flag;
        }
        //（修改）
        public static bool UpdateO2OMembersr(CW_O2OMenbersInfo member)
        {
            if (member == null)
            {
                return false;
            }
            Globals.EntityCoding(member, true);
            bool flag = new CW_O2OMemberDao().UpdateO2OMembersr(member);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了名称为”{0}”的会员信息", new object[] { member.name }));
            }
            return flag;
        }

        public static CW_O2OMenbersInfo GetO2OMember(int userId)
        {
            return new CW_O2OMemberDao().GetO2OMemberId(userId);
        }

        /// <summary>
        /// 根据条件，与查询字段得到粘性会员导出数据集
        /// </summary>
        /// <param name="query">条件参数对象</param>
        /// <param name="fields">查询字段集合</param>
        /// <returns></returns>
        public static DataTable GetAllMemberExprot(CW_O2OMemberQuery query, IList<string> fields)
        {
            return new CW_O2OMemberDao().GetAllMemberExprot(query, fields);
        }

        /// <summary>
        /// 根据条件，与查询字段得到粘性会员导出数据集
        /// </summary>
        /// <param name="query">条件参数对象</param>
        /// <param name="fields">查询字段集合</param>
        /// <returns></returns>
        public static DataTable GetO2OMemberExprot(CW_O2OMemberQuery query, IList<string> fields)
        {
            return new CW_O2OMemberDao().GetO2OMemberExprot(query, fields);
        } 


    }
}
