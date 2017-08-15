namespace Hidistro.Jobs
{
    using Hidistro.Core.Jobs;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Xml;

    public class ProductJob : IJob
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /// <summary>
        /// 商品维护提醒 2017-8-9 yk
        /// </summary>
        public void Execute(XmlNode node)
        {
            DataTable dtRemind = DistributorsBrower.GetProductMaintainRemindData();//当前维护提醒主表
            if (dtRemind.Rows.Count > 0)
            {
                foreach (DataRow rowRemind in dtRemind.Rows)
                {
                    #region *******得到该商品的所有订单***********
                    string query = string.Format("select oi.OrderId,UserId,ItemDescription,o.PayDate from  Hishop_Orders o left join  Hishop_OrderItems oi on o.orderId=oi.OrderId  where ProductId={0}", rowRemind["ProductId"]);
                    DataTable tableOrder = GetDataTable(query);
                    #endregion
                    foreach (DataRow rowOrder in tableOrder.Rows)
                    {
                        //***********根据当前订单和模板ID 查询提醒详情******************
                        string Str = string.Format("select*from CW_ProductReminDetail where OrderId='{0}' and MrID={1}", rowOrder["OrderId"],rowRemind["MrID"]);
                        ProductReminDetailInfo Info = GetRemainDetailinfo(Str);
                        if (Info!=null)
                        {
                            #region**************若当前订单提醒存在则是累计提醒************************
                            if (int.Parse(rowRemind["RemindNum"].ToString()) > Info.RemindNum)//如果该商品提醒总次数大于目前次数时则提醒
                            {
                                // ********现在时间=上一次提醒时间+警告周期  发送维护消息**************
                                if (!string.IsNullOrEmpty(Info.RemindDate.ToString()) && DateTime.Now.AddDays((double)-int.Parse(rowRemind["RemindCycle"].ToString())).ToString("yyyy-MM-dd") == Info.RemindDate.ToString("yyyy-MM-dd"))
                                {
                                    ProductBrowser.OrderPaymentToProductMaintainRemind(int.Parse(rowOrder["UserId"].ToString()), rowOrder["OrderId"].ToString(), rowOrder["ItemDescription"].ToString(), rowRemind["RemindRemark"].ToString());//发送消息
                                    //保存提醒记录(修改)
                                    Info.RemindRemark = rowRemind["RemindRemark"].ToString();
                                    updateRemainDetail(Info);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region**************若当前订单提醒不存在为作新提醒************************
                            // ********现在时间=订单付款时间+警告周期  发送维护消息**************
                            if (!string.IsNullOrEmpty(rowOrder["PayDate"].ToString()) && DateTime.Now.AddDays((double)-int.Parse(rowRemind["RemindCycle"].ToString())).ToString("yyyy-MM-dd") == DateTime.Parse(rowOrder["PayDate"].ToString()).ToString("yyyy-MM-dd"))
                              {
                                  ProductBrowser.OrderPaymentToProductMaintainRemind(int.Parse(rowOrder["UserId"].ToString()), rowOrder["OrderId"].ToString(), rowOrder["ItemDescription"].ToString(), rowRemind["RemindRemark"].ToString());//发送消息
                                    //保存提醒记录 新增
                                  ProductReminDetailInfo infos = new ProductReminDetailInfo 
                                  {
                                      MrID = int.Parse(rowRemind["MrID"].ToString()),
                                      OrderId = rowOrder["OrderId"].ToString(),
                                      RemindDate=DateTime.Now,
                                      RemindRemark = rowRemind["RemindRemark"].ToString(),
                                      RemindNum=1
                                  };
                                  InserRemainDetail(infos);
                              }
                            #endregion
                        }
                    }
                }
            }
        }
        public DataTable GetDataTable(string sql)
        {
            DbCommand command = database.GetSqlStringCommand(sql);
            DataTable table = database.ExecuteDataSet(command).Tables[0];
            return table;
        }
        public ProductReminDetailInfo GetRemainDetailinfo(string sql)
        {
            ProductReminDetailInfo info = new ProductReminDetailInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                info = ReaderConvert.ReaderToModel<ProductReminDetailInfo>(reader);
            }
            return info;
        }
        public void updateRemainDetail(ProductReminDetailInfo Info)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" update CW_ProductReminDetail set RemindDate=@RemindDate,RemindNum=RemindNum+1,RemindRemark=@RemindRemark where MrDetailID=@MrDetailID");
            database.AddInParameter(sqlStringCommand, "RemindDate", DbType.DateTime,DateTime.Now);
            database.AddInParameter(sqlStringCommand, "RemindRemark", DbType.String,Info.RemindRemark);
            database.AddInParameter(sqlStringCommand, "MrDetailID", DbType.Int32,Info.MrDetailID);
            database.ExecuteNonQuery(sqlStringCommand);
        }
        public void InserRemainDetail(ProductReminDetailInfo Info)
        {
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" insert into CW_ProductReminDetail(MrID,OrderId,RemindDate,RemindNum,RemindRemark) values(@MrID,@OrderId,@RemindDate,@RemindNum,@RemindRemark)");
            database.AddInParameter(sqlStringCommand, "RemindDate", DbType.DateTime, Info.RemindDate);
            database.AddInParameter(sqlStringCommand, "RemindRemark", DbType.String, Info.RemindRemark);
            database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, Info.OrderId);
            database.AddInParameter(sqlStringCommand, "MrID", DbType.Int32, Info.MrID);
            database.AddInParameter(sqlStringCommand, "RemindNum", DbType.Int32, Info.RemindNum);
            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

