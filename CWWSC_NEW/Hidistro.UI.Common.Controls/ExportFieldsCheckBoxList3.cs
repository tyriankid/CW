namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList3 : CheckBoxList
    {
        private int repeatColumns = 7;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList3()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("分公司名称", "f.fgsName"));
            this.Items.Add(new ListItem("DZ号", "s.accountALLHere"));
            this.Items.Add(new ListItem("门店名称", "s.storeName"));
            this.Items.Add(new ListItem("微会员数", "a.MemerNum"));
            this.Items.Add(new ListItem("录入会员数", "b.lrNum"));
            this.Items.Add(new ListItem("复购会员数", "c.fgNum"));
            this.Items.Add(new ListItem("粘性会员数", "d.nxNum"));
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

