using ASPNET.WebControls;
using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
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

public partial class Admin_member_CW_O2OMembers : AdminPage
{
    /// <summary>
    /// 购机日期-开始
    /// </summary>
    private string StartTime;
    /// <summary>
    /// 购机日期-结束
    /// </summary>
    private string EndTime;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
        this.grdMember.ReBindData += new Grid.ReBindDataEventHandler(this.grdMember_ReBindData);
        this.grdMember.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdMember_RowDeleting);
      
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        //2016-08-08验证当前登陆用户类型
        bool isFiliale = false;//是否分公司
        string strStoreCode = "";//门店Code
        ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        if (currentManager.RoleId == masterSettings.StoreRoleId)
        {
            DistributorsInfo disinfo = VShopHelper.GetUserIdDistributors(currentManager.ClientUserId);
            if (disinfo != null && disinfo.StoreId > 0)
            {
                StoreInfo info = StoreInfoHelper.GetStoreInfo(disinfo.StoreId);
                if (info != null && !string.IsNullOrEmpty(info.accountALLHere))
                {
                    strStoreCode = info.accountALLHere;//当前门店Code
                }
            }
        }
        if (currentManager.RoleId == masterSettings.FilialeRoleId)
        {
            isFiliale = true;//当前登陆为分公司用户
        }

        CW_O2OMemberQuery memberQuery = (CW_O2OMemberQuery)this.GetMemberQuery();
        if (!string.IsNullOrEmpty(strStoreCode))
            memberQuery.storeCode = strStoreCode;
        memberQuery.storeCodes = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";

        DbQueryResult members = O2OMemberHelper.GetListO2OMember(memberQuery);
        this.grdMember.DataSource = members.Data;
        this.grdMember.DataBind();
        this.txtSearchText.Text = memberQuery.name;
        this.txtStartTime.Text = memberQuery.startTime;
        this.txtEndTime.Text = memberQuery.endTime;
        this.pager1.TotalRecords = this.pager.TotalRecords =  members.TotalRecords;
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
        if(!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
        {
            query.startTime = base.Server.UrlDecode(this.Page.Request.QueryString["startTime"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
        {
            query.endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
        }
        query.PageSize = this.pager1.PageSize;
        query.PageIndex = this.pager1.PageIndex;
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
    private void grdMember_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int userId = (int)this.grdMember.DataKeys[e.RowIndex].Value;
        O2OMemberHelper.DeleteO2OMembers(userId);
        this.BindData();
        this.ShowMsg("成功删除了一个会员", true);
    }
    private void ReloadMemberLogs(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("name", System.Convert.ToString(this.txtSearchText.Text));
        queryStrings.Add("startTime", System.Convert.ToString(this.txtStartTime.Text));
        queryStrings.Add("endTime", System.Convert.ToString(this.txtEndTime.Text));
        if (!isSearch)
        {
            queryStrings.Add("PageIndex", this.pager1.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        queryStrings.Add("SortBy", this.grdMember.SortOrderBy);
        queryStrings.Add("SortOrder", SortAction.Desc.ToString());
        base.ReloadPage(queryStrings);
    }
    private void grdMember_ReBindData(object sender)
    {
        this.ReloadMemberLogs(false);
    }
    protected void btnSearchButton_Click(object sender, EventArgs e)
    {
        this.ReloadMemberLogs(true);
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
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
            {
                query.startTime = base.Server.UrlDecode(this.Page.Request.QueryString["startTime"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
            {
                query.endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
            }


            //2016-08-08验证当前登陆用户类型
            bool isFiliale = false;//是否分公司
            string strStoreCode = "";//门店Code
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.StoreRoleId)
            {
                DistributorsInfo disinfo = VShopHelper.GetUserIdDistributors(currentManager.ClientUserId);
                if (disinfo != null && disinfo.StoreId > 0)
                {
                    StoreInfo info = StoreInfoHelper.GetStoreInfo(disinfo.StoreId);
                    if (info != null && !string.IsNullOrEmpty(info.accountALLHere))
                    {
                        strStoreCode = info.accountALLHere;//当前门店Code
                    }
                }
            }
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }
            if (!string.IsNullOrEmpty(strStoreCode))
                query.storeCode = strStoreCode;
            query.storeCodes = isFiliale ? getStoreDzCode(currentManager.ClientUserId) : "";
            query.SortOrder = SortAction.Desc;

            DataSet ds = new DataSet();
            //得到待导出的数据集
            System.Data.DataTable o2oMember = O2OMemberHelper.GetO2OMemberExprot(query, fields);
            //分Sheet导出，没个Sheet 最多6万数据
            int icount = 60000;
            if (o2oMember.Rows.Count > icount)
            {
                float aa = (float)o2oMember.Rows.Count;
                double bb = (double)(aa / icount);
                //向下取整
                int itable = Convert.ToInt32(Math.Ceiling(bb));
                for (int itab = 0; itab < itable; itab++)
                {
                    DataTable dt = o2oMember.Clone();
                    dt.TableName = "录入用户信息" + (itab + 1);
                    if (itab == (itable - 1))
                    {
                        for (int i = icount * itab; i < o2oMember.Rows.Count; i++)
                        {
                            //DataRow dr = dt.NewRow();
                            dt.Rows.Add(o2oMember.Rows[i].ItemArray);
                        }
                    }
                    else
                    {
                        for (int i = icount * itab; i < icount * (itab + 1); i++)
                        {
                            dt.Rows.Add(o2oMember.Rows[i].ItemArray);
                        }
                    }
                    //将新建表添加到数据集
                    ds.Tables.Add(dt);
                }
            }
            else
            {
                ds.Tables.Add(o2oMember.Copy());
                ds.Tables[0].TableName = "录入用户信息";
            }

            string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\managemembersxls\\录入会员信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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


}