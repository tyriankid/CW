using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CeShiApi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 提交商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        ProductInfo pi = ProductHelper.GetProductBaseInfo(114);
        string message="";
        int spnm;
        CwAHAPI.SendProductToAH(pi, "123",out  message,out spnm);
    }
    /// <summary>
    /// 发送订单到AH
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string message = "";
        string orderid = "";
        OrderInfo oi = OrderHelper.GetOrderInfo("201609058992910");
        CwAHAPI.SendOrderoAH(oi, "1000000000000000001", out message, out orderid);
    }
    /// <summary>
    /// 订单发货接口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button3_Click(object sender, EventArgs e)
    {
        //string message = "";
        //OrderInfo oi = OrderHelper.GetOrderInfo("201609058992910");
        //CwAHAPI.SendOrderDeliveryToAH(oi, out "1000000000000000001", out message);
    }
    /// <summary>
    ///  订单完成接口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button4_Click(object sender, EventArgs e)
    {
        string message = "";
        OrderInfo oi = OrderHelper.GetOrderInfo("201609058992910");
        CwAHAPI.SendOrderAchieveToAH(oi, out message);
    }
    /// <summary>
    /// 订单退货接口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button5_Click(object sender, EventArgs e)
    {
        string message = "";
        OrderInfo oi = OrderHelper.GetOrderInfo("201609058992910");
        string time = DateTime.Now.ToString();
        CwAHAPI.SendOrderoReturnAH(oi, time, out message);
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string message = "";
        OrderInfo oi = OrderHelper.GetOrderInfo("201609058992910");
        CwAHAPI.SendReceiptAH(oi,  out message);
    }
}