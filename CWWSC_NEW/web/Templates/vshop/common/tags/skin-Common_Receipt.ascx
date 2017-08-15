<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li><a href="#" ReceiptId="<%# Eval("ReceiptId")%>" briefReceipt="<%# (Eval("Type").ToString() == "0" ? "电子发票：" + " " + (Eval("Type1").ToString() == "0" ? "【个人】 " : "【单位】 ") : "增值税发票：") + Eval("CompanyName")+" "+ Eval("TaxesCode") %>"> 
    <%# (Eval("Type").ToString() == "0" ? "电子发票：" + " " + (Eval("Type1").ToString() == "0" ? "【个人】 " : "【单位】 ") : "增值税发票：") + Eval("CompanyName")+" "+ Eval("TaxesCode") %>
</a>

</li>
