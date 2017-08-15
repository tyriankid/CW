using ControlPanel.Commodities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_AddO2OSubsidiary :AdminPage
{   
    private int ColId;
    protected void Page_Load(object sender, EventArgs e)
    {
        ColId = Convert.ToInt32(Request.QueryString["ColId"]);
        if (!IsPostBack)
        {
            if (ColId > 0)
            {
                load();
            }
        }
    }
    protected void load()
    {
        rdoType.Enabled = false;
        btnCreate.Text = "修改";
        litTitle.Text = "修改录入会员附属信息列名";
        CW_O2OSubsidiaryInfo listInfo=O2OSubsidiaryHelper.GetO2OSubsidiary(ColId);
        rdoType.SelectedValue = listInfo.type;
        txtListName.Text=listInfo.ColName;
        txtSort.Text = listInfo.scode;
       
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
            CW_O2OSubsidiaryInfo O2OListName = new CW_O2OSubsidiaryInfo
            {
                type= rdoType.SelectedValue,
                ColName = txtListName.Text,
                scode = txtSort.Text
            };
            if (ColId <= 0)
            {
                if (O2OSubsidiaryHelper.InsertO2OSubsidiary(O2OListName) >= 0)
                {
                    this.ShowMsg("成功添加了会员", true);
                }
                else
                {
                    this.ShowMsg("添加失败!", false);
                }
            }
            else
            {

                O2OListName.ColId = ColId;
                if (O2OSubsidiaryHelper.UpdateO2OSubsidiary(O2OListName))
                {
                    this.ShowMsg("修改成功", true);
                }

            }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      