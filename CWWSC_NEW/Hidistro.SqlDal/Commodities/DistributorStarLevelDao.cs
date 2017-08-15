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
    public class DistributorStarLevelDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加评分等级
        /// </summary>
        /// <param name="StarLevelInfo">评分等级实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddDistributorStarLevel(DistributorStarLevelInfo StarLevelInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorStarLevel (StarLevelID,LevelName,LevelNum,MinNum,MaxNum,CommissionType,CommissionRise,CommissionMoney,IsDefault,Ico) VALUES (@StarLevelID,@LevelName,@LevelNum,@MinNum,@MaxNum,@CommissionType,@CommissionRise,@CommissionMoney,@IsDefault,@Ico)");
            this.database.AddInParameter(sqlStringCommand, "StarLevelID", DbType.Guid, StarLevelInfo.StarLevelID);
            this.database.AddInParameter(sqlStringCommand, "LevelName", DbType.String, StarLevelInfo.LevelName);
            this.database.AddInParameter(sqlStringCommand, "LevelNum", DbType.Int32, StarLevelInfo.LevelNum);
            this.database.AddInParameter(sqlStringCommand, "MinNum", DbType.Int32, StarLevelInfo.MinNum);
            this.database.AddInParameter(sqlStringCommand, "MaxNum", DbType.Int32, StarLevelInfo.MaxNum);
            this.database.AddInParameter(sqlStringCommand, "CommissionType", DbType.Int32, StarLevelInfo.CommissionType);
            this.database.AddInParameter(sqlStringCommand, "CommissionRise", DbType.Decimal, StarLevelInfo.CommissionRise);
            this.database.AddInParameter(sqlStringCommand, "CommissionMoney", DbType.Currency, StarLevelInfo.CommissionMoney);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, StarLevelInfo.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, StarLevelInfo.Ico);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改评分等级
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateDistributorStarLevel(DistributorStarLevelInfo StarLevelInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorStarLevel SET LevelName = @LevelName,LevelNum=@LevelNum,MinNum=@MinNum,MaxNum=@MaxNum,CommissionType=@CommissionType,CommissionRise=@CommissionRise,CommissionMoney=@CommissionMoney,IsDefault=@IsDefault,Ico=@Ico where StarLevelID=@StarLevelID");
            this.database.AddInParameter(sqlStringCommand, "StarLevelID", DbType.Guid, StarLevelInfo.StarLevelID);
            this.database.AddInParameter(sqlStringCommand, "LevelName", DbType.String, StarLevelInfo.LevelName);
            this.database.AddInParameter(sqlStringCommand, "LevelNum", DbType.Int32, StarLevelInfo.LevelNum);
            this.database.AddInParameter(sqlStringCommand, "MinNum", DbType.Int32, StarLevelInfo.MinNum);
            this.database.AddInParameter(sqlStringCommand, "MaxNum", DbType.Int32, StarLevelInfo.MaxNum);
            this.database.AddInParameter(sqlStringCommand, "CommissionType", DbType.Int32, StarLevelInfo.CommissionType);
            this.database.AddInParameter(sqlStringCommand, "CommissionRise", DbType.Decimal, StarLevelInfo.CommissionRise);
            this.database.AddInParameter(sqlStringCommand, "CommissionMoney", DbType.Currency, StarLevelInfo.CommissionMoney);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, StarLevelInfo.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, StarLevelInfo.Ico);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DistributorStarLevelInfo GetDistributorStarLevelInfo(string StarLevelID)
        {
            DistributorStarLevelInfo info = new DistributorStarLevelInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from aspnet_DistributorStarLevel where StarLevelID=@StarLevelID");
            this.database.AddInParameter(sqlStringCommand, "StarLevelID", DbType.String, StarLevelID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<DistributorStarLevelInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDistributorStarLevelData(string where = "")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from aspnet_DistributorStarLevel " + where + "");
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
        public bool DeleteDistributorStarLevel(string StarLevelID)
        {
            List<DistributorStarLevelInfo> list = new List<DistributorStarLevelInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete aspnet_DistributorStarLevel where StarLevelID=@StarLevelID");
            this.database.AddInParameter(sqlStringCommand, "StarLevelID", DbType.String, StarLevelID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
