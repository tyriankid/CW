<%@ WebHandler Language="C#" Class="Hidistro.UI.Web.API.wx" %>

using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web;
using Hidistro.Core;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Request.Event;

namespace Hidistro.UI.Web.API
{
    public class wx : IHttpHandler
    {
        //private bool CheckSignature(HttpContext context)
        //{
        //    string signature = context.Request.QueryString["signature"].ToString();
        //    string timestamp = context.Request.QueryString["timestamp"].ToString();
        //    string nonce = context.Request.QueryString["nonce"].ToString();
        //    string[] ArrTmp = { "8B5AADD5A0AFD951", timestamp, nonce };
        //    Array.Sort(ArrTmp);     //字典排序  
        //    string tmpStr = string.Join("", ArrTmp);
        //    tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        //    tmpStr = tmpStr.ToLower();
        //    if (tmpStr == signature)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        
        public void ProcessRequest(System.Web.HttpContext context)
        {
            //string echoStr = context.Request.QueryString["echoStr"].ToString();
            //if (CheckSignature(context))
            //{
            //    if (!string.IsNullOrEmpty(echoStr))
            //    {
            //        context.Response.Write(echoStr);
            //        context.Response.End();
            //    }
            //}

            System.Web.HttpRequest request = context.Request;
            string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            if (request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
                {
                    context.Response.Write(s);
                }
                else
                {
                    context.Response.Write("");
                }
                context.Response.End();
            }
            else
            {
                try
                {
                    CustomMsgHandler handler = new CustomMsgHandler(request.InputStream);
                    handler.Execute();
                    context.Response.Write(handler.ResponseDocument);
                    
                }
                catch (System.Exception exception)
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(context.Server.MapPath("error.txt"));
                    writer.WriteLine(exception.Message);
                    writer.WriteLine(exception.StackTrace);
                    writer.WriteLine(System.DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
        
        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/error.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
