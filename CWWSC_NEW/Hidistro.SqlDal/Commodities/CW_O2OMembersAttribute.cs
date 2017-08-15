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
     public  class CW_O2OMembersAttribute
    {
         private Database database = DatabaseFactory.CreateDatabase();
    
     //(新增)
         public int InsertO2OMembersAttribute(CW_O2OMembersAttributeInfo Info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_O2OMembersAttribute(type,ColId,userid,ColValue,scode,GroupID) VALUES (@type, @ColId,@userid,@ColValue,@GroupID)");
            this.database.AddInParameter(sqlStringCommand, "type", DbType.String, Info.type);
            this.database.AddInParameter(sqlStringCommand, "ColId", DbType.String, Info.ColId);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.String, Info.userid);
            this.database.AddInParameter(sqlStringCommand, "ColValue", DbType.String, Info.ColValue);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.Int32, Info.scode);
            this.database.AddInParameter(sqlStringCommand, "GroupID", DbType.Int32, Info.GroupID);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
         public bool DeleteO2OAttribute(int Id)
         {
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_O2OMembersAttribute WHERE userid = @userid");
             this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, Id);
             return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
         }


    //(修改)
         public bool UpdateO2OMembersAttribute(CW_O2OMembersAttributeInfo Info)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_O2OMembersAttribute SET type=@type,ColId=@ColId,userid=@userid,ColValue=@ColValue,scode=@scode,GroupID=@GroupID WHERE id = @id");
            this.database.AddInParameter(sqlStringCommand, "type", DbType.String, Info.type);
            this.database.AddInParameter(sqlStringCommand, "ColId", DbType.String, Info.ColId);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.String, Info.userid);
            this.database.AddInParameter(sqlStringCommand, "ColValue", DbType.String, Info.ColValue);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.Int32, Info.scode);
            this.database.AddInParameter(sqlStringCommand, "GroupID", DbType.Int32, Info.GroupID);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Int32, Info.id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}
