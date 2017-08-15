using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
	public class BalanceDrawRequestList : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportButton;
		private string CheckTime = "";
		protected Pager pager;
		protected System.Web.UI.WebControls.Repeater reBalanceDrawRequest;
		private string RequestTime = "";
		private string StoreName = "";
        private string AccountALLHere = "";
		protected WebCalendar txtCheckTime;
		protected WebCalendar txtRequestTime;
		protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
		private void BindData()
		{
            bool isFiliale = false;//�Ƿ�ֹ�˾
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//��ǰ��½Ϊ�ֹ�˾�û�
            }

			BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery
			{
				CheckTime = this.CheckTime,
				RequestTime = this.RequestTime,
				StoreName = this.StoreName,
                AccountALLHere=this.AccountALLHere,
                UserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "",
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
				SortBy = "UserId",
				RequestEndTime = "",
				RequestStartTime = "",
				IsCheck = ""
			};
			Globals.EntityCoding(entity, true);
			DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(entity);
			this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
			this.reBalanceDrawRequest.DataBind();
			this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
		}

        private string getStorIds(int fgsid)
        {
            string strStorId = string.Empty;
            DataTable dtDistributors = StoreInfoHelper.SelectStoreClientUserIdByFgsId(fgsid);
            foreach (DataRow dr in dtDistributors.Rows)
            {
                strStorId += dr["UserId"].ToString() + ",";
            }
            strStorId = strStorId.TrimEnd(',');//ȥ�����һ�� , ����
            if (string.IsNullOrEmpty(strStorId))
                strStorId = "null";
            return strStorId;
        }
        private void btnExportButton_Click(object sender, System.EventArgs e)
        {
            bool isFiliale = false;//�Ƿ�ֹ�˾
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//��ǰ��½Ϊ�ֹ�˾�û�
            }

            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery();
            if (this.CheckTime!=null) 
                entity.CheckTime = this.CheckTime.ToString();
            if (this.RequestTime!=null) 
                entity.RequestTime = this.RequestTime.ToString();
            if (this.StoreName!=null)
                entity.StoreName = this.StoreName.ToString();
            if (this.AccountALLHere != null)
                entity.AccountALLHere = this.AccountALLHere.ToString();
            entity.UserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "";
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "UserId";
            entity.RequestEndTime = "";
            entity.RequestStartTime = "";
            entity.IsCheck = "";

            if (entity!=null)
            {
                DataTable balanceDrawRequest = VShopHelper.GetExportBalanceDrawRequest(entity);
                if (balanceDrawRequest.Rows.Count>=1)
                {
                    DataTable exportdata = new DataTable();
                    exportdata.Columns.Add("�����ʺ�");
                    exportdata.Columns.Add("�ŵ�����");
                    exportdata.Columns.Add("�������ֽ��");
                    exportdata.Columns.Add("��������");
                    exportdata.Columns.Add("��������");
                    exportdata.Columns.Add("�Ƿ񷢷�");
                    exportdata.Columns.Add("�ֻ�����");
                    exportdata.Columns.Add("��ע");
                    for (int i = 0; i < balanceDrawRequest.Rows.Count; i++)
                    {
                        exportdata.Rows.Add(
                            balanceDrawRequest.Rows[i]["AccountALLHere"].ToString(),
                            balanceDrawRequest.Rows[i]["StoreName"].ToString(),
                            "��" + balanceDrawRequest.Rows[i]["Amount"],
                            balanceDrawRequest.Rows[i]["RequestTime"].ToString(),
                            balanceDrawRequest.Rows[i]["CheckTime"].ToString(),
                            balanceDrawRequest.Rows[i]["IsCheck"].ToString() == "False" ? "δ����" : balanceDrawRequest.Rows[i]["Remark"].ToString().IndexOf("��ͨ��") > -1 ? "δͨ��" : "�ѷ���",
                            balanceDrawRequest.Rows[i]["CellPhone"].ToString(),
                            balanceDrawRequest.Rows[i]["Remark"].ToString()
                            );
                    }

                    string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\balancedrawrequestxls\\���ּ�¼" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                    ExcelDBClass exls = new ExcelDBClass(strPath, false);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(exportdata.Copy());
                    ds.Tables[0].TableName = "���ּ�¼";
                    exls.ImportToExcel(ds);
                    exls.Dispose();
                    //���ļ��ӷ����������ص�����
                    FileInfo fileInfo = new FileInfo(strPath);//�ļ�·���磺E:/11/22  
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + Server.UrlEncode(fileInfo.Name.ToString()));//�ļ�����  
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());//�ļ�����  
                    Response.ContentType = "application/octet-stream";//��ȡ������HTTP����  
                    Response.ContentEncoding = System.Text.Encoding.Default;
                    Response.WriteFile(strPath);//���ļ�������Ϊ�ļ���ֱ��д��HTTP��Ӧ�����


                    //System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
                    //fields.Add("�����ʺ�");
                    //list2.Add("�����ʺ�");
                    //fields.Add("�ŵ�����");
                    //list2.Add("�ŵ�����");
                    //fields.Add("�������ֽ��");
                    //list2.Add("�������ֽ��");
                    //fields.Add("��������");
                    //list2.Add("��������");
                    //fields.Add("��������");
                    //list2.Add("��������");
                    //fields.Add("�Ƿ񷢷�");
                    //list2.Add("�Ƿ񷢷�");
                    //fields.Add("�ֻ�����");
                    //list2.Add("�ֻ�����");
                    //fields.Add("��ע");
                    //list2.Add("��ע");

                    //System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    //foreach (string str in list2)
                    //{
                    //    builder.Append(str + ",");
                    //    if (str == list2[list2.Count - 1])
                    //    {
                    //        builder = builder.Remove(builder.Length - 1, 1);
                    //        builder.Append("\r\n");
                    //    }
                    //}
                    //foreach (System.Data.DataRow row in exportdata.Rows)
                    //{
                    //    foreach (string str2 in fields)
                    //    {
                    //        builder.Append(row[str2]).Append(",");
                    //        if (str2 == fields[list2.Count - 1])
                    //        {

                    //            builder = builder.Remove(builder.Length - 1, 1);
                    //            builder.Append("\r\n");
                    //        }
                    //    }
                    //}
                    //this.Page.Response.Clear();
                    //this.Page.Response.Buffer = false;
                    //this.Page.Response.Charset = "GB2312";
                    //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=���ּ�¼.csv");
                    //this.Page.Response.ContentType = "application/octet-stream";
                    //this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    //this.Page.EnableViewState = false;
                    //this.Page.Response.Write(builder.ToString());
                    //this.Page.Response.End();
                }
                else
                {
                    ShowMsg("û�в�ѯ������޷��������ݱ�", true);
                }
            }
        }

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void LoadParameters()
		{
            if (!this.Page.IsPostBack)
            {
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AccountALLHere"]))
                {
                    this.AccountALLHere = base.Server.UrlDecode(this.Page.Request.QueryString["AccountALLHere"]);
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CheckTime"]))
				{
					this.CheckTime = base.Server.UrlDecode(this.Page.Request.QueryString["CheckTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestTime"]))
				{
					this.RequestTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestTime"]);
				}
                this.txtAccountALLHere.Text = this.AccountALLHere;
				this.txtStoreName.Text = this.StoreName;
				this.txtRequestTime.Text = this.RequestTime;
				this.txtCheckTime.Text = this.CheckTime;
			}
            else
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                else
                {
                    this.StoreName = this.txtStoreName.Text;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AccountALLHere"]))
                {
                    this.AccountALLHere = base.Server.UrlDecode(this.Page.Request.QueryString["AccountALLHere"]);
                }
                else
                {
                    this.AccountALLHere=this.txtAccountALLHere.Text;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CheckTime"]))
                {
                    this.CheckTime = base.Server.UrlDecode(this.Page.Request.QueryString["CheckTime"]);
                }
                else
                {
                    this.CheckTime=this.txtCheckTime.Text;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestTime"]))
                {
                    this.RequestTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestTime"]);
                }
                else
                {
                    this.RequestTime=this.txtRequestTime.Text;
                }
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExportButton.Click += new System.EventHandler(this.btnExportButton_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("AccountALLHere", this.txtAccountALLHere.Text);
			queryStrings.Add("RequestTime", this.txtRequestTime.Text);
			queryStrings.Add("CheckTime", this.txtCheckTime.Text);
			queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}
	}
}
