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
    public class FilialeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 获取所有分公司  2017-7-27 yk
        /// </summary>
        /// <returns></returns>
        public DataTable GetFilialeBaseInfo()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select *from CW_Filiale ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 添加新的分公司
        /// </summary>
        /// <param name="Filiale"></param>
        /// <returns></returns>
        public int AddFiliale(FilialeInfo Filiale)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_Filiale(fgsName, fgsPhone,fgsAddress,scode) VALUES (@fgsName, @fgsPhone,@fgsAddress,@scode)");
            this.database.AddInParameter(sqlStringCommand, "fgsName", DbType.String, Filiale.fgsName);
            this.database.AddInParameter(sqlStringCommand, "fgsPhone", DbType.String, Filiale.fgsPhone);
            this.database.AddInParameter(sqlStringCommand, "fgsAddress", DbType.String, Filiale.fgsAddress);

            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, Filiale.scode);

            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        /// <summary>
        /// 根据主键ID获取分公司信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FilialeInfo GetFiliale(int Id)
        {
            FilialeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Filiale WHERE id = @Id;SELECT * FROM CW_Filiale WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<FilialeInfo>(reader);
                reader.NextResult();
            }
            return info;
        }
        public DataSet SelectAllFiliale()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Filiale");
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        /// <summary>
        /// 根据主键ID，更新此ID的所有信息
        /// </summary>
        /// <param name="Filiale"></param>
        /// <returns></returns>
        public bool UpdateFiliale(FilialeInfo Filiale)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_Filiale SET fgsName = @fgsName, fgsPhone = @fgsPhone ,fgsAddress=@fgsAddress,scode=@scode WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "fgsName", DbType.String, Filiale.fgsName);
            this.database.AddInParameter(sqlStringCommand, "fgsPhone", DbType.String, Filiale.fgsPhone);
            this.database.AddInParameter(sqlStringCommand, "fgsAddress", DbType.String, Filiale.fgsAddress);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, Filiale.scode);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Filiale.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 获取所有分公司信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListFiliales(ListFilialeQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_Filiale", "fgsName", string.IsNullOrEmpty(query.fgsName) ? string.Empty : string.Format("fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.fgsName)), "*");
        }
        /// <summary>
        /// 根据主键ID，删除分公司信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteFiliale(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_Filiale WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据分公司名称查找分公司信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>返回DateSet数据集</returns>
        public DataSet SelectFilialeByName(string Name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Filiale WHERE fgsName = @fgsName");
            this.database.AddInParameter(sqlStringCommand, "fgsName", DbType.String, Name);
            //DataSet dt = this.database.ExecuteDataSet(sqlStringCommand);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        
    }
}
