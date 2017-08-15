<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well address-box">
    <div class="font-xl">
        <span>【<%#Eval("Type1").ToString() == "0" ? "个人" : "单位"%>】</span>
        <span><%#(Eval("Type1").ToString() == "0" ?"发票抬头：" : "公司名称：") + Eval("CompanyName")%></span><em><%#Eval("Phone")%></em>
            <div>
                <a onclick='UpdateReceipt(<%#Eval("ReceiptId") %>)' href="javascript:void(0)"><span
                    class="glyphicon glyphicon-pencil"></span></a><a href="javascript:void(0)" onclick='DeleteReceipt(<%#Eval("ReceiptId") %>,this)'>
                        <span class="glyphicon glyphicon-trash"></span></a>
            </div>
    </div>
    <div class="font-m"><%# (Eval("Type1").ToString() == "0" ?"" : "纳税识别号：") + Eval("TaxesCode")%>&nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("Address")%></div>
</div>
