namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class JINLIMemberTagsLiteral : Literal
    {
        protected string m_selectvalue;

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            DataTable classTags = memberTagsHelper.GetmemberTagsData(" where TagType=1");
            if (classTags.Rows.Count < 0)
            {
                base.Text = "暂无标签";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                foreach (DataRow row in classTags.Rows)
                {
                    if (!string.IsNullOrEmpty(m_selectvalue))
                    {
                        string[] selectedIds = m_selectvalue.Split(',');
                        bool isExist = false;
                        foreach (string id in selectedIds)
                        {
                            if (row["TagID"].ToString().Equals(id))
                                isExist = true;
                        }
                        if (isExist)
                            builder.AppendFormat("<li id=\"{2}\" class=\"selected\">{1}<span  style=\"display: inline;\"  class=\"tag\" tagid=\"{0}\">x</span></li>", row["TagID"].ToString(), row["TagName"].ToString(), row["TagID"].ToString());
                        else
                            builder.AppendFormat("<li id=\"{2}\">{1}<span class=\"tag\" tagid=\"{0}\">x</span></li>", row["TagID"].ToString(), row["TagName"].ToString(), row["TagID"].ToString());
                    }
                    else
                        builder.AppendFormat("<li id=\"{2}\">{1}<span class=\"tag\" tagid=\"{0}\">x</span></li>", row["TagID"].ToString(), row["TagName"].ToString(), row["TagID"].ToString());
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
    }
}

