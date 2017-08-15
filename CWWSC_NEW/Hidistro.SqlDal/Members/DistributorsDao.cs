namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

    public class DistributorsDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddBalanceDrawRequest(BalanceDrawRequestInfo bdrinfo)
        {
            string query = "INSERT INTO Hishop_BalanceDrawRequest(UserId,RequestType,UserName,Amount,AccountName,CellPhone,MerchantCode,Remark,RequestTime,IsCheck) VALUES(@UserId,@RequestType,@UserName,@Amount,@AccountName,@CellPhone,@MerchantCode,@Remark,getdate(),0)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, bdrinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "RequestType", DbType.Int32, bdrinfo.RequesType);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, bdrinfo.UserName);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Decimal, bdrinfo.Amount);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, bdrinfo.AccountName);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, bdrinfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, bdrinfo.MerchanCade);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, bdrinfo.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void AddDistributorProducts(int productId, int distributorId)
        {
            string query = "INSERT INTO Hishop_DistributorProducts VALUES(@ProductId,@UserId)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool CreateDistributor(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Distributors(UserId,StoreName,Logo,BackImage,RequestAccount,GradeId,ReferralUserId,ReferralPath, ReferralOrders,ReferralBlance, ReferralRequestBalance,ReferralStatus,StoreDescription,DistributorGradeId,StoreId) VALUES(@UserId,@StoreName,@Logo,@BackImage,@RequestAccount,@GradeId,@ReferralUserId,@ReferralPath,@ReferralOrders,@ReferralBlance, @ReferralRequestBalance, @ReferralStatus,@StoreDescription,@DistributorGradeId,@StoreId)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int64, (int)distributor.DistributorGradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int64, distributor.ParentUserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ReferralPath", DbType.String, distributor.ReferralPath);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", DbType.Int64, distributor.DistriGradeId);
            this.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, distributor.StoreId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        public bool CreateAgent(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Distributors(UserId,StoreName,Logo,BackImage,RequestAccount,GradeId,ReferralUserId,ReferralPath, ReferralOrders,ReferralBlance, ReferralRequestBalance,ReferralStatus,StoreDescription,DistributorGradeId,IsAgent,AgentGradeId,AgentPath) VALUES(@UserId,@StoreName,@Logo,@BackImage,@RequestAccount,@GradeId,@ReferralUserId,@ReferralPath,@ReferralOrders,@ReferralBlance, @ReferralRequestBalance, @ReferralStatus,@StoreDescription,@DistributorGradeId,@IsAgent,@AgentGradeId,@AgentPath)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int64, (int)distributor.DistributorGradeId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int64, distributor.ParentUserId.Value);
            this.database.AddInParameter(sqlStringCommand, "ReferralPath", DbType.String, distributor.ReferralPath);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", DbType.Int64, distributor.DistriGradeId);
            this.database.AddInParameter(sqlStringCommand, "IsAgent", DbType.Int32, distributor.IsAgent);
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, distributor.AgentGradeId);
            this.database.AddInParameter(sqlStringCommand, "AgentPath", DbType.String, distributor.AgentPath);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        public bool CreateImport(ImportOfProductsQuery dy)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductsList(CommodityID,CommoditySource,CommodityCode)VALUES(@CommodityID,@CommoditySource,@CommodityCode)");
            this.database.AddInParameter(sqlStringCommand, "CommodityID", DbType.Int32, dy.CommodityID);
            this.database.AddInParameter(sqlStringCommand, "CommoditySource", DbType.String, dy.CommoditySource);
            this.database.AddInParameter(sqlStringCommand, "CommodityCode", DbType.String, dy.CommodityCode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateSendRedpackRecord(int serialid, int userid, string openid, int amount, string act_name, string wishing)
        {
            bool flag = true;
            int num = 0x4e20;
            int num2 = amount;
            SendRedpackRecordInfo sendredpackinfo = new SendRedpackRecordInfo
            {
                BalanceDrawRequestID = serialid,
                UserID = userid,
                OpenID = openid,
                ActName = act_name,
                Wishing = wishing,
                ClientIP = Globals.IPAddress
            };
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                SendRedpackRecordDao dao = new SendRedpackRecordDao();

                try
                {
                    if (num2 <= num)
                    {
                        sendredpackinfo.Amount = amount;
                        flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                        return this.UpdateSendRedpackRecord(serialid, 1, dbTran);
                    }
                    int num3 = amount % num;
                    int num4 = amount / num;
                    if (num3 > 0)
                    {
                        sendredpackinfo.Amount = num3;
                        flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                    }
                    if (flag)
                    {
                        for (int i = 0; i < num4; i++)
                        {
                            sendredpackinfo.Amount = num;
                            flag = dao.AddSendRedpackRecord(sendredpackinfo, dbTran);
                            if (!flag)
                            {
                                dbTran.Rollback();
                            }
                        }
                        int num6 = num4 + ((num3 > 0) ? 1 : 0);
                        flag = this.UpdateSendRedpackRecord(serialid, num6, dbTran);
                        if (!flag)
                        {
                            dbTran.Rollback();
                        }
                        return flag;
                    }
                    dbTran.Rollback();
                    return flag;
                }
                catch
                {
                    if (dbTran.Connection != null)
                    {
                        dbTran.Rollback();
                    }
                    flag = false;
                }
                finally
                {
                    if (flag)
                    {
                        dbTran.Commit();
                    }
                    connection.Close();
                }
            }

            return flag;
        }

        public bool FrozenCommision(int userid, string ReferralStatus)
        {
            string query = "UPDATE aspnet_Distributors set ReferralStatus=@ReferralStatus WHERE UserId=@UserId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.String, ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetAllDistributorsName(string keywords)
        {
            DataTable table = new DataTable();
            string[] strArray = Regex.Split(DataHelper.CleanSearchString(keywords), @"\s+");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(strArray[0])));
            for (int i = 1; (i < strArray.Length) && (i <= 5); i++)
            {
                builder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE " + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        public DataTable GetAgentDistributorsName(string keywords)
        {
            DataTable table = new DataTable();
            string[] strArray = Regex.Split(DataHelper.CleanSearchString(keywords), @"\s+");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(strArray[0])));
            for (int i = 1; (i < strArray.Length) && (i <= 5); i++)
            {
                builder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE IsAgent=1 and(" + builder.ToString() + ")");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetDistributorsName(string keywords)
        {
            DataTable table = new DataTable();
            string[] strArray = Regex.Split(DataHelper.CleanSearchString(keywords), @"\s+");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(DataHelper.CleanSearchString(strArray[0])));
            for (int i = 1; (i < strArray.Length) && (i <= 5); i++)
            {
                builder.AppendFormat(" OR StoreName LIKE '%{0}%' OR UserName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 10 StoreName,UserName from vw_Hishop_DistributorsMembers WHERE 1=1 and(" + builder.ToString() + ")");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountALLHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AccountALLHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountALLHere));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.RequestTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),RequestTime,120)='{0}'", query.RequestTime);
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            if (!string.IsNullOrEmpty(query.CheckTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),CheckTime,120)='{0}'", query.CheckTime);
            }
            if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',RequestTime)>=0", query.RequestStartTime);
            }
            if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("datediff(dd,'{0}',RequestTime)<=0", query.RequestEndTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BalanceDrawRequesDistributors ", "SerialID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public bool GetBalanceDrawRequestIsCheck(int serialid)
        {
            string query = "select IsCheck from Hishop_BalanceDrawRequest where SerialID=" + serialid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return bool.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }
        public ImportOfProductsQuery GetHishop_PruductsListCommodityCode(string CommodityCode)
        {
            string query = string.Format("select * from Hishop_productsList where CommodityCode='{0}'", CommodityCode);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ImportOfProductsQuery>(reader);
            }

           
        }
        public DbQueryResult GetCommissions(CommissionsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" State=1 ");
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (!string.IsNullOrEmpty(query.DistributorsUserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.DistributorsUserIds != "null")
                    builder.AppendFormat("UserId in ({0})", query.DistributorsUserIds);
                else
                    builder.Append("UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.FgsName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FgsName));
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountALLHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("AccountALLHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountALLHere));
            }
            if (!string.IsNullOrEmpty(query.OrderNum))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("OrderId = '{0}'", query.OrderNum);
            }
            if (!string.IsNullOrEmpty(query.StartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',TradeTime)>=0", query.StartTime);
            }
            if (!string.IsNullOrEmpty(query.EndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("  datediff(dd,'{0}',TradeTime)<=0", query.EndTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CommissionDistributors", "CommId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DataTable GetCurrentDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND OrderId in (select OrderId from dbo.Hishop_Orders where ReferralUserId={0})", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public IList<DistributorGradeInfo> GetDistributorGrades()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrade");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<DistributorGradeInfo>(reader);
            }
        }

        public DistributorsInfo GetDistributorInfo(int distributorId)
        {
            if (distributorId <= 0)
            {
                return null;
            }
            DistributorsInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT *,am.OpenId FROM aspnet_Distributors AD inner join aspnet_Members AM on ad.UserId=am.UserId where ad.UserId={0}", distributorId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorInfo(reader);
                }
            }
            return info;
        }
        /// <summary>
        /// 获取分销商的销售额
        /// </summary>
        public decimal GetDistributorDirectOrderTotal(int distributorId)
        {
            DbCommand sql = this.database.GetSqlStringCommand(string.Format("select case when SUM( ordertotal ) is null then 0  else SUM(ordertotal) end total from Hishop_Orders where ReferralUserId = {0} and OrderStatus=5", distributorId));
            return Convert.ToDecimal(this.database.ExecuteDataSet(sql).Tables[0].Rows[0]["total"]);
        }

        public DbQueryResult GetImportProducts(ImportOfProductsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.CommodityID >0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CommodityID LIKE '%{0}%'", query.CommodityID);
            }
            if (!string.IsNullOrEmpty(query.CommoditySource))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CommoditySource LIKE '%{0}%'", DataHelper.CleanSearchString(query.CommoditySource));
            }
            if (!string.IsNullOrEmpty(query.CommodityCode))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CommodityCode LIKE'%{0}%'", DataHelper.CleanSearchString(query.CommodityCode));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductsList", "CommodityID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }
        public int GetDistributorNum(DistributorGrade grade)
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", Globals.GetCurrentMemberUserId());
            if (grade != DistributorGrade.All)
            {
                query = query + " AND GradeId=" + ((int)grade);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int)reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        public DbQueryResult GetDistributors(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.IsServiceStore == 1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsServiceStore = {0}", query.IsServiceStore);
            }
            if (query.GradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("DistributorGradeId = {0}", query.GradeId);
            }
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (query.IsAgent > -1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsAgent = {0}", query.IsAgent);
            }

            if (query.ReferralStatus > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralStatus = '{0}'",query.ReferralStatus);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountAllHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("AccountAllHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountAllHere));
            }
            if (!string.IsNullOrEmpty(query.AccountAllHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("AccountAllHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountAllHere));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone='{0}'", DataHelper.CleanSearchString(query.CellPhone));
            }
            if (!string.IsNullOrEmpty(query.MicroSignal))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("MicroSignal = '{0}'", DataHelper.CleanSearchString(query.MicroSignal));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (!string.IsNullOrEmpty(query.StoreIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.StoreIds != "null")
                    builder.AppendFormat("UserId in ({0})", query.StoreIds);
                else
                    builder.Append("UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.ReferralPath))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("(ReferralPath LIKE '{0}|%' OR vd.ReferralPath LIKE '%|{0}|%' OR vd.ReferralPath LIKE '%|{0}' OR vd.ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
            }
            if (query.fgsId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("fgsid = '{0}'", query.fgsId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_DistributorsMembers", "UserId", (builder.Length > 0) ? builder.ToString() : null, "(select UserId from aspnet_Managers where ClientUserId=vw_Hishop_DistributorsMembers.UserId)sender,*");
        }
        

        public DataTable GetDistributorsCommission(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                builder.AppendFormat("AND GradeId = {0}", query.GradeId);
            }
            if (!string.IsNullOrEmpty(query.ReferralPath))
            {
                builder.AppendFormat(" AND (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, " UserId,StoreName,GradeId,CreateTime,isnull((select SUM(OrderTotal) from Hishop_Commissions where ", str, " ReferralUserId=aspnet_Distributors.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " ReferralUserId=aspnet_Distributors.UserId),0) as  CommTotal from aspnet_Distributors WHERE ", builder.ToString(), " order by CreateTime  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetDistributorsCommosion(int userId)
        {
            string query = string.Format("SELECT  GradeId,COUNT(*),SUM(OrdersTotal) AS OrdersTotal,SUM(ReferralOrders) AS ReferralOrders,SUM(ReferralBlance) AS ReferralBlance,SUM(ReferralRequestBalance) AS ReferralRequestBalance FROM aspnet_Distributors WHERE ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}' GROUP BY GradeId", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public DataTable GetDistributorsCommosion(int userId, DistributorGrade grade)
        {
            string query = string.Format("SELECT sum(OrderTotal) AS OrderTotal,sum(CommTotal) AS CommTotal from dbo.Hishop_Commissions where UserId={0} AND ReferralUserId in (select UserId from aspnet_Distributors  WHERE (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}') and GradeId={1})", userId, (int)grade);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public int GetDownDistributorNum(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT COUNT(*) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int)reader[0];
                    reader.Close();
                }
            }
            return num;
        }

        public int GetDownDistributorNumReferralOrders(string userid)
        {
            int num = 0;
            string query = string.Format("SELECT isnull(sum(ReferralOrders),0) FROM aspnet_Distributors where ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if (set.Tables[0].Rows.Count > 0)
            {
                num = int.Parse(set.Tables[0].Rows[0][0].ToString());
            }
            return num;
        }

        public DataTable GetDownDistributors(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                if (query.GradeId == 2)
                {
                    builder.AppendFormat(" AND ( nd.ReferralPath LIKE '%|{0}' OR nd.ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
                }
                if (query.GradeId == 3)
                {
                    builder.AppendFormat(" AND nd.ReferralPath LIKE '{0}|%' ", DataHelper.CleanSearchString(query.ReferralPath));
                }
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, "am.UserHead,am.UserName,nd.IsAgent,nd.UserId,nd.StoreName,nd.GradeId,nd.CreateTime,od.StoreName as ParentStoreName,isnull((select SUM(OrderTotal) from Hishop_Commissions where ", str, " ReferralUserId=nd.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " ReferralUserId=nd.UserId),0) as  CommTotal from aspnet_Distributors as nd left join aspnet_Distributors as od on od.UserId=nd.ReferralUserId left join aspnet_members am on am.userid=nd.UserId WHERE ", builder.ToString(), " order by nd.CreateTime  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetThreeDownDistributors(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                if (query.GradeId == 2)
                {
                    builder.AppendFormat(" AND ( nd.ReferralPath LIKE '%|{0}' OR nd.ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
                }
                if (query.GradeId == 3)
                {
                    builder.AppendFormat(" AND nd.ReferralPath LIKE '{0}|%' ", DataHelper.CleanSearchString(query.ReferralPath));
                }
                if (query.GradeId == 99)//三级关系全部展示
                {
                    builder.AppendFormat(" AND ( nd.ReferralPath LIKE '%|{0}' OR nd.ReferralPath='{0}' OR nd.ReferralPath LIKE '{0}|%' OR nd.ReferralPath LIKE '%|{0}|%' or nd.UserId = {0})", DataHelper.CleanSearchString(query.ReferralPath));
                }
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, "am.UserName,nd.IsAgent,nd.UserId,nd.StoreName,nd.GradeId,nd.CreateTime,od.StoreName as ParentStoreName,od.UserId as ParentUserId,isnull((select SUM(OrderTotal) from Hishop_Commissions where ", str, " UserId=nd.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " UserId=nd.UserId),0) as  CommTotal from aspnet_Distributors as nd left join aspnet_Distributors as od on od.UserId=nd.ReferralUserId left join aspnet_members am on am.userid=nd.UserId WHERE ", builder.ToString(), " order by nd.CreateTime,nd.ReferralUserId  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 获取下一级的distributor
        /// </summary>
        public DataTable GetDownDistributor(int distributorId,string startDate="",string endDate="")
        {
            string where = "";
            string commWhere = "";
            if (startDate != "") { where += string.Format(" and OrderDate >= '{0}'", startDate); commWhere += string.Format(" and TradeTime >= '{0}'", startDate); }
            if (endDate != "") { where += string.Format(" and OrderDate <= '{0}'", endDate); commWhere += string.Format(" and TradeTime <= '{0}'", endDate); }
            where += " and OrderStatus=5";//只对完成的订单进行计算
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select am.username,ad.ReferralPath,{0} as ParentUserId,ad.referralUserId,(case when ad.UserID=ad.ReferralUserId then '' else nad.StoreName end ) as ParentStoreName,ad.UserId,ad.StoreName,(select case when SUM( ordertotal ) is null then 0 else SUM(ordertotal)  end from Hishop_Orders where UserId = ad.UserId {1}) as ordertotal,(select case when SUM(commtotal) is null then 0 else SUM(commtotal) end from Hishop_Commissions where UserId=ad.UserId {2}) as CommTotal,(select case when SUM(OrderCostPrice) is null then 0 else SUM(OrderCostPrice) end from Hishop_Orders where UserId=ad.UserId {1})as CostTotal,ad.isagent from aspnet_Distributors ad left join aspnet_Members am on ad.UserId=am.UserId left join aspnet_Distributors as nad on nad.UserId=ad.ReferralUserId where CHARINDEX('|{0}|','|'+ad.ReferralPath+'|')>0 or am.userid={0} order by ad.ReferralPath ", distributorId,where,commWhere));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        /// <summary>
        /// 获取当前的distributor
        /// </summary>
        public DataTable GetDistributor(int distributorId, string startDate = "", string endDate = "")
        {
            string where = "";
            string commWhere = "";
            if (startDate != "") { where += string.Format(" and OrderDate >= '{0}'", startDate); commWhere += string.Format(" and TradeTime >= '{0}'", startDate); }
            if (endDate != "") { where += string.Format(" and OrderDate <= '{0}'", endDate); commWhere += string.Format(" and TradeTime <= '{0}'", endDate); }
            where += " and OrderStatus=5";//只对完成的订单进行计算
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select am.username,ad.ReferralPath,{0} as ParentUserId,ad.UserId,ad.StoreName,(select case when SUM( ordertotal ) is null then 0  else SUM(ordertotal) end from Hishop_Orders where UserId = {0} {1}) as ordertotal,(select case when SUM(commtotal) is null then 0  else SUM(commtotal) end  from Hishop_Commissions where UserId={0} {2})  as CommTotal,(select case when SUM(OrderCostPrice) is null then 0 else SUM(OrderCostPrice) end from Hishop_Orders where UserId={0} {1})as CostTotal,ad.isagent, (case when ad.UserID=ad.ReferralUserId then '' else nad.StoreName end ) as pname from aspnet_Distributors ad left join aspnet_Members am on ad.UserId=am.UserId left join aspnet_Distributors as nad on nad.UserId=ad.ReferralUserId where ad.UserId={0} ", distributorId,where,commWhere));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        //获得下级
        public DataTable GetDownDis(int UserID, string startDate = "", string endDate = "")
        {
            string where = "";
            string commWhere = "";
            if (startDate != "") { where += string.Format(" and OrderDate >= '{0}'", startDate); commWhere += string.Format(" and TradeTime >= '{0}'", startDate); }
            if (endDate != "") { where += string.Format(" and OrderDate <= '{0}'", endDate); commWhere += string.Format(" and TradeTime <= '{0}'", endDate); }
            where += " and OrderStatus=5";//只对完成的订单进行计算
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select am.username,ad.ReferralPath,{0} as ParentUserId,ad.UserId,ad.StoreName,(select case when SUM( ordertotal ) is null then 0  else SUM(ordertotal) end from Hishop_Orders where UserId = ad.UserId {1}) as ordertotal,(select case when SUM(commtotal) is null then 0 else SUM(commtotal) end  from Hishop_Commissions where UserId=ad.UserId {2})  as CommTotal,(select case when SUM(OrderCostPrice) is null then 0 else SUM(OrderCostPrice) end from Hishop_Orders where UserId=ad.UserId {1})as CostTotal,ad.isagent,nad.StoreName as pname from aspnet_Distributors ad left join aspnet_Members am on ad.UserId=am.UserId left join aspnet_Distributors as nad on nad.UserId=ad.ReferralUserId  where ad.ReferralUserId={0} and ad.UserId!=ad.ReferralUserId ", UserID, where, commWhere));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        /// <summary>
        /// 得到所有一级代理商
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFirstDis(string startDate = "", string endDate = "")
        {
            string where = "";
            string commWhere = "";
            if (startDate != "") { where += string.Format(" and OrderDate >= '{0}'", startDate); commWhere += string.Format(" and TradeTime >= '{0}'", startDate); }
            if (endDate != "") { where += string.Format(" and OrderDate <= '{0}'", endDate); commWhere += string.Format(" and TradeTime <= '{0}'", endDate); }
            where += " and OrderStatus=5";//只对完成的订单进行计算

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select am.username,ad.ReferralPath,ad.UserId as ParentUserId,ad.UserId,ad.StoreName,(select case when SUM( ordertotal ) is null then 0  else SUM(ordertotal) end from Hishop_Orders where UserId = ad.UserId {0}) as ordertotal,(select case when SUM(commtotal) is null then 0 else SUM(commtotal) end  from Hishop_Commissions where UserId=ad.UserId {1})  as CommTotal,(select case when SUM(OrderCostPrice) is null then 0 else SUM(OrderCostPrice) end from Hishop_Orders where UserId=ad.UserId {0})as CostTotal,ad.isagent,'' as pname from aspnet_Distributors ad left join aspnet_Members am on ad.UserId=am.UserId  where ad.UserId=ad.ReferralUserId",where,commWhere));
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 获取所有的一级分销商
        /// </summary>
        public DataTable GetFirstDistributors(string startDate = "", string endDate = "")
        {
            string where = "";
            string commWhere = "";
            if (startDate != "") { where += string.Format(" and OrderDate >= '{0}'", startDate); commWhere += string.Format(" and TradeTime >= '{0}'", startDate); }
            if (endDate != "") { where += string.Format(" and OrderDate <= '{0}'", endDate); commWhere += string.Format(" and TradeTime <= '{0}'", endDate); }
            where += " and OrderStatus=5";//只对完成的订单进行计算
            string selectSql = string.Format("select am.username,ad.UserId as ParentUserId,ad.ReferralUserId,ad.UserId,StoreName,(select case when SUM( ordertotal ) is null then 0  else SUM(ordertotal) end from Hishop_Orders where UserId = ad.UserId {0}) as ordertotal,(select case when SUM(commtotal) is null then 0 else SUM(commtotal) end  from Hishop_Commissions where UserId=ad.UserId {1}) as CommTotal,(select case when SUM(OrderCostPrice) is null then 0 else SUM(OrderCostPrice) end from Hishop_Orders where UserId=ad.UserId {0})as CostTotal,isagent,childCount=(Select COUNT(*) From aspnet_Distributors Where ReferralUserId=ad.UserId And ReferralUserId<>UserId) from aspnet_Distributors ad left join aspnet_Members am on ad.UserId=am.UserId where ad.UserId=ad.ReferralUserId",where,commWhere);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
        {
            DistributorGradeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorGradeInfo(reader);
                }
            }
            return info;
        }

        public decimal GetUserCommissions(int userid, DateTime fromdatetime)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" State=1 ");
            if (userid > 0)
            {
                builder.Append(" and UserID=" + userid);
            }
            builder.Append(" and TradeTime>='" + fromdatetime.ToString("yyyy-MM-dd") + " 00:00:00'");
            string query = " select isnull(sum(CommTotal),0) from Hishop_Commissions where " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return decimal.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
        }

        public DataSet GetUserRanking(int userid)
        {
            string query = string.Format("select d.UserId,d.Logo,d.OrdersTotal,d.ReferralBlance+d.ReferralRequestBalance as Blance,d.StoreName,(select count(0) from aspnet_Distributors a where (a.ReferralBlance+a.ReferralRequestBalance>(d.ReferralBlance+d.ReferralRequestBalance) or (a.ReferralBlance+a.ReferralRequestBalance=(d.ReferralBlance+d.ReferralRequestBalance) and a.UserID<d.UserID)))+1 as ccount  from aspnet_Distributors d where UserID={0};select top 10 UserId,Logo,OrdersTotal,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors order by Blance desc,userid asc;select top 10 UserId,Logo,OrdersTotal,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where (ReferralPath like '{0}|%' or ReferralPath like '%|{0}' or ReferralPath = '{0}') order by Blance desc", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        /// <summary>
        /// 根据条件获取data 2017-7-21 yk
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetUserRankingQuery(DistributorsQuery query, int userid)
        {
            int num = ((query.PageIndex - 1) * query.PageSize) + 1;
            int num2 = (num + query.PageSize) - 1;
            string str = string.Format("select d.UserId,d.Logo,d.OrdersTotal,d.ReferralBlance+d.ReferralRequestBalance as Blance,d.StoreName,(select count(0) from aspnet_Distributors a where (a.ReferralBlance+a.ReferralRequestBalance>(d.ReferralBlance+d.ReferralRequestBalance) or (a.ReferralBlance+a.ReferralRequestBalance=(d.ReferralBlance+d.ReferralRequestBalance) and a.UserID<d.UserID)))+1 as ccount,st.Ico  from aspnet_Distributors d left join aspnet_DistributorStarLevel st on d.StarLevelID=st.StarLevelID  where UserID={0};select *from (select ROW_NUMBER() OVER (ORDER BY (ReferralBlance+ReferralRequestBalance)  Desc) AS RowNumber,st.Ico,ReferralBlance+ReferralRequestBalance as Blance,(select count(UserId) from  aspnet_Distributors ) as number,UserId,Logo,OrdersTotal,StoreName  from aspnet_Distributors d left join aspnet_DistributorStarLevel st on d.StarLevelID=st.StarLevelID)t where t.RowNumber  BETWEEN {1} AND {2};select top 10 UserId,Logo,OrdersTotal,ReferralBlance+ReferralRequestBalance as Blance,StoreName  from aspnet_Distributors where (ReferralPath like '{0}|%' or ReferralPath like '%|{0}' or ReferralPath = '{0}') order by Blance desc", userid, num, num2);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
        public int IsExiteDistributorsByStoreName(string storname)
        {
            string query = "SELECT UserId FROM aspnet_Distributors WHERE StoreName=@StoreName";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, storname);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            return ((obj2 != null) ? ((int)obj2) : 0);
        }

        public bool IsExitsCommionsRequest(int userId)
        {
            bool flag = false;
            string query = "SELECT * FROM dbo.Hishop_BalanceDrawRequest WHERE IsCheck=0 AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 根据条件查询数据集
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public DataTable GetCommosionByTime( string where)
        {
            string query = string.Format("SELECT SUM(CommTotal)as CommTotal ,ReferralRequestBalance ,(SUM(CommTotal)- ReferralRequestBalance) as ktx FROM (select CommTotal, ReferralRequestBalance from Hishop_Commissions hc join aspnet_Distributors ad on hc.UserId =ad.UserId WHERE "+where+")T GROUP BY ReferralRequestBalance", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }
        public DataTable OrderIDGetCommosion(string orderid)
        {
            string query = string.Format("SELECT CommId,Userid,OrderTotal,CommTotal from Hishop_Commissions where OrderId='" + orderid + "' ", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            if ((set == null) || (set.Tables.Count <= 0))
            {
                return null;
            }
            return set.Tables[0];
        }

        public void RemoveDistributorProducts(List<int> productIds, int distributorId)
        {
            string str = string.Join<int>(",", productIds);
            string query = "DELETE FROM Hishop_DistributorProducts WHERE UserId=@UserId AND ProductId IN (" + str + ");";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public string SendRedPackToBalanceDrawRequest(int serialid)
        {
            if (!SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
            {
                return "管理员后台未开启微信付款！";
            }
            string query = "select a.SerialID,a.userid,a.Amount,b.OpenID,isnull(b.OpenId,'') as OpenId from Hishop_BalanceDrawRequest a inner join aspnet_Members b on a.userid=b.userid where SerialID=@SerialID and IsCheck=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, serialid);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            string str4 = string.Empty;
            int userid = 0;
            if (table.Rows.Count > 0)
            {
                str4 = table.Rows[0]["OpenId"].ToString();
                userid = int.Parse(table.Rows[0]["UserID"].ToString());
                decimal num2 = decimal.Parse(table.Rows[0]["Amount"].ToString()) * 100M;
                int amount = Convert.ToInt32(num2);
                if (string.IsNullOrEmpty(str4))
                {
                    return "用户未绑定微信号";
                }
                query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=" + table.Rows[0]["SerialID"].ToString();
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                if (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0)
                {
                    return "-1";
                }
                return (this.CreateSendRedpackRecord(serialid, userid, str4, amount, "您的提现申请已成功", "恭喜您提现成功!") ? "1" : "提现操作失败");
            }
            return "该用户没有提现申请";
        }

        public bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
        {
            string query = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance-@ReferralBlance,ReferralRequestBalance=ReferralRequestBalance+@ReferralRequestBalance  WHERE UserId=@UserId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
       

        public bool UpdateBalanceDrawRequest(int Id, string Remark)
        {
            string query = "UPDATE Hishop_BalanceDrawRequest set Remark=@Remark,IsCheck=1,CheckTime=getdate() WHERE SerialID=@SerialID ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, Remark);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCalculationCommission(string UserId, string ReferralUserId, string OrderId, decimal OrderTotal, decimal resultCommTatal)
        {
            string query = "";
            object obj2 = query + "begin try  " + "  begin tran TranUpdate";
            obj2 = string.Concat(new object[] { obj2, " INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(", UserId, ",", ReferralUserId, ",'", OrderId, "','", DateTime.Now.ToString(), "',", OrderTotal, ",", resultCommTatal, ",1,'',1) ;" });
            obj2 = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+", resultCommTatal, "  WHERE UserId=", UserId, "; " });
            query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", OrderTotal, ",ReferralOrders=ReferralOrders+1  WHERE UserId=", ReferralUserId, "; " }) + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 2017-07-27, 批发价不计佣金，服务订单不计佣金，但是要累加店铺订单总额
        /// </summary>
        /// <param name="disuserid">店铺ID</param>
        /// <param name="OrderTotal">订单金额</param>
        /// <returns></returns>
        public bool UpdateDistributorOrdersTotal(string disuserid, decimal OrderTotal)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET OrdersTotal=OrdersTotal+@OrdersTotal,ReferralOrders=ReferralOrders+1  WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, disuserid);
            this.database.AddInParameter(sqlStringCommand, "OrdersTotal", DbType.Decimal, OrderTotal);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributor(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,BackImage=@BackImage,RequestAccount=@RequestAccount,ReferralOrders=@ReferralOrders,ReferralBlance=@ReferralBlance, ReferralRequestBalance=@ReferralRequestBalance,StoreDescription=@StoreDescription,ReferralStatus=@ReferralStatus,AgentGradeId=@AgentGradeId WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            this.database.AddInParameter(sqlStringCommand, "ReferralOrders", DbType.Int64, distributor.ReferralOrders);
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.Int64, distributor.ReferralStatus);
            this.database.AddInParameter(sqlStringCommand, "ReferralBlance", DbType.Decimal, distributor.ReferralBlance);
            this.database.AddInParameter(sqlStringCommand, "ReferralRequestBalance", DbType.Decimal, distributor.ReferralRequestBalance);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "AgentGradeId", DbType.Int32, distributor.AgentGradeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorById(string requestAccount, int distributorId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET RequestAccount=@RequestAccount WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, requestAccount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributorMessage(DistributorsInfo distributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET StoreName=@StoreName,Logo=@Logo,BackImage=@BackImage,StoreDescription=@StoreDescription,RequestAccount=@RequestAccount WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributor.UserId);
            this.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, distributor.StoreName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, distributor.Logo);
            this.database.AddInParameter(sqlStringCommand, "BackImage", DbType.String, distributor.BackImage);
            this.database.AddInParameter(sqlStringCommand, "StoreDescription", DbType.String, distributor.StoreDescription);
            this.database.AddInParameter(sqlStringCommand, "RequestAccount", DbType.String, distributor.RequestAccount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateGradeId(ArrayList GradeIdList, ArrayList ReferralUserIdList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < ReferralUserIdList.Count; i++)
            {
                if (!GradeIdList[i].Equals(0))
                {
                    object obj2 = query;
                    query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors SET DistributorGradeId=", GradeIdList[i], " where UserId=", ReferralUserIdList[i], "; " });
                }
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateNotSetCalculationCommission(ArrayList UserIdList, ArrayList OrdersTotal)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set OrdersTotal=OrdersTotal+", OrdersTotal[i], " WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateSendRedpackRecord(int serialid, int num, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BalanceDrawRequest set RedpackRecordNum=@RedpackRecordNum where SerialID=@SerialID");
            this.database.AddInParameter(sqlStringCommand, "RedpackRecordNum", DbType.Int32, num);
            this.database.AddInParameter(sqlStringCommand, "SerialID", DbType.Int32, serialid);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateTwoCalculationCommission(ArrayList UserIdList, string ReferralUserId, string OrderId, ArrayList OrderTotalList, ArrayList CommTatalList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                obj2 = string.Concat(new object[] { obj2, " INSERT INTO   [Hishop_Commissions](UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,CommRemark,State)values(", UserIdList[i], ",", ReferralUserId, ",'", OrderId, "','", DateTime.Now.ToString(), "',", OrderTotalList[i], ",", CommTatalList[i], ",1,'',1) ;" });
                obj2 = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+", CommTatalList[i], "  WHERE UserId=", UserIdList[i], "; " });
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", OrderTotalList[i], ",ReferralOrders=ReferralOrders+1  WHERE UserId=", UserIdList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateTwoDistributorsOrderNum(ArrayList useridList, ArrayList OrdersTotalList)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < useridList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "  UPDATE aspnet_Distributors set  OrdersTotal=OrdersTotal+", useridList[i], ",ReferralOrders=ReferralOrders+1  WHERE UserId=", OrdersTotalList[i], "; " });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable SelectDistributorsByWhere(string where)
        {
            if (where != "") where = " Where " + where;
            string selectSql = "Select * From aspnet_Distributors" + where;
            return this.database.ExecuteDataSet(CommandType.Text, selectSql).Tables[0];
        }

        public void CommitDistributors(DataTable dtData)
        {
            SqlConnection cn = new SqlConnection(this.database.ConnectionString);
            using (SqlDataAdapter sqlAdpt = new SqlDataAdapter("Select * From aspnet_Distributors", cn))
            {
                sqlAdpt.MissingSchemaAction = MissingSchemaAction.Add;
                SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdpt);
                sqlAdpt.Update(dtData);
            }
        }

        //获取所有平级代理商和所有分销商
        public DataTable GetDownDistributorsAndA(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                /*AgentPath的查询情况分四种:
                 * 1:本身就是单个匹配,{0}
                 * 2:最顶级的(最左边的),{0}|%
                 * 3:中间的,%|{0}|%
                 * 4:上一级的(最右边的),%|{0}
                 */
                if (query.GradeId == 2)//所有代理商
                {
                    builder.AppendFormat(" AND ( nd.AgentPath LIKE '{0}|%' OR nd.AgentPath='{0}' OR nd.AgentPath LIKE '%|{0}|%' OR nd.AgentPath LIKE '%|{0}') AND nd.IsAgent = 1 ", DataHelper.CleanSearchString(query.AgentPath));
                }
                if (query.GradeId == 3)//所有分销商
                {
                    builder.AppendFormat(" AND ( nd.AgentPath LIKE '{0}|%' OR nd.AgentPath='{0}' OR nd.AgentPath LIKE '%|{0}|%' OR nd.AgentPath LIKE '%|{0}') AND nd.IsAgent = 0 ", DataHelper.CleanSearchString(query.AgentPath));
                }
                if (query.GradeId == 99)//代理商和分销商一起查
                {
                    builder.AppendFormat(" AND ( nd.AgentPath LIKE '{0}|%' OR nd.AgentPath='{0}' OR nd.AgentPath LIKE '%|{0}|%' OR nd.AgentPath LIKE '%|{0}' or nd.UserId = {0}) ", DataHelper.CleanSearchString(query.AgentPath));
                }
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, " am.UserName,am.UserHead,nd.UserId,nd.IsAgent,nd.StoreName,nd.GradeId,nd.CreateTime,od.StoreName as ParentStoreName,od.UserId as ParentUserId,isnull((select SUM(OrderTotal) from Hishop_Commissions  where ", str, " UserId=nd.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " UserId=nd.UserId),0) as  CommTotal from aspnet_Distributors as nd left join aspnet_Distributors as od on od.UserId=nd.ReferralUserId left join aspnet_members am on am.userid=nd.UserId  WHERE ", builder.ToString(), " order by CreateTime,nd.ReferralUserId  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetDownDistributorsAndAgents(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1");
            string str = "";
            if (query.GradeId > 0)
            {
                /*AgentPath的查询情况分四种:
                 * 1:本身就是单个匹配,{0}
                 * 2:最顶级的(最左边的),{0}|%
                 * 3:中间的,%|{0}|%
                 * 4:上一级的(最右边的),%|{0}
                 */
                if (query.GradeId == 2)//所有代理商
                {
                    builder.AppendFormat(" AND ( nd.AgentPath LIKE '{0}|%' OR nd.AgentPath='{0}' OR nd.AgentPath LIKE '%|{0}|%' OR nd.AgentPath LIKE '%|{0}') AND nd.IsAgent = 1 ", DataHelper.CleanSearchString(query.AgentPath));
                }
                if (query.GradeId == 3)//所有分销商
                {
                    builder.AppendFormat(" AND ( nd.AgentPath LIKE '{0}|%' OR nd.AgentPath='{0}' OR nd.AgentPath LIKE '%|{0}|%' OR nd.AgentPath LIKE '%|{0}') AND nd.IsAgent = 0 ", DataHelper.CleanSearchString(query.AgentPath));
                }
            }
            if (query.UserId > 0)
            {
                str = " UserId=" + query.UserId + " AND ";
            }
            string str2 = string.Concat(new object[] { "select TOP ", query.PageSize, "am.UserHead, nd.UserId,nd.StoreName,nd.GradeId,nd.CreateTime,od.StoreName as ParentStoreName,isnull((select SUM(OrderTotal) from Hishop_Commissions  where ", str, " ReferralUserId=nd.UserId),0) as OrderTotal,isnull((select SUM(CommTotal) from Hishop_Commissions where ", str, " ReferralUserId=nd.UserId),0) as  CommTotal from aspnet_Distributors as nd left join aspnet_Distributors as od on od.UserId=nd.ReferralUserId left join aspnet_members am on am.userid=nd.UserId WHERE ", builder.ToString(), " order by CreateTime,nd.ReferralUserId  desc" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        public DataTable GetDistributorProductRangeByUserid(int userid)
        {
            string selectSql = string.Format("Select * From Hishop_DistributorProductRange Where userid={0}", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        public void CommitDistributorProductRange(DataTable dtData)
        {
            SqlConnection cn = new SqlConnection(this.database.ConnectionString);
            using (SqlDataAdapter sqlAdpt = new SqlDataAdapter("Select * From Hishop_DistributorProductRange", cn))
            {
                sqlAdpt.MissingSchemaAction = MissingSchemaAction.Add;
                SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdpt);
                sqlAdpt.Update(dtData);
            }
        }
        /// <summary>
        /// 累加分销商店铺访问量,触发一次访问量+1
        /// </summary>
        /// <param name="distributorId">分销商id</param>
        public void UpdateDistributorVisitCount(int memberId, int distributorId)
        {
            string sql = string.Empty;
            if (GetDayDistributorVisitInfo(memberId, distributorId, DateTime.Now.ToString("yyyy-MM-dd")).Rows.Count <= 0)//如果当天已还未插入信息,则插入
            {
                sql = string.Format("INSERT INTO YiHui_DistributorVisitInfo (memberid,distributorid,visitcountperday,visitdate) values({0},{1},1,CONVERT(varchar(100), GETDATE(), 23))", memberId, distributorId);
            }
            else
            {
                sql = string.Format("UPDATE YiHui_DistributorVisitInfo set visitCountPerday = ISNULL(visitCountPerday,0)+1  WHERE memberId={0} and distributorId={1}", memberId, distributorId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int Updateaspnet_DistributorsUserId(int userid) {
            string Sqlupdate = string.Format("Update aspnet_Distributors set DistributorGradeId=(select top 1 GradeId from aspnet_DistributorGrade order by CommissionsLimit desc) where UserId='{0}'",userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sqlupdate);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int Updateaspnet_DistributorsServiceStoreId(int userid){ 
            string Sqlupdate =string.Format("Update aspnet_Distributors set IsServiceStore='1' where UserId='{0}'",userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sqlupdate);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int Updateaspnet_DistributorsServiceToreId(int userid)
        {
            string Sqlupdate = string.Format("Update aspnet_Distributors set IsServiceStore='0' where UserId='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sqlupdate);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int UpdateDistributorsSetNeiGouStore(int userid)
        {
            string Sqlupdate = string.Format("Update aspnet_Distributors set IsNeiGouStore='1' where UserId='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sqlupdate);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public int UpdateDistributorsCancelNeiGouStore(int userid)
        {
            string Sqlupdate = string.Format("Update aspnet_Distributors set IsNeiGouStore='0' where UserId='{0}'", userid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(Sqlupdate);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 获取分销商店铺访问总数
        /// </summary>
        /// <param name="distributorId">分销商id</param>
        /// <param name="date">日期</param>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public DataTable GetDistributorVisitCount(int distributorId, string date = "", int memberId = -1)
        {
            string sql = string.Format("SELECT SUM(visitcountperday) visitCount from YiHui_DistributorVisitInfo where DistributorId={0} ", distributorId);
            if (memberId != -1)
            {
                sql += " and memberId=" + memberId;
            }
            if (date != "")
            {
                sql += string.Format(" and visitDate >= '{0}'", date);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 查询访问记录分页列表
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        public DbQueryResult GetDistributorVisit(VisitQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.DistributorsUserId > 0)
            {
                builder.AppendFormat(" AND DistributorId = {0}", query.DistributorsUserId);
            }
            if (!string.IsNullOrEmpty(query.StartTime))
            {
                builder.AppendFormat(" AND VisitDate = '{0}'", query.StartTime);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.YiHui_DistributorVisitInfo", "DVId", builder.ToString(), "*,(select UserName from dbo.aspnet_Members where UserId = MemberId) as UserName");
        }


        /// <summary>
        /// 获取某天的分销商被访问详情
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DataTable GetDayDistributorVisitInfo(int memberId, int distributorId, string dateTime)
        {
            string sql = string.Format("SELECT * from YiHui_DistributorVisitInfo where visitdate='{0}' and distributorId={1} and memberId={2}", dateTime, distributorId, memberId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取所有访问信息
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DataTable GetDistributorVisitInfo()
        {
            string sql = "SELECT * from YiHui_DistributorVisitInfo order by VisitDate desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取分销商的会员数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DataTable GetDistributorMemberCount(int distributorId, string date = "")
        {
            string sql = string.Format("select COUNT(UserId) as memberCount from dbo.aspnet_Members where DistributorUserId={0}", distributorId);
            if (!string.IsNullOrEmpty(date))
            {
                sql = string.Format("select COUNT(UserId) as memberCount from dbo.aspnet_Members where DistributorUserId={0} and CreateDate >= '{1}'", distributorId, date);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取代理商下分销商的访问信息
        /// </summary>
        public DataTable GetAgentDistributorsVisitInfo(int agentId, string date = "")
        {
            string sql = string.Format("SELECT * from YiHui_DistributorVisitInfo where DistributorId in (select userid from aspnet_Distributors where charindex('|{0}|' ,'|'+ AgentPath +'|')> 0) ",agentId);
            if (date != "")
            {
                sql += string.Format(" and visitDate='{0}'", date);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 删除分销商
        /// </summary>
        public bool DeleteDistributor(int userId)
        {
            string sql = string.Empty;
            sql = string.Format("delete from dbo.aspnet_Managers where UserName in (select accountALLHere from dbo.CW_StoreInfo where id in (select StoreId from  dbo.aspnet_Distributors where UserId = {0}));delete from aspnet_members where UserId={0};delete from aspnet_Distributors where UserId={0}", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand)>0;
        }

        public bool DeleteProduct(int CommodityID)
        {
            string sql = string.Empty;
            sql = string.Format("delete from Hishop_ProductsList where CommodityID='{0}'", CommodityID);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool DeleteChannel(Guid ChannelId)
        {
            string sqldelete = string.Format("delete from Hishop_ChannelList where id='{0}'", ChannelId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sqldelete);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 根据子账号id获取前台绑定的分销商id(目前仅用于爽爽挝啡多店铺子账号管理)
        /// </summary>
        public int GetSenderDistributorId(string sender)
        {
            if (sender == null)
                return 0;
            string sql = string.Format("select UserId from aspnet_Distributors where UserId = (select ClientUserId from aspnet_Managers where UserId = {0})",sender);
            return Convert.ToInt32(this.database.ExecuteScalar(CommandType.Text, sql));
        }

        //  SELECT accountALLHere FROM dbo.CW_StoreInfo WHERE id=(SELECT StoreId FROM aspnet_Distributors WHERE UserId=1111)
        public string GetOrderStoreKeyId(string ReferralUserId)
        {
            string keyid = "";
            string selectSql = string.Format("SELECT storekeyid FROM dbo.CW_StoreInfo WHERE id=(SELECT StoreId FROM aspnet_Distributors WHERE UserId={0})", ReferralUserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (dt.Rows.Count > 0)
            {
                keyid = dt.Rows[0]["storekeyid"].ToString();
            }

            return keyid;
        }
        /// <summary>
        /// 门店认证查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  DataTable SelectDistributorsByStoreId(int id)
        {
            string selectSql = string.Format("Select * From aspnet_Distributors Where StoreId={0}", id);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 获取店铺用户总数
        /// </summary>
        /// <param name="distributorId">店铺ID</param>
        /// <returns></returns>
        public int GetDistributorUserCount(int distributorId)
        {
            string strSql = "SELECT COUNT(*) FROM dbo.aspnet_Members WHERE DistributorUserId = @DistributorUserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, distributorId);
            return (int)this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 分页获取店铺的所有用户信息
        /// </summary>
        /// <param name="usersquery">查询实体</param>
        /// <returns></returns>
        public DbQueryResult GetDistributorUserInfo(DistributorsUsersQuery usersquery)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (usersquery.DistributorUserId > 0)
            {
                builder.AppendFormat(" AND DistributorUserId = {0}", usersquery.DistributorUserId);
            }
            if (!string.IsNullOrEmpty(usersquery.KeyWord))
            {
                builder.AppendFormat(" AND (UserName like '%{0}%' or CellPhone like '%{0}%' )", DataHelper.CleanSearchString(usersquery.KeyWord));
            }
            if (usersquery.StartDate != null && usersquery.StartDate > DateTime.MinValue)
            {
                builder.AppendFormat(" AND CreateDate like '%{0}%'", usersquery.StartDate.ToShortDateString());
            }
            //得到变动排序列对应的数据值
            string strFields = "*";
            string startdate;
            string enddate;
            DateTime datanew = DateTime.Now;
            if (!string.IsNullOrEmpty(usersquery.SortBy))
            {
                switch (usersquery.SortBy)
                {
                    case "jinri":
                        startdate = datanew.ToShortDateString();
                        enddate = datanew.AddDays(1).ToShortDateString();
                        strFields += string.Format(",dbo.Fn_wt_GetUserConsumeByDate(UserId,'{0}','{1}') as jinri", startdate, enddate);
                        break;
                    case "benyue":
                        startdate = datanew.ToString("yyyy-MM-01");
                        enddate = datanew.AddMonths(1).ToString("yyyy-MM-01");
                        strFields += string.Format(",dbo.Fn_wt_GetUserConsumeByDate(UserId,'{0}','{1}') as benyue", startdate, enddate);
                        break;
                    case "jidu":
                        int m = datanew.Month;
                        if (m <= 3)
                        {
                            startdate = datanew.ToString("yyyy-01-01");
                            enddate = datanew.ToString("yyyy-04-01");
                        }
                        else if (m <= 6)
                        {
                            startdate = datanew.ToString("yyyy-04-01");
                            enddate = datanew.ToString("yyyy-07-01");
                        }
                        else if (m <= 9)
                        {
                            startdate = datanew.ToString("yyyy-07-01");
                            enddate = datanew.ToString("yyyy-10-01");
                        }
                        else
                        {
                            startdate = datanew.ToString("yyyy-10-01");
                            enddate = datanew.AddYears(1).ToString("yyyy-01-01");
                        }
                        strFields += string.Format(",dbo.Fn_wt_GetUserConsumeByDate(UserId,'{0}','{1}') as jidu", startdate, enddate);
                        break;
                    case "niandu":
                        startdate = datanew.ToString("yyyy-01-01");
                        enddate = datanew.AddYears(1).ToString("yyyy-01-01");
                        strFields += string.Format(",dbo.Fn_wt_GetUserConsumeByDate(UserId,'{0}','{1}') as niandu", startdate, enddate);
                        break;
                }
            }
            return DataHelper.PagingByTopsort(usersquery.PageIndex, usersquery.PageSize, usersquery.SortBy, usersquery.SortOrder, usersquery.IsCount, "dbo.aspnet_Members", "UserId", builder.ToString(), strFields);
        }























        /// <summary>
        /// 根据条件得到门店数据集
        /// </summary>
        /// <param name="query">条件实体对象</param>
        /// <returns></returns>
        public DataTable GetExprotDistrbutor(DistributorsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.IsServiceStore == 1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsServiceStore = {0}", query.IsServiceStore);
            }
            if (query.GradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("DistributorGradeId = {0}", query.GradeId);
            }
            if (query.UserId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserId = {0}", query.UserId);
            }
            if (query.IsAgent > -1)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("IsAgent = {0}", query.IsAgent);
            }

            if (query.ReferralStatus > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("ReferralStatus = '{0}'", query.ReferralStatus);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountAllHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("AccountAllHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountAllHere));
            }
            if (!string.IsNullOrEmpty(query.CellPhone))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("CellPhone='{0}'", DataHelper.CleanSearchString(query.CellPhone));
            }
            if (!string.IsNullOrEmpty(query.MicroSignal))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("MicroSignal = '{0}'", DataHelper.CleanSearchString(query.MicroSignal));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (!string.IsNullOrEmpty(query.StoreIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.StoreIds != "null")
                    builder.AppendFormat("UserId in ({0})", query.StoreIds);
                else
                    builder.Append("UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.ReferralPath))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("(ReferralPath LIKE '{0}|%' OR vd.ReferralPath LIKE '%|{0}|%' OR vd.ReferralPath LIKE '%|{0}' OR vd.ReferralPath='{0}')", DataHelper.CleanSearchString(query.ReferralPath));
            }
            string strSql = "select * from dbo.vw_Hishop_DistributorsMembers";
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                strSql += " where "+builder.ToString();
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }



        /// <summary>
        /// 根据条件得到提现数据集
        /// </summary>
        /// <param name="query">实体对象</param>
        /// <returns></returns>
        public DataTable GetExportBalanceDrawRequest(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountALLHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AccountALLHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountALLHere));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.RequestTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),RequestTime,120)='{0}'", query.RequestTime);
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            if (!string.IsNullOrEmpty(query.CheckTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" convert(varchar(10),CheckTime,120)='{0}'", query.CheckTime);
            }
            if (!string.IsNullOrEmpty(query.RequestStartTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" datediff(dd,'{0}',RequestTime)>=0", query.RequestStartTime);
            }
            if (!string.IsNullOrEmpty(query.RequestEndTime.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("datediff(dd,'{0}',RequestTime)<=0", query.RequestEndTime);
            }

            string strSql = "select * from vw_Hishop_BalanceDrawRequesDistributors";
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                strSql += " where " + builder.ToString();
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取分公司下的已认证门店数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetFilialeStatistics(FilialeStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.FilialeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FilialeName));
            }
            if (!string.IsNullOrEmpty(query.id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id = {0}", DataHelper.CleanSearchString(query.id));
            }
            if (!string.IsNullOrEmpty(query.ids))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id in ({0})", DataHelper.CleanSearchString(query.ids));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BalanceDrawRequiest", "id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        /// <summary>
        /// 获取分公司下的销售总量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbQueryResult GetFilialeStatisticsOrderJL(FilialeStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.FilialeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FilialeName));
            }
            if (!string.IsNullOrEmpty(query.id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id = {0}", DataHelper.CleanSearchString(query.id));
            }
            if (!string.IsNullOrEmpty(query.ids))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id in ({0})", DataHelper.CleanSearchString(query.ids));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrdersJL", "id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        /// <summary>
        ///获取所有分公司下门店的粉丝数
        /// </summary>
        /// <param name="query">实体</param>
        /// <returns></returns>
        public DbQueryResult GetFilialeStatisticsFans(FilialeStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.FilialeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FilialeName));
            }
            if (!string.IsNullOrEmpty(query.id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id = {0}", DataHelper.CleanSearchString(query.id));
            }
            if (!string.IsNullOrEmpty(query.ids))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id in ({0})", DataHelper.CleanSearchString(query.ids));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Fans", "id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        /// <summary>
        /// 获取分公司下的佣金金额
        /// </summary>
        /// <param name="query">实体</param>
        /// <returns></returns>
        public DbQueryResult GetFilialeStatisticscount(FilialeStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.FilialeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FilialeName));
            }
            if (!string.IsNullOrEmpty(query.id))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id = {0}", DataHelper.CleanSearchString(query.id));
            }
            if (!string.IsNullOrEmpty(query.ids))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" id in ({0})", DataHelper.CleanSearchString(query.ids));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vm_Hishop_FilialeCount", "id", (builder.Length > 0) ? builder.ToString() : null, "*");
        }


        /// <summary>
        ///获取分公司的认证门店数、销售总量、佣金总额、粉丝数，用于导出Excel
        /// </summary>
        /// <param name="query">实体</param>
        /// <returns></returns>
        public DataTable GetExportFilialeStatistics(FilialeStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.FilialeName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" a.fgsName LIKE '%{0}%'", DataHelper.CleanSearchString(query.FilialeName));
            }
            //if (!string.IsNullOrEmpty(query.UserId))
            //{
            //    if (builder.Length > 0)
            //    {
            //        builder.Append(" AND ");
            //    }
            //    builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            //}
            //if (!string.IsNullOrEmpty(query.UserIds))
            //{
            //    if (builder.Length > 0)
            //    {
            //        builder.Append(" AND ");
            //    }
            //    if (query.UserIds != "null")
            //        builder.AppendFormat(" UserId in ({0})", query.UserIds);
            //    else
            //        builder.Append(" UserId = -1");
            //}
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }

            string strSql = @"select a.id,a.fgsName,a.认证门店数,b.佣金总额,c.粉丝数,d.订单总数,d.订单总额
                            from vm_Hishop_FilialeCount a 
                            left join vw_Hishop_BalanceDrawRequiest b on a.id=b.id 
                            left join vw_Hishop_Fans c on b.id=c.id
                            left join vw_Hishop_OrdersJL d on c.id=d.id";
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                strSql += " where " + builder.ToString();
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据条件查询门店统计数据集
        /// </summary>
        /// <param name="where1"></param>
        /// <param name="where2"></param>
        /// <param name="where3"></param>
        /// <returns></returns>
        public DataTable GetStoreStatisticsByWhere(string where1, string where2, string where3, string where4)
        {
            string strSql = string.Format(@"SELECT  a.fgsid, fi.fgsName, a.accountALLHere, b.StoreName, b.StoreId, g.CommTotal, c.UserCount, e.OrderCount, f.OrderTotal
					FROM  dbo.CW_StoreInfo AS a INNER JOIN
                      dbo.aspnet_Distributors AS b ON a.id = b.StoreId LEFT OUTER JOIN 
                      dbo.CW_Filiale AS fi ON a.fgsid = fi.id  LEFT OUTER JOIN 
                          (SELECT     d.UserId, COUNT(m.UserId) AS UserCount
                            FROM          dbo.aspnet_Members AS m LEFT OUTER JOIN
                                                   dbo.aspnet_Distributors AS d ON m.DistributorUserId = d.UserId 
                                                   where {0} 
                            GROUP BY d.UserId) AS c ON c.UserId = b.UserId LEFT OUTER JOIN
                          (SELECT     d.UserId, COUNT(o.OrderId) AS OrderCount
                            FROM   dbo.aspnet_Distributors AS d LEFT OUTER JOIN
                                    dbo.Hishop_Orders AS o ON d.UserId = o.ReferralUserId    
                            WHERE o.OrderStatus not in (1,4,9) {1}
                            GROUP BY d.UserId) AS e ON b.UserId = e.UserId LEFT OUTER JOIN
                          (SELECT     d.UserId, SUM(o.OrderTotal) AS OrderTotal
                            FROM    dbo.aspnet_Distributors AS d LEFT OUTER JOIN 
                                    dbo.Hishop_Orders AS o ON d.UserId = o.ReferralUserId  
                            WHERE  o.OrderStatus not in (1,4,9) {1}
                            GROUP BY d.UserId) AS f ON b.UserId = f.UserId LEFT OUTER JOIN 
                            (SELECT     d.UserId, SUM(cm.CommTotal) AS CommTotal
                            FROM    dbo.aspnet_Distributors AS d LEFT OUTER JOIN 
                                    dbo.Hishop_Orders AS o ON d.UserId = o.ReferralUserId  INNER JOIN 
                                    dbo.Hishop_Commissions as cm ON  o.OrderId = cm.OrderId 
                            WHERE  o.OrderStatus not in (1,4,9) {2} 
                            GROUP BY d.UserId) AS g ON b.UserId = g.UserId 
                            where {3} order by a.fgsid", where1, where2, where3, where4);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }


        /// <summary>
        /// 获取分公司已认证门店的门店名称、销售总量、佣金总额、用户数
        /// </summary>
        /// <param name="query">实体</param>
        /// <returns></returns>
        public DbQueryResult GetFilialeStoreList(FilialeStoreListQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountALLHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AccountALLHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountALLHere));
            }
            if (!string.IsNullOrEmpty(query.fgsId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsid ='{0}'", DataHelper.CleanSearchString(query.fgsId));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vm_Hishop_FilialeStoreList", "fgsid", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        /// <summary>
        ///获取分公司的认证门店数、销售总量、佣金总额、粉丝数，用于导出Excel
        /// </summary>
        /// <param name="query">实体</param>
        /// <returns></returns>
        public DataTable GetExportFilialeStatistics(FilialeStoreListQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.AccountALLHere))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" AccountALLHere LIKE '%{0}%'", DataHelper.CleanSearchString(query.AccountALLHere));
            }
            if (!string.IsNullOrEmpty(query.fgsId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" fgsId='{0}'", DataHelper.CleanSearchString(query.fgsId));
            }
            if (!string.IsNullOrEmpty(query.UserId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" UserId = {0}", DataHelper.CleanSearchString(query.UserId));
            }
            if (!string.IsNullOrEmpty(query.UserIds))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                if (query.UserIds != "null")
                    builder.AppendFormat(" UserId in ({0})", query.UserIds);
                else
                    builder.Append(" UserId = -1");
            }
            if (!string.IsNullOrEmpty(query.IsCheck.ToString()))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" IsCheck={0}", query.IsCheck);
            }

            string strSql = @"select * from dbo.vm_Hishop_FilialeStoreList";
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                strSql += " where " + builder.ToString();
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 门店会员信息统计
        /// </summary>
        /// <param name="where1">条件1</param>
        /// <param name="where2">条件2</param>
        /// <param name="where3">条件3</param>
        /// <param name="where4">条件4</param>
        /// <returns></returns>
        public DataTable GetStoreStatis(string where1, string where2, string where3, string where4, string where5)
        {
            StringBuilder builder = new StringBuilder();
//            builder.AppendFormat(@"select f.fgsName,s.accountALLHere,s.storeName,a.MemerNum,b.lrNum,c.fgNum,d.nxNum from dbo.aspnet_Distributors as dis left join 
//                    dbo.CW_StoreInfo as s on dis.StoreId = s.id left join 
//                    dbo.CW_Filiale as f on s.fgsid = f.id 
//                    left join 
//                    (select DistributorUserId,COUNT(UserId) as MemerNum from dbo.aspnet_Members where {0} group by DistributorUserId) as a on dis.UserId = a.DistributorUserId 
//                    left join 
//                    (select si.id,COUNT(om.userid) as lrNum from dbo.CW_StoreInfo as si left join dbo.CW_O2OMembers as om on si.accountALLHere = om.storeCode where {1} group by si.id) as b on b.id = dis.StoreId 
//                    left join 
//                    (select DistributorUserId,COUNT(UserId) as fgNum from 
//                    (select dbo.aspnet_Members.* from dbo.aspnet_Members inner join CW_Members on dbo.aspnet_Members.UserId = CW_Members.RelationUserId where {2}) as a group by DistributorUserId) as c on dis.UserId = c.DistributorUserId 
//                    left join 
//                    (select si.id,COUNT(userid) as nxNum from dbo.CW_O2OMembers as om 
//            left join dbo.CW_StoreInfo as si on si.accountALLHere = om.storeCode 
//            where (mobile in (select CellPhone from dbo.CW_Members where RelationUserId >0) or (select count(*) from dbo.CW_O2OMembersAttribute where userid = om.userid) >= 10) and {3} group by si.id) as d on d.id = dis.StoreId where {4} order by dis.OrdersTotal desc", where1, where2, where3, where4, where5);

            builder.AppendFormat(@"select f.fgsName,s.accountALLHere,s.storeName,a.MemerNum,b.lrNum,c.fgNum,d.nxNum from dbo.aspnet_Distributors as dis left join 
                    dbo.CW_StoreInfo as s on dis.StoreId = s.id left join 
                    dbo.CW_Filiale as f on s.fgsid = f.id 
                    left join 
                    (select DistributorUserId,COUNT(UserId) as MemerNum from dbo.aspnet_Members where {0} group by DistributorUserId) as a on dis.UserId = a.DistributorUserId 
                    left join 
                    (select si.id,COUNT(om.userid) as lrNum from dbo.CW_StoreInfo as si left join dbo.CW_O2OMembers as om on si.accountALLHere = om.storeCode where {1} group by si.id) as b on b.id = dis.StoreId 
                    left join 
                    (select DistributorUserId,COUNT(UserId) as fgNum from 
                    (select dbo.aspnet_Members.* from dbo.aspnet_Members inner join CW_Members on dbo.aspnet_Members.UserId = CW_Members.RelationUserId where {2}) as a group by DistributorUserId) as c on dis.UserId = c.DistributorUserId 
                    left join 
                    (select id,COUNT(rowNum) as nxNum from 
                    (
                    SELECT  row_number() OVER (ORDER BY o2oM.userid) AS rowNum,CASE WHEN si.id IS NULL then fgM.StoreId ELSE si.id end as id, 
                            CASE WHEN o2oM.buydate IS NULL then fgM.CreateDate ELSE o2oM.buydate end as buydate, si.storeName AS storeNameNew 
		                    FROM  dbo.CW_O2OMembers AS o2oM LEFT JOIN dbo.CW_StoreInfo AS si ON o2oM.storeCode = si.accountALLHere FULL JOIN
		                    (
		                    SELECT m.UserId AS Memberuserid,d.StoreId,m.CreateDate, CASE WHEN m.CellPhone IS NULL THEN ua.CellPhone ELSE m.CellPhone END AS Membermobile  
		                    FROM   dbo.aspnet_Members AS m LEFT JOIN 
		                    dbo.aspnet_Distributors AS d ON m.UserId = d .UserId LEFT JOIN 
		                    (SELECT * FROM dbo.Hishop_UserShippingAddresses WHERE IsDefault = 1) AS ua ON m.UserId = ua.UserId 
		                    WHERE m.UserId IN (SELECT RelationUserId FROM dbo.CW_Members WHERE RelationUserId > 0)
		                    ) AS fgM ON o2oM.mobile = fgM.Membermobile 
                    ) as e where {3}  group by e.id  ) as d on d.id = dis.StoreId where {4} order by dis.OrdersTotal desc", where1, where2, where3, where4, where5);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 门店会员信息统计
        /// </summary>
        /// <param name="where1">条件1</param>
        /// <param name="where2">条件2</param>
        /// <param name="where3">条件3</param>
        /// <param name="where4">条件4</param>
        /// <returns></returns>
        public DataTable GetStoreStatis(string where1, string where2, string where3, string where4, string where5, IList<string> fields)
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
            str = str.Substring(0, str.Length - 1);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"select {5} from dbo.aspnet_Distributors as dis left join 
                    dbo.CW_StoreInfo as s on dis.StoreId = s.id left join 
                    dbo.CW_Filiale as f on s.fgsid = f.id 
                    left join 
                    (select DistributorUserId,COUNT(UserId) as MemerNum from dbo.aspnet_Members where {0} group by DistributorUserId) as a on dis.UserId = a.DistributorUserId 
                    left join 
                    (select si.id,COUNT(om.userid) as lrNum from dbo.CW_StoreInfo as si left join dbo.CW_O2OMembers as om on si.accountALLHere = om.storeCode where {1} group by si.id) as b on b.id = dis.StoreId 
                    left join 
                    (select DistributorUserId,COUNT(UserId) as fgNum from 
                    (select dbo.aspnet_Members.* from dbo.aspnet_Members inner join CW_Members on dbo.aspnet_Members.UserId = CW_Members.RelationUserId where {2}) as a group by DistributorUserId) as c on dis.UserId = c.DistributorUserId 
                    left join 
                    (select id,COUNT(rowNum) as nxNum from 
                    (
                    SELECT  row_number() OVER (ORDER BY o2oM.userid) AS rowNum,CASE WHEN si.id IS NULL then fgM.StoreId ELSE si.id end as id, 
                            CASE WHEN o2oM.buydate IS NULL then fgM.CreateDate ELSE o2oM.buydate end as buydate, si.storeName AS storeNameNew 
		                    FROM  dbo.CW_O2OMembers AS o2oM LEFT JOIN dbo.CW_StoreInfo AS si ON o2oM.storeCode = si.accountALLHere FULL JOIN
		                    (
		                    SELECT m.UserId AS Memberuserid, d.StoreId, m.CreateDate, CASE WHEN m.CellPhone IS NULL THEN ua.CellPhone ELSE m.CellPhone END AS Membermobile  
		                    FROM   dbo.aspnet_Members AS m LEFT JOIN 
		                    dbo.aspnet_Distributors AS d ON m.UserId = d .UserId LEFT JOIN 
		                    (SELECT * FROM dbo.Hishop_UserShippingAddresses WHERE IsDefault = 1) AS ua ON m.UserId = ua.UserId 
		                    WHERE m.UserId IN (SELECT RelationUserId FROM dbo.CW_Members WHERE RelationUserId > 0)
		                    ) AS fgM ON o2oM.mobile = fgM.Membermobile 
                    ) as e where {3} group by e.id) as d on d.id = dis.StoreId where {4} order by dis.OrdersTotal desc", where1, where2, where3, where4, where5, str);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        /// <summary>
        /// 将门店定位的地址存储到，2017-06-29修改
        /// </summary>
        /// <param name="distributors"></param>
        /// <returns></returns>
        public bool SetDistributorsLocation(DistributorsInfo distributors)
        {
            //修改2处， 1、修改店员信息为认证状态， 2、修改会员表，将认证的会员与认证店员所属的门店关系起来
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"UPDATE dbo.aspnet_Distributors SET Location_module = @Location_module,Location_lat=@Location_lat,Location_lng=@Location_lng,Location_poiaddress=@Location_poiaddress,Location_poiname=@Location_poiname,Location_cityname=@Location_cityname WHERE UserId = @UserId;");
            this.database.AddInParameter(sqlStringCommand, "Location_module", DbType.String, distributors.Location_module);
            this.database.AddInParameter(sqlStringCommand, "Location_lat", DbType.Double, distributors.Location_lat);
            this.database.AddInParameter(sqlStringCommand, "Location_lng", DbType.Double, distributors.Location_lng);
            this.database.AddInParameter(sqlStringCommand, "Location_poiaddress", DbType.String, distributors.Location_poiaddress);
            this.database.AddInParameter(sqlStringCommand, "Location_poiname", DbType.String, distributors.Location_poiname);
            this.database.AddInParameter(sqlStringCommand, "Location_cityname", DbType.String, distributors.Location_cityname);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributors.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查询门店信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable SelectVWDistributorsByWhere(string where)
        {
            string strSql = "select * from dbo.vw_Hishop_DistributorsMembers ";
            if(!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 根据位置信息查询附近门店信息
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>DataTable数据表</returns>
        public DbQueryResult SelectNearByPosition(DistributorListQuery query)
        {
            string sqlWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(query.keyword))
            {
                sqlWhere += string.Format(" AND (StoreName LIKE '%{0}%' or fgsName LIKE '%{0}%' or storeRelationPerson LIKE '%{0}%' or Location_poiaddress LIKE '%{0}%' or Location_poiname LIKE '%{0}%' or Location_cityname LIKE '%{0}%') ", DataHelper.CleanSearchString(query.keyword));
            }
            if (query.NearByValue > 0)
            {
                sqlWhere += string.Format(" AND distance < {0}", query.NearByValue);
            }
            string strTable = string.Format("dbo.F_DistributorsMembers({0},{1})", query.lng, query.lat);
            //return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strTable, "UserId", sqlWhere, "*");
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, strTable, "UserId", sqlWhere, "*");
        }
        /// <summary>
        ///根据门店ID获取门店的六项维度实际得分 2017-7-21 yk
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable getStoreSixScoreData()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ds.UserId,orderSumMoney,orderCountNum,StarCommenNum,memberCount,JLmemberCount,NXCount from aspnet_Distributors ds left join (select SUM(OrderTotal) as orderSumMoney,ReferralUserId from Hishop_Orders where OrderStatus not in (1,4,9) group by ReferralUserId) as orderSumMoney  on ds.UserId= orderSumMoney.ReferralUserId left join(select COUNT(OrderId) as orderCountNum,ReferralUserId  from Hishop_Orders where OrderStatus not in (1,4,9) group by ReferralUserId)as orderCountNum on ds.UserId=orderCountNum.ReferralUserId left join  (select AVG(Total) as StarCommenNum,DisUserId from aspnet_DistributorMark group by DisUserId)as DisStarNum on ds.UserId=DisStarNum.DisUserId left join(select  COUNT(UserId) as memberCount,DistributorUserId from aspnet_Members group by DistributorUserId)as memberCount on ds.UserId=memberCount.DistributorUserId left join (select SUM(Total) as JLmemberCount,StoreId from CW_Members group by StoreId)as JinLiMemberCount on ds.StoreId=JinLiMemberCount.StoreId left join (select count(userid) as NXCount,StoreId from CW_NianXingMembers  group by StoreId)as NXmemberCount on ds.storeId=NXmemberCount.StoreId ");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }
        /// <summary>
        /// 查询门店信息 2017-7-21 yk
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>DataTable数据表</returns>
        public DataTable GetDisTrobutorData()
        {
            string strSql = "select * from aspnet_Distributors";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

    }
}

