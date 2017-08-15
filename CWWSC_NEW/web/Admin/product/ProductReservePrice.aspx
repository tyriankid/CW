<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ProductReservePrice.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ProductReservePrice" %>
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
            <h1>商品预约价格</h1>
            <span>对商品维护提醒进行编辑、展示、修改和删除。</span>
        </div>
        <div class="datalist">
            <!--搜索-->
            
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>商品名称：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                     <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" OnClick="btnSearch_Click" /></li>
                     <li><input  onclick="add()" type="button" class="searchbutton" value="新增"/></li>  
                </ul>
            </div>
           
            </div>
            <!--结束-->
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdReservePrice" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ReserveId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField  HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <Hi:ListImage ID="ListImage1" runat="server" DataField="ImageUrl1"  Width="30px"  Height="30px"/>
                               <%#Eval("ProductName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="商品规格" >
                        <ItemTemplate>
                             <%#Eval("SKU")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="预约生效时间" >
                        <ItemTemplate>
                             <%#Eval("StartDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="执行状态" >
                        <ItemTemplate>
                             <%#Eval("State").ToString()=="0"?"<span style='color:red'>未执行</span>":"<span style='color:green'>已执行</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="预约成本价" >
                        <ItemTemplate>
                             <%#Eval("CostPrice","{0:F2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="预约一口价" >
                        <ItemTemplate>
                             <%#Eval("SalePrice","{0:F2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField  HeaderText="预约内购价" >
                        <ItemTemplate>
                             <%#Eval("NeigouPrice","{0:F2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="95" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <a href="addProductReservePrice.aspx?ReserveId=<%#Eval("ReserveId")%>">编辑</a>
                            <Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete"  CommandArgument='<%# Eval("ReserveId")%>'/>
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
        function add() {
            location.href = "addProductReservePrice.aspx";
        }
    </script>
</asp:Content>
