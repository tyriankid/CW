using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    /*
     活动相关的无刷新操作
     */
    public class ServiceClassHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private IDictionary<string, string> jsondict = new Dictionary<string, string>();

        
        public void ProcessRequest(System.Web.HttpContext context)
        {
            string text = context.Request["action"];
            switch (text)
            {
                case "ApplyServiceClass"://服务门店申请
                    this.ApplyServiceClass(context);
                    break;
                case "DelApply"://删除申请
                    this.DelApply(context);
                    break;
                case "ApplyStore": //成为店长申请
                    this.ApplyStore(context);
                    break;
            }
        }

        private void ApplyStore(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string applyUserId = context.Request["applyUserId"];
            string applyStoreId = context.Request["applyStoreId"];
            DataTable dtApply = StoreUserChangeApplyHelper.GetStoreUserChangeApplyData(string.Format(" ApplyUserId = {0} and StoreUserId={1}", applyUserId, applyStoreId));
            StoreUserChangeApply applyinfo = new StoreUserChangeApply();
            if (dtApply.Rows.Count > 0 && dtApply.Rows[0]["ApplyState"].ToString() != "0") //如果已经有记录并且为已经过审核,则更新
            {
                applyinfo = StoreUserChangeApplyHelper.GetStoreUserChangeApply(new Guid(dtApply.Rows[0]["ID"].ToString()));
                applyinfo.ApplyState = 0;
                applyinfo.AuditingDate = DateTime.Now;
                applyinfo.Reason = "";
                if (StoreUserChangeApplyHelper.UpdateStoreUserChangeApply(applyinfo))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"提交申请成功。\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"提交申请失败。\"}");
                }
                return;
            }
            else if(dtApply.Rows.Count > 0 && dtApply.Rows[0]["ApplyState"].ToString() == "0") //如果已经有记录并且还未审核,则弹重复提交错误提示
            {
                context.Response.Write("{\"success\":false,\"msg\":\"请勿重复提交申请。\"}");
                return;
            }
            else //新增申请
            {
                applyinfo.ID = Guid.NewGuid();
                applyinfo.ApplyUserId = applyUserId.ToInt();
                applyinfo.StoreUserId = applyStoreId.ToInt();
                applyinfo.ApplyState = 0;
                applyinfo.AuditingDate = DateTime.Now;
                applyinfo.Reason = "";
                if (StoreUserChangeApplyHelper.AddStoreUserChangeApply(applyinfo))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"提交申请成功。\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"提交申请失败。\"}");
                }
                return;
            }
        }

        /// <summary>
        /// 进行签到操作
        /// </summary>
        /// <param name="context"></param>
        private void ApplyServiceClass(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string dcid = context.Request["DcID"];
            string ids = context.Request["IDS"];
            string region = context.Request["Region"];
            //ids = ids.TrimEnd(',');//去除尾，分隔符
            if (!string.IsNullOrEmpty(ids))
            {
                if (string.IsNullOrEmpty(dcid))
                {
                    //新增
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    DistributorClass disclass = new DistributorClass();
                    disclass.DcID = Guid.NewGuid();
                    disclass.DisUserId = currentMember.UserId;
                    disclass.State = 0;
                    disclass.ApplyDate = DateTime.Now;
                    disclass.ScIDs = ids;
                    if (!string.IsNullOrEmpty(region))
                    {
                        disclass.RegionId = Convert.ToInt32(region);
                        disclass.RegionName = RegionHelper.GetFullRegion(disclass.RegionId, "，");
                    }
                    if(DistributorClassHelper.AddDistributorClass(disclass))
                        context.Response.Write("{\"success\":true,\"msg\":\"服务门店申请成功。\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"提交数据库时发生错误。\"}");
                }
                else
                {
                    //编辑
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    DistributorClass disclass = DistributorClassHelper.GetDistributorClassByDcID(new Guid(dcid));
                    if (disclass.State != 1)
                    {
                        disclass.State = 0;
                        disclass.ApplyDate = DateTime.Now;
                        disclass.ScIDs = ids;
                        if (!string.IsNullOrEmpty(region))
                        {
                            disclass.RegionId = Convert.ToInt32(region);
                            disclass.RegionName = RegionHelper.GetFullRegion(disclass.RegionId, "，");
                        }
                        if (DistributorClassHelper.UpdateDistributorClass(disclass))
                            context.Response.Write("{\"success\":true,\"msg\":\"服务门店申请成功。\"}");
                        else
                            context.Response.Write("{\"success\":false,\"msg\":\"保存时发生错误。\"}");
                    }
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"申请已经审核通过无法修改。\"}");
                }
            }
            else
                context.Response.Write("{\"success\":false,\"msg\":\"页面请求的参数错误。\"}");
        }


        /// <summary>
        /// 进行签到操作
        /// </summary>
        /// <param name="context"></param>
        private void DelApply(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Guid dcid = Guid.Empty;
            string strdcid = context.Request["DcID"];
            Guid.TryParse(strdcid, out dcid);
            if (dcid != Guid.Empty)
            {
                DistributorClass disclass = DistributorClassHelper.GetDistributorClassByDcID(dcid);
                if (disclass.State != 1)
                {
                    if (DistributorClassHelper.DeleteDistributorClass(dcid))
                        context.Response.Write("{\"success\":true,\"msg\":\"删除成功。\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"删除时发生错误。\"}");
                }
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"申请已经审核通过无法删除。\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"请求的参数错误。\"}");
            }
        }
        

    }
}
