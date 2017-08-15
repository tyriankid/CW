namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class CommonTagsLiteral : Literal
    {
        protected string m_selectvalue;

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            DataTable classTags = ServiceClassHelper.SelectClassByWhere();
            if (classTags.Rows.Count < 0)
            {
                base.Text = "暂无品类";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                foreach (DataRow row in classTags.Rows)
                {
                    if (!string.IsNullOrEmpty(m_selectvalue))
                    {
                        string[] selectedIds = m_selectvalue.TrimEnd(',').Split(',');
                        bool isExist = false;
                        foreach (string id in selectedIds)
                        {
                            if (row["ScID"].ToString().Equals(id))
                                isExist = true;
                        }
                        if (isExist)
                            builder.AppendFormat("<li class=\"selected\">{1}<span class=\"tag\" tagid=\"{0}\">x</span></li>", row["ScID"].ToString(), row["ClassName"].ToString());
                        else
                            builder.AppendFormat("<li>{1}<span class=\"tag\" tagid=\"{0}\">x</span></li>", row["ScID"].ToString(), row["ClassName"].ToString());
                    }
                    else
                        builder.AppendFormat("<li>{1}<span class=\"tag\" tagid=\"{0}\">x</span></li>", row["ScID"].ToString(), row["ClassName"].ToString());
                }
                base.Text = builder.ToString();
                base.Render(writer);
            }
        }

        public string SelectedValue
        {
            set
            {
                this.m_selectvalue = value;
            }
        }

        //public IList<int> SelectedValue
        //{
        //    set
        //    {
        //        this.m_selectvalue = value;
        //    }
        //}
    }
}

