using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_userHotSearch : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    protected void bind()
    {
        DataTable dt = DataBaseHelper.GetDataTable("CW_userHotSearch");
        StringBuilder stringBuilder = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                i++;
                stringBuilder.Append("<li id='searchText" + i + "'><input type='text' class='forminput formwidth'   value='" + row["SearchText"] + "'/><a href=\"javascript:delRow('" + i + "')\">删除</a></li>");
            }
            this.txtSearch.Text = stringBuilder.ToString();
        }
    }

    //保存数据
    [System.Web.Services.WebMethod]
    public static string Save(string text)
    {

        UserSearchLogsHelper.DeleteHotSearch();
        DataTable dt=DataBaseHelper.GetDataTable("CW_userHotSearch");
        string[] Strtext = text.Trim().TrimEnd('&').Split('&');
      
        for (int i = 0; i <Strtext.Length; i++)
        {
            DataRow row = dt.NewRow();
            if (!string.IsNullOrEmpty(Strtext[i]))
            {
                row["SearchText"] = Strtext[i];
                row["SearchDate"] = DateTime.Now;
                dt.Rows.Add(row);
            }
        }
        string str = "select*from CW_userHotSearch";
        int count = DataBaseHelper.CommitDataTable(dt, str);
        if (count < 0)
        {
            return "保存失败";
        }
        return "保存成功";
    }
}