<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_blank">
        <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
        <div class="info">
            <div class="name font-l bcolor"> <%# Eval("ProductName")%> </div>
        </div>
    </a>
    <div class="ask-answer">
        <p class="ask"><%# Eval("ConsultationText")%></p>
        <div class="dateTime font-s text-muted" style="text-align:right">
            提问时间：<%# Eval("ConsultationDate")%>&nbsp;&nbsp;&nbsp;&nbsp;<a href="ProductConsultationsReplyed.aspx?consultationId=<%# Eval("ConsultationId")%>&ProductId=<%# Eval("ProductId")%>">回复</a>
        </div>
    </div>
</div>
