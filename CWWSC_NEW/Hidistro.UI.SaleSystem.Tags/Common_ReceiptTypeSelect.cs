namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ReceiptTypeSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string strWhere = string.Format("UserId = '{0}'", currentMember.UserId);
            DataTable userRedPagerCanUse = UserReceiptInfoHelper.SelectUserReceiptInfoByWhere(strWhere);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择发票信息<span class=\"caret\"></span></button>");
            builder.AppendLine("<ul class=\"dropdown-menu\" role=\"menu\">");
            
            builder.AppendLine("<li><a href=\"#\" name=\"0\" value=\"0\">电子发票</a></li>");
            builder.AppendLine("<li><a href=\"#\" name=\"0\" value=\"0\">增值税发票</a></li>");
            //foreach (DataRow row in userRedPagerCanUse.Rows)
            //{
            //    object[] args = new object[] { row["RedPagerID"], row["RedPagerActivityName"], ((decimal)row["OrderAmountCanUse"]).ToString("F2"), ((decimal)row["Amount"]).ToString("F2") };
            //    builder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{3}\">{1}(满{2}抵用{3})</a></li>", args).AppendLine();
            //}
            builder.AppendLine("</ul>");
            writer.Write(builder.ToString());
        }

        //public decimal CartTotal { get; set; }
    }
}

