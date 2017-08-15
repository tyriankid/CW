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
    public class ProductMaintenanceReminderDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 添加维护提醒
        /// </summary>
        /// <param name="MaintainRemindInfo">维护提醒实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddProductMaintainRemind(ProductMaintainRemindInfo MaintainRemindInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_ProductMaintainRemind (ProductId,RemindCycle,RemindNum,RemindRemark) VALUES (@ProductId,@RemindCycle,@RemindNum,@RemindRemark)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, MaintainRemindInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "RemindCycle", DbType.Int32, MaintainRemindInfo.RemindCycle);
            this.database.AddInParameter(sqlStringCommand, "RemindNum", DbType.Int32, MaintainRemindInfo.RemindNum);
            this.database.AddInParameter(sqlStringCommand, "RemindRemark", DbType.String, MaintainRemindInfo.RemindRemark);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 修改维护提醒
        /// </summary>
        /// <param name="pcUserid"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        public bool UpdateProductMaintainRemind(ProductMaintainRemindInfo MaintainRemindInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_ProductMaintainRemind SET ProductId = @ProductId,RemindCycle=@RemindCycle,RemindNum=@RemindNum,RemindRemark=@RemindRemark where MrID=@MrID");
            this.database.AddInParameter(sqlStringCommand, "MrID", DbType.Int32, MaintainRemindInfo.MrID);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, MaintainRemindInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "RemindCycle", DbType.Int32, MaintainRemindInfo.RemindCycle);
            this.database.AddInParameter(sqlStringCommand, "RemindNum", DbType.Int32, MaintainRemindInfo.RemindNum);
            this.database.AddInParameter(sqlStringCommand, "RemindRemark", DbType.String, MaintainRemindInfo.RemindRemark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据根据主键查询实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ProductMaintainRemindInfo GetProductMaintainRemindInfo(int MrID)
        {
            ProductMaintainRemindInfo info = new ProductMaintainRemindInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_ProductMaintainRemind where MrID=@MrID");
            this.database.AddInParameter(sqlStringCommand, "MrID", DbType.String, MrID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ProductMaintainRemindInfo>(reader);
            }
            return info;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetProductMaintainRemindData(string where = "")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select*from CW_ProductMaintainRemind " + where + "");
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
        public bool DeleteProductMaintainRemind(int MrID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete CW_ProductMaintainRemind where MrID=@MrID");
            this.database.AddInParameter(sqlStringCommand, "MrID", DbType.String, MrID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
