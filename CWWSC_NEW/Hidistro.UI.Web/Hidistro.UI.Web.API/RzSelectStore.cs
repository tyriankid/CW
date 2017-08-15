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
    public class RzSelectStore : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/text";
            int fgsid = Convert.ToInt32(context.Request["fgsId"]);
            string MdName = context.Request["MdName"].Trim();
            string JlNum = context.Request["JlNum"].Trim();
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("{");
            StoreInfo storequery = new Entities.Commodities.StoreInfo
            {
                fgsid = fgsid,
                storeRelationPerson = MdName,
                accountALLHere = JlNum
            };
            DataTable dt = StoreInfoHelper.RzStoreInfo(storequery).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["storekeyid"].ToString()))
                {
                    if (VShopHelper.SelectDistributorsByStoreId(Convert.ToInt32(dt.Rows[0]["id"])).Rows.Count > 0)
                    {
                        builder.Append("\"Status\":\"YRZ\"");
                    }
                    else
                    {
                        int auditing = 0;
                        if (int.TryParse(dt.Rows[0]["Auditing"].ToString(), out auditing))
                        {
                            if (auditing == 1)
                            {
                                if (ManagerHelper.GetManager(JlNum) != null)
                                    builder.Append("\"Status\":\"ManagerError\"");
                                else
                                    builder.AppendFormat("\"Status\":\"OK\",\"KeyId\":\"{0}\"", dt.Rows[0]["id"].ToString());
                            }
                            else
                                builder.Append("\"Status\":\"AuditingError\"");
                        }
                        else
                            builder.Append("\"Status\":\"AuditingError\"");
                    }
                }
                else
                {
                    builder.Append("\"Status\":\"KeyError\"");
                }
            }
            else
            {
                builder.Append("\"Status\":\"Error\"");
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }
    }
}
