using ControlPanel.Commodities;
using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
    public class SetAuditing : AdminPage
	{
        protected TextBox txtAccount;//不通过原因
        protected System.Web.UI.WebControls.Button btnPass;
        protected System.Web.UI.WebControls.Button btnNoPass;
        protected System.Web.UI.WebControls.Button btnClose;
        protected HiddenField txtProductIds;//隐藏域,传递的商品ID集合
        
        protected int userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtProductIds.Value = this.Page.Request.QueryString["productIds"];
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            this.btnNoPass.Click += new System.EventHandler(this.btnNoPass_Click);
            if (!this.IsPostBack)
            {
                //第一次加载暂无内容
            }
        }

        //保存商品区域关系
        protected void btnPass_Click(object sender, EventArgs e)
        {
            string str = string.Empty;//定义回调提示语变量
            int pid = 0;
            if (!string.IsNullOrEmpty(this.txtProductIds.Value) && int.TryParse(this.txtProductIds.Value,out pid))
            {
                //将商品信息提交到AH
                System.Collections.Generic.IList<int> tagsId = null;
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                ProductInfo product = ProductHelper.GetProductDetails(pid, out dictionary, out tagsId);
                if (product != null)
                {
                    SupplierInfo supplierinfo = SupplierHelper.GetSupplier(product.SupplierId);
                    if(supplierinfo != null && !string.IsNullOrEmpty(supplierinfo.gysCode))
                    {
                        string spnmcode = "";
                        string mpfspcode = "";

                        StringBuilder strJson = new StringBuilder();
                        strJson.Append("{");
                        strJson.AppendFormat("\"MPF_SP\":\"{0}\",", product.ProductId);
                        strJson.AppendFormat("\"SPNM\":\"{0}\",", product.ProductCode);
                        strJson.AppendFormat("\"SPNAME\":\"{0}\",", product.ProductName);
                        strJson.AppendFormat("\"SPXH\":\"{0}\",", product.SKU);
                        strJson.AppendFormat("\"WEIGHT\":\"{0}\",", product.Weight);
                        strJson.AppendFormat("\"METERING\":\"{0}\",", product.Unit);
                        strJson.AppendFormat("\"SUPCODE\":\"{0}\"", supplierinfo.gysCode);
                        strJson.Append("}");
                        CwAHAPI.CwapiLog("**************发送商品接口数据：" + strJson.ToString());

                        AllHereServiceReference.MPFTOJLClient aa = new AllHereServiceReference.MPFTOJLClient();
                        string strResult = aa.MPFTOJL_SPXX(strJson.ToString());
                        CwAHAPI.CwapiLog("**************返回数据：" + strResult.ToString());
                        if (!string.IsNullOrEmpty(strResult))
                        {
                            if (CwAHAPI.ResolutionProductAHResultString(strResult, out str, out spnmcode, out mpfspcode) > -1)
                            {
                                if (txtProductIds.Value.Equals(mpfspcode))
                                {
                                    //得到选择的商品Id值集合
                                    bool isOk = ProductHelper.UpdateProductSaleStatus(mpfspcode, 1, spnmcode.ToString());
                                    if (isOk)
                                        str = string.Format("ShowMsg(\"{0}\", {1})", "所选商品已设置为通过，操作成功。", "true");//成功
                                    else
                                        str = string.Format("ShowMsg(\"{0}\", {1})", "所选商品未设置为通过，操作失败。", "false");//失败
                                }
                                else
                                    str = string.Format("ShowMsg(\"{0}\", {1})", "返回的微平台商品编码值与传递的不一致，操作失败。", "false");//失败
                            }
                            else
                                str = string.Format("ShowMsg(\"{0}\", {1})", "调用金力接口发生错误" + str.Replace(":","-").Replace(".",""), "false");//失败
                        }
                        else
                            str = string.Format("ShowMsg(\"{0}\", {1})", "调用金力接口发生错误，无返回结果！", "false");//失败
                    }
                    else
                        str = string.Format("ShowMsg(\"{0}\", {1})", "参数传递错误，查询不到对应的供应商信息！", "false");//失败
                }
                else
                    str = string.Format("ShowMsg(\"{0}\", {1})", "参数传递错误，查询不到对应的商品信息！", "false");//失败
            }
            else
            {
                str = string.Format("ShowMsg(\"{0}\", {1})", "参数传递错误！", "false");//失败
            }
            //前端（客户端）弹出提示
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);setTimeout(function(){CloseDialogFrame()},1000);</script>");
        }

        /// <summary>
        /// 不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoPass_Click(object sender, EventArgs e)
        {
            //得到选择的商品Id值集合
            string[] arrayProductId = this.txtProductIds.Value.Split(',');
            string str = string.Empty;//定义回调提示语变量
            if (!string.IsNullOrEmpty(this.txtProductIds.Value))
            {
                if (!string.IsNullOrEmpty(this.txtAccount.Text))
                {
                    //得到选择的商品Id值集合
                    bool isOk = ProductHelper.UpdateProductSaleStatus(txtProductIds.Value, -2);
                    if (isOk)
                    {
                        DataTable dtBack = ProductBackAccountHelper.GetProductBackAccount(0);
                        foreach(string id in arrayProductId)
                        {
                            ////存储不通过原因
                            //ProductBackAccount backAccount = new ProductBackAccount();
                            DataRow dr = dtBack.NewRow();
                            dr["productId"] = id;
                            dr["backTime"] = DateTime.Now;
                            dr["backAccount"] = this.txtAccount.Text;
                            dtBack.Rows.Add(dr);
                        }
                        //提交表数据
                        string strSql = "select * from CW_ProductBackAccount";
                        int iback = DataBaseHelper.CommitDataTable(dtBack, strSql);
                        if (iback > 0)
                        {
                            str = string.Format("ShowMsg(\"{0}\", {1})", "所选商品已设置为不通过，操作成功。", "true");//成功
                        }
                        else
                            str = string.Format("ShowMsg(\"{0}\", {1})", "参数传递错误！", "false");//失败
                    }
                    else
                        str = string.Format("ShowMsg(\"{0}\", {1})", "所选商品未设置为不通过，操作失败。", "false");//失败

                }
                else
                    str = string.Format("ShowMsg(\"{0}\", {1})", "您输入不通过原因！", "false");//失败
            }
            else
                str = string.Format("ShowMsg(\"{0}\", {1})", "参数传递错误！", "false");//失败
            //前端（客户端）弹出提示
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);setTimeout(function(){CloseDialogFrame()},1000);</script>");
        }
        

	}
}
