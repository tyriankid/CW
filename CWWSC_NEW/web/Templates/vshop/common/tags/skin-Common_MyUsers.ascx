<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <%--<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_blank">--%>
    <a href="#">
        <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="UserHead" />
        <div class="info">
            <div class="name font-l bcolor"><%# Eval("UserName")%></div>
            <div class="price text-danger">
                创建时间：<%# Eval("CreateDate")%>
            </div>
        </div>
        <div class="shu">88.00</div>
    </a>
    <a href="javascript:void(0)" onclick="startChat(<%#Eval("UserId") %>)">在线聊天</a>
    <a href="memberTags.aspx?userId=<%#Eval("UserId") %>">会员标签</a>
    <%--<div class="ask-answer">
        <p class="ask"><%# Eval("ConsultationText")%></p>
        <div class="dateTime font-s text-muted" style="text-align:right">
            提问时间：<%# Eval("ConsultationDate") %>
        </div>
        <p class="answer"><%# string.IsNullOrEmpty(Eval("ReplyText").ToString())?"暂无回复":Eval("ReplyText")%></p>
        <div class="dateTime font-s text-muted" style="text-align:right">
            <%# string.IsNullOrEmpty(Eval("ReplyText").ToString())?"": "回复时间：" + Eval("ReplyDate") %>
        </div>
    </div>--%>
</div>