using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class SouSuo : BaseModel
    {
        public DataTable dt = null;
        protected Repeater ReUserSearch;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //*****************star 获取当前模块的搜索记录 2017-8-8 yk********************************
            DataTable dtUserSearch = UserSearchLogsHelper.GetUserSearchData(" where FunctionType='Index' order by searchDate desc");
            if (dtUserSearch.Rows.Count > 0)
            {
                this.ReUserSearch.DataSource = dtUserSearch;
                this.ReUserSearch.DataBind();
            }
            //*******************end*********************************************************
            InitData();
        }
        private void InitData()
        {
            dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID + "'");


        }
    }
}
