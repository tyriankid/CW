using ControlPanel.Commodities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_member_AddO2OMember : AdminPage
{
    public int userId;
    protected void Page_Load(object sender, EventArgs e)
    {   
        userId = Convert.ToInt32(Request.QueryString["Id"]);
        if (!IsPostBack)
        {
            IList<StoreInfo> storelist = StoreHelper.GetStore();
            this.ddlStores.DataTextField = "NameAndCode";
            this.ddlStores.DataValueField = "accountALLHere";
            this.ddlStores.DataSource = storelist;
            this.ddlStores.DataBind();

            //this.drpMemberRankList.AllowNull = false;
            //this.drpMemberRankList.DataBind();
            //this.dropProductTypes.DataBind();
            //this.drpstroe.DataBind();

            //2016-08-08验证当前登陆用户类型
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentManager.RoleId == masterSettings.StoreRoleId)
            {
                DistributorsInfo disinfo = VShopHelper.GetUserIdDistributors(currentManager.ClientUserId);
                if(disinfo != null && disinfo.StoreId >0)
                {
                    StoreInfo info = StoreInfoHelper.GetStoreInfo(disinfo.StoreId);
                    if (info != null && !string.IsNullOrEmpty(info.accountALLHere))
                    {
                        //this.drpstroe.SelectedValue = info.accountALLHere;
                        this.ddlStores.SelectedValue = info.accountALLHere;
                    }
                }
                //this.drpstroe.Enabled = false;
                this.ddlStores.Enabled = false;
            }
            if (userId > 0)
            {
                load();
            }
        }
    }
    protected void load() 
    {
        btnCreate.Text = "修改";
        CW_O2OMenbersInfo userInfo = O2OMemberHelper.GetO2OMember(userId);
        this.txtUserName.Text = userInfo.name;
        this.txtPhone.Text = userInfo.mobile;
        this.txtProfession.Text = userInfo.profession;
        //drpMemberRankList.SelectedValue= userInfo.gradeid;
        this.MemberRankList.Text = userInfo.gradeName;//gradeName
        if (!string.IsNullOrEmpty(userInfo.birthday))
        {
            this.timeBirthday.Text = Convert.ToDateTime(userInfo.birthday).ToString("yyyy-MM-dd");
        }
        this.rdoSex.SelectedValue = userInfo.sex;
        this.txtModel.Text = userInfo.model;
        if (!string.IsNullOrEmpty(userInfo.buydate))
        {
            this.timeBuydate.Text = Convert.ToDateTime(userInfo.buydate).ToString("yyyy-MM-dd");
        }        
        this.ProductTypes.Text = userInfo.typename;
        this.txtprice.Text = Convert.ToDouble(userInfo.price).ToString("F2");
        this.ddlReggion.SetSelectedRegionId(new int?(userInfo.regionid));
        this.ddlStores.SelectedValue = userInfo.storeCode;
        
        this.txtAddress.Text = userInfo.Address;
        this.txtRemark.Text = userInfo.remark;
        //净水器
        if (userInfo.IsUserWaterDarifier.HasValue)
        {
            this.radIsUserWaterDarifier.SelectedValue = userInfo.IsUserWaterDarifier.Value == 1 ? true : false;
        }
        if (!string.IsNullOrEmpty(userInfo.BuyWaterDarifierDate))
        {
            this.timeBuyWaterDarifierDate.Text = Convert.ToDateTime(userInfo.BuyWaterDarifierDate).ToString("yyyy-MM-dd");
        }

        this.txtJiatingchengyuan.Text = userInfo.jiatingchengyuan;
        this.txtZhufangxinxi.Text = userInfo.zhufangxinxi;
        this.txtFangyujiadian.Text = userInfo.fangyujiadian;
        this.txtJiadianshiyong.Text = userInfo.jiadianshiyong;
        this.txtGerenqingxiang.Text = userInfo.gerenqingxiang;
        this.txtJinqixuqiu.Text = userInfo.jinqixuqiu;

        userId = Convert.ToInt32(Request.QueryString["Id"]);
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        btnCreate.Enabled = false;
        CW_O2OMenbersInfo O2OMember = new CW_O2OMenbersInfo();
        O2OMember.name = txtUserName.Text;
        O2OMember.mobile = txtPhone.Text;
        O2OMember.profession = txtProfession.Text;
        //gradeid = drpMemberRankList.SelectedValue.Value,
        O2OMember.gradeName = MemberRankList.Text;
        O2OMember.birthday = timeBirthday.Text;
        O2OMember.storeCode = ddlStores.SelectedValue;
        O2OMember.sex = rdoSex.SelectedValue;
        O2OMember.model = txtModel.Text;
        O2OMember.buydate = timeBuydate.Text;
        O2OMember.typename = ProductTypes.Text;
        decimal price = 0;
        if (!string.IsNullOrEmpty(txtprice.Text) && decimal.TryParse(txtprice.Text, out price))
        {
            O2OMember.price = price;
        }
        if (ddlReggion.GetSelectedRegionId() != null)
            O2OMember.regionid = ddlReggion.GetSelectedRegionId().Value;
        O2OMember.Address = txtAddress.Text;
        O2OMember.remark = txtRemark.Text;
        //净水器
        O2OMember.IsUserWaterDarifier = this.radIsUserWaterDarifier.SelectedValue ? 1 : 0; 
        //int.Parse(this.ckIsUserWaterDarifier.SelectedValue);
        O2OMember.BuyWaterDarifierDate = this.timeBuyWaterDarifierDate.Text;

        //附加信息
        O2OMember.jiatingchengyuan = this.txtJiatingchengyuan.Text;
        O2OMember.zhufangxinxi = this.txtZhufangxinxi.Text;
        O2OMember.fangyujiadian = this.txtFangyujiadian.Text;
        O2OMember.jiadianshiyong =  this.txtJiadianshiyong.Text;
        O2OMember.gerenqingxiang = this.txtGerenqingxiang.Text;
        O2OMember.jinqixuqiu =this.txtJinqixuqiu.Text;

        if (userId <= 0)
        {
            O2OMember.CreateDate = DateTime.Now;
            if (O2OMemberHelper.InsertO2OCwMembers(O2OMember) >= 0)
            {
                btnCreate.Enabled = true;
                this.ShowMsgAndReUrl("成功添加了会员", true, "CW_O2OMembers.aspx");
            }
            else
            {
                btnCreate.Enabled = true;
                this.ShowMsg("添加失败!", false);
            }
            
        }
        else
        {
            O2OMember.userid = userId;
            if (O2OMemberHelper.UpdateO2OMembersr(O2OMember))
            {
                btnCreate.Enabled = true;
                this.ShowMsgAndReUrl("修改成功。", true, "CW_O2OMembers.aspx");
                //this.ShowMsg("修改成功", true);
            }
            btnCreate.Enabled = true;
        }

    }



}