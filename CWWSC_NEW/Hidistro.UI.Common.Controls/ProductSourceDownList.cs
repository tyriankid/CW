namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class ProductSourceDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "全部";
        private bool allowCw = true;

        public override void DataBind()
        {
            this.Items.Clear();
            IList<SupplierInfo> supplierInfos = SupplierHelper.GetListSupplier();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            if (this.allowCw)
            {
                //添加创维选项
                base.Items.Add(new ListItem("创维商品", "0"));
            }

            //循环添加供应商
            foreach (SupplierInfo info in supplierInfos)
            {
                base.Items.Add(new ListItem(info.gysName, info.Id.ToString()));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        public bool AllowCw
        {
            get 
            {
                return this.allowCw;
            }
            set 
            {
                this.allowCw = value;
            }
        }

        new public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (value.HasValue && (value > 0))
                {
                    base.SelectedValue = value.Value.ToString();
                }
                else
                {
                    base.SelectedValue = string.Empty;
                }
            }
        }
    }
}

