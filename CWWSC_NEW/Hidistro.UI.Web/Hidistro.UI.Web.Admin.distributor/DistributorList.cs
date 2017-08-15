using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Hidistro.SaleSystem.Vshop;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using Hidistro.Entities.Store;
using System.Data;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using System.Text;
using Hidistro.ControlPanel.Excel;



namespace Hidistro.UI.Web.Admin.distributor
{
	public class DistributorList : AdminPage
	{
        
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportButton;
		private string CellPhone = "";
		protected System.Web.UI.WebControls.DropDownList DrGrade;
		protected System.Web.UI.WebControls.DropDownList DrStatus;
		private string Grade = "0";
		private string MicroSignal = "";
		protected Pager pager;
		private string RealName = "";
		protected System.Web.UI.WebControls.Repeater reDistributor;
		private string Status = "0";
		private string StoreName = "";
        private string AccountALLHere = "";
        private string IsAgent = "0";      
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtMicroSignal;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
        protected System.Web.UI.WebControls.HiddenField hiIsAgent;
        
        public string TypeTitle = "������";
        


		private void BindData()
		{
            bool isFiliale = false;//�Ƿ�ֹ�˾
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//��ǰ��½Ϊ�ֹ�˾�û�
            }

			DistributorsQuery entity = new DistributorsQuery
			{
				GradeId = int.Parse(this.Grade),
				StoreName = this.StoreName,
                AccountAllHere=this.AccountALLHere,
				CellPhone = this.CellPhone,
				RealName = this.RealName,
				MicroSignal = this.MicroSignal,
				ReferralStatus = int.Parse(this.Status),
                IsAgent = int.Parse(this.IsAgent),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
                StoreIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "",
				SortBy = "userid",           
			};
			Globals.EntityCoding(entity, true);
			DbQueryResult distributors = VShopHelper.GetDistributors(entity);
            //ViewState["entity"] = entity;
			this.reDistributor.DataSource = distributors.Data;
			this.reDistributor.DataBind();
			this.pager.TotalRecords = distributors.TotalRecords;
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

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
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

            DistributorsQuery entity = new DistributorsQuery();
            if (!string.IsNullOrEmpty(this.DrGrade.SelectedValue))
                entity.GradeId = int.Parse(this.DrGrade.SelectedValue);
            entity.StoreName = this.txtStoreName.Text;
            entity.AccountAllHere = this.txtAccountALLHere.Text;
            entity.CellPhone = this.txtCellPhone.Text;
            entity.RealName = this.txtRealName.Text;
            entity.MicroSignal = this.txtMicroSignal.Text;
            if (!string.IsNullOrEmpty(this.DrStatus.SelectedValue))
                entity.ReferralStatus = int.Parse(this.DrStatus.SelectedValue);
            if (!string.IsNullOrEmpty(this.hiIsAgent.Value))
                entity.IsAgent = int.Parse(this.hiIsAgent.Value);
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.StoreIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "";
            entity.SortBy = "userid";


