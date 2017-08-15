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
    public class BuyOneGetOnrFreeHelper
    {

        /// <summary>
        /// 添加买一送一
        /// </summary>
        /// <param name="salesnfo">买一送一实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddBuyOneGetOne(BuyOneGetOneFreeInfo buyoneId)
        {
            bool flag = new BuyOneGetOneFreeDao().AddBuyOneGetOne(buyoneId);
            return flag;
        }
        /// <summary>
        /// 添加买一送一活动详情
        /// </summary>
        /// <param name="buyoneId"></param>
        /// <returns></returns>
        public static bool AddBuyOneGetOneDetail(BuyOneGetOneDetailFreeInfo buyone)
        {
            bool flag = new BuyOneGetOneFreeDao().AddBuyOneGetOneDetail(buyone);
            return flag;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="buyoneId"></param>
        /// <returns></returns>
        public static BuyOneGetOneFreeInfo GetBuyOneGetOnrFreeInfo(int buyoneId)
        {
            return new BuyOneGetOneFreeDao().GetBuyOneGetOneFreeInfo(buyoneId);
        }
        /// <summary>
        /// 根据商品Id获取买一送一活动实体
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static BuyOneGetOneFreeInfo GetProductBuyOneGetOneFreeInfo(int productId)
        {
            return new BuyOneGetOneFreeDao().GetProductBuyOneGetOneFreeInfo(productId);
        }
        public static int getUserGetNum(int userId, int buyoneId)
        {
            return new BuyOneGetOneFreeDao().getUserGetNum(userId, buyoneId);
        }

        
        /// <summary>
        /// 根据条件查询DataTable
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetBuyOneGetOneData(string where = "")
        {
            return new BuyOneGetOneFreeDao().GetBuyOneGetOneData(where);
        }
        /// <summary>
        /// 根据条件查询商品
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetBuyOneGetOneProducts(ProductQuery query)
        {
            return new BuyOneGetOneFreeDao().GetBuyOneGetOneProducts(query);
        }
        /// <summary>
        /// 根据逐渐删除
        /// </summary>
        /// <param name="buyoneId"></param>
        /// <returns></returns>
        public static bool DeleteBuyOneGetOne(int buyoneId)
        {
            return new BuyOneGetOneFreeDao().DeleteBuyOneGetOne(buyoneId);
        }
        /// <summary>
        /// 买一送一价格
        /// </summary>
        /// <param name="salesnfo">买一送一实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateBuyOneGetOne(BuyOneGetOneFreeInfo buyoneId)
        {
            bool flag = new BuyOneGetOneFreeDao().UpdateBuyOneGetOne(buyoneId);
            return flag;
        }
        /// <summary>
        /// 验证当前商品是否存在
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool BuyOneGetOneProductExist(int productId)
        {
            return new BuyOneGetOneFreeDao().BuyOneGetOneProductExist(productId);
        }

    }

}
