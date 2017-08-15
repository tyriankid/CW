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
     ���ص���ˢ�²���
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
                case "ApplyServiceClass"://�����ŵ�����
                    this.ApplyServiceClass(context);
                    break;
                case "DelApply"://ɾ������
                    this.DelApply(context);
                    break;
                case "ApplyStore": //��Ϊ�곤����
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
            if (dtApply.Rows.Count > 0 && dtApply.Rows[0]["ApplyState"].ToString() != "0") //����Ѿ��м�¼����Ϊ�Ѿ������,�����
            {
                applyinfo = StoreUserChangeApplyHelper.GetStoreUserChangeApply(new Guid(dtApply.Rows[0]["ID"].ToString()));
                applyinfo.ApplyState = 0;
                applyinfo.AuditingDate = DateTime.Now;
                applyinfo.Reason = "";
                if (StoreUserChangeApplyHelper.UpdateStoreUserChangeApply(applyinfo))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"�ύ����ɹ���\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"�ύ����ʧ�ܡ�\"}");
                }
                return;
            }
            else if(dtApply.Rows.Count > 0 && dtApply.Rows[0]["ApplyState"].ToString() == "0") //����Ѿ��м�¼���һ�δ���,���ظ��ύ������ʾ
            {
                context.Response.Write("{\"success\":false,\"msg\":\"�����ظ��ύ���롣\"}");
                return;
            }
            else //��������
            {
                applyinfo.ID = Guid.NewGuid();
                applyinfo.ApplyUserId = applyUserId.ToInt();
                applyinfo.StoreUserId = applyStoreId.ToInt();
                applyinfo.ApplyState = 0;
                applyinfo.AuditingDate = DateTime.Now;
                applyinfo.Reason = "";
                if (StoreUserChangeApplyHelper.AddStoreUserChangeApply(applyinfo))
                {
                    context.Response.Write("{\"success\":true,\"msg\":\"�ύ����ɹ���\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":false,\"msg\":\"�ύ����ʧ�ܡ�\"}");
                }
                return;
            }
        }

        /// <summary>
        /// ����ǩ������
        /// </summary>
        /// <param name="context"></param>
        private void ApplyServiceClass(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string dcid = context.Request["DcID"];
            string ids = context.Request["IDS"];
            string region = context.Request["Region"];
            //ids = ids.TrimEnd(',');//ȥ��β���ָ���
            if (!string.IsNullOrEmpty(ids))
            {
                if (string.IsNullOrEmpty(dcid))
                {
                    //����
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
                        disclass.RegionName = RegionHelper.GetFullRegion(disclass.RegionId, "��");
                    }
                    if(DistributorClassHelper.AddDistributorClass(disclass))
                        context.Response.Write("{\"success\":true,\"msg\":\"�����ŵ�����ɹ���\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"�ύ���ݿ�ʱ��������\"}");
                }
                else
                {
                    //�༭
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
                            disclass.RegionName = RegionHelper.GetFullRegion(disclass.RegionId, "��");
                        }
                        if (DistributorClassHelper.UpdateDistributorClass(disclass))
                            context.Response.Write("{\"success\":true,\"msg\":\"�����ŵ�����ɹ���\"}");
                        else
                            context.Response.Write("{\"success\":false,\"msg\":\"����ʱ��������\"}");
                    }
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"�����Ѿ����ͨ���޷��޸ġ�\"}");
                }
            }
            else
                context.Response.Write("{\"success\":false,\"msg\":\"ҳ������Ĳ�������\"}");
        }


        /// <summary>
        /// ����ǩ������
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
                        context.Response.Write("{\"success\":true,\"msg\":\"ɾ���ɹ���\"}");
                    else
                        context.Response.Write("{\"success\":false,\"msg\":\"ɾ��ʱ��������\"}");
                }
                else
                    context.Response.Write("{\"success\":false,\"msg\":\"�����Ѿ����ͨ���޷�ɾ����\"}");
            }
            else
            {
                context.Response.Write("{\"success\":false,\"msg\":\"����Ĳ�������\"}");
            }
        }
        

    }
}
