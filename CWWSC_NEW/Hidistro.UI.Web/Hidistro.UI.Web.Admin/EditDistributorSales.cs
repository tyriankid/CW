﻿using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using Hidistro.ControlPanel.Function;
using System.Text.RegularExpressions;
using ControlPanel.Commodities;
using Hidistro.Core.Entities;
using ASPNET.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public class EditDistributorSales : AdminPage
    {
        protected System.Web.UI.WebControls.Literal litPageTip;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtstoreRelationPerson;
        protected System.Web.UI.WebControls.TextBox txtaccountALLHere;
        protected System.Web.UI.WebControls.TextBox txtDsName;
        protected System.Web.UI.WebControls.TextBox txtDsPhone;
        protected System.Web.UI.WebControls.TextBox txtScode;
        protected Pager pager;
        private string searchkey;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (currentManager.RoleId == masterSettings.StoreRoleId)
                {
                    if (this.Request.QueryString["id"] != null && !string.IsNullOrEmpty(this.Request.QueryString["id"].ToString()))
                    {
                        this.litPageTip.Text = "编辑门店店员信息。";

                        Guid guid = new Guid(this.Request.QueryString["id"].ToString());
                        ShowData(guid);
                    }
                    else
                    {
                        this.litPageTip.Text = "添加门店店员信息。";
                        ShowData(currentManager.ClientUserId);
                    }

                    this.txtStoreName.Enabled = false;
                    this.txtstoreRelationPerson.Enabled = false;
                    this.txtaccountALLHere.Enabled = false;
                }
                else
                {
                    this.ShowMsgAndReUrl("店员信息维护必须店长登录", false, Globals.ApplicationPath + "/admin/distributor/ListDistributorSales.aspx");
                }
            }
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }

        /// <summary>
        /// 添加时，显示当前店长信息
        /// </summary>
        protected void ShowData(int clientUserId)
        {
            //门店用户
            DataTable dt = DistributorSalesHelper.SelectDistributorsInfo(clientUserId);
            if (dt.Rows.Count > 0)
            {
                this.txtStoreName.Text = dt.Rows[0]["StoreName"].ToString();
                this.txtstoreRelationPerson.Text = dt.Rows[0]["storeRelationPerson"].ToString();
                this.txtaccountALLHere.Text = dt.Rows[0]["accountALLHere"].ToString();
            }
        }

        /// <summary>
        /// 页面加载显示值
        /// </summary>
        /// <param name="dsid"></param>
        protected void ShowData(Guid dsid)
        {
            DistributorSales salesinfo = DistributorSalesHelper.GetSalesByDsID(dsid);
            if (salesinfo != null && salesinfo.DsID != Guid.Empty)
            {
                this.txtStoreName.Text = salesinfo.StoreName;
                this.txtstoreRelationPerson.Text = salesinfo.storeRelationPerson;
                this.txtaccountALLHere.Text = salesinfo.accountALLHere;
                this.txtDsName.Text = salesinfo.DsName;
                this.txtDsPhone.Text = salesinfo.DsPhone;
                this.txtScode.Text = salesinfo.Scode;

                //认证后的店员不允许修改名称/电话
                if (salesinfo.IsRz == 1)
                {
                    this.txtDsName.Enabled = false;
                    this.txtDsPhone.Enabled = false;
                }
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int disuserid = 0;//当前门店ID（aspnet_Distributors 表的UserID值）
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.StoreRoleId)
            {
                //门店用户
                disuserid = currentManager.ClientUserId;
            }
            else
            {
                this.ShowMsg("店员信息维护必须店长", false);
                return;
            }

            Guid dsid = Guid.Empty;//页面参数，判断新增/编辑
            if (this.Request.QueryString["id"] != null && !string.IsNullOrEmpty(this.Request.QueryString["id"].ToString()))
            {
                dsid = new Guid(this.Request.QueryString["id"].ToString());
            }

            ///得到原始数据项
            DataTable dtSales = DistributorSalesHelper.SelectSalesByDisUserId(disuserid);


            //待处理的数据对象
            DistributorSales salesinfo;
            if(dsid != Guid.Empty)
            {
                //编辑
                salesinfo = DistributorSalesHelper.GetSalesByDsID(dsid);

                //查询店员姓名是否存在
                if (!salesinfo.DsName.Equals(this.txtDsName.Text.Trim()))
                {
                    if (dtSales.Select(string.Format("DsName = '{0}'", this.txtDsName.Text.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                    {
                        //已经存在则提示，并停止保存
                        this.ShowMsg("店员姓名【" + this.txtDsName.Text.Trim() + "】已经存在！", false);
                        return;
                    }
                }
                //已经认证后不允许修改名称
                if (salesinfo.IsRz != 1)
                {
                    //店员认证后是不允许修改信息的
                    salesinfo.DsName = this.txtDsName.Text.Trim();
                    salesinfo.DsPhone = this.txtDsPhone.Text.Trim();
                }
                salesinfo.Scode = this.txtScode.Text.Trim();
                if (DistributorSalesHelper.UpdateDistributorSales(salesinfo))
                    this.ShowMsgAndReUrl("编辑店员信息成功！", true, Globals.ApplicationPath + "/admin/distributor/ListDistributorSales.aspx");
                else
                    this.ShowMsg("编辑店员信息失败！", false);
            }
            else
            {
                //查询店员姓名是否存在
                if (dtSales.Select(string.Format("DsName = '{0}'", this.txtDsName.Text.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                {
                    //已经存在则提示，并停止保存
                    this.ShowMsg("店员姓名【" + this.txtDsName.Text.Trim() + "】已经存在！", false);
                    return;
                }

                //新增
                salesinfo = new DistributorSales();
                salesinfo.DsID = Guid.NewGuid();
                salesinfo.DisUserId = disuserid;
                salesinfo.DsName = this.txtDsName.Text.Trim();
                salesinfo.DsPhone = this.txtDsPhone.Text.Trim();
                salesinfo.Scode = this.txtScode.Text.Trim();
                if(DistributorSalesHelper.AddDistributorSales(salesinfo))
                    this.ShowMsgAndReUrl("新增店员信息成功！", true, Globals.ApplicationPath + "/admin/distributor/ListDistributorSales.aspx");
                else
                    this.ShowMsg("新增店员信息失败！", false);
            }
        }


        private bool ValidationFiliale(StoreInfo StoreInfo)
        {
            ValidationResults results = Validation.Validate<StoreInfo>(StoreInfo, new string[]
			{
				"ValStoreInfo"
			});
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (System.Collections.Generic.IEnumerable<ValidationResult>)results)
                {
                    msg += Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}
