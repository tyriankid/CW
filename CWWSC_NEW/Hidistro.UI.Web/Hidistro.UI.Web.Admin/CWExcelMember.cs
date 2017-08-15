using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
namespace Hidistro.UI.Web.Admin
{
   public class CWExcelMember : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnExcelPrint;
        protected System.Web.UI.WebControls.FileUpload fileUpload;
        protected System.Web.UI.WebControls.Button btnDownLoad;
        protected System.Web.UI.WebControls.Repeater repeateExcel;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnDownLoad.Click += new System.EventHandler(this.ExcelDown_Click);
            this.btnExcelPrint.Click += new System.EventHandler(this.btnExcelPrint_Click);
        
        }
        /// <summary>
        /// 会员批量导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelPrint_Click(object sender, System.EventArgs e)
        {   
            if (!string.IsNullOrEmpty(fileUpload.FileName) && fileUpload.FileContent.Length > 0)
            {
                #region  导入模板格式及表头判断
                string filePath = "";
                try
                {
                    filePath = MapPath("/Storage/temp/") + "yyyyMMddHHmmss" + fileUpload.FileName;
                    fileUpload.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    this.ShowMsg("出错了：" + ex.Message, false);
                }
                if (Path.GetExtension(filePath).ToLower() != ".xls")
                {
                    this.ShowMsg("导入的不是xls文件。", false);
                    return;
                }
                ExcelDBClass excelDBClass = new ExcelDBClass(filePath, true);
                //Excel转为DataTable
                DataTable dtMemberExcel = excelDBClass.ExportToDataSet().Tables[0];
                //表头格式验证
                string strHeaders = CustomConfigHelper.Instance.IsMemberExcel();
                string[] arrayHeader = strHeaders.Split(',');
                for (int i = 0; i < dtMemberExcel.Columns.Count; i++)
                {
                    if (dtMemberExcel.Columns[i].ColumnName != arrayHeader[i])
                    {
                        this.ShowMsg("导入的文件格式不正确,请检查模板文件。", false);
                        return;
                    }
                }
                #endregion
                string memberInfo = "CW_Members";
                dtMemberExcel.Columns.Add("errorInfo", typeof(string));
                dtMemberExcel.Columns["errorInfo"].DefaultValue = "";
                bool errorInfo = true;
                DataTable dtMemeber = DataBaseHelper.GetDataTable(memberInfo);
                if (dtMemberExcel.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMemberExcel.Rows)
                    {
                        #region 数据合法性判断
                        if (CWMembersHelper.GetMember(dr["会员昵称"].ToString()) != null || string.IsNullOrEmpty(dr["会员昵称"].ToString()))
                        {
                            dr["errorInfo"] = dr["errorInfo"] + "用户昵称不能为空或已被使用！";
                            errorInfo = false;
                        }
                        if (!PageValidateHelper.IsNumber(dr["电话"].ToString()) || string.IsNullOrEmpty(dr["电话"].ToString()))
                        {
                            dr["errorInfo"] = dr["errorInfo"] + " 电话不能为空或格式错误!";
                            errorInfo = false;
                        }
                        if (!PageValidateHelper.IsEmail(dr["邮箱"].ToString()))
                        {
                            dr["errorInfo"] = dr["errorInfo"] + " 邮箱格式错误!";
                            errorInfo = false;
                        }
                        #endregion
                        if (errorInfo)
                        {
                            DataRow drNew = dtMemeber.NewRow();
                            drNew["UserName"] = dr["会员昵称"]; drNew["RealName"] = dr["会员姓名"];
                            drNew["Email"] = dr["邮箱"]; drNew["CellPhone"] = dr["电话"]; drNew["QQ"] = dr["QQ"];
                            drNew["Address"] = dr["详细地址"];
                            dtMemeber.Rows.Add(drNew);
                        }
                    }
                    this.repeateExcel.DataSource = dtMemberExcel;
                    this.repeateExcel.DataBind();
                    #region 数据同步数据库
                    string sql = "select*from CW_Members";
                    int count = DataBaseHelper.CommitDataTable(dtMemeber, sql);
                    if (count > 0)
                    {
                        this.ShowMsg("导入成功！", true);
                    }
                    else
                    {
                        this.ShowMsg("导入失败！", false);
                    }
                    #endregion
                }
                else
                {
                    this.ShowMsg("Excel表格为空！", false);
                }
            }
        }
        /// <summary>
        /// 下载表格模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExcelDown_Click(object sender, EventArgs e)
        {
            string fileName = "Excel模板.xls";//客户端保存的文件名
            string filePath = Server.MapPath("../../Storage/Templates/会员导入模版.xls");//路径
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
}
