namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Xml;

    public class ProductPriceJob : IJob
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 商品价格修改  2017-8-10 yk
        /// </summary>
        public void Execute(XmlNode node)
        {
            DataTable dtReserve = DistributorsBrower.GetProductReservePriceData();//当前价格预约主表
            if (dtReserve.Rows.Count > 0)
            {
                foreach(DataRow row in dtReserve.Rows)
                {    
                    //首先验证修改时间是否满足
                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-mm-dd hh")) >= DateTime.Parse(row["StartDate"].ToString()) && row["State"].ToString() == "0")
                    {   
                        //若满足则修改该商品该规格下的价格
                        DbCommand command = database.GetSqlStringCommand(" update  Hishop_SKUs set CostPrice=@CostPrice,SalePrice=@SalePrice,NeigouPrice=@NeigouPrice where ProductId=@ProductId and SkuId=@SkuId");
                        database.AddInParameter(command, "CostPrice", DbType.Decimal, row["CostPrice"]);
                        database.AddInParameter(command, "SalePrice", DbType.Decimal, row["SalePrice"]);
                        database.AddInParameter(command, "NeigouPrice", DbType.Decimal, row["NeigouPrice"]);
                        database.AddInParameter(command, "ProductId", DbType.Int32, row["ProductId"]);
                        database.AddInParameter(command, "SkuId", DbType.String, row["SkuId"]);
                        if (database.ExecuteNonQuery(command) > 0)
                        {   
                            //修改成功则记录修改状态
                            DbCommand commandReserve = database.GetSqlStringCommand(" update dbo.CW_ProductReservePrice set State=1 where  ReserveId=@ReserveId");
                            database.AddInParameter(commandReserve, "ReserveId", DbType.Int32, row["ReserveId"]);
                            database.ExecuteNonQuery(commandReserve);
                        }
                    }
                }
            }
        }
    }
}

