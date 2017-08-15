namespace Hishop.Weixin.MP.Api
{
    using Hishop.Weixin.MP.Util;
    using System;

    public class MenuApi
    {
        public static string CreateMenus(string accessToken, string json)
        {
            WriteLog("accessToken_"+ accessToken);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);
            return new WebUtils().DoPost(url, json);
        }
        public static void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/wx_.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }

        public static string DeleteMenus(string accessToken)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", accessToken);
            return new WebUtils().DoGet(url, null);
        }

        public static string GetMenus(string accessToken)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);
            return new WebUtils().DoGet(url, null);
        }
    }
}

