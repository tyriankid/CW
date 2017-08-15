namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList4 : CheckBoxList
    {
        private int repeatColumns = 9;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList4()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("姓名", "UserName"));
            this.Items.Add(new ListItem("AllHere账号", "accountALLHere"));
            this.Items.Add(new ListItem("门店名称", "StoreName"));
            this.Items.Add(new ListItem("电话", "CellPhone"));
            this.Items.Add(new ListItem("地址", "Address"));
            this.Items.Add(new ListItem("商品编码", "ProductCode"));
            this.Items.Add(new ListItem("商品型号", "ProductModel"));
            this.Items.Add(new ListItem("单价", "Price"));
            this.Items.Add(new ListItem("购买数量", "BuyNum"));
        }

        public override int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}

