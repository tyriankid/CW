﻿using Hidistro.ControlPanel.Config;
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

public partial class Admin_store_ImportFgsFile : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 选择文件及预览
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnPack_Click(object sender, EventArgs e)
    {
        try
        {

            if (!string.IsNullOrEmpty(fileUpload.FileName) && fileUpload.FileContent.Length > 0)
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
                DataTable dtSupplierInfoExcel = excelDBClass.ExportToDataSet().Tables[0];
                //表头格式验证
                string strHeaders = "分公司名称,分公司电话,分公司地址,分公司排序";
                //将表头字符串用,切割，装到arrayHeader数组
                string[] arrayHeader = strHeaders.Split(',');
                //循环表头与选中的表格里面标题是否一致
                for (int i = 0; i < dtSupplierInfoExcel.Columns.Count; i++)
                {
                    if (dtSupplierInfoExcel.Columns[i].ColumnName != arrayHeader[i])
                    {
                        this.ShowMsg("导入的文件格式不正确,请检查模板文件。", false);
                        return;
                    }
                }

                //获取数据库数据 转成DataTable
                string strSql = "select * from CW_Filiale";
                DataTable dtSupplier = DataBaseHelper.GetDataSet(strSql).Tables[0];

                bool isOk = true;
                DataRow[] blank = dtSupplierInfoExcel.Select("分公司名称 is null");
                for (int i = 0; i < blank.Count(); i++)
                {
                    blank[i].Delete();
                }
                dtSupplierInfoExcel.AcceptChanges();
                dtSupplierInfoExcel.Columns.Add("编号", typeof(string));
                dtSupplierInfoExcel.Columns.Add("errorInfo", typeof(string));
                for (int i = 0; i < dtSupplierInfoExcel.Rows.Count; i++)
                {
                    DataRow[] dr = dtSupplierInfoExcel.Select(string.Format("分公司名称 = '{0}'", dtSupplierInfoExcel.Rows[i]["分公司名称"].ToString()), "", DataViewRowState.CurrentRows);
                    if (dr.Length > 1)
                    {
                        isOk = false;
                        dr[dr.Length - 1]["errorInfo"] = "重复数据！";
                    }
                    else
                    {
                        DataRow[] drs = dtSupplier.Select(string.Format(@"fgsName='{0}'", dtSupplierInfoExcel.Rows[i]["分公司名称"].ToString()), "", DataViewRowState.CurrentRows);
                        if (drs.Length > 0)
                        {
                            dtSupplierInfoExcel.Rows[i]["errorInfo"] = "分公司名称存在！点击保存更改分公司信息！";
                            drs[0]["fgsPhone"] = dtSupplierInfoExcel.Rows[i]["分公司电话"];
                            drs[0]["fgsAddress"] = dtSupplierInfoExcel.Rows[i]["分公司地址"];
                            drs[0]["scode"] = dtSupplierInfoExcel.Rows[i]["分公司排序"];
                        }
                        else
                        {
                            DataRow drnew = dtSupplier.NewRow();
                            drnew["fgsName"] = dtSupplierInfoExcel.Rows[i]["分公司名称"];
                            drnew["fgsPhone"] = dtSupplierInfoExcel.Rows[i]["分公司电话"];
                            drnew["fgsAddress"] = dtSupplierInfoExcel.Rows[i]["分公司地址"];
                            drnew["scode"] = dtSupplierInfoExcel.Rows[i]["分公司排序"];
                            dtSupplier.Rows.Add(drnew);
                        }
                    }
                    //设置行号
                    dtSupplierInfoExcel.Rows[i]["编号"] = i + 1;
                }
                if (!isOk)
                {
                    //this.ShowMsg("选择文件中有相同分公司名称，请检查后重新导入", false);
                    ViewState["dtSu"] = dtSupplierInfoExcel;
                    ViewState["state"] = "no";
                }
                else
                {
                    ViewState["state"] = "ok";
                    ViewState["dtSu"] = dtSupplier;
                }
                repeateExcel.DataSource = dtSupplierInfoExcel;
                repeateExcel.DataBind();


            }
            else
            {
                this.ShowMsg("Excel文件未导入或渠道商未选择！", false);
            }
        }
        catch {
            this.ShowMsg("内部错误！请联系管理员！", false);
        }


        
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBc_Click(object sender, EventArgs e)
    {
        try
        {


            DataTable dt = (DataTable)ViewState["dtSu"];
            string state = ViewState["state"].ToString();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (state == "no")
            {
                this.ShowMsg("请确保没有重复数据", false);
            }
            else
            {
                string sqls = "select * from CW_Filiale";
                string[] sql = sqls.Split(';');
                int count = DataBaseHelper.CommitDataSet(ds, sql);
                if (count > 0)
                {
                    this.ShowMsg("导入成功！", true);
                }
            }
        }
        catch
        {
            this.ShowMsg("操作有误！", false);

        }
    }
    /// <summary>
    /// 下载表格模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void file_Click(object sender, EventArgs e)
    {
        string fileName = "Excel模板.xls";//客户端保存的文件名
        string filePath = Server.MapPath("../../Storage/Templates/分公司导入模板.xls");//路径
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