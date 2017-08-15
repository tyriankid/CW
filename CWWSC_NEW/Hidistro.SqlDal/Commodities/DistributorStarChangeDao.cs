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
    public class DistributorStarChangeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
      

        /// <summary>
        /// 添加修改星级评分记录
        /// </summary>
        /// <param name="ChangeInfo">修改星级评分实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public bool AddDistributorStarChange(DistributorStarChangeInfo ChangeInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorStarChange (ID,DisUserId,ChangeType,ChangeMark,OpDataTime,OpReason) VALUES (@ID,@DisUserId,@ChangeType,@ChangeMark,@OpDataTime,@OpReason)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, ChangeInfo.ID);
            this.database.AddInParameter(sqlStringCommand, "DisUserId", DbType.Int32, ChangeInfo.DisUserId);
            this.database.AddInParameter(sqlStringCommand, "ChangeType", DbType.Boolean, ChangeInfo.ChangeType);
            this.database.AddInParameter(sqlStringCommand, "ChangeMark", DbType.Decimal, ChangeInfo.ChangeMark);
            this.database.AddInParameter(sqlStringCommand, "OpDataTime", DbType.DateTime, ChangeInfo.OpDataTime);
            this.database.AddInParameter(sqlStringCommand, "OpReason", DbType.String, ChangeInfo.OpReason);
            //返回数据库影响的行数，大于0则添加成功
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
