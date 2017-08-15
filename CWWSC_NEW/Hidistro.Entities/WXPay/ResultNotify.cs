﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public ResultNotify(Page page):base(page)
        {
        }

        public override string[] ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            
            string[] strValues = new string[2];

            
            string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            //string total_fee = notifyData.GetValue("total_fee").ToString();

            strValues[0] = out_trade_no;
            strValues[1] = transaction_id;
            //strValues[2] = total_fee;
            return strValues;

            ////查询订单，判断订单真实性
            //if (!QueryOrder(transaction_id))
            //{
            //    //若订单查询失败，则立即返回结果给微信支付后台
            //    WxPayData res = new WxPayData();
            //    res.SetValue("return_code", "FAIL");
            //    res.SetValue("return_msg", "订单查询失败");
            //    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
            //    page.Response.Write(res.ToXml());
            //    page.Response.End();
            //}
            ////查询订单成功
            //else
            //{
            //    WxPayData res = new WxPayData();
            //    res.SetValue("return_code", "SUCCESS");
            //    res.SetValue("return_msg", "OK");
            //    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
            //    page.Response.Write(res.ToXml());
            //    page.Response.End();
            //}
        }

        ////查询订单
        //private bool QueryOrder(string transaction_id)
        //{
        //    WxPayData req = new WxPayData();
        //    req.SetValue("transaction_id", transaction_id);
        //    WxPayData res = WxPayApi.OrderQuery(req);
        //    if (res.GetValue("return_code").ToString() == "SUCCESS" &&
        //        res.GetValue("result_code").ToString() == "SUCCESS")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}