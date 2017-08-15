using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    public class RzSelectStoreSales : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 执行提交的请求
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/text";
            string DsName = context.Request["DsName"].Trim();
            string DsPhone = context.Request["DsPhone"].Trim();
            string DsType = context.Request["DsType"].Trim();
            string storeId = context.Request["StoreID"];

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            DistributorSales salesinfo = new DistributorSales();
            #region 2017-7-31去掉Allhere账号认证，根据传送过来的门店StoreID查询门店账号 yk
            if (!string.IsNullOrEmpty(storeId))
            {
                StoreInfo info = StoreInfoHelper.GetStoreInfoByUserId(int.Parse(storeId));
                if (info != null)
                {
                    salesinfo.accountALLHere = info.accountALLHere;
                }
            }
            else
            {
                context.Response.Write("{\"Status\":Inexistence\"}");
                return;
            }
            #endregion
            salesinfo.DsName = DsName;
            salesinfo.DsPhone = DsPhone;
            salesinfo.DsType = int.Parse(DsType);
            ///验证是否存在
            DistributorSales resultSales = DistributorSalesHelper.RzSales(salesinfo);
            if (resultSales != null && resultSales.DsID != Guid.Empty)
            {
                if (resultSales.IsRz != 1)
                {
                    //认证修改店员数据为已认证
                    resultSales.SaleUserId = MemberProcessor.GetCurrentMember().UserId;
                    resultSales.IsRz = 1;
                    resultSales.RzTime = DateTime.Now;
                    if (DistributorSalesHelper.RzSuccessToUpdate(resultSales))
                    {
                        builder.Append("\"Status\":\"Ok\",\"StoreName\":\"" + resultSales.StoreName + "\"");//认证成功
                    }
                    else
                        builder.Append("\"Status\":\"Fail\"");//认证失败
                }
                else
                    builder.Append("\"Status\":\"Already\"");//已经认证过
            }
            else
            {
                builder.Append("\"Status\":\"Inexistence\"");//认证信息不正确
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }
    }
}
