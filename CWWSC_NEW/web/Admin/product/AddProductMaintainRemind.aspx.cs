using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Excel;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using Hidistro.Core.Entities;
using Hidistro.Core;
using Hidistro.Core.Enums;
using System.Collections.Specialized;



namespace Hidistro.UI.Web.Admin.product
{
    public partial class AddProductMaintainRemind : AdminPage
    {
        string MrID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSaveClientSettings_Click);
            MrID = this.Page.Request.QueryString["MrID"];
            DataTable dt = ProductHelper.GetStoreProductBaseInfo( );
            ReProduct.DataSource = dt;
            ReProduct.DataBind();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(MrID))
                {
                    Bind();
                }
            }

        }
        protected void Bind()
        {
            //编辑加载
            ProductMaintainRemindInfo remindInfo = ProductMaintainRemindHelper.GetProductMaintainRemindInfo(int.Parse(MrID));
            if (remindInfo != null)
            {   
                DataTable dt=new DataTable();
                if(!string.IsNullOrEmpty(MrID))
                {   
                    dt= ProductHelper.GetStoreProductBaseInfo(" where productId="+remindInfo.ProductId+"");   
                }
                this.hidProduct.Value = remindInfo.ProductId.ToString();
                this.RemindCycle.Text = remindInfo.RemindCycle.ToString();
                this.RemindNum.Text = remindInfo.RemindNum.ToString();
                this.RemindRemark.Text = remindInfo.RemindRemark.ToString();
                if (dt.Rows.Count > 0)
                {
                    this.product.Text = dt.Rows[0]["ProductName"].ToString();
                }
            }

        }
        protected void btnSaveClientSettings_Click(object sender, EventArgs e)
        {
            //编辑保存
            if (!string.IsNullOrEmpty(MrID))
            {
                ProductMaintainRemindInfo remindInfo = ProductMaintainRemindHelper.GetProductMaintainRemindInfo(int.Parse(MrID));
                remindInfo.ProductId = int.Parse(this.hidProduct.Value);
                remindInfo.RemindCycle = int.Parse(this.RemindCycle.Text);
                remindInfo.RemindRemark =this.RemindRemark.Text;
                remindInfo.RemindNum = int.Parse(this.RemindNum.Text);
                if (ProductMaintainRemindHelper.UpdateProductMaintainRemind(remindInfo))
                {
                    this.ShowMsgAndReUrl("修改成功", true, "productMaintenanceReminder.aspx");
                }
            }
            //添加保存
            else
            {
                ProductMaintainRemindInfo remindInfo = new ProductMaintainRemindInfo
                {

                    ProductId = int.Parse(this.hidProduct.Value),
                    RemindCycle = int.Parse(this.RemindCycle.Text),
                    RemindRemark = this.RemindRemark.Text,
                    RemindNum = int.Parse(this.RemindNum.Text),
                };
                if (ProductMaintainRemindHelper.AddProductMaintainRemind(remindInfo))
                {
                    this.ShowMsgAndReUrl("新增成功", true, "productMaintenanceReminder.aspx");
                }
            }
        }
    }
}

