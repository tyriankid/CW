using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class MemberIndividuality : AdminPage
    {
        protected System.Web.UI.WebControls.Repeater ShowIndividuality;
        protected System.Web.UI.WebControls.Repeater Repeater1;
        protected System.Web.UI.WebControls.HiddenField HiddenFieldSave;
        protected System.Web.UI.WebControls.Button btnEditUser;
        /// <summary>
        /// 加载当前用户的标签，并绑定到前台页面，并加载保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string UserId = Request["userid"];
            MembersTagInfo MembersTagInfo = new MembersTagInfo
            {
                userId = UserId
            };
            DataTable dtMembersTagInfo = MembersTagHelper.GetMembersTagInfo(MembersTagInfo);
            ViewState["dtMt"] = dtMembersTagInfo;

            dtMembersTagInfo.Columns.Add("paixu", typeof(int));
            for (int i = 0; i < dtMembersTagInfo.Rows.Count; i++)
            {
                dtMembersTagInfo.Rows[i]["paixu"] = i + 1;
            }
            ShowIndividuality.DataSource = dtMembersTagInfo;
            ShowIndividuality.DataBind();
            //加载保存事件
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
        }
        /// <summary>
        /// 点击保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditUser_Click(object sender, EventArgs e)
        {
            bool add = true;
            string UserId = Request["userid"];
            DataTable dt = (DataTable)ViewState["dtMt"];
            //获取隐藏控件的值
            string ok = this.HiddenFieldSave.Value;
            DataTable newdt = new DataTable();
            newdt.Columns.Add("userId", typeof(string));
            newdt.Columns.Add("tagName", typeof(string));
            newdt.Columns.Add("tagValue", typeof(string));
            newdt.Columns.Add("scode", typeof(string));
            //切割字符串
            string[] allok = ok.Split('/');
            for (int i = 0; i < allok.Length - 1; i++)
            {
                newdt.Rows.Add();
                string ook = allok[i];
                string[] okk = ook.Split(',');
                if (okk[0] == "" || okk[1] == "")
                {
                    this.ShowMsg("标签或标签内容不能为空！请重新输入！", false);
                    add = false;
                }
                else
                {
                    //把切割的数据转载到DataTable
                    newdt.Rows[i]["userId"] = UserId;
                    newdt.Rows[i]["tagName"] = okk[0];
                    newdt.Rows[i]["tagValue"] = okk[1];
                }

            }
            //如果标签或标签内容不为空的时候
            if (add)
            {
                //删除用户的所有标签
                int y = MembersTagHelper.DeleteMembersTagInfoByUserID(UserId, true);
                string sqls = "select * from CW_MembersTag";
                string[] sql = sqls.Split(';');
                DataSet ds = new DataSet();
                ds.Tables.Add(newdt);
                //添加最新的用户标签
                int count = DataBaseHelper.CommitDataSet(ds, sql);
                newdt.Columns.Add("paixu", typeof(int));
                for (int i = 0; i < newdt.Rows.Count; i++)
                {
                    newdt.Rows[i]["paixu"] = i + 1;
                }
                //绑定到前台
                ShowIndividuality.DataSource = newdt;
                ShowIndividuality.DataBind();
            }

        }
    }
}
