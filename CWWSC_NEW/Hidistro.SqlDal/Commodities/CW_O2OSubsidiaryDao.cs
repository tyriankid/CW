using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
    public  class CW_O2OSubsidiaryDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        //(新增)
        public int InsertO2OSubsidiary(CW_O2OSubsidiaryInfo info)
        {
            //DbCommand sqlStringCommands = this.database.GetSqlStringCommand("select*from CW_O2OMembersCol where ColId='18' ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_O2OMembersCol(ColName, userid,type,scode) VALUES (@ColName, @userid,@type,@scode)");
            this.database.AddInParameter(sqlStringCommand, "ColName", DbType.String, info.ColName);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, info.userid);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.String, info.type);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.Int32, info.scode);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        public bool insertO2OSubsidiary(CW_O2OSubsidiaryInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_O2OMembersCol(ColName, userid,type,scode) VALUES (@ColName, @userid,@type,@scode)");
            this.database.AddInParameter(sqlStringCommand, "ColName", DbType.String, info.ColName);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, info.userid);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.String, info.type);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.Int32, info.scode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(删除)
        public bool DeleteO2OSubsidiary(int ColId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_O2OMembersCol WHERE ColId = @ColId");
            this.database.AddInParameter(sqlStringCommand, "ColId", DbType.Int32, ColId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(修改)
        public bool UpdateO2OSubsidiary(CW_O2OSubsidiaryInfo info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_O2OMembersCol SET ColName=@ColName,userid=@userid,type=@type,scode=@scode WHERE ColId = @ColId");
            this.database.AddInParameter(sqlStringCommand, "ColName", DbType.String, info.ColName);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, info.userid);
            this.database.AddInParameter(sqlStringCommand, "type", DbType.String, info.type);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, info.scode);
            this.database.AddInParameter(sqlStringCommand, "ColId", DbType.Int32, info.ColId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(查询)
        public DbQueryResult GetListO2OSubsidiary(CW_O2OSubsidiaryQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_O2OMembersCol", "ColId", string.IsNullOrEmpty(query.ColName) ? string.Empty : string.Format("ColName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ColName)), "*");
        }
        public CW_O2OSubsidiaryInfo GetO2OSubsidiary(int ColId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_O2OMembersCol WHERE ColId = @ColId");
            this.database.AddInParameter(sqlStringCommand, "ColId", DbType.Int32, ColId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CW_O2OSubsidiaryInfo>(reader);
            }
        }
    }
}
