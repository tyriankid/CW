using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_O2OMemberInPort : AdminPage
{
    //protected System.Web.UI.WebControls.Button btnExcelPrint;
    //protected System.Web.UI.WebControls.FileUpload fileUpload;
    //protected System.Web.UI.WebControls.Button btnDownLoad;
    //protected System.Web.UI.WebControls.Repeater repeateExcel;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnDownLoad.Click += new System.EventHandler(this.ExcelDown_Click);
        this.btnExcelPrint.Click += new System.EventHandler(this.btnExcelPrint_Click);
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        //执行积分计算，2017-01-20设置，原积分问题，为处理积分写的方法
        //if (!IsPostBack)
        //{
        //    ClearPoints();
        //    DelPoints();
        //}
    }

    ///// <summary>
    ///// 扣除积分
    ///// </summary>
    //public void DelPoints()
    //{
    //    PointDetailDao PointDao = new PointDetailDao();
    //    DataTable dtOrderReturn = PointDao.GetOrderReturn();
    //    foreach (DataRow dr in dtOrderReturn.Rows)
    //    {
    //        //OrderInfo orderinfo = ShoppingProcessor.GetOrderInfo(dr["OrderId"].ToString());
    //        OrderInfo orderInfo = OrderHelper.GetOrderInfo(dr["OrderId"].ToString());
    //        if (orderInfo != null && !string.IsNullOrEmpty(orderInfo.OrderId))
    //        {
    //            continue;
    //            //修改积分
    //            MemberInfo memberInfo = MemberHelper.GetMember(orderInfo.UserId);
    //            if (OrderHelper.ReducedPoint(orderInfo, memberInfo))
    //            {
    //                //减少用户总积分
    //                memberInfo.Points = memberInfo.Points - orderInfo.Points;
    //                MemberHelper.Update(memberInfo);
    //            }
    //        }
    //    }

    //}


    /*
    public void ClearPoints()
    {
        PointDetailDao PointDao = new PointDetailDao();
        DataTable dtUser = PointDao.GetUsers();
        DataTable dtUserIds = PointDao.GetPointUserIds();
        DataTable dtPoints = PointDao.GetPoints();
        foreach (DataRow dr in dtUserIds.Rows)
        {
            int userid = int.Parse(dr["UserId"].ToString());
            DataRow[] drPoints = dtPoints.Select(string.Format("UserId = {0}", userid), "TradeDate", DataViewRowState.CurrentRows);
            int pointsSum = 0;
            foreach (DataRow drPoint in drPoints)
            {
                int iAdd = int.Parse(drPoint["Increased"].ToString());
                int iDel = int.Parse(drPoint["Reduced"].ToString());
                if (iAdd > 0 && iDel == 0)
                {
                    pointsSum += iAdd;
                }
                else
                {
                    pointsSum -= iDel;
                }
                drPoint["Points"] = pointsSum;
            }
            DataRow[] drUser = dtUser.Select(string.Format("UserId = {0}", userid), "", DataViewRowState.CurrentRows);
            if (drUser.Length > 0)
                drUser[0]["Points"] = pointsSum;
        }
        ///验证变化
        if (dtUser.GetChanges() != null && dtPoints.GetChanges() != null)
        {
            string sql = "select * from dbo.aspnet_Members";
            int count = DataBaseHelper.CommitDataTable(dtUser.GetChanges(), sql);

            string sql1 = "select * from dbo.Hishop_PointDetails";
            int count1 = DataBaseHelper.CommitDataTable(dtPoints.GetChanges(), sql1);
        }
    }*/

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
                if (Path.GetExtension(fileUpload.FileName).ToLower() != ".xls")
                {
                    this.ShowMsg("导入的不是xls文件。", false);
                    return;
                }
                filePath = MapPath("/Storage/temp/") + fileUpload.FileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                fileUpload.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                this.ShowMsg("上传文件存储出错：" + ex.Message, false);
                return;
            }
            
            ExcelDBClass excelDBClass = new ExcelDBClass(filePath, true);
            //Excel转为DataTable
            DataTable dtMemberExcel = excelDBClass.ExportToDataSet().Tables[0];

            //表头格式验证
            string strHeaders = CustomConfigHelper.Instance.O2OMemberInPortExcel();
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
            dtMemberExcel.Columns.Add("errorInfo", typeof(string));
            dtMemberExcel.Columns["errorInfo"].DefaultValue = "";
            //bool errorState = true;
            DataTable dtMemeberSubmit = DataBaseHelper.GetDataTable("CW_O2OMembers");
            DataTable dtStoreInfo = StoreInfoHelper.GetAllStore();
            //DataTable dtMemeberValidate = dtMemeberSubmit.Copy();
            if (dtMemberExcel.Rows.Count > 0)
            {
                //删除空行
                foreach (DataRow dr in dtMemberExcel.Rows)
                {
                    if (string.IsNullOrEmpty(dr["会员名称"].ToString()))
                    {
                        dr.Delete();
                    }
                }
                dtMemberExcel.AcceptChanges();

                int iModifyCount = 0;
                int iNewCount = 0;
                string strErrorInfo = string.Empty;
                for (int i = 0; i < dtMemberExcel.Rows.Count; i++)
                {
                    DataRow dr = dtMemberExcel.Rows[i];
                    string strDrError = string.Empty;
                    #region 数据合法性判断
                    //验证名称
                    if (string.IsNullOrEmpty(dr["会员名称"].ToString()))
                    {
                        strDrError = string.Format("行号：{0} 发生错误，原因：会员名称不能为空！", i);
                        dr["errorInfo"] = strDrError;
                        strErrorInfo += strDrError + "\r\n";
                        continue;
                    }
                    //验证电话
                    if (!PageValidateHelper.IsNumber(dr["会员电话"].ToString()) || string.IsNullOrEmpty(dr["会员电话"].ToString()))
                    {
                        strDrError = string.Format("行号：{0} 发生错误，原因：电话不能为空或格式错误！", (i + 1));
                        dr["errorInfo"] = strDrError;
                        strErrorInfo += strDrError + "\r\n";
                        continue;
                    }
                    if (dtMemberExcel.Select(string.Format("会员名称 = '{0}' and 会员电话 = '{1}'", dr["会员名称"].ToString(), dr["会员电话"].ToString())).Length > 1)
                    {
                        strDrError = string.Format("行号：{0} 发生错误，原因：会员在导入数据中重复！", (i + 1));
                        dr["errorInfo"] = strDrError;
                        strErrorInfo += strDrError + "\r\n";
                        continue;
                    }
                    if (string.IsNullOrEmpty(dr["门店DZ号"].ToString()))
                    {
                        strDrError = string.Format("行号：{0} 发生错误，原因：门店DZ号不能为空！", (i + 1));
                        dr["errorInfo"] = strDrError;
                        strErrorInfo += strDrError + "\r\n";
                        continue;
                    }
                    if (dtStoreInfo.Select(string.Format("accountALLHere = '{0}'", dr["门店DZ号"].ToString())).Length == 0)
                    {
                        strDrError = string.Format("行号：{0} 发生错误，原因：门店DZ号未匹配到对应的门店！", (i + 1));
                        dr["errorInfo"] = strDrError;
                        strErrorInfo += strDrError + "\r\n";
                        continue;
                    }

                    //验证名称与电话是否同时相同，如果存在则修改，如果不存在则新增
                    DataRow[] drs = dtMemeberSubmit.Select(string.Format("name = '{0}' and mobile = '{1}'", dr["会员名称"].ToString(), dr["会员电话"].ToString()));
                    if (drs.Length > 0)
                    {
                        iModifyCount++;
                    }
                    else
                    {
                        iNewCount++;
                    }
                    #endregion
                }
                this.txtErrorInfo.Text = string.Format("导入新增的数据行：{0}; 导入修改的数据行：{1}", iNewCount, iModifyCount) + "\r\n" + this.txtErrorInfo.Text;
                this.repeateExcel.DataSource = dtMemberExcel;
                this.repeateExcel.DataBind();

                //开始保存
                if (string.IsNullOrEmpty(strErrorInfo))
                {
                    ViewState["Submit"] = dtMemberExcel;
                    //#region 数据同步数据库
                    //string sql = "select * from CW_O2OMembers";
                    //int count = DataBaseHelper.CommitDataTable(dtMemeberSubmit.GetChanges(), sql);
                    //if (count > 0)
                    //{
                    //    dtMemeberSubmit.AcceptChanges();
                    //    this.ShowMsg("保存数据库成功！", true);
                    //}
                    //else
                    //{
                    //    this.ShowMsg("保存数据库失败！", false);
                    //}
                    //#endregion    
                }
                else
                {
                    ViewState["Submit"] = null;
                    //有问题提示问题
                    this.txtErrorInfo.Text = strErrorInfo;
                }
            }
            else
            {
                ViewState["Submit"] = null;
                this.ShowMsg("Excel表格为空！", false);
            }
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSave_Click(object sender, System.EventArgs e)
    {
        if (ViewState["Submit"] == null)
        {
            this.ShowMsg("请先导入数据文件，如果导入后提示框中存在错误，则先修改错误信息在尝试导入并保存！", false);
            return;
        }
        //待提交的数据集
        DataTable dtMemeberSubmit = DataBaseHelper.GetDataTable("CW_O2OMembers");
        
        DataTable dtMemberExcel = ViewState["Submit"] as DataTable;
        for (int i = 0; i < dtMemberExcel.Rows.Count; i++)
        {
            DataRow dr = dtMemberExcel.Rows[i];
            //验证名称与电话是否同时相同，如果存在则修改，如果不存在则新增
            DataRow[] drs = dtMemeberSubmit.Select(string.Format("name = '{0}' and mobile = '{1}'", dr["会员名称"].ToString(), dr["会员电话"].ToString()));
            if (drs.Length > 0)
            {
                //修改
                drs[0]["sex"] = dr["性别"];
                drs[0]["profession"] = dr["职业"];
                drs[0]["storeCode"] = dr["门店DZ号"];
                drs[0]["gradeName"] = dr["会员等级"];
                drs[0]["birthday"] = dr["会员生日"];
                drs[0]["model"] = dr["购买产品型号"];
                drs[0]["buydate"] = dr["购买日期"];
                drs[0]["typename"] = dr["产品类型"];
                drs[0]["price"] = dr["产品价格"];
                drs[0]["OldRegion"] = dr["所属地区"];
                drs[0]["Address"] = dr["详细地址"];
                drs[0]["remark"] = dr["备注"];
                drs[0]["jiatingchengyuan"] = dr["家庭成员构成"];
                drs[0]["zhufangxinxi"] = dr["用户住房信息"];
                drs[0]["fangyujiadian"] = dr["房屋家电配置"];
                drs[0]["jiadianshiyong"] = dr["家电使用情况"];
                drs[0]["gerenqingxiang"] = dr["个人品牌倾向"];
                drs[0]["jinqixuqiu"] = dr["近期购买需求"];
            }
            else
            {
                //新增
                DataRow drNew = dtMemeberSubmit.NewRow();
                drNew["name"] = dr["会员名称"];
                drNew["sex"] = dr["性别"];
                drNew["mobile"] = dr["会员电话"];
                drNew["profession"] = dr["职业"];
                drNew["storeCode"] = dr["门店DZ号"];
                drNew["gradeName"] = dr["会员等级"];
                drNew["birthday"] = dr["会员生日"];
                drNew["model"] = dr["购买产品型号"];
                drNew["buydate"] = dr["购买日期"];
                drNew["typename"] = dr["产品类型"];
                drNew["price"] = dr["产品价格"];
                drNew["OldRegion"] = dr["所属地区"];
                drNew["Address"] = dr["详细地址"];
                drNew["remark"] = dr["备注"];
                drNew["jiatingchengyuan"] = dr["家庭成员构成"];
                drNew["zhufangxinxi"] = dr["用户住房信息"];
                drNew["fangyujiadian"] = dr["房屋家电配置"];
                drNew["jiadianshiyong"] = dr["家电使用情况"];
                drNew["gerenqingxiang"] = dr["个人品牌倾向"];
                drNew["jinqixuqiu"] = dr["近期购买需求"];
                dtMemeberSubmit.Rows.Add(drNew);
            }
        }

        #region 数据同步数据库
        if (dtMemeberSubmit.GetChanges() != null)
        {
            string sql = "select * from CW_O2OMembers";
            int count = DataBaseHelper.CommitDataTable(dtMemeberSubmit.GetChanges(), sql);
            if (count > 0)
            {
                ViewState["Submit"] = null;
                dtMemeberSubmit.AcceptChanges();
                this.ShowMsgAndReUrl("保存数据库成功。", true, "CW_O2OMembers.aspx");
            }
            else
            {
                ViewState["Submit"] = null;
                this.ShowMsg("保存数据库失败！", false);
            }
        }
        else
        {
            ViewState["Submit"] = null;
            this.ShowMsg("无任何数据改变，无法保存！", false);
        }
        #endregion
    }

    /// <summary>
    /// 下载表格模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ExcelDown_Click(object sender, EventArgs e)
    {
        string fileName = "录入会员导入模版.xls";//客户端保存的文件名
        string filePath = Server.MapPath("../../Storage/Templates/录入会员导入模版.xls");//路径
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