namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList2 : CheckBoxList
    {
        private int repeatColumns = 15;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList2()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("会员名称", "case when name is null then MemberName else name end as "));
            this.Items.Add(new ListItem("性别", "sex as "));
            this.Items.Add(new ListItem("会员电话", "case when mobile is null then Membermobile else mobile end as "));
            this.Items.Add(new ListItem("职业", "profession as "));
            this.Items.Add(new ListItem("门店DZ号", "case when storeCode is null then MemberStoreCode else storeCode end as "));
            this.Items.Add(new ListItem("门店名称", "case when storeNameNew is null then case when StoreName is null then MemberstoreName else StoreName end else storeNameNew end as "));
            this.Items.Add(new ListItem("会员等级", "case when GradeName is null then MembergradeName else GradeName end as "));
            this.Items.Add(new ListItem("会员生日", "birthday as "));
            this.Items.Add(new ListItem("购买产品型号", "model as "));
            this.Items.Add(new ListItem("购买日期", "buydate as "));
            this.Items.Add(new ListItem("产品类型", "typename as "));
            this.Items.Add(new ListItem("产品价格", "price as "));
            this.Items.Add(new ListItem("所属地区", "OldRegion as "));
            this.Items.Add(new ListItem("详细地址", "Address as "));
            this.Items.Add(new ListItem("家庭成员构成", "jiatingchengyuan as "));
            this.Items.Add(new ListItem("用户住房信息", "zhufangxinxi as "));
            this.Items.Add(new ListItem("房屋家电配置", "fangyujiadian as "));
            this.Items.Add(new ListItem("家电使用情况", "jiadianshiyong as "));
            this.Items.Add(new ListItem("个人品牌倾向", "gerenqingxiang as "));
            this.Items.Add(new ListItem("近期购买需求", "jinqixuqiu as "));
            this.Items.Add(new ListItem("备注", "remark as "));
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

