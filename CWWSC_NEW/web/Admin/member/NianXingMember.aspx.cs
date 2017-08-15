using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_NianXingMember :AdminPage
{
    private void BindData()
    {
        //2016-08-08验证当前登陆用户类型
        bool isFiliale = false;//是否分公司
        ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        if (currentManager.RoleId == masterSettings.FilialeRoleId)
        {
            isFiliale = true;//当前登陆为分公司用户
        }

        nianxingMemberQuery memberQuery = (nianxingMemberQuery)this.GetMemberQuery();
        memberQuery.StoreCode = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";//如果是分公司登陆这只显示分公司下的人员信息

        DbQueryResult members = nianxingMembersHelper.GetListMember(memberQuery);
        this.grdMember.DataSource = members.Data;
        this.grdMember.DataBind();
        this.txtSearchText.Text = memberQuery.UserName;
        this.pager.TotalRecords = members.TotalRecords;
        this.pager1.TotalRecords = members.TotalRecords;
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

    private void btnSearchButton_Click(object sender, System.EventArgs e)
    {
        this.ReloadMemberLogs(true);
    }

    private nianxingMemberQuery GetMemberQuery()
    {
        nianxingMemberQuery query = new nianxingMemberQuery();
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
        {
            query.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
        }
        query.PageSize = this.pager.PageSize;
        query.PageIndex = this.pager.PageIndex;
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
        {
            query.SortBy = this.Page.Request.QueryString["SortBy"];
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
        {
            query.SortOrder = SortAction.Desc;
        }
        return query;
    }
    private void grdMember_ReBindData(object sender)
    {
        this.ReloadMemberLogs(false);
    }
    private void grdMember_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int userId = (int)this.grdMember.DataKeys[e.RowIndex].Value;
        nianxingMembersHelper.DeleteMembers(userId);
        this.BindData();
        this.ShowMsg("成功删除了一个会员", true);
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
        this.grdMember.ReBindData += new Grid.ReBindDataEventHandler(this.grdMember_ReBindData);
        this.grdMember.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMember_RowDeleting);
        this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
        this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
        if (!this.Page.IsPostBack)
        {
            this.grdMember.SortOrderBy = "id";
            this.BindData();
        }
    }
    private void ReloadMemberLogs(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("UserName", System.Convert.ToString(this.txtSearchText.Text));
        if (!isSearch)
        {
            queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("SortBy", this.grdMember.SortOrderBy);
        queryStrings.Add("SortOrder", SortAction.Desc.ToString());
        base.ReloadPage(queryStrings);
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

            nianxingMemberQuery query = new nianxingMemberQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                query.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            query.SortBy = "UserName";
            query.SortOrder = SortAction.Desc;

            //2016-08-08验证当前登陆用户类型
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }
            query.StoreCode = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";//如果是分公司登陆这只显示分公司下的人员信息

            //得到待导出的数据集
            System.Data.DataTable allHereMember = nianxingMembersHelper.GetExportCwMemberByQuery(query, fields);

            string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\managemembersxls\\粘性会员信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            ExcelDBClass exls = new ExcelDBClass(strPath, false);
            DataSet ds = new DataSet();
            ds.Tables.Add(allHereMember.Copy());
            ds.Tables[0].TableName = "金力会员信息";
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
}