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
    public class ProductReservePriceHelper
    {

        /// <summary>
        /// 添加定时修改价格
        /// </summary>
        /// <param name="salesnfo">定时修改价格实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddProductReservePrice(ProductReservePriceInfo ReserveId)
        {
            bool flag = new ProductReservePriceDao().AddProductReservePrice(ReserveId);
            return flag;
        }
        public static ProductReservePriceInfo GetProductReservePriceInfo(int ReserveId)
        {
            return new ProductReservePriceDao().GetProductReservePriceInfo(ReserveId);
        }
        public static DataTable GetProductReservePriceData(string where = "")
        {
            return new ProductReservePriceDao().GetProductReservePriceData(where);
        }
        public static bool DeleteProductReservePrice(int ReserveId)
        {
            return new ProductReservePriceDao().DeleteProductReservePrice(ReserveId);
        }
        /// <summary>
        /// 修改定时修改价格
        /// </summary>
        /// <param name="salesnfo">定时修改价格实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateProductReservePrice(ProductReservePriceInfo ReserveId)
        {
            bool flag = new ProductReservePriceDao().UpdateProductReservePrice(ReserveId);
            return flag;
        }

    }

}
