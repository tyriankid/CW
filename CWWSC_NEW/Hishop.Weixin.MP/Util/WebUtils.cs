namespace Hishop.Weixin.MP.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using System.Xml;

    public sealed class WebUtils
    {
        public string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                    return url;
                }
                url = url + "?" + BuildQuery(parameters);
            }
            return url;
        }

        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string str2 = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        builder.Append("&");
                    }
                    builder.Append(key);
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(str2, Encoding.UTF8));
                    flag = true;
                }
            }
            return builder.ToString();
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public string DoGet(string url, IDictionary<string, string> parameters)
        { 
            if ((parameters != null) && (parameters.Count > 0))
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string DoPost(string url, string value)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
            return this.GetResponseAsString(rsp, Encoding.UTF8);
        }

        public string HttpToWebService(string ServerPage, string MethodName, string Value)
        {
            string responseString = "";
            try
            {
                Console.WriteLine("新开始进行连接测试");

                string param = @"<?xml version=""1.0"" encoding=""utf-8""?>                                         
                <soap:Envelope  xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""  xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">                                           
                <soap:Body>                                              
                <HelloWorld xmlns=""http://tempuri.org/"">                                               
                <StudentName>1</StudentName>                                               
                <PassWord>1</PassWord>                                             
                </HelloWorld>                                           
                </soap:Body>                                         
                </soap:Envelope>";

                byte[] bs = Encoding.UTF8.GetBytes(param); HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(ServerPage);
                myRequest.Method = "POST";
                myRequest.ContentType = "text/xml; charset=utf-8";
                myRequest.Headers.Add("SOAPAction", "http://tempuri.org/HelloWorld");
                myRequest.ContentLength = bs.Length;
                Console.WriteLine("完成准备工作");
                using (Stream reqStream = myRequest.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    responseString = sr.ReadToEnd();
                    Console.WriteLine("反馈结果" + responseString);
                };
                Console.WriteLine("完成调用接口");
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(System.DateTime.Now.ToShortTimeString() + "LBS EXCEPTION:" + e.Message);
                Console.WriteLine(e.StackTrace);
                return responseString;
            } 
        }

        //发送消息到服务器
        public string HttpConnectToServer(string ServerPage, string MethodName, string Value)
        {
            byte[] dataArray = Encoding.Default.GetBytes(Value);
            //创建请求
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServerPage);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "http://jlinterface.jlserver/MPFTOJL_SPXX");
            request.ContentLength = dataArray.Length;
            //创建输入流
            Stream dataStream = null;
            try
            {
                dataStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                return null;//连接服务器失败
            }
            //发送请求
            dataStream.Write(dataArray, 0, dataArray.Length);
            dataStream.Close();
            //读取返回消息
            string res = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                res = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                return null;//连接服务器失败
            }
            return res;
        }

        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream responseStream = null;
            StreamReader reader = null;
            string str;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return str;
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                request = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest) WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "Hishop";
            return request;
        }
    }
}

