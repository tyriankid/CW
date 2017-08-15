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
    public class skysupplierinfo : IHttpHandler
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
                //string json = "[{\"suppliercode\": 100019,\"suppliername\": \"\",\"supplierphone\": \"111111111111\",\"supplieraddress\":\"湖北省\"},{\"suppliercode\": 100020,\"suppliername\": \"测试门店B\",\"supplierphone\": \"22222222222\",\"supplieraddress\": \"北京市\"},{\"suppliercode\": 100021,\"suppliername\": \"测试门店C\",\"supplierphone\": \"33333333333\",\"supplieraddress\": \"上海市\"},{\"suppliercode\": 100022,\"suppliername\": \"测试门店D\",\"supplierphone\": \"44444444444\",\"supplieraddress\": \"深圳\"}]";    // 模拟传输过来的json数据
                CwAHAPI.CwapiLog("---------------------------------开始同步供应商------------------------------------");
                CwAHAPI.CwapiLog("接收数据：" + json);
                List<CwSupplier> listsupplier = CwAPI.JsonToListModel<CwSupplier>(json);//通过json转换的实体集合
                DataTable dt = SupplierHelper.GetAllSupplier();//现有数据库表数据
                CwAHAPI.CwapiLog("现有数据库行数：——————" + dt.Rows.Count.ToString());
                //返回操作说明
                string RSPDESC = "";
                //成功执行的Code集合，以，分割，用于回调给AH
                string CODELIST = "";
                foreach (CwSupplier supplierinfo in listsupplier)
                {
                    string code = supplierinfo.suppliercode;
                    //查找要同步的数据里面的Code是否存在我们数据库里面，如果存在，则更新，不存在，新增
                    DataRow[] dr = dt.Select(string.Format("gyscode='{0}'" , supplierinfo.suppliercode), null, DataViewRowState.CurrentRows);
                    if (!string.IsNullOrEmpty(supplierinfo.suppliername) && !string.IsNullOrEmpty(supplierinfo.suppliercode))
                    {
                        if (dr.Length > 0)
                        {
                            CwAHAPI.CwapiLog("已存在的供应商Code：——————" + supplierinfo.suppliercode);
                            dr[0]["gysName"] = supplierinfo.suppliername;
                            if (!string.IsNullOrEmpty(supplierinfo.supplierphone))
                            {
                                dr[0]["gysPhone"] = supplierinfo.supplierphone;
                            }
                            if (!string.IsNullOrEmpty(supplierinfo.supplieraddress))
                            {
                                dr[0]["gysAddress"] = supplierinfo.supplieraddress;
                            }
                        }
                        else
                        {
                            CwAHAPI.CwapiLog("不存在的供应商Code：——————" + supplierinfo.suppliercode);
                            DataRow drNew = dt.NewRow();
                            drNew["gysName"] = supplierinfo.suppliername;
                            drNew["gysPhone"] = supplierinfo.supplierphone;
                            drNew["gysAddress"] = supplierinfo.supplieraddress;
                            drNew["gysCode"] = supplierinfo.suppliercode;
                            dt.Rows.Add(drNew);
                        }
                        CODELIST += supplierinfo.suppliercode + ",";
                    }
                    else
                    {
                        RSPDESC += "供应商Code为：" + supplierinfo.suppliercode + "的必填项有空值！，";
                    }
                }
                CODELIST = CODELIST.TrimEnd(',');//去掉最后一个逗号

                //验证Code集合是否有数据
                if (string.IsNullOrEmpty(CODELIST))
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\" 处理的Code集合为空，同步失败。\"}";
                    CwAHAPI.CwapiLog("返回内容：" + result);
                    context.Response.Write(result);
                    return;
                }

                //整表提交及处理提交结果验证
                if (DataBaseHelper.CommitDataTable(dt, "select * from CW_Supplier") > -1)
                {
                    string RSPDESCS = RSPDESC == "" ? "同步成功。" : RSPDESC;
                    result = "{\"STATE\": " + 1 + ",\"CODELIST\":\"" + CODELIST + "\",\"RSPDESC\":\"" + RSPDESCS + "\"}";
                    CwAHAPI.CwapiLog("返回内容：" + result);
                }
                else
                {
                    result = "{\"STATE\": 0,\"CODELIST\":\"\",\"RSPDESC\":\" 提交数据库出现错误，同步失败。\"}";
                    CwAHAPI.CwapiLog("返回内容：" + result);
                }
                CwAHAPI.CwapiLog("---------------------------------同步供应商END------------------------------------");
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
