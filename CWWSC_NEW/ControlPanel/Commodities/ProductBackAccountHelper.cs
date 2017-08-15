using Hidistro.Entities.Commodities;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ControlPanel.Commodities
{
    public class ProductBackAccountHelper
    {
        /// <summary>
        /// 新建不通过原因
        /// </summary>
        /// <param name="backAccount">实体对象</param>
        /// <returns></returns>
        public static int AddProductBackAccount(ProductBackAccount backAccount)
        {
            return new ProductBackAccountDao().AddProductBackAccount(backAccount);
        }

        /// <summary>
        /// 根据商品Id得到不通过原因
        /// </summary>
        /// <param name="productid">商品ID</param>
        /// <returns></returns>
        public static DataTable GetProductBackAccount(int productid)
        {
            return new ProductBackAccountDao().GetProductBackAccount(productid);
        }

        /// <summary>
        /// 得到所有不通过原因
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProductBackAccountByWhere(string where)
        {
            return new ProductBackAccountDao().GetProductBackAccountByWhere(where);
        }

    }
}
