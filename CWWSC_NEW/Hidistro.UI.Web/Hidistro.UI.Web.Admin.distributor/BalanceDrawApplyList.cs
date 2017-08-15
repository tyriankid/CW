using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using Hidistro.Entities.Store;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Excel;
using System.IO;
namespace Hidistro.UI.Web.Admin.distributor
{
    public class BalanceDrawApplyList : AdminPage
    {
        protected System.Web.UI.WebControls.Button btapply;
        protected System.Web.UI.WebControls.Button btnRefuse;//拒绝按钮
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportButton;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdapplyid;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdreferralblance;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hduserid;
        protected Pager pager;
        protected System.Web.UI.WebControls.Repeater reBalanceDrawRequest;
        private string RequestEndTime = "";
        private string RequestStartTime = "";
        private string StoreName = "";
        private string AccountALLHere = "";
        private string CheckTime = "";
        private string RequestTime = "";
        protected System.Web.UI.HtmlControls.HtmlTextArea txtcontent;
        protected WebCalendar txtRequestEndTime;
        protected WebCalendar txtRequestStartTime;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;

        private void BindData()
        {
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }

            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery
            {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                AccountALLHere=this.AccountALLHere,
                UserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "",
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SerialID",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "0",
                UserId = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity);
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
            strStorId = strStorId.TrimEnd(',');//去除最后一个 , 符号
            if (string.IsNullOrEmpty(strStorId))
                strStorId = "null";
            return strStorId;
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            int id = int.Parse(this.hdapplyid.Value);
            string remark = this.txtcontent.Value;
            int userId = int.Parse(this.hduserid.Value);
            decimal referralRequestBalance = decimal.Parse(this.hdreferralblance.Value);
            if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
            {
                if (VShopHelper.UpdateBalanceDistributors(userId, referralRequestBalance))
                {
                    this.ShowMsg("结算成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结算失败", false);
                }
            }
            else
            {
                this.ShowMsg("结算失败", false);
            }
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportButton_Click(object sender, System.EventArgs e)
        {
            bool isFiliale = false;//是否分公司
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.FilialeRoleId)
            {
                isFiliale = true;//当前登陆为分公司用户
            }

            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery();
            if (this.CheckTime != null)
                entity.CheckTime = this.CheckTime.ToString();
            if (this.RequestTime != null)
                entity.RequestTime = this.RequestTime.ToString();
            if (this.StoreName != null)
                entity.StoreName = this.StoreName.ToString();
            if (this.AccountALLHere != null)
                entity.AccountALLHere = this.AccountALLHere.ToString();
            entity.UserIds = isFiliale ? getStorIds(currentManager.ClientUserId) : "";
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "CheckTime";
            entity.RequestEndTime = "";
            entity.RequestStartTime = "";
            entity.IsCheck = "0";
            if (entity != null)
            {
                DataTable balanceDrawRequest = VShopHelper.GetExportBalanceDrawRequest(entity);
                if (balanceDrawRequest.Rows.Count >= 1)
                {
                    DataTable exportdata = new DataTable();
                    exportdata.Columns.Add("金力帐号");
                    exportdata.Columns.Add("门店名称");
                    exportdata.Columns.Add("申请提现金额");
                    exportdata.Columns.Add("可提现金额");
                    exportdata.Columns.Add("申请日期");
                    exportdata.Columns.Add("手机号码");
                    exportdata.Columns.Add("收款方式");
                    for (int i = 0; i < balanceDrawRequest.Rows.Count; i++)
                    {
                        exportdata.Rows.Add(
                            balanceDrawRequest.Rows[i]["AccountALLHere"].ToString(),
                            balanceDrawRequest.Rows[i]["StoreName"].ToString(),
                            balanceDrawRequest.Rows[i]["Amount"],
                            balanceDrawRequest.Rows[i]["ReferralBlance"].ToString(),
                            balanceDrawRequest.Rows[i]["RequestTime"].ToString(),  
                            balanceDrawRequest.Rows[i]["CellPhone"].ToString(),
                            formatPayType(int.Parse(balanceDrawRequest.Rows[i]["RequestType"].ToString()))
                            );
                    }


                    string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Resources\\xls\\balancedrawapplyxls\\申请结算信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    //ExcelDBClass excelDBClass = new ExcelDBClass(strPath, true);
                    ExcelDBClass exls = new ExcelDBClass(strPath, false);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(exportdata.Copy());
                    ds.Tables[0].TableName = "申请结算信息";
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

                    //System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
                    //fields.Add("金力帐号");
                    //list2.Add("金力帐号");
                    //fields.Add("门店名称");
                    //list2.Add("门店名称");
                    //fields.Add("申请提现金额");
                    //list2.Add("申请提现金额");
                    //fields.Add("可提现金额");
                    //list2.Add("可提现金额");
                    //fields.Add("申请日期");
                    //list2.Add("申请日期");
                    //fields.Add("手机号码");
                    //list2.Add("手机号码");
                    //fields.Add("收款方式");
                    //list2.Add("收款方式");

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
                    //this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=申请结算信息.csv");
                    //this.Page.Response.ContentType = "application/octet-stream";
                    //this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    //this.Page.EnableViewState = false;
                    //this.Page.Response.Write(builder.ToString());
                    //this.Page.Response.End();
                }
                else
                {
                    ShowMsg("没有查询结果，无法导出数据表", true);
                }
            }
        }

