using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.distributor
{
    public class FilialeStoreListDetails : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportDetail;
        protected System.Web.UI.WebControls.Button btnExportDetailAll;
        protected System.Web.UI.WebControls.Button btnSubmint;
        private string fgsId = "";
        private string StoreName="";
        private string AccountALLHere = "";
        private string StartTime = "";
        private string EndTime = "";
        //protected Pager pager;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
        protected WebCalendar txtStartTime;
        protected WebCalendar txtEndTime;
        protected System.Web.UI.WebControls.Repeater reList;

        public string FilialeName;
        public string ParentPage;
        private void BindData()
        {
            string where1 = " 1=1 ";
            string where2 = "";
            string where3 = "";
            string where4 = " 1=1 ";
            if (!string.IsNullOrEmpty(this.fgsId))
                where4 += string.Format(" and a.fgsid = {0}", this.fgsId);
            if (!string.IsNullOrEmpty(this.StoreName))
                where4 += string.Format(" and a.storeName like '%{0}%'", DataHelper.CleanSearchString(this.StoreName));
            if (!string.IsNullOrEmpty(this.AccountALLHere))
                where4 += string.Format(" and a.accountALLHere like '%{0}%'", DataHelper.CleanSearchString(this.AccountALLHere));
            if (!string.IsNullOrEmpty(this.StartTime))
            {
                where1 += string.Format(" and m.CreateDate >= '{0}'", this.StartTime);
                where2 += string.Format(" and o.OrderDate >= '{0}'", this.StartTime);
                where3 += string.Format(" and cm.TradeTime >= '{0}'", this.StartTime);
            }
            if (!string.IsNullOrEmpty(this.EndTime))
            {
                where1 += string.Format(" and m.CreateDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where2 += string.Format(" and o.OrderDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where3 += string.Format(" and cm.TradeTime <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
            }
            DataTable FilialeStoreData = VShopHelper.GetStoreStatisticsByWhere(where1, where2, where3, where4);
            this.reList.DataSource = FilialeStoreData;
            this.reList.DataBind();
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))                
                    this.fgsId = base.Server.UrlDecode(this.Page.Request.QueryString["id"]);
                else
                    ShowMsg("获取不到分公司信息，请重试", true);
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FilialeName"]))
                    this.FilialeName = this.Page.Request.QueryString["FilialeName"];
                else
                    ShowMsg("获取不到分公司信息，请重试", true);
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ParentPage"]))
                    this.ParentPage = this.Request.QueryString["ParentPage"];
                else
                    this.ParentPage = "1";
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                else
                    this.StoreName = null;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AccountALLHere"]))
                    this.AccountALLHere = base.Server.UrlDecode(this.Page.Request.QueryString["AccountALLHere"]);
                else
                    this.AccountALLHere = null;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                else
                    this.StartTime = null;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                else
                    this.EndTime = null;

                this.txtStoreName.Text = this.StoreName;
                this.txtAccountALLHere.Text = this.AccountALLHere;
                this.txtStartTime.Text = this.StartTime;
                this.txtEndTime.Text = this.EndTime;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
                    this.fgsId = base.Server.UrlDecode(this.Page.Request.QueryString["id"]);
                else
                    ShowMsg("获取不到分公司信息，请重试", true);
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FilialeName"]))
                    this.FilialeName = this.Page.Request.QueryString["FilialeName"];
                else
                    ShowMsg("获取不到分公司信息，请重试", true);
                this.StoreName = this.txtStoreName.Text;
                this.AccountALLHere = this.txtAccountALLHere.Text;
                this.StartTime = this.txtStartTime.Text;
                this.EndTime = this.txtEndTime.Text;
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        protected void btnSubmint_Click(object sender, EventArgs e)
        {
            Response.Redirect("FilialeStatistics.aspx?pageindex="+ParentPage);
        }

        protected void btnExportDetail_Click(object sender, EventArgs e)
        {
            string where1 = " 1=1 ";
            string where2 = "";
            string where3 = "";
            string where4 = " 1=1 ";
            if (!string.IsNullOrEmpty(this.fgsId))
                where4 += string.Format(" and a.fgsid = {0}", this.fgsId);
            if (!string.IsNullOrEmpty(this.StoreName))
                where4 += string.Format(" and a.storeName like '%{0}%'", DataHelper.CleanSearchString(this.StoreName));
            if (!string.IsNullOrEmpty(this.AccountALLHere))
                where4 += string.Format(" and a.accountALLHere like '%{0}%'", DataHelper.CleanSearchString(this.AccountALLHere));
            if (!string.IsNullOrEmpty(this.StartTime))
            {
                where1 += string.Format(" and m.CreateDate >= '{0}'", this.StartTime);
                where2 += string.Format(" and o.OrderDate >= '{0}'", this.StartTime);
                where3 += string.Format(" and cm.TradeTime >= '{0}'", this.StartTime);
            }
            if (!string.IsNullOrEmpty(this.EndTime))
            {
                where1 += string.Format(" and m.CreateDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where2 += string.Format(" and o.OrderDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where3 += string.Format(" and cm.TradeTime <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
            }
            DataTable FilialeStoreListData = VShopHelper.GetStoreStatisticsByWhere(where1, where2, where3, where4);
            if (FilialeStoreListData.Rows.Count >= 1)
            {
                DataTable exportdata = new DataTable();
                exportdata.Columns.Add("分公司名称");
                exportdata.Columns.Add("DZ账号");
                exportdata.Columns.Add("门店名称");
                exportdata.Columns.Add("佣金总额");
                exportdata.Columns.Add("有效订单数");
                exportdata.Columns.Add("订单总额");
                exportdata.Columns.Add("用户数");
                for (int i = 0; i < FilialeStoreListData.Rows.Count; i++)
                {
                    exportdata.Rows.Add(
                        FilialeStoreListData.Rows[i]["fgsName"].ToString(),
                        FilialeStoreListData.Rows[i]["accountALLHere"],
                        FilialeStoreListData.Rows[i]["StoreName"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["CommTotal"].ToString()) ? "0.00" : FilialeStoreListData.Rows[i]["CommTotal"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["OrderCount"].ToString()) ? "0" : FilialeStoreListData.Rows[i]["OrderCount"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["OrderTotal"].ToString()) ? "0.00" : FilialeStoreListData.Rows[i]["OrderTotal"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["UserCount"].ToString()) ? "0" : FilialeStoreListData.Rows[i]["UserCount"].ToString()
                        );
                }
                string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\fgsstorexls\\分公司门店统计信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExcelDBClass exls = new ExcelDBClass(strPath, false);
                DataSet ds = new DataSet();
                ds.Tables.Add(exportdata.Copy());
                ds.Tables[0].TableName = "分公司门店统计信息";
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
            else
            {
                ShowMsg("没有查询结果，无法导出数据表", true);
            }
        }

        /// <summary>
        /// 导出所有门店明细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportDetailAll_Click(object sender, EventArgs e)
        {
            string where1 = " 1=1 ";
            string where2 = "";
            string where3 = "";
            string where4 = " 1=1 ";
            if (!string.IsNullOrEmpty(this.StoreName))
                where4 += string.Format(" and a.storeName like '%{0}%'", DataHelper.CleanSearchString(this.StoreName));
            if (!string.IsNullOrEmpty(this.AccountALLHere))
                where4 += string.Format(" and a.accountALLHere like '%{0}%'", DataHelper.CleanSearchString(this.AccountALLHere));
            if (!string.IsNullOrEmpty(this.StartTime))
            {
                where1 += string.Format(" and m.CreateDate >= '{0}'", this.StartTime);
                where2 += string.Format(" and o.OrderDate >= '{0}'", this.StartTime);
                where3 += string.Format(" and cm.TradeTime >= '{0}'", this.StartTime);
            }
            if (!string.IsNullOrEmpty(this.EndTime))
            {
                where1 += string.Format(" and m.CreateDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where2 += string.Format(" and o.OrderDate <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
                where3 += string.Format(" and cm.TradeTime <= '{0}'", Convert.ToDateTime(this.EndTime).AddDays(1).ToShortDateString());
            }
            DataTable FilialeStoreListData = VShopHelper.GetStoreStatisticsByWhere(where1, where2, where3, where4);
            if (FilialeStoreListData.Rows.Count >= 1)
            {
                DataTable exportdata = new DataTable();
                exportdata.Columns.Add("分公司名称");
                exportdata.Columns.Add("DZ账号");
                exportdata.Columns.Add("门店名称");
                exportdata.Columns.Add("佣金总额");
                exportdata.Columns.Add("有效订单数");
                exportdata.Columns.Add("订单总额");
                exportdata.Columns.Add("用户数");
                for (int i = 0; i < FilialeStoreListData.Rows.Count; i++)
                {
                    exportdata.Rows.Add(
                        FilialeStoreListData.Rows[i]["fgsName"].ToString(),
                        FilialeStoreListData.Rows[i]["accountALLHere"],
                        FilialeStoreListData.Rows[i]["StoreName"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["CommTotal"].ToString()) ? "0.00" : FilialeStoreListData.Rows[i]["CommTotal"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["OrderCount"].ToString()) ? "0" : FilialeStoreListData.Rows[i]["OrderCount"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["OrderTotal"].ToString()) ? "0.00" : FilialeStoreListData.Rows[i]["OrderTotal"].ToString(),
                        string.IsNullOrEmpty(FilialeStoreListData.Rows[i]["UserCount"].ToString()) ? "0" : FilialeStoreListData.Rows[i]["UserCount"].ToString()
                        );
                }
                string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\fgsstorexls\\门店统计信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExcelDBClass exls = new ExcelDBClass(strPath, false);
                DataSet ds = new DataSet();
                ds.Tables.Add(exportdata.Copy());
                ds.Tables[0].TableName = "门店统计信息";
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
            else
            {
                ShowMsg("没有查询结果，无法导出数据表", true);
            }
        }

        private string getStorIds(int fgsid)
        {
            string strStorId = string.Empty;
            DataTable dtDistributors = StoreInfoHelper.SelectStoreClientUserIdByFgsId(fgsid);
            foreach (DataRow dr in dtDistributors.Rows)
            {
                strStorId += dr["StoreId"].ToString() + ",";
            }
            strStorId = strStorId.TrimEnd(',');//去除最后一个 , 符号
            if (string.IsNullOrEmpty(strStorId))
                strStorId = "null";
            return strStorId;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.reList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.reList_ItemDataBound);
            this.btnSubmint.Click += new System.EventHandler(this.btnSubmint_Click);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExportDetail.Click += new System.EventHandler(this.btnExportDetail_Click);
            this.btnExportDetailAll.Click += new System.EventHandler(this.btnExportDetailAll_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private int i;
        private void reList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litph");
                this.i++;
                literal.Text = "<span style=\"padding-left:10px;\">" + (int.Parse(literal.Text) + this.i).ToString() + "</span>";
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("id", this.fgsId.ToString());
            queryStrings.Add("FilialeName", this.FilialeName.ToString());
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("AccountALLHere", this.txtAccountALLHere.Text);
            queryStrings.Add("StartTime", this.txtStartTime.Text);
            queryStrings.Add("EndTime", this.txtEndTime.Text);
            //queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //if (!isSearch)
            //{
            //    queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //}
            base.ReloadPage(queryStrings);
        }
    }
}
