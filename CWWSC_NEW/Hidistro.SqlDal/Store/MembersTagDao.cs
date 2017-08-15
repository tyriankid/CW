using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
    public class MembersTagDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 获取当前用户所有标签，并绑定到前台
        /// </summary>
        /// <param name="MembersTagInfo"></param>
        /// <returns>返回一个DataTable</returns>
        public DataTable GetMembersTag(MembersTagInfo MembersTagInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_MembersTag WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, MembersTagInfo.userId);
            using (DataSet ds = this.database.ExecuteDataSet(sqlStringCommand))
            {
                return ds.Tables[0];
            }
        }
        /// <summary>
        /// 根据当前用户ID，删除其所有的标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>返回删除的影响行数</returns>
        public int DeleteMembersTagInfoByUserID(string userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_MembersTag WHERE userId=@userId");
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.String, userid);

            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}
