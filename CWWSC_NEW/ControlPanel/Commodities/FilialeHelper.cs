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
    public  class FilialeHelper
    {
        private FilialeHelper()
        {

        }
        /// <summary>
        /// 获取所有分公司
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetFilialeBaseInfo()
        {
            return new FilialeDao().GetFilialeBaseInfo();
        }
        /// <summary>
        /// 添加新的分公司
        /// </summary>
        /// <param name="Filiale"></param>
        /// <returns></returns>
        public static int AddFiliale(FilialeInfo Filiale)
        {
            if (Filiale == null)
            {
                return 0;
            }
            Globals.EntityCoding(Filiale, true);
            int id = new FilialeDao().AddFiliale(Filiale);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddFiliale, string.Format(CultureInfo.InvariantCulture, "添加了一个新的分公司信息:”{0}”", new object[] { Filiale.fgsName }));
            }
            return id;
        }
        /// <summary>
        /// 获取所有公司信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllFiliale()
        {
            DataSet flag = new FilialeDao().SelectAllFiliale();
            return flag;
        }
        /// <summary>
        /// 根据主键ID获取分公司信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回sql语句执行结果</returns>
        public static FilialeInfo GetFiliale(int id)
        {
            return new FilialeDao().GetFiliale(id);
        }
        /// <summary>
        /// 根据主键ID，更新此ID的所有信息
        /// </summary>
        /// <param name="filiale"></param>
        /// <returns>返回一个bool状态</returns>
        public static bool UpdateFiliale(FilialeInfo filiale)
        {
            if (filiale == null)
            {
                return false;
            }
            Globals.EntityCoding(filiale, true);
            bool flag = new FilialeDao().UpdateFiliale(filiale);
            if (flag)
            {

                EventLogs.WriteOperationLog(Privilege.UpdateFiliale, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的分公司信息", new object[] { filiale.Id }));
            }
            return flag;
        }
        /// <summary>
        /// 根据分公司名称查找分公司信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>返回一个DateSet</returns>
        public static DataSet SelectFilialeByName(string Name)
        {

            DataSet flag = new FilialeDao().SelectFilialeByName(Name);
            return flag;
        }
        /// <summary>
        /// 获取所有分公司信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetListFiliale(ListFilialeQuery query)
        {
            return new FilialeDao().GetListFiliales(query);
        }

        /// <summary>
        /// 根据主键ID，删除分公司信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>返回一个bool值</returns>
        public static bool DeleteFiliale(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new FilialeDao().DeleteFiliale(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteFiliale, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的分公司", new object[] { Id }));
            }
            return flag;
        }
    }
}
