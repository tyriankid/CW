<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="seeFilialeProductList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.seeFilialeProductList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Register TagPrefix="cc1" TagName="Order_ItemsList" Src="~/Admin/Ascx/Order_ItemsList.ascx" %>
<%@ Register TagPrefix="cc1" TagName="Order_ShippingAddress" Src="~/Admin/Ascx/Order_ShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img src="../images/05.gif" width="32" height="32" /></em>
            <h1 class="title_line">分公司线下商品上架列表</h1>
        </div>
        <div class="datalist">
            <!--搜索-->

            <div class="searcharea clearfix br_search search_titxt">
                <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                    <ul>
                        <li><span>分公司名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>

                        <li>
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                    </ul>
                </div>

            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdsFilialeProductList" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField HeaderText="分公司名称" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <%#Eval("fgsName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="46%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;">
                                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                            </div>
                            <div style="float: left;">
                                <span class="Name">
                                    <%# Eval("ProductName") %></span> <span class="colorC" style="display: block">商品内码：<%# Eval("ProductCode") %><span style="margin-left: 15px;">库存：<%# Eval("Stock") %></span><span style="margin-left: 15px;">成本价：<%# Eval("CostPrice", "{0:f2}")%></span></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="商品价格">
                        <ItemTemplate>
                            <span class="Name">一口价：<%# Eval("SalePrice", "{0:f2}")%></span><span style="margin-left: 15px;">市场价：<%#Eval("MarketPrice").ToString()==""?"-":Eval("MarketPrice","{0:f2}")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="商品状态" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <span>
                                <%#Eval("SaleStatus").ToString()=="3"?"仓库中":Eval("SaleStatus").ToString()=="1"?"出售中":"待审核"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品销售区域" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <%#Eval("RegionName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
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
    <script type="text/javascript">

    </script>
</asp:Content>
