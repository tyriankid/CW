<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("UserName") %>
    </td>
    <td>
       <%# Eval("VisitDate") %>
    </td>
    <td>
        <span class="money">￥<%#Eval("VisitCountPerday","{0:F2}" )%> </span>
    </td>
</tr>
