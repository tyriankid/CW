<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetProductVirtualCodes.aspx.cs" Inherits="Admin_product_SetProductVirtualCodes"  MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>
                虚拟商品</h1>
            <span>您可以维护虚拟编码。</span>
        </div>
        <div class="datalist">
            <!--搜索-->
            <asp:HiddenField ID="hiproductid" runat="server" />
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>商品虚拟码：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <%--<li><span>店长姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" CssClass="forminput" /></span></li>
                    <li><span>金立账号：</span><span><asp:TextBox ID="txtAllHere"  runat="server" CssClass="forminput" /></span></li>--%>
                    <li><span>状态：</span><asp:DropDownList ID="states" runat="server">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">未使用</asp:ListItem>
                        <asp:ListItem Value="1">已使用</asp:ListItem>
                    </asp:DropDownList></li>
                     <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                    <li><a href="AddProductVirtualCode.aspx?productId=<%=productid %>" class="submit_jia">添加虚拟商品编码</a></li>
                </ul>
            </div>
           
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdProductVirtual" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="VirtualId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField  HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <%#Eval("ProductName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="规格" >
                        <ItemTemplate>
                             <%#Eval("AttributeName").ToString() + "："+Eval("ValueStr").ToString() %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="虚拟码编码" >
                        <ItemTemplate>
                             <%#Eval("VirtualCode")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="创建时间" >
                        <ItemTemplate>
                             <%#Eval("CreateDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="虚拟码状态" >
                        <ItemTemplate>
                             <%#Eval("VirtualState").ToString()=="0"? "未使用": "已使用"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="95" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <%--<span  class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:ShowEdit('<%# Eval("VirtualId")%>','<%=productid %>')">编辑</a></span>--%>
                            <span  class="submit_bianji"><a href="AddProductVirtualCode.aspx?virtualId=<%# Eval("VirtualId")%>&productId=<%=productid %>">编辑</a></span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript">
        //当行审核商品
        function SetdistributorClass(id, status) {
            
        }
    </script>
</asp:Content>