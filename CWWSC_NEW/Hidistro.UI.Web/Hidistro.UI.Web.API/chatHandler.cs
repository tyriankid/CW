using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.CWAPI;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Plugins;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WxPayAPI;

namespace Hidistro.UI.Web.API
{

    public class chatHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string text = context.Request["action"];
            switch (text)
            {
                case "getUserInfoByIds":
                    this.getUserInfoByIds(context);
                    break;
                case "saveChatMessage":
                    saveChatMessage(context);
                    break;
                case "loadChatMessage":
                    loadChatMessage(context);
                    break;
                case "loadDialogList":
                    loadDialogList(context);
                    break;
                case "sendWxMessage":
                    sendWxMessage(context);
                    break;
            }
        }

        #region 聊天房间加解密
        private const int roomKey = 891011;
        private const int attrKey = 670;
        //生成加密的房间号
        private string encryptRoomNum(int hosterid,int memberid)
        {
            //1:接收者和发起者id从小到大排序
            ArrayList arr = new ArrayList();
            arr.Add(hosterid);
            arr.Add(memberid);
            arr.Sort();
            string roomName = "";
            for (int i = 0; i < arr.Count; i++)
            {
                arr[i] = arr[i].ToInt() ^ roomKey;
                roomName = roomName + arr[i].ToString()+ '|';
            }
            roomName = roomName.TrimEnd('|');
            return roomName;
        }
        //生成加密参数
        private string encryptRoomAttr(int hosterid, int memberid)
        {
            string roomName = "";
            roomName = roomName + (hosterid ^ attrKey).ToString() + '|';
            roomName = roomName + (memberid ^ attrKey).ToString();
            return roomName;
        }
        private string getChatAttrs(int hosterid,int memberid)
        {
            return encryptRoomNum(hosterid, memberid) + '|' + encryptRoomAttr(hosterid, memberid);
        }
        //解密房间参数
        private ArrayList decryptRoomAttr(string roomattr)
        {
            var a = roomattr.Split('|');
            var result = new ArrayList();
            for (int i = 0; i < a.Length; i++)
            {
                if (i <= 1)
                    result.Add(Convert.ToInt32(a[i]) ^ roomKey);
                else if (i > 1 && i <= 3)
                    result.Add(Convert.ToInt32(a[i]) ^ attrKey);
            }
            return result;
        }
        #endregion

        private void sendWxMessage(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                string hosterid = context.Request["hosterid"];
                string memberid = context.Request["memberid"];
                if(string.IsNullOrEmpty(hosterid) || string.IsNullOrEmpty(memberid))
                {
                    context.Response.Write(string.Format("{{\"state\":2}}"));return;
                }
                MemberInfo sender = MemberHelper.GetMember(hosterid.ToInt());
                MemberInfo reciver = MemberHelper.GetMember(memberid.ToInt());
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                 string   url = masterSettings.CurrentDomain + ":3000/userChat.html?tp=wx&k=" + getChatAttrs(memberid.ToInt(), hosterid.ToInt());


                TemplateMessage templateMessage = new TemplateMessage();
                
                templateMessage.TemplateId = "kB_wQgstT4UVjI71Za0O3mzbZzsK6VKOWz0hf07WQw4";//Globals.GetMasterSettings(true).WX_Template_01;// "b1_ARggaBzbc5owqmwrZ15QPj9Ksfs0p5i64C6MzXKw";//消息模板ID
                templateMessage.Touser = reciver.OpenId;//用户OPENID
                templateMessage.Url = url;
                
                TemplateMessage.MessagePart[] messateParts = new TemplateMessage.MessagePart[]{
                                                    new TemplateMessage.MessagePart{Name = "first",Value = "你收到了一条新消息！"},
                                                    new TemplateMessage.MessagePart{Name = "keyword1",Value =sender.UserName},
                                                    new TemplateMessage.MessagePart{Name = "keyword2",Value =DateTime.Now.ToShortTimeString()},
                                                    new TemplateMessage.MessagePart{Name = "remark",Value = "点击进入聊天页面"}};
                templateMessage.Data = messateParts;
                TemplateApi.SendMessage(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), templateMessage);

                //插入该用户未读消息表
                DataTable dtNotreadInfo = UserNotReadHelper.GetUserNotReadData(" where JSUserId =" + reciver.UserId);
                if (dtNotreadInfo.Rows.Count <= 0) //add
                {
                    CwAHAPI.CwapiLog("进入了add");
                    UserNotRead info = new UserNotRead()
                    {
                        ID = Guid.NewGuid(),
                        UpdateTime = DateTime.Now,
                        FQUserId = sender.UserId,
                        JSUserId = reciver.UserId,
                        NotReadMsgCount = 1
                    };
                    UserNotReadHelper.AddUserNotRead(info);
                    CwAHAPI.CwapiLog("进入了add成功");
                }
                else //update
                {
                    CwAHAPI.CwapiLog("进入了edit");
                    UserNotRead info = UserNotReadHelper.GetUserNotRead(new Guid(dtNotreadInfo.Rows[0]["ID"].ToString()));
                    info.UpdateTime = DateTime.Now;
                    info.NotReadMsgCount = info.NotReadMsgCount + 1;
                    UserNotReadHelper.UpdateUserNotRead(info);
                    CwAHAPI.CwapiLog("进入了edit成功");
                }



                context.Response.Write(string.Format("{{\"state\":0}}"));
            }
            catch(Exception ex)
            {
                context.Response.Write(string.Format("{{\"state\":-1,\"msg\":\"{0}\"}}", ex.Message));
            }
        }

        /// <summary>
        /// 读取我的聊天列表
        /// </summary>
        /// <param name="context"></param>
        private void loadDialogList(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                string userid = context.Request["userid"];
                //只取最近20条
                DataTable dtdialogInfo = UserDialogInfoHelper.GetUserDialogInfoData(string.Format(" where ud.FQUserId = {0} or ud.JSUserId = {0}",userid),20);
                StringBuilder builder = new StringBuilder();
                builder.Append(string.Format("{{\"state\":{0},\"Count\":{1},\"Data\":", dtdialogInfo.Rows.Count>0?0:1, dtdialogInfo.Rows.Count));
                builder.Append(JsonConvert.SerializeObject(dtdialogInfo, Formatting.Indented).TrimStart('{').TrimEnd('}'));
                builder.Append("}");
                context.Response.Write(builder.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format("{{\"state\":-1,\"msg\":\"{0}\"}}", ex.Message));
            }

        }

        /// <summary>
        /// 保存聊天记录
        /// 若单个文件超过100k，则文件名+1，以 1.data 2.data的方式连续存储，方便连续读取和避免大数据单文件出现。
        /// </summary>
        /// <param name="context"></param>
        private void saveChatMessage(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                string msgInfoStr = context.Request["messageJson"];
                //将字符串转换为json格式,再将json格式转换为datatable,之后便可以区分路径进行保存了.
                //JObject obj3 = JsonConvert.DeserializeObject(msgInfoStr) as JObject;
                DataTable dtMsgInfo = (DataTable)JsonConvert.DeserializeObject(msgInfoStr, typeof(DataTable));

                
                string webpath = "/ChatMessage/" + dtMsgInfo.Rows[0]["roomid"].ToString() + "/";
                string iopath = HttpContext.Current.Server.MapPath(webpath);
                bool isFirst = false;
                if (!Directory.Exists(iopath)) //若不存在路径，新建路径
                {
                    Directory.CreateDirectory(iopath);
                }
                int fileCount = Directory.GetFileSystemEntries(HttpContext.Current.Server.MapPath(webpath)).Length; //当前聊天文件个数
                string filename = (fileCount.ToString()=="0"?"1" : fileCount.ToString()) + ".DATA";//DateTime.Parse(dtMsgInfo.Rows[0]["sendtime"].ToString()).ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(HttpContext.Current.Server.MapPath(webpath + filename))) //不存在文件名，isFirst=true
                {
                    isFirst = true;
                }
                else
                {
                    //判断文件大小,若超过了50k,则新建文件
                    FileInfo chatInfo = new FileInfo(HttpContext.Current.Server.MapPath(webpath + filename)); //获取文件信息
                    if (chatInfo.Length / 1024 > 10) //若超过了10kb就创建新的文件
                    {
                        filename = (fileCount + 1).ToString() + ".DATA";
                    }
                }


                StreamWriter writer = File.AppendText(HttpContext.Current.Server.MapPath(webpath + filename));
                writer.WriteLine(msgInfoStr.TrimStart('[').TrimEnd(']')+",");
                writer.Flush();
                writer.Close();

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                //更新用户最近聊天列表
                string currentLocalhost = masterSettings.CurrentDomain; 
                DataTable dtdialogInfo = UserDialogInfoHelper.GetUserDialogInfoData(string.Format(" where (FQUserId = {0} and JSUserId = {1}) or (FQUserId = {1} and JSUserId = {0})  ", dtMsgInfo.Rows[0]["userid"], dtMsgInfo.Rows[0]["reciverid"]));
                //如果是初次聊天,则添加
                if (dtdialogInfo.Rows.Count <= 0)
                {
                    UserDialogInfo dialogInfo = new UserDialogInfo()
                    {
                        DialogID = Guid.NewGuid(),
                        FQUserId = dtMsgInfo.Rows[0]["userid"].ToInt(),
                        JSUserId = dtMsgInfo.Rows[0]["reciverid"].ToInt(),
                        CreateTime = DateTime.Parse(dtMsgInfo.Rows[0]["sendtime"].ToString()),
                        RoomNum = dtMsgInfo.Rows[0]["roomid"].ToString(),
                        //存储加密前的url
                        DialogURL = "&hosterid=" + dtMsgInfo.Rows[0]["userid"] + "&membersid=" + dtMsgInfo.Rows[0]["reciverid"] + "&id=" + dtMsgInfo.Rows[0]["roomid"]
                        //currentLocalhost + ":3000/userChat.html?hosterid=" + dtMsgInfo.Rows[0]["userid"] + "&membersid=" + dtMsgInfo.Rows[0]["reciverid"] + "&id=" + dtMsgInfo.Rows[0]["roomid"]
                    };
                    UserDialogInfoHelper.AddUserDialogInfo(dialogInfo);
                }
                else
                {
                    UserDialogInfo dialogInfo = new UserDialogInfo()
                    {
                        DialogID = new Guid(dtdialogInfo.Rows[0]["DialogID"].ToString()),
                        FQUserId = dtMsgInfo.Rows[0]["userid"].ToInt(),
                        JSUserId = dtMsgInfo.Rows[0]["reciverid"].ToInt(),
                        CreateTime = DateTime.Parse(dtMsgInfo.Rows[0]["sendtime"].ToString()),
                        RoomNum = dtMsgInfo.Rows[0]["roomid"].ToString(),
                        DialogURL = "&hosterid=" + dtMsgInfo.Rows[0]["userid"] + "&membersid=" + dtMsgInfo.Rows[0]["reciverid"] + "&id=" + dtMsgInfo.Rows[0]["roomid"]
                        //DialogURL = currentLocalhost + ":3000/userChat.html?hosterid=" + dtMsgInfo.Rows[0]["userid"] + "&membersid=" + dtMsgInfo.Rows[0]["reciverid"] + "&id=" + dtMsgInfo.Rows[0]["roomid"]
                    };
                    UserDialogInfoHelper.UpdateUserDialogInfo(dialogInfo);
                }
                context.Response.Write(string.Format("{{\"state\":0}}"));
            }
            catch(Exception ex)
            {
                context.Response.Write(string.Format("{{\"state\":-1,\"msg\":{0}}}",ex.Message));
            }
            
         }

        private void loadChatMessage(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                string roomid = context.Request["roomId"];
                string fileNum = context.Request["fileNum"];

                string webpath = "/ChatMessage/" + roomid + "/";
                //string filename = DateTime.Parse(msgDate).ToString("yyyy-MM-dd") + ".txt"; //DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                int fileCount;
                if (fileNum.ToInt() > 0)
                {
                    fileCount = fileNum.ToInt();
                }
                else
                {
                    fileCount = Directory.GetFileSystemEntries(HttpContext.Current.Server.MapPath(webpath)).Length; //当前聊天文件个数
                }
                string filename = (fileCount.ToString() == "0" ? "1" : fileCount.ToString()) + ".DATA";//DateTime.Parse(dtMsgInfo.Rows[0]["sendtime"].ToString()).ToString("yyyy-MM-dd") + ".txt";

                string str1 = File.ReadAllText(HttpContext.Current.Server.MapPath(webpath + filename));
                DataTable dtMsgInfo = (DataTable)JsonConvert.DeserializeObject("[" + str1.Substring(0, str1.Length - 3) + "]", typeof(DataTable));
                DataView dataView = dtMsgInfo.DefaultView;
                dataView.Sort = "sendtime asc";
                dtMsgInfo = dataView.ToTable();
                str1 = JsonConvert.SerializeObject(dtMsgInfo);
                context.Response.Write(string.Format("{{\"state\":{0},\"fileNum\":{1},\"msgInfos\":{2}}}",dtMsgInfo.Rows.Count>0?0:1,fileCount, str1));
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format("{{\"state\":-1,\"msg\":\"{0}\"}}",ex.Message));
            }

        }

        private void getUserInfoByIds(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string hosterid = context.Request["hosterid"].ToString();//发起者id
            string membersid = context.Request["membersid"].ToString();//接受者id
            if (string.IsNullOrEmpty(hosterid) || string.IsNullOrEmpty(membersid))
            {
                builder.AppendFormat("{{\"state\":2}}");
                context.Response.Write(builder.ToString());
                return;
            }
            DataTable dtHosterInfo = MemberProcessor.getUserInfoByIds(hosterid);
            DataTable dtMembersInfo = MemberProcessor.getUserInfoByIds(membersid);//接收者
            //将当前发起者和接收者的未读消息数归零
            DataTable dtUserNotRead  = UserNotReadHelper.GetUserNotReadData(string.Format(" where (FQUserId = {1} and JSUserId = {0})"  , hosterid, membersid));
            if(dtUserNotRead.Rows.Count > 0)
            {
                UserNotRead info = UserNotReadHelper.GetUserNotRead(new Guid(dtUserNotRead.Rows[0]["id"].ToString()));
                info.NotReadMsgCount = 0;
                info.UpdateTime = DateTime.Now;
                UserNotReadHelper.UpdateUserNotRead(info);
            }

            builder.Append(string.Format("{{\"Result\":\"{0}\",\"Count\":{1},\"HosterData\":", "OK", dtHosterInfo.Rows.Count+ dtMembersInfo.Rows.Count));
            builder.Append(JsonConvert.SerializeObject(dtHosterInfo, Formatting.Indented).TrimStart('{').TrimEnd('}'));//JsonConvert.SerializeObject(dt)
            builder.Append(",\"MemberData\":");
            builder.Append(JsonConvert.SerializeObject(dtMembersInfo, Formatting.Indented).TrimStart('{').TrimEnd('}'));
            builder.Append("}");
            context.Response.Write(builder.ToString());

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
