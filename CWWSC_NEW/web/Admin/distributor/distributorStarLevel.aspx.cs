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

public partial class Admin_distributor_distributorStarLevel: AdminPage
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
        this.reStarLevel.DataSource = DistributorStarLevelHelper.GetDistributorStarLevelData(" order by LevelNum Asc");
        this.reStarLevel.DataBind();
    }
    protected void reStarLevel_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            if (DistributorStarLevelHelper.DeleteDistributorStarLevel(e.CommandArgument.ToString()))
            {
                this.ShowMsgAndReUrl("删除成功", true,"distributorStarLevel.aspx");
                return;
            }
            else
            {
                this.ShowMsgAndReUrl("删除失败", false, "distributorStarLevel.aspx");
                return;
            }
        }

    }
}