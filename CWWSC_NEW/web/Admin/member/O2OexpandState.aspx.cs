using Hidistro.ControlPanel.Function;
using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.UI.ControlPanel.Utility;
using ControlPanel.Commodities;

public partial class Admin_member_O2OexpandState : AdminPage
{
    protected static string uid = string.Empty;
    protected static bool isAdd = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string type = Request.QueryString["type"].ToString();//编辑删除
            bool isEdit = false;
            if (Request.QueryString["userid"] != null && !string.IsNullOrEmpty(Request.QueryString["userid"].ToString()))
            {
                isEdit = true;
                uid = Request.QueryString["userid"].ToString();
            }
            //得到数据集
            DataTable dtCols = DataBaseHelper.GetDataTable("CW_O2OMembersCol", "", "");
            string strWhere = string.Format("userid = '{0}'", uid);
            DataTable dtValues = DataBaseHelper.GetDataTable("CW_O2OMembersAttribute", strWhere, "");

            BindData1(isEdit, uid, dtCols, dtValues);
            BindData2(isEdit, uid, dtCols, dtValues);
            BindData3(isEdit, uid, dtCols, dtValues);
            BindData4(isEdit, uid, dtCols, dtValues);
            BindData5(isEdit, uid, dtCols, dtValues);
            BindData6(isEdit, uid, dtCols, dtValues);
        }
    }
    //家庭成员构成
    public static DataRow[] drCols;
    private void BindData1(bool isedit, string uid,DataTable dtCols,DataTable dtValues)
    {
        //根据类型查询该类型下的列
        drCols = dtCols.Select("type = '家庭成员构成'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '家庭成员构成' ", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbFamily'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr id='trFamily' style='width:100%;'>");

            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }

        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td >");
                stringBuilder.Append("<input type='text' id='inpFamily' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "家庭成员构成", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='Family{0}'  style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td style='padding:5px'>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "家庭成员构成", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:delFamilyRow('{0}')\">删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");
        this.talbe1.Text = stringBuilder.ToString();
    }
    //用户住房信息
    private void BindData2(bool isedit, string uid, DataTable dtCols, DataTable dtValues)
    {
        //根据类型查询该类型下的列
        DataRow[] drCols = dtCols.Select("type = '用户住房信息'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '用户住房信息'", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbHouse'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr id='trHouse' style='width:100%;'>");
            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td>");
                stringBuilder.Append("<input type='text' id='' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
       else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "用户住房信息", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='House{0}' style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td style='padding:5px'>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "用户住房信息", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:delHouseRow('{0}')\">删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");

        this.talbe2.Text = stringBuilder.ToString();
    }
    //房屋家电配置
    private void BindData3(bool isedit, string uid, DataTable dtCols, DataTable dtValues)
    {
        //根据类型查询该类型下的列
        DataRow[] drCols = dtCols.Select("type = '房屋家电配置'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '房屋家电配置'", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute


        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbConfigure'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr  id='trConfigure' style='width:100%;'> ");
            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }

        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td>");
                stringBuilder.Append("<input type='text' id='' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "房屋家电配置", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='Configure{0}' style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td style='padding:5px'>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "房屋家电配置", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:onclick=delConfigureRow('{0}')\">删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");

        this.talbe3.Text = stringBuilder.ToString();
        
    }
    //家电使用情况
    private void BindData4(bool isedit, string uid, DataTable dtCols, DataTable dtValues)
    {//根据类型查询该类型下的列
        DataRow[] drCols = dtCols.Select("type = '家电使用情况'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '家电使用情况'", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbMake'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr id='trMake' style='width:100%;'>");
            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td>");
                stringBuilder.Append("<input type='text' id='' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "家电使用情况", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='Make{0}' style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td style='padding:5px'>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "家电使用情况", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:delMakeRow('{0}')\">删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");
        this.talbe4.Text = stringBuilder.ToString();
    }
    //个人品牌倾向
    private void BindData5(bool isedit, string uid, DataTable dtCols, DataTable dtValues)
    {
        //根据类型查询该类型下的列
        DataRow[] drCols = dtCols.Select("type = '个人品牌倾向'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '个人品牌倾向'", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute


        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbBrand'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr id='trBrand' style='width:100%;'>");
            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }

        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td>");
                stringBuilder.Append("<input type='text' id='' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "个人品牌倾向", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='Brand{0}' style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "个人品牌倾向", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:delBrandRow('{0}')\" >删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");

        this.talbe5.Text = stringBuilder.ToString();

    }
    //近期购买需求
    private void BindData6(bool isedit, string uid, DataTable dtCols, DataTable dtValues)
    {
        //根据类型查询该类型下的列
        DataRow[] drCols = dtCols.Select("type = '近期购买需求'", "scode", DataViewRowState.CurrentRows);
        DataRow[] drValues = dtValues.Select("type = '近期购买需求'", "scode", DataViewRowState.CurrentRows);//来源 CW_O2OMembersAttribute


        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table id='tbShop'>");
        if (drCols.Length > 0)
        {
            stringBuilder.Append("<tr id='trShop' style='width:100%;'>");
            foreach (DataRow dr in drCols)
            {
                stringBuilder.AppendFormat("<td id='{0}'>", dr["ColId"].ToString());
                stringBuilder.AppendFormat("{0}", dr["ColName"].ToString());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        if (!isedit)
        {
            //新增
            stringBuilder.Append("<tr>");
            foreach (DataRow drcol in drCols)
            {
                stringBuilder.Append("<td>");
                stringBuilder.Append("<input type='text' id='' value='' class='forminput'>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        else
        {
            if (drCols.Length>0)
            {
                DataRow[] drValuess = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "近期购买需求", drCols[0]["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                if (drValuess.Length > 0)
                {
                    isAdd = true;
                    for (int a = 0; a < drValuess.Length; a++)
                    {
                        //编辑
                        stringBuilder.AppendFormat("<tr id='Shop{0}' style='height:40px'>", a);
                        foreach (DataRow drcol in drCols)
                        {
                            //查找当前行、当前列的数据值，
                            stringBuilder.Append("<td style='padding:5px'>");
                            DataRow[] drValue = dtValues.Select(string.Format("type = '{0}' and ColId = '{1}'", "近期购买需求", drcol["ColId"].ToString()), "scode", DataViewRowState.CurrentRows);
                            if (drValue.Length > 0)
                            {
                                stringBuilder.AppendFormat("<input type='text' id='{0}' value='{1}' class='forminput'>", drValue[a]["ColId"].ToString(), drValue[a]["ColValue"].ToString());
                            }
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.AppendFormat("<td>&nbsp;<a href=\"javascript:delShopRow('{0}')\">删除</a></td>", a);
                        stringBuilder.Append("</tr>");
                    }
                }
            }
        }
        stringBuilder.Append("</table>");

        this.talbe6.Text = stringBuilder.ToString();
    }
    //保存数据
    [System.Web.Services.WebMethod]
    public static string Save(string ListInfo, string ListIdInfo)
    {   
         if(uid=="0"||uid=="")
            {
                return  "请填写基本信息！";
            }
         if (isAdd == true)
         {
             O2OAttributeHelper.DeleteO2OAttribute(int.Parse(uid));
         }
         if (ListInfo != "" && ListIdInfo!="")
         {
            DataTable dtAttribute = DataBaseHelper.GetDataTable("CW_O2OMembersAttribute", "", "");
            //所属类
            string type = "家庭成员构成,用户住房信息,房屋家电配置,家电使用情况,个人品牌倾向,近期购买需求";
            string[] types = type.Split(',');
            //值
            string[] listInfos = ListInfo.TrimEnd(',').Split('&');
            //添加input框的ID
            string[] listeIdInfo = ListIdInfo.TrimEnd(',').Split('&');
            try
            {
                for (int i = 0; i < types.Length; i++)
                {
                    string typeName = types.GetValue(i).ToString();
                    string Id = listeIdInfo.GetValue(i).ToString();
                    string[] ids = Id.TrimEnd(',').Split(',');
                    string values = listInfos.GetValue(i).ToString();
                    string[] value = values.TrimEnd(',').Split(',');
                    for (int y = 0; y < value.Length; y++)
                    {
                        if (ids.GetValue(y).ToString() != "")
                        {
                            DataRow drAttribute = dtAttribute.NewRow();
                            drAttribute["type"] = typeName;
                            drAttribute["ColId"] = ids.GetValue(y);
                            drAttribute["ColValue"] = value.GetValue(y);
                            drAttribute["userid"] = uid;
                            drAttribute["GroupID"] = ids.GetValue(y);
                            drAttribute["scode"] = 0;
                            dtAttribute.Rows.Add(drAttribute);
                        }
                    }
                }
            }
            catch
            {
                return "未知错误！请联系管理员。";
            }
            string sqls = "select*from CW_O2OMembersAttribute";
            int count = DataBaseHelper.CommitDataTable(dtAttribute, sqls);
            if (count < 0)
            {
                return "保存失败";
            }
        }
        return "保存成功"; 
    }
}