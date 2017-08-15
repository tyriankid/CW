using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class SaleInfoTables : AdminPage
    {
        protected DropDownList ddlSenders;
        //protected Grid grdOrderAvPrice;
        //protected Grid grdOrderTranslatePercentage;
        //protected Grid grdUserOrderAvNumb;
        //protected Grid grdUserOrderPercentage;
        //protected Grid grdVisitOrderAvPrice;
        //protected void Page_Load(object sender, System.EventArgs e)
        //{
        //    if (!this.Page.IsPostBack)
        //    {
        //DbQueryResult saleTargets = SalesHelper.GetSaleTargets();
        //        this.grdOrderAvPrice.DataSource = saleTargets.Data;
        //        this.grdOrderAvPrice.DataBind();
        //        this.grdOrderTranslatePercentage.DataSource = saleTargets.Data;
        //        this.grdOrderTranslatePercentage.DataBind();
        //        this.grdUserOrderAvNumb.DataSource = saleTargets.Data;
        //        this.grdUserOrderAvNumb.DataBind();
        //        this.grdVisitOrderAvPrice.DataSource = saleTargets.Data;
        //        this.grdVisitOrderAvPrice.DataBind();
        //        this.grdUserOrderPercentage.DataSource = saleTargets.Data;
        //        this.grdUserOrderPercentage.DataBind();
        //    }
        //}
        protected Literal litTables;
        protected Button btnExport;
        protected Button btnProductTotalExport;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ddlSenders.SelectedIndexChanged += new EventHandler(this.ddlSenders_SelectedIndexChanged);
            this.btnExport.Click += new EventHandler(this.orderStatistics);
            this.btnProductTotalExport.Click += new EventHandler(this.btnProductTotalExport_click);
            if (!this.Page.IsPostBack)
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                if (ManagerHelper.GetRole(currentManager.RoleId).RoleName == "��������Ա")
                {
                    DataTable dtSenders = ManagerHelper.GetAllManagers();
                    foreach (DataRow row in dtSenders.Rows)
                    {
                        ListItem item = new ListItem
                        {
                            Text = row["UserName"].ToString() + "��" + row["agentName"].ToString(),
                            Value = row["UserId"].ToString(),
                        };
                        if (!string.IsNullOrEmpty(row["clientUserId"].ToString()) && row["clientUserId"].ToString() != "0")
                            ddlSenders.Items.Add(item);
                    }
                    ddlSenders.SelectedIndex = 0;
                    loadTables("","",Convert.ToInt32(ddlSenders.SelectedValue));
                }
                else
                {
                    ddlSenders.Items.Clear();
                    ListItem item = new ListItem
                    {
                        Text = currentManager.UserName + "��" + currentManager.AgentName,
                        Value = currentManager.UserId .ToString(),
                    };
                    ddlSenders.Items.Add(item);
                    ddlSenders.SelectedIndex = 0;
                    ddlSenders.Visible = false;
                    loadTables("","",currentManager.UserId);
                }

            }
        }

        void ddlSenders_SelectedIndexChanged(object sender, EventArgs e)
        {
            string starttime="";
            string endtime="";
            if (calendarStartDate.SelectedDate.HasValue)
            {
                starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }        
            loadTables(starttime, endtime,Convert.ToInt32(ddlSenders.SelectedValue));
        }

        private void loadTables(string StartTime="",string EndTime="",int sender=0)
        {
            DataSet ds = SalesHelper.GetSaleInfoTables(StartTime, EndTime,sender);
            /*
        <div class='back back2'>
            <div class='backtt clear'><h1>3</h1><p>��������</p></div>
            <ul class='three_row clear'>
                <li><a href=''><span>������</span><b>4</b></a></li>
                <li><a href=''><span>������</span><b>4</b></a></li>
                <li><a href=''><span>������</span><b>4</b></a></li>
                <li><a href=''><span>������</span><b>4</b></a></li>
                <li><a href=''><span>������</span><b>4</b></a></li>
                <li><a href=''><span>������</span><b>4</b></a></li>
            </ul>
        </div>
             */

            string htmls = string.Empty;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].Rows.Count == 0) continue;
                string tableName = string.Empty;
                switch (i)
                {
                    case 0:
                        tableName = "�ܼ�";
                        break;
                    case 1:
                        tableName = "PC��ͳ��";
                        break;
                    case 2:
                        tableName = "΢�Ŷ�ͳ��";
                        break;
                    case 3:
                        tableName = "�����ܼ�";
                        break;
                    case 4:
                        tableName = "����PC��ͳ��";
                        break;
                    case 5:
                        tableName = "����΢�Ŷ�ͳ��";
                        break;
                    case 6:
                        tableName = "pc��΢��ɨ��֧���ܼ�";
                        break;
                    case 7:
                        tableName = "����pc��΢��ɨ��֧��ͳ��";
                        break;
                }
                htmls += "<div class='back back" + (i + 1) + "'>";
                htmls += "<div class='backtt clear'><h1>��" + ((decimal)ds.Tables[i].Rows[0]["OrderTotal"]).ToString("F2") + "</h1><p>" + tableName + "</p></div>";
                htmls += "<ul class='three_row clear'>";


                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "�˺�", ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName);
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "��������", ds.Tables[i].Rows[0]["orderCount"]);
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "�����ܶ�", ((decimal)ds.Tables[i].Rows[0]["OrderTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "�Ż�ȯ�ܶ�", ((decimal)ds.Tables[i].Rows[0]["CouponTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "�Ż��ܶ�", ((decimal)ds.Tables[i].Rows[0]["DiscountTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "��Ʒ�ܼ���", ds.Tables[i].Rows[0]["productTotal"]);
                htmls += "</ul></div>";
            }
            litTables.Text = htmls;
        }

        /// <summary>
        /// ��Ʒ������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnProductTotalExport_click(object sender, EventArgs e)
        {
            string starttime = "";
            string endtime = ""; 
            //��ȡʱ���ֵ
            if(calendarStartDate.SelectedDate.HasValue){
                 starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }           
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }
            int send = Convert.ToInt32(ddlSenders.SelectedValue);
            //��ȡ����Դ
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            DataTable dt = new DataTable();
            DataTable data = SalesHelper.GetProductQuantity(starttime, endtime,send);                   
            fields.Add("ProductName");
            list2.Add("��Ʒ����");
            fields.Add("Quantity");
            list2.Add("��Ʒ������");           
            //���ѭ��
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (DataRow row in data.Rows)
            {
                foreach (string str2 in fields)
                {
                    if (row.Table.Rows[0]["" + str2 + ""] == null || row.Table.Rows[0]["" + str2 + ""].ToString() == "")
                    {
                        switch (str2)
                        {
                            case "ProductName":
                                builder.Append(row[str2] = 0).Append(",");
                                break;
                            case "Quantity":
                                builder.Append(row[str2] = 0).Append(",");
                                break;
                        }
                    }
                    else
                    {
                        builder.Append(row[str2]).Append(",");
                    }
                    if (str2 == fields[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            //ʱ���ʽ��
            DateTime dte = Convert.ToDateTime(starttime);
            DateTime dte2 = Convert.ToDateTime(endtime);
            string sj = dte.ToString("yyyy-MM-dd").Replace("-", "/"); 
            string sj2 = dte2.ToString("yyyy-MM-dd").Replace("-", "/"); 
            //���������
            string FileName = "" + ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName + "";
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + "" + sj + "" + sj2 + ".csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }

        /// <summary>
        /// ����Excel���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void orderStatistics(object sender, EventArgs e)
        {
            string starttime = "";
            string endtime = "";
            if (calendarStartDate.SelectedDate.HasValue)
            {
                starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }
            
            #region ѭ������
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();            
            DataSet orderInfo = SalesHelper.GetSaleInfoTables(starttime,endtime,Convert.ToInt32(ddlSenders.SelectedValue));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string tableName = string.Empty;
            fields.Add("tableName"); list2.Add("");
            fields.Add("orderCount"); list2.Add("��������");
            fields.Add("OrderTotal"); list2.Add("�����ܶ�");
            fields.Add("CouponTotal"); list2.Add("�Ż�ȯ�ܶ�");
            fields.Add("DiscountTotal"); list2.Add("�Ż��ܶ�");
            fields.Add("productTotal"); list2.Add("��Ʒ�ܼ���");
            #endregion
            for (int i = 0; i< orderInfo.Tables.Count; i++)
            {
                if (orderInfo.Tables[i].Rows.Count == 0) continue;
                switch (i)
                {
                   #region tableNameѭ��
                    case 0:
                        tableName = "�ܼ�";
                        break;
                    case 1:
                        tableName = "PC��ͳ��";
                        break;
                    case 2:
                        tableName = "΢�Ŷ�ͳ��";
                        break;
                    case 3:
                        tableName = "�����ܼ�";
                        break;
                    case 4:
                        tableName = "����PC��ͳ��";
                        break;
                    case 5:
                        tableName = "����΢�Ŷ�ͳ��";
                        break;
                    case 6:
                        tableName = "pc��΢��ɨ��֧���ܼ�";
                        break;
                    case 7:
                        tableName = "����pc��΢��ɨ��֧��ͳ��";
                        break;
                    #endregion
                }
            //���ѭ��
            DataTable dataTable = orderInfo.Tables[i];
            //������ͷ
            dataTable.Columns.Add("tableName");
            dataTable.Rows[0]["tableName"] = tableName;
            int rowNumber = dataTable.Rows.Count;
            if (rowNumber == 0)
               {
                 this.ShowMsg("û���κ����ݿ��Ե��뵽Excel�ļ���", false);
                 return;
               }
             foreach (string str in list2)
                {
                   builder.Append(str + ",");
                   if (str == list2[list2.Count - 1])
                      {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                       }
                }
             #region ����ֵѭ��,�����ݸ���builder
             foreach (DataRow row in dataTable.Rows)
             {
                 foreach (string str2 in fields)
                 {
                     if (row.Table.Rows[0]["" + str2 + ""] == null||row.Table.Rows[0]["" + str2 + ""].ToString() =="")
                     {
                         switch (str2)
                         {
                             #region ���ݷǿ��ж�
                             case "orderCount":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "OrderTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "CouponTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "DiscountTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "productTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             #endregion
                         }
                     }
                     else
                     {
                         builder.Append(row[str2]).Append(",");
                     }
                     if (str2 == fields[list2.Count - 1])
                     {
                         builder = builder.Remove(builder.Length - 1, 1);
                         builder.Append("\r\n");
                     }
                   }
             }
             #endregion
            }
            #region ͨ��builder����Excel
            string FileName = "" + ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName + "";
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
            #endregion

        }
    }
}

