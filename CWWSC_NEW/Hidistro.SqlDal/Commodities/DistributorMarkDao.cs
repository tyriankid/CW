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
    public class DistributorMarkDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加评分
        /// </summary>
        /// <param name="salesnfo">评分实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddDistributorMarks(DistributorMarkInfo salesnfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorMark (ID,DisUserId,UserId,Mark1,Mark2,Mark3,Mark4,Mark5,Total) VALUES (@ID,@DisUserId,@UserId,@Mark1,@Mark2,@Mark3,@Mark4,@Mark5,@Total)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, salesnfo.ID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, salesnfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, salesnfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "Mark1", DbType.Decimal, salesnfo.Mark1);
            this.database.AddInParameter(sqlStringCommand, "Mark2", DbType.Decimal, salesnfo.Mark2);
            this.database.AddInParameter(sqlStringCommand, "Mark3", DbType.Decimal, salesnfo.Mark3);
            this.database.AddInParameter(sqlStringCommand, "Mark4", DbType.Decimal, salesnfo.Mark4);
            this.database.AddInParameter(sqlStringCommand, "Mark5", DbType.Decimal, salesnfo.Mark5);
            this.database.AddInParameter(sqlStringCommand, "Total", DbType.Decimal, salesnfo.Total);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
