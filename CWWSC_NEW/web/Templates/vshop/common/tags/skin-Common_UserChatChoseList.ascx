<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <%--<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>" target="_blank">--%>
    <a role="chatUser" href="#" userid="<%# Eval("UserId") %>">
        <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="UserHead" />
        <div class="info">
            <div class="name font-l bcolor"><%# Eval("UserName")%></div>
            <p class="messageWords">申达股份岁的法国申达股份岁的法国申达股份岁的法国申达股份岁的法国申达股份岁的法国!</p>
            <div class="price text-danger">
                <%# Eval("CreateDate")%>
            </div>
            <div class="roleMark">
              <%# Eval("RoleInfo") %>
            </div>
        </div>
    </a>

</div>