using Hidistro.Entities.Commodities;
using Hidistro.Entities.CWAPI;
using Hidistro.Entities.Orders;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Serialization;


namespace Hishop.Weixin.MP.Util
{
    public class CwAHAPI
    {
        private static string apiProductUrl = "http://183.62.206.240:8088/jlwebserver/services/MPFTOJL?wsdl";          //提交商品-Url
        //private static string apiProductUrl = "http://183.62.206.240:8088/jlwebserver/services/MPFTOJL";          //提交商品-Url
        private static string apiOrderoUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";           //发送订单到AH-Url
        private static string apiOrderDeliveryUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";    //订单发货接口-Url
        private static string apiOrderAchieveUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";     //订单完成接口-Url
        private static string apiOrderoReturnUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";     //订单退货接口-Url
        private static string apiReceiptUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";          //发票接口-Url

        /// <summary>
        /// 格式化商品回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="message">消息</param>
        /// <param name="spnmcode">商品内码</param>
        /// <param name="mpfspcode">商品编码</param>
        /// <returns>0为成功，-1失败</returns>
        public static int ResolutionProductAHResultString(string strResult, out string message, out string spnmcode, out string mpfspcode)
        {
            int iresult = -1;
            CwProductReceive cwreceive = CwAPI.JsonToModel<CwProductReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == "1")
            {
                //成功
                iresult = 0;
            }
            spnmcode = cwreceive.SPNM.ToString();
            mpfspcode = cwreceive.MPF_SP.ToString();
            message = cwreceive.RSPDESC;
            return iresult;
        }

        /// <summary>
        /// 格式化订单回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="message">消息</param>
        /// <param name="orderid">订单编码</param>
        /// <returns>0为成功，-1失败</returns>
        public static int ResolutionOrderAHResultString(string strResult, out string message, out string orderid)
        {
            int iresult = -1;
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;//说明
            return iresult;
        }

        /*
        /// <summary>
        /// 格式化订单发货回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="message">消息</param>
        /// <param name="orderid">订单编码</param>
        /// <returns>0为成功，-1失败</returns>
        public static int ResolutionOrderDeliveryToAHResultString(string strResult, out string message, out string orderid)
        {
            int iresult = -1;
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;//说明
            return iresult;
        }

        /// <summary>
        /// 格式化订单完成回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="message">消息</param>
        /// <param name="orderid">订单编码</param>
        /// <returns>0为成功，-1失败</returns>
        public static int ResolutioOrderAchieveToAHResultString(string strResult, out string message, out string orderid)
        {
            int iresult = -1;
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;
            return iresult;
        }

        public static int ResolutioOrderoReturnToAHResultString(string strResult, out string message, out string orderid)
        {
            int iresult = -1;
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;
            return iresult;
        }
         * */

        /// <summary>
        /// 格式化发票地址回调内容
        /// </summary>
        /// <param name="strResult">接收字符</param>
        /// <param name="orderNo">订单编码</param>
        /// <returns></returns>
        public static string ResolutioSendReceiptAHResultString(string strResult, out string orderNo)
        {
            string path = "";
            CwReceipt cwreceipt = CwAPI.JsonToModel<CwReceipt>(strResult);
            if (cwreceipt != null && !string.IsNullOrEmpty(cwreceipt.TaxPath))
            {
                //path = cwreceipt.TaxPath;
                //保存发票pdf文件到本地
                string strPdfPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ReceiptPdf\\" + cwreceipt.OrderNo + ".PDF";
                if (!string.IsNullOrEmpty(HttpDownloadFile(cwreceipt.TaxPath, strPdfPath)))
                {
                    path = "/ReceiptPdf/" + cwreceipt.OrderNo + ".PDF";
                    
                    /*
                    //将pdf文件转换为图片
                    string strImagePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "PdfToImage\\";
                    PdfToImage.ConvertPDFToImage(strPdfPath, strImagePath, cwreceipt.OrderNo, 1, 2, ImageFormat.Png, PdfToImage.Definition.Nine);
                    path = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/PdfToImage/" + cwreceipt.OrderNo + "." + ImageFormat.Png.ToString();
                     * */
                }
            }
            orderNo = cwreceipt.OrderNo;
            return path;
        }


