using ASPNET.WebControls;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
    public class StoreMemberStatis : AdminPage
	{
		private int i;
		protected System.Web.UI.WebControls.Repeater reDistributor;
        protected ExportFieldsCheckBoxList3 exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;

        protected System.Web.UI.WebControls.TextBox txtSearchkey;
        protected WebCalendar txtStartTime;
        protected WebCalendar txtEndTime;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExport;

        private string searchKey;
        private string stateTime;
        private string endTime;

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

            GetMemberQuery();//得到并设置查询值
            string where1 = "1=1";
            string where2 = "1=1";
            string where3 = "1=1";
            string where4 = "1=1";
            string where5 = "1=1";
            if (!string.IsNullOrEmpty(searchKey))
            {
                where5 += string.Format(" and (s.accountALLHere like '%{0}%' or s.storeName like '%{0}%' )", DataHelper.CleanSearchString(searchKey));
            }
            if (isFiliale)
            {
                if (currentManager.ClientUserId > 0)
                    where5 += string.Format(" and s.fgsid = {0}", currentManager.ClientUserId);
            }
            if (!string.IsNullOrEmpty(stateTime))
            {
                where1 += string.Format(" and CreateDate >= '{0}'", stateTime);
                where2 += string.Format(" and om.buydate >= '{0}'", stateTime);
                where3 += string.Format(" and CreateDate >= '{0}'", stateTime);
                where4 += string.Format(" and e.buydate >= '{0}'", stateTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where1 += string.Format(" and CreateDate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                where2 += string.Format(" and om.buydate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                where3 += string.Format(" and CreateDate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                where4 += string.Format(" and e.buydate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
            }
            DataTable data = VShopHelper.GetStoreStatis(where1, where2, where3, where4, where5);
            this.reDistributor.DataSource = data;
            this.reDistributor.DataBind();
            this.txtSearchkey.Text = searchKey;
            this.txtStartTime.Text = stateTime;
            this.txtEndTime.Text = endTime;
        }

        private void GetMemberQuery()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
            {
                searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["stateTime"]))
            {
                stateTime = base.Server.UrlDecode(this.Page.Request.QueryString["stateTime"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
            {
                endTime = base.Server.UrlDecode(this.Page.Request.QueryString["endTime"]);
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.reDistributor.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void reDistributor_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litph");
                i++;
                literal.Text = "<span style=\"padding-left:10px;\">" + (int.Parse(literal.Text) + this.i).ToString() + "</span>";
			}
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

                //2016-08-08验证当前登陆用户类型
                bool isFiliale = false;//是否分公司
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (currentManager.RoleId == masterSettings.FilialeRoleId)
                {
                    isFiliale = true;//当前登陆为分公司用户
                }


                GetMemberQuery();//得到并设置查询值
                string where1 = "1=1";
                string where2 = "1=1";
                string where3 = "1=1";
                string where4 = "1=1";
                string where5 = "1=1";
                if (!string.IsNullOrEmpty(searchKey))
                {
                    where5 += string.Format(" and (s.accountALLHere like '%{0}%' or s.storeName like '%{0}%' )", DataHelper.CleanSearchString(searchKey));
                }
                if (isFiliale)
                {
                    if (currentManager.ClientUserId > 0)
                        where5 += string.Format(" and s.fgsid = {0}", currentManager.ClientUserId);
                }
                if (!string.IsNullOrEmpty(stateTime))
                {
                    where1 += string.Format(" and CreateDate >= '{0}'", stateTime);
                    where2 += string.Format(" and om.buydate >= '{0}'", stateTime);
                    where3 += string.Format(" and CreateDate >= '{0}'", stateTime);
                    where4 += string.Format(" and e.buydate >= '{0}'", stateTime);
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    where1 += string.Format(" and CreateDate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                    where2 += string.Format(" and om.buydate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                    where3 += string.Format(" and CreateDate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                    where4 += string.Format(" and e.buydate < '{0}'", Convert.ToDateTime(this.endTime).AddDays(1).ToString("yyyy-MM-dd"));
                }
                DataTable data = VShopHelper.GetStoreStatis(where1, where2, where3, where4, where5, fields);

                string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\allmemberxls\\门店会员统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                ExcelDBClass exls = new ExcelDBClass(strPath, false);
                DataSet ds = new DataSet();
                ds.Tables.Add(data.Copy());
                ds.Tables[0].TableName = "门店会员统计";
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

        private void ReloadMemberLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", System.Convert.ToString(this.txtSearchkey.Text));
            queryStrings.Add("stateTime", System.Convert.ToString(this.txtStartTime.Text));
            queryStrings.Add("endTime", System.Convert.ToString(this.txtEndTime.Text));
            base.ReloadPage(queryStrings);
        }

	}
}
