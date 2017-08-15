using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class AddManager : AdminPage
    {
        private int clientuserId = 0;
        private bool isFliale = false;
        private bool isSupplier = false;
        private int userId = 0;
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

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else
            {
                ManagerInfo manager = new ManagerInfo
                {
                    RoleId = this.dropRole.SelectedValue,
                    UserName = this.txtUserName.Text.Trim(),
                    Email = this.txtEmail.Text.Trim(),
                    Password = HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()),
                    ClientUserId = clientuserId > 0 ? clientuserId : 0,
                    AgentName = this.txtAgentName.Text.Trim()
                };

                //int i = ManagerHelper.Create(manager);
                if (ManagerHelper.Create(manager))
                {
                    this.txtEmail.Text = string.Empty;
                    this.txtUserName.Text = string.Empty;
                    this.ShowMsg("成功添加了一个管理员", true);

                    //跳转地址
                    if (isSupplier)
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/store/ListSupplier.aspx"), true);
                    if (isFliale)
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/store/ListFiliale.aspx"), true);
                    else
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/distributor/DistributorList.aspx"), true);
                }
                else
                {
                    this.ShowMsg("添加失败!", false);
                }
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRole.DataBind();
            }

            //存储Url参数值 clientUserId
            //存储Url参数值 clientUserId

            //获取配置文件
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            if (this.Page.Request.QueryString["userId"] != null)
            {
                userId = Convert.ToInt32(this.Page.Request.QueryString["userId"].ToString());
            }

            if (this.Page.Request.QueryString["fgsid"] != null)
            {
                clientuserId = Convert.ToInt32(this.Page.Request.QueryString["fgsid"].ToString());
                dropRole.SelectedValue = masterSettings.FilialeRoleId;
                dropRole.Enabled = false;
                isFliale = true;
                //默认后台名称为供应商名称
                FilialeInfo filialeinfo = FilialeHelper.GetFiliale(clientuserId);
                this.txtAgentName.Text = filialeinfo.fgsName;
            }
            if (this.Page.Request.QueryString["gysid"] != null)
            {
                clientuserId = Convert.ToInt32(this.Page.Request.QueryString["gysid"].ToString());
                dropRole.SelectedValue = masterSettings.SupplierRoleId;
                dropRole.Enabled = false;
                isSupplier = true;
                //默认后台名称为供应商名称
                SupplierInfo supplierinfo = SupplierHelper.GetSupplier(clientuserId);
                this.txtAgentName.Text = supplierinfo.gysName;
            }

            if (ManagerHelper.ExistClientUserId(dropRole.SelectedValue, clientuserId))
            {
                //已经分配过后台账号
                this.PanelID.Visible = false;//禁用保存按钮
            }

        }
    }
}
