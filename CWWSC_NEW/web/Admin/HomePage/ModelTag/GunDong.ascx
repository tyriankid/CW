﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.GunDong" %>
<div id="module<%=PageSN %>" i="<%=PageSN %>" name="GunDong" class="small gd">
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div id="gongao" style="position: relative;left:10%;width: 90%;overflow:visible;">
        <img class='GunDongPic' src='../../../Templates/vshop/common/images/timg.png' style='width: auto;height:100%;position: absolute;top:0px;left: -10%;' />
        <div id="scroll_div" class="scroll_div wb_txt"<%-- style="height: <%=height%>; line-height:<%=height%>;"--%>>
            <div id="scroll_begin">
                <a href='<%=cons[2] %>' class="<%=cons[3] %>" style="color: <%=cons[1] %>;"><%=cons[0] %></a></div>
            <div id="scroll_end">
                 <a href='<%=cons[2] %>' style="color: <%=cons[1] %>;"><%=cons[0] %></a></div>
        </div>
    </div>
</div>