        /// <summary>
        /// 格式化支付方式,0:微信快捷支付,1:支付宝:2:银行
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public string formatPayType(int requestType)
        {
            string result = "";
            switch (requestType)
            {
                case 0:
                    result= "微信红包";
                    break;
                case 1:
                    result= "支付宝";
                    break;
                case 2:
                    result= "银行打款";
                    break;
            }
            return result;
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                this.txtAccountALLHere.Text = this.AccountALLHere;
                this.txtStoreName.Text = this.StoreName;
                this.txtRequestStartTime.Text = this.RequestStartTime;
                this.txtRequestEndTime.Text = this.RequestEndTime;
            }
            else
            {
                this.AccountALLHere = this.txtAccountALLHere.Text;
                this.StoreName = this.txtStoreName.Text;
                this.RequestStartTime = this.txtRequestStartTime.Text;
                this.RequestEndTime = this.txtRequestEndTime.Text;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btapply.Click += new System.EventHandler(this.btnApply_Click);
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
            queryStrings.Add("RequestStartTime", this.txtRequestStartTime.Text);
            queryStrings.Add("RequestEndTime", this.txtRequestEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }


        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int num = 0;
            int.TryParse(e.CommandArgument.ToString(), out num);


            if (num > 0)
            {
                string commandName = e.CommandName;
                string str = commandName;
                
                if (commandName != null)
                {
                    switch (str)
                    {
                        case "sendredpack":
                            if (str != "sendredpack")
                            {
                                return;
                            }
                            string balanceDrawRequest = DistributorsBrower.SendRedPackToBalanceDrawRequest(num);
                            string str1 = balanceDrawRequest;
                            string str2 = str1;
                            if (str1 != null)
                            {
                                if (str2 == "-1")
                                {
                                    base.Response.Redirect(string.Concat("SendRedpackRecord.aspx?serialid=", num));
                                    base.Response.End();
                                    return;
                                }
                                if (str2 == "1")
                                {
                                    base.Response.Redirect(string.Concat("SendRedpackRecord.aspx?serialid=", num));
                                    base.Response.End();
                                    return;
                                }
                            }
                            this.ShowMsg(string.Concat("生成红包失败，原因是：", balanceDrawRequest), false);
                            break;

                        case "refuse":
                                int id = Convert.ToInt32(e.CommandArgument);
                                string remark = "提现申请不通过";
                                if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
                                {
                                    this.ShowMsg("操作成功,该提现申请已被拒绝", true);
                                    this.BindData();
                                }
                                else
                                {
                                    this.ShowMsg("操作失败", false);
                                }
                            break;
                    }
                }
            }


        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                LinkButton linkButton = (LinkButton)e.Item.FindControl("lkBtnSendRedPack");
                int num = int.Parse(((DataRowView)e.Item.DataItem).Row["serialid"].ToString());
                if (int.Parse(((DataRowView)e.Item.DataItem).Row["RedpackRecordNum"].ToString()) > 0)
                {
                    linkButton.PostBackUrl = string.Concat("SendRedpackRecord.aspx?serialid=", num);
                    linkButton.Text = "查看微信红包";
                    return;
                }
                int num1 = int.Parse(((DataRowView)e.Item.DataItem).Row["RequestType"].ToString());
                linkButton.OnClientClick = "return confirm('提现金额将会拆分为最大金额为200元的微信红包，等待发送！确定生成微信红包吗？')";
                if (num1 == 0)
                {
                    linkButton.Style.Add("color", "red");
                }

                LinkButton refuse = (LinkButton)e.Item.FindControl("btnRefuse");

                refuse.OnClientClick = "return confirm('确认要拒绝【" + ((System.Data.DataRowView)(e.Item.DataItem)).Row["storename"] + "】的提款申请吗?')";
                }
            }
        }
    }

