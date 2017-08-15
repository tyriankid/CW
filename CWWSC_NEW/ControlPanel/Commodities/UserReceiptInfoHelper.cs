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
    public class UserReceiptInfoHelper
    {
        /// <summary>
        /// 添加发票信息
        /// </summary>
        /// <param name="UserReceiptInfo">发票实体类信息</param>
        /// <returns></returns>
        public static bool AddUserReceiptInfo(UserReceiptInfo receiptInfo)
        {
            UserReceiptInfoDao dao = new UserReceiptInfoDao();
            int receiptId = dao.AddUserReceiptInfo(receiptInfo);
            if (dao.SetDefaultReceiptInfo(receiptId, Globals.GetCurrentMemberUserId()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据主键UserID获取发票信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //public static UserReceiptInfo GetUserReceiptInfo(int UserId)
        //{
        //    return new UserReceiptInfoDao().GetUserReceiptInfo(UserId);
        //}
         

        /// <summary>
        /// 根据主键ID，更新发票信息
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns>返回bool状态</returns>
        public static bool UpdateUserReceiptInfo(UserReceiptInfo receiptInfo)
        {
            UserReceiptInfoDao dao = new UserReceiptInfoDao();
            if (dao.UpdateUserReceiptInfo(receiptInfo))
            {
                if (dao.SetDefaultReceiptInfo(receiptInfo.ReceiptId, Globals.GetCurrentMemberUserId()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据条件查找发票信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>返回DataSet数据集</returns>
        public static DataTable SelectUserReceiptInfoByWhere(string where)
        {
            return new UserReceiptInfoDao().SelectUserReceiptInfoByWhere(where);
        }

        public static UserReceiptInfo GetUserReceiptInfo(int ReceiptId)
        {
            return new UserReceiptInfoDao().GetUserReceiptInfo(ReceiptId);
        }

        public static IList<UserReceiptInfo> GetUserReceiptInfo(string type = "")
        {
            return new UserReceiptInfoDao().GetListUserReceiptInfo(Globals.GetCurrentMemberUserId(), type);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public static bool DeleteReceipt(int receiptId)
        {
            return new UserReceiptInfoDao().DeleteReceipt(receiptId);
        }

    }

}
