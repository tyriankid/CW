namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class MinuteDropDownList : DropDownList
    {
        public override void DataBind()
        {
            this.Items.Clear();
            for (int i = 0; i <= 59; i++)
            {
                string text = i + "分";
                this.Items.Add(new ListItem(text, i.ToString()));
            }
        }

        new public int? SelectedValue
        {
            get
            {
                int result = 0;
                int.TryParse(base.SelectedValue, out result);
                return new int?(result);
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedValue = value.Value.ToString();
                }
            }
        }
    }
}

