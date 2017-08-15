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
    public sealed class SupplierHelper
    {
        private SupplierHelper()
        {

        }
        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public static int AddSupplier(SupplierInfo Supplier)
        {
            if (Supplier == null)
            {
                return 0;
            }
            Globals.EntityCoding(Supplier, true);
            int id = new SupplierDao().AddSupplier(Supplier);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "添加了一个新的供应商:”{0}”", new object[] { Supplier.gysName }));
            }
            return id;
        }
        /// <summary>
        /// 根据主键ID，删除供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>返回bool状态</returns>
        public static bool DeleteSupplier(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new SupplierDao().DeleteSupplier(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的供应商信息", new object[] { Id }));
            }
            return flag;
        }
        /// <summary>
        /// 根据供应商名称查找供应商信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>返回一个DataSet</returns>
        public static DataSet SelectSupplierByName(string Name)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            DataSet flag = new SupplierDao().SelectSupplierByName(Name);

            return flag;
        }

        /// <summary>
        /// 查询所有供应商信息
        /// </summary>
        /// <returns>返回供应商实体集合</returns>
        public static IList<SupplierInfo> GetListSupplier()
        {
            return new SupplierDao().GetListSupplier();
        }
        /// <summary>
        /// 获取所有供应商信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetListSupplier(ListSupplierQuery query)
        {
            return new SupplierDao().GetListSupplier(query);
        }
        /// <summary>
        /// 根据主键ID，查找供应商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SupplierInfo GetSupplier(int id)
        {
            return new SupplierDao().GetSupplier(id);
        }

        public static DataTable GetAllSupplier()
        {
            return new SupplierDao().GetAllSupplier();
        }
        /// <summary>
        /// 根据主键ID，跟新此ID的所有信息
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns>返回Bool状态</returns>
        public static bool UpdateSupplier(SupplierInfo Supplier)
        {
            if (Supplier == null)
            {
                return false;
            }
            Globals.EntityCoding(Supplier, true);
            bool flag = new SupplierDao().UpdateSupplier(Supplier);
            if (flag)
            {

                EventLogs.WriteOperationLog(Privilege.UpdateSupplier, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的供应商信息", new object[] { Supplier.Id }));
            }
            return flag;
        }
    }
}
