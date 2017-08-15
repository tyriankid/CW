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
    public class CWMembersDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        //(新增)
        public int InsertCwMembers(CWMenbersInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_Members(UserName, accountALLHere,StoreName,CellPhone,Address,RelationUserId,UserCode,ProductCode,ProductModel,Price,BuyNum) VALUES (@UserName, @accountALLHere,@StoreName,@CellPhone,@Address,@RelationUserId,@UserCode,@ProductCode,@ProductModel,@Price,@BuyNum)");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, member.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, member.StoreName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "RelationUserId", DbType.Int32, member.RelationUserId);
            this.database.AddInParameter(sqlStringCommand, "UserCode", DbType.String, member.UserCode);
            this.database.AddInParameter(sqlStringCommand, "ProductCode", DbType.String, member.ProductCode);
            this.database.AddInParameter(sqlStringCommand, "ProductModel", DbType.String, member.ProductModel);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.String, member.Price);
            this.database.AddInParameter(sqlStringCommand, "BuyNum", DbType.String, member.BuyNum);

            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        public bool insertMember(CWMenbersInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_Members(UserName, accountALLHere,StoreName,CellPhone,Address,RelationUserId,UserCode,ProductCode,ProductModel,Price,BuyNum,UserId,StoreId) VALUES (@UserName, @accountALLHere,@StoreName,@CellPhone,@Address,@RelationUserId,@UserCode,@ProductCode,@ProductModel,@Price,@BuyNum,@UserId,@StoreId)");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, member.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, member.StoreName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "RelationUserId", DbType.Int32, member.RelationUserId);
            this.database.AddInParameter(sqlStringCommand, "UserCode", DbType.String, member.UserCode);
            this.database.AddInParameter(sqlStringCommand, "ProductCode", DbType.String, member.ProductCode);
            this.database.AddInParameter(sqlStringCommand, "ProductModel", DbType.String, member.ProductModel);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.String, member.Price);
            this.database.AddInParameter(sqlStringCommand, "BuyNum", DbType.String, member.BuyNum);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, member.StoreId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(删除)
        public bool DeleteMembers(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_Members WHERE id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(修改)
        public bool UpdateMembersr(CWMenbersInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_Members(UserName, accountALLHere,StoreName,CellPhone,Address,RelationUserId,UserCode,ProductCode,ProductModel,Price,BuyNum) VALUES (@UserName, @accountALLHere,@StoreName,@CellPhone,@Address,@RelationUserId,@UserCode,@ProductCode,@ProductModel,@Price,@BuyNum)");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, member.UserName);
            this.database.AddInParameter(sqlStringCommand, "accountALLHere", DbType.String, member.accountALLHere);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, member.StoreName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, member.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "RelationUserId", DbType.Int32, member.RelationUserId);
            this.database.AddInParameter(sqlStringCommand, "UserCode", DbType.String, member.UserCode);
            this.database.AddInParameter(sqlStringCommand, "ProductCode", DbType.String, member.ProductCode);
            this.database.AddInParameter(sqlStringCommand, "ProductModel", DbType.String, member.ProductModel);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.String, member.Price);
            this.database.AddInParameter(sqlStringCommand, "BuyNum", DbType.String, member.BuyNum);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        //(查询)
        public DbQueryResult GetListMember(ListMembersQuery query)
        {
            string strWhere = " 1=1 ";
            if (query.id != 0)
            {
                strWhere += string.Format(" and  StoreId={0}", query.id);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                strWhere += string.Format(" and UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.StoreCode))
            {
                strWhere += string.Format(" and accountALLHere in ({0})", query.StoreCode);
            }
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_Members", "UserName", strWhere, "*,(SELECT storeName FROM CW_StoreInfo where accountALLHere = CW_Members.accountALLHere) as StoreName2,(select m.UserHead from aspnet_Members  m  where CW_Members.UserId=m.UserId) as UserHead, (SELECT UserName+'/'+RealName FROM aspnet_Members WHERE UserId = CW_Members.RelationUserId) AS MeberName");
        }

        //(激活会员查询)
        public DbQueryResult GetActivateMember(ListMembersQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_Members C left join   aspnet_Members  A  on A.UserId=C.RelationUserId", "C.UserName", string.IsNullOrEmpty(query.UserName) ? "UserId = C.RelationUserId" : string.Format(" UserId = C.RelationUserId  and  C.UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName)), "C.*, (A.UserName+'/'+A.RealName) as  MeberName");
        }

        public  DataTable GetAllMember()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Members");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 查询所有门店
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFiliale()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Filiale");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        // <summary>
        ///根据条件查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetMemberwhere(string where="")
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * FROM CW_Members " + where + "");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        public CWMenbersInfo GetMember(string userName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Members WHERE UserName = @UserName");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CWMenbersInfo>(reader);
            }
        }
        public CWMenbersInfo GetMemberId(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Members WHERE id = @Id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.String, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CWMenbersInfo>(reader);
            }
        }
        ////(获取所有用户)
        public IList<CWMenbersInfo> GetMemberModelList()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_Members");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<CWMenbersInfo>(reader);
            }
        }

        /// <summary>
        /// 根据电话查询CwMember表
        /// </summary>
        /// <param name="cellphone">电话</param>
        /// <returns></returns>
        public DataTable GetCwMemberByCellPhone(string cellphone)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * FROM CW_Members where CellPhone = '{0}'", cellphone));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 金力会员导出
        /// </summary>
        /// <param name="cellphone">电话</param>
        /// <returns></returns>
        public DataTable GetExportCwMemberByQuery(ListMembersQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.TrimEnd(',');

            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(query.UserName))
            {
                strWhere += string.Format(" and UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.StoreCode))
            {
                strWhere += string.Format(" and accountALLHere in ({0})", query.StoreCode);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
                strWhere += string.Format(" order by {0} {1}", query.SortBy, query.SortOrder.ToString());
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select {0} FROM CW_Members where {1}", str, strWhere));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


    }
}
