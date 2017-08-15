using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class UpdateManager : AdminPage
    {

        private int clientUserId;
        protected System.Web.UI.WebControls.Button btnCreate;
        protected RoleDropDownList dropRole;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtPassword;
        protected System.Web.UI.WebControls.TextBox txtPasswordagain;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAgentName;
        //2015-11-17日修改
        protected System.Web.UI.WebControls.Literal litTitle;
        protected System.Web.UI.WebControls.Panel PanelID;

        protected void Page_Load(object sender, System.EventArgs e)
        {



        }
    }
}
