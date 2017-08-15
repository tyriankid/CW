namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList5 : CheckBoxList
    {
        private int repeatColumns = 15;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList5()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("会员名称", "name"));
            this.Items.Add(new ListItem("性别", "sex"));
            this.Items.Add(new ListItem("会员电话", "mobile"));
            this.Items.Add(new ListItem("职业", "profession"));
            this.Items.Add(new ListItem("门店DZ号", "storeCode"));
            this.Items.Add(new ListItem("门店名称", "storeName"));
            this.Items.Add(new ListItem("会员等级", "gradeName"));
            this.Items.Add(new ListItem("会员生日", "birthday"));
            this.Items.Add(new ListItem("购买产品型号", "model"));
            this.Items.Add(new ListItem("购买日期", "buydate"));
            this.Items.Add(new ListItem("产品类型", "typename"));
            this.Items.Add(new ListItem("产品价格", "price"));
            this.Items.Add(new ListItem("所属地区", "OldRegion"));
            this.Items.Add(new ListItem("详细地址", "Address"));
            this.Items.Add(new ListItem("家庭成员构成", "jiatingchengyuan"));
            this.Items.Add(new ListItem("用户住房信息", "zhufangxinxi"));
            this.Items.Add(new ListItem("房屋家电配置", "fangyujiadian"));
            this.Items.Add(new ListItem("家电使用情况", "jiadianshiyong"));
            this.Items.Add(new ListItem("个人品牌倾向", "gerenqingxiang"));
            this.Items.Add(new ListItem("近期购买需求", "jinqixuqiu"));
            this.Items.Add(new ListItem("备注", "remark"));
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

