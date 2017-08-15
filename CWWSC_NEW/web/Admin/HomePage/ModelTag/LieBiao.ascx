<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LieBiao.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.LieBiao" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='LieBiao' class='small splb'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
	<ul class='<%=StyleName %>' style='background:<%=colors[0]%>;'>
        <asp:Repeater ID="rptGoods" runat="server">
            <ItemTemplate>
                <li>
                    <a href="<%# (Eval("ProductSource").ToString()=="3"?"/Vshop/ProductServiceDetails.aspx?ProductId=":"/Vshop/ProductDetails.aspx?ProductId=") + Eval("ProductId") %>">
                        <div class="<%=IsDiv%>"><img src="<%#Eval("ImageUrl1") %>"></div>
                        <div class="<%=IsDiv2%>">
                            <p style="color:<%=colors[1]%>;" class="<%#Eval("ProductName").ToString().Length > 12 ? "twoLine" : "oneLine" %>"><%#Eval("ProductName") %></p>
                            <p id="productType" style="display:<%=IsShow%>">&nbsp;<%#Eval("typeName")%></p>
                            <p style="color:<%=colors[1]%>;"><span style="color:#D9690B;">￥<%# Eval("SalePrice", "{0:F2}") %></span>&nbsp;&nbsp;&nbsp;&nbsp;<del class="text-muted font-xs"><span style="color:gray;font-size:12px;">¥<%# Eval("MarketPrice", "{0:F2}") %></span></p>
                            <p id="productConment" style="font-size:12px;display:<%=IsShow%>"><span>&nbsp;<%#Eval("reviewsCount")%></span>条评论</p>
                        </div>
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
	</ul>				
</div>