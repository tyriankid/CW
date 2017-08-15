using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_distributor_SetdistributorClass :AdminPage
{
    protected string DcID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DcID = this.Page.Request.QueryString["DcID"];
    }
    protected void btnPass_Click(object sender, EventArgs e)
    {
        DistributorClass info = DistributorClassHelper.GetDistributorClassByDcID(new Guid(DcID));
        if (info != null)
        {
            info.State = 1;
            info.AuditDate = DateTime.Now;
            if (DistributorClassHelper.UpdateDistributorClassEx(info))
            {
                this.ShowMsg("审核成功", true);
            }
        }
    }
    protected void btnNoPass_Click(object sender, EventArgs e)
    {
         DistributorClass info = DistributorClassHelper.GetDistributorClassByDcID(new Guid(DcID));
        if (info != null)
        {
            info.State =2;
            info.AuditDate = DateTime.Now;
            info.AuditRemark=this.txtAccount.Text;
            if (DistributorClassHelper.UpdateDistributorClass(info))
            {
                this.ShowMsg("审核成功", true);  
            }
        }
    }
}