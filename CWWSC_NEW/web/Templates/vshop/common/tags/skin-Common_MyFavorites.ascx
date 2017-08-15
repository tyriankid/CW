﻿<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <a href="/Vshop/ProductDetails.aspx?ProductId=<%# Eval("ProductId") %><%=Globals.GetFavorites() %>" target="_blank">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl180" />
        <div class="info">
            <div class="name font-l bcolor">
                <%# Eval("ProductName")%></div>
            <%--<div class="intro font-m text-muted text-overflow">
                <%# Eval("ShortDescription")%></div>--%>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}")%>
            </div>
        </div>
 </a><a href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')" class="link-del"><span></span></a>
</div>