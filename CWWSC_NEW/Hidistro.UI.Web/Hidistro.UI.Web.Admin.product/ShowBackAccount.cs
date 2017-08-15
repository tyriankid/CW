using ControlPanel.Commodities;
using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
    //[PrivilegeCheck(Privilege.ShowBackAccount)]
    public class ShowBackAccount : AdminPage
	{
        protected Literal litAccount;//不通过原因
        protected Literal litBackTime;//不用过时间
        protected System.Web.UI.WebControls.Button btnClose;
        protected HiddenField txtProductIds;//隐藏域,传递的商品ID集合
        
        protected string strid;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.strid = this.Page.Request.QueryString["productId"];

            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.strid))
                {
                    //加载内容
                    DataTable dtBack = ProductBackAccountHelper.GetProductBackAccount(Convert.ToInt32(this.strid));
                    if (dtBack.Rows.Count > 0)
                    {
                        this.litBackTime.Text = Convert.ToDateTime(dtBack.Rows[0]["backTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        this.litAccount.Text = dtBack.Rows[0]["backAccount"].ToString();
                    }
                }
            }
        }


	}
}
