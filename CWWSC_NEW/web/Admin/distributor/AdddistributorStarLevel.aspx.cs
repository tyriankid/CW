using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_distributor_AdddistributorStarLevel : AdminPage
{
    string StarLevelID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        StarLevelID = this.Page.Request.QueryString["StarLevelID"];
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(StarLevelID))
            {
                Bind();
            }
        }
    }
    protected void Bind()
    {
        //编辑加载
        DistributorStarLevelInfo LevalInfo = DistributorStarLevelHelper.GetDistributorStarLevelInfo(StarLevelID);
        if (LevalInfo != null)
        {
            this.txtLevelName.Text = LevalInfo.LevelName;
            this.txtLevelNum.Text = LevalInfo.LevelNum.ToString();
            this.txtMinNum.Text = LevalInfo.MinNum.ToString();
            this.txtMaxNum.Text = LevalInfo.MaxNum.ToString();
            if (LevalInfo.CommissionType > 0)
            {
                if (LevalInfo.CommissionType == 1)
                {
                    this.RadioTypeA.Checked = true;
                    this.txtCommissionRise.Enabled = true;
                }
                else
                {
                    this.RadioTypeB.Checked = true;
                    this.txtCommissionMoney.Enabled = true;
                }
            }

            if (LevalInfo.CommissionRise > 0)
                this.txtCommissionRise.Text = LevalInfo.CommissionRise.ToString("F2");
            if (LevalInfo.CommissionMoney > 0)
                this.txtCommissionMoney.Text = LevalInfo.CommissionMoney.ToString("F0");
            this.IcoUpImg.UploadedImageUrl = LevalInfo.Ico.ToString();
        }

    }
    protected void btnSaveClientSettings_Click(object sender, EventArgs e)
    {
        int itype = 0;
        decimal commvalue = 0;
        decimal commvaluemoney = 0;
        if (this.RadioTypeA.Checked)
            itype = 1;
        else if (this.RadioTypeB.Checked)
            itype = 2;
        //编辑保存
        if (!string.IsNullOrEmpty(StarLevelID))
        {
            DistributorStarLevelInfo LevalInfo = DistributorStarLevelHelper.GetDistributorStarLevelInfo(StarLevelID);

            LevalInfo.LevelName = this.txtLevelName.Text;
            LevalInfo.LevelNum = this.txtLevelNum.Text == "" ? 0 : int.Parse(this.txtLevelNum.Text);
            LevalInfo.MinNum =this.txtMinNum.Text == "" ? 0 : int.Parse(this.txtMinNum.Text);
            LevalInfo.MaxNum = this.txtMaxNum.Text == "" ? 0 : int.Parse(this.txtMaxNum.Text);
            LevalInfo.CommissionType = itype;
            if (decimal.TryParse(this.txtCommissionRise.Text, out commvalue))
                LevalInfo.CommissionRise = commvalue;
            else
                LevalInfo.CommissionRise = 0;
            if (decimal.TryParse(this.txtCommissionMoney.Text, out commvaluemoney))
                LevalInfo.CommissionMoney = commvaluemoney;
            else
                LevalInfo.CommissionMoney = 0;
            LevalInfo.Ico = this.IcoUpImg.UploadedImageUrl;
            if (DistributorStarLevelHelper.UpdateDistributorStarLevel(LevalInfo))
            {
                this.ShowMsgAndReUrl("修改成功", true, "distributorStarLevel.aspx");
            }
        }
        //添加保存
        else
        {

            DistributorStarLevelInfo LevalInfo = new DistributorStarLevelInfo
            {
                StarLevelID = Guid.NewGuid(),
                LevelName = this.txtLevelName.Text,
                LevelNum = this.txtLevelNum.Text == "" ? 0 : int.Parse(this.txtLevelNum.Text),
                MinNum = this.txtMinNum.Text == "" ? 0 : int.Parse(this.txtMinNum.Text),
                MaxNum = this.txtMaxNum.Text == "" ? 0 : int.Parse(this.txtMaxNum.Text),
                CommissionType = itype,
                CommissionRise = decimal.TryParse(this.txtCommissionRise.Text, out commvalue) ? commvalue : 0,
                CommissionMoney = decimal.TryParse(this.txtCommissionMoney.Text, out commvaluemoney) ? commvaluemoney : 0,
                Ico = this.IcoUpImg.UploadedImageUrl,
            };
            if (DistributorStarLevelHelper.AddDistributorMarks(LevalInfo))
            {
                this.ShowMsgAndReUrl("新增成功", true, "distributorStarLevel.aspx");
            }
           

        }

    }
}