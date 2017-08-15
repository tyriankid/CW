using Hidistro.Entities.Commodities;
using Hidistro.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.ControlPanel.Commodities
{
    public class StoreUserChangeApplyHelper
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public static bool AddStoreUserChangeApply(StoreUserChangeApply info)
        {
            bool flag = new StoreUserChangeApplyDao().AddStoreUserChangeApply(info);
            return flag;
        }
        public static StoreUserChangeApply GetStoreUserChangeApply(Guid ID)
        {
            return new StoreUserChangeApplyDao().GetStoreUserChangeApply(ID);
        }
        public static DataTable GetStoreUserChangeApplyData(string where = "",int top =0)
        {
            return new StoreUserChangeApplyDao().GetStoreUserChangeApplyData(where,top);
        }
        public static bool DeleteStoreUserChangeApply(Guid ID)
        {
            return new StoreUserChangeApplyDao().DeleteStoreUserChangeApply(ID);
        }
        public static bool UpdateStoreUserChangeApply(StoreUserChangeApply info)
        {
            bool flag = new StoreUserChangeApplyDao().UpdateStoreUserChangeApply(info);
            return flag;
        }
        /// <summary>
        /// 店员变更为店长
        /// </summary>
        public static int ChangeUserToStore(int userid,int storeid)
        {
            int result = 0;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    StoreUserChangeApplyDao dao = new StoreUserChangeApplyDao();
                    result = dao.ChangeUserToStore(userid, storeid, dbTran);
                    if (result == 0)
                    {
                        dbTran.Rollback();
                        return 0;
                    }
                    dbTran.Commit();
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
            
        }

    }

}