            if(entity != null)
            {
                DataTable dtExport = VShopHelper.GetExprotDistrbutor(entity);
                if (dtExport.Rows.Count>=1)
                {
                    DataTable exportdata = new DataTable();
                    exportdata.Columns.Add("�ֹ�˾����");
                    exportdata.Columns.Add("��������");
                    exportdata.Columns.Add("�ŵ�״̬");
                    exportdata.Columns.Add("��֤ʱ��");
                    exportdata.Columns.Add("��ϵ��");
                    exportdata.Columns.Add("�ֻ�����");
                    exportdata.Columns.Add("QQ");
                    exportdata.Columns.Add("΢���ǳ�");
                    exportdata.Columns.Add("�����ʺ�");
                    for (int i = 0; i < dtExport.Rows.Count; i++)
                    {
                        exportdata.Rows.Add(
                            dtExport.Rows[i]["fgsName"].ToString(),
                            dtExport.Rows[i]["StoreName"].ToString(),
                            dtExport.Rows[i]["ReferralStatus"].ToString() == "0" ? "����" : "����",
                            dtExport.Rows[i]["CreateTime"].ToString(),
                            dtExport.Rows[i]["RealName"].ToString(),
                            dtExport.Rows[i]["CellPhone"].ToString(),
                            dtExport.Rows[i]["QQ"].ToString(),
                            dtExport.Rows[i]["UserName"].ToString(),
                            dtExport.Rows[i]["AccountAllHere"].ToString()
                            );
                    }

                    string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\distributorxls\\�ŵ���Ϣ" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                    ExcelDBClass exls = new ExcelDBClass(strPath, false);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(exportdata.Copy());
                    ds.Tables[0].TableName = "�ŵ���Ϣ";
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
                    //fields.Add("�ֹ�˾����");
                    //list2.Add("�ֹ�˾����");
                    //fields.Add("��������");
                    //list2.Add("��������");
                    //fields.Add("�ŵ�״̬");
                    //list2.Add("�ŵ�״̬");
                    //fields.Add("��ϵ��");
                    //list2.Add("��ϵ��");
                    //fields.Add("�ֻ�����");
                    //list2.Add("�ֻ�����");
                    //fields.Add("QQ");
                    //list2.Add("QQ");
                    //fields.Add("΢���ǳ�");
                    //list2.Add("΢���ǳ�");
                    //fields.Add("�����ʺ�");
                    //list2.Add("�����ʺ�");

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
                    //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=�ŵ���Ϣ.csv");
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
                    this.AccountALLHere = base.Page.Request.QueryString["AccountALLHere"];
                }
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
				{
					this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
				{
					this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
				{
					this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
				{
					this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
				{
					this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isagent"]))
                {
                    this.IsAgent = base.Server.UrlDecode(this.Page.Request.QueryString["isagent"]);
                }

                this.DrGrade.DataBind();

				//this.DrStatus.SelectedValue = this.Status;
				this.txtStoreName.Text = this.StoreName;
                this.txtAccountALLHere.Text=this.AccountALLHere;
                this.DrGrade.SelectedValue = this.Grade;
				this.txtCellPhone.Text = this.CellPhone;
				this.txtMicroSignal.Text = this.MicroSignal;
				this.txtRealName.Text = this.RealName;
                this.hiIsAgent.Value = this.IsAgent;
                //TypeTitle = (this.IsAgent == "1") ? "������" : "������";
                TypeTitle = "�ŵ�";
			}
			else
			{
				this.Status = this.DrStatus.SelectedValue;
				this.StoreName = this.txtStoreName.Text;
                this.AccountALLHere=this.txtAccountALLHere.Text;
				this.Grade = this.DrGrade.SelectedValue;
				this.CellPhone = this.txtCellPhone.Text;
				this.RealName = this.txtRealName.Text;
				this.MicroSignal = this.txtMicroSignal.Text;
                this.IsAgent = this.hiIsAgent.Value;
                //TypeTitle = (this.IsAgent == "1") ? "������" : "������";
                TypeTitle = "�ŵ�";
			}
		}

        //2015-8-20���:����˶���ͽⶳ����
        protected void reDistributor_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userid =Convert.ToInt32( e.CommandArgument);
            switch (e.CommandName)
            {
                case "Frozen":
                    DistributorsBrower.FrozenCommision(userid,"1");
                    break;
                case "UnFre":
                    DistributorsBrower.FrozenCommision(userid, "0");
                    break;
                case "ReSetPwd":
                    MemberProcessor.SetPwd(userid.ToString(), "888888");
                    break;
                case "Prom":
                    DistributorsBrower.Updateaspnet_DistributorsUserId(userid);
                    break;
                case"Store":
                    DistributorsBrower.Updateaspnet_DistributorsServiceStoreId(userid);//�����ŵ�
                    break;
                case"Tore":
                    DistributorsBrower.Updateaspnet_DistributorsServiceToreId(userid);//������
                    break;
                case "Delete":
                    if (DistributorsBrower.DeleteDistributor(userid))
                    {
                        this.ShowMsg("ɾ���ɹ�!", true);
                        return;
                    }
                    else
                    {
                        this.ShowMsg("�÷����̰����������޷�ɾ��!", false);
                        return;
                    }
                    break;
                case "Down":
                    /*
                    string str = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";

                    string path = Server.MapPath("/Storage/temp/" + userid + ".jpg");
                    string imgURL = string.Format("{0}/API/GetQRCode.ashx?code={0}/Vshop/Default.aspx?ReferralId={1}", Globals.HostPath(System.Web.HttpContext.Current.Request.Url), userid);
                    */
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string savepath = System.Web.HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("distributor_{0}", userid) + ".jpg";
                    if (!File.Exists(savepath))
                    {
                        Hishop.Weixin.MP.Api.TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("distributor_{0}", userid), false);
                    }
                    string qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("distributor_{0}", userid) + ".jpg";
                    DownFile(this, savepath);
                    break;
                case "SetNeigou":
                    DistributorsBrower.UpdateDistributorsSetNeiGouStore(userid);//ȡ���ڹ��ŵ�
                    break;
                case "CancelNeigou":
                    DistributorsBrower.UpdateDistributorsCancelNeiGouStore(userid);//�����ڹ��ŵ�
                    break;
            }

            
            if (e.CommandName == "ReSetPwd")
            {
                string uName = MemberProcessor.GetMember(userid).UserName;
                NameValueCollection queryStrings = new NameValueCollection();
                queryStrings.Add("Grade", this.DrGrade.SelectedValue);
                queryStrings.Add("StoreName", this.txtStoreName.Text);
                queryStrings.Add("CellPhone", this.txtCellPhone.Text);
                queryStrings.Add("RealName", this.txtRealName.Text);
                queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
                queryStrings.Add("Status", this.DrStatus.SelectedValue);
                queryStrings.Add("IsAgent", this.hiIsAgent.Value);
                queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
                string urlPage = base.GetPage(queryStrings);
                this.ShowMsgAndReUrl("���óɹ�!<br/>" + uName + "������ĿǰΪ 888888����֪ͨ���û������޸����룡", true, urlPage);
                return;
                
            }
            this.ReBind(true);
      
        }

        public void DownFile(System.Web.UI.Page page, string path)
        {
            try
            {
                System.IO.FileInfo myFile = new System.IO.FileInfo(path);
                page.Response.Clear();
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(myFile.Name));
                page.Response.AddHeader("Content-Length", myFile.Length.ToString());
                page.Response.ContentType = "application/octet-stream";
                page.Response.TransmitFile(myFile.FullName);
                page.Response.End();
            }
            catch
            {
                this.ShowMsg("�����ļ�ʱ�������󣡿������ļ������ڻ��߱�����Աɾ����",false);
            }
        }

        public void DownImgUrl(string path,string url)
        {
            WebRequest wreq = WebRequest.Create(url);
            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
            Stream s = wresp.GetResponseStream();
            System.Drawing.Image img;
            img = System.Drawing.Image.FromStream(s);
            img.Save(path, ImageFormat.Jpeg);   //����
            //����ֱ�����
            /*MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Gif);
            img.Dispose();
            Response.ClearContent();
            Response.ContentType = "image/gif";
            Response.BinaryWrite(ms.ToArray());
            */
            DownFile(this,path);
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExportButton.Click += new System.EventHandler(this.btnExportButton_Click);
			this.LoadParameters();

            if (!Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto && Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableAgentProductRange)
            {
                ViewState["IsSetProductRange"] = "1";
            }
            else
            {
                ViewState["IsSetProductRange"] = "0";
            }
            
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("Grade", this.DrGrade.SelectedValue);
			queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("AccountALLHere", this.txtAccountALLHere.Text);
			queryStrings.Add("CellPhone", this.txtCellPhone.Text);
			queryStrings.Add("RealName", this.txtRealName.Text);
			queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
			queryStrings.Add("Status", this.DrStatus.SelectedValue);
            queryStrings.Add("IsAgent", this.hiIsAgent.Value);
			queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}
	}
}
