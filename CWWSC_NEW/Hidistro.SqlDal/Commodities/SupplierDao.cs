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
    public class SupplierDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public int AddSupplier(SupplierInfo Supplier)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_Supplier(gysName, gysPhone,gysAddress,scode) VALUES (@gysName, @gysPhone,@gysAddress,@scode)");
            this.database.AddInParameter(sqlStringCommand, "gysName", DbType.String, Supplier.gysName);
            this.database.AddInParameter(sqlStringCommand, "gysAddress", DbType.String, Supplier.gysAddress);
            this.database.AddInParameter(sqlStringCommand, "gysPhone", DbType.String, Supplier.gysPhone);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, Supplier.scode);

            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        /// <summary>
        /// 根据主键ID，删除供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteSupplier(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_Supplier WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据供应商名称查找供应商信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public DataSet SelectSupplierByName(string Name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Supplier WHERE gysName = @gysName");
            this.database.AddInParameter(sqlStringCommand, "gysName", DbType.String, Name);
            return (this.database.ExecuteDataSet(sqlStringCommand) );
        }

        /// <summary>
        /// 查询所有供应商信息
        /// </summary>
        /// <returns>返回供应商实体集合</returns>
        public IList<SupplierInfo> GetListSupplier()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Supplier");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<SupplierInfo>(reader);
            }
        }
        /// <summary>
        /// 获取所有供应商信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListSupplier(ListSupplierQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_Supplier", "gysName", string.IsNullOrEmpty(query.gysName) ? string.Empty : string.Format("gysName LIKE '%{0}%'", DataHelper.CleanSearchString(query.gysName)), "*");
        }
        /// <summary>
        /// 根据主键ID，查找供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SupplierInfo GetSupplier(int Id)
        {
            SupplierInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Supplier WHERE id = @Id;SELECT * FROM CW_Filiale WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<SupplierInfo>(reader);
                reader.NextResult();
                //while (reader.Read())
                //{
                //    info.Brands.Add((int)reader["id"]);
                //}
            }
            return info;
        }

        public DataTable GetAllSupplier()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Supplier");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        /// <summary>
        /// 根据主键ID，跟新此ID的所有信息
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns>返回bool状态</returns>
        public bool UpdateSupplier(SupplierInfo Supplier)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_Supplier SET gysName = @gysName, gysPhone = @gysPhone ,gysAddress=@gysAddress,scode=@scode WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "gysName", DbType.String, Supplier.gysName);
            this.database.AddInParameter(sqlStringCommand, "gysPhone", DbType.String, Supplier.gysPhone);
            this.database.AddInParameter(sqlStringCommand, "gysAddress", DbType.String, Supplier.gysAddress);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, Supplier.scode);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Supplier.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
