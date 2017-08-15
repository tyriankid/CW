using ASPNET.WebControls;
using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Members)]
    public class AllMember : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnExport;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnSendEmail;
        protected System.Web.UI.WebControls.Button btnSendMessage;
        protected ExportFieldsCheckBoxList2 exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected Grid grdMemberList;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdenableemail;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdenablemsg;
        protected PageSize hrefPageSize;
        protected System.Web.UI.WebControls.Literal litsmscount;
        protected ImageLinkButton lkbDelectCheck;
        protected ImageLinkButton lkbDelectCheck1;
        protected Pager pager;
        protected Pager pager1;
        private string realName;
        private string cellPhone;
        private string startTime;
        private string endTime;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span2;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span3;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span4;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtemailcontent;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;
        protected System.Web.UI.WebControls.TextBox txtRealName;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected WebCalendar txtStartTime;
        protected WebCalendar txtEndTime;

        protected void BindData()
        {
            //2016-08-08验证当前登陆用户类型
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }


            CW_O2OMemberQuery memberQuery = (CW_O2OMemberQuery)this.GetMemberQuery();
            memberQuery.storeCodes = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";//如果是分公司登陆这只显示分公司下的人员信息


            DbQueryResult members = O2OMemberHelper.GetListNxMember(memberQuery);            
            this.txtRealName.Text = memberQuery.name;
            this.txtCellPhone.Text = memberQuery.cell;
            this.txtStartTime.Text = memberQuery.startTime;
            this.txtEndTime.Text = memberQuery.endTime;
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
        }

        //得到分公司下所有门店前端用户ID
        private string getStoreDzCode(int fgsid)
        {
            string strDzCodes = string.Empty;
            DataTable dtStores = StoreInfoHelper.SelectStoreInfoByWhere(string.Format("fgsid = {0}", fgsid));
            foreach (DataRow dr in dtStores.Rows)
            {
                strDzCodes += "'" + dr["accountALLHere"].ToString() + "',";
            }
            strDzCodes = strDzCodes.TrimEnd(',');//去除最后一个 , 符号
            return strDzCodes;
        }

        private CW_O2OMemberQuery GetMemberQuery()
        {
            CW_O2OMemberQuery query = new CW_O2OMemberQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["name"]))
            {
                query.name = base.Server.UrlDecode(this.Page.Request.QueryString["name"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cell"]))
            {
                query.cell = base.Server.UrlDecode(this.Page.Request.QueryString["cell"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
            {
                query.startTime = base.Server.UrlDecode(this.Page.Request.QueryString["startTime"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
            {
                query.endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
            }
            query.isNxMember = true;
            query.PageSize = this.pager1.PageSize;
            query.PageIndex = this.pager1.PageIndex;
            query.SortBy = "rowNum";
            query.SortOrder = SortAction.Asc;
            //if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
            //{
            //    query.SortBy = this.Page.Request.QueryString["SortBy"];
            //}
            //if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
            //{
            //    query.SortOrder = SortAction.Desc;
            //}
            return query;
        }

        private void btnExport_Click(object sender, System.EventArgs e)
        {
            if (this.exportFieldsCheckBoxList.SelectedItem == null)
            {
                this.ShowMsg("请选择需要导出的会员信息", false);
            }
            else
            {
                System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
                System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
                foreach (System.Web.UI.WebControls.ListItem item in this.exportFieldsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        fields.Add(item.Value + " " + item.Text);
                        list2.Add(item.Text);
                    }
                }

                CW_O2OMemberQuery query = new CW_O2OMemberQuery();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["name"]))
                {
                    query.name = base.Server.UrlDecode(this.Page.Request.QueryString["name"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cell"]))
                {
                    query.cell = base.Server.UrlDecode(this.Page.Request.QueryString["cell"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
                {
                    query.startTime = base.Server.UrlDecode(this.Page.Request.QueryString["startTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
                {
                    query.endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
                }
                query.isNxMember = true;

                //2016-08-08验证当前登陆用户类型
                bool isFiliale = false;//是否分公司
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (currentManager.RoleId == masterSettings.FilialeRoleId)
                {
                    isFiliale = true;//当前登陆为分公司用户
                }
                query.storeCodes = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";//如果是分公司登陆这只显示分公司下的人员信息

                DataSet ds = new DataSet();
                System.Data.DataTable allMember = O2OMemberHelper.GetAllMemberExprot(query, fields);
                int icount = 60000;
                if (allMember.Rows.Count > icount)
                {
                    float aa = (float)allMember.Rows.Count;
                    double bb = (double)(aa / icount);
                    //向下取整
                    int itable = Convert.ToInt32(Math.Ceiling(bb));
                    for (int itab = 0; itab < itable; itab++)
                    {
                        DataTable dt = allMember.Clone();
                        dt.TableName = "粘性会员总汇" + (itab + 1);
                        if (itab == (itable - 1))
                        {
                            for (int i = icount * itab; i < allMember.Rows.Count; i++)
                            {
                                //DataRow dr = dt.NewRow();
                                dt.Rows.Add(allMember.Rows[i].ItemArray);
                            }
                        }
                        else
                        {
                            for (int i = icount * itab; i < icount * (itab + 1); i++)
                            {
                                dt.Rows.Add(allMember.Rows[i].ItemArray);
                            }
                        }
                        //将新建表添加到数据集
                        ds.Tables.Add(dt);
                    }
                }
                else
                {
                    ds.Tables.Add(allMember.Copy());
                    ds.Tables[0].TableName = "粘性会员总汇";
                }

                string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\allmemberxls\\粘性会员总汇" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                ExcelDBClass exls = new ExcelDBClass(strPath, false);
                exls.ImportToExcel(ds);
                exls.Dispose();
                //将文件从服务器上下载到本地
                FileInfo fileInfo = new FileInfo(strPath);//文件路径如：E:/11/22  
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + Server.UrlEncode(fileInfo.Name.ToString()));//文件名称  
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());//文件长度  
                Response.ContentType = "application/octet-stream";//获取或设置HTTP类型  
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(strPath);//将文件内容作为文件块直接写入HTTP响应输出流
            }
        }

        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
        
        private void grdMemberList_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void grdMemberList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int userId = (int)this.grdMemberList.DataKeys[e.RowIndex].Value;
            if (O2OMemberHelper.DeleteO2OMembers(userId))
            {
                this.BindData();
                this.ShowMsg("成功删除了一个会员", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }  
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkbDelectCheck_Click(object sender, System.EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int num = 0;
            foreach (System.Web.UI.WebControls.GridViewRow row in this.grdMemberList.Rows)
            {
                System.Web.UI.WebControls.CheckBox box = (System.Web.UI.WebControls.CheckBox)row.FindControl("checkboxCol");
                if (box.Checked && MemberHelper.Delete(System.Convert.ToInt32(this.grdMemberList.DataKeys[row.RowIndex].Value)))
                {
                    num++;
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要删除的会员账号", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了选择的会员", true);
            }
        }

        /// <summary>
        /// 加载并设置参数
        /// </summary>
        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["name"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["name"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cell"]))
                {
                    this.cellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["cell"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
                {
                    this.startTime = base.Server.UrlDecode(this.Page.Request.QueryString["startTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
                {
                    this.endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
                }
                this.txtRealName.Text = this.realName;
                this.txtCellPhone.Text = this.cellPhone;
                this.txtStartTime.Text = this.startTime;
                this.txtEndTime.Text = this.endTime;
            }
            else
            {
                this.realName = this.txtRealName.Text.Trim();
                this.cellPhone = this.txtCellPhone.Text.Trim();
                this.startTime = this.txtStartTime.Text.Trim();
                this.endTime = this.txtEndTime.Text.Trim();
            }
        }
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
            this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
            this.lkbDelectCheck.Click += new System.EventHandler(this.lkbDelectCheck_Click);
            //this.lkbDelectCheck1.Click += new System.EventHandler(this.lkbDelectCheck_Click);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
        
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("name", this.txtRealName.Text);
            queryStrings.Add("cell", this.txtCellPhone.Text);
            queryStrings.Add("startTime", this.txtStartTime.Text);
            queryStrings.Add("endTime", this.txtEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}