        /// <summary>
        /// 提交商品
        /// </summary>
        /// <param name="productinfo">商品信息</param>
        /// <param name="supCode">所属供应商Code</param>
        /// <param name="message">调用说明，如有错误则为错误原因</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static int SendProductToAH(ProductInfo productinfo, string supCode, out string message, out int spnm)
        {
            int iresult = -1;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"MPF_SP\":\"{0}\",", productinfo.ProductId);
            strJson.AppendFormat("\"SPNM\":\"{0}\",", "");
            strJson.AppendFormat("\"SPNAME\":\"{0}\",", productinfo.ProductName);
            strJson.AppendFormat("\"SPXH\":\"{0}\",", productinfo.SKU);
            strJson.AppendFormat("\"WEIGHT\":\"{0}\",", productinfo.Weight);
            strJson.AppendFormat("\"METERING\":\"{0}\",", productinfo.Unit);
            strJson.AppendFormat("\"SUPCODE\":\"{0}\"", supCode);
            strJson.Append("}");
            string strResult = new WebUtils().HttpConnectToServer(apiProductUrl + "/MPFTOJL_SPXX", "MPFTOJL_SPXX", strJson.ToString());
            CwProductReceive cwreceive = CwAPI.JsonToModel<CwProductReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == "1")
            {
                //成功
                iresult = 0;
                spnm = cwreceive.SPNM;
            }
            else
            {
                //失败
                iresult = 1;
                spnm = 0;
            }
            message = cwreceive.RSPDESC;
            return iresult;
        }

        /// <summary>
        /// 发送订单到AH
        /// </summary>
        /// <param name="orderinfo">订单实体信息</param>
        /// <param name="serialNum">交易流水号</param>
        /// <param name="message">操作返回说明</param>
        /// <returns>返回0为成功，其它为失败</returns>
        public static int SendOrderoAH(OrderInfo orderinfo, string serialNum, out string message, out string orderid)
        {
            int iresult = -1;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderinfo.ShipTo);
            strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderinfo.TelPhone);
            strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderinfo.CellPhone);
            strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderinfo.RegionId.ToString().Substring(0, 2));
            strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderinfo.RegionId.ToString().Substring(0, 4));
            strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderinfo.RegionId.ToString());
            strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", cleanString(orderinfo.Address));
            strJson.AppendFormat("\"TaxName\":\"{0}\",", "创维直销部");
            strJson.AppendFormat("\"TaxPhone\":\"{0}\",", "027-81234567");
            strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", cleanString(orderinfo.Address));
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderinfo.OrderId);
            strJson.AppendFormat("\"OrderTime\":\"{0}\",", orderinfo.OrderDate);
            strJson.AppendFormat("\"FValue\":\"{0}\",", orderinfo.GetAmount().ToString("0.00"));//商品总价格
            strJson.AppendFormat("\"ReValue\":\"{0}\",", orderinfo.GetTotal().ToString("0.00"));//订单实际支付价格
            strJson.AppendFormat("\"OrderNote\":\"{0}\",", orderinfo.Remark);
            //strJson.AppendFormat("\"SerialNum\":\"{0}\",", serialNum);//serialNum 交易流水号
            strJson.Append("\"Details\":[");
            foreach (LineItemInfo iteminfo in orderinfo.LineItems.Values)
            {
                strJson.Append("{");
                strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//商品内码
                strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemListPrice.ToString("0.00"));//商品一口价格
                strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品结算价
                strJson.Append("}");
            }
            strJson.Append("]");
            strJson.Append("}");
            string strResult = new WebUtils().DoPost(apiOrderoUrl, strJson.ToString());
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            orderid = cwreceive.ORDERID;//订单编号
            message = cwreceive.RSPDESC;//说明
            return iresult;
        }


        /// <summary>
        /// 订单发货接口
        /// </summary>
        /// <param name="productinfo">商品信息</param>
        /// <param name="supCode">所属供应商Code</param>
        /// <param name="message">调用说明，如有错误则为错误原因</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static int SendOrderDeliveryToAH(OrderInfo orderinfo, string serialNum, out string message)
        {
            int iresult = -1;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"ReceivName\":\"{0}\",", orderinfo.ShipTo);//收货人名称
            strJson.AppendFormat("\"ReceivTel\":\"{0}\",", orderinfo.TelPhone);//收货人手机
            strJson.AppendFormat("\"ReceivPhone\":\"{0}\",", orderinfo.CellPhone);//收货人电话
            strJson.AppendFormat("\"ReceivProvice\":\"{0}\",", orderinfo.RegionId.ToString().Substring(0, 2));//收货人地址行政区域省的编码
            strJson.AppendFormat("\"ReceivCity\":\"{0}\",", orderinfo.RegionId.ToString().Substring(0, 4));//收货人地址行政区域市的编码
            strJson.AppendFormat("\"ReceivArea\":\"{0}\",", orderinfo.RegionId.ToString());//收货人地址行政区域区的编码
            strJson.AppendFormat("\"ReceivAddress\":\"{0}\",", cleanString(orderinfo.Address));//收货人详细地址
            strJson.AppendFormat("\"TaxName\":\"{0}\",", "创维直销部");//发票抬头
            strJson.AppendFormat("\"TaxPhone\":\"{0}\",", "027-81234567");//发票电话
            strJson.AppendFormat("\"TaxMailAdd\":\"{0}\",", cleanString(orderinfo.Address));//发票邮寄地址
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderinfo.OrderId);//订单号
            strJson.AppendFormat("\"OrderNote\":\"{0}\",", orderinfo.Remark);//买家留言
            strJson.AppendFormat("\"DelivCode\":\"{0}\",", orderinfo.ExpressCompanyAbb);//快递公司代码
            strJson.AppendFormat("\"DelivName\":\"{0}\",", orderinfo.ExpressCompanyName);//快递公司名称
            strJson.AppendFormat("\"DelivOrderCode\":\"{0}\"", orderinfo.ShipOrderNumber);//快递单号
            //strJson.AppendFormat("\"SerialNum\":\"{0}\"", serialNum);//serialNum 交易流水号 
            strJson.Append("}");
            string strResult = new WebUtils().DoPost(apiOrderDeliveryUrl, strJson.ToString());
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            message = cwreceive.RSPDESC;
            return iresult;
        }


        /// <summary>
        /// 订单完成接口
        /// </summary>
        /// <param name="orderinfo">订单ID</param>
        /// <param name="message">调用说明，如有错误则为错误原因</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static int SendOrderAchieveToAH(OrderInfo orderinfo, out string message)
        {
            int iresult = -1;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderinfo.OrderId);//订单号
            strJson.AppendFormat("\"FinishDate\":\"{0}\"", orderinfo.FinishDate);//订单完成时间
            strJson.Append("}");
            string strResult = new WebUtils().DoPost(apiOrderAchieveUrl, strJson.ToString());
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            message = cwreceive.RSPDESC;
            return iresult;
        }

        /// <summary>
        /// 订单退货接口
        /// </summary>
        /// <param name="OrderInfo">订单实体信息</param>
        /// <param name="HandleTime">退货时间</param>
        /// <param name="message">调用说明，如有错误则为错误原因</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static int SendOrderoReturnAH(OrderInfo orderinfo, string HandleTime, out string message)
        {
            int iresult = -1;
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"OrderNo\":\"{0}\",", orderinfo.OrderId);
            strJson.AppendFormat("\"HandleTime\":\"{0}\",", HandleTime);
            strJson.Append("\"Details\":[");
            foreach (LineItemInfo iteminfo in orderinfo.LineItems.Values)
            {
                strJson.Append("{");
                strJson.AppendFormat("\"GoodsCode\":\"{0}\",", iteminfo.ProductCode);//商品内码
                strJson.AppendFormat("\"OrderQuantity\":\"{0}\",", iteminfo.Quantity);//数据
                strJson.AppendFormat("\"OrderFprice\":\"{0}\",", iteminfo.ItemListPrice.ToString("0.00"));//商品一口价格
                strJson.AppendFormat("\"OrderRePrice\":\"{0}\"", iteminfo.ItemAdjustedPrice.ToString("0.00"));//商品结算价
                strJson.Append("}");
            }
            strJson.Append("]");
            strJson.Append("}");
            string strResult = new WebUtils().DoPost(apiOrderoReturnUrl, strJson.ToString());
            CwOrderReceive cwreceive = CwAPI.JsonToModel<CwOrderReceive>(strResult);
            if (cwreceive != null && cwreceive.STATE == 1)
            {
                iresult = 0;
            }
            message = cwreceive.RSPDESC;
            return iresult;
        }


        /// <summary>
        /// 发票接口
        /// </summary>
        /// <param name="orderinfo">订单实体信息</param>
        /// <param name="message">调用说明，如有错误则为错误原因</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static string SendReceiptAH(OrderInfo orderinfo, out string message)
        {
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            strJson.AppendFormat("\"OrderNo\":\"{0}\"", orderinfo.OrderId);//订单号
            strJson.Append("}");
            string strResult = new WebUtils().DoPost(apiReceiptUrl, strJson.ToString());
            CwReceipt cwreceipt = CwAPI.JsonToModel<CwReceipt>(strResult);
            string path = "";
            if (cwreceipt != null && string.IsNullOrEmpty(cwreceipt.TaxPath))
            {
                //string name = "https://www.baidu.com/img/bd_logo1.png";
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ReceiptPdf\\" + cwreceipt.OrderNo + ".PDF";
                HttpDownloadFile(cwreceipt.TaxPath, path);
            }
            message = cwreceipt.OrderNo;
            return path;
        }

        /// <summary>
        /// Http下载文件
        /// </summary>
        public static string HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }
        /// <summary>
        /// 特殊字符处理
        /// </summary>
        /// <param name="strvalue">需要处理的字符串</param>
        /// <returns>返回处理过后的字符串</returns>
        public static string cleanString(string strvalue)
        {
            string strResult = string.Empty;
            if (!string.IsNullOrEmpty(strvalue))
            {
                //strResult = strvalue.Replace("&nbsp;", "").Replace("&", "").Replace(";", ",").Replace("*", "").Trim();
                strResult = strvalue.Replace("&nbsp;", " ").Replace(";", "；").Replace("<br>", " ").Replace("*", "＊").Replace("<", "＜").Replace(">", "＞").Replace("&", "＆").Replace("'", "＇").Trim().Replace("\"", "＂").Trim();
            }

            return strResult;
        }

        private static int IsSaveLog = 1;

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="str">要记录的信息，字符串</param>
        public static void CwapiLog(string str)
        {
            if (IsSaveLog == 1)
            {
                string filename = "/LogsCw/" + System.DateTime.Now.ToString("yyyy-MM-dd") + "_CWAPI.txt";
                string path = HttpContext.Current.Server.MapPath(filename);
                System.IO.StreamWriter writer = System.IO.File.AppendText(path);
                writer.WriteLine("时间：" + System.DateTime.Now + "-------" + str);
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="str">要记录的信息，字符串</param>
        public static void KkapiLog(string str)
        {
            if (IsSaveLog == 1)
            {
                string filename = "/LogsCw/" + System.DateTime.Now.ToString("yyyy-MM-dd") + "_KKAPI.txt";
                string path = HttpContext.Current.Server.MapPath(filename);
                System.IO.StreamWriter writer = System.IO.File.AppendText(path);
                writer.WriteLine("时间：" + System.DateTime.Now + "-------" + str);
                writer.Flush();
                writer.Close();
            }
        }

    }
}
