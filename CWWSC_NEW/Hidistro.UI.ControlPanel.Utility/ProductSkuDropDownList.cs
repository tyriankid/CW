namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ProductSkuDropDownList : DropDownList
    {
        private int? productId;

        public override void DataBind()
        {
            this.Items.Clear();
            base.Items.Add(new ListItem("--请选择--", string.Empty));
            if (productId.HasValue && productId > 0)
            {
                DataTable dtSku = ProductHelper.GetProductSkuItem(productId.Value);
                foreach (DataRow row in dtSku.Rows)
                {
                    base.Items.Add(new ListItem((row["AttributeName"].ToString()==""?"默认":row["AttributeName"].ToString())+ "：" +(row["ValueStr"].ToString()==""?"无规格":row["ValueStr"].ToString()), row["SkuId"].ToString()));
                }
            }
        }

        public int? ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = value;
            }
        }

        public string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return base.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

