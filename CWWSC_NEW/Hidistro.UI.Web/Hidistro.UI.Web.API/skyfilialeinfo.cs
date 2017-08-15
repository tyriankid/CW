using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.Entities.CWAPI;
using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
    public class skyfilialeinfo : IHttpHandler
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
                //string json = "[{\"filialecode\": 100007,\"filialename\": \"zswceshi2\",\"filialephone\": \"1025\",\"filialeaddress\": \"wuhan1\"},{\"filialecode\": 100008,\"filialename\": \"zsw测试2\",\"filialephone\": \"1025\",\"filialeaddress\": \"wh1\"}]";    // 模拟传输过来的json数据
                List<CwFiliale> listFiliale = CwAPI.JsonToListModel<CwFiliale>(json);//通过json转换的实体集合
                CwAHAPI.CwapiLog("---------------------------------同步分公司------------------------------------");
                CwAHAPI.CwapiLog("接收的数据：——————" + json); 
                DataTable dt = FilialeHelper.GetAllFiliale().Tables[0];//现有数据库表数据
                CwAHAPI.CwapiLog("现有数据库行数：——————" + dt.Rows.Count.ToString()); 
                //返回操作说明
                string RSPDESC = "";
                //成功执行的Code集合，以,分割，用于回调给AH
                string CODELIST = "";
                foreach (CwFiliale filialeinfo in listFiliale)
                {
                    string code = filialeinfo.filialecode;
                    DataRow[] drCode = dt.Select(string.Format("filialecode='{0}'", filialeinfo.filialecode), null, DataViewRowState.CurrentRows);
                    DataRow[] drName = dt.Select(string.Format("fgsName='{0}'", filialeinfo.filialename), null, DataViewRowState.CurrentRows);
                    if (!string.IsNullOrEmpty(filialeinfo.filialename) && !string.IsNullOrEmpty(filialeinfo.filialecode))
                    {
                        if (drCode.Length > 0)
                        {
                            CwAHAPI.CwapiLog("已存在的分公司Code：——————" + filialeinfo.filialecode); 
                            drCode[0]["fgsName"] = filialeinfo.filialename;
                            if (!string.IsNullOrEmpty(filialeinfo.filialephone))
                            {
                                drCode[0]["fgsPhone"] = filialeinfo.filialephone;
                            }
                            if (!string.IsNullOrEmpty(filialeinfo.filialeaddress))
                            {
                                drCode[0]["fgsAddress"] = filialeinfo.filialeaddress;
                            }
                        }
                        else
                        {
                            if (drName.Length > 0)
                            {
                                CwAHAPI.CwapiLog("已存在的分公司名称：——————" + filialeinfo.filialename); 
                                drName[0]["filialecode"] = filialeinfo.filialecode;
                                if (!string.IsNullOrEmpty(filialeinfo.filialephone))
                                {
                                    drName[0]["fgsPhone"] = filialeinfo.filialephone;
                                }
                                if (!string.IsNullOrEmpty(filialeinfo.filialeaddress))
                                {
                                    drName[0]["fgsAddress"] = filialeinfo.filialeaddress;
                                }
                            }
                            else
                            {
                                CwAHAPI.CwapiLog("不存在的分公司Code：——————" + filialeinfo.filialecode); 
                                DataRow drNew = dt.NewRow();
                                drNew["fgsName"] = filialeinfo.filialename;
                                drNew["fgsPhone"] = filialeinfo.filialephone;
                                drNew["fgsAddress"] = filialeinfo.filialeaddress;
                                drNew["filialecode"] = filialeinfo.filialecode;
                                dt.Rows.Add(drNew);
                            }
                        }
                        CODELIST += filialeinfo.filialecode + ",";//拼接同步成功的分公司Code
                    }
                    else
                    {
                        RSPDESC += "分公司Code为：" + filialeinfo.filialecode + "的必填项有空值！，";
                    }
                }

                CODELIST = CODELIST.TrimEnd(',');//去掉最后一个逗号



                //验证Code集合是否有数据
                if (string.IsNullOrEmpty(CODELIST))
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"处理的Code集合为空，同步失败。\"}";
                    CwAHAPI.CwapiLog("**************返回内容：——————" + result);  
                    context.Response.Write(result);
                    return;
                }

                //整表提交及处理提交结果验证
                if (DataBaseHelper.CommitDataTable(dt, "select * from CW_Filiale") != -1)
                {

                    string RSPDESCS = RSPDESC == "" ? "同步成功" : RSPDESC;
                    CwAHAPI.CwapiLog("**************返回说明：——————" + RSPDESCS);
                    result = "{\"STATE\": " + 1 + ",\"CODELIST\":\"" + CODELIST + "\",\"RSPDESC\":\"" + RSPDESCS + "\"}";
                    CwAHAPI.CwapiLog("**************返回内容：——————" + result);  
                }
                else
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"提交数据库出现错误，同步失败。\"}";
                    CwAHAPI.CwapiLog("**************返回内容：——————" + result);  
                }
                CwAHAPI.CwapiLog("-----------------------------同步分公司END---------------------------------------");  
                context.Response.Write(result);
                return;

            }
            catch
            {
                string result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"内部处理错误，同步失败。\"}";
                context.Response.Write(result);
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
