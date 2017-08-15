namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class MessageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool Create(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate, ClientUserId, AgentName) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate, @ClientUserId, @AgentName)");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, manager.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, manager.ClientUserId);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateAgent(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate, ClientUserId, AgentName, [State]) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate, @ClientUserId, @AgentName, @State)");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, manager.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, manager.ClientUserId);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, manager.State);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteManagerByRoleIdandClientUserId(int roleId,int clientUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE from aspnet_Managers where RoleId=6 and ClientUserId = @clientUserId");
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, clientUserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ManagerInfo GetManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        public ManagerInfo GetManager(string userName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserName = @UserName");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }


        public DbQueryResult GetManagers(ManagerQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            if (query.RoleId != 0)
            {
                builder.AppendFormat(" AND RoleId = {0}", query.RoleId);
            }
            if (GetManager(Globals.GetCurrentManagerUserId()).UserName.ToLower() != "yihui") {
                builder.AppendFormat(" AND UserName != 'yihui'");
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Managers", "UserId", builder.ToString(), "*");
        }


        public DataTable GetManagersbyClientUserId(ManagerQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select UserId from aspnet_Managers where RoleId=@RoleId and ClientUserId=@ClientUserId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, query.RoleId);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, query.ClientUserId);


            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取所有后台manager
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllManagers()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select UserId,UserName,RoleId,AgentName,ClientUserId from aspnet_Managers");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool Update(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Managers SET RoleId = @RoleId, UserName = @UserName, Password = @Password, Email = @Email, AgentName = @AgentName, [State] = @State WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, manager.UserId);
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, manager.State);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 获取当前管理员的所属区域
        /// </summary>
        /// <param name="userId">管理员id</param>
        /// <returns></returns>
        public DataTable GetManagerArea(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from erp_managersregion where userid = "+userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
            //StringBuilder builder = new StringBuilder();
            //builder.Append("select * from erp_managersregion where userid = " + userId);
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            //return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 判断前台微信用户是否为代理商
        /// </summary>
        /// <param name="userId">前端用户ID</param>
        /// <returns></returns>
        public bool ExistClientUserId(int roleid, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Managers WHERE RoleId = @RoleId and ClientUserId = @ClientUserId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleid);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, userId);
            return (((int)this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        /// <summary>
        /// 根据前端用户Id得到管理端用户对象（是否是代理商）
        /// </summary>
        /// <param name="clientuserId">前端用户Id</param>
        /// <returns>管理端用户对象</returns>
        public ManagerInfo GetManagerByClientUserId(int clientuserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE ClientUserId = @ClientUserId");
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.String, clientuserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        /// <summary>
        /// 后台账号打卡
        /// </summary>
        public bool DutyOn(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into YiHui_DutyShift (ID,ManagerId,DutyDate,LoginTime,DutyState) values (@ID,@ManagerId,@DutyDate,@LoginTime,1)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, Guid.NewGuid());
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            this.database.AddInParameter(sqlStringCommand, "DutyDate", DbType.DateTime, DateTime.Today);
            this.database.AddInParameter(sqlStringCommand, "LoginTime", DbType.DateTime, DateTime.Now);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 后台账号关班
        /// </summary>
        public bool DutyOff(int managerId)
        {
            string dutyOffSql = @"declare @LoginTime datetime
                        set @LoginTime  = (select LoginTime from Yihui_DutyShift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23))
                        update YiHui_DutyShift set LoginOutTime=getdate(),OrdersCount=(select COUNT(*) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus=5),
                        OrdersTotal = (select ISNULL(sum(OrderTotal),0) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus=5),DutyState=2
                        where managerId = @ManagerId and DutyDate=Convert(varchar,getdate(),23)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(dutyOffSql);
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 关班重复判断
        /// </summary>
        public bool isDutyOffExist(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*)c from yihui_dutyshift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23) and DutyState=2");
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt16(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) == 1;
            }
        }

        /// <summary>
        /// 打卡重复判断
        /// </summary>
        public bool isDutyExist(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*)c from yihui_dutyshift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23)");
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt16(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) == 1;
            }
        }
        /// <summary>
        /// 获取当天打卡信息
        /// </summary>
        public DataTable GetDutyInfo(int managerId)
        {
            string sql = @"declare @LoginTime datetime
                        set @LoginTime  = (select LoginTime from Yihui_DutyShift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23))
                        select id,ManagerId,DutyDate,LoginTime,
                        (select COUNT(*) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus = 5) as orderCount,
                        (select sum(OrderTotal) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus = 5) as orderTotal
                        from yihui_DutyShift where managerId = @ManagerId and DutyDate=Convert(varchar,getdate(),23) ";
            DataTable dtDutyFull = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取后台账号的交接班列表
        /// </summary>
        public DataTable GetDutyInfoList(DateTime? dateStart, DateTime? dateEnd,int managerId=0)
        {
            string sql = "Select ID,am.UserName,DutyDate,LoginTime,LoginOutTime,OrdersCount,OrdersTotal From YiHui_DutyShift YD left join aspnet_Managers AM on YD.managerId = am.UserId Where YD.DutyState = 2 ";
            string sqlWhere = " and DutyDate = Convert(varchar,getdate(),23) ";
            if (dateStart != null)
            {
                sqlWhere = string.Format(" and DutyDate >= '{0}' ", dateStart);
            }
            if (dateEnd != null)
            {
                sqlWhere = string.Format(" and DutyDate <= '{0}' ", dateEnd);
            }
            if (dateStart != null && dateEnd != null)
            {
                sqlWhere = string.Format(" and DutyDate between '{0}' and '{1}' ", dateStart, dateEnd);
            }
            if (managerId != 0)
            {
                sqlWhere += string.Format(" and managerId = {0}",managerId);
            }
            sql += sqlWhere;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }

        }

        /// <summary>
        /// 爽爽挝啡pc点餐系统生成的订单根据sender获取后台点餐账号对应的店铺名
        /// </summary>
        public string getPcOrderStorenameBySender(int sender)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select storename from aspnet_Distributors where UserId = (select ClientUserId from aspnet_Managers where UserId="+sender+")");
            object returnStr = this.database.ExecuteScalar(sqlStringCommand);
            return returnStr == null ? "店内" : returnStr.ToString();
        }

    }
}

