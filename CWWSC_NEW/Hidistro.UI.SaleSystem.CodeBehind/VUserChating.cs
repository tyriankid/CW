using System;
using System.Web.UI;
using Hidistro.UI.Common.Controls;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System.Web.UI.HtmlControls;
using Hidistro.Entities.Commodities;
using Hidistro.Core.Enums;
using Hidistro.Core.Entities;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using System.Data;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true), WeiXinOAuth(Common.Controls.WeiXinOAuthPage.VMemberCenter)]
    public class VUserChating : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptDialogList;
        private HtmlInputHidden txtTotal;
        private HtmlInputHidden currentUserId;
        private HtmlInputHidden currentDomain;
        public int userid;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            /*******Start 2017-07-04，修改适用于店长和店员*******/
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            userid = currentMember.UserId;
            /****End******/

            
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            this.currentUserId = (HtmlInputHidden)this.FindControl("currentUserId");
            this.currentUserId.SetWhenIsNotNull(currentMember.UserId.ToString());
            this.rptDialogList = (VshopTemplatedRepeater)this.FindControl("rptDialogList");
            this.currentDomain = (HtmlInputHidden)this.FindControl("currentDomain");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }


            //查询条件
            string keyword = this.Page.Request.QueryString["keyWord"];
            string isdate = this.Page.Request.QueryString["isdate"];
            //排序字段与规则
            string strsort = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(strsort))
            {
                strsort = "CreateTime";
            }
            string strorder = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(strorder))
            {
                strorder = "desc";
            }

            UserDialogInfoQuery query = new UserDialogInfoQuery();
            query.FQUserId = currentMember.UserId;
            query.IsCount = true;
            query.PageIndex = num;
            query.PageSize = num2;
            query.SortBy = strsort;
            query.SortOrder = strorder.ToLower() == "asc" ? SortAction.Asc : SortAction.Desc;

            DbQueryResult dtdialoglist = DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "CW_UserDialogInfo ud left join aspnet_members amf on ud.FQUserid=amf.userid left join aspnet_members amj on ud.JSUserId = amj.userid left join CW_UserNotRead unr on unr.JSUserId = ud.JSUserId and unr.FQUserId = ud.FQUserId" , "DialogID", " ud.FQUserid = " + query.FQUserId+" or ud.JSUserid = "+query.FQUserId, "unr.NotReadMsgCount,ud.*,amf.username as FQUserName,amf.userhead as FQUserHead,amj.username as JSUserName,amj.userhead as JSUserHead ," + currentMember.UserId + " as currentUserid");
            //获取用户最近一次聊天信息
            ((DataTable)dtdialoglist.Data).Columns.Add("lastMsg");
            foreach (DataRow row in ((DataTable)dtdialoglist.Data).Rows)
            {
                string webpath = "/ChatMessage/" +row["RoomNum"].ToString() + "/";
                string filename = DateTime.Parse(row["CreateTime"].ToString()).ToString("yyyy-MM-dd") + ".txt";
                string str1 = File.ReadAllText((HttpContext.Current.Server.MapPath(webpath + filename)));
                DataTable dtMsgInfo = (DataTable)JsonConvert.DeserializeObject("[" + str1.Substring(0, str1.Length - 3) + "]", typeof(DataTable));
                row["lastMsg"] = dtMsgInfo.Rows[dtMsgInfo.Rows.Count-1]["content"];
            }

            this.rptDialogList.DataSource = dtdialoglist.Data;
            this.rptDialogList.DataBind();
            this.txtTotal.SetWhenIsNotNull(dtdialoglist.TotalRecords.ToString());
            
            PageTitle.AddSiteNameTitle("聊天列表");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.currentDomain.SetWhenIsNotNull(masterSettings.CurrentDomain);
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VUserChating.html";
            }
            base.OnInit(e);
        }
    }
}

