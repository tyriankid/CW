using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_store_ImportFileStore : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUnPack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(fileUpload.FileName) && fileUpload.FileContent.Length > 0)
        {
            try
            {
                string filePath = "";
                filePath = MapPath("/Storage/temp/") + "yyyyMMddHHmmss" + fileUpload.FileName;
                fileUpload.SaveAs(filePath);
                if (Path.GetExtension(filePath).ToLower() != ".xls")
                {
                    this.ShowMsg("导入的文件不是xls文件。", false);
                    return;
                }
                ExcelDBClass excelDBClass = new ExcelDBClass(filePath, true);
                //Excel转为DataTable   将选中的表格转换为datatable
                DataTable dtStoreInfoExcel = excelDBClass.ExportToDataSet().Tables[0];
                //表头格式验证
                string strHeaders = "所属分公司,门店名称,负责人,联系电话,金力账号,排序";
                //将表头字符串用,切割，装到arrayHeader数组
                string[] arrayHeader = strHeaders.Split(',');
                //循环表头与选中的表格里面标题是否一致
                for (int i = 0; i < dtStoreInfoExcel.Columns.Count; i++)
                {
                    if (dtStoreInfoExcel.Columns[i].ColumnName != arrayHeader[i])
                    {
                        this.ShowMsg("导入的文件格式不正确,请检查模板文件。", false);
                        return;
                    }
                }

                //获取数据库数据 转成DataTable
                string strSql = "select * from CW_StoreInfo";
                string strfgsSql = "select * from CW_Filiale";
                DataTable dtStoreInfo = DataBaseHelper.GetDataSet(strSql).Tables[0];
                DataTable dtfgsInfo = DataBaseHelper.GetDataSet(strfgsSql).Tables[0];
                bool isOk = true;
                DataRow[] blank = dtStoreInfoExcel.Select("门店名称 is null");
                for (int i = 0; i < blank.Count(); i++)
                {
                    blank[i].Delete();
                }
                dtStoreInfoExcel.AcceptChanges();
                DataTable dtResult = dtStoreInfoExcel.Clone();
                dtResult.Columns.Add("编号", typeof(string));
                dtResult.Columns.Add("errorInfo", typeof(string));
                dtResult.Columns.Add("fgsid", typeof(Int32));
                string strErr = string.Empty;
                for (int i = 0; i < dtStoreInfoExcel.Rows.Count; i++)
                {
                    DataRow drNew = dtResult.NewRow();
                    //设置行号
                    drNew["编号"] = i + 1;

                    DataRow[] drstore = dtStoreInfoExcel.Select(string.Format("门店名称 = '{0}'", dtStoreInfoExcel.Rows[i]["门店名称"].ToString()), "", DataViewRowState.CurrentRows);
                    if (drstore.Length > 1)
                    {
                        isOk = false;
                        drNew["errorInfo"] = "门店名称在文件中重复。<br/>";
                        strErr += "第" + (i + 2) + "行，门店名称在文件中重复\r\n";
                    }
                    DataRow[] drfgs = dtfgsInfo.Select(string.Format(@"fgsName='{0}'", dtStoreInfoExcel.Rows[i]["所属分公司"].ToString()), "", DataViewRowState.CurrentRows);
                    if (drfgs.Length <= 0)
                    {
                        isOk = false;
                        drNew["errorInfo"] += "所属分公司名称不存在，无法保存。<br/>";
                        strErr += "第" + (i + 2) + "行，所属分公司名称不存在，无法保存。\r\n";
                    }
                    DataRow[] drs = dtStoreInfo.Select(string.Format(@"storeName='{0}'", dtStoreInfoExcel.Rows[i]["门店名称"].ToString()), "", DataViewRowState.CurrentRows);
                    if (drs.Length > 0)
                    {
                        isOk = false;
                        drNew["errorInfo"] += "门店名称已经存在，无法保存。";
                        strErr += "第" + (i + 2) + "行，门店名称已经存在，无法保存。\r\n";
                    }

                    drNew["所属分公司"] = dtStoreInfoExcel.Rows[i]["所属分公司"];
                    drNew["门店名称"] = dtStoreInfoExcel.Rows[i]["门店名称"];
                    drNew["负责人"] = dtStoreInfoExcel.Rows[i]["负责人"];
                    drNew["联系电话"] = dtStoreInfoExcel.Rows[i]["联系电话"];
                    drNew["金力账号"] = dtStoreInfoExcel.Rows[i]["金力账号"];
                    drNew["排序"] = dtStoreInfoExcel.Rows[i]["排序"];
                    dtResult.Rows.Add(drNew);
                }
                repeateExcel.DataSource = dtResult;
                repeateExcel.DataBind();
                if (!isOk)
                {
                    //this.ShowMsg("选择文件中有相同分公司名称，请检查后重新导入", false);
                    ViewState["dtSu"] = dtResult;
                    ViewState["state"] = "no";
                    this.tboxErr.Text = strErr;
                }
                else
                {
                    ViewState["state"] = "ok";
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        DataRow[] drfgs = dtfgsInfo.Select(string.Format(@"fgsName='{0}'", dtResult.Rows[i]["所属分公司"].ToString().Trim()), "", DataViewRowState.CurrentRows);
                        for (int w = 0; w < drfgs.Length; w++)
                        {
                            dtResult.Rows[i]["fgsid"] = drfgs[w]["id"];
                        }
                    }
                    dtResult.Columns["门店名称"].ColumnName = "storeName";
                    dtResult.Columns["负责人"].ColumnName = "storeRelationPerson";
                    dtResult.Columns["联系电话"].ColumnName = "storeRelationCell";
                    dtResult.Columns["金力账号"].ColumnName = "accountALLHere";
                    dtResult.Columns["排序"].ColumnName = "scode";
                    dtResult.Columns.Remove("所属分公司");
                    dtResult.Columns.Remove("errorInfo");
                    dtResult.Columns.Remove("编号");
                    ViewState["dtSu"] = dtResult;
                }
            }
            catch
            {
                this.ShowMsg("文件正在被使用！", false);
            }
        }
        else
        {
            this.ShowMsg("Excel文件未导入或渠道商未选择！", false);
        }
    }
    protected void btnBc_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtSu"];
        string state = ViewState["state"].ToString();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        if (state == "no")
        {
            this.ShowMsg("请查看导入提示列显示内容", false);
        }
        else
        {
            string sqls = "select * from CW_StoreInfo";
            string[] sql = sqls.Split(';');
            int count = DataBaseHelper.CommitDataSet(ds, sql);
            if (count > 0)
            {
                this.ShowMsg("导入成功！", true);
            }
        }
    }
    protected void file_Click(object sender, EventArgs e)
    {
        string fileName = "Excel模板.xls";//客户端保存的文件名
        string filePath = Server.MapPath("../../Storage/Templates/创维门店导入模板.xls");//路径
        FileInfo fileInfo = new FileInfo(filePath);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        Response.AddHeader("Content-Transfer-Encoding", "binary");
        Response.ContentType = "application/octet-stream";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        Response.WriteFile(fileInfo.FullName);
        Response.Flush();
        Response.End();
    }
}