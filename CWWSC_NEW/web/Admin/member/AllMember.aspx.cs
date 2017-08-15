using Hidistro.ControlPanel.Function;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_AllMember : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            AllMemberInfos("");
        }
    }
    public void AllMemberInfos(string name)
    {
        string aspwhere = "";
        string o2owhere="";
        string ahwhere = "";
        if(name !="")
        {
            aspwhere = "RealName='" + name + "'";
           o2owhere="name='"+name+"'";
           ahwhere = "RealName='" + name + "'";
        }
        string aspMemberInfo = "select RealName,CellPhone,Address from dbo.aspnet_Members";
        DataTable dtAspMember = DataBaseHelper.GetMemberDataTable(aspMemberInfo, aspwhere);

        string o2oMemberInfo="select name,mobile,Address  from dbo.CW_O2OMembers";
        DataTable dtO2oMember = DataBaseHelper.GetMemberDataTable(o2oMemberInfo, o2owhere);

        string ahMemberInfo="select RealName,CellPhone,Address  from dbo.CW_Members";
        DataTable dtAhMember = DataBaseHelper.GetMemberDataTable(ahMemberInfo, ahwhere);

        DataTable dtNewMember = new DataTable();
        dtNewMember.Columns.Add("name", typeof(string));
        dtNewMember.Columns.Add("phone", typeof(string));
        dtNewMember.Columns.Add("address", typeof(string));
        
        foreach(DataRow Arow in dtAspMember.Rows)
        {
            DataRow newARow = dtNewMember.NewRow();
            newARow["name"] = Arow["RealName"];
            newARow["phone"] = Arow["CellPhone"];
            newARow["address"] = Arow["Address"];
            dtNewMember.Rows.Add(newARow);
        }
       
        foreach (DataRow Orow in dtO2oMember.Rows)
        {
            DataRow newORow = dtNewMember.NewRow();
            newORow["name"] = Orow["name"];
            newORow["phone"] = Orow["mobile"];
            newORow["address"] = Orow["Address"];
            dtNewMember.Rows.Add(newORow);
        }
        
        foreach (DataRow Crow in dtAhMember.Rows)
        {
            DataRow newCRow = dtNewMember.NewRow();
            newCRow["name"] = Crow["RealName"];
            newCRow["phone"] = Crow["CellPhone"];
            newCRow["address"] = Crow["Address"];
            dtNewMember.Rows.Add(newCRow);
        }
        DeleteSameRow(dtNewMember, "phone");
        this.rptList.DataSource = dtNewMember;
        this.rptList.DataBind();
    }
    public static DataTable DeleteSameRow(DataTable dt, string Field)
    {
        ArrayList indexList = new ArrayList();
        // 找出待删除的行索引   
        for (int i = 0; i < dt.Rows.Count - 1; i++)
        {
            if (!IsContain(indexList, i))
            {
                for (int j = i + 1; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[i][Field].ToString() == dt.Rows[j][Field].ToString())
                    {
                        indexList.Add(j);
                    }
                }
            }
        }
        // 根据待删除索引列表删除行   
        for (int i = indexList.Count - 1; i >= 0; i--)
        {
            int index = Convert.ToInt32(indexList[i]);
            dt.Rows.RemoveAt(index);
        }
        return dt;
    }
    /// <summary>   
    /// 判断数组中是否存在   
    /// </summary>   
    /// <param name="indexList">数组</param>   
    /// <param name="index">索引</param>   
    /// <returns></returns>   
    public static bool IsContain(ArrayList indexList, int index)
    {
        for (int i = 0; i < indexList.Count; i++)
        {
            int tempIndex = Convert.ToInt32(indexList[i]);
            if (tempIndex == index)
            {
                return true;
            }
        }
        return false;
    }

    protected void btnSearchButton_Click(object sender, EventArgs e)
    {
        if (this.txtName.Text != "")
        {
            string name = this.txtName.Text;
            AllMemberInfos(name);
        }
    }
}