﻿<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %><li total="<%#Eval("Blance") %>" id="li<%#Eval("UserID") %>">
    <div class="top-num"></div>
    <div class="top-img"><img src="<%#Eval("Logo").ToString().Length>5?Eval("Logo").ToString():"/Utility/pics/headLogo.jpg" %>"></div>
    <div class="top-exp">
        <div class="exp-top">
            <span><%#Eval("StoreName").ToString().Length > 8 ?  Eval("StoreName").ToString().Substring(0,8)+ "..." : Eval("StoreName").ToString() %></span>
               <span><img src="<%#Eval("Ico") %>" class="imgStar"/></span>
            <%--<em><%#Eval("Blance","{0:F2}") %>元</em>--%>
            <em><%#Eval("OrdersTotal","{0:F2}") %>元</em>
        </div>
        <%--<div class="exp-bottom"><em title="<%#Eval("Blance") %>"></em></div>--%>
        <div class="exp-bottom"><em title="<%#Eval("OrdersTotal") %>"></em></div>
    </div>
</li>