namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class PointDetailDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddPointDetail(PointDetailInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) point.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int GetHistoryPoint(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased-Reduced) FROM Hishop_PointDetails WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        /// <summary>
        /// 根据用户id 订单id删除积分详情
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="OrderId">订单id</param>
        /// <returns></returns>
        public bool Delete(int userId, string OrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PointDetails WHERE UserId = @UserId and OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 得到所有积分记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetPoints()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_PointDetails ORDER BY UserId,TradeDate");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 得到所有存在积分记录的用户Id
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointUserIds()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT distinct UserId FROM Hishop_PointDetails ORDER BY UserId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 得到所有存在积分记录的用户Id
        /// </summary>
        /// <returns></returns>
        public DataTable GetUsers()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM dbo.aspnet_Members WHERE UserId in (SELECT distinct UserId FROM Hishop_PointDetails)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 得到退款的订单集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderReturn()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select * from dbo.Hishop_OrderReturns where HandleStatus = 2");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }


    }
}

