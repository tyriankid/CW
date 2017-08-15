using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
    public class ProductReservePriceDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加预定价格
        /// </summary>
        /// <param name="ReservePriceInfo">预定价格实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddProductReservePrice(ProductReservePriceInfo ReservePriceInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_ProductReservePrice (ProductId,SkuId,StartDate,State,CostPrice,SalePrice,NeigouPrice) VALUES (@ProductId,@SkuId,@StartDate,@State,@CostPrice,@SalePrice,@NeigouPrice)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ReservePriceInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, ReservePriceInfo.SkuId);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, ReservePriceInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, ReservePriceInfo.State);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Decimal, ReservePriceInfo.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Decimal, ReservePriceInfo.SalePrice);
            this.database.AddInParameter(sqlStringCommand, "NeigouPrice", DbType.Decimal, ReservePriceInfo.NeigouPrice);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改预定价格
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateProductReservePrice(ProductReservePriceInfo ReservePriceInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_ProductReservePrice SET ProductId = @ProductId,SkuId=@SkuId,StartDate=@StartDate,State=@State,CostPrice=@CostPrice,SalePrice=@SalePrice,NeigouPrice=@NeigouPrice where ReserveId=@ReserveId");
            this.database.AddInParameter(sqlStringCommand, "ReserveId", DbType.Int32, ReservePriceInfo.ReserveId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ReservePriceInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, ReservePriceInfo.SkuId);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, ReservePriceInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, ReservePriceInfo.State);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Decimal, ReservePriceInfo.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Decimal, ReservePriceInfo.SalePrice);
            this.database.AddInParameter(sqlStringCommand, "NeigouPrice", DbType.Decimal, ReservePriceInfo.NeigouPrice);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ProductReservePriceInfo GetProductReservePriceInfo(int ReserveId)
        {
            ProductReservePriceInfo info = new ProductReservePriceInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_ProductReservePrice where ReserveId=@ReserveId");
            this.database.AddInParameter(sqlStringCommand, "ReserveId", DbType.String, ReserveId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ProductReservePriceInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetProductReservePriceData(string where = "")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_ProductReservePrice " + where + "");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteProductReservePrice(int ReserveId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_ProductReservePrice where ReserveId=@ReserveId");
            this.database.AddInParameter(sqlStringCommand, "ReserveId", DbType.String, ReserveId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
