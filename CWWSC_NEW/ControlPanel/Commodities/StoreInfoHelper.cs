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
    public class StoreInfoHelper
    {
        /// <summary>
        /// 获取所有门店信息，并绑定到前台
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult ListStoreInfo(ListStoreInfoQuery query)
        {
            return new StoreInfoDao().GetListStoreInfo(query);
        }
        /// <summary>
        /// 根据主键ID删除其门店信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>返回bool状态</returns>
        public static bool DeleteStoreInfo(int Id)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = new StoreInfoDao().DeleteStoreInfo(Id);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteStoreInfo, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的门店", new object[] { Id }));
            }
            return flag;
        }

        public static DataTable GetAllStore()
        {
            return new StoreInfoDao().GetAllStore();
        }
        /// <summary>
        /// 根据主键ID获取其门店信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static StoreInfo GetStoreInfo(int id)
        {
            return new StoreInfoDao().GetStoreInfo(id);
        }
         
        /// <summary>
        /// 根据主键ID，更新门店信息
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateStoreInfo(StoreInfo StoreInfo)
        {
            if (StoreInfo == null)
            {
                return false;
            }
            Globals.EntityCoding(StoreInfo, true);
            bool flag = new StoreInfoDao().UpdateFiliale(StoreInfo);
            if (flag)
            {

                EventLogs.WriteOperationLog(Privilege.UpdateFiliale, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的门店信息", new object[] { StoreInfo.Id }));
            }
            return flag;
        }
        /// <summary>
        /// 门店认证
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns></returns>
        public static DataSet RzStoreInfo(StoreInfo StoreInfo)
        {
            DataSet flag = new StoreInfoDao().RzStoreInfo(StoreInfo);
            return flag;
        }
        /// <summary>
        /// 根据门店名称查找门店信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>返回DataSet数据集</returns>
        public static DataTable SelectStoreInfoByWhere(string where)
        {
            return new StoreInfoDao().SelectStoreInfoByWhere(where);
        }

        /// <summary>
        /// 根据分公司ID得到下属“认证门店”信息
        /// </summary>
        /// <param name="fgsid"></param>
        /// <returns></returns>
        public static DataTable SelectStoreClientUserIdByFgsId(int fgsid)
        {
            return new StoreInfoDao().SelectStoreClientUserIdByFgsId(fgsid);
        }

        public static DataTable GetAllStoreInfo()
        {
            return new StoreInfoDao().GetAllStoreInfo();
        }

        /// <summary>
        /// 添加新门店
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns></returns>
        public static int AddStoreInfo(StoreInfo StoreInfo)
        {
            if (StoreInfo == null)
            {
                return 0;
            }
            Globals.EntityCoding(StoreInfo, true);
            int id = new StoreInfoDao().AddStoreInfo(StoreInfo);
            if (id > 0)
            {
                EventLogs.WriteOperationLog(Privilege.AddStoreInfo, string.Format(CultureInfo.InvariantCulture, "添加了一个新的门店:”{0}”", new object[] { StoreInfo.storeName }));
            }
            return id;
        }

        //public static DataSet GetStoreInfoBystoreid(int storeid)
        //{
        //    return new StoreInfoDao().GetStoreInfoBystoreid(storeid);
        //}

        /// <summary>
        /// 根据用户id得到门店对象信息
        /// </summary>
        /// <param name="userid">用户Id</param>
        /// <returns>门店实体对象</returns>
        public static StoreInfo GetStoreInfoByUserId(int userid)
        {
            return new StoreInfoDao().GetStoreInfoByUserId(userid);
        }

        //查询订单来源明细信息
        public static DataTable GetOrderSourceByOrderId(string orderId)
        {
            return new StoreInfoDao().GetOrderSourceByOrderId(orderId);
        }

        //查询订单服务门店明细信息
        public static DataTable GetOrderSourceServiceByOrderId(string orderId)
        {
            return new StoreInfoDao().GetOrderSourceServiceByOrderId(orderId);
        }
    }

}
