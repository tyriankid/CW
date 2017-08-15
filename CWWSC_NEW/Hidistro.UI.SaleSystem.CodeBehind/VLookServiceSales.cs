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
    public class VLookServiceSales : VWeiXinOAuthTemplatedWebControl
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
            DataTable dtsales = OrderSalesHelper.SelectServiceOrderSales(strorderid);
            if (dtsales.Rows.Count > 0)
            {
                //成功分配查看
                this.Literal1 = (Literal)this.FindControl("Literal1");
                this.Literal2 = (Literal)this.FindControl("Literal2");
                this.Literal3 = (Literal)this.FindControl("Literal3");
                this.Literal4 = (Literal)this.FindControl("Literal4");
                this.Literal5 = (Literal)this.FindControl("Literal5");
                this.Literal6 = (Literal)this.FindControl("Literal6");
                this.Literal7 = (Literal)this.FindControl("Literal7");
                this.Literal8 = (Literal)this.FindControl("Literal8");
                this.Literal9 = (Literal)this.FindControl("Literal9");

                if (dtsales.Rows[0]["State"].ToString() == "1")
                {
                    this.Literal1.SetWhenIsNotNull("订单编号：" + dtsales.Rows[0]["OrderId"].ToString());
                    this.Literal2.SetWhenIsNotNull("服务人员姓名：" + dtsales.Rows[0]["DsName"].ToString());
                    this.Literal3.SetWhenIsNotNull("服务人员电话：" + dtsales.Rows[0]["DsPhone"].ToString());
                    this.Literal4.SetWhenIsNotNull("接单时间：" + dtsales.Rows[0]["CreateDate"].ToString());
                    this.Literal5.SetWhenIsNotNull("服务门店：" + dtsales.Rows[0]["StoreName"].ToString());
                    this.Literal6.SetWhenIsNotNull("服务门店位置：" + dtsales.Rows[0]["Location_poiname"].ToString());
                    this.Literal7.SetWhenIsNotNull("服务门店城市：" + dtsales.Rows[0]["Location_cityname"].ToString());
                    this.Literal8.SetWhenIsNotNull("服务门店详细地址：" + dtsales.Rows[0]["Location_poiaddress"].ToString());
                    this.Literal9.SetWhenIsNotNull("核销码：" + dtsales.Rows[0]["serviceCode"].ToString());

                }
                if (dtsales.Rows[0]["State"].ToString() == "0")
                { 
                    //拒绝分配查看
                    this.Literal1.SetWhenIsNotNull("订单编号：" + dtsales.Rows[0]["OrderId"].ToString());
                    this.Literal2.SetWhenIsNotNull("拒绝时间：" + dtsales.Rows[0]["CreateDate"].ToString());
                    this.Literal3.SetWhenIsNotNull("拒绝理由：" + dtsales.Rows[0]["RefuseRemark"].ToString());
                    this.Literal4.SetWhenIsNotNull("服务门店：" + dtsales.Rows[0]["StoreName"].ToString());
                    this.Literal5.SetWhenIsNotNull("服务门店位置：" + dtsales.Rows[0]["Location_poiname"].ToString());
                    this.Literal6.SetWhenIsNotNull("服务门店城市：" + dtsales.Rows[0]["Location_cityname"].ToString());
                    this.Literal7.SetWhenIsNotNull("服务门店详细地址：" + dtsales.Rows[0]["Location_poiaddress"].ToString());
                }
            }
            else
                base.GotoResourceNotFound("");
            
            PageTitle.AddSiteNameTitle("服务订单配单结果");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vLookServiceSales.html";
            }
            base.OnInit(e);
        }
    }
}


