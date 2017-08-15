namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Jobs;
    using Hidistro.Entities.CWAPI;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;
    using System.Web;

    public class OrderJob : IJob
    {
        public void Execute(XmlNode node)
        {
            WriteLog("**********定时器进入判断是否存在过期并已经发货的订单*******");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(" UPDATE Hishop_Orders SET OrderStatus=4,CloseReason='过期没付款，自动关闭' WHERE OrderStatus=1 AND OrderDate <= @OrderDate;");
            database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double)-masterSettings.CloseOrderDays));
            database.ExecuteNonQuery(sqlStringCommand);
            string query = string.Format("SELECT OrderId FROM  Hishop_Orders WHERE  OrderStatus=3 AND ShippingDate <= '" + DateTime.Now.AddDays((double)-masterSettings.FinishOrderDays) + "'", new object[0]);
            DbCommand command = database.GetSqlStringCommand(query);
            DataTable table = database.ExecuteDataSet(command).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                bool flag = false;
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(table.Rows[i][0].ToString());
                Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
                LineItemInfo info2 = new LineItemInfo();
                foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
                {
                    info2 = pair.Value;
                    if ((info2.OrderItemsStatus == OrderStatus.ApplyForRefund) || (info2.OrderItemsStatus == OrderStatus.ApplyForReturns))
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    try
                    {
                        //只有供应商商品完成时才对接AH接口
                        if (orderInfo.OrderSource == 2)
                        {
                            WriteLog("******定时器开始调用接口*******");
                            //2016-11-07 开始调用完成订单接口
                            StringBuilder strJson = new StringBuilder();
                            strJson.Append("{");
                            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderInfo.OrderId);//订单号
                            strJson.AppendFormat("\"Date\":\"{0}\",", DateTime.Now);//订单完成时间
                            strJson.AppendFormat("\"OrderState\":\"{0}\"", "Finish");//订单完成时间
                            strJson.Append("}");
                            WriteLog("******发送的内容：" + strJson.ToString());
                            AllHereServiceReferenceJob.MPFTOJLClient ahservice = new AllHereServiceReferenceJob.MPFTOJLClient();
                            string strResult = ahservice.MPFTOJL_DHD_QS(strJson.ToString());
                            WriteLog("******接收的内容：" + strResult);
                            string orderid = string.Empty;
                            string message = string.Empty;
                            if (ResolutionOrderAHResultString(strResult, out message, out orderid) == -1)
                            {
                                WriteLog("******返回失败，原因：" + message);
                                continue;
                            }
                            if (orderid != orderInfo.OrderId)
                            {
                                WriteLog("******订单编码与发送的不相同。");
                                continue;
                            }
                            WriteLog("******订单完成接口对接成功!");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("******订单完成接口对接发生错误，原因：" + ex.ToString());
                        continue;
                    }
                    DbCommand command3 = database.GetSqlStringCommand(" UPDATE Hishop_Orders SET FinishDate = getdate(), OrderStatus = 5,CloseReason='订单自动完成' WHERE OrderStatus=3 AND ShippingDate <= @ShippingDate AND OrderId=@OrderId");
                    database.AddInParameter(command3, "ShippingDate", DbType.DateTime, DateTime.Now.AddDays((double)-masterSettings.FinishOrderDays));
                    database.AddInParameter(command3, "OrderId", DbType.String, orderInfo.OrderId);
                    if (database.ExecuteNonQuery(command3) > 0)
                    {
                        DistributorsBrower.UpdateCalculationCommission(orderInfo);
                        foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                        {
                            if (info3.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString() || info3.OrderItemsStatus.ToString() == OrderStatus.ConfirmTakeGoods.ToString())
                            {
                                DbCommand command4 = database.GetSqlStringCommand("update Hishop_OrderItems set OrderItemsStatus=@OrderItemsStatus where orderid=@orderid and skuid=@skuid");
                                database.AddInParameter(command4, "OrderItemsStatus", DbType.Int32, 5);
                                database.AddInParameter(command4, "skuid", DbType.String, info3.SkuId);
                                database.AddInParameter(command4, "orderid", DbType.String, orderInfo.OrderId);
                                database.ExecuteNonQuery(command4);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 格式化订单回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="message">消息</param>
        /// <param name="orderid">订单编码</param>
        /// <returns>0为成功，-1失败</returns>
        public int ResolutionOrderAHResultString(string strResult, out string message, out string orderid)
        {
            int iresult = -1;
            CwOrderReceive cwreceive = JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;//说明
            return iresult;
        }

        //<summary>
        //JSON字条串转实体
        //</summary>
        public static T JsonToModel<T>(string jsonText)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string strPath = string.Empty;

        public void WriteLog(string str)
        {
            //string path = HttpContext.Current.Server.MapPath("/CWAPI.txt");
            string filename = "/LogsCw/" + System.DateTime.Now.ToString("yyyy-MM-dd") + "_CWAPI.txt";
            if (string.IsNullOrEmpty(strPath))
                strPath = GetMapPath(filename);
            System.IO.StreamWriter writer = System.IO.File.AppendText(strPath);
            writer.WriteLine(str);
            writer.WriteLine(System.DateTime.Now);
            writer.Flush();
            writer.Close();
        }

        ///   
        /// 获得当前绝对路径  
        ///   
        /// 指定的路径  
        /// 绝对路径  
        public string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用  
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\") || strPath.StartsWith("~"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 0)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
    }
}

