namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFriendsCircle : VWeiXinOAuthTemplatedWebControl//VshopTemplatedWebControl
    {
        private Pager pager;
        private VshopTemplatedRepeater refriendscircle;
        private Literal litItemParams;

        protected override void AttachChildControls()
        {
            DataTable dt = new DataTable();
            this.refriendscircle = (VshopTemplatedRepeater) this.FindControl("refriendscircle");
            this.pager = (Pager) this.FindControl("pager");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.refriendscircle.ItemDataBound += new RepeaterItemEventHandler(this.refriendscircle_ItemDataBound);
            this.BindData(out dt);
            //PageTitle.AddSiteNameTitle("朋友圈素材");


            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            
            string strImgUrl = "";
            string strName = "";
            string strDescription = "";
            if (dt.Rows.Count > 0)
            {
                string[] strImgs = dt.Rows[0]["ExensionImg"].ToString().Split('|');
                if (strImgs.Length > 0)
                    strImgUrl = "http://" + Globals.DomainName + strImgs[0];
                strName = dt.Rows[0]["ExensiontRemark"].ToString();
                strDescription = strName;
            }

            //构建URL
            string strUrl = HttpContext.Current.Request.Url.ToString();
            if (strUrl.IndexOf("ReferralId") < 0)
            {
                if (HttpContext.Current.Request.Cookies["Vshop-ReferralId"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["Vshop-ReferralId"].ToString()))
                {
                    strUrl += "?ReferralId=" + HttpContext.Current.Request.Cookies.Get("Vshop-ReferralId").Value;
                }
            }

            //this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url) + strImgUrl, "|", strName, "|", strDescription, "|", strUrl });
            this.litItemParams.Text = (string.IsNullOrEmpty(strImgUrl) ? str3 : Globals.HostPath(HttpContext.Current.Request.Url) + strImgUrl) + "|" + (string.IsNullOrEmpty(strName) ? masterSettings.GoodsName : strName) + "|" + (string.IsNullOrEmpty(strDescription) ? masterSettings.GoodsDescription : strDescription) + "|" + strUrl;

        }

        private void BindData(out DataTable frienddt)
        {
            FriendExtensionQuery entity = new FriendExtensionQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "ExtensionId"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VshopBrowser.FriendExtensionList(entity);
            frienddt = (DataTable)result.Data;
            this.refriendscircle.DataSource = result.Data;
            this.refriendscircle.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VFriendsCircle.html";
            }
            base.OnInit(e);
        }

        private void refriendscircle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.Controls[0].FindControl("ImgPic");
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "ExensionImg").ToString()))
                {
                    string[] strArray = DataBinder.Eval(e.Item.DataItem, "ExensionImg").ToString().Split(new char[] { '|' });
                    string str = "";
                    foreach (string str2 in strArray)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            string str3 = str;
                            str = str3 + "<div class=\"col-xs-6\"><img src='http://" + Globals.DomainName + str2 + "' width='150' height='150'/></div>";
                        }
                    }
                    literal.Text = str;
                }
            }
        }
    }
}

