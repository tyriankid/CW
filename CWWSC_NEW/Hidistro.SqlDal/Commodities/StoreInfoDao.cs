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
    public class StoreInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 获取所有门店列表，并分页。
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetListStoreInfo(ListStoreInfoQuery query)
        {
            //return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_StoreInfo", "id", string.IsNullOrEmpty(query.storeName) ? string.Empty : string.Format("(storeName LIKE '%{0}%' or storeRelationPerson LIKE '%{0}%' or accountALLHere LIKE '%{0}%')", DataHelper.CleanSearchString(query.storeName)), "*,(select fgsName from CW_Filiale where id=fgsid) as fgsName,(select UserId from aspnet_Distributors where StoreId=id) as UserId");
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_StoreInfo", "id", string.IsNullOrEmpty(query.storeName) ? string.Empty : string.Format("(storeName LIKE '%{0}%' or storeRelationPerson LIKE '%{0}%' or accountALLHere LIKE '%{0}%')", DataHelper.CleanSearchString(query.storeName)), "*,(select fgsName from CW_Filiale where id=fgsid) as fgsName,(select UserId from aspnet_Distributors where StoreId=id) as UserId");
        }
        public bool DeleteStoreInfo(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_StoreInfo WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据主键ID获取其门店信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public StoreInfo GetStoreInfo(int Id)
        {
            StoreInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_StoreInfo WHERE id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<StoreInfo>(reader);
                reader.NextResult();
            }
            return info;
        }


        public DataTable GetAllStore()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_StoreInfo");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        /// <summary>
        /// 根据主键ID，更新门店信息
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns>返回bool状态</returns>
        public bool UpdateFiliale(StoreInfo StoreInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_StoreInfo SET storeName = @storeName, accountALLHere = @accountALLHere ,scode=@scode,storeRelationCell=@storeRelationCell,storeRelationPerson=@storeRelationPerson,Auditing=@Auditing WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "storeName", DbType.String, StoreInfo.storeName);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, StoreInfo.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, StoreInfo.scode);
            this.database.AddInParameter(sqlStringCommand, "storeRelationCell", DbType.String, StoreInfo.storeRelationCell);
            this.database.AddInParameter(sqlStringCommand, "storeRelationPerson", DbType.String, StoreInfo.storeRelationPerson);
            this.database.AddInParameter(sqlStringCommand, "Auditing", DbType.Int32, StoreInfo.Auditing);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, StoreInfo.Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 根据门店名称查找门店信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>返回DataSet数据集</returns>
        public DataTable SelectStoreInfoByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_StoreInfo ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable SelectStoreClientUserIdByFgsId(int fgsid)
        {
            string strSql = string.Format("select * from dbo.aspnet_Distributors where StoreId in (select id from dbo.CW_StoreInfo where fgsid = '{0}')", fgsid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public DataTable GetAllStoreInfo()
        {
            string strSql = string.Format("SELECT  *,(select fgsName from CW_Filiale where id=fgsid) as fgsName FROM CW_StoreInfo  ORDER BY id Asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        
        }
        /// <summary>
        /// 添加新门店
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns></returns>
        public int AddStoreInfo(StoreInfo StoreInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_StoreInfo(storeName,fgsid, storeRelationPerson,storeRelationCell,accountALLHere,scode) VALUES (@storeName,@fgsid, @storeRelationPerson,@storeRelationCell,@accountALLHere,@scode)");
            this.database.AddInParameter(sqlStringCommand, "storeName", DbType.String, StoreInfo.storeName);
            this.database.AddInParameter(sqlStringCommand, "fgsid", DbType.Int32, StoreInfo.fgsid);
            this.database.AddInParameter(sqlStringCommand, "storeRelationPerson", DbType.String, StoreInfo.storeRelationPerson);
            this.database.AddInParameter(sqlStringCommand, "storeRelationCell", DbType.String, StoreInfo.storeRelationCell);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, StoreInfo.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "scode", DbType.String, StoreInfo.scode);

            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        /// <summary>
        /// 门店认证
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns></returns>
        public DataSet RzStoreInfo(StoreInfo StoreInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_StoreInfo WHERE fgsid = @fgsid and storeRelationPerson=@storeRelationPerson and accountALLHere=@accountALLHere");
            this.database.AddInParameter(sqlStringCommand, "fgsid", DbType.String, StoreInfo.fgsid);
            this.database.AddInParameter(sqlStringCommand, "storeRelationPerson", DbType.String, StoreInfo.storeRelationPerson);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, StoreInfo.accountALLHere);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        ////根据门店ID得到门店信息
        //public DataSet GetStoreInfoBystoreid(int storeid)
        //{
        //    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_StoreInfo WHERE id = @id");
        //    this.database.AddInParameter(sqlStringCommand, "id", DbType.String, storeid);
        //    return this.database.ExecuteDataSet(sqlStringCommand);
        //}

        /// <summary>
        /// 根据用户id得到门店对象信息
        /// </summary>
        /// <param name="userid">用户Id</param>
        /// <returns>门店实体对象</returns>
        public StoreInfo GetStoreInfoByUserId(int userid)
        {
            StoreInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_StoreInfo WHERE id = (select storeId from aspnet_Distributors where UserId = @userId)");
            this.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<StoreInfo>(reader);
                reader.NextResult();
            }
            return info;
        }

        public DataTable GetOrderSourceByOrderId(string orderId)
        {
            string strSql = string.Format(@"select d.*,f.fgsName,f.fgsPhone,s.storeName,s.storeRelationPerson,s.storeRelationCell,s.accountALLHere 
                    from dbo.Hishop_Orders as o 
                    inner join dbo.aspnet_Distributors as d on o.ReferralUserId = d.UserId 
                    left join dbo.CW_StoreInfo as s on d.StoreId = s.id 
                    left join dbo.CW_Filiale as f on s.fgsid = f.id where o.OrderId = '{0}'", orderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable GetOrderSourceServiceByOrderId(string orderId)
        {
            string strSql = string.Format(@"select d.*,f.fgsName,f.fgsPhone,s.storeName,s.storeRelationPerson,s.storeRelationCell,s.accountALLHere 
                    from dbo.Hishop_Orders as o 
                    inner join dbo.aspnet_Distributors as d on o.serviceUserId = d.UserId 
                    left join dbo.CW_StoreInfo as s on d.StoreId = s.id 
                    left join dbo.CW_Filiale as f on s.fgsid = f.id where o.OrderId = '{0}'", orderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

    }
}
