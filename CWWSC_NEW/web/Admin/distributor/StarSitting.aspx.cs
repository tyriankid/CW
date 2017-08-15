using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_distributor_StarSitting : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnScore.Click += new System.EventHandler(this.btnScore_Click);
        if (!IsPostBack)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //门店评价
            if (!string.IsNullOrEmpty(masterSettings.StoreCommon))
            {
                string[] storecommon = masterSettings.StoreCommon.Split(',');
                this.txtstorecommon.Text = storecommon[0];
                this.txtstandardStorecommon.Text = storecommon[1];
            }
            //在线销售
            if (!string.IsNullOrEmpty(masterSettings.OnlineSale))
            {
                string[] OnlineSale = masterSettings.OnlineSale.Split(',');
                this.txtonlinesale.Text = OnlineSale[0];
                this.txtstandardOnlinesale.Text = OnlineSale[1];
            }
            //金力销售
            if (!string.IsNullOrEmpty(masterSettings.JinliSale))
            {
                string[] JinliSale = masterSettings.JinliSale.Split(',');
                this.txtjinlisale.Text = JinliSale[0];
                this.txtstandardJinlisale.Text = JinliSale[1];
            }
            //用户数量
            if (!string.IsNullOrEmpty(masterSettings.MemberNum))
            {
                string[] MemberNum = masterSettings.MemberNum.Split(',');
                this.txtmembernum.Text = MemberNum[0];
                this.txtstandardMembernum.Text = MemberNum[1];
            }
            //粘性会员数
            if (!string.IsNullOrEmpty(masterSettings.NXmemberNum))
            {
                string[] NXmemberNum = masterSettings.NXmemberNum.Split(',');
                this.txtnxmembernum.Text = NXmemberNum[0];
                this.txtstandardNXmembernum.Text = NXmemberNum[1];
            }
            //服务订单数
            if (!string.IsNullOrEmpty(masterSettings.OrderNum))
            {
                string[] OrderNum = masterSettings.OrderNum.Split(',');
                this.txtordernum.Text = OrderNum[0];
                this.txtstandardOrdernum.Text = OrderNum[1];
            }
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        if (!string.IsNullOrEmpty(this.txtstorecommon.Text) && !string.IsNullOrEmpty(this.txtstandardStorecommon.Text))
        {
            masterSettings.StoreCommon = this.txtstorecommon.Text + "," + this.txtstandardStorecommon.Text;
        }
        
        if (!string.IsNullOrEmpty(this.txtonlinesale.Text) && !string.IsNullOrEmpty(this.txtstandardOnlinesale.Text))
        {
            masterSettings.OnlineSale = this.txtonlinesale.Text + "," + this.txtstandardOnlinesale.Text;
        }
       
        if (!string.IsNullOrEmpty(this.txtjinlisale.Text) && !string.IsNullOrEmpty(this.txtstandardJinlisale.Text))
        {
            masterSettings.JinliSale = this.txtjinlisale.Text + "," + this.txtstandardJinlisale.Text;
        }
       
        if (!string.IsNullOrEmpty(this.txtmembernum.Text) && !string.IsNullOrEmpty(this.txtstandardMembernum.Text))
        {
            masterSettings.MemberNum = this.txtmembernum.Text + "," + this.txtstandardMembernum.Text;
        }
     
        if (!string.IsNullOrEmpty(this.txtnxmembernum.Text) && !string.IsNullOrEmpty(this.txtstandardNXmembernum.Text))
        {
            masterSettings.NXmemberNum = this.txtnxmembernum.Text + "," + this.txtstandardNXmembernum.Text;
        }
      
        if (!string.IsNullOrEmpty(this.txtordernum.Text) && !string.IsNullOrEmpty(this.txtstandardOrdernum.Text))
        {
            masterSettings.OrderNum = this.txtordernum.Text + "," + this.txtstandardOrdernum.Text;
        }
        SettingsManager.Save(masterSettings);
        this.ShowMsg("修改成功", true);
    }
    public static decimal  judge(string  str,decimal strs)
    {
        if (string.IsNullOrEmpty(str))
        {
            str ="0";
        }
        else if (decimal.Parse(str)> strs)
        {
            str="1";
        }
        return decimal.Parse(str);
    }

    /// <summary>
    /// 评分
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnScore_Click(object sender, EventArgs e)
    {
      
        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        if (string.IsNullOrEmpty(masterSettings.StoreCommon) || string.IsNullOrEmpty(masterSettings.OnlineSale) || string.IsNullOrEmpty(masterSettings.JinliSale) || string.IsNullOrEmpty(masterSettings.MemberNum) || string.IsNullOrEmpty(masterSettings.NXmemberNum) || string.IsNullOrEmpty(masterSettings.OrderNum))
        {
            this.ShowMsg("门店评分权重分值或满分标准未完善,暂不能评分", false);
            return;
        }
        DataTable dt = DistributorsBrower.GetDisTrobutorData();
        DataTable StarLevel = DistributorStarLevelHelper.GetDistributorStarLevelData();
        DataTable Data = DistributorsBrower.getStoreSixScoreData();
        try
        {
           
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataRow rows in Data.Rows)
                    {
                        if (row["UserId"].Equals(rows["UserID"]))
                        {
                            #region 评分计算
                            //维度实际得分计算方式  （实际数据/满分标准*权重分值）
                            //星级评分计算方式【六项维度的实际得分总和】
                            //门店评价实际得分  x*权重分值 xx*标准分值 xxx*实际分值
                            //如果维度统计的结果大于满分标准 ，则取值1*权重*10=？？
                            string[] storecommon = masterSettings.StoreCommon.Split(',');
                            int x1 = int.Parse(storecommon[0].ToString());
                            int xx1 = int.Parse(storecommon[1].ToString());
                            decimal StoreSum =judge(rows["orderSumMoney"].ToString(),xx1)/ xx1 * x1;
                            //在线销售实际得分
                            string[] OnlineSale = masterSettings.OnlineSale.Split(',');
                            int x2 = int.Parse(OnlineSale[0].ToString());
                            int xx2 = int.Parse(OnlineSale[1].ToString());
                            decimal OnlineSaleSum = judge(rows["orderSumMoney"].ToString(), xx2) / xx2 * x2;
                            //金力销售实际得分
                            string[] JinliSale = masterSettings.JinliSale.Split(',');
                            int x3 = int.Parse(JinliSale[0].ToString());
                            int xx3 = int.Parse(JinliSale[1].ToString());
                            decimal JinliSaleSum = judge(rows["JLmemberCount"].ToString(), xx3) / xx3 * x3;
                            //用户数量实际得分
                            string[] MemberNum = masterSettings.MemberNum.Split(',');
                            int x4 = int.Parse(MemberNum[0].ToString());
                            int xx4 = int.Parse(MemberNum[1].ToString());
                            decimal MemberNumSum = judge(rows["memberCount"].ToString(), xx4) / xx4 * x4;
                            //粘性会员数实际得分
                            string[] NXmemberNum = masterSettings.NXmemberNum.Split(',');
                            int x5 = int.Parse(NXmemberNum[0].ToString());
                            int xx5 = int.Parse(NXmemberNum[1].ToString());
                            decimal NXmemberNumSum = judge(rows["NXCount"].ToString(), xx5) / xx5 * x5;
                            //服务订单数实际得分
                            string[] OrderNum = masterSettings.OrderNum.Split(',');
                            int x6 = int.Parse(OrderNum[0].ToString());
                            int xx6 = int.Parse(OrderNum[1].ToString());
                            decimal OrderNumSum = judge(rows["orderCountNum"].ToString(), xx6) / xx6 * x6;
                            //得到六个维度得分的总和
                            decimal sum = StoreSum + OnlineSaleSum + JinliSaleSum + MemberNumSum + NXmemberNumSum + OrderNumSum;
                            row["StarTotal"] = sum;
                            //获取星级评分标准的信息
                            if (StarLevel.Rows.Count > 0)
                            {
                                //得到当前门店维度总和所属星级
                                foreach (DataRow rowss in StarLevel.Rows)
                                {
                                    if (decimal.Parse(rowss["MinNum"].ToString()) <= sum && sum < decimal.Parse(rowss["MaxNum"].ToString()))
                                    {
                                        row["StarLevelID"] = new Guid(rowss["StarLevelID"].ToString());
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            string sql = "select*from aspnet_Distributors";
            int count = DataBaseHelper.CommitDataTable(dt, sql);
            if (count > 0)
            {
                this.ShowMsg("评分成功！", true);
            }
        }
        catch (Exception ex)
        {
            this.ShowMsg(ex.Message, true);
        }
    }
}