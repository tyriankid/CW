

namespace Hishop.Weixin.MP.Util
{
    using Hidistro.Entities.CWAPI;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hishop.Weixin.MP.Util;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class CwAPI
    {
        private static string appId = "2016090101";
        private static string getKeyAction = "GetEncode";
        private static string addOrderAction = "AddOrder";
        private static string getStockAction = "GetStock";
        private static string apiUrl = "http://port.skyworth.com:8000/WebAPI/api.ashx";

        public static ResultXml GetKeyValue()
        {
            string strParamValue = string.Format("action={0}&appId={1}", getKeyAction, appId);
            string strResult = new WebUtils().DoPost(apiUrl, strParamValue);
            ResultXml resultxml = GetResult(strResult);
            return resultxml;
        }

        /// <summary>
        /// 根据商品编码得到库存信息
        /// </summary>
        /// <param name="procode">商品编码</param>
        /// <returns></returns>
        public static int GetStock(string procode)
        {
            int istock = 0;
            string strParamValue = string.Format("ProNO={0}&action={1}&appId={2}", procode, getStockAction, appId);
            //string strParamValue = string.Format("ProNO={0}&action={1}&appId={2}", "63212", getStockAction, appId);
            string strResult = new WebUtils().DoPost(apiUrl, strParamValue);
            List<SynGoodsStore> SynGoodsStore_List = JsonToListModel<SynGoodsStore>("[" + strResult + "]");
            if (SynGoodsStore_List.Count > 0)
            {
                if (SynGoodsStore_List[0].SynGoodsStore_header.State == "0")
                {
                    //得到库存成功
                    if (SynGoodsStore_List[0].SynGoodsStore_body.GoodsCode.Equals(procode))
                    {
                        int.TryParse(SynGoodsStore_List[0].SynGoodsStore_body.StoreQuantity, out istock);
                    }
                }
            }
            return istock;//库存值
        }

        /// <summary>
        /// 提交订单到港口系统
        /// </summary>
        /// <param name="orderinfo">订单实体对象</param>
        /// <param name="storecode">门店金立账号</param>
        /// <param name="serialNum">交易流水号</param>
        /// <param name="message">文字输出说明</param>
        /// <returns>接口返回值，0为成功，其它失败</returns>
        public static int SendOrderToGankKou(OrderInfo orderinfo, string storecode, out string message)
        {
            int iresult = -1;
            try
            {
                //创维内部商品则调用接口传输订单到港口系统
                if (orderinfo.OrderSource == 1)
                {
                    ResultXml resultxml = GetKeyValue();

                    //构建订单数据提交到港口
                    StringBuilder strXml = new StringBuilder();
                    strXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    strXml.Append("<SynSaleOrder>");
                    strXml.Append("<OrderCustormer>");
                    strXml.AppendFormat("<ReceivName>{0}</ReceivName>", cleanString(orderinfo.ShipTo));//用户名称
                    strXml.AppendFormat("<ReceivTel>{0}</ReceivTel>", cleanString(orderinfo.CellPhone));//联系电话
                    strXml.AppendFormat("<ReceivPhone>{0}</ReceivPhone>", "");//手机手机
                    strXml.AppendFormat("<ReceivProvice>{0}</ReceivProvice>", orderinfo.RegionId.ToString().Substring(0, 2));//地区省
                    strXml.AppendFormat("<ReceivCity>{0}</ReceivCity>", orderinfo.RegionId.ToString().Substring(0, 4));//地区市
                    strXml.AppendFormat("<ReceivArea>{0}</ReceivArea>", orderinfo.RegionId.ToString());//地区区
                    strXml.AppendFormat("<ReceivAddress>{0}</ReceivAddress>", cleanString(orderinfo.Address));//详细地址
                    strXml.Append("</OrderCustormer>");
                    strXml.Append("<OrderTax>");
                    strXml.Append("<TaxType>2</TaxType>");//0为普通发票
                    strXml.AppendFormat("<TaxName>{0}</TaxName>", string.IsNullOrEmpty(orderinfo.TelPhone) ? orderinfo.ShipTo : orderinfo.TelPhone);//发票抬头 - 收货人
                    strXml.Append("</OrderTax>");
                    strXml.Append("<OrderMain>");
                    strXml.AppendFormat("<OrderId>{0}</OrderId>", orderinfo.id);//订单ID
                    strXml.AppendFormat("<OrderNo>{0}</OrderNo>", orderinfo.OrderId);//订单号
                    strXml.AppendFormat("<OrderTime>{0}</OrderTime>", orderinfo.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"));//订单时间
                    strXml.Append("<ORDERNO_DJ></ORDERNO_DJ>");
                    strXml.Append("<FVALUE_DJ></FVALUE_DJ>");
                    strXml.AppendFormat("<FValue>{0}</FValue>", orderinfo.GetAmount().ToString("0.00"));//订单原总额
                    strXml.AppendFormat("<ReValue>{0}</ReValue>", orderinfo.GetTotal().ToString("0.00"));//订单总额
                    strXml.Append("<OrderNote>直销部微商城订单</OrderNote>");//订单备注
                    strXml.Append("<DelivCode>0</DelivCode>");//暂时为0 ，物流单号
                    strXml.AppendFormat("<Freight>{0}</Freight>", orderinfo.AdjustedFreight);//运费
                    strXml.AppendFormat("<AdjustedDiscount>{0}</AdjustedDiscount>", orderinfo.AdjustedDiscount);//订单折扣
                    strXml.AppendFormat("<DelivName>{0}</DelivName>", orderinfo.ExpressCompanyName);//物流公司名称
                    //得到门店信息
                    strXml.AppendFormat("<OrderDesc>{0}</OrderDesc>", storecode);//门店号
                    //strXml.AppendFormat("<OrderDesc>DZ160159</OrderDesc>");//门店号
                    strXml.Append("</OrderMain>");
                    strXml.Append("<OrderPay>");
                    strXml.AppendFormat("<OrderPayTime>{0}</OrderPayTime>", DateTime.Now);//支付时间
                    strXml.AppendFormat("<OrderPayNote> 支付类型：{0} 流水号：{1}</OrderPayNote>", orderinfo.PaymentType, orderinfo.GatewayOrderId);
                    //根据PayTypeList.xml创维文档，得到paytype的值。
                    string strPayment = string.Empty;
                    if (orderinfo.PaymentType.IndexOf("支付宝") >= 0)
                    {
                        strPayment = "alipayinstant";//港口提供值
                    }
                    else if (orderinfo.PaymentType.IndexOf("微信") >= 0)
                    {
                        strPayment = "tenpaywx";//港口提供值
                    }
                    strXml.AppendFormat("<OrderPayment>{0}</OrderPayment>", strPayment);//支付类型码，与PayTypeList.xml中的类型paytype值匹配
                    strXml.AppendFormat("<OrderPayName>{0}</OrderPayName>", orderinfo.PaymentType);//支付方式名称
                    strXml.AppendFormat("<OrderPayValue>{0}</OrderPayValue>", orderinfo.GetTotal().ToString("0.00"));
                    strXml.Append("</OrderPay>");
                    strXml.Append("<Details>");
                    foreach (LineItemInfo iteminfo in orderinfo.LineItems.Values)
                    {
                        strXml.Append("<OrderDetail>");
                        strXml.AppendFormat("<GoodsCode>{0}</GoodsCode>", iteminfo.ProductCode);
                        //strXml.AppendFormat("<GoodsCode>{0}</GoodsCode>", "63212");//港口提供测试商品号
                        strXml.AppendFormat("<OrderQuantity>{0}</OrderQuantity>", iteminfo.Quantity);
                        strXml.AppendFormat("<OrderFprice>{0}</OrderFprice>", iteminfo.ItemListPrice.ToString("0.00"));
                        strXml.AppendFormat("<OrderRePrice>{0}</OrderRePrice>", iteminfo.ItemAdjustedPrice.ToString("0.00"));
                        strXml.Append("</OrderDetail>");
                    }
                    strXml.Append("</Details>");
                    strXml.Append("</SynSaleOrder>");
                    //定义参数
                    //CwAHAPI.CwapiLog("发送港口数据：" + strXml.ToString());
                    string paramXml = strXml.ToString();
                    string paramHkXml = string.Empty;
                    string paramkey = resultxml.Message;
                    string strResult = new WebUtils().DoPost(apiUrl, "xml=" + paramXml + "&HkXml=" + paramHkXml + "&action=" + addOrderAction + "&key=" + paramkey + "&appId=" + appId);
                    //CwAHAPI.CwapiLog("接收港口数据：" + strResult);
                    ResultXml rxml = GetResult(strResult);
                    if (rxml != null)
                    {
                        int.TryParse(rxml.State, out iresult);
                        message = rxml.Message;
                    }
                    else
                        message = "接口返回对象出错";
                }
                else
                    message = "订单不是创维内部商品订单";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return iresult;
        }


        public static string cleanString(string strvalue)
        {
            string strResult = string.Empty;
            if (!string.IsNullOrEmpty(strvalue))
            {
                //strResult = strvalue.Replace("&nbsp;", "").Replace("&", "").Replace(";", ",").Replace("*", "").Trim();
                strResult = strvalue.Replace("&nbsp;", " ").Replace(";", "；").Replace("*", "＊").Replace("<", "＜").Replace(">", "＞").Replace("&", "＆").Replace("'", "＇").Trim().Replace("\"", "＂").Trim();
            }
            return strResult;
        }

        /// <summary>
        /// 根据xml字符串得到对象值
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static ResultXml GetResult(string xmlString)
        {
            ResultXml resultxml = new ResultXml();
            //Xml解析  
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeList xxList = doc.GetElementsByTagName("Result"); //取得节点名为Result的集合  
            foreach (XmlNode xxNode in xxList)  //xxNode 是每一个<CL>...</CL>体  
            {
                XmlNodeList childList = xxNode.ChildNodes; //取得CL下的子节点集合  
                foreach (XmlNode node in childList)
                {
                    String temp = node.Name;
                    switch (temp)
                    {
                        case "State"://返回状态
                            resultxml.State = node.InnerText;
                            break;
                        case "Message"://返回值
                            resultxml.Message = node.InnerText;
                            break;
                    }
                }
            }
            return resultxml;
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

        /// <summary>
        /// JSON字条串转实体
        /// </summary>
        public static List<T> JsonToListModel<T>(string jsonText)
        {
            List<T> list = new List<T>();
            DataContractJsonSerializer _Json = new DataContractJsonSerializer(list.GetType());
            byte[] _Using = System.Text.Encoding.UTF8.GetBytes(jsonText);
            System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_Using);
            _MemoryStream.Position = 0;
            return (List<T>)_Json.ReadObject(_MemoryStream);
        }

    }

    #region 库存内容对象

    public class SynGoodsStore
    {
        //头信息
        public Storeheader SynGoodsStore_header { get; set; }
        //内容信息
        public Storebody SynGoodsStore_body { get; set; }
    }

    public class Storeheader
    {
        public string State { get; set; }

        public string Message { get; set; }
    }

    public class Storebody
    {
        public string GoodsCode { get; set; }

        public string StoreQuantity { get; set; }
    }

    #endregion 库存内容

}
