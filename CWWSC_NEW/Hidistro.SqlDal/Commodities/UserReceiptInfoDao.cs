using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
    public class UserReceiptInfoDao
    {
        private Database database = DatabaseFactory.CreateDatabase();


        /// <summary>
        /// 添加发票信息
        /// </summary>
        /// <param name="UserReceiptInfo">发票实体类信息</param>
        /// <returns></returns>
        public int AddUserReceiptInfo(UserReceiptInfo receiptInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_UserReceiptInfo(UserId,Type,Type1,CellPhone,Email,CompanyName,TaxesCode,Address,Phone,Bank,BankNumber,RegistrationImg,EmpowerEntrustImg,TaxpayerProveImg,IsDefault) VALUES (@UserId,@Type,@Type1,@CellPhone,@Email,@CompanyName,@TaxesCode,@Address,@Phone,@Bank,@BankNumber,@RegistrationImg,@EmpowerEntrustImg,@TaxpayerProveImg,@IsDefault); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, receiptInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, receiptInfo.Type);
            if (receiptInfo.Type == 0)
                this.database.AddInParameter(sqlStringCommand, "Type1", DbType.Int32, receiptInfo.Type1);
            else
                this.database.AddInParameter(sqlStringCommand, "Type1", DbType.Int32, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, receiptInfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, receiptInfo.Email);
            this.database.AddInParameter(sqlStringCommand, "CompanyName", DbType.String, receiptInfo.CompanyName);
            this.database.AddInParameter(sqlStringCommand, "TaxesCode", DbType.String, receiptInfo.TaxesCode);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, receiptInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, receiptInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Bank", DbType.String, receiptInfo.Bank);
            this.database.AddInParameter(sqlStringCommand, "BankNumber", DbType.String, receiptInfo.BankNumber);
            this.database.AddInParameter(sqlStringCommand, "RegistrationImg", DbType.String, receiptInfo.RegistrationImg);
            this.database.AddInParameter(sqlStringCommand, "EmpowerEntrustImg", DbType.String, receiptInfo.EmpowerEntrustImg);
            this.database.AddInParameter(sqlStringCommand, "TaxpayerProveImg", DbType.String, receiptInfo.TaxpayerProveImg);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Int32, receiptInfo.IsDefault);

            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        /// <summary>
        /// 根据主键ID获取发票信息
        /// </summary>
        /// <param name="ReceiptId">发票信息主键ID</param>
        /// <returns></returns>
        public UserReceiptInfo GetUserReceiptInfo(int ReceiptId)
        {
            UserReceiptInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_UserReceiptInfo WHERE ReceiptId = @ReceiptId;");
            this.database.AddInParameter(sqlStringCommand, "ReceiptId", DbType.Int32, ReceiptId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<UserReceiptInfo>(reader);
                reader.NextResult();
            }
            return info;
        }

        /// <summary>
        /// 根据UserID获取发票信息
        /// </summary>
        /// <param name="UserId">用户UserId</param>
        /// <returns></returns>
        public IList<UserReceiptInfo> GetListUserReceiptInfo(int UserId, string strtype = "")
        {
            string strwhere = "UserId = @UserId";
            if (!string.IsNullOrEmpty(strtype))
                strwhere += string.Format(" AND Type = '{0}'", strtype);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM CW_UserReceiptInfo WHERE {0} order by ReceiptId desc", strwhere));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<UserReceiptInfo>(reader);
            }
        }

        /// <summary>
        /// 根据主键ID，更新发票信息
        /// </summary>
        /// <param name="StoreInfo"></param>
        /// <returns>返回bool状态</returns>
        public bool UpdateUserReceiptInfo(UserReceiptInfo receiptInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_UserReceiptInfo SET UserId = @UserId, Type = @Type, Type1 = @Type1, CellPhone = @CellPhone ,Email=@Email,CompanyName=@CompanyName,TaxesCode=@TaxesCode,Address=@Address,Phone=@Phone,Bank=@Bank,BankNumber=@BankNumber,RegistrationImg=@RegistrationImg,EmpowerEntrustImg=@EmpowerEntrustImg,TaxpayerProveImg=@TaxpayerProveImg,IsDefault=@IsDefault WHERE ReceiptId = @ReceiptId");
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, receiptInfo.Type);
            if (receiptInfo.Type == 0)
                this.database.AddInParameter(sqlStringCommand, "Type1", DbType.Int32, receiptInfo.Type1);
            else
                this.database.AddInParameter(sqlStringCommand, "Type1", DbType.Int32, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, receiptInfo.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, receiptInfo.Email);
            this.database.AddInParameter(sqlStringCommand, "CompanyName", DbType.String, receiptInfo.CompanyName);
            this.database.AddInParameter(sqlStringCommand, "TaxesCode", DbType.String, receiptInfo.TaxesCode);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, receiptInfo.Address);
            this.database.AddInParameter(sqlStringCommand, "Phone", DbType.String, receiptInfo.Phone);
            this.database.AddInParameter(sqlStringCommand, "Bank", DbType.String, receiptInfo.Bank);
            this.database.AddInParameter(sqlStringCommand, "BankNumber", DbType.String, receiptInfo.BankNumber);
            this.database.AddInParameter(sqlStringCommand, "RegistrationImg", DbType.String, receiptInfo.RegistrationImg);
            this.database.AddInParameter(sqlStringCommand, "EmpowerEntrustImg", DbType.String, receiptInfo.EmpowerEntrustImg);
            this.database.AddInParameter(sqlStringCommand, "TaxpayerProveImg", DbType.String, receiptInfo.TaxpayerProveImg);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Int32, receiptInfo.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, receiptInfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReceiptId", DbType.Int32, receiptInfo.ReceiptId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 根据条件查找发票信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>返回DataSet数据集</returns>
        public DataTable SelectUserReceiptInfoByWhere(string where)
        {
            string strSql = string.Format("select * FROM CW_UserReceiptInfo ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " where " + where;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public bool SetDefaultReceiptInfo(int receiptId, int UserId)
        {
            StringBuilder builder = new StringBuilder("UPDATE  CW_UserReceiptInfo SET IsDefault = 0 where UserId=@UserId;");
            builder.Append("UPDATE  CW_UserReceiptInfo SET IsDefault = 1 where ReceiptId=@ReceiptId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            this.database.AddInParameter(sqlStringCommand, "ReceiptId", DbType.Int32, receiptId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteReceipt(int receiptId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_UserReceiptInfo WHERE ReceiptId = @ReceiptId");
            this.database.AddInParameter(sqlStringCommand, "ReceiptId", DbType.Int32, receiptId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

    }
}
