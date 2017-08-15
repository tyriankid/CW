using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
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
	public class CommissionsAllList : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportDetail;
		private string EndTime = "";
		private string OrderId = "";
		protected Pager pager;
		protected System.Web.UI.WebControls.Repeater reCommissions;
		private string StartTime = "";
		private string StoreName = "";
        private string AccountALLHere = "";
        private string FgsName = "";
		protected WebCalendar txtEndTime;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected WebCalendar txtStartTime;
		protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtFgsName;
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

			CommissionsQuery entity = new CommissionsQuery
			{
				EndTime = this.EndTime,
				StartTime = this.StartTime,
                FgsName = this.FgsName,
				StoreName = this.StoreName,
                AccountALLHere=this.AccountALLHere,
				OrderNum = this.OrderId,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
                DistributorsUserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "",
				SortBy = "CommId"
			};
			Globals.EntityCoding(entity, true);
			DbQueryResult commissions = VShopHelper.GetCommissions(entity);
			this.reCommissions.DataSource = commissions.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = commissions.TotalRecords;
		}

        //�õ��ֹ�˾�������ŵ�ǰ���û�ID
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

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FgsName"]))
                {
                    this.FgsName = base.Server.UrlDecode(this.Page.Request.QueryString["FgsName"]);
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AccountALLHere"]))
                {
                    this.AccountALLHere = base.Server.UrlDecode(this.Page.Request.QueryString["AccountALLHere"]);
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
				{
					this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
				{
					this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
				{
					this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
				}
                this.txtFgsName.Text = this.FgsName;
				this.txtStoreName.Text = this.StoreName;
                this.txtAccountALLHere.Text = this.AccountALLHere;
				this.txtOrderId.Text = this.OrderId;
				this.txtStartTime.Text = this.StartTime;
				this.txtEndTime.Text = this.EndTime;
			}
			else
			{
				this.OrderId = this.txtOrderId.Text;
                this.FgsName = this.txtFgsName.Text;
				this.StoreName = this.txtStoreName.Text;
                this.AccountALLHere = this.txtAccountALLHere.Text;
				this.StartTime = this.txtStartTime.Text;
				this.EndTime = this.txtEndTime.Text;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExportDetail.Click += new System.EventHandler(this.btnExportDetail_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("FgsName", this.txtFgsName.Text);
			queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("AccountALLHere", this.txtAccountALLHere.Text);
			queryStrings.Add("OrderId", this.txtOrderId.Text);
			queryStrings.Add("StartTime", this.txtStartTime.Text);
			queryStrings.Add("EndTime", this.txtEndTime.Text);
			queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}

        protected void btnExportDetail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FgsName"]))
            {
                this.FgsName = base.Server.UrlDecode(this.Page.Request.QueryString["FgsName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
            {
                this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AccountALLHere"]))
            {
                this.AccountALLHere = base.Server.UrlDecode(this.Page.Request.QueryString["AccountALLHere"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
            {
                this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
            }

            string strStoreIds = string.Empty;
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                strStoreIds = getStorIds(currentManager.ClientUserId);//��ǰ��½Ϊ�ֹ�˾�û�
            }

            DataTable dt = OrderHelper.GetCommissionDetails(this.OrderId, this.FgsName, this.StoreName,this.AccountALLHere, this.StartTime, this.EndTime, strStoreIds);

            string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\commissionsallxls\\Ӷ�������ϸ" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
            ExcelDBClass exls = new ExcelDBClass(strPath, false);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ds.Tables[0].TableName = "Ӷ�������ϸ";
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
            //fields.Add("OrderId");
            //list2.Add("�������");

            //fields.Add("fgsName");
            //list2.Add("�ֹ�˾");

            //fields.Add("accountALLHere");
            //list2.Add("AllHere�˺�");

            //fields.Add("storename");
            //list2.Add("�ŵ�����");

            //fields.Add("L1");
            //list2.Add("�ŵ�Ӷ��");

            //fields.Add("productname");
            //list2.Add("��Ʒ��");

            //fields.Add("itemAdjustedPrice");
            //list2.Add("��Ʒ����");

            //fields.Add("Quantity");
            //list2.Add("��Ʒ����");

            //fields.Add("Time");
            //list2.Add("ʱ��");

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
            //foreach (System.Data.DataRow row in dt.Rows)
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
            //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=Ӷ�������ϸ.csv");
            //this.Page.Response.ContentType = "application/octet-stream";
            //this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //this.Page.EnableViewState = false;
            //this.Page.Response.Write(builder.ToString());
            //this.Page.Response.End();
        }
	}
}
