using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Commodities
{
    public class BuyOneGetOneFreeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加买一送一
        /// </summary>
        /// <param name="FreeInfo">买一送一实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddBuyOneGetOne(BuyOneGetOneFreeInfo FreeInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_BuyOneGetOneFree (productId,GetProductId,startime,endtime,getNum) VALUES (@productId,@GetProductId,@startime,@endtime,@getNum)");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, FreeInfo.productId);
            this.database.AddInParameter(sqlStringCommand, "GetProductId", DbType.Int32, FreeInfo.GetProductId);
            this.database.AddInParameter(sqlStringCommand, "startime", DbType.DateTime, FreeInfo.startime);
            this.database.AddInParameter(sqlStringCommand, "endtime", DbType.DateTime, FreeInfo.endtime);
            this.database.AddInParameter(sqlStringCommand, "getNum", DbType.Int32, FreeInfo.getNum);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 添加买一送一记录
        /// </summary>
        /// <param name="FreeInfo">买一送一实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddBuyOneGetOneDetail(BuyOneGetOneDetailFreeInfo FreeInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_BuyOneGetOneFree (buyoneId,buyDate,userId) VALUES (@buyoneId,@buyDate,@userId)");
            this.database.AddInParameter(sqlStringCommand, "buyoneId", DbType.Int32, FreeInfo.buyoneId);
            this.database.AddInParameter(sqlStringCommand, "buyDate", DbType.DateTime, FreeInfo.buyDate);
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, FreeInfo.userId);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改买一送一
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateBuyOneGetOne(BuyOneGetOneFreeInfo FreeInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_BuyOneGetOneFree SET productId = @productId,GetProductId=@GetProductId,startime=@startime,endtime=@endtime,getNum=@getNum where buyoneId=@buyoneId");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, FreeInfo.productId);
            this.database.AddInParameter(sqlStringCommand, "GetProductId", DbType.Int32, FreeInfo.GetProductId);
            this.database.AddInParameter(sqlStringCommand, "startime", DbType.DateTime, FreeInfo.startime);
            this.database.AddInParameter(sqlStringCommand, "endtime", DbType.DateTime, FreeInfo.endtime);
            this.database.AddInParameter(sqlStringCommand, "getNum", DbType.Int32, FreeInfo.getNum);
            this.database.AddInParameter(sqlStringCommand, "buyoneId", DbType.Int32, FreeInfo.buyoneId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public BuyOneGetOneFreeInfo GetBuyOneGetOneFreeInfo(int buyoneId)
        {
            BuyOneGetOneFreeInfo info = new BuyOneGetOneFreeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_BuyOneGetOneFree where buyoneId=@buyoneId");
            this.database.AddInParameter(sqlStringCommand, "buyoneId", DbType.String, buyoneId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<BuyOneGetOneFreeInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据根据商品ID查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public BuyOneGetOneFreeInfo GetProductBuyOneGetOneFreeInfo(int ProductId)
        {
            BuyOneGetOneFreeInfo info = new BuyOneGetOneFreeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_BuyOneGetOneFree where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.String, ProductId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<BuyOneGetOneFreeInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 得到当前用户在当前活动中是否有活动资格
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int getUserGetNum(int userId, int buyoneId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*)from dbo.CW_BuyOneGetOneFreeDetail where userId=@userId and buyoneId=@buyoneId");
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "buyoneId", DbType.Int32, buyoneId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBuyOneGetOneData(string where = "")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_BuyOneGetOneFree " + where + "");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        public bool BuyOneGetOneProductExist(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM CW_BuyOneGetOneFree WHERE productId=@productId");
            this.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productId);
            return (((int)this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DeleteBuyOneGetOne(int buyoneId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_BuyOneGetOneFree where buyoneId=@buyoneId");
            this.database.AddInParameter(sqlStringCommand, "buyoneId", DbType.String, buyoneId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetBuyOneGetOneProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" WHERE SaleStatus = {0}", 1);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

    }
}
