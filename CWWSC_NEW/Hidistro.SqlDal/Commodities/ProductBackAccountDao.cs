using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
    public class ProductBackAccountDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 新建不通过原因
        /// </summary>
        /// <param name="backAccount">实体对象</param>
        /// <returns></returns>
        public int AddProductBackAccount(ProductBackAccount backAccount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_ProductBackAccount(productId, backTime, backAccount) VALUES (@productId, @backTime, @backAccount); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, backAccount.productId);
            this.database.AddInParameter(sqlStringCommand, "backTime", DbType.DateTime, backAccount.backTime);
            this.database.AddInParameter(sqlStringCommand, "backAccount", DbType.String, backAccount.backAccount);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        /// <summary>
        /// 根据商品Id得到不通过原因
        /// </summary>
        /// <param name="productid">商品ID</param>
        /// <returns></returns>
        public DataTable GetProductBackAccount(int productid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_ProductBackAccount WHERE productId=@productId ORDER BY backTime DESC");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 得到所有不通过原因
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductBackAccountByWhere(string where)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_ProductBackAccount WHERE @sqlWhere ORDER BY backTime DESC");
            this.database.AddInParameter(sqlStringCommand, "sqlWhere", DbType.String, where);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

    }
}
