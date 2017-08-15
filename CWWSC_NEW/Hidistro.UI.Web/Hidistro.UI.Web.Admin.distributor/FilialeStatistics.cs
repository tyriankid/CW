using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Sales;
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
    public class FilialeStatistics : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportDetail;
        protected WebCalendar txtStartTime;
        protected WebCalendar txtEndTime;
		protected Pager pager;
        protected System.Web.UI.WebControls.Repeater reList;
        protected System.Web.UI.WebControls.TextBox txtFgsName;
        private string FilialeName;
        private string StartTime;
        private string EndTime;

        private void BindData(string where1, string where2, string where3, string where4)
		{
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                where4 += string.Format(" AND a.id = '{0}'", currentManager.ClientUserId);
            }
            
            FilialeStatisticsQuery entity = new FilialeStatisticsQuery()
            {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
            };
            int count = 0;
            DataTable dtFs = SalesHelper.GetFilialeStatisticsPage(entity, where1, where2, where3, where4, out count);
            this.reList.DataSource = dtFs;
            this.reList.DataBind();
            this.pager.TotalRecords = count;
		}

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FilialeName"]))
                {
                    this.FilialeName = base.Server.UrlDecode(this.Page.Request.QueryString["FilialeName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                {
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                {
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                }
                this.txtFgsName.Text = this.FilialeName;
                this.txtStartTime.Text = this.StartTime;
                this.txtEndTime.Text = this.EndTime;
            }
            else
            {
                this.FilialeName = this.txtFgsName.Text;
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
                string where1 = " 1=1 ";
                string where2 = " 1=1 ";
                string where3 = " 1=1 ";
                string where4 = " 1=1 ";
                if (!string.IsNullOrEmpty(this.FilialeName))
                {
                    where4 += string.Format(" AND a.fgsName like '%{0}%'", DataHelper.CleanSearchString(this.FilialeName));
                }
                if (!string.IsNullOrEmpty(this.StartTime))
                {
                    where1 += string.Format(" AND c.TradeTime >= '{0}'", this.StartTime);
                    where2 += string.Format(" AND o.OrderDate >= '{0}'", this.StartTime);
                    where3 += string.Format(" AND m.CreateDate >= '{0}'", this.StartTime);
                }
                if (!string.IsNullOrEmpty(this.EndTime))
                {
                    where1 += string.Format(" AND c.TradeTime < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                    where2 += string.Format(" AND o.OrderDate < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                    where3 += string.Format(" AND m.CreateDate < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                }
                this.BindData(where1, where2, where3, where4);
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadPage(true);
        }

        protected void btnExportDetail_Click(object sender, EventArgs e)
        {
            string strWhere1 = " 1=1 ";
            string strWhere2 = " 1=1 ";
            string strWhere3 = " 1=1 ";
            string strWhere4 = " 1=1 ";
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                strWhere4 += string.Format(" AND a.id = '{0}'", currentManager.ClientUserId);
            }
            if (!string.IsNullOrEmpty(this.FilialeName))
            {
                strWhere4 += string.Format(" AND a.fgsName like '%{0}%'", this.FilialeName);
            }
            if (!string.IsNullOrEmpty(this.StartTime))
            {
                strWhere1 += string.Format(" AND c.TradeTime >= '{0}'", this.StartTime);
                strWhere2 += string.Format(" AND o.OrderDate >= '{0}'", this.StartTime);
                strWhere3 += string.Format(" AND m.CreateDate >= '{0}'", this.StartTime);
            }
            if (!string.IsNullOrEmpty(this.EndTime))
            {
                strWhere1 += string.Format(" AND c.TradeTime < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                strWhere2 += string.Format(" AND o.OrderDate < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                strWhere3 += string.Format(" AND m.CreateDate < '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToString("yyyy-MM-dd"));
            }

            FilialeStatisticsQuery entity = new FilialeStatisticsQuery()
            {
                PageIndex = 1,
                PageSize = 9999999,
            };
            int count = 0;
            DataTable filialeStatistics = SalesHelper.GetFilialeStatisticsPage(entity, strWhere1, strWhere2, strWhere3, strWhere4, out count);
            if (filialeStatistics.Rows.Count >= 1)
            {
                DataTable exportdata = new DataTable();
                exportdata.Columns.Add("分公司名称");
                exportdata.Columns.Add("认证门店数");
                exportdata.Columns.Add("佣金总额");
                exportdata.Columns.Add("粉丝数");
                exportdata.Columns.Add("订单总数");
                exportdata.Columns.Add("订单总额");
                for (int i = 0; i < filialeStatistics.Rows.Count; i++)
                {
                    exportdata.Rows.Add(
                        filialeStatistics.Rows[i]["fgsName"].ToString(),
                        filialeStatistics.Rows[i]["storeNum"].ToString(),
                        filialeStatistics.Rows[i]["commTotal"].ToString(),
                        filialeStatistics.Rows[i]["UserNum"].ToString(),
                        filialeStatistics.Rows[i]["orderNum"].ToString(),
                         filialeStatistics.Rows[i]["orderTotal"].ToString()
                        );
                }

                string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\fgsxls\\分公司统计信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                ExcelDBClass exls = new ExcelDBClass(strPath, false);
                DataSet ds = new DataSet();
                ds.Tables.Add(exportdata.Copy());
                ds.Tables[0].TableName = "分公司统计信息";
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

        private void ReloadPage(bool flag)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("FilialeName", this.txtFgsName.Text);
            queryStrings.Add("StartTime", this.txtStartTime.Text);
            queryStrings.Add("EndTime", this.txtEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!flag)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        //private void ReBind(bool isSearch)
        //{
        //    string name = this.txtFgsName.Text;
        //    string where1 = "1=1";
        //    string where2 = "1=1";
        //    if (!string.IsNullOrEmpty(this.txtStartTime.Text))
        //    {
        //         where1 += " AND c.TradeTime>" + "'"+this.txtStartTime.Text+"'";
        //         where2 += " AND m.CreateDate>" + "'" + this.txtStartTime.Text + "'";
        //    }
        //    if (!string.IsNullOrEmpty(this.txtEndTime.Text))
        //    {
        //        where1 += " AND c.TradeTime<" + "'" + this.txtEndTime.Text + "'";
        //        where2 += " AND m.CreateDate<" + "'" + this.txtEndTime.Text + "'";
        //    }
        //    this.BindData(where1, where2, name);
        //}
    }
}
