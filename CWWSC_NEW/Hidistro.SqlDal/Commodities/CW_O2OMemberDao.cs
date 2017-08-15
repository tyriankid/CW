using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
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
     public  class CW_O2OMemberDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        //(新增)
        public int InsertO2OCwMembers(CW_O2OMenbersInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO CW_O2OMembers(name, mobile,profession,OpenId,gradeName,storeCode,birthday,sex,model,buydate,typeid,price,regionid,Address,remark,RelationUserId,typename,Province,City,County,Village,OldRegion,CreateDate,jiatingchengyuan,zhufangxinxi,fangyujiadian,jiadianshiyong,gerenqingxiang,jinqixuqiu,IsUserWaterDarifier,BuyWaterDarifierDate) VALUES (@name, @mobile,@profession,@OpenId,@gradeName,@storeCode,@birthday,@sex,@model,@buydate,@typeid,@price,@regionid,@Address,@remark,@RelationUserId,@typename,@Province,@City,@County,@Village,@OldRegion,@CreateDate,@jiatingchengyuan,@zhufangxinxi,@fangyujiadian,@jiadianshiyong,@gerenqingxiang,@jinqixuqiu,@IsUserWaterDarifier,@BuyWaterDarifierDate)");
            this.database.AddInParameter(sqlStringCommand, "name", DbType.String, member.name);
            this.database.AddInParameter(sqlStringCommand, "mobile", DbType.String, member.mobile);
            this.database.AddInParameter(sqlStringCommand, "profession", DbType.String, member.profession);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "gradeName", DbType.String, member.gradeName);
            this.database.AddInParameter(sqlStringCommand, "storeCode", DbType.String, member.storeCode);
            if(!string.IsNullOrEmpty(member.birthday))
                this.database.AddInParameter(sqlStringCommand, "birthday", DbType.String, member.birthday);
            else
                this.database.AddInParameter(sqlStringCommand, "birthday", DbType.String, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "sex", DbType.String, member.sex);
            this.database.AddInParameter(sqlStringCommand, "model", DbType.String, member.model);
            if (!string.IsNullOrEmpty(member.buydate))
                this.database.AddInParameter(sqlStringCommand, "buydate", DbType.String, member.buydate);
            else
                this.database.AddInParameter(sqlStringCommand, "buydate", DbType.String, DBNull.Value);
            this.database.AddInParameter(sqlStringCommand, "typeid", DbType.String, member.typeid);
            this.database.AddInParameter(sqlStringCommand, "price", DbType.String, member.price);
            this.database.AddInParameter(sqlStringCommand, "regionid", DbType.Int32, member.regionid);
            this.database.AddInParameter(sqlStringCommand, "remark", DbType.String, member.remark);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "RelationUserId", DbType.Int32, member.RelationUserId);
            this.database.AddInParameter(sqlStringCommand, "typename", DbType.String, member.typename);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.String, member.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.String, member.City);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.String, member.County);
            this.database.AddInParameter(sqlStringCommand, "Village", DbType.String, member.Village);
            this.database.AddInParameter(sqlStringCommand, "OldRegion", DbType.String, member.OldRegion);
            //新增字段
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, member.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "jiatingchengyuan", DbType.String, member.jiatingchengyuan);
            this.database.AddInParameter(sqlStringCommand, "zhufangxinxi", DbType.String, member.zhufangxinxi);
            this.database.AddInParameter(sqlStringCommand, "fangyujiadian", DbType.String, member.fangyujiadian);
            this.database.AddInParameter(sqlStringCommand, "jiadianshiyong", DbType.String, member.jiadianshiyong);
            this.database.AddInParameter(sqlStringCommand, "gerenqingxiang", DbType.String, member.gerenqingxiang);
            this.database.AddInParameter(sqlStringCommand, "jinqixuqiu", DbType.String, member.jinqixuqiu);
            //净水器
            this.database.AddInParameter(sqlStringCommand, "IsUserWaterDarifier", DbType.Int32, member.IsUserWaterDarifier.Value);
            this.database.AddInParameter(sqlStringCommand, "BuyWaterDarifierDate", DbType.String, member.BuyWaterDarifierDate);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        
        //(删除)
        public bool DeleteO2OMembers(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM CW_O2OMembers WHERE userid = @userid");
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(修改)
        public bool UpdateO2OMembersr(CW_O2OMenbersInfo member)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_O2OMembers SET name=@name,mobile=@mobile,profession=@profession,OpenId=@OpenId,gradeName=@gradeName,birthday=@birthday,sex=@sex,model=@model,buydate=@buydate,typeid=@typeid,price=@price,regionid=@regionid,Address=@Address,remark=@remark ,RelationUserId=@RelationUserId,typename=@typename,Province=@Province,City=@City,County=@County,Village=@Village,OldRegion=@OldRegion,jiatingchengyuan=@jiatingchengyuan,zhufangxinxi=@zhufangxinxi,fangyujiadian=@fangyujiadian,jiadianshiyong=@jiadianshiyong,gerenqingxiang=@gerenqingxiang,jinqixuqiu=@jinqixuqiu,IsUserWaterDarifier=@IsUserWaterDarifier,BuyWaterDarifierDate=@BuyWaterDarifierDate WHERE userid = @userid");
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE CW_O2OMembers SET name=@name,mobile=@mobile,profession=@profession,OpenId=@OpenId,gradeName=@gradeName,storeid=@storeid,birthday=@birthday,sex=@sex,model=@model,buydate=@buydate,typeid=@typeid,price=@price,regionid=@regionid,Address=@Address,remark=@remark ,RelationUserId=@RelationUserId,typename=@typename,Province=@Province,City=@City,County=@County,Village=@Village WHERE userid = @userid");
            this.database.AddInParameter(sqlStringCommand, "name", DbType.String, member.name);
            this.database.AddInParameter(sqlStringCommand, "mobile", DbType.String, member.mobile);
            this.database.AddInParameter(sqlStringCommand, "profession", DbType.String, member.profession);
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, member.OpenId);
            this.database.AddInParameter(sqlStringCommand, "gradeName", DbType.String, member.gradeName);
            //this.database.AddInParameter(sqlStringCommand, "storeCode", DbType.String, member.storeCode);
            this.database.AddInParameter(sqlStringCommand, "birthday", DbType.String, member.birthday);
            this.database.AddInParameter(sqlStringCommand, "sex", DbType.String, member.sex);
            this.database.AddInParameter(sqlStringCommand, "model", DbType.String, member.model);
            this.database.AddInParameter(sqlStringCommand, "buydate", DbType.String, member.buydate);
            this.database.AddInParameter(sqlStringCommand, "typeid", DbType.String, member.typeid);
            this.database.AddInParameter(sqlStringCommand, "price", DbType.String, member.price);
            this.database.AddInParameter(sqlStringCommand, "regionid", DbType.Int32, member.regionid);
            this.database.AddInParameter(sqlStringCommand, "remark", DbType.String, member.remark);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, member.Address);
            this.database.AddInParameter(sqlStringCommand, "RelationUserId", DbType.Int32, member.RelationUserId);
            this.database.AddInParameter(sqlStringCommand, "typename", DbType.String, member.typename);
            this.database.AddInParameter(sqlStringCommand, "Province", DbType.String, member.Province);
            this.database.AddInParameter(sqlStringCommand, "City", DbType.String, member.City);
            this.database.AddInParameter(sqlStringCommand, "County", DbType.String, member.County);
            this.database.AddInParameter(sqlStringCommand, "Village", DbType.String, member.Village);
            this.database.AddInParameter(sqlStringCommand, "OldRegion", DbType.String, member.OldRegion);
            //新增字段
            this.database.AddInParameter(sqlStringCommand, "jiatingchengyuan", DbType.String, member.jiatingchengyuan);
            this.database.AddInParameter(sqlStringCommand, "zhufangxinxi", DbType.String, member.zhufangxinxi);
            this.database.AddInParameter(sqlStringCommand, "fangyujiadian", DbType.String, member.fangyujiadian);
            this.database.AddInParameter(sqlStringCommand, "jiadianshiyong", DbType.String, member.jiadianshiyong);
            this.database.AddInParameter(sqlStringCommand, "gerenqingxiang", DbType.String, member.gerenqingxiang);
            this.database.AddInParameter(sqlStringCommand, "jinqixuqiu", DbType.String, member.jinqixuqiu);
            //净水器
            this.database.AddInParameter(sqlStringCommand, "IsUserWaterDarifier", DbType.Int32, member.IsUserWaterDarifier.Value);
            this.database.AddInParameter(sqlStringCommand, "BuyWaterDarifierDate", DbType.String, member.BuyWaterDarifierDate);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, member.userid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        //(查询)
        public DbQueryResult GetListO2OMember(CW_O2OMemberQuery query)
        {
            string sqlWhere = "1=1";
            if (!string.IsNullOrEmpty(query.name))
            {
                sqlWhere += string.Format(" and (name like '%{0}%' or mobile like '%{0}%')", DataHelper.CleanSearchString(query.name));
            }
            if (!string.IsNullOrEmpty(query.storeCode))
            {
                sqlWhere += string.Format(" and storeCode = '{0}'", DataHelper.CleanSearchString(query.storeCode));
            }
            if (!string.IsNullOrEmpty(query.startTime))
            {
                sqlWhere += string.Format(" and buydate >= '{0}'", query.startTime);
            }
            if (!string.IsNullOrEmpty(query.endTime))
            {
                sqlWhere += string.Format(" and buydate < '{0}'", Convert.ToDateTime(query.endTime).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(query.storeCodes))
            {
                sqlWhere += string.Format(" and storeCode in ({0}) ", query.storeCodes);
            }
            //if (query.isNxMember)
            //{
            //    sqlWhere += string.Format(" and (mobile in (select CellPhone from dbo.CW_Members where RelationUserId > 0) or (select count(*) from dbo.CW_O2OMembersAttribute where userid = CW_O2OMembers.userid) >= 10)");
            //}
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_O2OMembers", "userid", sqlWhere, "*, (SELECT storeName FROM CW_StoreInfo where accountALLHere =CW_O2OMembers.storeCode) AS StoreName");
        }

        public DbQueryResult GetListNxMember(CW_O2OMemberQuery query)
        {
            string sqlWhere = "1=1";
            if (!string.IsNullOrEmpty(query.name))
            {
                sqlWhere += string.Format(" and (name LIKE '%{0}%' or MemberName LIKE '%{0}%')", DataHelper.CleanSearchString(query.name));
            }
            if (!string.IsNullOrEmpty(query.startTime))
            {
                sqlWhere += string.Format(" and buydate >= '{0}'", query.startTime);
            }
            if (!string.IsNullOrEmpty(query.endTime))
            {
                sqlWhere += string.Format(" and buydate < '{0}'", Convert.ToDateTime(query.endTime).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(query.cell))
            {
                sqlWhere += string.Format(" and (mobile LIKE '%{0}%' or Membermobile LIKE '%{0}%')", DataHelper.CleanSearchString(query.cell));
            }
            if (!string.IsNullOrEmpty(query.storeCodes))
            {
                sqlWhere += string.Format(" and (storeCode in ({0}) or MemberStoreCode in ({0}))", query.storeCodes);
            }
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "dbo.vw_Hishop_nxMember2", "rowNum", sqlWhere, "*");
        }


        public CW_O2OMenbersInfo GetO2OMember(string userName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_O2OMembers WHERE name = @name");
            this.database.AddInParameter(sqlStringCommand, "name", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CW_O2OMenbersInfo>(reader);
            }
        }

        public CW_O2OMenbersInfo GetO2OMemberId(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM CW_O2OMembers WHERE userid = @userid");
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CW_O2OMenbersInfo>(reader);
            }
        }

        public DataTable GetAllMemberExprot(CW_O2OMemberQuery query, IList<string> fields)
        {
            DataTable table = null;
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

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"select {0} from dbo.vw_Hishop_nxMember2 where 1=1 ", str);

            if (!string.IsNullOrEmpty(query.name))
            {
                builder.AppendFormat(" and (name LIKE '%{0}%' or MemberName LIKE '%{0}%')", DataHelper.CleanSearchString(query.name));
            }
            if (!string.IsNullOrEmpty(query.startTime))
            {
                builder.AppendFormat(" and buydate >= '{0}'", query.startTime);
            }
            if (!string.IsNullOrEmpty(query.endTime))
            {
                builder.AppendFormat(" and buydate <= '{0}'", query.endTime);
            }
            if (!string.IsNullOrEmpty(query.storeCodes))
            {
                builder.AppendFormat(" and (storeCode in ({0}) or MemberStoreCode in ({0}))", query.storeCodes);
            }
            if (!string.IsNullOrEmpty(query.cell))
            {
                builder.AppendFormat(" and (mobile LIKE '%{0}%' or Membermobile LIKE '%{0}%')", DataHelper.CleanSearchString(query.cell));
                //builder.Append(" and (mobile in (select CellPhone from dbo.CW_Members where RelationUserId > 0) or (select count(*) from dbo.CW_O2OMembersAttribute where userid = m.userid) >= 10)");
            }
            builder.Append(" order by userid ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public DataTable GetO2OMemberExprot(CW_O2OMemberQuery query, IList<string> fields)
        {
            DataTable table = null;
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

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"select {0} from dbo.CW_O2OMembers left join dbo.CW_StoreInfo on dbo.CW_O2OMembers.storeCode = dbo.CW_StoreInfo.accountALLHere  where 1=1 ", str);

            if (!string.IsNullOrEmpty(query.name))
            {
                builder.AppendFormat(" and (name like '%{0}%' or mobile like '%{0}%')", DataHelper.CleanSearchString(query.name));
            }
            if (!string.IsNullOrEmpty(query.storeCode))
            {
                builder.AppendFormat(" and storeCode = '{0}'", DataHelper.CleanSearchString(query.storeCode));
            }
            if (!string.IsNullOrEmpty(query.startTime))
            {
                builder.AppendFormat(" and buydate >= '{0}'", query.startTime);
            }
            if (!string.IsNullOrEmpty(query.endTime))
            {
                builder.AppendFormat(" and buydate < '{0}'", Convert.ToDateTime(query.endTime).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(query.storeCodes))
            {
                builder.AppendFormat(" and storeCode in ({0}) ", query.storeCodes);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
                builder.AppendFormat(" order by {0} {1}", query.SortBy, query.SortOrder.ToString());
            else
                builder.AppendFormat(" order by userid {0} ", query.SortOrder.ToString());
            
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }
    
    }
}
