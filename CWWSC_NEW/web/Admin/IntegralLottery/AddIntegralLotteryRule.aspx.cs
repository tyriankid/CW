using Hidistro.Entities.Lottery;
using ControlPanel.Lottery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Hidistro.UI.ControlPanel.Utility;

public partial class Admin_IntegralLottery_AddIntegralLotteryRule : AdminPage
{
    public string action;
    bool isAdd;
    protected void Page_Load(object sender, EventArgs e)
    {
        getMaxClass();

        if (!string.IsNullOrEmpty(Request.QueryString["IsAdd"]))
        {
            isAdd = Convert.ToBoolean(Request.QueryString["IsAdd"].ToString());
        }
        if (!IsPostBack)
        {
            action = Request.QueryString["Action"];
            load();
        }
    }

    public int getMaxClass()
    {
        string from = @"select MaxCount=ISNULL(MAX(LotteryClass),0) from Hishop_LotteryRule";
        int rMax = int.Parse(LotteryRuleHelper.GetSql(from).Rows[0]["MaxCount"].ToString());
        return 1;
    }


    private void load()
    {
        switch (action)
        {
            case "Update":
                if (!string.IsNullOrEmpty(Request.QueryString["RuleId"]))
                {
                    LotteryRuleInfo lotteryRuleInfo = LotteryRuleHelper.LoadInfo(new Guid(Request.QueryString["RuleId"].ToString()));
                    txtLotteryClass.Text = lotteryRuleInfo.LotteryClass.ToString();
                    txtLotteryItem.Text = lotteryRuleInfo.LotteryItem;
                    txtLotteryProportion.Text = lotteryRuleInfo.LotteryProportion.ToString();
                    txtProductName.Text = lotteryRuleInfo.Name;
                    isAdd = false;
                }
                break;
            
        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string from = @"select * from Hishop_LotteryRule";

        DataTable dt = LotteryRuleHelper.GetSql(from);
        LotteryRuleInfo lotteryRuleInfo = null;
        if (isAdd)
        {
            lotteryRuleInfo = new LotteryRuleInfo();
            lotteryRuleInfo.RuleId = Guid.NewGuid();
            lotteryRuleInfo.LotteryItem = txtLotteryItem.Text.Trim();
            if (txtLotteryItem.Text.Trim() == "谢谢参与" || txtLotteryItem.Text.Trim() == "欢迎再来" || txtLotteryItem.Text.Trim() == "再来一次")
            {
                lotteryRuleInfo.GiftId = 0;
            }
            else
            {
                lotteryRuleInfo.GiftId = int.Parse(hiProductId.Value);
            }
            lotteryRuleInfo.LotteryProportion = int.Parse(txtLotteryProportion.Text.Trim());
            lotteryRuleInfo.Name = txtProductName.Text.Trim();
            lotteryRuleInfo.LotteryClass = int.Parse(txtLotteryClass.Text.Trim());
            if (dt.Select("LotteryItem='" + txtLotteryItem.Text.Trim() + "'").Length > 0)
            {
                Response.Write("<script>alert('" + txtLotteryItem.Text.Trim() + "已存在!')</script>");
                return;
            }
            if (dt.Select("Name='" + txtProductName.Text.Trim() + "'").Length > 0)
            {
                Response.Write("<script>alert('商品" + txtProductName.Text + "已存在!')</script>");
                return;
            }
            if (dt.Select("LotteryClass='" + txtLotteryClass.Text.Trim() + "'").Length > 0)
            {
                Response.Write("<script>alert('已存在录入等级!')</script>");
                return;
            }
            if (LotteryRuleHelper.AddLotteryRule(lotteryRuleInfo)) {
                ShowMsgAndReUrl("添加成功", true, "IntegralLotteryRule.aspx");
            }
            else
            {
                ShowMsgAndReUrl("添加失败", false, "IntegralLotteryRule.aspx");
            }
        }
        if (!isAdd)
        {
            lotteryRuleInfo = LotteryRuleHelper.LoadInfo(new Guid(Request.QueryString["RuleId"]));
            lotteryRuleInfo.LotteryItem = txtLotteryItem.Text.Trim();
            lotteryRuleInfo.LotteryProportion = int.Parse(txtLotteryProportion.Text.Trim());
            if (!string.IsNullOrEmpty(Request.QueryString["GiftId"]))
            {
                lotteryRuleInfo.GiftId = int.Parse(Request.QueryString["GiftId"]);
            }
            lotteryRuleInfo.Name = txtProductName.Text.Trim();
            lotteryRuleInfo.LotteryClass = int.Parse(txtLotteryClass.Text.Trim());
            if (dt.Select("LotteryItem='" + txtLotteryItem.Text.Trim() + "' and RuleId<>'" + lotteryRuleInfo.RuleId.ToString() + "'").Length > 0)
            {
                Response.Write("<script>alert('" + txtLotteryItem.Text.Trim() + "已存在!')</script>");
                return;
            }
            if (dt.Select("Name='" + txtProductName.Text.Trim() + "' and RuleId<>'" + lotteryRuleInfo.RuleId + "'").Length > 0)
            {
                Response.Write("<script>alert('商品" + txtProductName.Text.Trim() + "已存在!')</script>");
                return;
            }
            if (dt.Select("LotteryClass='" + txtLotteryClass.Text.Trim() + "' and  RuleId<>'" + lotteryRuleInfo.RuleId + "'").Length > 0)
            {
                Response.Write("<script>alert('已存在录入等级!')</script>");
                return;
            }
            if (LotteryRuleHelper.UpdateLotteryRule(lotteryRuleInfo)) {
                ShowMsgAndReUrl("修改成功", true, "IntegralLotteryRule.aspx");
            }
            else
            {
                ShowMsgAndReUrl("修改失败", false, "IntegralLotteryRule.aspx");
            }
        }
        
    }



    protected void btnChoose_Click(object sender, EventArgs e)
    {

    }
    
}