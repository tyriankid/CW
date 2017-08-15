namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VLookVirtualCode : VWeiXinOAuthTemplatedWebControl
    {
        private Literal Literal1;
        private Literal Literal2;
        private Literal Literal3;
        private Literal Literal4;
        private Literal Literal5;
        private Literal Literal6;
        private Literal Literal7;
        private Literal Literal8;
        private Literal Literal9;

        protected override void AttachChildControls()
        {
            string strorderid = this.Page.Request.QueryString["OrderId"];
            if (string.IsNullOrEmpty(strorderid))
            {
                base.GotoResourceNotFound("");
            }

            //成功分配查看
            this.Literal1 = (Literal)this.FindControl("Literal1");
            this.Literal2 = (Literal)this.FindControl("Literal2");

            this.Literal1.SetWhenIsNotNull("订单编码：" + strorderid);
            //得到虚拟订单虚拟码信息
            DataTable dtVirtualCode = OrderVirtualInfoHelper.SelectOvAndPvByOrderId(strorderid);
            if (dtVirtualCode.Rows.Count > 0)
            {
                string strcodes = "";
                for (int i = 0; i < dtVirtualCode.Rows.Count; i++)
                {
                    strcodes += "卡号" + (i + 1) + "：" + dtVirtualCode.Rows[0]["VirtualCode"].ToString() + "<br />";
                }
                this.Literal2.SetWhenIsNotNull(strcodes);
            }
            else
                base.GotoResourceNotFound("");
            
            PageTitle.AddSiteNameTitle("虚拟订单虚拟码");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vLookVirtualCode.html";
            }
            base.OnInit(e);
        }
    }
}


