using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace Hidistro.UI.Web.Admin
{  
    public class CWStoreInfoProving : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnInfoProing;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtAccountALLHere;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnInfoProing.Click += new System.EventHandler(this.btnCWInfo_Click);
        }
        private void btnCWInfo_Click(object sender, System.EventArgs e)
        {
            string storeName = this.txtStoreName.Text.Trim();
            string AccountALLHere = this.txtAccountALLHere.Text.Trim();
            DataTable dtProingInfo = StoreInfoHelper.SelectStoreInfoByWhere(string.Format(" storeName = '{0}'", storeName.Trim()));
            if (dtProingInfo.Rows.Count > 0)
            {
                if (dtProingInfo.Rows[0]["accountALLHere"].ToString() == AccountALLHere)
                {
                    this.ShowMsg("信息验证通过！", true);
                    Response.Redirect("ApplicationDescription.aspx", true);
                }
                else {
                    this.ShowMsg("信息验证失败！", false);
                }
            }
            else
            {
                this.ShowMsg("信息验证失败！", false);
            }

        }
    }
}
