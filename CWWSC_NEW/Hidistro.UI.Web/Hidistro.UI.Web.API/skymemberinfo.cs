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
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WxPayAPI;

namespace Hidistro.UI.Web.API
{

    public class skymemberinfo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string result = "";
                if (context.Request["json"] == null || string.IsNullOrEmpty(context.Request["json"].ToString()))
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"参数错误，同步失败。\"}";
                    context.Response.Write(result);
                    return;
                }

                string json = context.Request["json"];//接收的json格式的字符串
                CwAHAPI.CwapiLog("---------------------------------开始同步会员------------------------------------");
                //string json = "[{\"usercode\": 100001,\"username\": \"\",\"cellphone\": \"13479878951\",\"dzcode\": \"DZ0000001\",\"storename\": \"测试门店A\",\"productcode\": \"11112233\",\"productmodel\": \"222222222\",\"address\": \"湖北省武汉市硚口区\"},{\"usercode\": 100002,\"username\": \"测试用户B\",\"cellphone\": \"13479878952\",\"dzcode\": \"DZ0000002\",\"storename\": \"测试门店B\",\"productcode\": \"11112233\",\"productmodel\":\"33333333\",\"address\": \"湖北省武汉市硚口区\"},{\"usercode\": 100003,\"username\":\"测试用户C\",\"cellphone\": \"13479878953\",\"dzcode\": \"DZ0000003\",\"storename\": \"测试门店C\",\"productcode\": \"11112233\",\"productmodel\": \"444444444\",\"address\": \"湖北省武汉市硚口区\"}]";    // 模拟传输过来的json数据
                List<CwMember> listmember = CwAPI.JsonToListModel<CwMember>(json);//通过json转换的实体集合
                CwAHAPI.CwapiLog("——————接收的数据：" + json);
                DataTable dt = CWMembersHelper.GetAllMember();//现有数据库表数据
                //返回操作说明
                string RSPDESC = "";
                //成功执行的Code集合，以，分割，用于回调给AH
                string CODELIST = ",";
                CwAHAPI.CwapiLog("——————现有数据库行数：" + dt.Rows.Count.ToString());
                foreach (CwMember memberinfo in listmember)
                {
                    //如果同步编码已经存在
                    if (CODELIST.IndexOf("," + memberinfo.usercode + ",", 0) >= 0)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(memberinfo.username) && !string.IsNullOrEmpty(memberinfo.cellphone) && !string.IsNullOrEmpty(memberinfo.usercode))
                    {
                        CwAHAPI.CwapiLog("——————当前Code为：" + memberinfo.usercode);
                        //查找要同步的数据里面的Code是否存在我们数据库里面，如果存在，则更新，不存在，新增
                        DataRow[] drs = dt.Select(string.Format("UserCode='{0}'", memberinfo.usercode), null, DataViewRowState.CurrentRows);
                        if (drs.Length > 0)
                        {
                            CwAHAPI.CwapiLog("——————原始数据库中是否存在：是");
                            
                            drs[0]["UserName"] = memberinfo.username;
                            drs[0]["CellPhone"] = memberinfo.cellphone;
                            if (!string.IsNullOrEmpty(memberinfo.dzcode))
                            {
                                drs[0]["accountALLHere"] = memberinfo.dzcode;
                            }
                            if (!string.IsNullOrEmpty(memberinfo.storename))
                            {
                                drs[0]["StoreName"] = memberinfo.storename;
                            }
                            if (!string.IsNullOrEmpty(memberinfo.address))
                            {
                                drs[0]["Address"] = memberinfo.address;
                            } 
                            //if (!string.IsNullOrEmpty(memberinfo.productcode))
                            //{
                            //    drs[0]["ProductCode"] = memberinfo.productcode;
                            //} 
                            //if (!string.IsNullOrEmpty(memberinfo.productmodel))
                            //{
                            //    drs[0]["ProductModel"] = memberinfo.productmodel;
                            //} 
                        }
                        else
                        {
                            CwAHAPI.CwapiLog("——————原始数据库中是否存在：否");

                            DataRow drNew = dt.NewRow();
                            drNew["UserName"] = memberinfo.username;
                            drNew["CellPhone"] = memberinfo.cellphone;
                            drNew["accountALLHere"] = memberinfo.dzcode;
                            drNew["StoreName"] = memberinfo.storename;
                            drNew["Address"] = memberinfo.address;
                            //drNew["ProductCode"] = memberinfo.productcode;
                            //drNew["ProductModel"]  = memberinfo.productmodel;
                            drNew["UserCode"] = memberinfo.usercode;
                            dt.Rows.Add(drNew);
                        }
                        if (CODELIST.IndexOf("," + memberinfo.usercode + ",", 0) == -1)
                            CODELIST += memberinfo.usercode + ",";//拼接同步成功的用户Code
                    }
                    else
                    {

                        RSPDESC += "用户Code为：" + memberinfo.usercode + "的必填项有空值！，";
                    }
                }
                CODELIST = CODELIST.TrimStart(',');
                CODELIST = CODELIST.TrimEnd(',');//去掉最后一个逗号

                //处理的Code集合为空
                if (string.IsNullOrEmpty(CODELIST))
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"处理的Code集合为空，同步失败。\"}";
                    context.Response.Write(result);
                    return;
                }

                CwAHAPI.CwapiLog("——————开始提交数据库，总行数：" + dt.Rows.Count);

                //整表提交及处理提交结果验证
                if (DataBaseHelper.CommitDataTable(dt, "select * from CW_Members") > 0)
                {
                    string RSPDESCS = RSPDESC == "" ? "同步成功" : RSPDESC;
                    CwAHAPI.CwapiLog("——————提交成功——————");
                    result = "{\"STATE\": " + 1 + ",\"CODELIST\":\"" + CODELIST + "\",\"RSPDESC\":\"" + RSPDESCS + "\"}";
                    CwAHAPI.CwapiLog("——————返回数据：" + result);
                }
                else
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"提交数据库出现错误，同步失败。\"}";
                    CwAHAPI.CwapiLog("——————返回数据：" + result);
                }
                CwAHAPI.CwapiLog("——————结束同步会员END——————");
                context.Response.Write(result);
                return;

            }
            catch (Exception ex)
            {
                CwAHAPI.CwapiLog("——————执行发生错误：" + ex.ToString());
                CwAHAPI.CwapiLog("——————结束同步会员END——————");
                string resultTry = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"内部处理错误，同步失败。\"}";
                context.Response.Write(resultTry);
                return;
            }
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
