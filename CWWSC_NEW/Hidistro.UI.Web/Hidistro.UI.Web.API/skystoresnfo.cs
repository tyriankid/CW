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
    public class skystoresnfo : IHttpHandler
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
                //string json = "[{\"storename\": \"双龙旗舰店1\",\"storemanager\": \"宋璠1\",\"storemanagercell\": \"13686419951\",\"allherenumber\": \"3\",\"filialecode\": \"100008\"}]";    // 模拟传输过来的json数据
                CwAHAPI.CwapiLog("---------------------------------开始同步门店------------------------------------");
                CwAHAPI.CwapiLog("接收的数据：" + json);
                List<CwStore> listStore = CwAPI.JsonToListModel<CwStore>(json); //通过json转换的实体集合
                DataTable dtStore = StoreInfoHelper.GetAllStore();              //现有数据库表数据    门店信息
                DataTable dtFiliale = CWMembersHelper.GetAllFiliale();          //现有数据库表数据    分公司信息
                CwAHAPI.CwapiLog("现有数据库门店行数：——————" + dtStore.Rows.Count.ToString());
                CwAHAPI.CwapiLog("现有数据库分公司行数：——————" + dtFiliale.Rows.Count.ToString());
                //返回操作说明
                string jlzh = "";
                //成功执行的Code集合，以，分割，用于回调给AH
                string CODELIST = "";
                foreach (CwStore Storeinfo in listStore)
                {
                    if (!string.IsNullOrEmpty(Storeinfo.storename) && !string.IsNullOrEmpty(Storeinfo.storemanager) && !string.IsNullOrEmpty(Storeinfo.storemanagercell) && !string.IsNullOrEmpty(Storeinfo.allherenumber) && !string.IsNullOrEmpty(Storeinfo.filialecode) && !string.IsNullOrEmpty(Storeinfo.storekeyid))
                    {
                        DataRow[] drStore = dtStore.Select(string.Format("accountALLHere='{0}'", Storeinfo.allherenumber.Trim()), null, DataViewRowState.CurrentRows);
                        DataRow[] drFiliale = dtFiliale.Select(string.Format("filialecode='{0}'", Storeinfo.filialecode.Trim()), null, DataViewRowState.CurrentRows);
                        if (drStore.Length > 0)
                        {
                            if (drFiliale.Length > 0)
                            {
                                CwAHAPI.CwapiLog("已存在的分公司Code：——————" + Storeinfo.filialecode);
                                drStore[0]["storeName"] = Storeinfo.storename;
                                drStore[0]["storeRelationPerson"] = Storeinfo.storemanager;
                                drStore[0]["storeRelationCell"] = Storeinfo.storemanagercell;
                                drStore[0]["accountALLHere"] = Storeinfo.allherenumber;
                                drStore[0]["fgsid"] = drFiliale[0]["id"];
                                drStore[0]["storekeyid"] = Storeinfo.storekeyid;
                                CODELIST += Storeinfo.storekeyid + ",";
                            }
                            else
                            {
                                jlzh += "分公司Code为：" + Storeinfo.filialecode + "的数据库中不存在,";
                            }
                        }
                        else
                        {
                            if (drFiliale.Length > 0)
                            {
                                CwAHAPI.CwapiLog("不存在的分公司Code：——————" + Storeinfo.filialecode);
                                DataRow drNew = dtStore.NewRow();
                                drNew["storeName"] = Storeinfo.storename;
                                drNew["storeRelationPerson"] = Storeinfo.storemanager;
                                drNew["storeRelationCell"] = Storeinfo.storemanagercell;
                                drNew["accountALLHere"] = Storeinfo.allherenumber;
                                drNew["fgsid"] = drFiliale[0]["id"];
                                drNew["storekeyid"] = Storeinfo.storekeyid;
                                dtStore.Rows.Add(drNew);
                                CODELIST += Storeinfo.storekeyid + ",";
                            }
                            else
                            {
                                jlzh += "分公司Code为：" + Storeinfo.filialecode + "的数据库中不存在,";
                            }
                        }
                    }
                }
                CODELIST = CODELIST.TrimEnd(',');//去掉最后一个逗号
                
                //验证Code集合是否有数据
                if (string.IsNullOrEmpty(CODELIST))
                {
                    jlzh = "处理的Code集合为空，同步失败。";
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"" + jlzh + "\"}";
                    context.Response.Write(result);
                    return;
                }

                //整表提交及处理提交结果验证
                if (DataBaseHelper.CommitDataTable(dtStore, "select * from CW_StoreInfo") > 0)
                {
                    string RSPDESC = jlzh == "" ? "同步成功" : jlzh;
                    result = "{\"STATE\": 1,\"CODELIST\":\"" + CODELIST + "\",\"RSPDESC\":\"" + RSPDESC + "\"}";
                    CwAHAPI.CwapiLog("返回内容：——————" + result);
                }
                else
                {
                    jlzh = "提交数据库出现错误，同步失败。";
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\"" + jlzh + "\"}";
                    CwAHAPI.CwapiLog("返回内容：——————" + result);
                }
                CwAHAPI.CwapiLog("---------------------------------同步门店END------------------------------------");
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
