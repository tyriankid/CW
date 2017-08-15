using Hidistro.ControlPanel.Commodities;
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
    public class EditServiceClass : AdminPage
    {
        protected System.Web.UI.WebControls.Literal litPageTip;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.TextBox txtClassName;
        protected System.Web.UI.WebControls.TextBox txtScode;
        protected Pager pager;
        private string searchkey;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarkTip;
        //protected System.Web.UI.HtmlControls.HtmlGenericControl txtTypeNameTip;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int scid = 0;
                //ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                if (this.Request.QueryString["id"] != null && !string.IsNullOrEmpty(this.Request.QueryString["id"].ToString()) && int.TryParse(this.Request.QueryString["id"].ToString(), out scid))
                {
                    this.litPageTip.Text = "编辑服务品类信息。";
                    ShowData(scid);
                }
                else
                {
                    this.litPageTip.Text = "添加服务品类信息。";
                }
            }
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }

        /// <summary>
        /// 页面加载显示值
        /// </summary>
        /// <param name="dsid"></param>
        protected void ShowData(int scid)
        {
            ServiceClass classinfo = ServiceClassHelper.GetClassByDsID(scid);
            if (classinfo != null && classinfo.ScID > 0)
            {
                this.txtClassName.Text = classinfo.ClassName;
                this.txtScode.Text = classinfo.Scode.ToString();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //ManagerInfo currentManager = ManagerHelper.GetCurrentManager();

            int scid = 0;//页面参数，判断新增/编辑
            if (this.Request.QueryString["id"] != null && !string.IsNullOrEmpty(this.Request.QueryString["id"].ToString()) && int.TryParse(this.Request.QueryString["id"].ToString(), out scid)) { }

            ///得到原始数据项
            DataTable dtClass = ServiceClassHelper.SelectClassByWhere(string.Empty);

            //待处理的数据对象
            ServiceClass classinfo;
            if (scid > 0)
            {
                //编辑
                classinfo = ServiceClassHelper.GetClassByDsID(scid);

                //查询店员姓名是否存在
                if (!classinfo.ClassName.Equals(this.txtClassName.Text.Trim()))
                {
                    if (dtClass.Select(string.Format("ClassName = '{0}'", this.txtClassName.Text.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                    {
                        //已经存在则提示，并停止保存
                        this.ShowMsg("服务品类【" + this.txtClassName.Text.Trim() + "】已经存在！", false);
                        return;
                    }
                }
            }
            else
            {
                classinfo = new ServiceClass();
                //查询服务品类名称是否存在
                if (dtClass.Select(string.Format("ClassName = '{0}'", this.txtClassName.Text.Trim()), "", DataViewRowState.CurrentRows).Length > 0)
                {
                    //已经存在则提示，并停止保存
                    this.ShowMsg("服务品类【" + this.txtClassName.Text.Trim() + "】已经存在！", false);
                    return;
                }
            }
            //保存前得到实体对象值
            classinfo.ClassName = this.txtClassName.Text.Trim();
            int scode = 0;
            if (int.TryParse(this.txtScode.Text.Trim(), out scode))
                classinfo.Scode = scode;

            if (classinfo.ScID > 0)
            {
                if (ServiceClassHelper.UpdateServiceClass(classinfo))
                    this.ShowMsgAndReUrl("编辑服务品类信息成功！", true, Globals.ApplicationPath + "/admin/product/ServiceClassList.aspx");
                else
                    this.ShowMsg("编辑服务品类信息失败！", false);
            }
            else
            {
                if (ServiceClassHelper.AddServiceClass(classinfo))
                    this.ShowMsgAndReUrl("新增服务品类信息成功！", true, Globals.ApplicationPath + "/admin/product/ServiceClassList.aspx");
                else
                    this.ShowMsg("新增服务品类信息失败！", false);
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
